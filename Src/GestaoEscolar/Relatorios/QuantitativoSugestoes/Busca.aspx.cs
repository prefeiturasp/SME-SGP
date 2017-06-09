using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;

namespace GestaoEscolar.Relatorios.QuantitativoSugestoes
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
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "QuantitativoSugestoes.Busca.lblMessage.ErroPermissao").ToString(), UtilBO.TipoMensagem.Alerta);
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
            UCComboAnoLetivo1.Carregar();
            UCComboTipoNivelEnsino1.CarregarTipoNivelEnsinoSemInfantil();
            UCComboTipoModalidadeEnsino1.CarregarTipoModalidadeEnsino();
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
                
                report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.QuantitativoSugestoes).ToString();
                parametros = "cal_ano=" + UCComboAnoLetivo1.ano.ToString() +
                             "&tne_id=" + UCComboTipoNivelEnsino1.Valor.ToString() +
                             "&tne_nome=" + UCComboTipoNivelEnsino1.Texto +
                             "&tme_id=" + UCComboTipoModalidadeEnsino1.Valor.ToString() +
                             "&tme_nome=" + UCComboTipoModalidadeEnsino1.Texto +
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
            filtros.Add("cal_ano", UCComboAnoLetivo1.ano.ToString());
            filtros.Add("tne_id", UCComboTipoNivelEnsino1.Valor.ToString());
            filtros.Add("tme_id", UCComboTipoModalidadeEnsino1.Valor.ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.QuantitativoSugestoes, Filtros = filtros };

        }

        /// <summary>
        /// Método carrega os filtros última busca realizada
        /// </summary>
        protected void CarregarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.QuantitativoSugestoes)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_ano", out valor);
                UCComboAnoLetivo1.ano = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tne_id", out valor);
                UCComboTipoNivelEnsino1.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tme_id", out valor);
                UCComboTipoModalidadeEnsino1.Valor = Convert.ToInt32(valor);

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
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Relatorios/QuantitativoSugestoes/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion Eventos
    }
}