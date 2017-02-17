using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using System.Web.UI.HtmlControls;

namespace GestaoEscolar.Configuracao.ModuloClasse
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
                grvConfig.DataBind();
        }

        #endregion

        #region Eventos

        protected void grvConfig_DataBinding(object sender, EventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (grv.DataSource == null)
                {
                    if (ApplicationWEB.AreaAlunoSistemaID > 0)
                        grv.DataSource = CFG_ModuloClasseBO.SelecionaAtivos(ApplicationWEB.AreaAlunoSistemaID);
                    else
                        UtilBO.GetErroMessage("Não existe configuração de sistema para a área do aluno.", UtilBO.TipoMensagem.Alerta);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar configurações.", UtilBO.TipoMensagem.Erro);
                updMessage.Update();
            }
        }

        protected void grvConfig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                HtmlGenericControl divIcone = (HtmlGenericControl)e.Row.FindControl("divIcone");
                if (divIcone != null)
                    divIcone.Attributes["class"] = "p_" + grvConfig.DataKeys[e.Row.RowIndex]["mdc_classe"].ToString();
            }
        }

        protected void grvConfig_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = e.NewEditIndex;
            grv.DataBind();

            ImageButton imgSalvar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("imgSalvar");
            if (imgSalvar != null)
                imgSalvar.Visible = true;
            ImageButton imgEditar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("imgEditar");
            if (imgEditar != null)
            {
                imgEditar.Visible = false;
                ImageButton imgCancelar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("imgCancelar");
                if (imgCancelar != null)
                    imgCancelar.Visible = true;
            }

            DropDownList ddlClasseIcone = (DropDownList)grv.Rows[e.NewEditIndex].FindControl("ddlClasseIcone");
            if (ddlClasseIcone != null && !string.IsNullOrEmpty(grv.DataKeys[e.NewEditIndex]["mdc_classe"].ToString()))
                ddlClasseIcone.SelectedValue = grv.DataKeys[e.NewEditIndex]["mdc_classe"].ToString();

            HtmlGenericControl divIcone = (HtmlGenericControl)grv.Rows[e.NewEditIndex].FindControl("divIconeEdit");
            if (divIcone != null)
                divIcone.Attributes["class"] = "p_" + grv.DataKeys[e.NewEditIndex]["mdc_classe"].ToString();

            grv.Rows[e.NewEditIndex].Focus();
        }

        protected void grvConfig_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                CFG_ModuloClasse entityModClasse = new CFG_ModuloClasse()
                {
                    IsNew = false
                    ,
                    mod_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["mod_id"].ToString())
                    ,
                    mdc_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["mdc_id"].ToString())
                    ,
                    mdc_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["mdc_situacao"].ToString())
                };

                DropDownList ddlClasseIcone = (DropDownList)grvConfig.Rows[e.RowIndex].FindControl("ddlClasseIcone");
                if (ddlClasseIcone != null)
                    entityModClasse.mdc_classe = ddlClasseIcone.SelectedValue;

                if (CFG_ModuloClasseBO.Save(entityModClasse))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "mod_id: " + entityModClasse.mod_id + " mdc_id: " + entityModClasse.mdc_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Configuração alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    grv.EditIndex = -1;
                    grv.DataBind();
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
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar configuração.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                updMessage.Update();
            }
        }

        protected void grvConfig_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = -1;
            grv.DataBind();
        }

        protected void ddlClasseIcone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlClasseIcone = (DropDownList)sender;
                GridViewRow rowEdit = (GridViewRow)ddlClasseIcone.NamingContainer;

                HtmlGenericControl divIcone = (HtmlGenericControl)rowEdit.FindControl("divIconeEdit");
                if (divIcone != null)
                    divIcone.Attributes["class"] = "p_" + ddlClasseIcone.SelectedValue;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a classe de ícone.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}