using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.AreaConhecimento
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de aco_id (ID da área de conhecimento)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_aco_id
        {
            get
            {
                if (ViewState["VS_aco_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_aco_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_aco_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor da ordem area conhecimento
        /// </summary>
        private int VS_aco_ordem
        {
            get
            {
                if (ViewState["VS_aco_ordem"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_aco_ordem"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_aco_ordem"] = value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método para carregar os valores dos combos
        /// </summary>
        private void CarregarCombos()
        {
            ddlTipoBaseGeral.Items.Add(new ListItem("-- Selecione um tipo base geral --", "0"));
            ddlTipoBaseGeral.Items.Add(new ListItem("Resolução", ((byte)TipoBaseGeral.Resolucao).ToString()));
            ddlTipoBaseGeral.Items.Add(new ListItem("Decreto", ((byte)TipoBaseGeral.Decreto).ToString()));
            ddlTipoBaseGeral.SelectedIndex = 0;

            ddlTipoBase.Items.Add(new ListItem("-- Selecione um tipo base --", "0"));
            ddlTipoBase.Items.Add(new ListItem("Nacional comum", ((byte)TipoBase.Nacional).ToString()));
            ddlTipoBase.Items.Add(new ListItem("Parte diversificada", ((byte)TipoBase.Diversificada).ToString()));
            ddlTipoBase.SelectedIndex = 0;
        }

        /// <summary>
        /// Método para carregar um registro de área de conhecimento, a fim de atualizar suas informações.
        /// Recebe dados referente à área de conhecimento para realizar a busca.
        /// </summary>
        /// <param name="aco_id">ID da área de conhecimento</param>
        public void Carregar(int aco_id)
        {
            try
            {
                ACA_AreaConhecimento areaConhecimento = new ACA_AreaConhecimento { aco_id = aco_id };
                ACA_AreaConhecimentoBO.GetEntity(areaConhecimento);
                VS_aco_id = aco_id;
                VS_aco_ordem = areaConhecimento.aco_ordem;
                txtAreaConhecimento.Text = areaConhecimento.aco_nome;
                ddlTipoBaseGeral.SelectedValue = areaConhecimento.aco_tipoBaseGeral.ToString();
                ddlTipoBase.SelectedValue = areaConhecimento.aco_tipoBase.ToString();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar tipo de ciclo.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método para salvar um tipo de qualidade.
        /// </summary>
        private void Salvar()
        {
            try
            {
                ACA_AreaConhecimento areaConhecimento = new ACA_AreaConhecimento 
                    { 
                        aco_id = VS_aco_id,
                        aco_nome = txtAreaConhecimento.Text,
                        aco_tipoBaseGeral = Convert.ToByte(ddlTipoBaseGeral.SelectedValue),
                        aco_tipoBase = Convert.ToByte(ddlTipoBase.SelectedValue),
                        aco_ordem = VS_aco_ordem,
                        aco_situacao = Convert.ToByte(1),
                        IsNew = VS_aco_id <= 0
                    };

                if (ACA_AreaConhecimentoBO.Save(areaConhecimento))
                {
                    ApplicationWEB._GravaLogSistema(VS_aco_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "aco_id: " + areaConhecimento.aco_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Área de conhecimento " + (VS_aco_id > 0 ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

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
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar área de conhecimento.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos

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
                    CarregarCombos();

                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                        Carregar(PreviousPage.EditItem);

                    Page.Form.DefaultFocus = txtAreaConhecimento.ClientID;
                    Page.Form.DefaultButton = bntSalvar.UniqueID;

                    bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion Eventos
    }
}