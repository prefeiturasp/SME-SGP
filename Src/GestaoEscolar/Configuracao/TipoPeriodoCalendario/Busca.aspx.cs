using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.Validation.Exceptions;
using System.Web;

public partial class Configuracao_PeriodoCalendario_Busca : MotherPageLogado
{
    
    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            if (_dgvTipoPeriodoCalendario.Rows.Count > 0)
            {
                ((ImageButton)_dgvTipoPeriodoCalendario.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                ((ImageButton)_dgvTipoPeriodoCalendario.Rows[_dgvTipoPeriodoCalendario.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
            }

            _dgvTipoPeriodoCalendario.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnNovoTipoPeriodoCalendario.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Recebe o Id para enviar os dados para edição.
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[_dgvTipoPeriodoCalendario.EditIndex].Value);
        }
    }

    #endregion

    #region EVENTOS

    protected void odsTipoPeriodoCalendario_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }    

    protected void _dgvTipoPeriodoCalendario_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int tpc_id = Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[index].Value.ToString());

                ACA_TipoPeriodoCalendario entity = new ACA_TipoPeriodoCalendario { tpc_id = tpc_id };
                ACA_TipoPeriodoCalendarioBO.GetEntity(entity);

                if (ACA_TipoPeriodoCalendarioBO.Delete(entity))
                {
                    if (_dgvTipoPeriodoCalendario.Rows.Count > 0)
                    {
                        ((ImageButton)_dgvTipoPeriodoCalendario.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_dgvTipoPeriodoCalendario.Rows[_dgvTipoPeriodoCalendario.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tpc_id: " + tpc_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Tipo de período do calendário excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    _dgvTipoPeriodoCalendario.PageIndex = 0;
                    _dgvTipoPeriodoCalendario.DataBind();

                    if (_dgvTipoPeriodoCalendario.Rows.Count > 0)
                    {
                        ((ImageButton)_dgvTipoPeriodoCalendario.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_dgvTipoPeriodoCalendario.Rows[_dgvTipoPeriodoCalendario.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }
                else
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o tipo período do calendário.", UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o tipo de período do calendário.", UtilBO.TipoMensagem.Erro);
            }
        }

        if (e.CommandName == "Subir")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());

                int tpc_idDescer = Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[index - 1]["tpc_id"]);
                int tpc_ordemDescer = Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[index]["tpc_ordem"]);
                ACA_TipoPeriodoCalendario entityDescer = new ACA_TipoPeriodoCalendario { tpc_id = tpc_idDescer };
                ACA_TipoPeriodoCalendarioBO.GetEntity(entityDescer);
                entityDescer.tpc_ordem = tpc_ordemDescer;

                int tes_idSubir = Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[index]["tpc_id"]);
                int tes_ordemSubir = Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[index - 1]["tpc_ordem"]);
                ACA_TipoPeriodoCalendario entitySubir = new ACA_TipoPeriodoCalendario { tpc_id = tes_idSubir };
                ACA_TipoPeriodoCalendarioBO.GetEntity(entitySubir);
                entitySubir.tpc_ordem = tes_ordemSubir;

                if (ACA_TipoPeriodoCalendarioBO.SaveOrdem(entityDescer, entitySubir))
                {
                    odsTipoPeriodoCalendario.DataBind();
                    _dgvTipoPeriodoCalendario.PageIndex = 0;
                    _dgvTipoPeriodoCalendario.DataBind();

                    if (_dgvTipoPeriodoCalendario.Rows.Count > 0)
                    {
                        ((ImageButton)_dgvTipoPeriodoCalendario.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_dgvTipoPeriodoCalendario.Rows[_dgvTipoPeriodoCalendario.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tes_id: " + tes_idSubir);
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tes_id: " + tpc_idDescer);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }

        if (e.CommandName == "Descer")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());

                int tpc_idDescer = Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[index]["tpc_id"]);
                int tpc_ordemDescer = Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[index + 1]["tpc_ordem"]);
                ACA_TipoPeriodoCalendario entityDescer = new ACA_TipoPeriodoCalendario { tpc_id = tpc_idDescer };
                ACA_TipoPeriodoCalendarioBO.GetEntity(entityDescer);
                entityDescer.tpc_ordem = tpc_ordemDescer;

                int tes_idSubir = Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[index + 1]["tpc_id"]);
                int tes_ordemSubir = Convert.ToInt32(_dgvTipoPeriodoCalendario.DataKeys[index]["tpc_ordem"]);
                ACA_TipoPeriodoCalendario entitySubir = new ACA_TipoPeriodoCalendario { tpc_id = tes_idSubir };
                ACA_TipoPeriodoCalendarioBO.GetEntity(entitySubir);
                entitySubir.tpc_ordem = tes_ordemSubir;

                if (ACA_TipoPeriodoCalendarioBO.SaveOrdem(entityDescer, entitySubir))
                {
                    odsTipoPeriodoCalendario.DataBind();
                    _dgvTipoPeriodoCalendario.PageIndex = 0;
                    _dgvTipoPeriodoCalendario.DataBind();

                    if (_dgvTipoPeriodoCalendario.Rows.Count > 0)
                    {
                        ((ImageButton)_dgvTipoPeriodoCalendario.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_dgvTipoPeriodoCalendario.Rows[_dgvTipoPeriodoCalendario.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tes_id: " + tes_idSubir);
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tes_id: " + tpc_idDescer);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _dgvTipoPeriodoCalendario_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label _lblAlterar = (Label)e.Row.FindControl("_lblAlterar");
            if (_lblAlterar != null)
            {
                _lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
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

    protected void _btnNovoTipoPeriodoCalendario_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoPeriodoCalendario/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion
}
