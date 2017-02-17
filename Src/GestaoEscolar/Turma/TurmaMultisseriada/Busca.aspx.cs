using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Turma.TurmaMultisseriada
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        private const int IndiceColunaCurso = 3;

        #endregion

        #region Propriedades

        /// <summary>
        /// Propriedade na qual retorna o valor de tur_id - Primeiro DataKeyName da GridView de Turma
        /// </summary>
        public long Edit_tur_id
        {
            get
            {
                return Convert.ToInt64(gvTurma.DataKeys[gvTurma.EditIndex].Values["tur_id"]);
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.TurmaMultisseriada)
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
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.TurmaMultisseriada)
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

        #region Métodos

        /// <summary>
        /// Configura valores iniciais nos user controls da tela.
        /// </summary>
        private void InicializarUserControls()
        {
            uccFiltroEscola.Inicializar();
            uccDisciplina.Texto = GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " multisseriada";

            uccCurriculoPeriodo._MostrarMessageSelecione = true;
            uccCurriculoPeriodo._Combo.Enabled = false;

            uccDocente._MostrarMessageSelecione = true;
            uccDocente.PermiteEditar = false;

            if (uccFiltroEscola.Esc_ID > 0)
            {
                string esc_uni_id = uccFiltroEscola.Esc_ID.ToString() + ";" + uccFiltroEscola.Uni_ID.ToString();
                uccDocente._CancelaSelect = false;
                uccDocente._Load_By_esc_uni_id(esc_uni_id, 1);
                uccDocente.PermiteEditar = true;
            }
            // Carrega todos os turnos no combo, não tem como buscar pelo controle de tempo.
            uccTurno.CarregarTurnoPorTipoTurno();
        }

        /// <summary>
        /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
        /// </summary>
        public void _Pesquisar()
        {
            try
            {
                fdsResultado.Visible = true;

                Dictionary<string, string> filtros = new Dictionary<string, string>();

                gvTurma.PageIndex = 0;
                odsTurma.SelectParameters.Clear();
                odsTurma.SelectParameters.Add("uad_idSuperior", uccFiltroEscola.Uad_ID.ToString());
                odsTurma.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                odsTurma.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                odsTurma.SelectParameters.Add("esc_id", uccFiltroEscola.Esc_ID.ToString());
                odsTurma.SelectParameters.Add("uni_id", uccFiltroEscola.Uni_ID.ToString());
                odsTurma.SelectParameters.Add("cal_id", uccCalendario.Valor.ToString());
                odsTurma.SelectParameters.Add("cur_id", uccCursoCurriculo.Valor[0].ToString());
                odsTurma.SelectParameters.Add("crr_id", uccCursoCurriculo.Valor[1].ToString());
                odsTurma.SelectParameters.Add("crp_id", uccCurriculoPeriodo.Valor[2].ToString());
                odsTurma.SelectParameters.Add("doc_id", uccDocente.Doc_id.ToString());
                odsTurma.SelectParameters.Add("ttn_id", uccTurno.Valor.ToString());
                odsTurma.SelectParameters.Add("dis_id", uccDisciplina.Valor.ToString());
                odsTurma.SelectParameters.Add("tur_codigo", txtCodigoTurma.Text);
                odsTurma.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                // odsTurma.SelectParameters.Add("paginado", "false");

                // quantidade de itens por página            
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                uccQtdePaginacao.Valor = itensPagina;
                // atribui essa quantidade para o grid
                gvTurma.PageSize = itensPagina;
                // atualiza o grid

                gvTurma.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                foreach (Parameter param in odsTurma.SelectParameters)
                {
                    filtros.Add(param.Name, param.DefaultValue);
                }
                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.TurmaMultisseriada
                    ,
                    Filtros = filtros
                };

                #endregion

                gvTurma.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as turmas.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta, 
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.TurmaMultisseriada)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;
                string valor2;
                string valor3;

                if (uccFiltroEscola.VisibleUA)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);

                    if (!string.IsNullOrEmpty(valor))
                    {
                        uccFiltroEscola.Uad_ID = new Guid(valor);
                        //lblMessage.Text = UtilBO.GetErroMessage(valor, UtilBO.TipoMensagem.Erro);
                    }

                    if (valor != Guid.Empty.ToString())
                    {
                        SelecionarEscola(uccFiltroEscola.VisibleUA);
                    }
                }
                else
                {
                    SelecionarEscola(uccFiltroEscola.VisibleUA);
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                uccCalendario.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
                uccCursoCurriculo.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                UCComboCursoCurriculo1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor3);
                if (Convert.ToInt32(valor3) > 0)
                    uccCurriculoPeriodo._Combo.SelectedValue = valor + ";" + valor2 + ";" + valor3;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                uccCalendario.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor);
                uccTurno.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("dis_id", out valor);
                uccDisciplina.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("doc_id", out valor);
                if (Convert.ToInt64(valor) > 0)
                    uccDocente.Doc_id = Convert.ToInt64(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_codigo", out valor);
                txtCodigoTurma.Text = valor;

                _Pesquisar();
            }
            else
            {
                fdsResultado.Visible = false;
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
            {
                uccFiltroEscola.IndexChangedUA += uccFiltroEscola_IndexChangedUA;
            }

            string esc_id;
            string uni_id;

            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                uccFiltroEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                uccFiltroEscola.IndexChangedUnidadeEscola += uccFiltroEscola_IndexChangedUnidadeEscola;

                uccFiltroEscola.EnableEscolas = !(uccFiltroEscola.VisibleUA && uccFiltroEscola.Uad_ID == Guid.Empty);
                uccFiltroEscola_IndexChangedUnidadeEscola();
            }
        }

        /// <summary>
        /// Seta os delegates.
        /// </summary>
        private void SetaDelegates()
        {
            uccFiltroEscola.IndexChangedUA += uccFiltroEscola_IndexChangedUA;
            uccFiltroEscola.IndexChangedUnidadeEscola += uccFiltroEscola_IndexChangedUnidadeEscola;
            uccCalendario.IndexChanged += UCComboCalendario1_IndexChanged;
            uccCursoCurriculo.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
            uccCurriculoPeriodo._OnSelectedIndexChange += uccCurriculoPeriodo__OnSelectedIndexChange;
        }

        #endregion

        #region Delegates

        protected void UCComboCalendario1_IndexChanged()
        {
            try
            {
                uccDisciplina.Valor = -1;
                uccDisciplina.PermiteEditar = false;

                uccCurriculoPeriodo._Combo.SelectedValue = "-1;-1;-1";
                uccCurriculoPeriodo._Combo.Enabled = false;

                uccCursoCurriculo.PermiteEditar = false;
                uccCursoCurriculo.Valor = new[] { Convert.ToInt32(-1), Convert.ToInt32(-1) };

                if (uccCalendario.Valor != -1)
                {
                    uccCursoCurriculo.PermiteEditar = true;
                    uccCursoCurriculo.CarregarCursoCurriculo();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            gvTurma.PageSize = uccQtdePaginacao.Valor;
            gvTurma.PageIndex = 0;
            // atualiza o grid
            gvTurma.DataBind();
        }

        protected void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                uccDisciplina.Valor = -1;
                uccDisciplina.PermiteEditar = false;

                uccCurriculoPeriodo._Combo.SelectedValue = "-1;-1;-1";
                uccCurriculoPeriodo._Combo.Enabled = false;

                int cur_id = uccCursoCurriculo.Valor[0];
                int crr_id = uccCursoCurriculo.Valor[1];

                if (cur_id > 0 && crr_id > 0)
                {
                    uccCurriculoPeriodo.PermiteEditar = true;
                    uccCurriculoPeriodo._Load(cur_id, crr_id);
                }
                else
                {
                    uccCurriculoPeriodo.Valor = new int[] { -1, -1 };
                    uccCurriculoPeriodo.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void uccCurriculoPeriodo__OnSelectedIndexChange()
        {
            try
            {
                int cur_id = uccCurriculoPeriodo.Valor[0];
                int crr_id = uccCurriculoPeriodo.Valor[1];
                int crp_id = uccCurriculoPeriodo.Valor[2];

                if (cur_id > 0 && crr_id > 0 && crp_id > 0)
                {
                    uccDisciplina.Valor = -1;
                    uccDisciplina.PermiteEditar = true;
                    // Carregar disciplinas do curso.
                    uccDisciplina.CarregarDisciplinasMultisseriadas
                        (cur_id, crr_id, crp_id, uccFiltroEscola.Esc_ID, uccFiltroEscola.Uni_ID);
                }
                else
                {
                    uccDisciplina.Valor = -1;
                    uccDisciplina.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void uccFiltroEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                uccDocente.Doc_id = -1;
                uccDocente.PermiteEditar = true;

                uccDisciplina.Valor = -1;
                uccDisciplina.PermiteEditar = false;

                uccCurriculoPeriodo._Combo.SelectedValue = "-1;-1;-1";
                uccCurriculoPeriodo._Combo.Enabled = false;

                uccCalendario.Valor = -1;
                uccCalendario.PermiteEditar = true;//para deixar o combo de disciplina sempre visivel.
                uccCalendario.CarregarCalendarioAnual();

                uccCursoCurriculo.PermiteEditar = false;
                uccCursoCurriculo.Valor = new[] { -1, -1, -1 };
                uccCursoCurriculo.CarregarCursoCurriculo();

                if (uccFiltroEscola.Esc_ID > 0)
                {
                    string esc_uni_id = uccFiltroEscola.Esc_ID.ToString() + ";" + uccFiltroEscola.Uni_ID.ToString();
                    uccDocente._CancelaSelect = false;
                    uccDocente._Load_By_esc_uni_id(esc_uni_id, 1);
                    uccDocente.PermiteEditar = true;

                    uccCalendario.CarregarCalendarioAnual();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void uccFiltroEscola_IndexChangedUA()
        {
            uccFiltroEscola_IndexChangedUnidadeEscola();
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            SetaDelegates();

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                ScriptManager.RegisterStartupScript(upnPesquisa, typeof(UpdatePanel), pnlTurma.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", pnlTurma.ClientID)), true);
            }

            uccQtdePaginacao.GridViewRelacionado = gvTurma;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                try
                {
                    InicializarUserControls();

                    gvTurma.Columns[IndiceColunaCurso].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    fdsResultado.Visible = false;

                    VerificaBusca();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = btnPesquisar.UniqueID;
                Page.Form.DefaultFocus = uccFiltroEscola.ComboUA_ClientID;

                uccCursoCurriculo.CarregarCursoCurriculo();
                uccCalendario.CarregarCalendarioAnual();
            }
        }

        protected void gvTurma_DataBound(object sender, EventArgs e)
        {
            ucTotalRegistros.Total = TUR_TurmaBO.GetTotalRecords();
            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(gvTurma);

            if ((!string.IsNullOrEmpty(gvTurma.SortExpression)) &&
          (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.TurmaMultisseriada))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = gvTurma.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", gvTurma.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = gvTurma.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", gvTurma.SortDirection.ToString());
                }
                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.TurmaMultisseriada
                    ,
                    Filtros = filtros
                };
            }
        }
        
        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Inicializa variável de sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("~/Turma/TurmaMultisseriada/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            _Pesquisar();
        }

        #endregion
    }
}
