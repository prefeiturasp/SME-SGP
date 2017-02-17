using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace GestaoEscolar.Academico.AberturaTurmasAnosAnteriores
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Recebe o Id para enviar os dados para edição.
        /// </summary>
        public int EditItem
        {
            get
            {
                return Convert.ToInt32(gdvAberturaAnosAnteriores.DataKeys[gdvAberturaAnosAnteriores.EditIndex].Value);
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

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                }

                UCComboQtdePaginacao.GridViewRelacionado = gdvAberturaAnosAnteriores;

                if (!IsPostBack)
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                        lblMessage.Text = message;

                    gdvAberturaAnosAnteriores.PageSize = ApplicationWEB._Paginacao;

                    CarregarComboStatus();
                    InicializarEscolas();

                    Page.Form.DefaultButton = btnPesquisar.UniqueID;
                    Page.Form.DefaultFocus = txtAno.ClientID;

                    btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }

                ucComboUAEscola.IndexChangedUA += UCFiltroEscolas1__Selecionar;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Busca.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            Pesquisar(0, itensPagina);
            fdsResultado.Visible = true;
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/AberturaTurmasAnosAnteriores/Busca.aspx", false);
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/AberturaTurmasAnosAnteriores/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void gdvAberturaAnosAnteriores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int tab_id = Convert.ToInt32(gdvAberturaAnosAnteriores.DataKeys[index].Values["tab_id"].ToString());

                    TUR_TurmaAberturaAnosAnteriores entity = new TUR_TurmaAberturaAnosAnteriores { tab_id = tab_id };
                    TUR_TurmaAberturaAnosAnterioresBO.GetEntity(entity);

                    if (TUR_TurmaAberturaAnosAnterioresBO.Delete(entity))
                    {
                        gdvAberturaAnosAnteriores.PageIndex = 0;
                        gdvAberturaAnosAnteriores.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tab_id: " + tab_id);
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Busca.AgendamentoExcluidoSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Busca.ErroExcluirAgendamento").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void gdvAberturaAnosAnteriores_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    bool permiteExcluir = true;
                   
                    short tab_status = Convert.ToInt16(gdvAberturaAnosAnteriores.DataKeys[e.Row.RowIndex].Values["tab_status"].ToString());
                    DateTime tab_dataFim = string.IsNullOrEmpty(gdvAberturaAnosAnteriores.DataKeys[e.Row.RowIndex].Values["tab_dataFim"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(gdvAberturaAnosAnteriores.DataKeys[e.Row.RowIndex].Values["tab_dataFim"].ToString());

                    if((tab_dataFim != DateTime.MinValue && tab_dataFim <= DateTime.Now.Date) || tab_status != (byte)TUR_TurmaAberturaAnosAnterioresBO.EnumTurmaAberturaAnosAnterioresStatus.AguardandoExecucao)
                    {
                        permiteExcluir = false;
                    }

                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && permiteExcluir;
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void gdvAberturaAnosAnteriores_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = TUR_TurmaAberturaAnosAnterioresBO.GetTotalRecords();
            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(gdvAberturaAnosAnteriores);

            if ((!string.IsNullOrEmpty(gdvAberturaAnosAnteriores.SortExpression)) &&
                 (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AberturaTurmasAnosAnteriores))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = gdvAberturaAnosAnteriores.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", gdvAberturaAnosAnteriores.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = gdvAberturaAnosAnteriores.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", gdvAberturaAnosAnteriores.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.AberturaTurmasAnosAnteriores
                    ,
                    Filtros = filtros
                };
            }
        }

        protected void odsAberturaAnosAnteriores_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa os combos.
        /// </summary>
        public void InicializarEscolas()
        {
            try
            {
                ucComboUAEscola.FocusUA();
                ucComboUAEscola.Inicializar();

                this.VerificaBusca();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Busca.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta,
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AberturaTurmasAnosAnteriores)
            {
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tab_ano", out valor);
                txtAno.Text = valor;
                
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);

                if (!string.IsNullOrEmpty(valor))
                {
                    ucComboUAEscola.Uad_ID = new Guid(valor);
                    UCFiltroEscolas1__Selecionar();
                }

                string valor2;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out valor2);

                if(!string.IsNullOrEmpty(valor) && !string.IsNullOrEmpty(valor2))
                ucComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tab_status", out valor);
                ddlStatus.SelectedValue = valor;

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                Pesquisar(0, itensPagina);
            }
            else
            {
                fdsResultado.Visible = false;
            }
        }

        /// <summary>
        /// Realiza a consulta pelos filtros informados.
        /// </summary>
        private void Pesquisar(int pageIndex, int pageSize)
        {
            try
            {
                Dictionary<string, string> filtros = new Dictionary<string, string>();

                gdvAberturaAnosAnteriores.PageIndex = pageIndex;
                odsAberturaAnosAnteriores.SelectParameters.Clear();
                odsAberturaAnosAnteriores.SelectParameters.Add("tab_ano", txtAno.Text);
                odsAberturaAnosAnteriores.SelectParameters.Add("uad_idSuperior", ucComboUAEscola.Uad_ID.ToString());
                odsAberturaAnosAnteriores.SelectParameters.Add("uni_id", ucComboUAEscola.Uni_ID.ToString());
                odsAberturaAnosAnteriores.SelectParameters.Add("esc_id", ucComboUAEscola.Esc_ID.ToString());
                odsAberturaAnosAnteriores.SelectParameters.Add("tab_status", ddlStatus.SelectedValue);

                gdvAberturaAnosAnteriores.Sort("", SortDirection.Ascending);
                // mostra essa quantidade no combobox            
                UCComboQtdePaginacao.Valor = pageSize;
                // atribui essa quantidade para o grid
                gdvAberturaAnosAnteriores.PageSize = pageSize;
                odsAberturaAnosAnteriores.DataBind();

                gdvAberturaAnosAnteriores.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                foreach (Parameter param in odsAberturaAnosAnteriores.SelectParameters)
                {
                    filtros.Add(param.Name, param.DefaultValue);
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.AberturaTurmasAnosAnteriores
                    ,
                    Filtros = filtros
                };

                #endregion
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Busca.ErroConsultarAgendamentos").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega o combo de status
        /// </summary>
        public void CarregarComboStatus()
        {
            Dictionary<byte, string> dic = new Dictionary<byte, string>();
            
            dic = (from Enum valor in Enum.GetValues(typeof(TUR_TurmaAberturaAnosAnterioresBO.EnumTurmaAberturaAnosAnterioresStatus))
                   select new
                   {
                       chave = (byte)((TUR_TurmaAberturaAnosAnterioresBO.EnumTurmaAberturaAnosAnterioresStatus)valor)
                       ,
                       valor = DescricaoEnum(valor)
                   }).ToDictionary(p => p.chave, p => p.valor);


            ddlStatus.Items.Clear();
            ddlStatus.DataTextField = "Value";
            ddlStatus.DataValueField = "Key";
            ddlStatus.Items.Add(new ListItem("-- Selecione um status --", "-1"));
            ddlStatus.DataSource = dic;
            ddlStatus.DataBind();
        }

        /// <summary>
        /// Retorna a descrição de um determinado elemento de um Enumerador.
        /// </summary>
        /// <param name="elemento">Elemento do enumerador de onde a descrição será retornada.</param>
        /// <returns>String com a descrição do elemento do Enumerador.</returns>
        public string DescricaoEnum(Enum elemento)
        {
            FieldInfo infoElemento = elemento.GetType().GetField(elemento.ToString());
            DescriptionAttribute[] atributos = (DescriptionAttribute[])infoElemento.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (atributos.Length > 0)
            {
                if (atributos[0].Description != null)
                {
                    return atributos[0].Description;
                }
            }

            return elemento.ToString();
        }

        #endregion

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            try
            {
                Pesquisar(gdvAberturaAnosAnteriores.PageIndex, UCComboQtdePaginacao.Valor);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Busca.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de UA Superior.
        /// </summary>
        private void UCFiltroEscolas1__Selecionar()
        {
            try
            {
                ucComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

                if (ucComboUAEscola.Uad_ID != Guid.Empty)
                {
                    ucComboUAEscola.FocoEscolas = true;
                    ucComboUAEscola.PermiteAlterarCombos = true;
                }

                ucComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "AberturaTurmasAnosAnteriores.Busca.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}