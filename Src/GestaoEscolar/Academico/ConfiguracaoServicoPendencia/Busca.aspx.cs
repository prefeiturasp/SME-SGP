using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.ConfiguracaoServicoPendencia
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades
        public int Edit_csp_id
        {
            get
            {
                return Convert.ToInt32(grvConfigServPendencia.DataKeys[grvConfigServPendencia.EditIndex].Values["csp_id"] ?? 0);
            }
        }

        private int VS_csp_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_csp_id"] ?? -1);
            }

            set
            {
                ViewState["VS_csp_id"] = value;
            }
        }

        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConfiguracaoServicoPendencia)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_Ordenacao", out valor))
                    {
                        return valor;
                    }
                }

                return string.Empty;
            }
        }

        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConfiguracaoServicoPendencia)
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
        public void Pesquisar()
        {
            try
            {
                odsConfigServPendencia.SelectParameters.Clear();
                odsConfigServPendencia.SelectParameters.Add("tne_id", UCComboTipoNivelEnsino.Valor.ToString());
                odsConfigServPendencia.SelectParameters.Add("tme_id", UCComboTipoModalidadeEnsino.Valor.ToString());
                odsConfigServPendencia.SelectParameters.Add("tur_tipo", UCComboTipoTurma.Valor.ToString());
                odsConfigServPendencia.SelectParameters.Add("paginado", "true");

                grvConfigServPendencia.PageIndex = 0;
                grvConfigServPendencia.PageSize = UCComboQtdePaginacao.Valor;

                fdsResultado.Visible = true;

                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // Limpar a ordenação realizada.
                grvConfigServPendencia.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                Dictionary<string, string> filtros = odsConfigServPendencia.SelectParameters.Cast<Parameter>().ToDictionary(param => param.Name, param => param.DefaultValue);

                __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.ConfiguracaoServicoPendencia, Filtros = filtros };

                #endregion Salvar busca realizada com os parâmetros do ODS.

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvConfigServPendencia.PageSize = itensPagina;

                // Atualiza o grid
                grvConfigServPendencia.DataBind();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Busca.ErroPesquisar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConfiguracaoServicoPendencia)
            {
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tne_id", out valor);
                UCComboTipoNivelEnsino.Valor = Convert.ToInt32(valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tme_id", out valor);
                UCComboTipoModalidadeEnsino.Valor = Convert.ToInt32(valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_tipo", out valor);
                UCComboTipoTurma.Valor = Convert.ToByte(valor);

                Pesquisar();
            }
        }

        private void VerificaPermissaoUsuario()
        {
            if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Busca.ErroPermissao").ToString(), UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        private void Inicializar()
        {
            try
            {
                VerificaPermissaoUsuario();

                UCComboTipoModalidadeEnsino.CarregarTipoModalidadeEnsino();
                UCComboTipoNivelEnsino.CarregarTipoNivelEnsino();

                VerificaBusca();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Busca.ErroInicializar").ToString(), UtilBO.TipoMensagem.Erro);
            }

        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                grvConfigServPendencia.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    Inicializar();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Busca.ErroSistema").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }

            Page.Form.DefaultButton = btnPesquisar.UniqueID;
            Page.Form.DefaultFocus = UCComboTipoNivelEnsino.ClientID;

        }

        protected void odsConfigServPendencia_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Inicializa variável de sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void grvConfigServPendencia_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = ACA_ConfiguracaoServicoPendenciaBO.GetTotalRecords();

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvConfigServPendencia);

            if ((!string.IsNullOrEmpty(grvConfigServPendencia.SortExpression)) && (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConfiguracaoServicoPendencia))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = grvConfigServPendencia.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", grvConfigServPendencia.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = grvConfigServPendencia.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", grvConfigServPendencia.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.ConfiguracaoServicoPendencia
                    ,
                    Filtros = filtros
                };
            }
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvConfigServPendencia.PageSize = UCComboQtdePaginacao.Valor;
            grvConfigServPendencia.PageIndex = 0;
            grvConfigServPendencia.Sort("", SortDirection.Ascending);
            // atualiza o grid
            grvConfigServPendencia.DataBind();
        }

        protected void grvConfigServPendencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");
                if (btnEditar != null)
                {
                    btnEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                Label lblPendencias = (Label)e.Row.FindControl("lblPendencias");
                if (lblPendencias != null)
                {
                    string pendencias = string.Empty;

                    pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semNota"].ToString() == false.ToString() ? "" : "Sem nota/";
                    pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semParecer"].ToString() == false.ToString() ? "" : "Sem parecer/";
                    pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_disciplinaSemAula"].ToString() == false.ToString() ? "" : "Disciplina sem aula/";
                    pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semResultadoFinal"].ToString() == false.ToString() ? "" : "Sem resultado final/";
                    pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semPlanejamento"].ToString() == false.ToString() ? "" : "Sem planejamento/";
                    pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semSintese"].ToString() == false.ToString() ? "" : "Sem síntese/";
                    pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semPlanoAula"].ToString() == false.ToString() ? "" : "Aula sem plano de aula/";

                    lblPendencias.Text = pendencias.Substring(0, pendencias.Length - 1);
                }
            }
        }
        
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ConfiguracaoServicoPendencia/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion
    }
}
