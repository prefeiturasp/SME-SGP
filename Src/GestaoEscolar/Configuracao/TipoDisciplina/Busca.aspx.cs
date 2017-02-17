using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Configuracao_TipoDisciplina_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Retorna o id do tipo de disciplina para editar
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvTipoDisciplina.DataKeys[_dgvTipoDisciplina.EditIndex].Value);
        }
    }

    #endregion

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

            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            
            _dgvTipoDisciplina.Columns[1].Visible = controlarOrdem;

            odsTipoDisciplina.SelectParameters.Clear();
            odsTipoDisciplina.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _dgvTipoDisciplina.PageSize = itensPagina;
            _dgvTipoDisciplina.DataBind();

            if (_dgvTipoDisciplina.Rows.Count > 0)
            {
                ((ImageButton)_dgvTipoDisciplina.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                ((ImageButton)_dgvTipoDisciplina.Rows[_dgvTipoDisciplina.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
            }
        }
    }

    #endregion

    #region Eventos

    protected void _dgvTipoDisciplina_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_TipoDisciplinaBO.GetTotalRecords();

        if (_dgvTipoDisciplina.Rows.Count > 0)
        {
            ((ImageButton)_dgvTipoDisciplina.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
            ((ImageButton)_dgvTipoDisciplina.Rows[_dgvTipoDisciplina.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
        }
    }

    protected void odsTipoDisciplina_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {   
        if (e.ExecutingSelectCount)
        {
            e.InputParameters.Clear();            
        }
    }

    protected void _dgvTipoDisciplina_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Subir")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());

                int tds_idDescer = Convert.ToInt32(_dgvTipoDisciplina.DataKeys[index - 1]["tds_id"]);
                int tds_ordemDescer = Convert.ToInt32(_dgvTipoDisciplina.DataKeys[index]["tds_ordem"]);
                ACA_TipoDisciplina entityDescer = new ACA_TipoDisciplina { tds_id = tds_idDescer };
                ACA_TipoDisciplinaBO.GetEntity(entityDescer);
                entityDescer.tds_ordem = tds_ordemDescer;

                int tds_idSubir = Convert.ToInt32(_dgvTipoDisciplina.DataKeys[index]["tds_id"]);
                int tds_ordemSubir = Convert.ToInt32(_dgvTipoDisciplina.DataKeys[index - 1]["tds_ordem"]);
                ACA_TipoDisciplina entitySubir = new ACA_TipoDisciplina { tds_id = tds_idSubir };
                ACA_TipoDisciplinaBO.GetEntity(entitySubir);
                entitySubir.tds_ordem = tds_ordemSubir;

                if (ACA_TipoDisciplinaBO.SaveOrdem(entityDescer, entitySubir))
                {
                    _dgvTipoDisciplina.DataBind();


                    if (_dgvTipoDisciplina.Rows.Count > 0)
                    {
                        ((ImageButton)_dgvTipoDisciplina.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_dgvTipoDisciplina.Rows[_dgvTipoDisciplina.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tds_id: " + tds_idSubir);
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tds_id: " + tds_idDescer);
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

                int tds_idDescer = Convert.ToInt32(_dgvTipoDisciplina.DataKeys[index]["tds_id"]);
                int tds_ordemDescer = Convert.ToInt32(_dgvTipoDisciplina.DataKeys[index + 1]["tds_ordem"]);
                ACA_TipoDisciplina entityDescer = new ACA_TipoDisciplina { tds_id = tds_idDescer };
                ACA_TipoDisciplinaBO.GetEntity(entityDescer);
                entityDescer.tds_ordem = tds_ordemDescer;

                int tds_idSubir = Convert.ToInt32(_dgvTipoDisciplina.DataKeys[index + 1]["tds_id"]);
                int tds_ordemSubir = Convert.ToInt32(_dgvTipoDisciplina.DataKeys[index]["tds_ordem"]);
                ACA_TipoDisciplina entitySubir = new ACA_TipoDisciplina { tds_id = tds_idSubir };
                ACA_TipoDisciplinaBO.GetEntity(entitySubir);
                entitySubir.tds_ordem = tds_ordemSubir;

                if (ACA_TipoDisciplinaBO.SaveOrdem(entityDescer, entitySubir))
                {
                    _dgvTipoDisciplina.DataBind();

                    if (_dgvTipoDisciplina.Rows.Count > 0)
                    {
                        ((ImageButton)_dgvTipoDisciplina.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_dgvTipoDisciplina.Rows[_dgvTipoDisciplina.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tds_id: " + tds_idSubir);
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tds_id: " + tds_idDescer);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _dgvTipoDisciplina_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
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

    protected void _btnNovoTipoDisciplina_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoDisciplina/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion
}
