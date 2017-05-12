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

            UCComboQtdePaginacao.GridViewRelacionado = dgv;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                dgv.PageIndex = 0;
                dgv.PageSize = ApplicationWEB._Paginacao;

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

                dgv.PageIndex = 0;
                ods.SelectParameters.Clear();
                ods.SelectParameters.Add("tne_id", UCComboTipoNivelEnsino.Valor.ToString());
                ods.SelectParameters.Add("tme_id", UCComboTipoModalidadeEnsino.Valor.ToString());
                ods.SelectParameters.Add("tur_tipo", UCComboTipoTurma.Valor.ToString());
                //TODO:[ANA] é necessário?
                ods.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao)
                {
                    ods.SelectParameters.Add("usu_id", Guid.Empty.ToString());
                    ods.SelectParameters.Add("gru_id", Guid.Empty.ToString());
                }
                else
                {
                    ods.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                    ods.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                }

                dgv.Sort("", SortDirection.Ascending);
                ods.DataBind();

                // quantidade de itens por página            
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                dgv.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                foreach (Parameter param in ods.SelectParameters)
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
                dgv.PageSize = itensPagina;
                // atualiza o grid
                dgv.DataBind();
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
            dgv.PageSize = UCComboQtdePaginacao.Valor;
            dgv.PageIndex = 0;
            dgv.Sort("", SortDirection.Ascending);
            // atualiza o grid
            dgv.DataBind();
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
                chkSemNota.Checked = String.IsNullOrEmpty(dgv.DataKeys[e.Row.RowIndex].Values["SemNota"].ToString()) ? false
                    : Convert.ToBoolean(dgv.DataKeys[e.Row.RowIndex].Values["SemNota"].ToString());

                CheckBox chkSemParecer = (CheckBox)e.Row.FindControl("chkSemParecer");
                chkSemParecer.Checked = String.IsNullOrEmpty(dgv.DataKeys[e.Row.RowIndex].Values["SemParecer"].ToString()) ? false
                    : Convert.ToBoolean(dgv.DataKeys[e.Row.RowIndex].Values["SemParecer"].ToString());

                CheckBox chkDisciplinaSemAula = (CheckBox)e.Row.FindControl("chkDisciplinaSemAula");
                chkDisciplinaSemAula.Checked = String.IsNullOrEmpty(dgv.DataKeys[e.Row.RowIndex].Values["DisciplinaSemAula"].ToString()) ? false
                    : Convert.ToBoolean(dgv.DataKeys[e.Row.RowIndex].Values["DisciplinaSemAula"].ToString());

                CheckBox chkSemResultadoFinal = (CheckBox)e.Row.FindControl("chkSemResultadoFinal");
                chkSemResultadoFinal.Checked = String.IsNullOrEmpty(dgv.DataKeys[e.Row.RowIndex].Values["SemResultadoFinal"].ToString()) ? false
                    : Convert.ToBoolean(dgv.DataKeys[e.Row.RowIndex].Values["SemResultadoFinal"].ToString());

                CheckBox chkSemPlanejamento = (CheckBox)e.Row.FindControl("chkSemPlanejamento");
                chkSemPlanejamento.Checked = String.IsNullOrEmpty(dgv.DataKeys[e.Row.RowIndex].Values["SemPlanejamento"].ToString()) ? false
                    : Convert.ToBoolean(dgv.DataKeys[e.Row.RowIndex].Values["SemPlanejamento"].ToString());
            }
        }

        protected void dgv_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = ACA_BO.GetTotalRecords();
            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(dgv);

            if ((!string.IsNullOrEmpty(dgv.SortExpression)) &&
                 (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConfiguracaoServicoPendencia))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = dgv.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", dgv.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = dgv.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", dgv.SortDirection.ToString());
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