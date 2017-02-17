using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Relatorios.IndicadorFrequencia
{
    public partial class Busca : MotherPageLogado
    {
        #region Metodos

        /// <summary>Carrega os filtros última busca realizada</summary>
        protected void CarregarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.IndicadorFrequencia)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("agrupamento", out valor);
                ddlAgrupamento.SelectedIndex = ddlAgrupamento.Items.IndexOf(ddlAgrupamento.Items.FindByValue(valor));
            }
        }

        /// <summary>Salvar os filtros última busca realizada</summary>
        protected void SalvarBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("agrupamento", ddlAgrupamento.SelectedValue);

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.IndicadorFrequencia, Filtros = filtros };
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var tituloPeriodoCurso = GestaoEscolarUtilBO.nomePadraoTipoCurrPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (!string.IsNullOrEmpty(tituloPeriodoCurso))
                {
                    var itemPeriodoCurso = ddlAgrupamento.Items.FindByValue("2");
                    itemPeriodoCurso.Text = tituloPeriodoCurso;
                }

                pnlBusca.GroupingText = GestaoEscolarUtilBO.BuscaNomeModulo("~/Relatorios/IndicadorFrequencia/Busca.aspx", "Indicador de frequência", __SessionWEB.__UsuarioWEB.Grupo.gru_id);

                CarregarBusca();

                Page.Form.DefaultFocus = ddlAgrupamento.UniqueID;
                Page.Form.DefaultButton = btnGerar.UniqueID;
            }
        }

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    SalvarBusca();

                    string report;
                    if (ddlAgrupamento.SelectedValue == "1")
                        report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.IndicadorFrequenciaDRE).ToString();
                    else
                        report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.IndicadorFrequenciaPeriodoCurso).ToString();

                    string parameter =
                           "nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio")
                        + "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria")
                        + "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS);

                    MSTech.GestaoEscolar.BLL.CFG_RelatorioBO.CallReport("Relatorios", report, parameter, HttpContext.Current);

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "IndicadorFrequencia.Busca.btnGerar.Click.Erro").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }
    }
}