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
        /// <summary>
        /// Retorna o valor do parâmetro "Permanecer na tela após gravações"
        /// </summary>
        private bool ParametroPermanecerTela
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tne_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_tne_id
        {
            get
            {
                if (ViewState["VS_tne_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tne_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_tne_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tme_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_tme_id
        {
            get
            {
                if (ViewState["VS_tme_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tme_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_tme_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tur_tipo
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private byte VS_tur_tipo
        {
            get
            {
                if (ViewState["VS_tur_tipo"] != null)
                {
                    return Convert.ToByte(ViewState["VS_tur_tipo"]);
                }
                return 0;
            }
            set
            {
                ViewState["VS_tur_tipo"] = value;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada.
        /// </summary>
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

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada.
        /// </summary>
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

        /// <summary>
        /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
        /// </summary>
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

                // Limpar a ordenação realizada.
                grvConfigServPendencia.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                Dictionary<string, string> filtros = odsConfigServPendencia.SelectParameters.Cast<Parameter>().ToDictionary(param => param.Name, param => param.DefaultValue);

                __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.ConfiguracaoServicoPendencia, Filtros = filtros };

                #endregion Salvar busca realizada com os parâmetros do ODS.

                // Atualiza o grid
                grvConfigServPendencia.DataBind();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as configurações do serviço de pendência.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta,
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConfiguracaoServicoPendencia)
            {
                string valor;
                string valor2;
                string valor3;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tne_id", out valor);
                UCComboTipoNivelEnsino.Valor = Convert.ToInt32(valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tme_id", out valor2);
                UCComboTipoModalidadeEnsino.Valor = Convert.ToInt32(valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_tipo", out valor2);
                UCComboTipoTurma.Valor = Convert.ToByte(valor);

                Pesquisar();
            }
            else
            {
                setValorCombos(-1, -1, 0);
            }
        }

        /// <summary>
        /// Verifica se usuário tem permissão de acesso à página.
        /// </summary>
        private void VerificaPermissaoUsuario()
        {
            if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// Inicializa os combos.
        /// </summary>
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
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }

        }

        private void setValorCombos(int tne_id, int tme_id, byte tur_tipo)
        {
            UCComboTipoNivelEnsino.Valor = tne_id <= 0 ? -1 : tne_id;
            UCComboTipoModalidadeEnsino.Valor = tme_id <= 0 ? -1 : tme_id;
            UCComboTipoTurma.Valor = tur_tipo <= 0 ? Convert.ToByte(0) : tur_tipo;
        }

        private void VerificaCadastro()
        {
            fdsConfiguracao.Visible = VS_tne_id > 0 && VS_tme_id > 0 && VS_tur_tipo > 0;
            UCComboTipoNivelEnsino.PermiteEditar = UCComboTipoModalidadeEnsino.PermiteEditar = UCComboTipoTurma.PermiteEditar = !fdsConfiguracao.Visible;
            if (fdsConfiguracao.Visible)
            {
                LoadByEntity(VS_tne_id, VS_tme_id, VS_tur_tipo);
            }
        }

        private void LoadByEntity(int tne_id, int tme_id, byte tur_tipo)
        {
            ACA_ConfiguracaoServicoPendencia entity = ACA_ConfiguracaoServicoPendenciaBO.SelectBy_tne_id_tme_id_tur_tipo(tne_id, tme_id, tur_tipo);
            if (entity.csp_id > 0)
            {
                chkDisciplinaSemAula.Checked = entity.csp_disciplinaSemAula;
                chkSemNota.Checked = entity.csp_semNota;
                chkSemParecer.Checked = entity.csp_semParecer;
                chkSemPlanejamento.Checked = entity.csp_semPlanejamento;
                chkSemResultadoFinal.Checked = entity.csp_semResultadoFinal;
                chkSemSintese.Checked = entity.csp_semSintese;
                chkSemPlanoAula.Checked = entity.csp_semPlanoAula;
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
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }

            Page.Form.DefaultButton = btnPesquisar.UniqueID;
            Page.Form.DefaultFocus = UCComboTipoNivelEnsino.ClientID;

            UCComboTipoNivelEnsino.IndexChanged += UCComboTipoNivelEnsino_IndexChanged;
            UCComboTipoModalidadeEnsino.IndexChanged += UCComboTipoModalidadeEnsino_IndexChanged;
            UCComboTipoTurma.IndexChanged += UCComboTipoTurma_IndexChanged;

        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            ACA_ConfiguracaoServicoPendencia entity = ACA_ConfiguracaoServicoPendenciaBO.SelectBy_tne_id_tme_id_tur_tipo(UCComboTipoNivelEnsino.Valor, UCComboTipoModalidadeEnsino.Valor, UCComboTipoTurma.Valor);

            entity.tne_id = UCComboTipoNivelEnsino.Valor;
            entity.tme_id = UCComboTipoModalidadeEnsino.Valor;
            entity.tur_tipo = UCComboTipoTurma.Valor;
            entity.csp_disciplinaSemAula = chkDisciplinaSemAula.Checked;
            entity.csp_semNota = chkSemNota.Checked;
            entity.csp_semParecer = chkSemParecer.Checked;
            entity.csp_semPlanejamento = chkSemPlanejamento.Checked;
            entity.csp_semResultadoFinal = chkSemResultadoFinal.Checked;
            entity.csp_semSintese = chkSemSintese.Checked;
            entity.csp_semPlanoAula = chkSemPlanoAula.Checked;
            entity.IsNew = entity.csp_id <= 0;

            if (ACA_ConfiguracaoServicoPendenciaBO.Save(entity))
            {
                ApplicationWEB._GravaLogSistema(entity.IsNew ? LOG_SistemaTipo.Insert : LOG_SistemaTipo.Update, "Cadastro de configuração do serviço de pendência. csp_id" + entity.csp_id);
                string message = UtilBO.GetErroMessage("Configuração do serviço de pendência cadastrada com sucesso.", UtilBO.TipoMensagem.Sucesso);

                if (ParametroPermanecerTela)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = message;
                    VerificaCadastro();
                    setValorCombos(VS_tne_id, VS_tme_id, VS_tur_tipo);
                }
                else
                {
                    __SessionWEB.PostMessages = message;
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ConfiguracaoServicoPendencia/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Inicializa();
                }
            }
        }

        protected void UCComboTipoNivelEnsino_IndexChanged()
        {
            VS_tne_id = UCComboTipoNivelEnsino.Valor;
        }

        protected void UCComboTipoModalidadeEnsino_IndexChanged()
        {
            VS_tme_id = UCComboTipoModalidadeEnsino.Valor;
        }

        protected void UCComboTipoTurma_IndexChanged()
        {
            VS_tur_tipo = UCComboTipoTurma.Valor;
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
            UCTotalRegistros.Total = ACA_ConfiguracaoServicoPendencia.GetTotalRecords();

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

        protected void odsConfigServPendencia_Selecting1(object sender, ObjectDataSourceSelectingEventArgs e)
        {

        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {

        }

        #endregion

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
                    String pendencias = String.Empty;

                    pendencias +=
                        grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semNota"].ToString() == false.ToString() ? "" : "Sem nota/"
                        + grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semParecer"].ToString() == false.ToString() ? "" : "Sem parecer/"
                        + grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_disciplinaSemAula"].ToString() == false.ToString() ? "" : "Disciplina sem aula/"
                        + grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semResultadoFinal"].ToString() == false.ToString() ? "" : "Sem resultado final/"
                        + grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semPlanejamento"].ToString() == false.ToString() ? "" : "Sem planejamento/"
                        + grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semSintese"].ToString() == false.ToString() ? "" : "Sem síntese/"
                        + grvConfigServPendencia.DataKeys[e.Row.RowIndex].Values["csp_semPlanoAula"].ToString() == false.ToString() ? "" : "Aula sem plano de aula/";

                    pendencias.Remove(pendencias.Length - 1);

                    lblPendencias.Text = pendencias;
                }
            }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}
}