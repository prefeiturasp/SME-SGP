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
        #region Propriedades
        public int _VS_qst_id
        {
            get
            {
                if (ViewState["_VS_qst_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_qst_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_qst_id"] = value;
            }
        }
        
        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    _Carregar(PreviousPage.PaginaQuestionario_qst_id);
                else
                {
                    _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }

                Page.Form.DefaultFocus = _txtTitulo.ClientID;
                Page.Form.DefaultButton = _btnSalvar.UniqueID;
            }
        }
        
        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("BuscaQuestionario.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _btnSalvar_Click(object sender, EventArgs e)
        {
            _Salvar();
        }

        #endregion

        #region Métodos
        
        private void _Carregar(int qst_id)
        {
            try
            {
                CLS_Questionario Questionario = new CLS_Questionario { qst_id = qst_id };
                CLS_QuestionarioBO.GetEntity(Questionario);
                _VS_qst_id = Questionario.qst_id;
                _txtTitulo.Text = Questionario.qst_titulo;
            }
            catch (Exception e)
            {
                ApplicationWEB._GravaErro(e);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o recurso de aula.", UtilBO.TipoMensagem.Erro);
            }
        }
        
        private void _Salvar()
        {
            try
            {
                CLS_Questionario Questionario = new CLS_Questionario
                {
                    qst_id = _VS_qst_id
                };

                CLS_QuestionarioBO.GetEntity(Questionario);

                if (_txtTitulo.Text.Length > 500)
                    throw new ValidationException("O título do questionário não deve exceder 500 caracteres.");

                Questionario.qst_titulo = _txtTitulo.Text;
                Questionario.qst_situacao = 1; //ativo

                DataTable dtTituloRepetido = CLS_QuestionarioBO.GetQuestionarioBy_qst_titulo(_txtTitulo.Text.Trim());
                if (dtTituloRepetido.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dtTituloRepetido.Rows[0]["qst_id"]) != Questionario.qst_id)
                    {
                        throw new ValidationException("Existe um questionário cadastrado com esse nome!");
                    }
                }

                if (CLS_QuestionarioBO.Save(Questionario))
                {
                    if (_VS_qst_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "qst_id: " + Questionario.qst_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Questionário incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qst_id: " + Questionario.qst_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Questionário alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }

                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/Questionario/BuscaQuestionario.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o questionário.", UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ValidationException e)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException e)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException e)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception e)
            {
                ApplicationWEB._GravaErro(e);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o questionário.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}