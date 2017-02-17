using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using MSTech.Validation.Exceptions;

public partial class Configuracao_ParametroIntegracao_Cadastro : MotherPageLogado
{
    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        if (!IsPostBack)
        {
            _btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            _grvParametroIntegracao.DataBind();
        }
    }

    #endregion

    #region Eventos

    protected void _grvParametroIntegracao_DataBinding(object sender, EventArgs e)
    {
        try
        {
            GridView grv = ((GridView)sender);
            if (grv.DataSource == null)
                grv.DataSource = ACA_ParametroIntegracaoBO.Consultar();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar parâmetros de integração.", UtilBO.TipoMensagem.Erro);
            _updMessage.Update();
        }
    }

    protected void _grvParametroIntegracao_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgEditar = (ImageButton)e.Row.FindControl("_imgEditar");
            if (imgEditar != null)
                imgEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

            ImageButton imgExcluir = (ImageButton)e.Row.FindControl("_imgExcluir");
            if (imgExcluir != null)
                imgExcluir.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir) &&
                    (Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "pri_situacao")) != (Byte)eSituacao.Interno);

            if (DataBinder.Eval(e.Row.DataItem, "pri_chave").ToString() == eChaveIntegracao.HABILITA_INTEG_COLAB_DOCENTES.ToString())
            {
                TextBox txtValor = (TextBox)e.Row.FindControl("_txtValor");
                if (txtValor != null)
                    txtValor.Visible = false;

                DropDownList ddlValor = (DropDownList)e.Row.FindControl("_ddlValor");
                if (ddlValor != null)
                {
                    ddlValor.Visible = true;
                    ddlValor.SelectedValue = DataBinder.Eval(e.Row.DataItem, "pri_valor").ToString() == "Sim" ? "1" : "2";
                }
            }
        }
    }

    protected void _grvParametroIntegracao_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView grv = ((GridView)sender);
        grv.EditIndex = e.NewEditIndex;
        grv.DataBind();

        byte situacao = Byte.Parse(grv.DataKeys[e.NewEditIndex]["pri_situacao"].ToString());

        TextBox txtChave = (TextBox)grv.Rows[e.NewEditIndex].FindControl("_txtChave");
        if (txtChave != null)
            txtChave.Enabled = situacao != (Byte)eSituacao.Interno;
        ImageButton imgSalvar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("_imgSalvar");
        if (imgSalvar != null)
            imgSalvar.Visible = true;
        ImageButton imgEditar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("_imgEditar");
        if (imgEditar != null)
        {
            imgEditar.Visible = false;
            ImageButton imgCancelar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("_imgCancelar");
            if (imgCancelar != null)
                imgCancelar.Visible = true;
        }

        grv.Rows[e.NewEditIndex].Focus();
    }

    protected void _grvParametroIntegracao_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView grv = ((GridView)sender);
        try
        {
            ACA_ParametroIntegracao entityParametroIntegracao = new ACA_ParametroIntegracao()
            {
                IsNew = Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString())
                ,
                pri_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["pri_id"])
                ,
                pri_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["pri_situacao"].ToString())
            };

            TextBox txtChave = (TextBox)_grvParametroIntegracao.Rows[e.RowIndex].FindControl("_txtChave");
            if (txtChave != null)
                entityParametroIntegracao.pri_chave = txtChave.Text;
            TextBox txtDescricao = (TextBox)_grvParametroIntegracao.Rows[e.RowIndex].FindControl("_txtDescricao");
            if (txtDescricao != null)
                entityParametroIntegracao.pri_descricao = txtDescricao.Text;

            if (entityParametroIntegracao.pri_chave == eChaveIntegracao.HABILITA_INTEG_COLAB_DOCENTES.ToString())
            {
                DropDownList ddlValor = (DropDownList)_grvParametroIntegracao.Rows[e.RowIndex].FindControl("_ddlValor");
                if (ddlValor != null)
                    entityParametroIntegracao.pri_valor = ddlValor.SelectedItem.Text;
            }
            else
            {
                TextBox txtValor = (TextBox)_grvParametroIntegracao.Rows[e.RowIndex].FindControl("_txtValor");
                if (txtValor != null)
                    entityParametroIntegracao.pri_valor = txtValor.Text;
            }

            if (ACA_ParametroIntegracaoBO.Salvar(entityParametroIntegracao))
            {
                ACA_ParametroIntegracaoBO.RecarregaParametrosAtivos();

                if (Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "pri_id: " + entityParametroIntegracao.pri_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de integração incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "pri_id: " + entityParametroIntegracao.pri_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de integração alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                ApplicationWEB.RecarregarConfiguracoes();
                grv.EditIndex = -1;
                grv.DataBind();
            }
        }
        catch (ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar parâmetro de integração.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            _updMessage.Update();
        }
    }

    protected void _grvParametroIntegracao_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView grv = ((GridView)sender);
        try
        {
            if (!Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
            {
                ACA_ParametroIntegracao entityParametroIntegracao = new ACA_ParametroIntegracao()
                {
                    pri_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["pri_id"])
                    ,
                    pri_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["pri_situacao"].ToString())
                };

                if (ACA_ParametroIntegracaoBO.Delete(entityParametroIntegracao))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "pri_id: " + entityParametroIntegracao.pri_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de integração excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    ApplicationWEB.RecarregarConfiguracoes();
                    grv.DataBind();
                }
            }
        }
        catch (ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir parâmetro de integração.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            _updMessage.Update();
        }
    }

    protected void _grvParametroIntegracao_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView grv = ((GridView)sender);
        grv.EditIndex = -1;
        grv.DataBind();
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        try
        {
            List<ACA_ParametroIntegracao> parametrosIntegracao = ACA_ParametroIntegracaoBO.Consultar();
            parametrosIntegracao.Add(new ACA_ParametroIntegracao()
            {
                IsNew = true
                ,
                pri_id = -1
                ,
                pri_chave = ""
                ,
                pri_descricao = ""
                ,
                pri_valor = ""
                ,
                pri_situacao = (Byte)eSituacao.Ativo
            });

            int index = (parametrosIntegracao.Count - 1);
            _grvParametroIntegracao.EditIndex = index;
            _grvParametroIntegracao.DataSource = parametrosIntegracao;
            _grvParametroIntegracao.DataBind();

            ImageButton imgEditar = (ImageButton)_grvParametroIntegracao.Rows[index].FindControl("_imgEditar");
            if (imgEditar != null)
                imgEditar.Visible = false;
            ImageButton imgSalvar = (ImageButton)_grvParametroIntegracao.Rows[index].FindControl("_imgSalvar");
            if (imgSalvar != null)
                imgSalvar.Visible = true;
            ImageButton imgCancelar = (ImageButton)_grvParametroIntegracao.Rows[index].FindControl("_imgCancelarParametroIntegracao");
            if (imgCancelar != null)
                imgCancelar.Visible = true;

            ImageButton imgExcluir = (ImageButton)_grvParametroIntegracao.Rows[index].FindControl("_imgExcluir");
            if (imgExcluir != null)
                imgExcluir.Visible = false;

            string script = String.Format("SetConfirmDialogLoader('{0}','{1}');", String.Concat("#", imgExcluir.ClientID), "Confirma a exclusão?");
            Page.ClientScript.RegisterStartupScript(GetType(), imgExcluir.ClientID, script, true);

            _grvParametroIntegracao.Rows[index].Focus();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar novo parâmetro de integração.", UtilBO.TipoMensagem.Erro);
            _updMessage.Update();
        }
    }

    #endregion
}

