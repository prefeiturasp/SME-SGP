using System;
using System.Data;
using System.Web;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Web.UI.WebControls;
using MSTech.Security.Cryptography;
using ReportNameGestaoAcademicaDocumentosDocente = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Web.UI;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.WebControls.AlunoAnotacoes
{
    public partial class UCAlunoAnotacoes : MotherUserControl
    {
        #region Propriedades

        public string mensagem
        {
            set
            {
                lblMessage.Text = value;
            }
        }

        public int VS_ano_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_ano_id"] ?? -1);
            }

            set
            {
                ViewState["VS_ano_id"] = value;
            }
        }

        public int VS_mtu_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_mtu_id"] ?? -1);
            }

            set
            {
                ViewState["VS_mtu_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o valor do ID do calendário.
        /// </summary>
        public int VS_cal_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_id"] ?? -1);
            }

            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o ano do calendário.
        /// </summary>
        public int VS_cal_ano
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_ano"] ?? -1);
            }

            set
            {
                ViewState["VS_cal_ano"] = value;
            }
        }

        public long _VS_alu_id
        {
            get
            {
                if (ViewState["_VS_alu_id"] != null)
                    return Convert.ToInt64(ViewState["_VS_alu_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_alu_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da turma disciplina em viewstate.
        /// </summary>
        public long VS_tud_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tud_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da turma em viewstate.
        /// </summary>
        public long VS_tur_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o valor do ID da escola.
        /// </summary>
        public int VS_esc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_esc_id"] ?? -1);
            }

            set
            {
                ViewState["VS_esc_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o valor do ID do curso.
        /// </summary>
        public int VS_cur_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cur_id"] ?? -1);
            }

            set
            {
                ViewState["VS_cur_id"] = value;
            }
        }

        /// <summary>
        /// Ent_Id do usuário logado, recupera da sessão.
        /// </summary>
        private Guid Ent_id
        {
            get
            {
                return __SessionWEB.__UsuarioWEB.Usuario.ent_id;
            }
        }

        /// <summary>
        /// Propriedade que seta a url de retorno da página.
        /// </summary>
        public string VS_PaginaRetorno
        {
            get
            {
                if (ViewState["VS_PaginaRetorno"] != null)
                    return ViewState["VS_PaginaRetorno"].ToString();

                return "";
            }
            set
            {
                ViewState["VS_PaginaRetorno"] = value;
            }
        }

        /// <summary>
        /// Propriedade que guarda dados necessários para a página de retorno.
        /// </summary>
        public object VS_DadosPaginaRetorno
        {
            get
            {
                return ViewState["VS_DadosPaginaRetorno"];
            }
            set
            {
                ViewState["VS_DadosPaginaRetorno"] = value;
            }
        }

        /// <summary>
        /// Propriedade que guarda dados necessários para a página de retorno Minhas turmas.
        /// </summary>
        public object VS_DadosPaginaRetorno_MinhasTurmas
        {
            get
            {
                return ViewState["VS_DadosPaginaRetorno_MinhasTurmas"];
            }
            set
            {
                ViewState["VS_DadosPaginaRetorno_MinhasTurmas"] = value;
            }
        }

        /// <summary>
        /// Retorna se o usuário logado é docente.
        /// </summary>
        private bool VS_visaoDocente
        {
            get
            {
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;
                return (visao == SysVisaoID.Individual && doc_id > 0);
            }
        }

        public bool CancelaSelect;

        public bool FitroCalendario = true;

        /// <summary>
        /// Retorna se é documento oficial (se for, exibir nome social e de registro)
        /// </summary>
        public eExibicaoNomePessoa VS_exibicaoNomePessoa
        {
            get
            {
                if (ViewState["VS_exibicaoNomePessoa"] == null)
                {
                    ViewState["VS_exibicaoNomePessoa"] = eExibicaoNomePessoa.NomeSocial;
                }

                return (eExibicaoNomePessoa)ViewState["VS_exibicaoNomePessoa"];
            }
            set
            {
                ViewState["VS_exibicaoNomePessoa"] = value;
            }
        }

        #endregion

        #region CONSTANTES

        public const int columnAlterar = 3;
        public const int columnExcluir = 4;

        #endregion

        #region Delegates

        public delegate void btnCancelar_Click();

        public event btnCancelar_Click cancelar_Click;

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega as informações do aluno que serão mostradas na tela.
        /// </summary>
        private void LoadInformacoesAluno()
        {
            try
            {
                ACA_Aluno alu = new ACA_Aluno();
                PES_Pessoa pes = new PES_Pessoa();
                PES_Pessoa mae = new PES_Pessoa();

                if (_VS_alu_id > 0)
                {
                    alu.alu_id = _VS_alu_id;
                    ACA_AlunoBO.GetEntity(alu);

                    if (alu.ent_id != Ent_id)
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("O aluno não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);

                        Response.Redirect("Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    pes.pes_id = alu.pes_id;
                    PES_PessoaBO.GetEntity(pes);

                    mae.pes_id = pes.pes_idFiliacaoMae;
                    PES_PessoaBO.GetEntity(mae);
                }

                lblNome.Text += pes.NomeFormatado(VS_exibicaoNomePessoa) + "<br />";
                lblDataNascimento.Text += (Convert.ToDateTime(pes.pes_dataNascimento).ToShortDateString()) + "<br />";
                string nomeMae = String.IsNullOrEmpty(mae.pes_nome) ? "-" : mae.pes_nome;
                lblNomeMae.Text += nomeMae + "<br />";
                lblDataCadastro.Text += (Convert.ToDateTime(pes.pes_dataCriacao).ToShortDateString()) + "<br />";
                lblDataAlteracao.Text += (Convert.ToDateTime(pes.pes_dataAlteracao).ToShortDateString()) + "<br />";
                lblSituacao.Text += situacao(alu.alu_situacao) + "<br />";

                DataTable matricula = VS_mtu_id >= 0 ? MTR_MatriculaTurmaBO.GetSelectDadosMatriculaAlunoMtu(_VS_alu_id, VS_mtu_id) : MTR_MatriculaTurmaBO.GetSelectDadosMatriculaAluno(_VS_alu_id);

                if (matricula.Rows.Count > 0)
                {

                    if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        lblEscola.Text += String.IsNullOrEmpty(matricula.Rows[0]["esc_nome"].ToString()) ? " - <br />" : matricula.Rows[0]["esc_codigo"] + " - " + matricula.Rows[0]["esc_nome"] + "<br />";
                    }
                    else
                    {
                        lblEscola.Text += String.IsNullOrEmpty(matricula.Rows[0]["esc_nome"].ToString()) ? " - <br />" : matricula.Rows[0]["esc_nome"] + "<br />";
                    }

                    lblCurso.Text = String.IsNullOrEmpty(matricula.Rows[0]["cur_nome"].ToString()) ? "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + " - " + "<br />" : "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + matricula.Rows[0]["cur_nome"] + "<br />";
                    lblPeriodo.Text = String.IsNullOrEmpty(matricula.Rows[0]["crp_descricao"].ToString()) ? "<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + " - " + "<br />" : "<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + matricula.Rows[0]["crp_descricao"] + "<br />";

                    lblTurma.Text += String.IsNullOrEmpty(matricula.Rows[0]["tur_codigo"].ToString()) ? " - <br />" : matricula.Rows[0]["tur_codigo"] + "<br />";

                    if (string.IsNullOrEmpty(matricula.Rows[0]["crp_nomeAvaliacao"].ToString()))
                    {
                        lblAvaliacao.Visible = false;
                    }
                    else
                    {
                        lblAvaliacao.Text = "<b>" + matricula.Rows[0]["crp_nomeAvaliacao"] + ": </b>" + matricula.Rows[0]["crp_nomeAvaliacao"] + " " + matricula.Rows[0]["tca_numeroAvaliacao"] + "<BR />";
                        lblAvaliacao.Visible = true;
                    }
                    if (!String.IsNullOrEmpty(matricula.Rows[0]["mtu_numeroChamada"].ToString()))
                    {
                        if (Convert.ToInt32(matricula.Rows[0]["mtu_numeroChamada"]) > 0)
                            lblNChamada.Text += matricula.Rows[0]["mtu_numeroChamada"] + "<br />";
                        else
                            lblNChamada.Text += " - <br />";
                    }
                    else
                    {
                        lblNChamada.Text += " - <br />";
                    }


                    string matriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    if (!String.IsNullOrEmpty(matriculaEstadual))
                    {
                        string mtrEstadual = String.IsNullOrEmpty(matricula.Rows[0]["alc_matriculaEstadual"].ToString()) ? "-" : matricula.Rows[0]["alc_matriculaEstadual"].ToString();
                        lblRA.Text = "<b>" + matriculaEstadual + ": </b>" + mtrEstadual + "<br />";
                        lblRA.Visible = true;
                    }
                    else
                    {
                        string mtr = String.IsNullOrEmpty(matricula.Rows[0]["alc_matricula"].ToString()) ? "-" : matricula.Rows[0]["alc_matricula"].ToString();
                        lblRA.Text = "<b>" + GetGlobalResourceObject("Mensagens", "MSG_NUMEROMATRICULA") + ": " + "</b>" + mtr + "<br />";
                        lblRA.Visible = true;
                    }

                    //Carrega nas propriedades os ids: Escola, Curso, Turma
                    VS_cur_id = Convert.ToInt32(matricula.Rows[0]["cur_id"]);
                    VS_esc_id = Convert.ToInt32(matricula.Rows[0]["esc_id"]);
                    VS_tur_id = Convert.ToInt32(matricula.Rows[0]["tur_id"]);
                    VS_cal_id = Convert.ToInt32(matricula.Rows[0]["cal_id"]);
                    VS_cal_ano = Convert.ToInt32(matricula.Rows[0]["cal_ano"]);
                }
                else
                {
                    lblEscola.Visible = false;
                    lblCurso.Visible = false;
                    lblPeriodo.Visible = false;
                    lblTurma.Visible = false;
                    lblNChamada.Visible = false;
                    lblRA.Visible = false;
                    lblAvaliacao.Visible = false;
                }

                if (FitroCalendario)
                {

                    ddlAnoCalendario.Items.Clear();
                    odsAnoCalendario.SelectParameters.Add("alu_id", _VS_alu_id.ToString());
                    ddlAnoCalendario.DataBind();

                    int cal_id = -1;
                    int max = -1;
                    //Pega o calendário do ano atual ou o último ano que o aluno possui anotação.
                    foreach (ListItem lst in ddlAnoCalendario.Items)
                        if (Convert.ToInt32(lst.Text) == DateTime.Today.Year)
                        {
                            cal_id = Convert.ToInt32(lst.Value);
                            break;
                        }
                        else if (Convert.ToInt32(lst.Text) > max)
                        {
                            cal_id = Convert.ToInt32(lst.Value);
                            max = Convert.ToInt32(lst.Text);
                        }
                    ddlAnoCalendario.SelectedValue = cal_id.ToString();

                    if (ddlAnoCalendario.Items.Count == 0)
                    {
                        ddlAnoCalendario.Visible = false;
                        lblAnoCalendario.Visible = false;
                    }
                }
                else
                {
                    divAnoCalendario.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar dados do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega o grid com as anotações dos alunos.
        /// </summary>
        private void LoadGridAnotacoes()
        {
            try
            {
                odsAnotacoes.SelectParameters.Clear();
                odsAnotacoes.SelectParameters.Add("alu_id", _VS_alu_id.ToString());
                odsAnotacoes.SelectParameters.Add("cal_ano", ddlAnoCalendario.Items.Count > 0 ? ddlAnoCalendario.SelectedItem.Text : VS_cal_ano.ToString());
                odsAnotacoes.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                odsAnotacoes.SelectParameters.Add("usuario_superior", (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0).ToString());
                odsAnotacoes.DataBind();

                odsAnotacoesGerais.SelectParameters.Clear();
                odsAnotacoesGerais.SelectParameters.Add("alu_id", _VS_alu_id.ToString());
                odsAnotacoesGerais.SelectParameters.Add("cal_ano", ddlAnoCalendario.Items.Count > 0 ? ddlAnoCalendario.SelectedItem.Text : VS_cal_ano.ToString());
                odsAnotacoesGerais.DataBind();

                grvAnotacoes.DataBind();
                grvAnotacoesGerais.DataBind();

                btnImprimir.Visible = grvAnotacoes.Rows.Count > 0 || grvAnotacoesGerais.Rows.Count > 0;

                updAnotacoes.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as anotações sobre o aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método que retorna a situação do aluno em forma de String.
        /// </summary>
        /// <param name="codigo">int pes_situacao</param>
        /// <returns>String com situação do aluno</returns>
        private static String situacao(int codigo)
        {
            String ret = "";
            switch (codigo)
            {
                case (int)ACA_AlunoSituacao.Ativo:
                    ret = CustomResource.GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.Ativo");
                    break;
                case (int)ACA_AlunoSituacao.Excluido:
                    ret = CustomResource.GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.Excluido");
                    break;
                case (int)ACA_AlunoSituacao.Inativo:
                    ret = CustomResource.GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.Inativo");
                    break;
                case (int)ACA_AlunoSituacao.Formado:
                    ret = CustomResource.GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.Formado");
                    break;
                case (int)ACA_AlunoSituacao.EmMatricula:
                    ret = CustomResource.GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.EmMatricula");
                    break;
                case (int)ACA_AlunoSituacao.Excedente:
                    ret = CustomResource.GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.Excedente");
                    break;
                case (int)ACA_AlunoSituacao.EmPreMatricula:
                    ret = CustomResource.GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.PreMatricula");
                    break;
                case (int)ACA_AlunoSituacao.Evadido:
                    ret = CustomResource.GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.Evadido");
                    break;
                case (int)ACA_AlunoSituacao.EmMovimentacao:
                    ret = CustomResource.GetGlobalResourceObject("Academico", "Aluno.AlunoSituacao.EmMovimentacao");
                    break;
            }

            return ret;
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (_VS_alu_id > 0)
                    {
                        btnAddAnotacao.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir &&
                                                 !VS_visaoDocente;
                        grvAnotacoesGerais.Columns[columnAlterar].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar &&
                                                                            !VS_visaoDocente;
                        grvAnotacoesGerais.Columns[columnExcluir].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir &&
                                                                            !VS_visaoDocente;

                        divAnotacoesGerais.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_CADASTRO_ANOTACOES_GERAIS_ALUNO,
                                                                                                                __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                    && (!VS_visaoDocente
                                                        || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_APENAS_ANOTACOES_ALUNO_DOCENTE,
                                                                                                                        __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                        LoadInformacoesAluno();
                        odsAnotacoes.SelectParameters.Add("cal_ano", "-1");
                        odsAnotacoesGerais.SelectParameters.Add("cal_ano", "-1");
                        LoadGridAnotacoes();
                    }
                    else
                    {
                        CancelaSelect = true;

                        if (cancelar_Click != null)
                            cancelar_Click();
                        //Response.Redirect("Busca.aspx", false);
                        //HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            if (cancelar_Click != null)
                cancelar_Click();
        }

        protected void ddlAnoCalendario_IndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlAnoCalendario.SelectedValue) > 0)
            {
                odsAnotacoes.SelectParameters.Clear();
                odsAnotacoes.SelectParameters.Add("cal_ano", ddlAnoCalendario.SelectedValue);

                odsAnotacoesGerais.SelectParameters.Clear();
                odsAnotacoesGerais.SelectParameters.Add("cal_ano", ddlAnoCalendario.SelectedValue);
                LoadGridAnotacoes();
            }
        }

        protected void odsAnoCalendario_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.Cancel = CancelaSelect;
        }

        protected void odsAnotacoes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.Cancel = CancelaSelect;
        }

        protected void odsAnotacoesGerais_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.Cancel = CancelaSelect;
        }

        protected void grvAnotacoesGerais_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");
                if (btnEditar != null)
                {
                    btnEditar.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void grvAnotacoesGerais_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    VS_ano_id = Convert.ToInt32(grvAnotacoesGerais.DataKeys[index]["ano_id"].ToString());

                    ACA_AlunoAnotacao ano = new ACA_AlunoAnotacao
                    {
                        alu_id = _VS_alu_id,
                        ano_id = VS_ano_id
                    };
                    ACA_AlunoAnotacaoBO.GetEntity(ano);

                    txtDataAnotacao.Text = ano.ano_dataAnotacao.ToShortDateString();
                    txtAnotacao.Text = ano.ano_anotacao;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddAnotacao", "var exibeMensagemSair=" + btnSalvar.Visible.ToString().ToLower() + ";" +
                                                                                           "$('#divCadastroAnotacao').dialog('option', 'title', '" + CustomResource.GetGlobalResourceObject("UserControl", "UCAlunoAnotacoes.divCadastroAnotacao.Title") + "');" +
                                                                                           "$(document).ready(function() { $('#divCadastroAnotacao').dialog('open'); }); " + 
                                                                                           "$(document).ready(function(){ LimitarCaracter(" + txtAnotacao.ClientID + ",'contadesc3','4000'); });", true);
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + ".", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int ano_id = Convert.ToInt32(grvAnotacoesGerais.DataKeys[index]["ano_id"].ToString());

                    ACA_AlunoAnotacao ano = new ACA_AlunoAnotacao
                    {
                        alu_id = _VS_alu_id,
                        ano_id = ano_id
                    };
                    ACA_AlunoAnotacaoBO.GetEntity(ano);

                    ano.ano_dataAlteracao = DateTime.Now;
                    ano.ano_situacao = 3;

                    ACA_AlunoAnotacaoBO.Save(ano);

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "AnotacaoAluno alu_id:" + _VS_alu_id.ToString() +
                                                                            " ano_id:" + ano.ano_id.ToString());

                    LoadGridAnotacoes();
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a anotação.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnAddAnotacao_Click(object sender, EventArgs e)
        {
            try
            {
                VS_ano_id = -1;
                txtDataAnotacao.Text = DateTime.Today.ToShortDateString();
                txtAnotacao.Text = "";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddAnotacao", "var exibeMensagemSair=" + btnSalvar.Visible.ToString().ToLower() + ";" + 
                                                                                       "$('#divCadastroAnotacao').dialog('option', 'title', '" + CustomResource.GetGlobalResourceObject("UserControl", "UCAlunoAnotacoes.divCadastroAnotacao.Title") + "');" +
                                                                                       "$(document).ready(function() { $('#divCadastroAnotacao').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar cadastrar nova anotação geral sobre o aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    DateTime dataAnotacao = new DateTime();
                    if (!DateTime.TryParse(txtDataAnotacao.Text, out dataAnotacao))
                        throw new ValidationException(GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data da anotação"));
                    if (dataAnotacao == new DateTime())
                        throw new ValidationException("Data da anotação inválida.");

                    ACA_AlunoAnotacao ano = new ACA_AlunoAnotacao
                                                {
                                                    alu_id = _VS_alu_id,
                                                    ano_id = VS_ano_id
                                                };
                    ACA_AlunoAnotacaoBO.GetEntity(ano);

                    if (ano.IsNew)
                        ano.ano_dataCriacao = DateTime.Now;

                    ano.ano_anotacao = txtAnotacao.Text;
                    ano.ano_dataAnotacao = Convert.ToDateTime(txtDataAnotacao.Text);
                    ano.usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                    ano.gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id;
                    ano.ano_situacao = 1;
                    ano.ano_dataAlteracao = DateTime.Now;

                    ACA_AlunoAnotacaoBO.Save(ano);

                    ApplicationWEB._GravaLogSistema(VS_ano_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "AnotacaoAluno alu_id:" + _VS_alu_id.ToString() +
                                                                                                                     " ano_id:" + ano.ano_id.ToString());
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharAnotacao", "var exibirMensagemConfirmacao=false;$('#divCadastroAnotacao').dialog('close');", true);

                    LoadGridAnotacoes();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao salvar anotação geral sobre o aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelar_Click1(object sender, EventArgs e)
        {
            try
            {
                VS_ano_id = -1;
                txtDataAnotacao.Text = "";
                txtAnotacao.Text = "";
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                string parameter = string.Empty;
                string report = string.Empty;

                if (!ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.RELATORIO_ANOTACAO_GERAL_BUSCA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {  // SME-SP
                    report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelAnotacoesAula).ToString();
                    parameter = "esc_id=" + VS_esc_id +
                                "&cal_id=" + VS_cal_id +
                                "&cur_id=" + VS_cur_id +
                                "&tur_id=" + VS_tur_id +
                                "&cap_id=" + -1 +
                                "&tud_id=" + -1 +
                                "&doc_id=" + -1 +
                                "&dataAula=" + -1 +
                                "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                "&nomeDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                "&alu_id=" + _VS_alu_id +
                                "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio") +
                                "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria") +
                                "&lblCodigoEOL2=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctRelAnotacoesAula.lblCodigoEOL2.label") +
                                "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarCredencialServidorPorRelatorio(__SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToInt32(report)).srr_pastaRelatorios.ToString()
                                                , ApplicationWEB.LogoRelatorioSSRS) +
                                "&cal_ano=" + VS_cal_ano.ToString() +
                                "&documentoOficial=false";
                }
                else
                {   // demais clientes
                    report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.GestaoAcademicaAnotacoes).ToString();
                    parameter = "alu_id=" + _VS_alu_id +
                                "&cal_ano=" + ACA_CalendarioAnualBO.SelecionaPorTurma(VS_tur_id).cal_ano +
                                "&matricula=" + GetGlobalResourceObject("Mensagens", "MSG_NUMEROMATRICULA").ToString() +
                                "&usu_id=" + __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString() +
                                "&usuario_superior=" + (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0).ToString() + 
                                "&ordenaPorCodigo=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString() +
                                "&atg_tipo=" + ((int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString() +
                                "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS);
                }

                Session["PaginaRetorno_AnotacoesAluno"] = VS_PaginaRetorno;
                Session["DadosPaginaRetorno"] = VS_DadosPaginaRetorno;
                Session["VS_DadosTurmas"] = VS_DadosPaginaRetorno_MinhasTurmas;
                Session["tud_id_anotacoes"] = VS_tud_id > 0 ? (object)VS_tud_id : null;
                Session["alu_id_anotacoes"] = _VS_alu_id;
                Session["mtu_id_anotacoes"] = VS_mtu_id;

                CFG_RelatorioBO.CallReport("Documentos", report, parameter, HttpContext.Current);
            }
            catch (Exception error)
            {
                ApplicationWEB._GravaErro(error);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar relatório de anotações sobre o aluno.", UtilBO.TipoMensagem.Erro);
            }

        }

        #endregion
    }
}