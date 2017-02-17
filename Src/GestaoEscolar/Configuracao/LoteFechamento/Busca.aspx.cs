using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.LoteFechamento
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LoteFechamento)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_Ordenacao", out valor))
                    {
                        return valor;
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LoteFechamento)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_SortDirection", out valor))
                    {
                        return (SortDirection)Enum.Parse(typeof(SortDirection), valor);
                    }
                }

                return SortDirection.Ascending;
            }
        }

        /// <summary>
        /// Guarda o nome do arquivo csv para download
        /// </summary>
        private string VS_NomeArquivoCSV
        {
            get
            {
                return (ViewState["VS_NomeArquivoCSV"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_NomeArquivoCSV"] = value;
            }
        }


        /// <summary>
        /// Guarda os dados do arquivo csv para download
        /// </summary>
        private string VS_ConteudoArquivoCSV
        {
            get
            {
                return (ViewState["VS_ConteudoArquivoCSV"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_ConteudoArquivoCSV"] = value;
            }
        }

        /// <summary>
        /// Guarda o dado do datakey do grid de fechamento
        /// </summary>
        private DataKey gvFechamentoDataKeysEdit
        {
            get
            {
                return gvFechamento.DataKeys[gvFechamento.EditIndex];
            }
        }

        /// <summary>
        /// Recebe o Id para enviar os dados para edição.
        /// </summary>
        public int Edit_cal_id
        {
            get
            {
                return Convert.ToInt32(gvFechamentoDataKeysEdit.Values["cal_id"]);
            }
        }

        /// <summary>
        /// Recebe o Id para enviar os dados para edição.
        /// </summary>
        public int Edit_tpc_id
        {
            get
            {
                return Convert.ToInt32(gvFechamentoDataKeysEdit.Values["tpc_id"]);
            }
        }

        /// <summary>
        /// Recebe o Id para enviar os dados para edição.
        /// </summary>
        public long Edit_tur_id
        {
            get
            {
                return Convert.ToInt64(gvFechamentoDataKeysEdit.Values["tur_id"]);
            }
        }

        #endregion Propriedades

        #region Constantes

        public const string validationGroup = "vgFechamento";

        public const int gvFechamentoColunaImportar = 4;

        #endregion Constantes

        #region Delegates

        public void UCCUA_IndexChanged()
        {
            try
            {
                //UCCDocente.Valor = new long[] { -1, -1, -1, -1 };
                //UCCDocente.PermiteEditar = false;

                if (UCCUAEscola.Uad_ID != Guid.Empty)
                {
                    UCCUAEscola.CarregaUnidadesEscolaresSemAcesso(UCCUAEscola.Uad_ID, UCCCalendario.Valor, UCCPeriodoCalendario.Valor[0]);
                    UCCUAEscola.EnableEscolas = true;
                    UCCUAEscola.FocoEscolas = true;
                }
                else
                {
                    UCCUAEscola.EnableEscolas = false;
                    UCCUAEscola.SelectedValueEscolas = new[] { -1, -1 };//Nescessario quando existe somente um registro de escola.
                }

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        //public void UCCUAEscola_IndexChanged()
        //{
        //    try
        //    {
        //        UCCDocente.Valor = new long[] { -1, -1, -1, -1 };
        //        UCCDocente.PermiteEditar = false;

        //        if (UCCUAEscola.Esc_ID != -1)
        //        {
        //            UCCDocente._Load_By_esc_uni_id(UCCUAEscola.Esc_ID + ";" + UCCUAEscola.Uni_ID, 1);
        //            UCCDocente.PermiteEditar = true;
        //        }
        //        else
        //        {
        //            UCCCalendario.Valor = -1;
        //            UCCCalendario.PermiteEditar = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationWEB._GravaErro(ex);
        //        _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
        //    }
        //}

        public void UCCCalendario_IndexChanged()
        {
            try
            {
                UCCUAEscola.SelectedIndexEscolas = -1;
                UCCUAEscola.SelectedIndexUa = -1;
                UCCUAEscola.PermiteAlterarCombos = false;
                //UCCDocente.Valor = new long[] { -1, -1, -1, -1 };
                //UCCDocente.PermiteEditar = false;

                if (UCCCalendario.Valor != -1)
                {
                    UCCPeriodoCalendario.CarregarPeriodosAtualFechamentoPorCalendario(UCCCalendario.Valor);
                    UCCPeriodoCalendario.PermiteEditar = true;
                    if (UCCPeriodoCalendario.Valor[0] > 0 && UCCPeriodoCalendario.Valor[1] > 0)
                        UCCPeriodoCalendario_IndexChanged();

                }
                else
                {
                    UCCPeriodoCalendario.Valor = new int[] { -1, -1 };
                    UCCPeriodoCalendario.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        public void UCCPeriodoCalendario_IndexChanged()
        {
            UCCUAEscola.SelectedIndexEscolas = -1;
            UCCUAEscola.SelectedIndexUa = -1;
            UCCUAEscola.PermiteAlterarCombos = false;
            //UCCDocente.Valor = new long[] { -1, -1, -1, -1 };
            //UCCDocente.PermiteEditar = false;

            if (UCCPeriodoCalendario.Valor[0] > 0 && UCCPeriodoCalendario.Valor[1] > 0)
            {
                UCCUAEscola.PermiteAlterarCombos = true;
                UCCUAEscola.CarregarEscolaAutomatico = false;
                UCCUAEscola.Inicializar();
            }
        }

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Seta os delegates
        /// </summary>
        public void Delegates()
        {
            //UCCUAEscola.IndexChangedUnidadeEscola += UCCUAEscola_IndexChanged;
            UCCUAEscola.IndexChangedUA += UCCUA_IndexChanged;
            UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;
            UCCPeriodoCalendario.IndexChanged += UCCPeriodoCalendario_IndexChanged;
        }

        /// <summary>
        /// Atualiza o grid
        /// </summary>
        private void AtualizarGrid()
        {
            // atribui nova quantidade itens por página para o grid
            gvFechamento.PageSize = UCCQtdePaginacao.Valor;
            gvFechamento.PageIndex = 0;
            // atualiza o grid
            gvFechamento.DataBind();
        }

        /// <summary>
        /// Realiza a pesquisa no banco e preenche o grid
        /// </summary>
        private void Pesquisar()
        {
            fdsResultado.Visible = true;

            Dictionary<string, string> filtros = new Dictionary<string, string>();
            gvFechamento.PageIndex = 0;
            odsFechamento.SelectParameters.Clear();
            odsFechamento.SelectParameters.Add("esc_id", DbType.Int32, UCCUAEscola.Esc_ID.ToString());
            odsFechamento.SelectParameters.Add("uni_id", DbType.Int32, UCCUAEscola.Uni_ID.ToString());
            odsFechamento.SelectParameters.Add("cal_id", DbType.Int32, UCCCalendario.Valor.ToString());
            odsFechamento.SelectParameters.Add("tur_codigo", DbType.AnsiString, txtCodigoTurma.Text.Trim());
            odsFechamento.SelectParameters.Add("tpc_id", DbType.Int32, UCCPeriodoCalendario.Valor[0].ToString());
            odsFechamento.SelectParameters.Add("paginado", DbType.Boolean, false.ToString());
            odsFechamento.SelectParameters.Add("currentPage", DbType.Int32, "0");
            odsFechamento.SelectParameters.Add("pageSize", DbType.Int32, UCCQtdePaginacao.Valor.ToString());

            odsFechamento.DataBind();

            gvFechamento.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in odsFechamento.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            //Filtros que serão utilizados para setar o selected value do combo de UA e de periodo
            filtros.Add("uad_id", UCCUAEscola.Uad_ID.ToString());
            filtros.Add("cap_id", UCCPeriodoCalendario.Valor[1].ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.LoteFechamento
                ,
                Filtros = filtros
            };

            #endregion

            // atribui essa quantidade para o grid
            gvFechamento.PageSize = UCCQtdePaginacao.Valor;

            // atualiza o grid
            gvFechamento.DataBind();

            bool periodoFechado = VerificaPeriodoFechado
                                  (
                                      UCCCalendario.Valor, 
                                      UCCPeriodoCalendario.Valor[0], 
                                      UCCUAEscola.Esc_ID, 
                                      UCCUAEscola.Uni_ID
                                  );

            gvFechamento.Columns[gvFechamentoColunaImportar].Visible = !periodoFechado;
            lblInformacao.Visible = periodoFechado;

            if (periodoFechado)
            {
                string nomeEfetivacao = (string)GetGlobalResourceObject("Mensagens", "MSG_EFETIVACAO");
                string nomeBimestre = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                lblInformacao.Text = UtilBO.GetErroMessage(String.Format("A importação de arquivo só será permitida durante o período de {0} do {1}.", nomeEfetivacao.ToLower(), nomeBimestre.ToLower()), UtilBO.TipoMensagem.Informacao);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta,
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificaBusca()
        {
            UCCCalendario.Carregar();

            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LoteFechamento)
            {
                string valor;
                string valor1;

                odsFechamento.SelectParameters.Add("esc_id", DbType.Int32, UCCUAEscola.Esc_ID.ToString());
                odsFechamento.SelectParameters.Add("uni_id", DbType.Int32, UCCUAEscola.Uni_ID.ToString());

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCCCalendario.Valor = Convert.ToInt32(valor);
                if (UCCCalendario.Valor != -1)
                {
                    UCCPeriodoCalendario.CarregarPeriodosAtualFechamentoPorCalendario(UCCCalendario.Valor);
                    UCCPeriodoCalendario.PermiteEditar = true;
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tpc_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cap_id", out valor1);
                UCCPeriodoCalendario.Valor = new int[] { Convert.ToInt32(valor), Convert.ToInt32(valor1) };
                if (UCCPeriodoCalendario.Valor[0] > 0 && UCCPeriodoCalendario.Valor[1] > 0)
                {
                    UCCUAEscola.PermiteAlterarCombos = true;
                    UCCUAEscola.CarregarEscolaAutomatico = false;
                    UCCUAEscola.Inicializar();
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_id", out valor);
                UCCUAEscola.Uad_ID = new Guid(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out valor1);

                UCCUAEscola.CarregaUnidadesEscolaresSemAcesso(UCCUAEscola.Uad_ID, UCCCalendario.Valor, UCCPeriodoCalendario.Valor[0]);

                UCCUAEscola.SelectedValueEscolas = new int[] { Convert.ToInt32(valor), Convert.ToInt32(valor1) };
                UCCUAEscola.EnableEscolas = true;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_codigo", out valor);
                txtCodigoTurma.Text = valor;
                txtCodigoTurma.Focus();

                Pesquisar();
            }
            else
            {
                if (UCCCalendario.Valor != -1)
                {
                    UCCCalendario_IndexChanged();
                }
            }
        }

        /// <summary>
        /// Verifica se o período está fechado para o fechamento de bimestre.
        /// </summary>
        /// <returns></returns>
        private bool VerificaPeriodoFechado(int cal_id, int tpc_id, int esc_id, int uni_id)
        {
            ACA_CalendarioPeriodo entityCap = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario_TipoPeriodo(cal_id, tpc_id, ApplicationWEB.AppMinutosCacheLongo);
            List<ACA_Evento> ltEventos = ACA_EventoBO.SelecionaEventosEfetivacaoPeriodoCalendario(cal_id, entityCap.cap_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, esc_id, uni_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            DateTime dataAtual = DateTime.Now;

            int tev_EfetivacaoNotas = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            return (dataAtual > entityCap.cap_dataFim && dataAtual < entityCap.cap_dataInicio) ||
                   (!ltEventos.Exists(p => p.tpc_id == tpc_id && p.tev_id == tev_EfetivacaoNotas));
        }

        #endregion Métodos

        #region Eventos

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    fdsPesquisa.Visible = false;
                    _lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                    return;
                }
                if (!IsPostBack)
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                        _lblMessage.Text = message;

                    VerificaBusca();
                }

                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsDownloadLoteFechamento.js"));
                }

                Delegates();

                Page.Form.DefaultButton = btnPesquisar.UniqueID;

                gvFechamento.Columns[3].Visible =
                gvFechamento.Columns[2].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Page Life Cycle

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                Pesquisar();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Inicializa variável de sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            AtualizarGrid();
        }

        protected void gvFechamento_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = CLS_ArquivoEfetivacaoBO.GetTotalRecords();

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(gvFechamento);

            if ((!string.IsNullOrEmpty(gvFechamento.SortExpression)) && (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LoteFechamento))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = gvFechamento.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", gvFechamento.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = gvFechamento.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", gvFechamento.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.LoteFechamento
                    ,
                    Filtros = filtros
                };
            }
        }

        protected void gvFechamento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Exportar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int esc_id = Convert.ToInt32(gvFechamento.DataKeys[index]["esc_id"]);
                    int uni_id = Convert.ToInt32(gvFechamento.DataKeys[index]["uni_id"]);
                    int cal_id = Convert.ToInt32(gvFechamento.DataKeys[index]["cal_id"]);
                    int tpc_id = Convert.ToInt32(gvFechamento.DataKeys[index]["tpc_id"]);
                    Int64 tur_id = Convert.ToInt64(gvFechamento.DataKeys[index]["tur_id"]);

                    string ordemAluno = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.ORDENACAO_COMBO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    ordemAluno = string.IsNullOrEmpty(ordemAluno) ? "1" : ordemAluno;
                    DataTable dt = CLS_ArquivoEfetivacaoBO.ExportacaoAlunosFechamento(esc_id, uni_id, cal_id, tur_id, tpc_id,
                                                                            GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id),
                                                                            GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL").ToString(),
                                                                            Convert.ToInt16(ordemAluno));
                    if (dt.Rows.Count == 0)
                        throw new ValidationException("Não existem itens para essa turma");

                    CLS_ArquivoEfetivacao entity = new CLS_ArquivoEfetivacao
                    {
                        aef_id = -1,
                        esc_id = esc_id,
                        uni_id = uni_id,
                        cal_id = cal_id,
                        tpc_id = tpc_id,
                        tur_id = tur_id,
                        aef_dataCriacao = DateTime.Now,
                        aef_dataAlteracao = DateTime.Now,
                        aef_tipo = (short)CLS_ArquivoEfetivacaoBO.eTipoArquivoEfetivacao.Exportacao,
                        aef_situacao = (short)CLS_ArquivoEfetivacaoBO.eSituacao.Ativo,
                        IsNew = true
                    };

                    if (CLS_ArquivoEfetivacaoBO.Save(entity))
                    {
                        VS_NomeArquivoCSV = dt.Rows[0][0] + "_" + dt.Rows[0][1] + "_" + dt.Rows[0][2] + "-" + dt.Rows[0][3] + "_" + entity.aef_id + ".csv";
                        VS_ConteudoArquivoCSV = GestaoEscolarUtilBO.ConverterTabelaParaArquivoCSV(dt).ToString();

                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "aef_id: " + entity.aef_id);
                        lblMsgDownload.Text = UtilBO.GetErroMessage("Arquivo gerado com sucesso.", UtilBO.TipoMensagem.Sucesso);// +
                            //UtilBO.GetErroMessage("O nome do arquivo será utilizado para importação, portanto não deve ser alterado.", UtilBO.TipoMensagem.Alerta);
                        Pesquisar();
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "Donwload", "$(document).ready(function() { $('#divDownload').dialog({title: 'Download do arquivo do fechamento do bimestre'}).dialog('open'); });", true);
                    }
                }
                catch (ValidationException ex)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar o arquivo.", UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Importar")
            {
                try
                {
                    gvFechamento.EditIndex = int.Parse(e.CommandArgument.ToString());
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                HttpResponse response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.AddHeader("Content-Disposition", "attachment; filename=" + VS_NomeArquivoCSV.Replace(" ", "-"));
                response.ContentEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");
                response.ContentType = "text/csv";
                response.Write(VS_ConteudoArquivoCSV);
                response.Flush();
                response.End();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar exportar o arquivo.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos

    }
}