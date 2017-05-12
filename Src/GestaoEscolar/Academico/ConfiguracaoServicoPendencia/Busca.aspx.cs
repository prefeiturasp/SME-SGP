using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.ConfiguracaoServicoPendencia
{
    public partial class Busca : MotherPageLogado
    {
        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
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

                return "";
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
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
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            UCComboQtdePaginacao.GridViewRelacionado = dgvConfiguracaoServicoPendencia;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                dgvConfiguracaoServicoPendencia.PageIndex = 0;
                dgvConfiguracaoServicoPendencia.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    VerificaBusca();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = btnPesquisar.UniqueID;

                Page.Form.DefaultFocus = UCComboTipoNivelEnsino.ClientID;

                divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConfiguracaoServicoPendencia)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tne_id", out valor);
                UCComboTipoNivelEnsino.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tme_id", out valor);
                UCComboTipoModalidadeEnsino.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_tipo", out valor);
                UCComboTipoTurma.Valor = Convert.ToInt32(valor);

                Pesquisar();
            }
            else
            {
                fdsResultados.Visible = false;
            }
        }

        /// <summary>
        /// Realiza a pesquisa com base nos filtros selecionados.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                Dictionary<string, string> filtros = new Dictionary<string, string>();

                dgvConfiguracaoServicoPendencia.PageIndex = 0;
                odsConfiguracaoServicoPendencia.SelectParameters.Clear();
                odsConfiguracaoServicoPendencia.SelectParameters.Add("tne_id", UCComboTipoNivelEnsino.Valor.ToString());
                odsConfiguracaoServicoPendencia.SelectParameters.Add("tme_id", UCComboTipoModalidadeEnsino.Valor.ToString());
                odsConfiguracaoServicoPendencia.SelectParameters.Add("tur_tipo", UCComboTipoTurma.Valor.ToString());
                //TODO:[ANA] é necessário?
                odsConfiguracaoServicoPendencia.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao)
                {
                    odsConfiguracaoServicoPendencia.SelectParameters.Add("usu_id", Guid.Empty.ToString());
                    odsConfiguracaoServicoPendencia.SelectParameters.Add("gru_id", Guid.Empty.ToString());
                }
                else
                {
                    odsConfiguracaoServicoPendencia.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                    odsConfiguracaoServicoPendencia.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                }

                dgvConfiguracaoServicoPendencia.Sort("", SortDirection.Ascending);
                odsConfiguracaoServicoPendencia.DataBind();

                // quantidade de itens por página            
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                dgvConfiguracaoServicoPendencia.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                foreach (Parameter param in odsConfiguracaoServicoPendencia.SelectParameters)
                {
                    filtros.Add(param.Name, param.DefaultValue);
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.ConfiguracaoServicoPendencia
                    ,
                    Filtros = filtros
                };

                #endregion

                // mostra essa quantidade no combobox            
                UCComboQtdePaginacao.Valor = itensPagina;
                // atribui essa quantidade para o grid
                dgvConfiguracaoServicoPendencia.PageSize = itensPagina;
                // atualiza o grid
                dgvConfiguracaoServicoPendencia.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
            fdsResultados.Visible = true;
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ConfiguracaoServicoPendencia/Busca.aspx", false);
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            dgvConfiguracaoServicoPendencia.PageSize = UCComboQtdePaginacao.Valor;
            dgvConfiguracaoServicoPendencia.PageIndex = 0;
            dgvConfiguracaoServicoPendencia.Sort("", SortDirection.Ascending);
            // atualiza o grid
            dgvConfiguracaoServicoPendencia.DataBind();
        }

        protected void dgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAlterar = (Label)e.Row.FindControl("lblAlterar");
                if (lblAlterar != null)
                {
                    lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                CheckBox chkSemNota = (CheckBox)e.Row.FindControl("chkSemNota");
                chkSemNota.Checked = String.IsNullOrEmpty(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["SemNota"].ToString()) ? false
                    : Convert.ToBoolean(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["SemNota"].ToString());

                CheckBox chkSemParecer = (CheckBox)e.Row.FindControl("chkSemParecer");
                chkSemParecer.Checked = String.IsNullOrEmpty(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["SemParecer"].ToString()) ? false
                    : Convert.ToBoolean(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["SemParecer"].ToString());

                CheckBox chkDisciplinaSemAula = (CheckBox)e.Row.FindControl("chkDisciplinaSemAula");
                chkDisciplinaSemAula.Checked = String.IsNullOrEmpty(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["DisciplinaSemAula"].ToString()) ? false
                    : Convert.ToBoolean(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["DisciplinaSemAula"].ToString());

                CheckBox chkSemResultadoFinal = (CheckBox)e.Row.FindControl("chkSemResultadoFinal");
                chkSemResultadoFinal.Checked = String.IsNullOrEmpty(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["SemResultadoFinal"].ToString()) ? false
                    : Convert.ToBoolean(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["SemResultadoFinal"].ToString());

                CheckBox chkSemPlanejamento = (CheckBox)e.Row.FindControl("chkSemPlanejamento");
                chkSemPlanejamento.Checked = String.IsNullOrEmpty(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["SemPlanejamento"].ToString()) ? false
                    : Convert.ToBoolean(dgvConfiguracaoServicoPendencia.DataKeys[e.Row.RowIndex].Values["SemPlanejamento"].ToString());
            }
        }

        protected void dgv_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = ACA_ConfiguracaoServicoPendenciaBO.GetTotalRecords();
            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(dgvConfiguracaoServicoPendencia);

            if ((!string.IsNullOrEmpty(dgvConfiguracaoServicoPendencia.SortExpression)) &&
                 (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConfiguracaoServicoPendencia))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = dgvConfiguracaoServicoPendencia.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", dgvConfiguracaoServicoPendencia.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = dgvConfiguracaoServicoPendencia.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", dgvConfiguracaoServicoPendencia.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.ConfiguracaoServicoPendencia
                    ,
                    Filtros = filtros
                };
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

        }
    }
}