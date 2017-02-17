namespace GestaoEscolar.Configuracao.LoteFechamento
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using MSTech.Validation.Exceptions;

    public partial class Cadastro : MotherPageLogado
    {
        #region Enumerador

        /// <summary>
        /// Enumera os passos a serem realizados para a importação do arquivo de fechamento de bimestre.
        /// </summary>
        public enum ePasso
        {
            SelecaoArquivo
            ,

            AnaliseArquivo
            ,

            ImportacaoArquivo
        }

        #endregion Enumerador

        #region Constantes

        public const string ValidationGroup = "ImportacaoFechamento";

        private const int grvArquivoColunaErros = 6;

        #endregion Constantes

        #region Propriedades

        /// <summary>
        /// ViewState que armazena o valor do ID do calendário.
        /// </summary>
        private int VS_cal_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o valor do ID do período do calendário.
        /// </summary>
        private int VS_tpc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tpc_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o valor do ID da turma.
        /// </summary>
        private long VS_tur_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena lista de registros analisados do arquivo de fechamento.
        /// </summary>
        private List<LoteFechamento> VS_listaLoteFechamento
        {
            get
            {
                return (List<LoteFechamento>)(ViewState["VS_listaLoteFechamento"] ?? new List<LoteFechamento>());
            }

            set
            {
                ViewState["VS_listaLoteFechamento"] = value;
            }
        }

        /// <summary>
        /// Armazena em viewstate o arquivo selecionado.
        /// </summary>
        private SYS_Arquivo VS_arquivo
        {
            get
            {
                return (SYS_Arquivo)(ViewState["VS_arquivo"] ?? new SYS_Arquivo());
            }

            set
            {
                ViewState["VS_arquivo"] = value;
            }
        }

        /// <summary>
        /// Armazena o passo atual da operação.
        /// </summary>
        private ePasso VS_passoAtual
        {
            get
            {
                return (ePasso)(ViewState["VS_passoAtual"] ?? ePasso.SelecaoArquivo);
            }

            set
            {
                ViewState["VS_passoAtual"] = value;
            }
        }

        /// <summary>
        /// Nome padrão do fechamento de bimestre no sistema.
        /// </summary>
        private string nomeEfetivacao
        {
            get
            {
                return (string)GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO");
            }
        }

        /// <summary>
        /// Nome padrão do bimestre no sistema.
        /// </summary>
        private string nomeBimestre
        {
            get
            {
                return GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Ordena as escolas por código.
        /// </summary>
        private bool ordenaEscolaPorCodigo
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        private string path;

        /// <summary>
        /// Propriedade que armazena o caminho das imagens no sistema.
        /// </summary>
        private string CaminhoImagens
        {
            get
            {
                return path ?? (path = VirtualPathUtility.ToAbsolute("~/App_Themes/" + __SessionWEB.TemaPadrao + "/images/"));
            }
        }

        #endregion Propriedades

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar)
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                        RedirecionarPagina("~/Configuracao/LoteFechamento/Busca.aspx");
                    }

                    ConfigurarPasso(ePasso.SelecaoArquivo);

                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_cal_id = PreviousPage.Edit_cal_id;
                        VS_tpc_id = PreviousPage.Edit_tpc_id;
                        VS_tur_id = PreviousPage.Edit_tur_id;

                        CarregarDados();

                        if (VerificaPeriodoFechado())
                        {
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage(String.Format("A importação de arquivo só é permitida durante o período de {0} do {1}.", nomeEfetivacao.ToLower(), nomeBimestre.ToLower()), UtilBO.TipoMensagem.Alerta);
                            RedirecionarPagina("~/Configuracao/LoteFechamento/Busca.aspx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }

            UCComboQtdePaginacao.IndexChanged += UCComboQtdePaginacao_IndexChanged;

            string script = String.Format("SetConfirmDialogLoader('{0}','{1}');", String.Concat("#", btnImportar.ClientID), "Confirma a importação dos registros processados com sucesso?");
            Page.ClientScript.RegisterStartupScript(GetType(), btnImportar.ClientID, script, true);
        }

        #endregion Page Life Cycle

        #region Delegates

        private void UCComboQtdePaginacao_IndexChanged()
        {
            grvArquivo.PageSize = UCComboQtdePaginacao.Valor;
            grvArquivo.PageIndex = 0;
            grvArquivo.DataBind();
        }

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Carrega as informações de qual escola, qual turma e qual bimestre deve ser importado
        /// </summary>
        private void CarregarDados()
        {
            TUR_Turma tur = new TUR_Turma { tur_id = VS_tur_id };
            TUR_TurmaBO.GetEntity(tur);

            ACA_CalendarioPeriodo tpc = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario_TipoPeriodo(VS_cal_id, VS_tpc_id, ApplicationWEB.AppMinutosCacheLongo);

            ESC_Escola esc = new ESC_Escola
            {
                esc_id = tur.esc_id,
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
            };
            ESC_EscolaBO.GetEntity(esc);

            lblEscola.Text = "Escola: " + esc.esc_nome;
            lblTurma.Text = "Turma: " + tur.tur_codigo;
            lblBimestre.Text = (string)GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO") + ": " + tpc.cap_descricao;
        }

        /// <summary>
        /// Verifica se o período está fechado para o fechamento de bimestre.
        /// </summary>
        /// <returns></returns>
        private bool VerificaPeriodoFechado()
        {
            ACA_CalendarioPeriodo entityCap = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario_TipoPeriodo(VS_cal_id, VS_tpc_id, ApplicationWEB.AppMinutosCacheLongo);
            List<ACA_Evento> ltEventos = ACA_EventoBO.GetEntity_Efetivacao_List(VS_cal_id, VS_tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id);

            DateTime dataAtual = DateTime.Now;

            int tev_EfetivacaoNotas = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            return (dataAtual > entityCap.cap_dataFim && dataAtual < entityCap.cap_dataInicio) ||
                   (!ltEventos.Exists(p => p.tpc_id == VS_tpc_id && p.tev_id == tev_EfetivacaoNotas));
        }

        /// <summary>
        /// O método copia o stream do arquivo selecionado para uma MemoryStream, possibilitando sua leitura múltiplas vezes.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        private Stream CopiarArquivo(Stream inputStream)
        {
            long tamanho = inputStream.Length;
            byte[] buffer = new byte[tamanho];
            MemoryStream memoryStream = new MemoryStream();

            int cont = inputStream.Read(buffer, 0, (int)tamanho);

            while (cont > 0)
            {
                memoryStream.Write(buffer, 0, cont);
                cont = inputStream.Read(buffer, 0, (int)tamanho);
            }

            memoryStream.Position = 0;
            inputStream.Close();
            return memoryStream;
        }

        /// <summary>
        /// Monta uma entidade de arquivo de acordo com o documento passado.
        /// </summary>
        /// <returns>Entidade de arquivo.</returns>
        public SYS_Arquivo CriarAnexo(Stream arquivo, string nomeArquivo, int tamanho, string typeMime)
        {
            return !string.IsNullOrEmpty(nomeArquivo) ?
                   new SYS_Arquivo
                   {
                       arq_nome = Path.GetFileName(nomeArquivo)
                       ,
                       arq_tamanhoKB = tamanho
                       ,
                       arq_typeMime = typeMime
                       ,
                       arq_data = GetBytes(arquivo)
                       ,
                       arq_situacao = (byte)SYS_ArquivoSituacao.Ativo
                       ,
                       arq_dataCriacao = DateTime.Now
                       ,
                       arq_dataAlteracao = DateTime.Now
                   } : null;
        }

        /// <summary>
        /// Retorna o array de Bytes do arquivo
        /// </summary>
        /// <param name="arquivo">Stream do arquivo</param>
        /// <returns>Array de Bytes</returns>
        public byte[] GetBytes(Stream arquivo)
        {
            byte[] file = null;

            if (arquivo != null)
            {
                int tamanho = Convert.ToInt32(arquivo.Length);
                file = new byte[tamanho];

                if (arquivo.Length == 0)
                    throw new ValidationException("O arquivo tem 0 bytes, por isso ele não será anexado.");

                arquivo.Read(file, 0, tamanho);
            }

            return file;
        }

        /// <summary>
        /// O método configura a tela de acordo com o passo da importação..
        /// </summary>
        /// <param name="passo"></param>
        private void ConfigurarPasso(ePasso passo)
        {
            VS_passoAtual = passo;
            switch (passo)
            {
                case ePasso.SelecaoArquivo:
                    lblSelecaoArquivo.CssClass = "passo_atual";
                    lblAnaliseArquivo.CssClass = lblImportacaoArquivo.CssClass = "passo";

                    pnlAnaliseImportacaoArquivo.Visible = false;
                    pnlSelecaoArquivo.Visible = true;
                    break;

                case ePasso.AnaliseArquivo:
                    lblAnaliseArquivo.CssClass = "passo_atual";
                    lblSelecaoArquivo.CssClass = lblImportacaoArquivo.CssClass = "passo";

                    msgImportacao.Visible =
                    pnlAnaliseImportacaoArquivo.Visible = btnImportar.Visible = true;

                    pnlSelecaoArquivo.Visible = false;

                    pnlAnaliseImportacaoArquivo.GroupingText = "Análise dos registro dos arquivos";
                    lblTituloSucesso.Text = "Registros processados com sucesso:";
                    lblTituloErro.Text = "Registros com erro:";
                    lblTituloTotal.Text = "Total de registros processados:";

                    grvArquivo.Columns[grvArquivoColunaErros].Visible = true;

                    break;

                case ePasso.ImportacaoArquivo:
                    lblSelecaoArquivo.CssClass = lblAnaliseArquivo.CssClass = "passo";
                    lblImportacaoArquivo.CssClass = "passo_atual";

                    pnlSelecaoArquivo.Visible = msgImportacao.Visible =
                    btnImportar.Visible = pnlSelecaoArquivo.Visible = false;
                    pnlAnaliseImportacaoArquivo.Visible = true;

                    pnlAnaliseImportacaoArquivo.GroupingText = "Importação dos registro dos arquivos";
                    lblTituloSucesso.Text = "Registros importados com sucesso:";
                    lblTituloErro.Text = "Registros com erro não importados:";
                    lblTituloTotal.Text = "Total de registros:";

                    grvArquivo.Columns[grvArquivoColunaErros].Visible = false;

                    break;
            }
        }

        /// <summary>
        /// Retorna o nome a ser exibido na legenda da página.
        /// </summary>
        /// <returns></returns>
        public string RetornaLegendaPagina()
        {
            return String.Format("Importação do arquivo de {0} de {1}", nomeEfetivacao.ToLower(), nomeBimestre.ToLower());
        }

        /// <summary>
        /// O método retorna a url da imagem de status do processamento do registro.
        /// </summary>
        /// <param name="statusGrid"></param>
        /// <returns></returns>
        public string RetornaImagemStatus(string statusGrid)
        {
            LoteStatus status = (LoteStatus)Enum.Parse(typeof(LoteStatus), statusGrid);
            string path = VirtualPathUtility.ToAbsolute("~/App_Themes/" + __SessionWEB.TemaPadrao + "/images/");

            switch (status)
            {
                case LoteStatus.Sucess:
                    path += "success.png";
                    break;

                case LoteStatus.Error:
                    path += "error.png";
                    break;
            }

            return path;
        }

        /// <summary>
        /// O método valida o nome do arquivo selecionado.
        /// </summary>
        /// <returns></returns>
        private bool ValidarNomeArquivo()
        {
            Regex rgx = new Regex(@"^\d{6}[_]([A-Z,a-z,0-9,à-ú,À-Ú,-])+[_]\d{4}[-]\d[ºª][A-Z,a-z,-]+[_]\d+$", RegexOptions.IgnoreCase);
            string nomeArquivo = Path.GetFileNameWithoutExtension(fupArquivo.PostedFile.FileName);

            bool valido = rgx.IsMatch(nomeArquivo);

            if (!valido)
                throw new ValidationException("O nome do arquivo é inválido.");

            return GestaoEscolarUtilBO.VerificarArquivo(fupArquivo.PostedFile) && valido;
        }

        /// <summary>
        /// O método analisa os registros do arquivo de fechamento.
        /// </summary>
        private void AnalisarArquivo()
        {
            try
            {
                //if (ValidarNomeArquivo())
                if (GestaoEscolarUtilBO.VerificarArquivo(fupArquivo.PostedFile))
                {
                    int sucesso, erro;

                    string nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(fupArquivo.PostedFile.FileName);
                    string nomeArquivo = fupArquivo.PostedFile.FileName;
                    int tamanhoArquivo = fupArquivo.PostedFile.ContentLength;
                    string typeMime = fupArquivo.PostedFile.ContentType;

                    Stream arquivo = CopiarArquivo(fupArquivo.PostedFile.InputStream);

                    VS_arquivo = CriarAnexo(arquivo, nomeArquivo, tamanhoArquivo, typeMime);

                    arquivo.Position = 0;

                    VS_listaLoteFechamento = CLS_ArquivoEfetivacaoBO.AnalisarRegistrosLote
                                                                    (
                                                                        (string)GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA"),
                                                                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                                                        VS_cal_id, VS_tpc_id, VS_tur_id,
                                                                        arquivo, nomeArquivoSemExtensao,
                                                                        out sucesso, out erro
                                                                    );

                    if (VS_listaLoteFechamento.Any())
                    {
                        UCComboQtdePaginacao.Valor = 10;
                        grvArquivo.PageIndex = 0;
                        grvArquivo.PageSize = UCComboQtdePaginacao.Valor;
                        grvArquivo.DataBind();
                        updArquivo.Update();

                        // Configura análise dos registro do arquivo
                        lblSucesso.Text = sucesso.ToString();
                        lblErro.Text = erro.ToString();
                        lblTotal.Text = (sucesso + erro).ToString();

                        ConfigurarPasso(ePasso.AnaliseArquivo);
                    }
                    else
                    {
                        lblMessage.Text = UtilBO.GetErroMessage("Não existem registros para analisar no arquivo enviado.", UtilBO.TipoMensagem.Alerta);
                        updMessage.Update();
                    }
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao realizar a análise dos registros do arquivo.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
            finally
            {
                updMessage.Update();
            }
        }

        /// <summary>
        /// O método realiza a importação dos registros que foram analisados com sucesso.
        /// </summary>
        private void ImportarArquivo()
        {
            try
            {
                List<LoteFechamento> listaSucesso = VS_listaLoteFechamento.Where(p => p.status == LoteStatus.Sucess).ToList();

                if (listaSucesso.Any())
                {
                    int sucesso, erro;

                    VS_listaLoteFechamento = CLS_ArquivoEfetivacaoBO.SalvarRegistrosLote(__SessionWEB.__UsuarioWEB.Usuario.ent_id, listaSucesso, VS_arquivo, out sucesso, out erro);

                    // Configura e atualiza GridView
                    UCComboQtdePaginacao.Valor = 10;
                    grvArquivo.PageIndex = 0;
                    grvArquivo.PageSize = Convert.ToInt32(UCComboQtdePaginacao.Valor);
                    grvArquivo.DataBind();

                    // Configura importação dos registro do arquivo
                    lblSucesso.Text = sucesso.ToString();
                    lblErro.Text = erro.ToString();
                    lblTotal.Text = (sucesso + erro).ToString();

                    ConfigurarPasso(ePasso.ImportacaoArquivo);

                    // Configura mensagem da importação dos registro
                    lblMessage.Text = sucesso > 0 ?
                                      (
                                          erro > 0 ?
                                          UtilBO.GetMessage(String.Format("Importação de dados de {0} de {1} realizada com sucesso, porém alguns registros não foram importados devido a erros encontrados.", nomeEfetivacao.ToLower(), nomeBimestre.ToLower()), UtilBO.TipoMensagem.Sucesso) :
                                          UtilBO.GetMessage(String.Format("Importação de dados de {0} de {1} realizada com sucesso.", nomeEfetivacao.ToLower(), nomeBimestre.ToLower()), UtilBO.TipoMensagem.Sucesso)
                                      ) :
                                      UtilBO.GetMessage(String.Format("Não foi possível realizar importação de dados de {0} de {1} devido a erros encontrados.", nomeEfetivacao.ToLower(), nomeBimestre.ToLower()), UtilBO.TipoMensagem.Erro);

                    if (sucesso > 0)
                    {
                        ApplicationWEB._GravaLogSistema
                            (
                                LOG_SistemaTipo.Insert,
                                String.Format("Importação de dados de {0} de {1} - sucesso: {2} / erro: {3} / tur_id: {4} / tpc_id: {5}",
                                                nomeEfetivacao.ToLower(), nomeBimestre.ToLower(), sucesso.ToString(), erro.ToString(), VS_tur_id.ToString(), VS_tpc_id.ToString())
                            );
                    }
                }
                else
                {
                    lblMessage.Text = UtilBO.GetMessage("Não existem registros processados com sucesso para realizar a importação.", UtilBO.TipoMensagem.Alerta);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar importar dados do arquivo.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                updMessage.Update();
            }
        }

        #endregion Métodos

        #region Eventos

        #region Selecionar Arquivo

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("~/Configuracao/LoteFechamento/Busca.aspx");
        }

        protected void btnAnalisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                AnalisarArquivo();
            }
        }

        #endregion Selecionar Arquivo

        #region Analisar arquivo

        protected void grvArquivo_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = VS_listaLoteFechamento.Count;
            ConfiguraColunasOrdenacao(grvArquivo);
        }

        protected void grvArquivo_DataBinding(object sender, EventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (grv.DataSource == null)
                    grv.DataSource = (from LoteFechamento lote in VS_listaLoteFechamento
                                      let nomeEscola = ordenaEscolaPorCodigo && !string.IsNullOrEmpty(lote.codigoEscola) ?
                                                       String.Format("{0} - {1}", lote.codigoEscola, lote.nomeEscola) :
                                                       lote.nomeEscola
                                      select new
                                      {
                                          nomeEscola
                                          ,
                                          lote.codigoTurma
                                          ,
                                          lote.nomeAluno
                                          ,
                                          lote.disciplina
                                          ,
                                          docente = string.IsNullOrEmpty(lote.docente) ? "-" : lote.docente
                                          ,
                                          lote.mensagem
                                          ,
                                          lote.status
                                      }).ToList();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                updMessage.Update();
            }
        }

        protected void grvArquivo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView grv = (GridView)sender;
            grv.PageSize = UCComboQtdePaginacao.Valor;
            grv.PageIndex = e.NewPageIndex;
            grv.DataBind();
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (VS_passoAtual == ePasso.AnaliseArquivo)
                ConfigurarPasso(ePasso.SelecaoArquivo);
            else
                RedirecionarPagina("~/Configuracao/LoteFechamento/Busca.aspx");
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            ImportarArquivo();
        }

        #endregion Analisar arquivo

        #endregion Eventos
    }
}