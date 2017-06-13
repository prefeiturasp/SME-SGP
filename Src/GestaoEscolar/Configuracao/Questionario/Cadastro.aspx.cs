using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
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

namespace GestaoEscolar.Configuracao.Questionario
{
    public partial class Cadastro : MotherPageLogado
    {
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
                _grvQuestionario.DataBind();
            }
        }

        #endregion

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

        protected void _grvQuestionario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgEditar = (ImageButton)e.Row.FindControl("_imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                ImageButton imgExcluir = (ImageButton)e.Row.FindControl("_imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;

                //DropDownList ddlChave = e.Row.FindControl("_ddlTipoConteudo") as DropDownList;
                //if (ddlChave != null)
                //{
                //    CarregarComboEnum<ChaveParametroDocumentoAluno>(ddlChave);
                //    CFG_ParametroDocumentoAluno row = (CFG_ParametroDocumentoAluno)e.Row.DataItem;
                //    ddlChave.SelectedValue = row.pda_chave.ToString();
                //    ddlChave.Enabled = false;
                //}

            }
        }

        protected void _grvQuestionario_DataBinding(object sender, EventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (grv.DataSource == null)
                    grv.DataSource = new List<CLS_QuestionarioConteudo>();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar.", UtilBO.TipoMensagem.Erro);
                _updMessage.Update();
            }
        }

        protected void _grvQuestionario_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = e.NewEditIndex;
            grv.DataBind();

            int qtc_id = Convert.ToInt32(grv.DataKeys[e.NewEditIndex]["qtc_id"].ToString());

            //DropDownList _ddlTipoConteudo = (DropDownList)grv.Rows[e.NewEditIndex].FindControl("_ddlTipoConteudo");
            //if (_ddlTipoConteudo != null)
            //    _ddlTipoConteudo.Enabled = true;

            //TextBox _txtDescricao = (TextBox)grv.Rows[e.NewEditIndex].FindControl("_txtDescricao");
            //if (_txtDescricao != null)
            //    _txtDescricao.Text = HttpUtility.HtmlDecode(_txtDescricao.Text);


            //TextBox _txtValor = (TextBox)grv.Rows[e.NewEditIndex].FindControl("_txtValor");
            //if (_txtValor != null)
            //    _txtValor.Text = HttpUtility.HtmlDecode(_txtValor.Text);

            //DropDownList _ddlRelatorio = (DropDownList)grv.Rows[e.NewEditIndex].FindControl("_ddlRelatorio");
            //if (_ddlRelatorio != null)
            //    _ddlRelatorio.Enabled = pda_id <= 0;

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

        protected void _grvQuestionario_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                CLS_QuestionarioConteudo entity = new CLS_QuestionarioConteudo
                {
                    IsNew = Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString())
                    ,
                    qtc_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["qtc_id"])
                    ,
                    qst_id = Byte.Parse(grv.DataKeys[e.RowIndex]["qst_id"].ToString())
                    ,
                    qtc_texto = grv.DataKeys[e.RowIndex]["qtc_texto"].ToString()
                    ,
                    qtc_tipo = Convert.ToByte(grv.DataKeys[e.RowIndex]["qst_tipo"].ToString())
                    , 
                    qtc_tipoResposta = Convert.ToByte(grv.DataKeys[e.RowIndex]["qtc_tipoResposta"].ToString())
                };

                //DropDownList _ddlRelatorio = (DropDownList)_grvParametros.Rows[e.RowIndex].FindControl("_ddlRelatorio");
                //if (_ddlRelatorio != null)
                //    entity.rlt_id = Convert.ToByte(_ddlRelatorio.SelectedValue);

                //DropDownList _ddlChave = (DropDownList)_grvParametros.Rows[e.RowIndex].FindControl("_ddlChave");
                //if (_ddlChave != null)
                //    entity.pda_chave = _ddlChave.SelectedItem.Text;

                //TextBox txtDescricao = (TextBox)_grvParametros.Rows[e.RowIndex].FindControl("_txtDescricao");
                //if (txtDescricao != null)
                //    entity.pda_descricao = txtDescricao.Text;

                //TextBox txtValor = (TextBox)_grvParametros.Rows[e.RowIndex].FindControl("_txtValor");
                //if (txtValor.Text.Length > 1000)
                //{
                //    _lblMessage.Text = UtilBO.GetErroMessage(
                //        "O tamanho do campo Valor não pode ser maior que 1000.",
                //        UtilBO.TipoMensagem.Alerta);
                //    return;
                //}
                //if (txtValor != null)
                //{
                //    entity.pda_valor = HttpUtility.HtmlEncode(txtValor.Text);
                //}

                if (CLS_QuestionarioConteudoBO.Save(entity))
                {
                    //CFG_ParametroDocumentoAlunoBO.RecarregaParametrosAtivos();

                    if (Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "qst_id: " + entity.qst_id + ", qtc_id: " + entity.qtc_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qst_id: " + entity.qst_id + ", qtc_id: " + entity.qtc_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
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
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                _updMessage.Update();
            }
        }

        protected void _grvQuestionario_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void _grvQuestionario_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void _btnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                List<CLS_QuestionarioConteudo> conteudos = (List<CLS_QuestionarioConteudo>)_grvQuestionario.DataSource;
                if (conteudos == null) conteudos = new List<CLS_QuestionarioConteudo>();
                conteudos.Add(new CLS_QuestionarioConteudo
                {
                    IsNew = true
                    ,
                    qst_id = -1
                    ,
                    qtc_id = -1
                    ,
                    qtc_texto = String.Empty
                    ,
                    qtc_tipo = 0
                });

                int index = (conteudos.Count - 1);
                _grvQuestionario.EditIndex = index;
                _grvQuestionario.DataSource = conteudos;
                _grvQuestionario.DataBind();

                ImageButton imgEditar = (ImageButton)_grvQuestionario.Rows[index].FindControl("_imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = false;
                ImageButton imgSalvar = (ImageButton)_grvQuestionario.Rows[index].FindControl("_imgSalvar");
                if (imgSalvar != null)
                    imgSalvar.Visible = true;
                ImageButton imgCancelar = (ImageButton)_grvQuestionario.Rows[index].FindControl("_imgCancelar");
                if (imgCancelar != null)
                    imgCancelar.Visible = true;

                ImageButton imgExcluir = (ImageButton)_grvQuestionario.Rows[index].FindControl("_imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = false;

                string script = String.Format("SetConfirmDialogLoader('{0}','{1}');", String.Concat("#", imgExcluir.ClientID), "Confirma a exclusão?");
                Page.ClientScript.RegisterStartupScript(GetType(), imgExcluir.ClientID, script, true);

                _grvQuestionario.Rows[index].Focus();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar nova linha.", UtilBO.TipoMensagem.Erro);
                _updMessage.Update();
            }
        }

        protected void _btnNovaResposta_Click(object sender, EventArgs e)
        {

        }
    }
}