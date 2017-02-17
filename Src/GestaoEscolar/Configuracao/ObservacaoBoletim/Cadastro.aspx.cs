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

namespace GestaoEscolar.Configuracao.ObservacaoBoletim
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de obb_id (ID da observacao do boletim)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_obb_id
        {
            get
            {
                if (ViewState["VS_obb_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_obb_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_obb_id"] = value;
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
                        VS_obb_id = PreviousPage.Edit_obb_id;
                        Carregar(PreviousPage.Edit_obb_id);
                    }

                    Page.Form.DefaultFocus = UCComboObservacaoBoletim.ClientID;
                    Page.Form.DefaultButton = btnSalvar.UniqueID;

                    btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
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
        /// Método para carregar um registro de observacao do boletim, a fim de atualizar suas informações.
        /// Recebe dados referente aobservacao do boletim para realizar busca.
        /// </summary>
        /// <param name="obb_id">ID da observacao do boletim</param>
        public void Carregar(int obb_id)
        {
            try
            {
                // Armazena valor ID do informativo a ser alterada.
                VS_obb_id = obb_id;

                // Busca do informativo baseado no ID do informativo.
                CFG_ObservacaoBoletim entObservacao = new CFG_ObservacaoBoletim { obb_id = obb_id,
                                                                                  ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id };
                CFG_ObservacaoBoletimBO.GetEntity(entObservacao);

                // Tras os campos preenchidos
                // Tipo
                UCComboObservacaoBoletim.Valor = entObservacao.obb_tipoObservacao;
                // Nome
                txtNome.Text = entObservacao.obb_nome;
                // Descricao
                txtDescricao.Text = entObservacao.obb_descricao;

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a observação do boletim.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método para salvar um informativo.
        /// </summary>
        private void Salvar()
        {
            try
            {
                CFG_ObservacaoBoletim entObservacao = new CFG_ObservacaoBoletim();

                entObservacao.ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                entObservacao.obb_id = VS_obb_id > 0 ? VS_obb_id : 0;
                entObservacao.obb_tipoObservacao = UCComboObservacaoBoletim.Valor;
                entObservacao.obb_nome = txtNome.Text;
                entObservacao.obb_descricao = txtDescricao.Text;
                entObservacao.IsNew = VS_obb_id < 0;

                if (CFG_ObservacaoBoletimBO.Save(entObservacao))
                {
                    ApplicationWEB._GravaLogSistema(VS_obb_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "obb_id: " + entObservacao.obb_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Observação do boletim " + (VS_obb_id > 0 ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    Response.Redirect("~/Configuracao/ObservacaoBoletim/Busca.aspx", false);
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
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a observação do bolemtim.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/ObservacaoBoletim/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/ObservacaoBoletim/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion Eventos
    }
}