using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Web;

public partial class Academico_Curso_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Retorna o cur_id do registro que esta sendo editado.
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_grvCursos.DataKeys[_grvCursos.EditIndex].Value);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Curso)
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                string valor;
                if (filtros.TryGetValue("VS_Ordenacao", out valor))
                {
                    return valor;
                }
            }

            return "";
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private SortDirection VS_SortDirection
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Curso)
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

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        _grvCursos.PageSize = UCComboQtdePaginacao1.Valor;
        _grvCursos.PageIndex = 0;
        _grvCursos.Sort("", SortDirection.Ascending);
        // atualiza o grid
        _grvCursos.DataBind();
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Realiza a pesquisa com base nos filtros selecionados.
    /// </summary>
    private void _Pesquisar()
    {
        try
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();

            _grvCursos.PageIndex = 0;
            odsCurso.SelectParameters.Clear();
            odsCurso.SelectParameters.Add("esc_id", "0");
            odsCurso.SelectParameters.Add("uni_id", "0");
            odsCurso.SelectParameters.Add("cur_id", "0");
            odsCurso.SelectParameters.Add("tne_id", UCComboTipoNivelEnsino1.Valor.ToString());
            odsCurso.SelectParameters.Add("tme_id", UCComboTipoModalidadeEnsino1.Valor.ToString());
            odsCurso.SelectParameters.Add("cur_nome", _txtNomeCurso.Text);
            odsCurso.SelectParameters.Add("cur_codigo", _txtCodigoCurso.Text);
            odsCurso.SelectParameters.Add("cur_situacao", "0");
            odsCurso.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao)
            {
                odsCurso.SelectParameters.Add("usu_id", Guid.Empty.ToString());
                odsCurso.SelectParameters.Add("gru_id", Guid.Empty.ToString());
            }
            else
            {
                odsCurso.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                odsCurso.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            }

            _grvCursos.Sort("", SortDirection.Ascending);
            odsCurso.DataBind();

            // quantidade de itens por página            
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _grvCursos.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in odsCurso.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Curso
                ,
                Filtros = filtros
            };

            #endregion

            // mostra essa quantidade no combobox            
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _grvCursos.PageSize = itensPagina;
            // atualiza o grid
            _grvCursos.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + ".", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Curso)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_nome", out valor);
            _txtNomeCurso.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_codigo", out valor);
            _txtCodigoCurso.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tne_id", out valor);
            UCComboTipoNivelEnsino1.Valor = Convert.ToInt32(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tme_id", out valor);
            UCComboTipoModalidadeEnsino1.Valor = Convert.ToInt32(valor);

            _Pesquisar();
        }
        else
        {
            fdsResultados.Visible = false;
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _grvCursos;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _grvCursos.PageSize = ApplicationWEB._Paginacao;

            try
            {
                UCComboTipoNivelEnsino1.CarregarTipoNivelEnsino();
                UCComboTipoModalidadeEnsino1.CarregarTipoModalidadeEnsino();

                VerificaBusca();

                if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), fdsCurso.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsCurso.ClientID)), true);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }

            #region SetaNomeCurso
            _grvCursos.Columns[1].HeaderText = "Nome do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            lgdConsultaCurso.InnerText = "Consulta de " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            LabelCodigoCurso.Text = "Código do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            LabelNomeCurso.Text = "Nome do(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();


            #endregion

            Page.Form.DefaultFocus = UCComboTipoNivelEnsino1.Combo_ClientID;
            Page.Form.DefaultButton = _btnPesquisar.UniqueID;

            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
        }
    }

    protected void _grvCursos_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_CursoBO.GetTotalRecords();
        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_grvCursos);

        if ((!string.IsNullOrEmpty(_grvCursos.SortExpression)) &&
             (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Curso))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _grvCursos.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _grvCursos.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _grvCursos.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _grvCursos.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Curso
                ,
                Filtros = filtros
            };
        }
    }

    protected void _grvCursos_RowDataBound(object sender, GridViewRowEventArgs e)
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
        }
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Curso/Busca.aspx", false);
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        _Pesquisar();
        fdsResultados.Visible = true;
    }

    #endregion
}
