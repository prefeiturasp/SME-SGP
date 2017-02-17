using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Data;

public partial class Configuracao_TipoPeriodoCurso_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Recebe o Id para enviar os dados para edição.
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(gvPeriodoCurso.DataKeys[gvPeriodoCurso.EditIndex].Value);
        }
    }

    /// <summary>
    /// Guarda os filtros
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.TipoPeriodoCurso)
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                string valor;
                if (filtros.TryGetValue("VS_Ordenacao", out valor))
                {
                    return valor;
                }
            }

            return String.Empty;
        }
    }

    #endregion Propriedades
    
    #region Métodos

    /// <summary>
    /// Realiza a consulta pelos filtros informados.
    /// </summary>
    private void Pesquisar()
    {
        try
        {
            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            gvPeriodoCurso.PageIndex = 0;
            odsPeriodoCurso.SelectParameters.Clear();
            odsPeriodoCurso.SelectParameters.Add("tne_id", UCComboTipoNivelEnsino.Valor.ToString());
            odsPeriodoCurso.SelectParameters.Add("tme_id", UCComboTipoModalidadeEnsino.Valor.ToString());

            odsPeriodoCurso.DataBind();

            fdsResultados.Visible = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar a consulta de tipo de {0}.", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Erro);
        }
    }               

    #endregion Métodos

    #region Eventos

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
                lblMessage.Text = message;
            try
            {
                gvPeriodoCurso.PageSize = ApplicationWEB._Paginacao;

                UCComboTipoNivelEnsino.CarregarTipoNivelEnsino();
                UCComboTipoModalidadeEnsino.CarregarTipoModalidadeEnsino();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
            Page.Form.DefaultFocus = UCComboTipoNivelEnsino.Combo_ClientID;
            Page.Form.DefaultButton = btnPesquisar.UniqueID;

            lblLegendaBuscaPeriodoCurso.Text = "Consulta de" + " " + "tipos de" + " " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
        }       
    }

    /// <summary>
    /// Controle do databound da grid e tratamentos necessarios.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void gvPeriodoCurso_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_TipoCurriculoPeriodoBO.GetTotalRecords();
        
        if (gvPeriodoCurso.Rows.Count > 0)
        {
            if (gvPeriodoCurso.Rows[0].FindControl("_btnSubir") != null)//para deixar a seta do primeiro registro do grid só uma seta para baixo             
                ((ImageButton)gvPeriodoCurso.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
            if (gvPeriodoCurso.Rows[gvPeriodoCurso.Rows.Count - 1].FindControl("_btnDescer") != null)//para deixar a seta do último registro do grid só uma seta para cima                
                ((ImageButton)gvPeriodoCurso.Rows[gvPeriodoCurso.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
        }
    }

    /// <summary>
    /// Chama metodo Pesquisa para buscar valores
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {

        if (Page.IsValid)
            Pesquisar();
    }

    protected void btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoPeriodoCurso/Busca.aspx", false);
    }
    
    protected void gvPeriodoCurso_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e != null && e.CommandName != "Page")
        {
            if (e.CommandName == "Subir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int tcp_idDescer = Convert.ToInt32(gvPeriodoCurso.DataKeys[index - 1]["tcp_id"]);
                    byte tcp_ordemDescer = Convert.ToByte(gvPeriodoCurso.DataKeys[index]["tcp_ordem"]);
                    ACA_TipoCurriculoPeriodo entityDescer = new ACA_TipoCurriculoPeriodo { tcp_id = tcp_idDescer };
                    ACA_TipoCurriculoPeriodoBO.GetEntity(entityDescer);
                    entityDescer.tcp_ordem = tcp_ordemDescer;

                    int tcp_idSubir = Convert.ToInt32(gvPeriodoCurso.DataKeys[index]["tcp_id"]);
                    byte tcp_ordemSubir = Convert.ToByte(gvPeriodoCurso.DataKeys[index - 1]["tcp_ordem"]);
                    ACA_TipoCurriculoPeriodo entitySubir = new ACA_TipoCurriculoPeriodo { tcp_id = tcp_idSubir };
                    ACA_TipoCurriculoPeriodoBO.GetEntity(entitySubir);
                    entitySubir.tcp_ordem = tcp_ordemSubir;

                    if (ACA_TipoCurriculoPeriodoBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        Pesquisar();

                        if (gvPeriodoCurso.Rows.Count > 0)
                        {
                            if (gvPeriodoCurso.Rows[0].FindControl("_btnSubir") != null)//para deixar a seta do primeiro registro do grid só uma seta para baixo             
                                ((ImageButton)gvPeriodoCurso.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            if (gvPeriodoCurso.Rows[gvPeriodoCurso.Rows.Count - 1].FindControl("_btnDescer") != null)//para deixar a seta do último registro do grid só uma seta para cima                
                                ((ImageButton)gvPeriodoCurso.Rows[gvPeriodoCurso.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }
                    }                   

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tcp_id: " + tcp_idSubir);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tcp_id: " + tcp_idDescer);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao alterar ordem de tipo de {0}.", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Alerta);
                }
            }

            if (e.CommandName == "Descer")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int tcp_idDescer = Convert.ToInt32(gvPeriodoCurso.DataKeys[index]["tcp_id"]);
                    byte tcp_ordemDescer = Convert.ToByte(gvPeriodoCurso.DataKeys[index + 1]["tcp_ordem"]);
                    ACA_TipoCurriculoPeriodo entityDescer = new ACA_TipoCurriculoPeriodo { tcp_id = tcp_idDescer };
                    ACA_TipoCurriculoPeriodoBO.GetEntity(entityDescer);
                    entityDescer.tcp_ordem = tcp_ordemDescer;

                    int tcp_idSubir = Convert.ToInt32(gvPeriodoCurso.DataKeys[index + 1]["tcp_id"]);
                    byte tcp_ordemSubir = Convert.ToByte(gvPeriodoCurso.DataKeys[index]["tcp_ordem"]);
                    ACA_TipoCurriculoPeriodo entitySubir = new ACA_TipoCurriculoPeriodo { tcp_id = tcp_idSubir };
                    ACA_TipoCurriculoPeriodoBO.GetEntity(entitySubir);
                    entitySubir.tcp_ordem = tcp_ordemSubir;

                    if (ACA_TipoCurriculoPeriodoBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        Pesquisar();

                        if (gvPeriodoCurso.Rows.Count > 0)
                        {
                            if (gvPeriodoCurso.Rows[0].FindControl("_btnSubir") != null)//para deixar a seta do primeiro registro do grid só uma seta para baixo             
                                ((ImageButton)gvPeriodoCurso.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            if (gvPeriodoCurso.Rows[gvPeriodoCurso.Rows.Count - 1].FindControl("_btnDescer") != null)//para deixar a seta do último registro do grid só uma seta para cima                
                                ((ImageButton)gvPeriodoCurso.Rows[gvPeriodoCurso.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tcp_id: " + tcp_idSubir);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tcp_id: " + tcp_idDescer);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao alterar ordem de tipo de {0}.", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Alerta);
                }
            }
        }
    }
    protected void gvPeriodoCurso_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton _btnSubir = (ImageButton)e.Row.FindControl("_btnSubir");
            if (_btnSubir != null)
            {
                _btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                _btnSubir.CommandArgument = e.Row.RowIndex.ToString();
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
    
    #endregion Eventos

}