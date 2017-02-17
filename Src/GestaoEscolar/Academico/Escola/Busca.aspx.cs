using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Web;

public partial class Academico_Escola_Busca : MotherPageLogado
{
    #region Constantes

    /// <summary>
    /// Constante que indica o número da coluna de exclusão do grid de turnos.
    /// </summary>
    private const int grvEscolasColunaImportacaoFech = 5;

    #endregion

    #region Propriedades

    public int EditItem_esc_id
    {
        get
        {
            return Convert.ToInt32(_grvEscolas.DataKeys[_grvEscolas.EditIndex].Values[0] ?? 0);
        }
    }

    public int EditItem_uni_id
    {
        get
        {
            return Convert.ToInt32(_grvEscolas.DataKeys[_grvEscolas.EditIndex].Values[1] ?? 0);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Escolas)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Escolas)
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
        _grvEscolas.PageSize = UCComboQtdePaginacao1.Valor;
        _grvEscolas.PageIndex = 0;
        // atualiza o grid
        _grvEscolas.DataBind();
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Realiza a consulta pelos filtros informados.
    /// </summary>
    private void _Pesquisar()
    {
        try
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();

            _grvEscolas.PageIndex = 0;
            odsEscola.SelectParameters.Clear();
            odsEscola.SelectParameters.Add("esc_id", "-1");
            odsEscola.SelectParameters.Add("esc_nome", _txtNome.Text);
            odsEscola.SelectParameters.Add("esc_codigo", _txtCodigo.Text);
            odsEscola.SelectParameters.Add("TIPO_MEIOCONTATO_TELEFONE", _txtTelefone.Text);
            odsEscola.SelectParameters.Add("cur_id", UCComboCursoCurriculo.Valor[0].ToString());
            odsEscola.SelectParameters.Add("crr_id", UCComboCursoCurriculo.Valor[1].ToString());
            odsEscola.SelectParameters.Add("esc_situacao", "0");
            odsEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            odsEscola.SelectParameters.Add("uad_idSuperior", UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue);
            odsEscola.SelectParameters.Add("tua_id", UCComboTipoUAEscola1.Valor.ToString());
            odsEscola.SelectParameters.Add("tce_id", uccTipoClassificacaoEscola.uccTipoClassificacaoVisible ? uccTipoClassificacaoEscola.Valor.ToString() : "0");
            
            // Filtra pela visão do usuário.
            odsEscola.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            odsEscola.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
            odsEscola.DataBind();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _grvEscolas.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in odsEscola.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Escolas
                ,
                Filtros = filtros
            };

            #endregion

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _grvEscolas.PageSize = itensPagina;
            // atualiza o grid
            _grvEscolas.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as escolas.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Escolas)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_nome", out valor);
            _txtNome.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_codigo", out valor);
            _txtCodigo.Text = valor;

            //concatena o cur_crr_id, que é retornado do combo
            string cur_id;
            string crr_id;
            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out cur_id)) && (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out crr_id)))            
                UCComboCursoCurriculo.Valor = new[] {Convert.ToInt32(cur_id), Convert.ToInt32(crr_id)};            
          
            if (UCFiltroEscolas1._VS_FiltroEscola)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue = valor;
            }

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tua_id", out valor);
            UCComboTipoUAEscola1.Valor = new Guid(valor);

            if (uccTipoClassificacaoEscola.uccTipoClassificacaoVisible)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tce_id", out valor);
                uccTipoClassificacaoEscola.Valor = Convert.ToInt32(valor);
            }

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

        UCComboQtdePaginacao1.GridViewRelacionado = _grvEscolas;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _grvEscolas.PageSize = ApplicationWEB._Paginacao;

            try
            {
                UCComboTipoUAEscola1.CarregarTipoUAEscola();

                UCFiltroEscolas1.UnidadeAdministrativaCampoObrigatorio = false;
                UCFiltroEscolas1.EscolaCampoObrigatorio = false;

                // Carrega os filtros conforme os parâmetros Acadêmicos.
                UCFiltroEscolas1._LoadInicialFiltroUA();

                UCComboCursoCurriculo.Obrigatorio = false;
                UCComboCursoCurriculo.CarregarCursoCurriculo();

                // Carrega combo de classificações da escola somente se existir tipo cadastrado.
                uccTipoClassificacaoEscola.Carregar();
                
                VerificaBusca();

                if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), fdsEscola.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsEscola.ClientID)), true);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultButton = _btnPesquisar.UniqueID;
            Page.Form.DefaultFocus = UCComboTipoUAEscola1.Combo_ClientID;

            UCFiltroEscolas1.Visible = (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao 
                                            || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao);
            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
        }
    }

    protected void _grvEscolas_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ESC_EscolaBO.GetTotalRecords();

        // seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_grvEscolas);

        if ((!string.IsNullOrEmpty(_grvEscolas.SortExpression)) &&
            (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Escolas))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _grvEscolas.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _grvEscolas.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _grvEscolas.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _grvEscolas.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Escolas
                ,
                Filtros = filtros
            };
        }

        bool permiteImportacao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_IMPORTACAO_DADOS_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        _grvEscolas.Columns[grvEscolasColunaImportacaoFech].Visible = permiteImportacao;
    }

    protected void _grvEscolas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton _btnImportacao = (ImageButton)e.Row.FindControl("_btnImportacao");
            if (_btnImportacao != null)
            {
                _btnImportacao.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }
    
    protected void _grvEscolas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ImportacaoFechamento")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int esc_id = Convert.ToInt32(_grvEscolas.DataKeys[index].Values[0]);

                Session["ImportacaoFechamento"] = "1";

                Session["EscolaImportacaoFechamento"] = esc_id.ToString();

                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Escola/Cadastro.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao cadastrar importação de fechamento para a escola.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Escola/Busca.aspx", false);
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        _Pesquisar();
        fdsResultados.Visible = true;
    }

    #endregion    
}
