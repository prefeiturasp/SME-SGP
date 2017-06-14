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
    public partial class CadastroConteudo : MotherPageLogado
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    //_Carregar(PreviousPage.Edit_qst_id, PreviousPage.Edit_qtc_id);
                    _VS_qst_id = PreviousPage.Edit_qst_id;
                }

                else
                {
                    //_btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }

                Page.Form.DefaultFocus = _txtTexto.ClientID;
                Page.Form.DefaultButton = _btnSalvar.UniqueID;
            }
        }

        /// <summary>
        /// Carrega os dados do Questionário nos controles caso seja alteração.
        /// </summary>
        /// <param name="qst_id">ID do recurso</param>
        private void _Carregar(int qst_id, int qtc_id)
        {
            try
            {
                CLS_QuestionarioConteudo Conteudo = new CLS_QuestionarioConteudo { qst_id = qst_id, qtc_id = qtc_id };
                CLS_QuestionarioConteudoBO.GetEntity(Conteudo);
                _VS_qst_id = Conteudo.qst_id;
                _VS_qtc_id = Conteudo.qtc_id;
                _txtTexto.Text = Conteudo.qtc_texto;
                _ddlTipoConteudo.SelectedValue = Conteudo.qtc_tipo.ToString();
                _ddlTipoResposta.SelectedValue = Conteudo.qtc_tipoResposta > 0 ? Conteudo.qtc_tipoResposta.ToString() : "0";
            }
            catch (Exception e)
            {
                ApplicationWEB._GravaErro(e);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o conteúdo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("BuscaConteudo.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _btnSalvar_Click(object sender, EventArgs e)
        {
            _Salvar();
        }

        /// <summary>
        /// Insere e altera um Questionário.
        /// </summary>
        private void _Salvar()
        {
            try
            {
                CLS_QuestionarioConteudo Conteudo = new CLS_QuestionarioConteudo
                {
                    qst_id = _VS_qst_id
                    ,
                    qtc_id = _VS_qtc_id
                    ,
                    IsNew = _VS_qst_id > 0 && _VS_qtc_id > 0
                };

                CLS_QuestionarioConteudoBO.GetEntity(Conteudo);
                Conteudo.qtc_texto = _txtTexto.Text;
                Conteudo.qtc_tipo = Convert.ToByte(_ddlTipoConteudo.SelectedValue.ToString());
                Conteudo.qtc_tipoResposta = Convert.ToByte(_ddlTipoResposta.SelectedValue.ToString());
                Conteudo.qtc_situacao = 1; //ativo

                if (CLS_QuestionarioConteudoBO.Save(Conteudo))
                {
                    if (_VS_qtc_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "qtc_id: " + Conteudo.qtc_id + "qst_id: " + Conteudo.qst_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Conteúdo incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtc_id: " + Conteudo.qtc_id + "qst_id: " + Conteudo.qst_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Conteúdo alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }

                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/Questionario/BuscaConteudo.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o conteúdo.", UtilBO.TipoMensagem.Erro);
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
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o conteúdo.", UtilBO.TipoMensagem.Erro);
            }
        }
    }
}