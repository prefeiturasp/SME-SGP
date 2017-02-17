namespace GestaoEscolar.Classe.Efetivacao
{
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.CustomResourceProviders;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public partial class CadastroGestor : MotherPageLogadoCompressedViewState
    {
        #region Constantes

        /// <summary>
        /// Nome do resource
        /// </summary>
        protected const string RESOURCE_NAME = "Fechamento";

        /// <summary>
        /// Prefixo da chave do resource
        /// </summary>
        protected const string RESOURCE_KEY = "FechamentoGestor.Cadastro.{0}";

        #endregion

        #region Propriedades

        /// <summary>
        /// Ordenação dos dados.
        /// </summary>
        private byte OrdenacaoDados
        {
            get
            {
                string ordenacao = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.ORDENACAO_COMBO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (string.IsNullOrEmpty(ordenacao))
                {
                    ordenacao = "0";
                }

                return Convert.ToByte(ordenacao);
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena o id da turma.
        /// </summary>
        private long VS_tur_id
        {
            get
            {
                if (ViewState["VS_tur_id"] != null)
                {
                    return Convert.ToInt64(ViewState["VS_tur_id"]);
                }

                return -1;
            }
            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena o id da escola.
        /// </summary>
        private int VS_esc_id
        {
            get
            {
                if (ViewState["VS_esc_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_esc_id"]);
                }

                return -1;
            }
            set
            {
                ViewState["VS_esc_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena o id do aluno selecionado.
        /// </summary>
        private long VS_alu_idSelecionado
        {
            get
            {
                if (ViewState["VS_alu_idSelecionado"] != null)
                {
                    return Convert.ToInt64(ViewState["VS_alu_idSelecionado"]);
                }

                return -1;
            }
            set
            {
                ViewState["VS_alu_idSelecionado"] = value;
            }
        }

        /// <summary>
        /// ViewState com a lista de alunos.
        /// </summary>
        private List<CLS_AlunoAvaliacaoTurDis_DadosAlunos> VS_ListaAlunos
        {
            get
            {
                if (ViewState["VS_ListaAlunos"] != null)
                {
                    return (List<CLS_AlunoAvaliacaoTurDis_DadosAlunos>)ViewState["VS_ListaAlunos"];
                }

                return new List<CLS_AlunoAvaliacaoTurDis_DadosAlunos>();
            }
            set
            {
                ViewState["VS_ListaAlunos"] = value;
            }
        }

        /// <summary>
        /// Session - página de busca
        /// </summary>
        private PaginaGestao _paginaBusca;
        public PaginaGestao PaginaBusca
        {
            get
            {
                return _paginaBusca;
            }
            set
            {
                _paginaBusca = value;
            }
        }

        /// <summary>
        /// Lista com o quantitativo das disciplinas da turma.
        /// </summary>
        private List<TUR_TurmaDisciplinaAulaPrevistaBO.QuantitativoTurmaDisciplina> _listaQuantitativoTurmaDisciplina;
        private List<TUR_TurmaDisciplinaAulaPrevistaBO.QuantitativoTurmaDisciplina> ListaQuantitativoTurmaDisciplina
        {
            get
            {
                return _listaQuantitativoTurmaDisciplina;
            }
            set
            {
                _listaQuantitativoTurmaDisciplina = value;
            }
        }

        #endregion

        #region Eventos

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                // Adiciona JavaScripts da tela.
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsFechamentoGestor.js"));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    InicializaCombos();
                    CarregaBusca();
                    UCAlunoEfetivacaoObservacaoGeral1.VS_MensagemLogEfetivacaoObservacao = "Fechamento do gestor";
                    UCAlunoEfetivacaoObservacaoGeral1.AnotacoesAlunoVisible = true;
                }

                UCComboUAEscola1.IndexChangedUA += UCFiltroEscolas1__Selecionar;
                UCComboUAEscola1.IndexChangedUnidadeEscola += UCFiltroEscolas1__SelecionarEscola;
                UCComboCalendario1.IndexChanged += UCComboCalendario1_IndexChanged;
                UCAlunoEfetivacaoObservacaoGeral1.ReturnValues += UCAlunoEfetivacaoObservacaoGeral_ReturnValues;

                if (!Convert.ToString(btnExibir.CssClass).Contains("btnMensagemUnload"))
                {
                    btnExibir.CssClass += " btnMensagemUnload";
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnExibir_Click(object sender, EventArgs e)
        {
            try
            {
                Pesquisar();
            }
            catch (Exception ex)
            {
                divResultados.Visible = false;
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void chkTurmaExtinta_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarComboTurma();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptDadosTurma_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptPeriodoCalendario = (Repeater)e.Item.FindControl("rptPeriodoCalendario");
                if (rptPeriodoCalendario != null)
                {
                    var periodos = ListaQuantitativoTurmaDisciplina.Select(row => new { tpc_id = row.tpc_id, cap_descricao = row.cap_descricao }).Distinct();
                    rptPeriodoCalendario.DataSource = periodos.ToList();
                    rptPeriodoCalendario.DataBind();
                }

                Repeater rptPeriodoCalendarioColunasFixas = (Repeater)e.Item.FindControl("rptPeriodoCalendarioColunasFixas");
                if (rptPeriodoCalendarioColunasFixas != null)
                {
                    var periodos = ListaQuantitativoTurmaDisciplina.Select(row => new { tpc_id = row.tpc_id, cap_descricao = row.cap_descricao }).Distinct();
                    rptPeriodoCalendarioColunasFixas.DataSource = periodos.ToList();
                    rptPeriodoCalendarioColunasFixas.DataBind();
                }
            }

            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptAulas = (Repeater)e.Item.FindControl("rptAulas");
                if (rptAulas != null)
                {
                    var aulas = ListaQuantitativoTurmaDisciplina.Select(row => new { 
                                                                                    tud_id = row.tud_id, 
                                                                                    tpc_id = row.tpc_id, 
                                                                                    aulasDadas = (row.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia &&
                                                                                                  !row.experienciaVigente ? "-" : row.aulasDadas.ToString()),
                                                                                    aulasPrevistas = (row.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia &&
                                                                                                      !row.experienciaVigente ? "-" : row.aulasPrevistas.ToString()) 
                                                                                   }).Where(row => row.tud_id == Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id"))).Distinct();

                    rptAulas.DataSource = aulas;
                    rptAulas.DataBind();
                }

            }
        }

        protected void lkbDadosTurma_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarDadosTurma(VS_tur_id);
                updDadosTurma.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "AbrirDadosTurma", "$('#divDadosTurma').dialog('open');", true);
                
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptAlunos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlTableRow trNomeAluno = (HtmlTableRow)e.Item.FindControl("trNomeAluno");
                if (trNomeAluno != null)
                {
                    bool inativo = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "inativo"));
                    bool baixaFrequencia = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "baixaFrequencia"));
                    bool pendencia = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "pendencia"));

                    Label lblLegendaAluno = (Label)e.Item.FindControl("lblLegendaAluno");

                    if (inativo)
                    {
                        lblLegendaAluno.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }
                    else if (baixaFrequencia)
                    {
                        lblLegendaAluno.Style["background-color"] = ApplicationWEB.AlunoFrequenciaLimite;
                    }

                    ImageButton imgStatusFechamento = (ImageButton)e.Item.FindControl("imgStatusFechamento");
                    if (imgStatusFechamento != null)
                    {
                        imgStatusFechamento.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "statusAlertaPendencia.png";
                    }

                    imgStatusFechamento.Visible = pendencia;

                    LinkButton lblNomeAluno = (LinkButton)e.Item.FindControl("lblNomeAluno");
                    if (lblNomeAluno != null && !Convert.ToString(lblNomeAluno.CssClass).Contains("btnMensagemUnload"))
                    {
                        lblNomeAluno.CssClass += " btnMensagemUnload";
                    }
                }
            }
        }

        protected void lblNomeAluno_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem item in rptAlunos.Items)
                {
                    // Remove o estilo das outras linhas.
                    HtmlTableRow trNomeAluno = (HtmlTableRow)item.FindControl("trNomeAluno");
                    trNomeAluno.Attributes.Remove("class");
                }

                LinkButton lkbNomeAluno = (LinkButton)sender;
                RepeaterItem rpiAluno = (RepeaterItem)((lkbNomeAluno).NamingContainer);

                // Adiciona o estilo ao item selecionado.
                HtmlTableRow trNomeAlunoAtivo = (HtmlTableRow)rpiAluno.FindControl("trNomeAluno");
                trNomeAlunoAtivo.Attributes.Add("class", "aluno-selecionado");

                HiddenField hdnAluId = (HiddenField)rpiAluno.FindControl("hdnAluId");
                HiddenField hdnMtuId = (HiddenField)rpiAluno.FindControl("hdnMtuId");

                VS_alu_idSelecionado = Convert.ToInt64(hdnAluId.Value);

                UCAlunoEfetivacaoObservacaoGeral1.SelectedTab = 0;
                spanMensagemSelecionarAluno.Visible = false;
                UCAlunoEfetivacaoObservacaoGeral1.CarregarDadosAluno(Convert.ToInt64(hdnAluId.Value), Convert.ToInt32(hdnMtuId.Value), VS_tur_id, VS_esc_id, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id), true);
                UCAlunoEfetivacaoObservacaoGeral1.AnotacoesAlunoVisible = true;
                UCAlunoEfetivacaoObservacaoGeral1.ObservacaoVisible = true;

                updDadosAluno.Update();
            }
            catch (Exception ex)
            {
                UCAlunoEfetivacaoObservacaoGeral1.ObservacaoVisible = false;
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Retorna valor de um resource.
        /// </summary>
        /// <param name="chave">Chave do resource.</param>
        /// <returns></returns>
        private string RetornaValorResource(string chave)
        {
            return CustomResource.GetGlobalResourceObject(RESOURCE_NAME, String.Format(RESOURCE_KEY, chave)).ToString();
        }

        /// <summary>
        /// Verifica se há busca salva e carrega os combos da tela.
        /// </summary>
        protected void CarregaBusca()
        {
            UCAlunoEfetivacaoObservacaoGeral1.ObservacaoVisible = false;
            // Recuperar busca realizada e pesquisar automaticamente
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.EfetivacaoGestor)
            {
                string turmaExtinta;
                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("turmaExtinta", out turmaExtinta))
                {
                    chkTurmaExtinta.Checked = Convert.ToBoolean(turmaExtinta);
                }

                // Carrega os dados da UA superior e escola.
                string uad_id;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out uad_id);
                if (!string.IsNullOrEmpty(uad_id))
                {
                    UCComboUAEscola1.Uad_ID = new Guid(uad_id);
                    UCFiltroEscolas1__Selecionar();

                    string esc_id;
                    string uni_id;

                    if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                        (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                    {
                        UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                        UCFiltroEscolas1__SelecionarEscola();
                    }
                }

                // Carrega os dados do calendário.
                string cal_id;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out cal_id);
                UCComboCalendario1.Valor = Convert.ToInt32(cal_id);
                UCComboCalendario1_IndexChanged();

                // Carrega os dados da turma.
                string tur_id, ttn_id, crp_idTurma;
                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out tur_id) &&
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_idTurma", out crp_idTurma) &&
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out ttn_id))
                {
                    UCComboTurma1.Valor = new[] { Convert.ToInt64(tur_id), Convert.ToInt64(crp_idTurma), Convert.ToInt64(ttn_id) };
                }

                if (btnExibir.Visible)
                {
                    Pesquisar();
                }

                upnBusca.Update();
            }
        }

        /// <summary>
        /// Inicializa os combos.
        /// </summary>
        private void InicializaCombos()
        {
            UCComboCalendario1.CarregarCalendarioAnual();

            UCComboUAEscola1.ObrigatorioUA = true;
            UCComboUAEscola1.ObrigatorioEscola = true;
            UCComboCalendario1.Obrigatorio = true;
            UCComboTurma1.Obrigatorio = true;

            UCComboUAEscola1.ValidationGroup = "FechamentoGestor";
            UCComboCalendario1.ValidationGroup = "FechamentoGestor";
            UCComboTurma1.ValidationGroup = "FechamentoGestor";

            UCComboUAEscola1.Inicializar();
            UCComboUAEscola1.MostraApenasAtivas = true;
            UCComboUAEscola1.EnableEscolas = (UCComboUAEscola1.Uad_ID != Guid.Empty || !UCComboUAEscola1.DdlUA.Visible);

            if (UCComboUAEscola1.DdlUA.Visible)
            {
                UCComboUAEscola1.SelectedIndexEscolas = 0;
            }

            UCFiltroEscolas1__Selecionar();
        }

        /// <summary>
        /// Pesquisa os dados da turma.
        /// </summary>
        private void Pesquisar()
        {
            fdsSemAlunos.Visible = false;
            divResultados.Visible = false;
            UCAlunoEfetivacaoObservacaoGeral1.Mensagem = string.Empty;
            UCAlunoEfetivacaoObservacaoGeral1.ObservacaoVisible = false;
            spanMensagemSelecionarAluno.Visible = false;

            #region Salvar busca realizada com os parâmetros

            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("uad_idSuperior", UCComboUAEscola1.Uad_ID.ToString());
            filtros.Add("esc_id", UCComboUAEscola1.Esc_ID.ToString());
            filtros.Add("uni_id", UCComboUAEscola1.Uni_ID.ToString());
            filtros.Add("cal_id", UCComboCalendario1.Valor.ToString());
            filtros.Add("tur_id", UCComboTurma1.Valor[0].ToString());
            filtros.Add("crp_idTurma", UCComboTurma1.Valor[1].ToString());
            filtros.Add("ttn_id", UCComboTurma1.Valor[2].ToString());
            filtros.Add("turmaExtinta", chkTurmaExtinta.Checked.ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.EfetivacaoGestor
                ,
                Filtros = filtros
            };

            #endregion Salvar busca realizada com os parâmetros

            VS_esc_id = UCComboUAEscola1.Esc_ID;
            VS_tur_id = UCComboTurma1.Valor[0];

            CarregarAlunosTurma(VS_tur_id);

            if (rptAlunos.Items.Count > 0)
            {
                divResultados.Visible = true;
                spanMensagemSelecionarAluno.Visible = true;
                fdsSemAlunos.Visible = false;
            }
            else
            {
                divResultados.Visible = false;
                fdsSemAlunos.Visible = true;

                lblMensagemSemAlunos.Text = UtilBO.GetErroMessage(RetornaValorResource("lblMensagemSemAlunos.Text"), UtilBO.TipoMensagem.Informacao);
            }

            // Formatacao da nota numerica
            string arredondamento = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ARREDONDAMENTO_NOTA_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString();
            ACA_FormatoAvaliacao formatoAvaliacao = TUR_TurmaBO.SelecionaFormatoAvaliacao(VS_tur_id);           
            ACA_EscalaAvaliacaoNumerica escalaNum = new ACA_EscalaAvaliacaoNumerica { esa_id = formatoAvaliacao.esa_idPorDisciplina };
            ACA_EscalaAvaliacaoNumericaBO.GetEntity(escalaNum);
            hdnFormatacaoNota.Value = RetornaNumeroCasasDecimais(escalaNum) + ";"
                                      + arredondamento.ToString().ToLower() + ";"
                                      + escalaNum.ean_variacao.ToString().Replace(',', '.');
            //
        }

        /// <summary>
        /// Retorna o número de casas decimais de acordo com a variação da escala de avaliação
        /// (só se for do tipo numérica.
        /// </summary>
        /// <returns></returns>
        private int RetornaNumeroCasasDecimais(ACA_EscalaAvaliacaoNumerica entEscalaNumerica)
        {
            int numeroCasasDecimais = 1;
            if (entEscalaNumerica.esa_id > 0)
            {
                // Calcula a quantidade de casas decimais da variação de notas
                string variacao = Convert.ToDouble(entEscalaNumerica.ean_variacao).ToString();
                int notainteira;
                if (Int32.TryParse(variacao, out notainteira))
                {
                    numeroCasasDecimais = 0;
                }
                else if (variacao.IndexOf(",") >= 0)
                {
                    numeroCasasDecimais = variacao.Substring(variacao.IndexOf(","), variacao.Length - 1).Length - 1;
                }
            }
            return numeroCasasDecimais;
        }

        /// <summary>
        /// Carrega os dados no combo de turma.
        /// </summary>
        private void CarregarComboTurma()
        {
            if (UCComboCalendario1.Valor > 0)
            {
                TUR_TurmaSituacao tur_situacao = TUR_TurmaSituacao.Ativo;
                if (chkTurmaExtinta.Checked)
                {
                    tur_situacao = TUR_TurmaSituacao.Extinta;
                }
                UCComboTurma1.CarregarPorEscolaCalendarioSituacao_TurmasNormais(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, UCComboCalendario1.Valor, tur_situacao);
                UCComboTurma1.SetarFoco();
                UCComboTurma1.PermiteEditar = true;
            }
            else
            {
                UCComboTurma1.Valor = new long[] { -1, -1, -1 };
                UCComboTurma1.PermiteEditar = false;
            }
        }

        /// <summary>
        /// Evento change do combo de UA Superior.
        /// </summary>
        private void UCFiltroEscolas1__Selecionar()
        {
            try
            {
                UCComboUAEscola1.CarregaEscolaPorUASuperiorSelecionada();

                if (UCComboUAEscola1.Uad_ID != Guid.Empty || !UCComboUAEscola1.DdlUA.Visible)
                {
                    UCComboUAEscola1.FocoEscolas = true;
                    UCComboUAEscola1.EnableEscolas = true;
                }
                else
                {
                    UCComboUAEscola1.EnableEscolas = false;
                }

                UCFiltroEscolas1__SelecionarEscola();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de Escola.
        /// </summary>
        private void UCFiltroEscolas1__SelecionarEscola()
        {
            try
            {
                UCComboCalendario1.Valor = -1;
                UCComboCalendario1.PermiteEditar = false;
                UCComboTurma1.Valor = new long[] { -1, -1, -1 };
                UCComboTurma1.PermiteEditar = false;

                if (UCComboUAEscola1.Esc_ID > 0 && UCComboUAEscola1.Uni_ID > 0)
                {
                    UCComboCalendario1.SetarFoco();
                    UCComboCalendario1.PermiteEditar = true;

                    // Configura combo de calendário, caso tenha apenas uma opção.
                    if (UCComboCalendario1.QuantidadeItensCombo > 1)
                    {
                        UCComboCalendario1.SelectedIndex = 1;
                    }

                    UCComboCalendario1_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de calendário.
        /// </summary>
        private void UCComboCalendario1_IndexChanged()
        {
            try
            {
                CarregarComboTurma();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }
        
        /// <summary>
        /// Retorno dos dados.
        /// </summary>
        protected void UCAlunoEfetivacaoObservacaoGeral_ReturnValues(CLS_AlunoAvaliacaoTurmaObservacao entityObservacaoSelecionada, List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao, byte resultado, List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina)
        {
            try
            {
                // Carrega os alunos da turma.
                List<CLS_AlunoAvaliacaoTurDis_DadosAlunos> listaAlunos = VS_ListaAlunos;
                CLS_AlunoAvaliacaoTurDis_DadosAlunos dadoAlunoSelecionado = CLS_AlunoAvaliacaoTurmaDisciplinaBO.SelecionarAlunosTurma(VS_tur_id, OrdenacaoDados, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id), true, VS_alu_idSelecionado).FirstOrDefault();
                if (listaAlunos.Any(p => p.alu_id == VS_alu_idSelecionado))
                {
                    int i = listaAlunos.FindIndex(p => p.alu_id == VS_alu_idSelecionado);
                    listaAlunos[i] = dadoAlunoSelecionado;
                }
                rptAlunos.DataSource = listaAlunos;
                rptAlunos.DataBind();
                updDadosAluno.Update();
                VS_ListaAlunos = listaAlunos;

                foreach (RepeaterItem item in rptAlunos.Items)
                {
                    HiddenField hdnAluId = (HiddenField)item.FindControl("hdnAluId");
                    if (Convert.ToInt64(hdnAluId.Value) == VS_alu_idSelecionado)
                    {
                        // Adiciona o estilo ao item selecionado.
                        HtmlTableRow trNomeAlunoAtivo = (HtmlTableRow)item.FindControl("trNomeAluno");
                        trNomeAlunoAtivo.Attributes.Add("class", "aluno-selecionado");
                    }
                }

                updDadosAluno.Update();
            }
            catch (Exception ex)
            {
                divResultados.Visible = false;
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaValorResource("ErroCarregarSistema"), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os dados da turma.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        private void CarregarDadosTurma(long tur_id)
        {
            ListaQuantitativoTurmaDisciplina = ACA_CalendarioPeriodoBO.Seleciona_QtdeAulas_Turma(tur_id);
            rptDadosTurma.DataSource = ListaQuantitativoTurmaDisciplina.Select(row => new { tud_id = row.tud_id, tud_nome = row.tud_nome }).Distinct();
            rptDadosTurma.DataBind();
        }

        /// <summary>
        /// Carrega os alunos da turma.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        private void CarregarAlunosTurma(long tur_id)
        {
            VS_ListaAlunos = CLS_AlunoAvaliacaoTurmaDisciplinaBO.SelecionarAlunosTurma(tur_id, OrdenacaoDados, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id), true);
            rptAlunos.DataSource = VS_ListaAlunos;
            rptAlunos.DataBind();

            updDadosAluno.Update();
        }

        #endregion
    }
}