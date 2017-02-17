using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System.Web;

namespace GestaoEscolar.Configuracao.ParametroMensagem
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        string txtChaveText = "";

        #endregion

        #region Constantes

        protected const string validationGroup = "Parametros";

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grvParametro.Rows)
            {
                TextBox txtChave = (TextBox)row.FindControl("_txtChave");
                if (txtChave != null)
                    txtChaveText = txtChave.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
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
                grvParametro.DataBind();
            }
        }

        #endregion

        #region Eventos

        protected void grvParametro_DataBinding(object sender, EventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (grv.DataSource == null)
                    grv.DataSource = CFG_ParametroMensagemBO.GetSelect();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar parâmetros de mensagem.", UtilBO.TipoMensagem.Erro);
                _updMessage.Update();
            }
        }

        protected void grvParametro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgEditar = (ImageButton)e.Row.FindControl("_imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                ImageButton imgExcluir = (ImageButton)e.Row.FindControl("_imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir) &&
                        (Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "pms_situacao")) != (Byte)eSituacao.Interno);

            }
        }

        protected void grvParametro_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = e.NewEditIndex;
            grv.DataBind();

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

            TextBox txtChave = (TextBox)grvParametro.Rows[e.NewEditIndex].FindControl("_txtChave");
            if (txtChave != null)
            {
                txtChave.Enabled = false;
                txtChave.ReadOnly = true;
            }

            grv.Rows[e.NewEditIndex].Focus();
        }

        protected void grvParametro_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                CFG_ParametroMensagem entity = new CFG_ParametroMensagem
                {
                    pms_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["pms_id"])
                };

                CFG_ParametroMensagemBO.GetEntity(entity);

                if (entity.IsNew)
                {
                    entity.pms_situacao = (Byte)CFG_ParametroMensagemSituacao.Ativo;
                    entity.pms_chave = txtChaveText;
                }

                entity.pms_tela = 0;

                TextBox txtDescricao = (TextBox)grvParametro.Rows[e.RowIndex].FindControl("_txtDescricao");
                if (txtDescricao != null)
                    entity.pms_descricao = txtDescricao.Text;

                TextBox txtValor = (TextBox)grvParametro.Rows[e.RowIndex].FindControl("_txtValor");
                if (txtValor != null)
                    entity.pms_valor = txtValor.Text;

                if (entity.IsNew && CFG_ParametroMensagemBO.VerificaParametroExistente(entity.pms_chave))
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Já existe um parâmetro com a mesma chave cadastrado no sistema.",
                                                             UtilBO.TipoMensagem.Alerta);
                    grv.EditIndex = -1;
                    grv.DataBind();
                }
                else if (CFG_ParametroMensagemBO.Save(entity))
                {
                    CFG_ParametroMensagemBO.RecarregaParametrosAtivos();

                    if (Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "pms_id: " + entity.pms_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de mensagem incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "pms_id: " + entity.pms_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de mensagem alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
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
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar parâmetro.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                _updMessage.Update();
            }
        }

        protected void grvParametro_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                if (!Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                {
                    CFG_ParametroMensagem entity = new CFG_ParametroMensagem
                    {
                        pms_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["pms_id"])
                        ,
                        pms_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["pms_situacao"].ToString())
                    };

                    if (CFG_ParametroMensagemBO.Delete(entity))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "pms_id: " + entity.pms_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de mensagem excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

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
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir parâmetro de mensagem.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                _updMessage.Update();
            }
        }

        protected void grvParametro_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = -1;
            grv.DataBind();
        }

        protected void _btnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                List<CFG_ParametroMensagem> parametros = CFG_ParametroMensagemBO.GetSelect().ToList();
                parametros.Add(new CFG_ParametroMensagem
                {
                    IsNew = true
                    ,
                    pms_id = -1
                    ,
                    pms_chave = ""
                    ,
                    pms_descricao = ""
                    ,
                    pms_valor = ""
                    ,
                    pms_tela = (byte)CFG_ParametroMensagemTela.PlanejamentoAnual
                    ,
                    pms_situacao = (Byte)CFG_ParametroMensagemSituacao.Ativo
                });

                int index = (parametros.Count - 1);
                grvParametro.EditIndex = index;
                grvParametro.DataSource = parametros;
                grvParametro.DataBind();

                ImageButton imgEditar = (ImageButton)grvParametro.Rows[index].FindControl("_imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = false;
                ImageButton imgSalvar = (ImageButton)grvParametro.Rows[index].FindControl("_imgSalvar");
                if (imgSalvar != null)
                    imgSalvar.Visible = true;
                ImageButton imgCancelar = (ImageButton)grvParametro.Rows[index].FindControl("_imgCancelarParametro");
                if (imgCancelar != null)
                    imgCancelar.Visible = true;

                ImageButton imgExcluir = (ImageButton)grvParametro.Rows[index].FindControl("_imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = false;

                TextBox txtChave = (TextBox)grvParametro.Rows[index].FindControl("_txtChave");
                if (txtChave != null)
                    txtChave.Text = "MSG_";

                txtChave.Enabled = true;
                txtChave.ReadOnly = false;

                string script = String.Format("SetConfirmDialogLoader('{0}','{1}');", String.Concat("#", imgExcluir.ClientID), "Confirma a exclusão?");
                Page.ClientScript.RegisterStartupScript(GetType(), imgExcluir.ClientID, script, true);

                grvParametro.Rows[index].Focus();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar novo parâmetro de mensagem.", UtilBO.TipoMensagem.Erro);
                _updMessage.Update();
            }
        }

        #endregion
    }
}
