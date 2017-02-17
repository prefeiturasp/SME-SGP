using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.TipoDesempenhoAprendizado
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Retorna o id do tipo de desempenho aprendizado para editar
        /// </summary>
        public int Edit_tda_id
        {
            get
            {
                return Convert.ToInt32(grvTipoAprendizando.DataKeys[grvTipoAprendizando.EditIndex].Values[0] ?? 0);
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.TipoDesempenhoAprendizado)
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
        /// Guarda o SortDirection da coluna ordenada
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.TipoDesempenhoAprendizado)
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

        #region Constantes

        private const int INDEX_COLUNA_CURRICULO_PERIODO = 1;

        #endregion Constantes

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvTipoAprendizando.PageSize = UCComboQtdePaginacao1.Valor;
            grvTipoAprendizando.PageIndex = 0;
            // atualiza o grid
            grvTipoAprendizando.DataBind();
        }

        protected void UCCCalendario_IndexChanged()
        {
            try
            {
                UCCCursoCurriculo.PermiteEditar = false;

                UCCCursoCurriculo.Valor = new int[2] { -1, -1 };

                if (UCCCalendario.Valor > 0)
                {
                    UCCCursoCurriculo.CarregarPorEscolaCalendarioSituacaoCurso(-1, -1, UCCCalendario.Valor, 1);
                    UCCCursoCurriculo.SetarFoco();
                    UCCCursoCurriculo.PermiteEditar = true;
                }

                UCCCursoCurriculo_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do calendario.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCCCursoCurriculo_IndexChanged()
        {
            try
            {
                UCCCurriculoPeriodo.Valor = new[] { -1, -1 };

                UCCCurriculoPeriodo.PermiteEditar = false;

                if (UCCCursoCurriculo.Valor[0] > 0 && UCCCursoCurriculo.Valor[1] > 0)
                {
                    UCCCurriculoPeriodo.CarregarPorCursoCurriculo(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);

                    UCCCurriculoPeriodo.SetarFoco();
                    UCCCurriculoPeriodo.PermiteEditar = true;

                }

                UCCCurriculoPeriodo_IndexChanged();
            }
            catch (Exception ex)
            {
                
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do(a) " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_CADASTRO_CURSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) + ".", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCCCurriculoPeriodo_IndexChanged()
        {
            try
            {
                UCComboTipoDisciplina.Valor = -1;
                UCComboTipoDisciplina.PermiteEditar = false;

                if (UCCCurriculoPeriodo.Valor[0] > 0)
                {
                    UCComboTipoDisciplina.CarregarTipoDisciplinaPorCursoCurriculoPeriodo(UCCCurriculoPeriodo.Valor[0], UCCCurriculoPeriodo.Valor[1], UCCCurriculoPeriodo.Valor[2]);
                    UCComboTipoDisciplina.SetarFoco();
                    UCComboTipoDisciplina.PermiteEditar = true;

                    if (UCComboTipoDisciplina._Combo.Items.Count > 1)
                    {  // somente adiciono essa opcao caso exista pelo menos uma disciplina cadastrada.
                        UCComboTipoDisciplina._Combo.Items.RemoveAt(0); // remove o selecione
                        UCComboTipoDisciplina._Combo.Items.Insert(0, new ListItem("Todos", "0", true));
                    }
                }
                //UCCCalendario_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do(a) " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_PERIODO_CURSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) + ".", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            // Seta delegates
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;
            UCCCurriculoPeriodo.IndexChanged += UCCCurriculoPeriodo_IndexChanged;
            UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                grvTipoAprendizando.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsTipoAprendizando.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsTipoAprendizando.ClientID)), true);
                    }

                    //Inicializa os campos de busca
                    InicializaCamposBusca();

                    //Carrega uma busca salva na memoria
                    VerificaBusca();

                    UCComboTipoDisciplina._Combo.Items.RemoveAt(0); // remove o selecione
                    UCComboTipoDisciplina._Combo.Items.Insert(0, new ListItem("Todos", "0", true));

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = _btnPesquisar.UniqueID;
                Page.Form.DefaultFocus = UCCCalendario.ClientID;

                // Permissões da pagina
                divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa);
            }
        }

        protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
        }

        protected void _btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Pesquisar();
            }
        }

        protected void _grvTipoAprendizando_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_TipoDesempenhoAprendizadoBO.GetTotalRecords();

            // seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvTipoAprendizando);

            if ((!string.IsNullOrEmpty(grvTipoAprendizando.SortExpression)) &&
                (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.TipoDesempenhoAprendizado))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = grvTipoAprendizando.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", grvTipoAprendizando.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = grvTipoAprendizando.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", grvTipoAprendizando.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.TipoDesempenhoAprendizado
                    ,
                    Filtros = filtros
                };
            }
        }

        protected void _grvTipoAprendizando_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa);
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void _grvTipoAprendizando_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int tda_id = Convert.ToInt32(grvTipoAprendizando.DataKeys[index].Values[0]);

                    ACA_TipoDesempenhoAprendizado entity = new ACA_TipoDesempenhoAprendizado { tda_id = tda_id };
                    ACA_TipoDesempenhoAprendizadoBO.GetEntity(entity);

                    if (ACA_TipoDesempenhoAprendizadoBO.Delete(entity))
                    {
                        grvTipoAprendizando.PageIndex = 0;
                        grvTipoAprendizando.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tda_id: " + tda_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Tipo de " + GetGlobalResourceObject("Mensagens","MSG_DESEMPENHO") + " excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir tipo de " + GetGlobalResourceObject("Mensagens","MSG_DESEMPENHO") + ".", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Realiza a consulta pelos filtros informados.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                fdsResultados.Visible = true;

                Dictionary<string, string> filtros = new Dictionary<string, string>();

                odsTipoAprendizando.SelectParameters.Clear();

                grvTipoAprendizando.PageIndex = 0;
                odsTipoAprendizando.SelectParameters.Clear();
                odsTipoAprendizando.SelectParameters.Add("cal_id", UCCCalendario.Valor.ToString());
                odsTipoAprendizando.SelectParameters.Add("cur_id", UCCCursoCurriculo.Valor[0].ToString());
                odsTipoAprendizando.SelectParameters.Add("crr_id", UCCCursoCurriculo.Valor[1].ToString());
                odsTipoAprendizando.SelectParameters.Add("crp_id", UCCCurriculoPeriodo.Valor[2].ToString());
                odsTipoAprendizando.SelectParameters.Add("tds_id", UCComboTipoDisciplina.Valor.ToString());

                //odsTipoAprendizando.DataBind();

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                grvTipoAprendizando.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                foreach (Parameter param in odsTipoAprendizando.SelectParameters)
                {
                    filtros.Add(param.Name, param.DefaultValue);
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.TipoDesempenhoAprendizado
                    ,
                    Filtros = filtros
                };

                #endregion

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvTipoAprendizando.PageSize = itensPagina;
                // atualiza o grid

                if (grvTipoAprendizando.Rows.Count > 0)
                {
                    grvTipoAprendizando.HeaderRow.Cells[INDEX_COLUNA_CURRICULO_PERIODO].Text = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }
                
                grvTipoAprendizando.DataBind();

                //updResultado.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar tipo de " + GetGlobalResourceObject("Mensagens","MSG_DESEMPENHO") + ".", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca
        /// </summary>
        private void InicializaCamposBusca()
        {
            // Todo verificar com o juliano se ira carregar por ano
            UCCCalendario.Carregar();
            UCComboTipoDisciplina.CarregarTipoDisciplina();
            UCComboTipoDisciplina._Combo.SelectedValue = "-1";
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            try
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.TipoDesempenhoAprendizado)
                {
                    // Recuperar busca realizada e pesquisar automaticamente

                    string valor, valor2, valor3;

                    //Calendario
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                    UCCCalendario.Valor = Convert.ToInt32(valor);
                    UCCCalendario_IndexChanged();
                    
                    //Etapa de ensino
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
                    UCCCursoCurriculo.Valor = new int[2] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                    UCCCursoCurriculo_IndexChanged();

                    //Grupamento
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor3);
                    UCCCurriculoPeriodo.Valor = new int[3] { Convert.ToInt32(valor), Convert.ToInt32(valor2), Convert.ToInt32(valor3) };
                    UCCCurriculoPeriodo_IndexChanged();

                    //Disciplina
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tds_id", out valor);
                    UCComboTipoDisciplina.Valor = Convert.ToInt32(valor);

                    fdsResultados.Visible = true;

                    Pesquisar();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar tipo de " + GetGlobalResourceObject("Mensagens","MSG_DESEMPENHO") + ".", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

    }
}