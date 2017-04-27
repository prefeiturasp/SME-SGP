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
    public partial class BuscaEixo : MotherPageLogado
    {
        #region PROPRIEDADES

        /// <summary>
        /// Propriedade em ViewState que armazena a lista de questões
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

        public int oae_id
        {
            get
            {
                if (_grvEixoObjetoAprendizagem.EditIndex >= 0)
                    return Convert.ToInt32(_grvEixoObjetoAprendizagem.DataKeys[_grvEixoObjetoAprendizagem.EditIndex].Value);

                return -1;
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
                return UCComboAnoLetivo1.ano;
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
                    _btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                    _grvEixoObjetoAprendizagem.EmptyDataText = string.Format("Não existe eixo objeto de conhecimento associado a este {0}.", GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_MIN"));
                    UCComboAnoLetivo1.CarregarAnoAtual();

                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        Session["tds_id_oae"] = PreviousPage.tds_id;
                        LoadPage(PreviousPage.tds_id);
                    }
                    else if (Session["tds_id_oae"] != null)
                    {
                        LoadPage(Convert.ToInt32(Session["tds_id_oae"]));
                        Session["tds_id_oae"] = null;
                    }
                    else
                    {
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ObjetoAprendizagem/BuscaDisciplina.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }

                UCComboAnoLetivo1.IndexChanged += UCComboAnoLetivo1_IndexChanged;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboAnoLetivo1_IndexChanged()
        {
            LoadPage(_VS_tds_id);
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
                    Session["cal_ano_oae"] = UCComboAnoLetivo1.ano;
                    Session["oae_id"] = id;
                    Session["oae_idPai"] = null;

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
        
        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Academico/ObjetoAprendizagem/BuscaDisciplina.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _btnNovo_Click(object sender, EventArgs e)
        {
            txtDescricao.Text = "";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "EditarAulas", "$('#divInserir').dialog('open');", true);
            updPopUp.Update();
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
                    cal_ano = UCComboAnoLetivo1.ano,
                    oae_ordem = VS_ListaEixo.Any() ? VS_ListaEixo.Max(p => p.oae_ordem) + 1 : 1,
                    oae_descricao = txtDescricao.Text
                };

                if (!ACA_ObjetoAprendizagemEixoBO.Salvar(oae))
                    throw new ValidationException("Erro ao tentar salvar eixo de objeto de conhecimento.");

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "oae_id: " + oae.oae_id);

                VS_ListaEixo = ACA_ObjetoAprendizagemEixoBO.SelectByDiscAno(_VS_tds_id, cal_ano, -1);
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
                lblMessagePopUp.Text = UtilBO.GetErroMessage("Erro ao excluir o objeto de conhecimento.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region MÉTODOS

        private void LoadPage(int tds_id)
        {
            try
            {
                _VS_tds_id = tds_id;
                var tds = new ACA_TipoDisciplina { tds_id = tds_id };
                ACA_TipoDisciplinaBO.GetEntity(tds);

                txtDisciplina.Text = tds.tds_nome;

                _grvEixoObjetoAprendizagem.PageIndex = 0;

                VS_ListaEixo = ACA_ObjetoAprendizagemEixoBO.SelectByDiscAno(_VS_tds_id, UCComboAnoLetivo1.ano, -1);
                VS_ListaEixo = VS_ListaEixo.OrderBy(q => q.oae_ordem).ThenBy(q => q.oae_descricao).ToList();

                _grvEixoObjetoAprendizagem.DataSource = VS_ListaEixo;
                _grvEixoObjetoAprendizagem.DataBind();
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