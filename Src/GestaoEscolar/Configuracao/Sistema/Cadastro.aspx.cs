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

public partial class Configuracao_Sistema_Cadastro : MotherPageLogado
{
    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));

        if (!IsPostBack)
            btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
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
                grv.DataSource = CFG_ConfiguracaoAcademicoBO.Consultar();
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

            ImageButton imgExcluir = (ImageButton)e.Row.FindControl("imgExcluir");
            if (imgExcluir != null)
                imgExcluir.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir) &&
                    (Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "cfg_situacao")) != Convert.ToByte(CFG_ConfiguracaoAcademicoBO.eSituacao.Interno));
        }
    }

    protected void grvConfig_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView grv = ((GridView)sender);
        grv.EditIndex = e.NewEditIndex;
        grv.DataBind();

        byte situacao = Byte.Parse(grv.DataKeys[e.NewEditIndex]["cfg_situacao"].ToString());

        TextBox txtChave = (TextBox)grv.Rows[e.NewEditIndex].FindControl("txtChave");
        if (txtChave != null)
            txtChave.Enabled = situacao != Convert.ToByte(CFG_ConfiguracaoAcademicoBO.eSituacao.Interno);
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
        grv.Rows[e.NewEditIndex].Focus();
    }

    protected void grvConfig_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView grv = ((GridView)sender);
        try
        {
            CFG_ConfiguracaoAcademico entityConfiguracao = new CFG_ConfiguracaoAcademico()
            {
                IsNew = Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString())
                ,
                cfg_id = new Guid(grv.DataKeys[e.RowIndex]["cfg_id"].ToString())
                ,
                cfg_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["cfg_situacao"].ToString())
            };

            TextBox txtValor = (TextBox)grvConfig.Rows[e.RowIndex].FindControl("txtValor");
            if (txtValor != null)
                entityConfiguracao.cfg_valor = txtValor.Text;
            TextBox txtDescricao = (TextBox)grvConfig.Rows[e.RowIndex].FindControl("txtDescricao");
            if (txtDescricao != null)
                entityConfiguracao.cfg_descricao = txtDescricao.Text;
            TextBox txtChave = (TextBox)grvConfig.Rows[e.RowIndex].FindControl("txtChave");
            if (txtChave != null)
                entityConfiguracao.cfg_chave = txtChave.Text;

            if (CFG_ConfiguracaoAcademicoBO.Salvar(entityConfiguracao))
            {
                if (Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "cfg_id: " + entityConfiguracao.cfg_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Configuração incluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "cfg_id: " + entityConfiguracao.cfg_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Configuração alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                ApplicationWEB.RecarregarConfiguracoes();
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

    protected void grvConfig_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView grv = ((GridView)sender);
        try
        {
            if (!Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
            {
                CFG_ConfiguracaoAcademico entityConfiguracao = new CFG_ConfiguracaoAcademico()
                {
                    cfg_id = new Guid(grv.DataKeys[e.RowIndex]["cfg_id"].ToString())
                    ,
                    cfg_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["cfg_situacao"].ToString())
                };

                if (CFG_ConfiguracaoAcademicoBO.Deletar(entityConfiguracao))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "cfg_id: " + entityConfiguracao.cfg_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Configuração excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    ApplicationWEB.RecarregarConfiguracoes();
                    grv.DataBind();
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
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir configuração.", UtilBO.TipoMensagem.Erro);
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

    protected void btnNovo_Click(object sender, EventArgs e)
    {
        try
        {
            List<CFG_ConfiguracaoAcademico> lt = CFG_ConfiguracaoAcademicoBO.Consultar();
            lt.Add(new CFG_ConfiguracaoAcademico()
            {
                IsNew = true
                ,
                cfg_id = Guid.Empty
                ,
                cfg_situacao = Convert.ToByte(CFG_ConfiguracaoAcademicoBO.eSituacao.Ativo)
            });

            int index = (lt.Count - 1);
            grvConfig.EditIndex = index;
            grvConfig.DataSource = lt;
            grvConfig.DataBind();

            ImageButton imgEditar = (ImageButton)grvConfig.Rows[index].FindControl("imgEditar");
            if (imgEditar != null)
                imgEditar.Visible = false;
            ImageButton imgSalvar = (ImageButton)grvConfig.Rows[index].FindControl("imgSalvar");
            if (imgSalvar != null)
                imgSalvar.Visible = true;
            ImageButton imgCancelar = (ImageButton)grvConfig.Rows[index].FindControl("imgCancelarConfg");
            if (imgCancelar != null)
                imgCancelar.Visible = true;
                
            ImageButton imgExcluir = (ImageButton)grvConfig.Rows[index].FindControl("imgExcluir");
            if (imgExcluir != null)
                imgExcluir.Visible = false;
            grvConfig.Rows[index].Focus();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar nova configuração.", UtilBO.TipoMensagem.Erro);
            updMessage.Update();
        }
    }

    #endregion  
}
