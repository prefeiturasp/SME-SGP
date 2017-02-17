using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using MSTech.CoreSSO.Entities;
using System.Linq;
using System.Data;


public partial class Busca : MotherPageLogado
{

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
        }
        if (!IsPostBack)
        {            
                lblMensagemErro.Text = "";

            try
            {
                uccUaEscola.Focus();
                uccUaEscola.EnableEscolas = true;
                uccUaEscola.FiltroEscola = true;
                uccUaEscola.CarregarEscolaAutomatico = true;
                uccUaEscola.Inicializar();

                uccUaEscola.ObrigatorioEscola = !uccUaEscola.FiltroEscola;
                this.VerificaBusca();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemErro.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
            Page.Form.DefaultFocus = uccUaEscola.VisibleUA ? uccUaEscola.ComboUA_ClientID : uccUaEscola.ComboEscola_ClientID;

            btn_pesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
        }
        string script = String.Format("SetConfirmDialogButton('{0}','{1}');", String.Concat("#", _btnExportar.ClientID), "Essa operação irá salvar <b>" + NomeModulo + "</b>.<br /><br />Confirma a operação?");
        Page.ClientScript.RegisterStartupScript(GetType(), _btnExportar.ClientID, script, true);

        string uad_id;
        string esc_id;

        __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_id", out uad_id);
        __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id);
        VS_uadId = uad_id;
        VS_escId = Convert.ToInt32(esc_id);
    }

    #endregion

    #region Propriedades

    private string _nomeModulo;

    /// <summary>
    /// Propriedade com o nome do modulo.
    /// </summary>
    private string NomeModulo
    {
        get
        {
            if (string.IsNullOrEmpty(_nomeModulo))
            {
                SYS_Modulo entModulo;
                if (Modulo.IsNew)
                {
                    entModulo = new SYS_Modulo
                    {
                        mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                        sis_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.sis_id
                    };
                    entModulo = GestaoEscolarUtilBO.GetEntityModuloCache(entModulo);
                }
                else
                {
                    entModulo = Modulo;
                }
                _nomeModulo = entModulo.mod_nome.ToString();
            }

            return _nomeModulo;
        }
    }

    /// <summary>
    /// Armazena o id da UA
    /// </summary>
    private string VS_uadId
    {
        get
        {
            if (ViewState["VS_uadId"] != null)
                return Convert.ToString(ViewState["VS_uadId"]);
            return null;
        }
        set
        {
            ViewState["VS_uadId"] = value;
        }
    }

    /// <summary>
    /// Armazena o id da escola
    /// </summary>
    private int VS_escId
    {
        get
        {
            if (ViewState["VS_escId"] != null)
                return Convert.ToInt32(ViewState["VS_escId"]);
            return -1;
        }
        set
        {
            ViewState["VS_escId"] = value;
        }
    }

    #endregion Propriedades

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // Atribui nova quantidade de itens por página para o grid.
        grvConsultaLogTablets.PageSize = UCComboQtdePaginacao1.Valor;
        grvConsultaLogTablets.PageIndex = 0;

        // Atualiza o grid
        grvConsultaLogTablets.DataBind();
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Verifica se o usuário tem permissão de acesso a página.
    /// </summary>
    private bool VerificarPermissaoUsuario()
    {
        if (!(__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir ||
            __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar))
        {
            __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
            Response.Redirect("~/Index.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            return false;
        }
        return true;
    }

    /// <summary>
    /// Pesquisa os alunos de acordo com os filtros de busca definidos.
    /// </summary>
    private void Pesquisar()
    {        
        try
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();

            grvConsultaLogTablets.PageIndex = 0;
            odsConsultaLogTablets.SelectParameters.Clear();

            odsConsultaLogTablets.SelectParameters.Add("uad_id", uccUaEscola.Uad_ID.ToString());
            odsConsultaLogTablets.SelectParameters.Add("esc_id", uccUaEscola.Esc_ID.ToString());

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in odsConsultaLogTablets.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            filtros.Add("uni_id", uccUaEscola.Uni_ID.ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.ConsultaEquipamentos
                ,
                Filtros = filtros
            };

            #endregion Salvar busca realizada com os parâmetros do ODS.

            grvConsultaLogTablets.DataBind();

            fdsResultados.Visible = true;

        }
        catch (ValidationException ex)
        {
            lblMensagemErro.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagemErro.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os equipamentos.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta, 
    /// colocando os filtros nos campos da tela.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConsultaEquipamentos)
        {
            // Recuperar busca realizada e btnPesquisar automaticamente.
            string valor1;

            if (uccUaEscola.FiltroEscola)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_id", out valor1);

                if (!string.IsNullOrEmpty(valor1))
                {
                    uccUaEscola.DdlUA.SelectedValue = valor1;
                }

                if (valor1 != Guid.Empty.ToString())
                {
                    uccUaEscola.CarregaEscolaPorUASuperiorSelecionada();
                    SelecionarEscola();
                }
            }
            else
            {
                SelecionarEscola();
            }

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
    private void SelecionarEscola()
    {
        string uni_id;
        string esc_id;

        if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)) &&
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id))
        {
            uccUaEscola.SelectedValueEscolas = new int[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
            uccUaEscola.EnableEscolas = !(uccUaEscola.VisibleUA && uccUaEscola.Uad_ID == Guid.Empty);
        }
    }

    #endregion

    #region Eventos

    protected void odsConsultaEquipamentos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!IsPostBack)
        {
            // Cancela o select se for a primeira entrada na tela.
            e.Cancel = true;
        }
    }

    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        updPequisa.Update();
        Pesquisar();
    }


    protected void grvConsultaLogTablets_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = SYS_EquipamentoBO.GetTotalRecords();

        // seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(grvConsultaLogTablets);
    }

    protected void _btnExportar_Click(object sender, EventArgs e)
    {
        try
        {
            MSTech.Web.Util.GeraHTML.GeraHTML gera = new MSTech.Web.Util.GeraHTML.GeraHTML
                                                            {
                                                                _FileName = NomeModulo + " " + DateTime.Now.ToString("dd_MM_yyyy"),
                                                                _FileExtension = ".xls",
                                                                _Encoding = Encoding.GetEncoding("ISO-8859-1")
                                                            };

            HtmlGenericControl div = new HtmlGenericControl("div");
            HtmlTable table = new HtmlTable();
            HtmlTableCell tdNumSerie;
            HtmlTableCell tdDataEnvio;
            HtmlTableCell tdVersaoAPP;
            HtmlTableCell tdVersaoSO;

            /*** Cabeçalho ***/
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tdUnidadeAdministrativa = new HtmlTableCell
                {
                    InnerHtml = "Diretoria regional de educação: <b>" + uccUaEscola.ValorComboUA + "</b>",
                    ColSpan = 4
                };
                tdUnidadeAdministrativa.Style.Add("text-align", "center");
                tdUnidadeAdministrativa.Style.Add("width", "600");
                tr.Cells.Add(tdUnidadeAdministrativa);
                table.Rows.Add(tr);

            tr = new HtmlTableRow();

            HtmlTableCell tdEscola = new HtmlTableCell
                {
                    InnerHtml = "Escola: <b>" + uccUaEscola.ValorComboEscola + "</b>",
                    ColSpan = 4
                };
                tdEscola.Style.Add("text-align", "center");
                tdEscola.Style.Add("width", "600");
                tr.Cells.Add(tdEscola);
                table.Rows.Add(tr);
            /*** Fim cabeçalho ***/

           /*** Descrição |Nº Serie|Data de envio|Versão APP|Versão SO| ***/
            if (grvConsultaLogTablets.Rows.Count > 0)
            {
                tr = new HtmlTableRow();

                tdNumSerie = new HtmlTableCell { InnerText = "Nº Serie" };
                tdNumSerie.Style.Add("text-align", "center");
                tdNumSerie.Style.Add("background-color", "#000000");
                tdNumSerie.Style.Add("color", "#FFFFFF");                
                tr.Cells.Add(tdNumSerie);

                tdDataEnvio = new HtmlTableCell { InnerText = "Data de envio" };
                tdDataEnvio.Style.Add("text-align", "center");
                tdDataEnvio.Style.Add("background-color", "#000000");
                tdDataEnvio.Style.Add("color", "#FFFFFF");
                tr.Cells.Add(tdDataEnvio);

                tdVersaoAPP = new HtmlTableCell { InnerText = "Versão APP" };
                tdVersaoAPP.Style.Add("text-align", "center");
                tdVersaoAPP.Style.Add("background-color", "#000000");
                tdVersaoAPP.Style.Add("color", "#FFFFFF");
                tr.Cells.Add(tdVersaoAPP);

                tdVersaoSO = new HtmlTableCell { InnerText = "Versão SO" };
                tdVersaoSO.Style.Add("text-align", "center");
                tdVersaoSO.Style.Add("background-color", "#000000");
                tdVersaoSO.Style.Add("color", "#FFFFFF");
                tr.Cells.Add(tdVersaoSO);
                tr.Style.Add("font-weight", "bold");
                table.Rows.Add(tr);
            /***  Fim descrição ***/

            /***  Registros da pesquisa ***/
                DataTable dt = new DataTable();
                dt = SYS_EquipamentoBO.SelectLogTabletEquipamento(uccUaEscola.Esc_ID, uccUaEscola.Uad_ID);

                bool linha = true;
                foreach (DataRow row in dt.Rows)
                {
                    
                    tr = new HtmlTableRow();

                    tdNumSerie = new HtmlTableCell { InnerText = row.ItemArray[2].ToString() };
                    tdNumSerie.Style.Add("text-align", "center");
                    if (tdVersaoAPP.InnerText == null)
                    {
                        tdVersaoAPP.Style.Add("text", "-");
                    }
                    if (!linha)
                    {
                        tdNumSerie.Style.Add("background-color", "#D9D9D9");// linha colorida quando a linha for (!linha), pinta a linha de cinza
                    }
                    tdNumSerie.Style.Add("border-style", "solid");
                    tdNumSerie.Style.Add("border-width", "thin");
                    tr.Cells.Add(tdNumSerie);

                    tdDataEnvio = new HtmlTableCell { InnerText = row.ItemArray[3].ToString() };
                    tdDataEnvio.Style.Add("text-align", "center");
                    if (!linha)
                    {
                        tdDataEnvio.Style.Add("background-color", "#D9D9D9");
                    }
                    tdDataEnvio.Style.Add("border-style", "solid");
                    tdDataEnvio.Style.Add("border-width", "thin");
                    tr.Cells.Add(tdDataEnvio);

                    tdVersaoAPP = new HtmlTableCell { InnerText = row.ItemArray[4].ToString() };
                    tdVersaoAPP.Style.Add("text-align", "center");
                    if (!linha)
                    {
                        tdVersaoAPP.Style.Add("background-color", "#D9D9D9");
                    }
                    tdVersaoAPP.Style.Add("border-style", "solid");
                    tdVersaoAPP.Style.Add("border-width", "thin");
                    tr.Cells.Add(tdVersaoAPP);

                    tdVersaoSO = new HtmlTableCell { InnerText = row.ItemArray[5].ToString() };
                    tdVersaoSO.Style.Add("text-align", "center");
                    if (!linha)
                    {
                        tdVersaoSO.Style.Add("background-color", "#D9D9D9");
                    }
                    tdVersaoSO.Style.Add("border-style", "solid");
                    tdVersaoSO.Style.Add("border-width", "thin");
                    tr.Cells.Add(tdVersaoSO);

                    table.Rows.Add(tr);

                    linha = !linha;
                }
            /*** Fim registros da pesquisa ***/

                    table.Style.Add("border-style", "solid");
                    table.Style.Add("border-width", "3px");
                    div.Controls.Add(table);

                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    div.RenderControl(hw);
                    gera._Add(sw.ToString());
                    gera._GenerateForDownload();
                // na linha acima ele gera uma exception, porém é assim mesmo e ela cai no ThreadAbortException abaixo para gerar o relatorio.
            }      
        }
        catch (Exception ex)
        {
            if (!(ex is System.Threading.ThreadAbortException))
                lblMensagemErro.Text = UtilBO.GetErroMessage("Ocorreu um erro ao exportar para excel.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Eventos

}