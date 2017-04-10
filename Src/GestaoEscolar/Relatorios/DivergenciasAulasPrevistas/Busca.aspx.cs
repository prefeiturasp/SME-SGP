using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace GestaoEscolar.Relatorios.DivergenciasAulasPrevistas
{
    public partial class Busca : MotherPageLogado
    {
        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                pnlBusca.GroupingText = GestaoEscolarUtilBO.BuscaNomeModulo("~/Relatorios/DivergenciasAulasPrevistas/Busca.aspx", "Divergências entre aulas criadas e previstas", __SessionWEB.__UsuarioWEB.Grupo.gru_id);
                InicializaCombos();
                CarregarBusca();
            }

            UCComboUAEscola.IndexChangedUA += UCComboUAEscola_IndexChangedUA;
            UCComboUAEscola.IndexChangedUnidadeEscola += UCComboUAEscola_IndexChangedUnidadeEscola;
            UCCCalendario1.IndexChanged += UCCCalendario1_IndexChanged;
        }

        #endregion Eventos Page Life Cycle

        #region Delegates

        protected void UCComboUAEscola_IndexChangedUA()
        {
            if (UCComboUAEscola.VisibleUA)
            {
                UCComboUAEscola.MostraApenasAtivas = true;

                UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

                UCComboUAEscola.EnableEscolas = (UCComboUAEscola.Uad_ID != Guid.Empty || !UCComboUAEscola.DdlUA.Visible);
            }

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
            {
                try
                {
                    UCCCalendario1.Valor = -1;
                    UCCCalendario1.PermiteEditar = false;

                    if (UCComboUAEscola.Uad_ID != Guid.Empty || !UCComboUAEscola.DdlUA.Visible)
                    {
                        UCCCalendario1.CarregarCalendarioAnual();
                        UCCCalendario1.SetarFoco();
                        UCCCalendario1.PermiteEditar = true;
                    }

                    UCCCalendario1_IndexChanged();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
            else
                UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                UCCCalendario1.CarregarCalendarioAnual();
                UCCCalendario1.SetarFoco();
                UCCCalendario1.PermiteEditar = true;

                UCCCalendario1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

        }

        protected void UCCCalendario1_IndexChanged()
        {
            UCCPeriodoCalendario1.Valor = new int[2] { -1, -1 };
            UCCPeriodoCalendario1.PermiteEditar = false;

            if (UCCCalendario1.Valor > 0)
            {
                UCCPeriodoCalendario1.CarregarPorCalendario(UCCCalendario1.Valor);
                UCCPeriodoCalendario1.SetarFoco();
                UCCPeriodoCalendario1.PermiteEditar = true;
            }
        }

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Inicializa os filtros da pagina.
        /// </summary>
        private void InicializaCombos()
        {
            UCComboUAEscola.Inicializar();
            UCComboUAEscola_IndexChangedUA();

            UCComboUAEscola.ObrigatorioEscola = !(__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao);
            UCComboUAEscola.ObrigatorioUA = UCComboUAEscola.VisibleUA;
        }

        /// <summary>
        /// Método para salvar os filtros última busca realizada
        /// </summary>
        protected void SalvaBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("uad_idSuperior", UCComboUAEscola.Uad_ID.ToString());
            filtros.Add("esc_id", UCComboUAEscola.Esc_ID.ToString());
            filtros.Add("uni_id", UCComboUAEscola.Uni_ID.ToString());
            filtros.Add("cal_id", UCCCalendario1.Valor.ToString());
            filtros.Add("tpc_id", UCCPeriodoCalendario1.Valor[0].ToString());
            filtros.Add("cap_id", UCCPeriodoCalendario1.Valor[1].ToString());
            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.DivergenciasAulasPrevistas, Filtros = filtros };
        }

        /// <summary>
        /// Método carrega os filtros última busca realizada
        /// </summary>
        protected void CarregarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.DivergenciasAulasPrevistas)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor2;

                long doc_id = -1;
                UCComboUAEscola.Inicializar();

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    UCComboUAEscola.InicializarVisaoIndividual(doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    string esc_id;
                    string uni_id;

                    if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                        (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                    {
                        UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                        UCComboUAEscola_IndexChangedUnidadeEscola();
                    }
                }
                else
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                    if (UCComboUAEscola.VisibleUA && !string.IsNullOrEmpty(valor))
                    {
                        UCComboUAEscola.Uad_ID = new Guid(valor);

                        if (UCComboUAEscola.Uad_ID != Guid.Empty)
                        {
                            UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();
                            UCComboUAEscola.FocoEscolas = true;
                            UCComboUAEscola.PermiteAlterarCombos = true;
                        }
                    }

                    string esc_id;
                    string uni_id;

                    if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                        (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                    {
                        UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                        UCComboUAEscola_IndexChangedUnidadeEscola();
                    }
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCCCalendario1.Valor = Convert.ToInt32(valor);
                UCCCalendario1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tpc_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cap_id", out valor2);
                UCCPeriodoCalendario1.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
            }
        }

        #endregion Métodos

        #region Eventos

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    SalvaBusca();
                    string parameter = string.Empty;
                    string report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.DivergenciasAulasPrevistas).ToString();

                    parameter = "uad_idSuperiorGestao=" + (UCComboUAEscola.Uad_ID.ToString() != new Guid().ToString() ? UCComboUAEscola.Uad_ID.ToString() : string.Empty)
                                + "&esc_id=" + UCComboUAEscola.Esc_ID.ToString()
                                + "&cal_id=" + UCCCalendario1.Valor.ToString()
                                + "&tpc_id=" + UCCPeriodoCalendario1.Valor[0].ToString()
                                + "&gru_id=" + __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString()
                                + "&usu_id=" + __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString()
                                + "&adm=" + (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
                                + "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio")
                                + "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria")
                                + "&logo=" + string.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS)
                                + "&cal_ano=" + UCCCalendario1.Cal_ano.ToString()
                                + "&cap_descricao=" + UCCPeriodoCalendario1.Texto;

                    CFG_RelatorioBO.CallReport("Relatorios", report, parameter, HttpContext.Current);
                }
            }
            catch (Exception error)
            {
                ApplicationWEB._GravaErro(error);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar relatório de divergências entre aulas criadas e previstas.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos
    }
}