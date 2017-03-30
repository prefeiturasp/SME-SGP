using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using System.Data;
using MSTech.GestaoEscolar.Entities;
using GestaoEscolar.WebControls.Combos.Novos;

namespace GestaoEscolar.Classe.JustificativaFalta
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// ID do aluno.
        /// </summary>
        private long VS_alu_id
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

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));

                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroJustificativaFalta.js"));
            }

            string script = String.Format("SetConfirmDialogButton('{0}','{1}');", String.Concat("#", btnVoltar.ClientID), String.Format("ATENÇÃO: Todas as informações não salvas serão perdidas. Deseja continuar?"));
            Page.ClientScript.RegisterStartupScript(GetType(), btnVoltar.ClientID, script, true);


            if (!IsPostBack)
            {
                lblInformacao.Visible = false;
                divLimparPesquisa.Visible = false;
                divPesquisaAluno.Visible = true;
                btnNovaJustificativaFalta.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }

            CarregaDelegatesGrid();
        }

        #endregion

        #region Eventos

        protected void UCAluno1_ReturnValues(IDictionary<string, object> parameters)
        {
            try
            {
                // Configura campos da pesquisa do aluno
                txtNomeAluno.Text = parameters["pes_nome"].ToString();
                // Carrega as justificativas de falta do aluno
                CarregaJustificativasAluno(Convert.ToInt64(parameters["alu_id"]));
                updAluno.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as justificativas de falta.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "FecharBuscaAluno", "$('#divBuscaAluno').dialog('close');", true);
            }
        }

        protected void btnBuscaAluno_Click(object sender, ImageClickEventArgs e)
        {
            UCAluno1.Limpar();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "AbrirBuscaAluno", "$('#divBuscaAluno').dialog('open');", true);
        }

        protected void btnNovaJustificativaFalta_Click(object sender, EventArgs e)
        {
            try
            {
                List<ACA_AlunoJustificativaFalta> lt = ACA_AlunoJustificativaFaltaBO.SelecionaPorAluno(VS_alu_id);
                lt.Add(new ACA_AlunoJustificativaFalta()
                {
                    IsNew = true
                    ,
                    alu_id = VS_alu_id
                    ,
                    afj_id = -1
                });

                int index = (lt.Count - 1);
                grvJustificativaFalta.EditIndex = index;
                grvJustificativaFalta.DataSource = lt;

                grvJustificativaFalta.DataBind();
                CarregaDelegatesGrid();

                grvJustificativaFalta.Rows[index].Focus();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar nova justificativa de falta.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvJustificativaFalta_DataBinding(object sender, EventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (grv.DataSource == null)
                    grv.DataSource = ACA_AlunoJustificativaFaltaBO.SelecionaPorAluno(VS_alu_id);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as justificativas de falta.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvJustificativaFalta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");
                if (btnEditar != null)
                    btnEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                    btnExcluir.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir);

                DateTime dataFim = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "afj_dataFim"));
                DateTime dataInicio = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "afj_dataInicio"));
                Label lblObservacao = (Label)e.Row.FindControl("lblObservacao");

                //Campo de observação
                string observacao = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "afj_observacao"));
                TextBox txtObservacao = (TextBox)e.Row.FindControl("txtObservacao");
                txtObservacao.Text = observacao;

                if (e.Row.RowIndex == grvJustificativaFalta.EditIndex)
                {
                    lblObservacao.Visible = false;
                    txtObservacao.Visible = true;
                    // Tipo de justificativa
                    UCCTipoJustificativa ddlTipoJustificativaFalta = (UCCTipoJustificativa)e.Row.FindControl("ddlTipoJustificativaFalta");
                    if (ddlTipoJustificativaFalta != null)
                    {
                        ddlTipoJustificativaFalta.Carregar();

                        if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "tjf_id")) > 0)
                            ddlTipoJustificativaFalta.Valor = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "tjf_id"));
                    }

                    // Data inicial da justificativa de falta
                    TextBox txtDataInicio = (TextBox)e.Row.FindControl("txtDataInicio");
                    if ((txtDataInicio != null) && (dataInicio != new DateTime()))
                    {
                        txtDataInicio.Text = dataInicio.ToString("dd/MM/yyyy");
                    }
                    // Data final da justificativa de falta
                    TextBox txtDataFim = (TextBox)e.Row.FindControl("txtDataFim");
                    if ((txtDataFim != null) && (dataFim != new DateTime()))
                    {
                        txtDataFim.Text = dataFim.ToString("dd/MM/yyyy");
                    }

                }
                else
                {
                    // Data inicial da justificativa de falta
                    Label lblDataFim = (Label)e.Row.FindControl("lblDataFim");
                    if ((lblDataFim != null) && (dataFim != new DateTime()))
                    {
                        lblDataFim.Text = dataFim.ToString("dd/MM/yyyy");
                    }

                    if (lblObservacao != null)
                    {
                        
                        lblObservacao.Text = txtObservacao.Text;
                    }
                    txtObservacao.Visible = false;
                    lblObservacao.Visible = true;

                }
            }
        }

        protected void grvJustificativaFalta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = -1;
            grv.DataBind();
        }

        protected void grvJustificativaFalta_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = e.NewEditIndex;
            grv.DataBind();

            grv.Rows[e.NewEditIndex].Focus();
        }

        protected void grvJustificativaFalta_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                GridViewRow row = grv.Rows[e.RowIndex];
                ACA_AlunoJustificativaFalta entityAlunoJustificativaFalta = new ACA_AlunoJustificativaFalta()
                {
                    IsNew = Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString())
                    ,
                    alu_id = Convert.ToInt64(grv.DataKeys[e.RowIndex]["alu_id"].ToString())
                        ,
                    afj_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["afj_id"].ToString())
                };

                UCCTipoJustificativa ddlTipoJustificativaFalta = (UCCTipoJustificativa)row.FindControl("ddlTipoJustificativaFalta");
                if (ddlTipoJustificativaFalta != null)
                    entityAlunoJustificativaFalta.tjf_id = Convert.ToInt32(ddlTipoJustificativaFalta.Valor);

                TextBox txtDataInicio = (TextBox)row.FindControl("txtDataInicio");
                if (txtDataInicio != null)
                    entityAlunoJustificativaFalta.afj_dataInicio = Convert.ToDateTime(txtDataInicio.Text);

                TextBox txtDataFim = (TextBox)row.FindControl("txtDataFim");

                TextBox txtObservacao = (TextBox)row.FindControl("txtObservacao");

                if (txtDataFim != null)
                {
                    if (!string.IsNullOrEmpty(txtDataFim.Text))
                    {
                        entityAlunoJustificativaFalta.afj_dataFim = Convert.ToDateTime(txtDataFim.Text);
                    }
                    else
                    {
                        entityAlunoJustificativaFalta.afj_dataFim = new DateTime();
                    }
                }

                //Atribui campo Observação na entidade
                entityAlunoJustificativaFalta.afj_observacao = txtObservacao.Text;

                entityAlunoJustificativaFalta.afj_situacao = Convert.ToByte(ACA_AlunoJustificativaFaltaBO.eSituacao.Ativo);
                if (ACA_AlunoJustificativaFaltaBO.Salvar(entityAlunoJustificativaFalta))
                {
                    if (Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "alu_id: " + entityAlunoJustificativaFalta.alu_id + ", afj_id" + entityAlunoJustificativaFalta.afj_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Justificativa de falta incluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "alu_id: " + entityAlunoJustificativaFalta.alu_id + ", afj_id" + entityAlunoJustificativaFalta.afj_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Justificativa de falta alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }

                    grv.EditIndex = -1;
                    grv.DataBind();

                    // Mostra mensagem informativa, caso tenha avaliação efetivada para o aluno no período da justificativa de falta
                    string nomeAvaliacao;
                    if (ACA_AlunoJustificativaFaltaBO.VerificaAlunoAvaliacao(entityAlunoJustificativaFalta, out nomeAvaliacao))
                    {
                        lblMessageInfo.Text = UtilBO.GetErroMessage(String.Concat("O aluno já teve as frequências do (", nomeAvaliacao, ") efetivadas, pode ser necessário recalcular sua frequência na opção de efetivação de notas."), UtilBO.TipoMensagem.Alerta);
                    }
                }
            }
            catch (MSTech.Validation.Exceptions.ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar justificativa de falta.", UtilBO.TipoMensagem.Erro);
            }

        }

        protected void grvJustificativaFalta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                if (!Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                {
                    ACA_AlunoJustificativaFalta entityAlunoJustificativaFalta = new ACA_AlunoJustificativaFalta()
                    {
                        alu_id = Convert.ToInt64(grv.DataKeys[e.RowIndex]["alu_id"].ToString())
                        ,
                        afj_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["afj_id"].ToString())
                        ,
                        afj_dataInicio = Convert.ToDateTime(grv.DataKeys[e.RowIndex]["afj_dataInicio"].ToString())
                        ,
                        afj_dataFim = Convert.ToDateTime(grv.DataKeys[e.RowIndex]["afj_dataFim"].ToString())
                    };

                    if (ACA_AlunoJustificativaFaltaBO.Delete(entityAlunoJustificativaFalta))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "alu_id: " + entityAlunoJustificativaFalta.alu_id + ", afj_id" + entityAlunoJustificativaFalta.afj_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Justificativa de falta excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);

                        grv.DataBind();

                        // Mostra mensagem informativa, caso tenha avaliação efetivada para o aluno no período da justificativa de falta
                        string nomeAvaliacao;
                        if (ACA_AlunoJustificativaFaltaBO.VerificaAlunoAvaliacao(entityAlunoJustificativaFalta, out nomeAvaliacao))
                        {
                            lblMessageInfo.Text = UtilBO.GetErroMessage(String.Concat("O aluno já teve as frequências do (", nomeAvaliacao, ") efetivadas, pode ser necessário recalcular sua frequência na opção de efetivação de notas."), UtilBO.TipoMensagem.Alerta);
                        }
                    }
                }
            }
            catch (MSTech.Validation.Exceptions.ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir justificativa de falta.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void ValidarDatas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool flag = true;

            CustomValidator cpvData = (CustomValidator)source;
            int index = ((GridViewRow)cpvData.NamingContainer).RowIndex;

            TextBox txtDataInicio = (TextBox)grvJustificativaFalta.Rows[index].FindControl("txtDataInicio");
            TextBox txtDataFim = (TextBox)grvJustificativaFalta.Rows[index].FindControl("txtDataFim");

            DateTime dtIni;
            DateTime.TryParse(txtDataInicio.Text, out dtIni);

            DateTime dtFim;
            DateTime.TryParse(txtDataFim.Text, out dtFim);

            if ((dtFim != new DateTime()) && (dtIni > dtFim))
                flag = false;

            args.IsValid = flag;
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Classe/JustificativaFalta/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega o grid de justificativas de falta do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        private void CarregaJustificativasAluno(Int64 alu_id)
        {
            VS_alu_id = alu_id;

            grvJustificativaFalta.DataSource = ACA_AlunoJustificativaFaltaBO.SelecionaPorAluno(alu_id);
            grvJustificativaFalta.DataBind();
            CarregaDelegatesGrid();

            lblInformacao.Text += "<b>Aluno: </b>" + txtNomeAluno.Text + "<br/>";
            lblInformacao.Visible = true;
            divLimparPesquisa.Visible = true;
            divPesquisaAluno.Visible = false;
            pnlJustificativaFalta.Visible = true;
            updJustificativaFalta.Update();
        }

        private void CarregaDelegatesGrid()
        {
            foreach (GridViewRow row in grvJustificativaFalta.Rows)
            {
                UCCTipoJustificativa ddlTipoJustificativaFalta = (UCCTipoJustificativa)row.FindControl("ddlTipoJustificativaFalta");
                if (ddlTipoJustificativaFalta != null)
                {
                    ddlTipoJustificativaFalta.IndexChanged_Sender += ddlTipoJustificativaFalta_SelectedIndexChanged;
                    ddlTipoJustificativaFalta.Visible_Label = false;
                    ddlTipoJustificativaFalta.ValidationGroup = "JustificativaFalta";
                }
            }
        }

        #endregion

        #region Delegates

        private void ddlTipoJustificativaFalta_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label lblAbonaFalta = (Label)grvJustificativaFalta.Rows[grvJustificativaFalta.EditIndex].FindControl("lblAbonaFalta");
            UCCTipoJustificativa ddlTipoJustificativaFalta = (UCCTipoJustificativa)grvJustificativaFalta.Rows[grvJustificativaFalta.EditIndex].FindControl("ddlTipoJustificativaFalta");
            if (ddlTipoJustificativaFalta != null && lblAbonaFalta != null)
            {
                ACA_TipoJustificativaFalta aux = new ACA_TipoJustificativaFalta { tjf_id = Convert.ToInt32(ddlTipoJustificativaFalta.Valor) };
                ACA_TipoJustificativaFaltaBO.GetEntity(aux);
                if (aux != null)
                {
                    if (ddlTipoJustificativaFalta.Valor != -1)
                    {
                        lblAbonaFalta.Visible = true;
                        if (aux.tjf_abonaFalta)
                            lblAbonaFalta.Text = "(Abona falta)";
                        else
                            lblAbonaFalta.Text = "(Não abona falta)";
                    }
                    else
                    {
                        lblAbonaFalta.Visible = false;
                    }
                }
            }
        }

        #endregion

    }
}
