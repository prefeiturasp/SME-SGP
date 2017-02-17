using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.Recomendacoes
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de rar_id (ID do tipo de recomendacao)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_rar_id
        {
            get
            {
                if (ViewState["VS_rar_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_rar_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_rar_id"] = value;
            }
        }

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            if (!IsPostBack)
            {
                try
                {

                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_rar_id = PreviousPage.Edit_rar_id;
                        Carregar(PreviousPage.Edit_rar_id);
                    }

                    Page.Form.DefaultFocus = txtDescricao.ClientID;
                    Page.Form.DefaultButton = btnIncluirCadastroRecomendacao.UniqueID;

                    btnIncluirCadastroRecomendacao.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion Eventos Page Life Cycle

        #region Métodos

        /// <summary>
        /// Método para carregar um registro de recomendacao, a fim de atualizar suas informações.
        /// Recebe dados referente a recomendacao para realizar busca.
        /// </summary>
        /// <param name="rar_id">ID da recomendacao</param>
        public void Carregar(int rar_id)
        {
            try
            {
                // Armazena valor ID da recomendacao a ser alterada.
                VS_rar_id = rar_id;

                // Busca da recomendacao baseado no ID da recomendacao.
                ACA_RecomendacaoAlunoResponsavel entRecomendacao = new ACA_RecomendacaoAlunoResponsavel { rar_id = rar_id };
                ACA_RecomendacaoAlunoResponsavelBO.GetEntity(entRecomendacao);

                // Descricao
                txtDescricao.Text = entRecomendacao.rar_descricao;
                rblDestino.SelectedValue = entRecomendacao.rar_tipo.ToString();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a recomendação.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método para salvar uma recomendação.
        /// </summary>
        private void Salvar()
        {
            try
            {
                ACA_RecomendacaoAlunoResponsavel entRecomendacao = new ACA_RecomendacaoAlunoResponsavel();

                entRecomendacao.rar_id = VS_rar_id;
                entRecomendacao.rar_descricao = txtDescricao.Text;
                entRecomendacao.rar_tipo = Convert.ToInt16(rblDestino.SelectedValue);
                entRecomendacao.IsNew = VS_rar_id < 0;

                if (ACA_RecomendacaoAlunoResponsavelBO.Save(entRecomendacao))
                {
                    ApplicationWEB._GravaLogSistema(VS_rar_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "rar_id: " + entRecomendacao.rar_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Recomendação " + (VS_rar_id > 0 ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    Response.Redirect("Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (ValidationException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a recomendação.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos

        protected void btnIncluirCadastroRecomendacao_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        protected void btnCancelarCadastroRecomendacao_Click(object sender, EventArgs e)
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion Eventos
    }
}