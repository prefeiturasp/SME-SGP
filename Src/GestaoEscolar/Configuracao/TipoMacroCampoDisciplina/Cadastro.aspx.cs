using System;
using System.Web;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Data;

namespace GestaoEscolar.Configuracao.TipoMacroCampoDisciplina
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Armazena o ID do tipo de macro-campo
        /// </summary>
        private int VS_tea_id
        {
            get
            {
                if (ViewState["VS_tea_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tea_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_tea_id"] = value;
            }
        }

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    Carregar(PreviousPage.EditItem);
                else
                    btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

                Page.Form.DefaultButton = btnSalvar.UniqueID;
                Page.Form.DefaultFocus = txtTipoMacroCampo.ClientID;

                rfvTipoMacroCampo.ErrorMessage = "Tipo de macro-campo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " eletivo(a) é obrigatório.";

            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega dados do tipo de macro-campo disciplina eletiva.
        /// </summary>
        /// <param name="tea_id">Id do tipo de macro-campo disciplina eletiva</param>
        private void Carregar(int tea_id)
        {
            try
            {
                ACA_TipoMacroCampoEletivaAluno TipoMacroCampo = new ACA_TipoMacroCampoEletivaAluno { tea_id = tea_id };
                ACA_TipoMacroCampoEletivaAlunoBO.GetEntity(TipoMacroCampo);
                VS_tea_id = TipoMacroCampo.tea_id;
                txtTipoMacroCampo.Text = TipoMacroCampo.tea_nome;
                txtSigla.Text = TipoMacroCampo.tea_sigla;
               
            }
            catch (Exception e)
            {
                ApplicationWEB._GravaErro(e);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de macro-campo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " eletivo(a).", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Insere e altera o tipo de macro-campo disciplina eletiva.
        /// </summary>
        public void Salvar()
        {
            try
            {
                ACA_TipoMacroCampoEletivaAluno TipoMacroCampo = new ACA_TipoMacroCampoEletivaAluno
                {
                    tea_id = VS_tea_id
                    ,
                    tea_nome = txtTipoMacroCampo.Text
                    ,
                    tea_sigla = txtSigla.Text
                    ,
                    IsNew = (VS_tea_id > 0) ? false : true
                };

                if (ACA_TipoMacroCampoEletivaAlunoBO.Save(TipoMacroCampo))
                {
                    if (VS_tea_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tea_id: " + TipoMacroCampo.tea_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de macro-campo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + 
                                                                          " eletivo(a) incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tea_id: " + TipoMacroCampo.tea_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de macro-campo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + 
                                                                          " eletivo(a) alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }

                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoMacroCampoDisciplina/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de macro-campo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") +
                                                            " eletivo(a).", UtilBO.TipoMensagem.Erro);
                }
            }
            catch (MSTech.Validation.Exceptions.ValidationException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception e)
            {
                ApplicationWEB._GravaErro(e);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de macro-campo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + 
                                                        " eletivo(a).", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoMacroCampoDisciplina/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        #endregion
    }
}
