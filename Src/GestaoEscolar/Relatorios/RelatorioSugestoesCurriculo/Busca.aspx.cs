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
            txtDataInicio.Text = "";
            txtDataFim.Text = "";
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
                
                if (string.IsNullOrEmpty(txtDataFim.Text) || !DateTime.TryParse(txtDataFim.Text, out dataFim))
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "RelatorioSugestoesCurriculo.Busca.DataFimInvalida").ToString());
                
                if (dataInicio > dataFim)
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "RelatorioSugestoesCurriculo.Busca.DataFimMenorInicio").ToString());

                report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.RelatorioSugestoesCurriculo).ToString();
                parametros = "dataInicio=" + dataInicio.ToShortDateString() +
                             "&dataFim=" + dataFim.ToShortDateString() +
                             "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
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

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("dataInicio", out valor);
                txtDataInicio.Text = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("dataFim", out valor);
                txtDataFim.Text = valor;

                updFiltros.Update();
            }
        }

        #endregion Métodos

        #region Eventos

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

        #endregion Eventos
    }
}