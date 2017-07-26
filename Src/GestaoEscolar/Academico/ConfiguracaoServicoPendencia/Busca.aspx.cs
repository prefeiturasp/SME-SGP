using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
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
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

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
            try
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

                        pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semNota"].ToString() == false.ToString() ? "" : GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Cadastro.chkSemNota.Text").ToString() + " / ";
                        pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semParecer"].ToString() == false.ToString() ? "" : GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Cadastro.chkSemParecerConclusivo.Text").ToString() + " / ";
                        pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_disciplinaSemAula"].ToString() == false.ToString() ? "" : GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Cadastro.chkDisciplinaSemAula.Text").ToString() + " / ";
                        pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semResultadoFinal"].ToString() == false.ToString() ? "" : GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Cadastro.chkSemResultadoFinal.Text").ToString() + " / ";
                        pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semPlanejamento"].ToString() == false.ToString() ? "" : GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Cadastro.chkSemPlanejamento.Text").ToString() + " / ";
                        pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semSintese"].ToString() == false.ToString() ? "" : GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Cadastro.chkSemSinteseFinal.Text").ToString() + " / ";
                        pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semPlanoAula"].ToString() == false.ToString() ? "" : GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Cadastro.chkSemPlanoAula.Text").ToString() + " / ";
                        pendencias += grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semObjetoConhecimento"].ToString() == false.ToString() ? "" : GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Cadastro.chkSemObjetoConhecimento.Text").ToString() + " / ";

                        eConfiguracaoServicoPendenciaSemRelatorioAtendimento pendenciaRelatorio =
                            (eConfiguracaoServicoPendenciaSemRelatorioAtendimento)Enum.Parse(typeof(eConfiguracaoServicoPendenciaSemRelatorioAtendimento), grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semRelatorioAtendimento"].ToString());

                        Type objType = typeof(eConfiguracaoServicoPendenciaSemRelatorioAtendimento);
                        FieldInfo[] propriedades = objType.GetFields();
                        foreach (FieldInfo objField in propriedades)
                        {
                            DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

                            if (attributes.Length > 0)
                            {
                                eConfiguracaoServicoPendenciaSemRelatorioAtendimento pend =
                                (eConfiguracaoServicoPendenciaSemRelatorioAtendimento)Enum.Parse(typeof(eConfiguracaoServicoPendenciaSemRelatorioAtendimento), Convert.ToString(objField.GetRawConstantValue()));

                                pendencias += pendenciaRelatorio.HasFlag(pend) ? GetGlobalResourceObject("Enumerador", attributes[0].Description).ToString() + " / " : "";
                            }
                        }

                        lblPendencias.Text = pendencias.Length > 0 ? pendencias.Substring(0, pendencias.Length - 3) : GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Busca.lblPendencias.Text.Nenhuma").ToString();
                    }
                    ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                    if (btnExcluir != null)
                    {                        
                        btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                        btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Busca.ErroPesquisar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }
        
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ConfiguracaoServicoPendencia/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion
        
        protected void grvConfigServPendencia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int csp_id = Convert.ToInt32(grvConfigServPendencia.DataKeys[index].Values["csp_id"].ToString());

                    ACA_ConfiguracaoServicoPendencia entity = new ACA_ConfiguracaoServicoPendencia { csp_id = csp_id };
                    ACA_ConfiguracaoServicoPendenciaBO.GetEntity(entity);

                    if (ACA_ConfiguracaoServicoPendenciaBO.Delete(entity))
                    {
                        grvConfigServPendencia.PageIndex = 0;
                        grvConfigServPendencia.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "csp_id: " + csp_id);
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Busca.Mensagem.ExcluidoSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ConfiguracaoServicoPendencia.Busca.Mensagem.ErroExcluir").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }
        
    }
}
