using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.ObjetoAprendizagem
{
    public partial class CadastroEixo : MotherPageLogado
    {
        #region PROPRIEDADES

        /// <summary>
        /// Propriedade em ViewState que armazena a lista de eixos
        /// </summary>
        private List<ACA_ObjetoAprendizagemEixo> VS_ListaEixo
        {
            get
            {
                if (ViewState["VS_ListaEixo"] == null)
                    ViewState["VS_ListaEixo"] = new List<ACA_ObjetoAprendizagemEixo>();
                return (List<ACA_ObjetoAprendizagemEixo>)ViewState["VS_ListaEixo"];
            }
            set
            {
                ViewState["VS_ListaEixo"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena a lista de objetos de conhecimento
        /// </summary>
        private List<ACA_ObjetoAprendizagem> VS_ListaObjetos
        {
            get
            {
                if (ViewState["VS_ListaObjetos"] == null)
                    ViewState["VS_ListaObjetos"] = new List<ACA_ObjetoAprendizagem>();
                return (List<ACA_ObjetoAprendizagem>)ViewState["VS_ListaObjetos"];
            }
            set
            {
                ViewState["VS_ListaObjetos"] = value;
            }
        }

        public int oap_id
        {
            get
            {
                if (_grvObjetoAprendizagem.EditIndex >= 0)
                    return Convert.ToInt32(_grvObjetoAprendizagem.DataKeys[_grvObjetoAprendizagem.EditIndex].Value);

                return -1;
            }
        }

        public int oae_id
        {
            get
            {
                return _VS_oae_id;
            }
        }

        public int oae_idPai
        {
            get
            {
                return _VS_oae_idPai;
            }
        }

        public int tds_id
        {
            get
            {
                return _VS_tds_id;
            }
        }

        public int cal_ano
        {
            get
            {
                return _VS_cal_ano;
            }
        }

        private int _VS_tds_id
        {
            get
            {
                if (ViewState["_VS_tds_id"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_tds_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["_VS_tds_id"] = value;
            }
        }

        private int _VS_cal_ano
        {
            get
            {
                if (ViewState["_VS_cal_ano"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_cal_ano"]);
                }
                return -1;
            }
            set
            {
                ViewState["_VS_cal_ano"] = value;
            }
        }

        private int _VS_oae_idPai
        {
            get
            {
                if (ViewState["_VS_oae_idPai"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_oae_idPai"]);
                }
                return -1;
            }
            set
            {
                ViewState["_VS_oae_idPai"] = value;
            }
        }

        private int _VS_oae_id
        {
            get
            {
                if (ViewState["_VS_oae_id"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_oae_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["_VS_oae_id"] = value;
            }
        }

        #endregion

        #region EVENTOS

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroEixoObjConhecimento.js"));
                }

                string message = __SessionWEB.PostMessages;
                if (!string.IsNullOrEmpty(message))
                    _lblMessage.Text = message;

                if (!IsPostBack)
                {
                    btnNovoObjeto.Visible = _btnNovoSub.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

                    if (Session["tds_id_oae"] != null && Session["cal_ano_oae"] != null && Session["oae_id"] != null)
                    {
                        LoadPage(Convert.ToInt32(Session["tds_id_oae"]), Convert.ToInt32(Session["cal_ano_oae"]), Convert.ToInt32(Session["oae_id"]), Session["oae_idPai"] != null ? Convert.ToInt32(Session["oae_idPai"]) : -1);
                    }
                    else
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Selecione um eixo de objeto de conhecimento para edição.", UtilBO.TipoMensagem.Alerta);
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ObjetoAprendizagem/BuscaDisciplina.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void _grvEixoObjetoAprendizagem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Alterar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int id = Convert.ToInt32(_grvEixoObjetoAprendizagem.DataKeys[index]["oae_id"]);

                    Session["tds_id_oae"] = _VS_tds_id;
                    Session["cal_ano_oae"] = _VS_cal_ano;
                    Session["oae_id"] = id;
                    Session["oae_idPai"] = _VS_oae_id;

                    Response.Redirect("~/Academico/ObjetoAprendizagem/CadastroEixo.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar alterar o eixo de objeto de conhecimento.", UtilBO.TipoMensagem.Erro);
                }
            }
            if (e.CommandName == "Subir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idDescer = Convert.ToInt32(_grvEixoObjetoAprendizagem.DataKeys[index - 1]["oae_id"]);
                    int idSubir = Convert.ToInt32(_grvEixoObjetoAprendizagem.DataKeys[index]["oae_id"]);
                    int ordemSubir = VS_ListaEixo[VS_ListaEixo.IndexOf(VS_ListaEixo.Where(l => l.oae_id == idSubir).First())].oae_ordem;
                    int ordemDescer = VS_ListaEixo[VS_ListaEixo.IndexOf(VS_ListaEixo.Where(l => l.oae_id == idDescer).First())].oae_ordem;

                    ACA_ObjetoAprendizagemEixo entitySubir = new ACA_ObjetoAprendizagemEixo { oae_id = idSubir };
                    ACA_ObjetoAprendizagemEixoBO.GetEntity(entitySubir);
                    entitySubir.oae_ordem = ordemDescer;
                    ACA_ObjetoAprendizagemEixo entityDescer = new ACA_ObjetoAprendizagemEixo { oae_id = idDescer };
                    ACA_ObjetoAprendizagemEixoBO.GetEntity(entityDescer);
                    entityDescer.oae_ordem = ordemSubir;

                    if (!ACA_ObjetoAprendizagemEixoBO.SalvarOrdem(entitySubir, entityDescer))
                        throw new ValidationException("Erro ao tentar alterar a ordem dos eixos de objeto de conhecimento.");
                    else
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "idDescer: " + idDescer + " idSubir: " + idSubir);

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage("Ordem dos eixos de objeto de conhecimento alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    VS_ListaEixo[VS_ListaEixo.IndexOf(VS_ListaEixo.Where(l => l.oae_id == idSubir).First())].oae_ordem = ordemDescer;
                    VS_ListaEixo[VS_ListaEixo.IndexOf(VS_ListaEixo.Where(l => l.oae_id == idDescer).First())].oae_ordem = ordemSubir;

                    VS_ListaEixo = VS_ListaEixo.OrderBy(q => q.oae_ordem).ThenBy(q => q.oae_descricao).ToList();

                    _grvEixoObjetoAprendizagem.DataSource = VS_ListaEixo;
                    _grvEixoObjetoAprendizagem.DataBind();

                    if (_grvEixoObjetoAprendizagem.Rows.Count > 0)
                    {
                        ((ImageButton)_grvEixoObjetoAprendizagem.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_grvEixoObjetoAprendizagem.Rows[_grvEixoObjetoAprendizagem.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao subir o eixo de objeto de conhecimento.", UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Descer")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idDescer = Convert.ToInt32(_grvEixoObjetoAprendizagem.DataKeys[index]["oae_id"]);
                    int idSubir = Convert.ToInt32(_grvEixoObjetoAprendizagem.DataKeys[index + 1]["oae_id"]);
                    int ordemSubir = VS_ListaEixo[VS_ListaEixo.IndexOf(VS_ListaEixo.Where(l => l.oae_id == idSubir).First())].oae_ordem;
                    int ordemDescer = VS_ListaEixo[VS_ListaEixo.IndexOf(VS_ListaEixo.Where(l => l.oae_id == idDescer).First())].oae_ordem;

                    ACA_ObjetoAprendizagemEixo entitySubir = new ACA_ObjetoAprendizagemEixo { oae_id = idSubir };
                    ACA_ObjetoAprendizagemEixoBO.GetEntity(entitySubir);
                    entitySubir.oae_ordem = ordemDescer;
                    ACA_ObjetoAprendizagemEixo entityDescer = new ACA_ObjetoAprendizagemEixo { oae_id = idDescer };
                    ACA_ObjetoAprendizagemEixoBO.GetEntity(entityDescer);
                    entityDescer.oae_ordem = ordemSubir;

                    if (!ACA_ObjetoAprendizagemEixoBO.SalvarOrdem(entitySubir, entityDescer))
                        throw new ValidationException("Erro ao tentar alterar a ordem dos eixos de objeto de conhecimento.");
                    else
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "idDescer: " + idDescer + " idSubir: " + idSubir);

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage("Ordem dos eixos de objeto de conhecimento alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    VS_ListaEixo[VS_ListaEixo.IndexOf(VS_ListaEixo.Where(l => l.oae_id == idSubir).First())].oae_ordem = ordemDescer;
                    VS_ListaEixo[VS_ListaEixo.IndexOf(VS_ListaEixo.Where(l => l.oae_id == idDescer).First())].oae_ordem = ordemSubir;

                    VS_ListaEixo = VS_ListaEixo.OrderBy(q => q.oae_ordem).ThenBy(q => q.oae_descricao).ToList();

                    _grvEixoObjetoAprendizagem.DataSource = VS_ListaEixo;
                    _grvEixoObjetoAprendizagem.DataBind();

                    if (_grvEixoObjetoAprendizagem.Rows.Count > 0)
                    {
                        ((ImageButton)_grvEixoObjetoAprendizagem.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_grvEixoObjetoAprendizagem.Rows[_grvEixoObjetoAprendizagem.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao descer o eixo de objeto de conhecimento.", UtilBO.TipoMensagem.Erro);
                }
            }
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int id = Convert.ToInt32(_grvEixoObjetoAprendizagem.DataKeys[index]["oae_id"]);

                    ACA_ObjetoAprendizagemEixo entity = new ACA_ObjetoAprendizagemEixo { oae_id = id };

                    if (ACA_ObjetoAprendizagemEixoBO.Excluir(entity))
                    {
                        VS_ListaEixo.RemoveAt(index);
                        VS_ListaEixo = VS_ListaEixo.OrderBy(q => q.oae_ordem).ThenBy(q => q.oae_descricao).ToList();

                        _grvEixoObjetoAprendizagem.DataSource = VS_ListaEixo;
                        _grvEixoObjetoAprendizagem.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "oae_id: " + id);
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                        _lblMessage.Text = UtilBO.GetErroMessage("Eixo de objeto de conhecimento excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir o eixo de objeto de conhecimento.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void _grvEixoObjetoAprendizagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton _btnSelecionar = (LinkButton)e.Row.FindControl("_btnSelecionar");
                if (_btnSelecionar != null)
                {
                    _btnSelecionar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                    _btnSelecionar.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
                if (_btnExcluir != null)
                {
                    _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton _btnSubir = (ImageButton)e.Row.FindControl("_btnSubir");
                if (_btnSubir != null)
                {
                    _btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                    _btnSubir.CommandArgument = e.Row.RowIndex.ToString();
                    _btnSubir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                ImageButton _btnDescer = (ImageButton)e.Row.FindControl("_btnDescer");
                if (_btnDescer != null)
                {
                    _btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                    _btnDescer.CommandArgument = e.Row.RowIndex.ToString();
                    _btnDescer.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void _grvEixoObjetoAprendizagem_DataBound(object sender, EventArgs e)
        {
            GridView grv = (GridView)sender;
            if (grv.Rows.Count > 0)
            {
                ((ImageButton)grv.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                ((ImageButton)grv.Rows[grv.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
            }
        }

        protected void _grvObjetoAprendizagem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int id = Convert.ToInt32(_grvObjetoAprendizagem.DataKeys[index].Value);

                    ACA_ObjetoAprendizagem entity = new ACA_ObjetoAprendizagem { oap_id = id };
                    
                    if (ACA_ObjetoAprendizagemBO.Excluir(entity))
                    {
                        VS_ListaObjetos.RemoveAt(index);
                        VS_ListaObjetos = VS_ListaObjetos.OrderBy(q => q.oap_descricao).ToList();

                        _grvObjetoAprendizagem.DataSource = VS_ListaObjetos;
                        _grvObjetoAprendizagem.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "oap_id: " + id);
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                        _lblMessage.Text = UtilBO.GetErroMessage("Objeto de conhecimento excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o objeto de conhecimento.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void _grvObjetoAprendizagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
                if (_btnExcluir != null)
                {
                    _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void btnNovoObjeto_Click(object sender, EventArgs e)
        {
            Session["tds_id_oae"] = _VS_tds_id;
            Session["cal_ano_oae"] = _VS_cal_ano;
            Session["oae_id"] = _VS_oae_id;
            Session["oae_idPai"] = _VS_oae_idPai;

            Response.Redirect("~/Academico/ObjetoAprendizagem/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _btnNovoSub_Click(object sender, EventArgs e)
        {
            txtDescricao.Text = "";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "EditarAulas", "$('#divInserir').dialog('open');", true);
            updPopUp.Update();
        }

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            //Se está editando um subeixo então volta para a edição do eixo pai
            if (_VS_oae_idPai > 0)
            {
                Session["tds_id_oae"] = _VS_tds_id;
                Session["cal_ano_oae"] = _VS_cal_ano;
                Session["oae_id"] = _VS_oae_idPai;
                Session["oae_idPai"] = null;

                Response.Redirect("~/Academico/ObjetoAprendizagem/CadastroEixo.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            //Se está editando um eixo pai então volta para a busca de eixos
            else
            {
                Session["tds_id_oae"] = _VS_tds_id;
                Session["cal_ano_oae"] = _VS_cal_ano;
                Session["oae_id"] = null;
                Session["oae_idPai"] = null;

                Response.Redirect("~/Academico/ObjetoAprendizagem/BuscaEixo.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDescricao.Text))
                    throw new ValidationException("Descrição do subeixo de objeto de conhecimento é obrigatória.");

                ACA_ObjetoAprendizagemEixo oae = new ACA_ObjetoAprendizagemEixo
                {
                    tds_id = _VS_tds_id,
                    cal_ano = _VS_cal_ano,
                    oae_idPai = _VS_oae_id,
                    oae_ordem = VS_ListaEixo.Any() ? VS_ListaEixo.Max(p => p.oae_ordem) + 1 : 1,
                    oae_descricao = txtDescricao.Text
                };

                if (!ACA_ObjetoAprendizagemEixoBO.Salvar(oae))
                    throw new ValidationException("Erro ao tentar salvar subeixo de objeto de conhecimento.");

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "oae_id: " + oae.oae_id);

                VS_ListaEixo = ACA_ObjetoAprendizagemEixoBO.SelectByDiscAno(_VS_tds_id, cal_ano, _VS_oae_id);
                VS_ListaEixo = VS_ListaEixo.OrderBy(q => q.oae_ordem).ThenBy(q => q.oae_descricao).ToList();

                _grvEixoObjetoAprendizagem.DataSource = VS_ListaEixo;
                _grvEixoObjetoAprendizagem.DataBind();

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharPopUp", "$('#divInserir').dialog('close');", true);
            }
            catch (ValidationException ex)
            {
                lblMessagePopUp.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessagePopUp.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar o subeixo de objeto de conhecimento.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void _grvObjetoAprendizagem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            _grvObjetoAprendizagem.EditIndex = e.NewEditIndex;
        }

        protected void _grvEixoObjetoAprendizagem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            _grvEixoObjetoAprendizagem.EditIndex = e.NewEditIndex;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEixo.Text))
                    throw new ValidationException("Descrição do " + (_VS_oae_idPai > 0 ? "sub" : "") + "eixo de objeto de conhecimento é obrigatória.");

                ACA_ObjetoAprendizagemEixo oae = new ACA_ObjetoAprendizagemEixo { oae_id = _VS_oae_id };
                ACA_ObjetoAprendizagemEixoBO.GetEntity(oae);

                oae.oae_descricao = txtEixo.Text;

                if (!ACA_ObjetoAprendizagemEixoBO.Salvar(oae))
                    throw new ValidationException("Erro ao tentar salvar " + (_VS_oae_idPai > 0 ? "sub" : "") + "eixo de objeto de conhecimento.");
                
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "oae_id: " + oae.oae_id);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                _lblMessage.Text = UtilBO.GetErroMessage("Eixo de objeto de conhecimento alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o objeto de conhecimento.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region MÉTODOS

        private void LoadPage(int tds_id, int cal_ano, int oae_id, int oae_idPai)
        {
            try
            {
                _VS_tds_id = tds_id;
                _VS_cal_ano = cal_ano;
                _VS_oae_id = oae_id;
                _VS_oae_idPai = oae_idPai;
                ACA_TipoDisciplina tds = new ACA_TipoDisciplina { tds_id = tds_id };
                ACA_TipoDisciplinaBO.GetEntity(tds);

                txtDisciplina.Text = tds.tds_nome;
                txtAnoletivo.Text = cal_ano.ToString();

                ACA_ObjetoAprendizagemEixo oae = new ACA_ObjetoAprendizagemEixo { oae_id = oae_id };
                ACA_ObjetoAprendizagemEixoBO.GetEntity(oae);

                txtEixo.Text = oae.oae_descricao;

                if (oae_idPai > 0)
                {
                    divBotoesSub.Visible = fdsSubEixos.Visible = _btnNovoSub.Visible = false;
                    divEixoPai.Visible = true;

                    ACA_ObjetoAprendizagemEixo oaePai = new ACA_ObjetoAprendizagemEixo { oae_id = oae_idPai };
                    ACA_ObjetoAprendizagemEixoBO.GetEntity(oaePai);

                    txtEixoPai.Text = oaePai.oae_descricao;

                    rfvEixo.ErrorMessage = "Descrição do subeixo de objeto de conhecimento é obrigatória.";
                }
                else
                {
                    VS_ListaEixo = ACA_ObjetoAprendizagemEixoBO.SelectByDiscAno(_VS_tds_id, cal_ano, _VS_oae_id);
                    VS_ListaEixo = VS_ListaEixo.OrderBy(q => q.oae_ordem).ThenBy(q => q.oae_descricao).ToList();

                    _grvEixoObjetoAprendizagem.DataSource = VS_ListaEixo;
                    _grvEixoObjetoAprendizagem.DataBind();
                }

                VS_ListaObjetos = ACA_ObjetoAprendizagemBO.SelectBy_TipoDisciplinaEixo(_VS_tds_id, _VS_cal_ano, _VS_oae_id);
                VS_ListaObjetos = VS_ListaObjetos.OrderBy(o => o.oap_descricao).ToList();

                _grvObjetoAprendizagem.DataSource = VS_ListaObjetos;
                _grvObjetoAprendizagem.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar página.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}