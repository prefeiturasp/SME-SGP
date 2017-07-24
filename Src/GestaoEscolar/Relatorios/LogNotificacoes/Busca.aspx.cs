using System;
using System.Web;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using MSTech.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Relatorios.LogNotificacoes
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
                    InicializarTela();
                    VerificaBusca();
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
                relTitulo.InnerText = Modulo.mod_nome;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa os componentes da tela
        /// </summary>
        private void InicializarTela()
        {
            UCComboAlerta1.Obrigatorio = true;
            UCComboAlerta1.CarregarAlerta();
        }

        /// <summary>
        /// O método gera o relatório de Análise de Sondagem abaixo da frequência
        /// </summary>
        private void GerarRelatorio()
        {
            try
            {
                string report, parametros;

                DateTime dataInicio = new DateTime();
                DateTime dataFim = new DateTime();

                if (string.IsNullOrEmpty(txtDataInicio.Text) || !DateTime.TryParse(txtDataInicio.Text, out dataInicio))
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "LogNotificacoes.Busca.DataInicioInvalida").ToString());

                if (dataInicio > DateTime.Today)
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "LogNotificacoes.Busca.DataInicioMaiorHoje").ToString());

                if (string.IsNullOrEmpty(txtDataFim.Text) || !DateTime.TryParse(txtDataFim.Text, out dataFim))
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "LogNotificacoes.Busca.DataFimInvalida").ToString());

                if (dataFim > DateTime.Today)
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "LogNotificacoes.Busca.DataFimMaiorHoje").ToString());

                if (dataInicio > dataFim)
                    throw new ValidationException(GetGlobalResourceObject("Relatorios", "LogNotificacoes.Busca.DataFimMenorInicio").ToString());

                SalvaBusca();

                report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.LogNotificacoes).ToString();
                parametros = "ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                             "&cfa_id=" + UCComboAlerta1.Valor +
                             "&cfa_nome=" + UCComboAlerta1.Texto +
                             "&dataInicio=" + txtDataInicio.Text +
                             "&dataFim=" + txtDataFim.Text +
                             "&adm=" + (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa) +
                             "&usu_id=" + __SessionWEB.__UsuarioWEB.Usuario.usu_id +
                             "&gru_id=" + __SessionWEB.__UsuarioWEB.Grupo.gru_id +
                             "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                   , ApplicationWEB.LogoRelatorioSSRS) +
                             "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                             "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria");

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
            filtros.Add("cfa_id", UCComboAlerta1.Valor.ToString());
            filtros.Add("cfa_dataIncio", Convert.ToDateTime(txtDataInicio.Text).ToString());
            filtros.Add("cfa_dataFim", Convert.ToDateTime(txtDataFim.Text).ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.LogNotificacoes, Filtros = filtros };

        }

        /// <summary>
        /// Método carrega os filtros última busca realizada
        /// </summary>
        protected void VerificaBusca()
        {
            try
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LogNotificacoes)
                {
                    // Recuperar busca realizada e pesquisar automaticamente
                    string valor, valor2, valor3;
                    
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cfa_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cfa_dataIncio", out valor2);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cfa_dataFim", out valor3);
                    UCComboAlerta1.Valor = Convert.ToInt32(valor);
                    txtDataInicio.Text = valor2;
                    txtDataFim.Text = valor3;
                    
                    updFiltros.Update();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GerarRelatorio();
            }
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
        }

        #endregion Eventos
    }
}