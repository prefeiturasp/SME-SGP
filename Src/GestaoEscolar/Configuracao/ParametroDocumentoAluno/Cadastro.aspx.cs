using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.Validation.Exceptions;
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace GestaoEscolar.Configuracao.ParametroDocumentoAluno
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Constantes

        protected const string validationGroup = "Parametros";

        #endregion

        #region Eventos Page Life Cycle

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
                _grvParametros.DataBind();
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Carrega o combo a partir do enumerador.
        /// </summary>
        /// <typeparam name="T">Tipo do dado do Enumerador</typeparam>
        /// <param name="cbo">Combo</param>
        public static void CarregarComboEnum<T>(DropDownList cbo)
        {
            Type objType = typeof(T);

            cbo.DataSource = System.Enum.GetValues(objType);
            cbo.DataBind();
        }

        protected void _grvParametros_DataBinding(object sender, EventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (grv.DataSource == null)
                    grv.DataSource = CFG_ParametroDocumentoAlunoBO.GetSelect();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar parâmetros de documentos do aluno.", UtilBO.TipoMensagem.Erro);
                _updMessage.Update();
            }


        }

        protected void _grvParametros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgEditar = (ImageButton)e.Row.FindControl("_imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                ImageButton imgExcluir = (ImageButton)e.Row.FindControl("_imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir) &&
                        (Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "pda_situacao")) != (Byte)eSituacao.Interno);

                DropDownList ddlChave = e.Row.FindControl("_ddlChave") as DropDownList;
                if (ddlChave != null)
                {
                    CarregarComboEnum<ChaveParametroDocumentoAluno>(ddlChave);
                    CFG_ParametroDocumentoAluno row = (CFG_ParametroDocumentoAluno)e.Row.DataItem;
                    ddlChave.SelectedValue= row.pda_chave.ToString();
                    ddlChave.Enabled = false;
                }

            }
        }

        protected void _grvParametros_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = e.NewEditIndex;
            grv.DataBind();

            int pda_id = Convert.ToInt32(grv.DataKeys[e.NewEditIndex]["pda_id"].ToString());

            DropDownList _ddlChave = (DropDownList)grv.Rows[e.NewEditIndex].FindControl("_ddlChave");
            if (_ddlChave != null)
                _ddlChave.Enabled = true;

            TextBox _txtDescricao = (TextBox)grv.Rows[e.NewEditIndex].FindControl("_txtDescricao");
            if (_txtDescricao != null)
                _txtDescricao.Text = HttpUtility.HtmlDecode(_txtDescricao.Text);


            TextBox _txtValor = (TextBox)grv.Rows[e.NewEditIndex].FindControl("_txtValor");
            if (_txtValor != null)
                _txtValor.Text = HttpUtility.HtmlDecode(_txtValor.Text);

            DropDownList _ddlRelatorio = (DropDownList)grv.Rows[e.NewEditIndex].FindControl("_ddlRelatorio");
            if (_ddlRelatorio != null)
                _ddlRelatorio.Enabled = pda_id <= 0;

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

        protected void _grvParametros_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                CFG_ParametroDocumentoAluno entity = new CFG_ParametroDocumentoAluno
                {
                    IsNew = Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString())
                    ,
                    pda_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["pda_id"])
                    ,
                    pda_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["pda_situacao"].ToString())
                    ,
                    ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                };

                DropDownList _ddlRelatorio = (DropDownList)_grvParametros.Rows[e.RowIndex].FindControl("_ddlRelatorio");
                if (_ddlRelatorio != null)
                    entity.rlt_id = Convert.ToByte(_ddlRelatorio.SelectedValue);

                DropDownList _ddlChave = (DropDownList)_grvParametros.Rows[e.RowIndex].FindControl("_ddlChave");
                if (_ddlChave != null)
                    entity.pda_chave = _ddlChave.SelectedItem.Text;

                TextBox txtDescricao = (TextBox)_grvParametros.Rows[e.RowIndex].FindControl("_txtDescricao");
                if (txtDescricao != null)
                    entity.pda_descricao = txtDescricao.Text;

                TextBox txtValor = (TextBox)_grvParametros.Rows[e.RowIndex].FindControl("_txtValor");
                if (txtValor.Text.Length > 1000)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(
                        "O tamanho do campo Valor não pode ser maior que 1000.",
                        UtilBO.TipoMensagem.Alerta);
                    return;
                }
                if (txtValor != null)
                {
                    entity.pda_valor = HttpUtility.HtmlEncode(txtValor.Text);
                }

                if (CFG_ParametroDocumentoAlunoBO.Save(entity))
                {
                    CFG_ParametroDocumentoAlunoBO.RecarregaParametrosAtivos();

                    if (Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "pda_id: " + entity.pda_id + ", rlt_id: " + entity.rlt_id + ", ent_id: " + entity.ent_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de documentos do aluno incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "pda_id: " + entity.pda_id + ", rlt_id: " + entity.rlt_id + ", ent_id: " + entity.ent_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de documentos do aluno alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
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

        protected void _grvParametros_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                if (!Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                {
                    CFG_ParametroDocumentoAluno entity = new CFG_ParametroDocumentoAluno
                    {
                        pda_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["pda_id"])
                        ,
                        rlt_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["rlt_id"])
                        ,
                        ent_id = new Guid(Convert.ToString(grv.DataKeys[e.RowIndex]["ent_id"]))
                        ,
                        pda_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["pda_situacao"].ToString())
                    };

                    if (CFG_ParametroDocumentoAlunoBO.Delete(entity))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "pda_id: " + entity.pda_id + ", rlt_id: " + entity.rlt_id + ", ent_id: " + entity.ent_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de documentos do aluno excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

                        ApplicationWEB.RecarregarConfiguracoes();
                        grv.EditIndex = -1;
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
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir parâmetro de documentos do aluno.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                _updMessage.Update();
            }
        }

        protected void _grvParametros_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = -1;
            grv.DataBind();
        }

        protected void _btnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                List<CFG_ParametroDocumentoAluno> parametros = CFG_ParametroDocumentoAlunoBO.GetSelect().ToList();
                parametros.Add(new CFG_ParametroDocumentoAluno
                {
                    IsNew = true
                    ,
                    pda_id = -1
                    ,
                    rlt_id = (byte)ParametroDocumentoAlunoRelatorio.BoletimEscolar
                    ,
                    ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    ,
                    pda_chave = Convert.ToString(ChaveParametroDocumentoAluno.FILTRA_POR_PERIODO)
                    ,
                    pda_descricao = ""
                    ,
                    pda_valor = ""
                    ,
                    pda_situacao = 1
                    ,
                    NomeRelatorio = ""
                });

                int index = (parametros.Count - 1);
                _grvParametros.EditIndex = index;
                _grvParametros.DataSource = parametros;
                _grvParametros.DataBind();

                ImageButton imgEditar = (ImageButton)_grvParametros.Rows[index].FindControl("_imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = false;
                ImageButton imgSalvar = (ImageButton)_grvParametros.Rows[index].FindControl("_imgSalvar");
                if (imgSalvar != null)
                    imgSalvar.Visible = true;
                ImageButton imgCancelar = (ImageButton)_grvParametros.Rows[index].FindControl("_imgCancelarParametro");
                if (imgCancelar != null)
                    imgCancelar.Visible = true;

                ImageButton imgExcluir = (ImageButton)_grvParametros.Rows[index].FindControl("_imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = false;

                string script = String.Format("SetConfirmDialogLoader('{0}','{1}');", String.Concat("#", imgExcluir.ClientID), "Confirma a exclusão?");
                Page.ClientScript.RegisterStartupScript(GetType(), imgExcluir.ClientID, script, true);

                _grvParametros.Rows[index].Focus();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar novo parâmetro de documentos do aluno.", UtilBO.TipoMensagem.Erro);
                _updMessage.Update();
            }
        }


        #endregion
    }
}
