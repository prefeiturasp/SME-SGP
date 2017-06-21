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
using System.Data;

namespace GestaoEscolar.Configuracao.DeficienciaDetalhe
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tde_id (ID do tipo de deficiencia, no Core)
        /// </summary>
        private Guid VS_tde_id
        {
            get
            {
                if (ViewState["VS_tde_id"] != null)
                {
                    return new Guid(Convert.ToString(ViewState["VS_tde_id"]));
                }

                return Guid.NewGuid();
            }

            set
            {
                ViewState["VS_tde_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tde_id (ID do tipo de deficiencia, no Core)
        /// </summary>
        private int VS_dfd_id
        {
            get
            {
                if (ViewState["VS_dfd_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_dfd_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_dfd_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tde_idFilha, Id da deficiencia relacionada (ID do tipo de deficiencia, no Core)
        /// </summary>
        private Guid VS_tde_idFilha
        {
            get
            {
                if (ViewState["VS_tde_idFilha"] != null)
                {
                    return new Guid(Convert.ToString(ViewState["VS_tde_idFilha"]));
                }

                return Guid.NewGuid();
            }

            set
            {
                ViewState["VS_tde_idFilha"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena a lista de detalhes
        /// </summary>
        private List<CFG_DeficienciaDetalhe> VS_ListaDetalhe
        {
            get
            {
                if (ViewState["VS_ListaDetalhe"] == null)
                    ViewState["VS_ListaDetalhe"] = new List<CFG_DeficienciaDetalhe>();
                return (List<CFG_DeficienciaDetalhe>)ViewState["VS_ListaDetalhe"];
            }
            set
            {
                ViewState["VS_ListaDetalhe"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena a lista de filhas
        /// </summary>
        private List<CFG_DeficienciaFIlha> VS_ListaFilha
        {
            get
            {
                if (ViewState["VS_ListaFilha"] == null)
                    ViewState["VS_ListaFilha"] = new List<CFG_DeficienciaFIlha>();
                return (List<CFG_DeficienciaFIlha>)ViewState["VS_ListaFilha"];
            }
            set
            {
                ViewState["VS_ListaFilha"] = value;
            }
        }

        #endregion

        #region Métodos


        /// <summary>
        /// Método para carregar um registro de área de conhecimento, a fim de atualizar suas informações.
        /// Recebe dados referente à área de conhecimento para realizar a busca.
        /// </summary>
        /// <param name="tde_id">ID do tipo de deficiencia</param>
        public void Carregar(Guid tde_id)
        {
            try
            {

                //verifica se é deficiencia multipla
                if (tde_id == ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.DEFICIENCIA_MULTIPLA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    divDetalhe.Visible = false;
                    divFilha.Visible = true;

                    UCComboTipoDeficienciaFilha._Label.Text = string.Format("Deficiência relacionada");

                    List<CFG_DeficienciaFIlha> lstAux = CFG_DeficienciaFIlhaBO.SelectFilhaBy_Deficiencia(tde_id);
                    VS_ListaFilha = lstAux.ToList();

                    UCComboTipoDeficiencia._Combo.Enabled = !(VS_ListaFilha.Count > 0);

                    gdvDeficienciaFilha.DataSource = VS_ListaFilha;
                    gdvDeficienciaFilha.DataBind();
                }
                //não é multipla e vai inserir o detalhamento 
                else
                {
                    divDetalhe.Visible = true;
                    divFilha.Visible = false;

                    List<CFG_DeficienciaDetalhe> lstAux = CFG_DeficienciaDetalheBO.SelectDetalheBy_Deficiencia(tde_id);
                    VS_ListaDetalhe = lstAux.ToList();

                    UCComboTipoDeficiencia._Combo.Enabled = !(VS_ListaDetalhe.Count > 0);
                    
                    grvDetalhes.DataSource = VS_ListaDetalhe.Where(q => q.dfd_situacao != 3);
                    grvDetalhes.DataBind();
                }

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar detalhamento.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método para salvar um tipo de qualidade.
        /// </summary>
        private void Salvar()
        {
            try
            {
                VS_tde_id = new Guid(UCComboTipoDeficiencia._Combo.SelectedValue);
                //verifica se é deficência multipla
                if (VS_tde_id == ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.DEFICIENCIA_MULTIPLA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    //passar a lista para salvar as deficiencias relacionadas
                    if (CFG_DeficienciaFIlhaBO.Salvar(VS_tde_id, VS_ListaFilha))
                    {
                        ApplicationWEB._GravaLogSistema(VS_tde_id != Guid.NewGuid() ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "tde_id: " + VS_tde_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Detalhamento de deficiência " + (VS_tde_id != Guid.NewGuid() ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                        Response.Redirect("Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }

                else
                {
                    //passar a lista para salvar o detalhamento
                    if (CFG_DeficienciaDetalheBO.Salvar(VS_tde_id, VS_ListaDetalhe))
                    {
                        ApplicationWEB._GravaLogSistema(VS_tde_id != Guid.NewGuid() ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "tde_id: " + VS_tde_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Detalhamento de deficiência " + (VS_tde_id != Guid.NewGuid() ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                        Response.Redirect("Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
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
                    UCComboTipoDeficiencia._Combo.DataBind();

                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_tde_id = PreviousPage.EditItem;
                        UCComboTipoDeficiencia._Combo.SelectedValue = VS_tde_id.ToString();
                        Carregar(VS_tde_id);
                    }

                    Page.Form.DefaultFocus = UCComboTipoDeficiencia.ClientID;
                    Page.Form.DefaultButton = bntSalvar.UniqueID;

                    bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }

            UCComboTipoDeficiencia._IndexChanged += UCComboTipoDeficiencia__IndexChanged;

        }

        private void UCComboTipoDeficiencia__IndexChanged()
        {
            if (UCComboTipoDeficiencia._Combo.SelectedIndex > 0)
            {

                VS_tde_id = new Guid(UCComboTipoDeficiencia._Combo.SelectedValue);
                Carregar(VS_tde_id);
            }
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        //adicionar detalhes
        protected void btnAdicionarDetalhe_Click(object sender, EventArgs e)
        {
            try
            {
                string mensagem = "";

                if (string.IsNullOrEmpty(txtItemDetalhe.Text))
                    mensagem += (string.IsNullOrEmpty(mensagem) ? "" : "<br/>") + string.Format("Descrição do detalhe é obrigatório.");

                if (!string.IsNullOrEmpty(mensagem))
                    throw new ValidationException(mensagem.Replace(" *", ""));

                if (VS_ListaDetalhe.Any(p => p.dfd_nome.Equals(txtItemDetalhe.Text)))
                    throw new ValidationException(string.Format("Detalhe já existe."));

                if (VS_dfd_id > 0 && VS_ListaDetalhe.Any(l => l.dfd_id == VS_dfd_id))
                    VS_ListaDetalhe[VS_ListaDetalhe.IndexOf(VS_ListaDetalhe.Where(l => l.dfd_id == VS_dfd_id).First())].dfd_nome = txtItemDetalhe.Text;
                else
                {
                    int dfd_id = VS_ListaDetalhe.Any() ? VS_ListaDetalhe.Max(l => l.dfd_id) + 1 : 1;
                    if (txtItemDetalhe.Text.Length > 100)
                        throw new ValidationException("O nome do detalhe não deve exceder 100 caracteres.");
                    VS_ListaDetalhe.Add(new CFG_DeficienciaDetalhe
                    {
                        tde_id = VS_tde_id,
                        dfd_id = VS_dfd_id,
                        dfd_nome = txtItemDetalhe.Text,
                        IsNew = true
                    });
                }

                VS_ListaDetalhe = VS_ListaDetalhe.OrderBy(q => q.dfd_nome).ToList();

                grvDetalhes.DataSource = VS_ListaDetalhe.Where(q => q.dfd_situacao != 3);
                grvDetalhes.DataBind();

                txtItemDetalhe.Text = "";
                divInserirDetalhe.Visible = false;
                updCadastroQualidade.Update();
            }
            catch (ValidationException ex)
            {
                lblMessagePopUpDetalhe.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessagePopUpDetalhe.Text = UtilBO.GetErroMessage("Erro ao adicionar detalhamento de deficiência.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvDetalhes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int idExcluir = Convert.ToInt32(grvDetalhes.DataKeys[index]["dfd_id"]);

                    if (idExcluir > 0 && VS_ListaDetalhe.Any(l => l.dfd_id == idExcluir))
                    {
                        int ind = VS_ListaDetalhe.IndexOf(VS_ListaDetalhe.Where(l => l.dfd_id == idExcluir).First());

                        VS_ListaDetalhe.RemoveAt(ind);
                    }
                    VS_ListaDetalhe = VS_ListaDetalhe.OrderBy(q => q.dfd_nome).ToList();

                    grvDetalhes.DataSource = VS_ListaDetalhe.Where(q => q.dfd_situacao != 3);
                    grvDetalhes.DataBind();
                    updCadastroQualidade.Update();

                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
        }


        protected void btnNovoDetalhe_Click(object sender, EventArgs e)
        {
            try
            {
                VS_dfd_id = 0;
                txtItemDetalhe.Text = "";
                txtItemDetalhe.Focus();
                updPopUpDetalhe.Update();
                divInserirDetalhe.Visible = true;
                btnAdicionarDetalhe.Text = "Adicionar detalhe";
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao adicionar detalhe.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelarItemDetalhe_Click(object sender, EventArgs e)
        {
            divInserirDetalhe.Visible = false;
            updCadastroQualidade.Update();
        }


        protected void grv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                }
            }
        }

        //adicionar deficiencias filhas

        protected void btnAdicionarFilha_Click(object sender, EventArgs e)
        {
            try
            {
                string mensagem = "";

                if (UCComboTipoDeficienciaFilha._Combo.SelectedIndex <= 0)
                    mensagem += (string.IsNullOrEmpty(mensagem) ? "" : "<br/>") + string.Format("Deficiência relacionada é obrigatória.");

                if (!string.IsNullOrEmpty(mensagem))
                    throw new ValidationException(mensagem.Replace(" *", ""));

                if (VS_ListaFilha.Any(p => p.tde_idFilha.Equals(new Guid(UCComboTipoDeficienciaFilha._Combo.SelectedValue))))
                    throw new ValidationException(string.Format("Deficiência relacionada já existe."));

                VS_ListaFilha.Add(new CFG_DeficienciaFIlha
                {
                    tde_id = VS_tde_id,
                    tde_idFilha = new Guid(UCComboTipoDeficienciaFilha._Combo.SelectedValue),
                    tde_nomeFilha = UCComboTipoDeficienciaFilha._Combo.SelectedItem.Text,
                    IsNew = true
                });

                VS_ListaFilha = VS_ListaFilha.OrderBy(q => q.tde_nomeFilha).ToList();

                gdvDeficienciaFilha.DataSource = VS_ListaFilha;
                gdvDeficienciaFilha.DataBind();

                UCComboTipoDeficienciaFilha._Combo.SelectedIndex = 0;
                divInserirFilha.Visible = false;
                updCadastroQualidade.Update();
            }
            catch (ValidationException ex)
            {
                lblPopupFilha.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblPopupFilha.Text = UtilBO.GetErroMessage("Erro ao adicionar detalhamento de deficiência.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvFilha_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    Guid idExcluir = new Guid(gdvDeficienciaFilha.DataKeys[index]["tde_idFilha"].ToString());

                    if (idExcluir != Guid.NewGuid() && VS_ListaFilha.Any(l => l.tde_idFilha == idExcluir))
                    {
                        int ind = VS_ListaFilha.IndexOf(VS_ListaFilha.Where(l => l.tde_idFilha == idExcluir).First());

                        VS_ListaFilha.RemoveAt(ind);
                    }
                    VS_ListaFilha = VS_ListaFilha.OrderBy(q => q.tde_nomeFilha).ToList();

                    gdvDeficienciaFilha.DataSource = VS_ListaFilha;
                    gdvDeficienciaFilha.DataBind();
                    updCadastroQualidade.Update();

                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnNovaFilha_Click(object sender, EventArgs e)
        {
            try
            {
                VS_tde_idFilha = Guid.NewGuid();
                UCComboTipoDeficienciaFilha._Combo.SelectedIndex = 0;
                UCComboTipoDeficienciaFilha.Focus();

                divInserirFilha.Visible = true;
                updPopUpFilha.Update();
                btnFilha.Text = "Adicionar deficiência relacionada";

                lblDefRelacionada.Text += divInserirFilha.Visible.ToString();
                // updCadastroQualidade.Update();
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao adicionar deficiência relacionada.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelarItemFilha_Click(object sender, EventArgs e)
        {
            divInserirFilha.Visible = false;
        }


        protected void grvFilha_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluirFilha = (ImageButton)e.Row.FindControl("btnExcluirFilha");
                if (btnExcluirFilha != null)
                {
                    btnExcluirFilha.CommandArgument = e.Row.RowIndex.ToString();
                    btnExcluirFilha.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                }
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