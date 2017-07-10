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
    public partial class CadastroResposta : MotherPageLogado
    {
        #region Propriedades
        private int _VS_qtc_id
        {
            get
            {
                if (ViewState["_VS_qtc_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_qtc_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_qtc_id"] = value;
            }
        }

        private int _VS_qst_id
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

        public int _VS_qtr_id
        {
            get
            {
                if (ViewState["_VS_qtr_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_qtr_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_qtr_id"] = value;
            }
        }

        public bool IsMultiplaSelecao {
            get
            { 
                if (ViewState["IsMultiplaSelecao"] != null)
                    return Convert.ToBoolean(ViewState["IsMultiplaSelecao"]);
                return false;
            }
            set
            {
                ViewState["IsMultiplaSelecao"] = value;
            }
        }

        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            if (!IsPostBack)
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {                   
                    _VS_qtc_id = PreviousPage._VS_qtc_id;
                    _VS_qtr_id = PreviousPage.PaginaResposta_qtr_id;
                    _VS_qst_id = PreviousPage._VS_qst_id;                    
                    CLS_QuestionarioConteudo Conteudo = new CLS_QuestionarioConteudo { qtc_id = _VS_qtc_id, qst_id = _VS_qst_id };
                    CLS_QuestionarioConteudoBO.GetEntity(Conteudo);
                    IsMultiplaSelecao = Conteudo.qtc_tipoResposta == (byte)QuestionarioTipoResposta.MultiplaSelecao;
                    _Carregar(_VS_qtc_id, _VS_qtr_id);
                }

                else
                {
                    _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }

                Page.Form.DefaultFocus = _txtTexto.ClientID;
                Page.Form.DefaultButton = _btnSalvar.UniqueID;
            }
        }
        
        protected void _btnSalvar_Click(object sender, EventArgs e)
        {
            _Salvar();
        }

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            Session["qtc_id"] = _VS_qtc_id;
            Session["qst_id"] = _VS_qst_id;
            Response.Redirect("BuscaResposta.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion

        #region Métodos
        private void _Salvar()
        {
            try
            {
                CLS_QuestionarioResposta Resposta = new CLS_QuestionarioResposta
                {
                    qtr_id = _VS_qtr_id
                    ,
                    qtc_id = _VS_qtc_id
                };

                CLS_QuestionarioRespostaBO.GetEntity(Resposta);

                if (_txtTexto.Text.Length > 4000)
                    throw new ValidationException("O texto da resposta não deve exceder 4000 caracteres.");

                if (IsMultiplaSelecao && Convert.ToInt32(_txtPeso.Text) <= 0)
                    throw new ValidationException("O peso da resposta deve ser maior que zero.");

                Resposta.qtr_texto = _txtTexto.Text;
                Resposta.qtr_permiteAdicionarTexto = _chkPermiteAdicionarTexto.Checked;
                Resposta.qtr_peso = IsMultiplaSelecao ? Convert.ToInt32(_txtPeso.Text) : 0;
                Resposta.qtr_situacao = 1; //ativo

                if (CLS_QuestionarioRespostaBO.Save(Resposta))
                {
                    if (_VS_qtr_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "qtc_id: " + Resposta.qtc_id + "qtr_id: " + Resposta.qtr_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Resposta incluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtc_id: " + Resposta.qtc_id + "qtr_id: " + Resposta.qtr_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Resposta alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }

                    Session["qtc_id"] = _VS_qtc_id;
                    Session["qst_id"] = _VS_qst_id;
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/Questionario/BuscaResposta.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a resposta.", UtilBO.TipoMensagem.Erro);
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
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a resposta.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void _Carregar(int qtc_id, int qtr_id)
        {
            try
            {
                CLS_QuestionarioResposta Resposta = new CLS_QuestionarioResposta { qtc_id = qtc_id, qtr_id = qtr_id };
                
                CLS_QuestionarioRespostaBO.GetEntity(Resposta);
                
                _VS_qtr_id = Resposta.qtr_id;
                _VS_qtc_id = Resposta.qtc_id;
                _txtTexto.Text = Resposta.qtr_texto;
                divPeso.Visible = IsMultiplaSelecao;
                _txtPeso.Text = divPeso.Visible ? Resposta.qtr_peso.ToString() : "0";
                _chkPermiteAdicionarTexto.Checked = Resposta.qtr_permiteAdicionarTexto;
            }
            catch (Exception e)
            {
                ApplicationWEB._GravaErro(e);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a resposta.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}