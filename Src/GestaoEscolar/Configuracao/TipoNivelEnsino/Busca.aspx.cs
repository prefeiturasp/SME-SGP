using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Configuracao_TipoNivelEnsino_Busca : MotherPageLogado
{

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvTipoNivelEnsino;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
            {
                _lblMessage.Text = message;
            }

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvTipoNivelEnsino.PageSize = itensPagina;
            // atualiza o grid
            _dgvTipoNivelEnsino.DataBind();
            
            if (_dgvTipoNivelEnsino.Rows.Count > 0)
            {
                ((ImageButton)_dgvTipoNivelEnsino.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                ((ImageButton)_dgvTipoNivelEnsino.Rows[_dgvTipoNivelEnsino.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
            }
        }
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        _dgvTipoNivelEnsino.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvTipoNivelEnsino.PageIndex = 0;
        // atualiza o grid
        _dgvTipoNivelEnsino.DataBind();
    }

    #endregion Delegates
    
    #region Eventos

    protected void _odsTipoNivelEnsino_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void _dgvTipoNivelEnsino_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Subir")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());

                int tne_idDescer = Convert.ToInt32(_dgvTipoNivelEnsino.DataKeys[index - 1]["tne_id"]);
                int tne_ordemDescer = Convert.ToInt32(_dgvTipoNivelEnsino.DataKeys[index]["tne_ordem"]);
                ACA_TipoNivelEnsino entityDescer = new ACA_TipoNivelEnsino { tne_id = tne_idDescer };
                ACA_TipoNivelEnsinoBO.GetEntity(entityDescer);
                entityDescer.tne_ordem = tne_ordemDescer;

                int tne_idSubir = Convert.ToInt32(_dgvTipoNivelEnsino.DataKeys[index]["tne_id"]);
                int tne_ordemSubir = Convert.ToInt32(_dgvTipoNivelEnsino.DataKeys[index - 1]["tne_ordem"]);
                ACA_TipoNivelEnsino entitySubir = new ACA_TipoNivelEnsino { tne_id = tne_idSubir };
                ACA_TipoNivelEnsinoBO.GetEntity(entitySubir);
                entitySubir.tne_ordem = tne_ordemSubir;

                if (ACA_TipoNivelEnsinoBO.SaveOrdem(entityDescer, entitySubir))
                {
                    _odsTipoNivelEnsino.DataBind();
                    _dgvTipoNivelEnsino.PageIndex = 0;
                    _dgvTipoNivelEnsino.DataBind();

                    if (_dgvTipoNivelEnsino.Rows.Count > 0)
                    {
                        ((ImageButton)_dgvTipoNivelEnsino.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_dgvTipoNivelEnsino.Rows[_dgvTipoNivelEnsino.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tne_id: " + tne_idSubir);
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tne_id: " + tne_idDescer);
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

                int tne_idDescer = Convert.ToInt32(_dgvTipoNivelEnsino.DataKeys[index]["tne_id"]);
                int tne_ordemDescer = Convert.ToInt32(_dgvTipoNivelEnsino.DataKeys[index + 1]["tne_ordem"]);
                ACA_TipoNivelEnsino entityDescer = new ACA_TipoNivelEnsino { tne_id = tne_idDescer };
                ACA_TipoNivelEnsinoBO.GetEntity(entityDescer);
                entityDescer.tne_ordem = tne_ordemDescer;

                int tne_idSubir = Convert.ToInt32(_dgvTipoNivelEnsino.DataKeys[index + 1]["tne_id"]);
                int tne_ordemSubir = Convert.ToInt32(_dgvTipoNivelEnsino.DataKeys[index]["tne_ordem"]);
                ACA_TipoNivelEnsino entitySubir = new ACA_TipoNivelEnsino { tne_id = tne_idSubir };
                ACA_TipoNivelEnsinoBO.GetEntity(entitySubir);
                entitySubir.tne_ordem = tne_ordemSubir;

                if (ACA_TipoNivelEnsinoBO.SaveOrdem(entityDescer, entitySubir))
                {
                    _odsTipoNivelEnsino.DataBind();
                    _dgvTipoNivelEnsino.PageIndex = 0;
                    _dgvTipoNivelEnsino.DataBind();

                    if (_dgvTipoNivelEnsino.Rows.Count > 0)
                    {
                        ((ImageButton)_dgvTipoNivelEnsino.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)_dgvTipoNivelEnsino.Rows[_dgvTipoNivelEnsino.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tne_id: " + tne_idSubir);
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tne_id: " + tne_idDescer);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _dgvTipoNivelEnsino_RowDataBound(object sender, GridViewRowEventArgs e)
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
    
    #endregion Eventos
}