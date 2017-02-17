using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Classe.JustificativaAbonoFalta
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Armazena o ID do aluno em viewstate.
        /// </summary>
        public long VS_alu_id
        {
            get
            {
                if (ViewState["VS_alu_id"] != null)
                    return Convert.ToInt64(ViewState["VS_alu_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da matrícula do aluno em viewstate.
        /// </summary>
        public int VS_mtu_id
        {
            get
            {
                if (ViewState["VS_mtu_id"] != null)
                    return Convert.ToInt32(ViewState["VS_mtu_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_mtu_id"] = value;
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
        /// Armazena o ID da justificativa de abono de falta do aluno em viewstate.
        /// </summary>
        public int VS_ajf_id
        {
            get
            {
                if (ViewState["VS_ajf_id"] != null)
                    return Convert.ToInt32(ViewState["VS_ajf_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_ajf_id"] = value;
            }
        }

        #endregion

        #region Constantes

        /// <summary>
        /// Posição da coluna alterar.
        /// </summary>
        public const int columnAlterar = 3;

        /// <summary>
        /// Posição da coluna excluir.
        /// </summary>
        public const int columnExcluir = 4;

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega as informações do aluno que serão exibidas na tela.
        /// </summary>
        private void LoadInformacoesAluno()
        {
            try
            {
                DadosAlunoPessoa dados = ACA_AlunoBO.GetDadosAluno(VS_alu_id);

                TUR_TurmaDisciplina tud = new TUR_TurmaDisciplina { tud_id = VS_tud_id };
                TUR_TurmaDisciplinaBO.GetEntity(tud);

                lblDisciplina.Text += tud.tud_nome + "<br />";
                lblNome.Text += dados.pes_nome + "<br />";
                lblDataNascimento.Text += (Convert.ToDateTime(dados.pes_dataNascimento).ToShortDateString()) + "<br />";
                lblNomeMae.Text += dados.pesMae_nome + "<br />";
                lblDataCadastro.Text += (Convert.ToDateTime(dados.pes_dataCriacao).ToShortDateString()) + "<br />";
                lblDataAlteracao.Text += (Convert.ToDateTime(dados.pes_dataAlteracao).ToShortDateString()) + "<br />";
                lblSituacao.Text += situacao(dados.alu_situacao) + "<br />";

                DataTable matricula = VS_mtu_id >= 0 ? MTR_MatriculaTurmaBO.GetSelectDadosMatriculaAlunoMtu(VS_alu_id, VS_mtu_id) : MTR_MatriculaTurmaBO.GetSelectDadosMatriculaAluno(VS_alu_id);

                if (matricula.Rows.Count > 0)
                {

                    if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        lblEscola.Text += string.IsNullOrEmpty(matricula.Rows[0]["esc_nome"].ToString()) ? " - <br />" : matricula.Rows[0]["esc_codigo"] + " - " + matricula.Rows[0]["esc_nome"] + "<br />";
                    }
                    else
                    {
                        lblEscola.Text += string.IsNullOrEmpty(matricula.Rows[0]["esc_nome"].ToString()) ? " - <br />" : matricula.Rows[0]["esc_nome"] + "<br />";
                    }

                    lblCurso.Text = string.IsNullOrEmpty(matricula.Rows[0]["cur_nome"].ToString()) ? "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + " - " + "<br />" : "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + matricula.Rows[0]["cur_nome"] + "<br />";
                    lblPeriodo.Text = string.IsNullOrEmpty(matricula.Rows[0]["crp_descricao"].ToString()) ? "<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + " - " + "<br />" : "<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + matricula.Rows[0]["crp_descricao"] + "<br />";

                    lblTurma.Text += string.IsNullOrEmpty(matricula.Rows[0]["tur_codigo"].ToString()) ? " - <br />" : matricula.Rows[0]["tur_codigo"] + "<br />";

                    if (!string.IsNullOrEmpty(matricula.Rows[0]["mtu_numeroChamada"].ToString()))
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
                    if (!string.IsNullOrEmpty(matriculaEstadual))
                    {
                        string mtrEstadual = string.IsNullOrEmpty(matricula.Rows[0]["alc_matriculaEstadual"].ToString()) ? "-" : matricula.Rows[0]["alc_matriculaEstadual"].ToString();
                        lblRA.Text = "<b>" + matriculaEstadual + ": </b>" + mtrEstadual + "<br />";
                        lblRA.Visible = true;
                    }
                    else
                    {
                        string mtr = string.IsNullOrEmpty(matricula.Rows[0]["alc_matricula"].ToString()) ? "-" : matricula.Rows[0]["alc_matricula"].ToString();
                        lblRA.Text = "<b>" + GetGlobalResourceObject("Mensagens", "MSG_NUMEROMATRICULA") + ": " + "</b>" + mtr + "<br />";
                        lblRA.Visible = true;
                    }
                }
                else
                {
                    lblEscola.Visible = false;
                    lblCurso.Visible = false;
                    lblPeriodo.Visible = false;
                    lblTurma.Visible = false;
                    lblNChamada.Visible = false;
                    lblRA.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega o grid com as anotações dos alunos.
        /// </summary>
        private void LoadGridJustificativaFalta()
        {
            try
            {
                odsJustificativaFalta.SelectParameters.Clear();
                odsJustificativaFalta.SelectParameters.Add("alu_id", VS_alu_id.ToString());
                odsJustificativaFalta.SelectParameters.Add("tud_id", VS_tud_id.ToString());
                grvJustificativaFalta.DataBind();

                updJustificativaFalta.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método que retorna a situação do aluno em forma de string.
        /// </summary>
        /// <param name="codigo">int pes_situacao</param>
        /// <returns>string com situação do aluno</returns>
        private static string situacao(int codigo)
        {
            string ret = "";
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
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsAlunoJustificativaAbonoFalta.js"));
                }

                if (!IsPostBack)
                {
                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        VS_alu_id = PreviousPage.EditItem_AluId;
                        VS_mtu_id = PreviousPage.EditItem_MtuId;
                        VS_tud_id = PreviousPage.EditItem_TudId;

                        btnAddJustificativaFalta.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

                        grvJustificativaFalta.Columns[columnAlterar].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        grvJustificativaFalta.Columns[columnExcluir].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;

                        LoadInformacoesAluno();
                        LoadGridJustificativaFalta();
                    }
                    else
                    {
                        RedirecionarPagina("Busca.aspx");
                    }

                    btnAddJustificativaFalta.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }
            }
            catch (Exception ex)
            {
                fdsConsulta.Visible = false;
                fdsJustificativaFalta.Visible = false;
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("Busca.aspx");
        }

        protected void grvJustificativaFalta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                    if (btnExcluir != null)
                    {
                        btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                        btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    }

                    ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");
                    if (btnEditar != null)
                    {
                        btnEditar.CommandArgument = e.Row.RowIndex.ToString();
                        btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                    }
                }
            }
            catch(Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvJustificativaFalta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    VS_ajf_id = Convert.ToInt32(grvJustificativaFalta.DataKeys[index]["ajf_id"].ToString());

                    ACA_AlunoJustificativaAbonoFalta justificativa = new ACA_AlunoJustificativaAbonoFalta
                    {
                        alu_id = VS_alu_id,
                        tud_id = VS_tud_id,
                        ajf_id = VS_ajf_id
                    };
                    ACA_AlunoJustificativaAbonoFaltaBO.GetEntity(justificativa);

                    txtDataInicio.Text = justificativa.ajf_dataInicio.ToShortDateString();
                    txtDataFim.Text = justificativa.ajf_dataFim.ToShortDateString();
                    txtObservacao.Text = justificativa.ajf_observacao;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddJustificativaFalta", "$('#divCadastroJustificativaFalta').dialog('option', 'title', '" + CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.divCadastroJustificativaFalta.Title") + "');" +
                                                                                                     "$(document).ready(function() { $('#divCadastroJustificativaFalta').dialog('open'); }); " +
                                                                                                     "$(document).ready(function(){ LimitarCaracter(" + txtObservacao.ClientID + ",'contadesc3','4000'); });", true);
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int ajf_id = Convert.ToInt32(grvJustificativaFalta.DataKeys[index]["ajf_id"].ToString());

                    ACA_AlunoJustificativaAbonoFalta justificativa = new ACA_AlunoJustificativaAbonoFalta
                    {
                        alu_id = VS_alu_id,
                        tud_id = VS_tud_id,
                        ajf_id = ajf_id
                    };

                    if (ACA_AlunoJustificativaAbonoFaltaBO.Delete(justificativa))
                    {
                        lblMessage.Text = UtilBO.GetErroMessage("Justificativa de abono de falta excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, string.Format("Justificativa de abono de falta - alu_id: {0} / tud_id: {1} / ajf_id: {2}", VS_alu_id, VS_tud_id, ajf_id));
                        LoadGridJustificativaFalta();
                    }
                    else
                    {
                        lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.ErroExcluir"), UtilBO.TipoMensagem.Erro);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.ErroExcluir"), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnAddJustificativaFalta_Click(object sender, EventArgs e)
        {
            try
            {
                VS_ajf_id = -1;
                txtDataInicio.Text = "";
                txtDataFim.Text = "";
                txtObservacao.Text = "";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddJustificativaFalta", "$('#divCadastroJustificativaFalta').dialog('option', 'title', '" + CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.divCadastroJustificativaFalta.Title") + "');" +
                                                                                                 "$(document).ready(function() { $('#divCadastroJustificativaFalta').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    DateTime dataInicio = new DateTime();
                    if (!DateTime.TryParse(txtDataInicio.Text, out dataInicio))
                        throw new ValidationException(GestaoEscolarUtilBO.RetornaMsgValidacaoData(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.grvJustificativaFalta.ajf_dataInicio.HeaderText")));
                    if (dataInicio == new DateTime())
                        throw new ValidationException(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.grvJustificativaFalta.ajf_dataInicio.HeaderText"));

                    DateTime dataFim = new DateTime();
                    if (!DateTime.TryParse(txtDataFim.Text, out dataFim))
                        throw new ValidationException(GestaoEscolarUtilBO.RetornaMsgValidacaoData(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.grvJustificativaFalta.ajf_dataFim.HeaderText")));
                    if (dataFim == new DateTime())
                        throw new ValidationException(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.grvJustificativaFalta.ajf_dataFim.HeaderText"));

                    if (dataFim < dataInicio)
                    {
                        throw new ValidationException(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.ValidacaoData"));
                    }

                    ACA_AlunoJustificativaAbonoFalta justificativa = new ACA_AlunoJustificativaAbonoFalta
                    {
                        alu_id = VS_alu_id,
                        tud_id = VS_tud_id,
                        ajf_id = VS_ajf_id
                    };
                    ACA_AlunoJustificativaAbonoFaltaBO.GetEntity(justificativa);

                    justificativa.ajf_dataInicio = Convert.ToDateTime(txtDataInicio.Text);
                    justificativa.ajf_dataFim = Convert.ToDateTime(txtDataFim.Text);
                    justificativa.ajf_observacao = txtObservacao.Text;
                    justificativa.ajf_situacao = (byte)ACA_AlunoJustificativaAbonoFalta.Situacao.Ativo;
                    justificativa.ajf_status = (byte)ACA_AlunoJustificativaAbonoFalta.Status.AguardandoProcessamento;

                    if (ACA_AlunoJustificativaAbonoFaltaBO.Save(justificativa))
                    {
                        lblMessage.Text = UtilBO.GetErroMessage( VS_ajf_id > 0 ? CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.SucessoUpdate") : CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.SucessoInsert"), UtilBO.TipoMensagem.Sucesso);
                        ApplicationWEB._GravaLogSistema(VS_ajf_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, string.Format("Justificativa de abono de falta - alu_id: {0} / tud_id: {1} / ajf_id: {2}", VS_alu_id, VS_tud_id, VS_ajf_id));
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharJustificativaFalta", "$('#divCadastroJustificativaFalta').dialog('close');", true);
                        LoadGridJustificativaFalta();
                    }
                    else
                    {
                        lblMessageCadastro.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.ErroSalvar"), UtilBO.TipoMensagem.Erro);
                    }
                }
            }
            catch (ValidationException ex)
            {
                lblMessageCadastro.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageCadastro.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.ErroSalvar"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                VS_ajf_id = -1;
                txtDataInicio.Text = "";
                txtDataFim.Text = "";
                txtObservacao.Text = "";

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharJustificativaFalta", "$('#divCadastroJustificativaFalta').dialog('close');", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageCadastro.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Classe", "JustificativaAbonoFalta.Mensagem.Erro"), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}