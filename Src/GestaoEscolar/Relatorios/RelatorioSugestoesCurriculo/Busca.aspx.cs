using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;

namespace GestaoEscolar.Relatorios.RelatorioSugestoesCurriculo
{
    public partial class Busca : MotherPageLogado
    {
        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                }

                UCComboTipoNivelEnsino1.IndexChanged += UCComboTipoNivelEnsino1_IndexChanged;
                UCComboTipoModalidadeEnsino1.IndexChanged += UCComboTipoModalidadeEnsino1_IndexChanged;

                if (!IsPostBack)
                {
                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        updFiltros.Visible = false;
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "RelatorioSugestoesCurriculo.Busca.lblMessage.ErroPermissao").ToString(), UtilBO.TipoMensagem.Alerta);
                    }

                    Page.Form.DefaultButton = btnGerar.UniqueID;

                    InicializarTela();
                    CarregarBusca();
                }
            }
            catch (Exception error)
            {
                ApplicationWEB._GravaErro(error);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;
                // Inserção do título do relatório
                pnlBusca.GroupingText = Modulo.mod_nome;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa os componentes da tela
        /// </summary>
        private void InicializarTela()
        {
            UCComboTipoNivelEnsino1.CarregarTipoNivelEnsino();
            UCComboTipoModalidadeEnsino1.CarregarTipoModalidadeEnsino();
            UCComboTipoCurriculoPeriodo1.CarregarPorNivelEnsinoModalidade(UCComboTipoNivelEnsino1.Valor, UCComboTipoModalidadeEnsino1.Valor);
            UCComboTipoDisciplina1.CarregarTipoDisciplinaPorNivelEnsino(UCComboTipoNivelEnsino1.Valor);
            txtDataInicio.Text = "";
            txtDataFim.Text = "";
            updFiltros.Update();
        }

        /// <summary>
        /// O método gera o relatório de alunos abaixo da frequência
        /// </summary>
        private void GerarRelatorio()
        {
            try
            {
                string report, parametros;

                DateTime dataInicio = new DateTime();
                DateTime dataFim = new DateTime();

                if (string.IsNullOrEmpty(txtDataInicio.Text) || !DateTime.TryParse(txtDataInicio.Text, out dataInicio))
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "RelatorioSugestoesCurriculo.Busca.DataInicioInvalida").ToString());

                if (dataInicio > DateTime.Today)
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "RelatorioSugestoesCurriculo.Busca.DataInicioMaiorHoje").ToString());

                if (string.IsNullOrEmpty(txtDataFim.Text) || !DateTime.TryParse(txtDataFim.Text, out dataFim))
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "RelatorioSugestoesCurriculo.Busca.DataFimInvalida").ToString());

                if (dataFim > DateTime.Today)
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "RelatorioSugestoesCurriculo.Busca.DataFimMaiorHoje").ToString());

                if (dataInicio > dataFim)
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "RelatorioSugestoesCurriculo.Busca.DataFimMenorInicio").ToString());
                
                report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.RelatorioSugestoesCurriculo).ToString();
                parametros = "tne_id=" + UCComboTipoNivelEnsino1.Valor.ToString() +
                             "&tne_nome=" + (UCComboTipoNivelEnsino1.Valor > 0 ? UCComboTipoNivelEnsino1.Texto : "Todos") +
                             "&tme_id=" + UCComboTipoModalidadeEnsino1.Valor.ToString() +
                             "&tme_nome=" + (UCComboTipoModalidadeEnsino1.Valor > 0 ? UCComboTipoModalidadeEnsino1.Texto : "Todas") +
                             "&apenasGeral=" + chkGeral.Checked +
                             "&tcp_id=" + UCComboTipoCurriculoPeriodo1.Valor.ToString() +
                             "&tcp_descricao=" + (UCComboTipoCurriculoPeriodo1.Valor > 0 ? UCComboTipoCurriculoPeriodo1.Texto : "Todos") +
                             "&tds_id=" + UCComboTipoDisciplina1.Valor.ToString() +
                             "&tds_nome=" + (UCComboTipoDisciplina1.Valor > 0 ? UCComboTipoDisciplina1.Texto : "Todos") +
                             "&tipoSugestao=" + ddlTipoSugestao.SelectedValue +
                             "&tipoSugestaoText=" + ddlTipoSugestao.SelectedItem.Text +
                             "&dataInicioText=" + dataInicio.ToShortDateString() +
                             "&dataFimText=" + dataFim.ToShortDateString() +
                             "&dataInicio=" + dataInicio +
                             "&dataFim=" + dataFim +
                             "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                             "&nomeTipoDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                             "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                             "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                             "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                     , ApplicationWEB.LogoRelatorioSSRS);

                CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método para salvar os filtros última busca realizada
        /// </summary>
        protected void SalvaBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("tne_id", UCComboTipoNivelEnsino1.Valor.ToString());
            filtros.Add("tme_id", UCComboTipoModalidadeEnsino1.Valor.ToString());
            filtros.Add("apenasGeral", chkGeral.Checked.ToString());
            filtros.Add("tcp_id", UCComboTipoCurriculoPeriodo1.Valor.ToString());
            filtros.Add("tds_id", UCComboTipoDisciplina1.Valor.ToString());
            filtros.Add("tipoSugestao", ddlTipoSugestao.SelectedValue);
            filtros.Add("dataInicio", txtDataInicio.Text);
            filtros.Add("dataFim", txtDataFim.Text);

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioSugestoesCurriculo, Filtros = filtros };

        }

        /// <summary>
        /// Método carrega os filtros última busca realizada
        /// </summary>
        protected void CarregarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioSugestoesCurriculo)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tne_id", out valor);
                UCComboTipoNivelEnsino1.Valor = Convert.ToInt32(valor);
                UCComboTipoNivelEnsino1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tme_id", out valor);
                UCComboTipoModalidadeEnsino1.Valor = Convert.ToInt32(valor);
                UCComboTipoModalidadeEnsino1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("apenasGeral", out valor);
                chkGeral.Checked = Convert.ToBoolean(valor);
                chkGeral_CheckedChanged(chkGeral, new EventArgs());

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tcp_id", out valor);
                UCComboTipoCurriculoPeriodo1.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tds_id", out valor);
                UCComboTipoDisciplina1.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipoSugestao", out valor);
                ddlTipoSugestao.SelectedValue = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("dataInicio", out valor);
                txtDataInicio.Text = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("dataFim", out valor);
                txtDataFim.Text = valor;

                updFiltros.Update();
            }
        }

        #endregion Métodos

        #region Eventos

        private void UCComboTipoNivelEnsino1_IndexChanged()
        {
            try
            {
                UCComboTipoCurriculoPeriodo1.CarregarPorNivelEnsinoModalidade(UCComboTipoNivelEnsino1.Valor, UCComboTipoModalidadeEnsino1.Valor);
                UCComboTipoDisciplina1.CarregarTipoDisciplinaPorNivelEnsino(UCComboTipoNivelEnsino1.Valor);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar currículo.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboTipoModalidadeEnsino1_IndexChanged()
        {
            try
            {
                UCComboTipoCurriculoPeriodo1.Valor = -1;
                UCComboTipoCurriculoPeriodo1.CarregarPorNivelEnsinoModalidade(UCComboTipoNivelEnsino1.Valor, UCComboTipoModalidadeEnsino1.Valor);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar currículo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SalvaBusca();
                GerarRelatorio();
            }
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Relatorios/RelatorioSugestoesCurriculo/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void chkGeral_CheckedChanged(object sender, EventArgs e)
        {
            UCComboTipoDisciplina1.Valor = -1;
            UCComboTipoCurriculoPeriodo1.Valor = -1;
            divFiltrosEspecificos.Visible = !chkGeral.Checked;
        }

        #endregion Eventos
    }
}