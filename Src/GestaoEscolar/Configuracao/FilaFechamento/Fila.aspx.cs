using DevExpress.XtraReports.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.FilaFechamento
{
    public partial class Fila : MotherPageLogado
    {
        #region Atributos/Propriedades
        public int VS_TPC_ID
        {
            get
            {
                return Convert.ToInt32((ViewState["VS_TPC_ID"] ?? string.Empty).ToString());
            }
            set
            {
                ViewState["VS_TPC_ID"] = value;
            }
        }
        public int VS_TUD_ID
        {
            get
            {
                return Convert.ToInt32((ViewState["VS_TUD_ID"] ?? string.Empty).ToString());
            }
            set
            {
                ViewState["VS_TUD_ID"] = value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Gerar fila para nota e frequência
        /// </summary>
        public void Gerar()
        {
            try
            {
                foreach (GridViewRow item in grvFilaFechamento.Rows)
                {
                    VS_TUD_ID = Convert.ToInt32(grvFilaFechamento.DataKeys[item.RowIndex].Values["tud_id"]);
                    CheckBox chk = (CheckBox)item.FindControl("chkItemGerarFilaNota");
                    if (chk.Checked)
                    {
                        //chamar NEW_CLS_AlunoFechamentoPendencia_SalvarFilaNota // tpc_id e tud_id
                        if (!CLS_AlunoFechamentoPendenciaBO.SalvarFilaNota(VS_TUD_ID, VS_TPC_ID))
                        {
                            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar fila.",
                                UtilBO.TipoMensagem.Erro);
                        }
                    }
                    chk = (CheckBox)item.FindControl("chkItemGerarFilaFrequencia");
                    if (chk.Checked)
                    {
                        //chamar NEW_CLS_AlunoFechamentoPendencia_SalvarFilaFrequencia // tpc_id e tud_id
                        if (!CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(VS_TUD_ID, VS_TPC_ID))
                        {
                            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar fila.",
                                UtilBO.TipoMensagem.Erro);
                        }
                    }
                }
                lblMessage.Text = UtilBO.GetErroMessage("Fila gerada com sucesso.",
                    UtilBO.TipoMensagem.Sucesso);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar fila.",
                    UtilBO.TipoMensagem.Erro);
            }
        }

        public void Pesquisar()
        {
            try
            {
                odsFilaFechamento.SelectParameters.Clear();
                odsFilaFechamento.SelectParameters.Add("tur_id", UCComboTurma.Valor[0].ToString());
                odsFilaFechamento.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                odsFilaFechamento.SelectParameters.Add("mostraFilhosRegencia", "false");
                odsFilaFechamento.SelectParameters.Add("mostraRegencia", "true");
                odsFilaFechamento.SelectParameters.Add("mostraExperiencia", "true");
                odsFilaFechamento.SelectParameters.Add("mostraTerritorio", "false");
                odsFilaFechamento.SelectParameters.Add("cap_id", "-1");
                odsFilaFechamento.SelectParameters.Add("paginado", "true");


                grvFilaFechamento.PageIndex = 0;
                grvFilaFechamento.PageSize = UCComboQtdePaginacao1.Valor;

                fdsResultado.Visible = true;

                // Limpar a ordenação realizada.
                //grvFilaFechamento.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                Dictionary<string, string> filtros = odsFilaFechamento.SelectParameters.Cast<Parameter>().ToDictionary(param => param.Name, param => param.DefaultValue);

                // Salvar UA Superior.            
                if (UCComboUAEscola.FiltroEscola)
                {
                    filtros.Add("ua_superior", UCComboUAEscola.Uad_ID.ToString());
                    filtros.Add("esc_uni_id", UCComboUAEscola.DdlEscola.SelectedValue);
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.ParametrizacaoDisciplinas, Filtros = filtros };

                #endregion Salvar busca realizada com os parâmetros do ODS.

                //Atualiza o grid
                grvFilaFechamento.DataBind();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (__SessionWEB.__UsuarioWEB == null ||
                __SessionWEB.__UsuarioWEB.Usuario == null ||
                __SessionWEB.__UsuarioWEB.Grupo == null ||
                __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao)
            {
                Response.Redirect("~/logout.ashx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                //sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsFilaFilaFechamento.js"));
            }

            if (!IsPostBack)
            {
                UCComboUAEscola.Inicializar();
                UCCCalendario.CarregarCalendarioAnual();
            }

            // Seta o metodo criado no delegate ao evento do componente
            UCComboUAEscola.IndexChangedUnidadeEscola += UCComboUAEscola_IndexChangedUnidadeEscola;

            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;
            UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;
            UCComboCurriculoPeriodo._OnSelectedIndexChange += UCComboCurriculoPeriodo__OnSelectedIndexChange;
            UCCPeriodoCalendario.IndexChanged += UCCPeriodoCalendario_IndexChanged;
        }
                
        protected void grvFilaFechamento_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = TUR_TurmaDisciplinaBO.GetTotalRecords();

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvFilaFechamento);

            if ((!string.IsNullOrEmpty(grvFilaFechamento.SortExpression)) && (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ParametrizacaoDisciplinas))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = grvFilaFechamento.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", grvFilaFechamento.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = grvFilaFechamento.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", grvFilaFechamento.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.ParametrizacaoDisciplinas
                    ,
                    Filtros = filtros
                };
            }
        }

        protected void odsFilaFechamento_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Inicializa variável de sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Fila.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
            VS_TPC_ID = UCCPeriodoCalendario.Valor[0];
        }

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Gerar();
        }
        #endregion

        #region Delegates

        public delegate void SelectedIndexChangedPeriodoCalendario();
        public event SelectedIndexChangedPeriodoCalendario IndexChanged_PeriodoCalendario;
        public delegate void SelectedIndexChangedCalendario();
        public event SelectedIndexChangedCalendario IndexChanged_Calendario;
        public delegate void SelectedIndexChangedCurriculoPeriodo();
        public event SelectedIndexChangedCurriculoPeriodo IndexChanged_CurriculoPeriodo;

        /// <summary>
        /// Verifica alteracao do index do combo Escola e trata o combo curso
        /// </summary>
        public void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                UCCCursoCurriculo.Valor = new[] { -1, -1 };
                UCCCursoCurriculo.PermiteEditar = false;

                if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                {
                    UCCCursoCurriculo.CarregarPorEscola(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID);
                    UCCCursoCurriculo.SetarFoco();
                    UCCCursoCurriculo.PermiteEditar = true;
                }

                if (UCCCursoCurriculo.PermiteEditar)
                    UCCCursoCurriculo_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo cursocurriculo e trata o combo curriculoperiodo
        /// </summary>
        public void UCCCursoCurriculo_IndexChanged()
        {
            try
            {
                UCComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                UCComboCurriculoPeriodo.PermiteEditar = false;

                if (UCCCursoCurriculo.Valor[0] > 0 && UCCCursoCurriculo.Valor[1] > 0)
                {
                    UCComboCurriculoPeriodo._Load(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);
                    UCComboCurriculoPeriodo.FocaCombo();
                    UCComboCurriculoPeriodo.PermiteEditar = true;
                    UCComboCurriculoPeriodo__OnSelectedIndexChange();
                }
                if (UCComboCurriculoPeriodo.PermiteEditar)
                    UCComboCurriculoPeriodo__OnSelectedIndexChange();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo curriculoperiodo e trata o combo calendário
        /// </summary>
        public void UCComboCurriculoPeriodo__OnSelectedIndexChange()
        {
            try
            {
                UCCCalendario.Valor = -1;
                UCCCalendario.PermiteEditar = false;
                if (UCComboCurriculoPeriodo.Valor[0] > 0)
                {
                    UCCCalendario.CarregarPorCurso(UCCCursoCurriculo.Valor[0]);
                    UCCCalendario.SetarFoco();
                    UCCCalendario.PermiteEditar = true;
                }
                if (UCCCalendario.PermiteEditar)
                    UCCCalendario_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo calendario e trata o combo periodocalendario
        /// </summary>
        public void UCCCalendario_IndexChanged()
        {
            try
            {
                UCCPeriodoCalendario.Valor = new int[] { -1, -1 };
                UCCPeriodoCalendario.PermiteEditar = false;

                if (UCCCalendario.Valor > 0)
                {
                    UCCPeriodoCalendario.CarregarPorCalendario(UCCCalendario.Valor);

                    UCCPeriodoCalendario.SetarFoco();
                    UCCPeriodoCalendario.PermiteEditar = true;
                }
                if (UCCPeriodoCalendario.PermiteEditar)
                    UCCPeriodoCalendario_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }


        }

        /// <summary>
        /// Verifica alteracao do index do combo periodocalendario e trata o combo turma
        /// </summary>
        private void UCCPeriodoCalendario_IndexChanged()
        {

            try
            {
                UCComboTurma.Valor = new long[] { -1, -1, -1 };
                if (UCCPeriodoCalendario.Valor[1] > 0)
                {
                    UCComboTurma.CarregaPorEscolaCurriculoPeriodoCalendario(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1], UCComboCurriculoPeriodo.Valor[2], UCCCalendario.Valor);
                    UCComboTurma.PermiteEditar = true;
                }

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }


        }

        /// <summary>
        /// Atualiza o grid de acordo com a paginação
        /// </summary>
        protected void UCComboQtdePaginacao_IndexChanged()
        {
            try
            {
                // Atribui nova quantidade de itens por página para o grid.
                grvFilaFechamento.PageSize = UCComboQtdePaginacao1.Valor;
                grvFilaFechamento.PageIndex = 0;
                // Atualiza o grid
                grvFilaFechamento.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.",
                    UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}