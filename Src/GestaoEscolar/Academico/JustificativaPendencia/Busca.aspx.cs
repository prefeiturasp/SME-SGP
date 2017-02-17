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

public partial class Academico_JustificativaPendencia_Busca : MotherPageLogado
{
    #region Constantes

    private int COLUNA_PERIODO_CALENDARIO = 3;

    #endregion Constantes

    #region Propriedades

    /// <summary>
    /// Retorna as chaves do registro que esta sendo editado.
    /// </summary>
    public string[] EditItem
    {
        get
        {
            string[] retorno = new string[7];
            retorno[0] = grvJustificativas.DataKeys[grvJustificativas.EditIndex]["tud_id"].ToString();
            retorno[1] = grvJustificativas.DataKeys[grvJustificativas.EditIndex]["cal_id"].ToString();
            retorno[2] = grvJustificativas.DataKeys[grvJustificativas.EditIndex]["tpc_id"].ToString();
            retorno[3] = grvJustificativas.DataKeys[grvJustificativas.EditIndex]["fjp_id"].ToString();
            retorno[4] = grvJustificativas.DataKeys[grvJustificativas.EditIndex]["uaSuperior"].ToString();
            retorno[5] = grvJustificativas.DataKeys[grvJustificativas.EditIndex]["esc_uni_id"].ToString();
            retorno[6] = grvJustificativas.DataKeys[grvJustificativas.EditIndex]["fjp_justificativa"].ToString();
            return retorno;
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.JustificativaPendencia)
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

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private SortDirection VS_SortDirection
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.JustificativaPendencia)
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                string valor;
                if (filtros.TryGetValue("VS_SortDirection", out valor))
                {
                    return (SortDirection)Enum.Parse(typeof(SortDirection), valor);
                }
            }

            return SortDirection.Ascending;
        }
    }

    #endregion Propriedades

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        comboPaginacao.GridViewRelacionado = grvJustificativas;

        try
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaJustificativaPendencia.js"));
            }

            grvJustificativas.Columns[COLUNA_PERIODO_CALENDARIO].HeaderText = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_PERIODO_CALENDARIO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            comboUAEscola.IndexChangedUA += comboUAEscola_IndexChangedUA;
            comboUAEscola.IndexChangedUnidadeEscola += comboUAEscola_IndexChangedUnidadeEscola;
            comboCalendario.IndexChanged += comboCalendario_IndexChanged;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                grvJustificativas.PageIndex = 0;
                grvJustificativas.PageSize = ApplicationWEB._Paginacao;

                comboCalendario.PermiteEditar = comboTurmaDisciplina.PermiteEditar = comboPeriodoCalendario.PermiteEditar = false;
                comboUAEscola.FocusUA();
                comboUAEscola.Inicializar();
                VerificaBusca();

                Page.Form.DefaultButton = btnPesquisar.UniqueID;
                Page.Form.DefaultFocus = comboUAEscola.VisibleUA ? comboUAEscola.ComboUA_ClientID : comboUAEscola.ComboEscola_ClientID;

                divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        Pesquisar();
        fdsResultados.Visible = true;
    }

    protected void btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/JustificativaPendencia/Busca.aspx", false);
    }

    protected void btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/JustificativaPendencia/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void grvJustificativas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AbrirJustificativa")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                DataKey keys = grvJustificativas.DataKeys[index];
                GridViewRow row = grvJustificativas.Rows[index];

                lblJustificativa.Text = "<b>" + ((Label)row.FindControl("lblAlterar")).Text + "</b>"
                                            + "<br/><b>" + ((Label)row.FindControl("lblPeriodoCalendario")).Text + "</b>"
                                            + "<br/><br/>" + keys.Values["fjp_justificativa"].ToString();

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "AbrirJustificativa",
                                                            "$(document).ready(function() { $('#divJustificativa').dialog('option', 'title', '"
                                                            + GetGlobalResourceObject("Academico", "JustificativaPendencia.Busca.grvJustificativas.colunaJustificativa").ToString()
                                                            + "'); $('#divJustificativa').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }
        else if (e.CommandName == "Excluir")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                DataKey keys = grvJustificativas.DataKeys[index];

                CLS_FechamentoJustificativaPendencia entity = new CLS_FechamentoJustificativaPendencia
                {
                    tud_id = Convert.ToInt64(keys.Values["tud_id"])
                    ,
                    cal_id = Convert.ToInt32(keys.Values["cal_id"])
                    ,
                    tpc_id = Convert.ToInt32(keys.Values["tpc_id"])
                    ,
                    fjp_id = Convert.ToInt32(keys.Values["fjp_id"])
                };
                CLS_FechamentoJustificativaPendenciaBO.GetEntity(entity);

                if (CLS_FechamentoJustificativaPendenciaBO.Excluir(entity))
                {
                    grvJustificativas.PageIndex = 0;
                    grvJustificativas.DataBind();

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "fjp_id: " + entity.fjp_id + ", tud_id: " + entity.tud_id + ", cal_id: " + entity.cal_id + ", tpc_id: " + entity.tpc_id);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "JustificativaPendencia.Busca.SucessoExcluir").ToString(), UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "JustificativaPendencia.Busca.ErroExcluir").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void grvJustificativas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAlterar = (Label)e.Row.FindControl("lblAlterar");
            if (lblAlterar != null)
            {
                lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
            if (btnAlterar != null)
            {
                btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
            if (btnExcluir != null)
            {
                btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }

            ImageButton btnJustificativa = (ImageButton)e.Row.FindControl("btnJustificativa");
            if (btnJustificativa != null)
            {
                btnJustificativa.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void grvJustificativas_DataBound(object sender, EventArgs e)
    {
        ucTotalRegistros.Total = CLS_FechamentoJustificativaPendenciaBO.GetTotalRecords();
        
        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(grvJustificativas);

        if (!string.IsNullOrEmpty(grvJustificativas.SortExpression) &&
           __SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.JustificativaPendencia)
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = grvJustificativas.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", grvJustificativas.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = grvJustificativas.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", grvJustificativas.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.JustificativaPendencia
                ,
                Filtros = filtros
            };
        }
    }

    #endregion Eventos

    #region Delegates

    protected void comboPaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        grvJustificativas.PageSize = comboPaginacao.Valor;
        grvJustificativas.PageIndex = 0;
        // atualiza o grid
        grvJustificativas.DataBind();
    }

    protected void comboUAEscola_IndexChangedUA()
    {
        try
        {
            comboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

            if (comboUAEscola.Uad_ID != Guid.Empty)
            {
                comboUAEscola.FocoEscolas = true;
                comboUAEscola.PermiteAlterarCombos = true;
            }

            comboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void comboUAEscola_IndexChangedUnidadeEscola()
    {
        try
        {
            comboCalendario.CarregarPorEscola(comboUAEscola.Esc_ID);
            comboCalendario.PermiteEditar = comboUAEscola.Esc_ID > 0;
            if (comboCalendario.QuantidadeItensCombo == 1)
            {
                comboCalendario_IndexChanged();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void comboCalendario_IndexChanged()
    {
        try
        {
            comboTurmaDisciplina.CarregarTurmaDisciplinaEletivaAluno(comboUAEscola.Esc_ID, comboCalendario.Valor);
            comboPeriodoCalendario.CarregarPorCalendario(comboCalendario.Valor);
            comboTurmaDisciplina.PermiteEditar = comboPeriodoCalendario.PermiteEditar = comboCalendario.Valor > 0;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Delegates

    #region Métodos

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.JustificativaPendencia)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor, valor2;
            int valorInt;

            if (comboUAEscola.FiltroEscola)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ua_superior", out valor);
                if (!string.IsNullOrEmpty(valor))
                    comboUAEscola.DdlUA.SelectedValue = valor;

                if (valor != Guid.Empty.ToString())
                    SelecionarEscola(comboUAEscola.FiltroEscola);
            }
            else
                SelecionarEscola(comboUAEscola.FiltroEscola);
            comboUAEscola_IndexChangedUnidadeEscola();

            //Calendario
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
            if (Int32.TryParse(valor, out valorInt))
                comboCalendario.Valor = valorInt;
            comboCalendario_IndexChanged();

            //Disciplina
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tud_id", out valor);
            comboTurmaDisciplina.Valor = Convert.ToInt64(valor);

            //Periodo Calendario
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cap_id", out valor);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tpc_id", out valor2);
            comboPeriodoCalendario.Valor = new int[2] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };

            Pesquisar();
        }
        else
        {
            fdsResultados.Visible = false;
        }
    }

    /// <summary>
    /// Seleciona a escola no combo de acordo com o parâmetro salvo na sessão de busca 
    /// realizada.
    /// </summary>
    /// <param name="filtroEscolas"></param>
    private void SelecionarEscola(bool filtroEscolas)
    {
        if (filtroEscolas)
            comboUAEscola_IndexChangedUA();

        string esc_uni_id;

        if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_uni_id", out esc_uni_id))
        {
            comboUAEscola.DdlEscola.SelectedValue = esc_uni_id;
        }
    }

    /// <summary>
    /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
    /// </summary>
    private void Pesquisar()
    {
        try
        {
            grvJustificativas.PageIndex = 0;
            odsJustificativas.SelectParameters.Clear();
            odsJustificativas.SelectParameters.Add("esc_id", comboUAEscola.Esc_ID.ToString());
            odsJustificativas.SelectParameters.Add("uni_id", comboUAEscola.Uni_ID.ToString());
            odsJustificativas.SelectParameters.Add("cal_id", comboCalendario.Valor.ToString());
            odsJustificativas.SelectParameters.Add("tud_id", comboTurmaDisciplina.Valor.ToString());
            odsJustificativas.SelectParameters.Add("tpc_id", comboPeriodoCalendario.Valor[0].ToString());

            #region Salvar busca realizada com os parâmetros do ODS.

            Dictionary<string, string> filtros = new Dictionary<string, string>();

            //Salvar UA Superior.            
            if (comboUAEscola.FiltroEscola)
                filtros.Add("ua_superior", comboUAEscola.Uad_ID.ToString());

            foreach (Parameter param in odsJustificativas.SelectParameters)
            {
                if (param.Name != "esc_id"
                    && param.Name != "uni_id")
                {
                    filtros.Add(param.Name, param.DefaultValue);
                }
            }
            filtros.Add("esc_uni_id", comboUAEscola.ValorComboEscolaSelectedValue);
            filtros.Add("cap_id", comboPeriodoCalendario.Valor[1].ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.JustificativaPendencia
                ,
                Filtros = filtros
            };

            #endregion

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            // mostra essa quantidade no combobox
            comboPaginacao.Valor = itensPagina;
            // atribui essa quantidade para o grid
            grvJustificativas.PageSize = itensPagina;
            // atualiza o grid
            grvJustificativas.DataBind();
            grvJustificativas.Sort(VS_Ordenacao, VS_SortDirection);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar as justificativas de pendências", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Métodos
}