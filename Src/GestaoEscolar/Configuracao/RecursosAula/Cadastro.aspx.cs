using System;
using System.Data;
using System.Web;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.RecursosAula
{
    public partial class Cadastro : MotherPageLogado
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    _Carregar(PreviousPage.EditItem);
                else
                {
                    _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }

                Page.Form.DefaultFocus = txtRecursoNome.ClientID;
                Page.Form.DefaultButton = _btnSalvar.UniqueID;
            }
        }

        #region PROPRIEDADES


        private int _VS_rsa_id
        {
            get
            {
                if (ViewState["_VS_rsa_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_rsa_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_rsa_id"] = value;
            }
        }

        #endregion

        #region METODOS

        /// <summary>
        /// Insere e altera um Tipo de Movimentação.
        /// </summary>
        private void _Salvar()
        {
            try
            {
                ACA_RecursosAula RecursoAula = new ACA_RecursosAula
                {
                    rsa_id = _VS_rsa_id
                    };

                ACA_RecursosAulaBO.GetEntity(RecursoAula);
                RecursoAula.rsa_nome = txtRecursoNome.Text;
                RecursoAula.rsa_situacao = 1; //ativo

                DataTable dtNomeRepetido = ACA_RecursosAulaBO.GetRecursoAulaBy_rsa_nome(txtRecursoNome.Text.Trim());
                if (dtNomeRepetido.Rows.Count > 0)
                {
                    if(Convert.ToInt32(dtNomeRepetido.Rows[0]["rsa_id"])!= RecursoAula.rsa_id)
                    {
                        throw new ValidationException("Existe um recurso de aula cadastrado com esse nome!");
                    }
                }

                if (ACA_RecursosAulaBO.Save(RecursoAula))
                {
                    if (_VS_rsa_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "rsa_id: " + RecursoAula.rsa_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Recurso de aula incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "rsa_id: " + RecursoAula.rsa_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Recurso de aula alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }

                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/RecursosAula/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o recurso de aula.", UtilBO.TipoMensagem.Erro);
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
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o recurso de aula.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os dados do Tipo de Movimentação nos controles caso seja alteração.
        /// </summary>
        /// <param name="rsa_id">ID do recurso</param>
        private void _Carregar(int rsa_id)
        {
            try
            {
                ACA_RecursosAula RecursoAula= new ACA_RecursosAula { rsa_id = rsa_id };
                ACA_RecursosAulaBO.GetEntity(RecursoAula);
                _VS_rsa_id = RecursoAula.rsa_id;
                txtRecursoNome.Text = RecursoAula.rsa_nome;
             }
            catch (Exception e)
            {
                ApplicationWEB._GravaErro(e);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o recurso de aula.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region EVENTOS

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _btnSalvar_Click(object sender, EventArgs e)
        {
            _Salvar();
        }

        #endregion
    }
}
