using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.RecursosHumanos.AtribuicaoEsporadica
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AtribuicaoEsporadica)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_Ordenacao", out valor))
                    {
                        return valor;
                    }
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AtribuicaoEsporadica)
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

        public ColaboradoresAtribuicao Atribuicao { get; set; }

        #endregion

        #region Métodos

        /// <summary>
        /// Busca as atribuições esporádicas salvas para a escola do filtro.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                Dictionary<string, string> filtros = new Dictionary<string, string>();

                grvResultados.PageIndex = 0;
                odsResultado.SelectParameters.Clear();
                odsResultado.SelectParameters.Add("esc_id", UCComboUAEscola1.Esc_ID.ToString());
                
                #region Salvar busca realizada com os parâmetros do ODS.

                //Salvar UA Superior.            
                filtros.Add("uad_id", UCComboUAEscola1.Uad_ID.ToString());
                filtros.Add("esc_id", UCComboUAEscola1.Esc_ID.ToString());
                filtros.Add("uni_id", UCComboUAEscola1.Uni_ID.ToString());

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.AtribuicaoEsporadica
                    ,
                    Filtros = filtros
                };

                #endregion
                
                // atualiza o grid
                grvResultados.DataBind();
                grvResultados.Sort(VS_Ordenacao, VS_SortDirection);
                pnlResultados.Visible = true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar as atribuições esporádicas.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AtribuicaoEsporadica)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor2;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_id", out valor);
                if (UCComboUAEscola1.VisibleUA)
                {
                    UCComboUAEscola1.Uad_ID = new Guid(valor);
                    UCComboUAEscola1.PermiteAlterarCombos = true;
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out valor2);
                UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };

                Pesquisar();
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                lblMensagem.Text = message;

            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                }

                UCComboQtdePaginacao1.GridViewRelacionado = grvResultados;

                if (!IsPostBack)
                {
                    Page.Form.DefaultButton = btnPesquisar.UniqueID;
                    pnlBusca.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                    btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                    btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                    btnIncluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

                    UCComboUAEscola1.Inicializar();

                    // quantidade de itens por página
                    string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                    int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);
                    // mostra essa quantidade no combobox
                    UCComboQtdePaginacao1.Valor = itensPagina;
                    // atribui essa quantidade para o grid
                    grvResultados.PageSize = itensPagina;
                    
                    VerificaBusca();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Academico/RecursosHumanos/AtribuicaoEsporadica/Cadastro.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch
            {
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }
        
        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            try
            {
                // Limpa busca da sessão.
                __SessionWEB.BuscaRealizada = new BuscaGestao();
                Response.Redirect("~/Academico/RecursosHumanos/AtribuicaoEsporadica/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch
            {
            }
        }
        
        protected void grvResultados_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvResultados.EditIndex = e.NewEditIndex;
            grvResultados.SelectedIndex = e.NewEditIndex;
            Atribuicao = new ColaboradoresAtribuicao
            {
                col_id = Convert.ToInt64(grvResultados.SelectedDataKey["col_id"])
                ,
                crg_id = Convert.ToInt32(grvResultados.SelectedDataKey["crg_id"])
                ,
                coc_id = Convert.ToInt32(grvResultados.SelectedDataKey["coc_id"])
                ,
                doc_id = Convert.ToInt64(grvResultados.SelectedDataKey["doc_id"])
                ,
                pes_nome = grvResultados.SelectedDataKey["pes_nome"].ToString()
                ,
                usu_id = new Guid(grvResultados.SelectedDataKey["usu_id"].ToString())
                ,
                esc_id = Convert.ToInt32(grvResultados.SelectedDataKey["esc_id"])
            };
        }

        protected void grvResultados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    RHU_ColaboradorCargo ent = new RHU_ColaboradorCargo
                    {
                        col_id = Convert.ToInt64(grvResultados.DataKeys[index].Values["col_id"])
                        ,
                        crg_id = Convert.ToInt32(grvResultados.DataKeys[index].Values["crg_id"])
                        ,
                        coc_id = Convert.ToInt32(grvResultados.DataKeys[index].Values["coc_id"])
                    };

                    if (RHU_ColaboradorCargoBO.ExcluirAtribuicaoEsporadica(ent,
                        Convert.ToInt64(grvResultados.DataKeys[index].Values["doc_id"]), __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete
                        , string.Format("Atribuição esporádica: col_id: {0}, crg_id: {1}, coc_id: {2}"
                        , ent.col_id, ent.crg_id, ent.coc_id));
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Atribuição excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        Response.Redirect("~/Academico/RecursosHumanos/AtribuicaoEsporadica/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                catch(ThreadAbortException)
                {

                }
                catch (ValidationException ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a atribuição esporádica.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void UCComboQtdePaginacao1_IndexChanged()
        {
            try
            {
                // atribui nova quantidade itens por página para o grid
                grvResultados.PageSize = UCComboQtdePaginacao1.Valor;
                Pesquisar();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar as atribuições esporádicas.", UtilBO.TipoMensagem.Erro);
            }
        }
        
        protected void grvResultados_Sorted(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grid);

            if ((!string.IsNullOrEmpty(grid.SortExpression)) &&
               (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AtribuicaoEsporadica))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = grid.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", grid.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = grid.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", grid.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.AtribuicaoEsporadica
                    ,
                    Filtros = filtros
                };
            }
        }

        protected void grvResultados_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = RHU_ColaboradorBO.GetTotalRecords();
        }

        protected void grvResultados_RowDataBound(object sender, GridViewRowEventArgs e)
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

                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }
        
        protected void odsResultado_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!IsPostBack && 
                (__SessionWEB.BuscaRealizada.PaginaBusca != PaginaGestao.AtribuicaoEsporadica))
                e.Cancel = true;
        }

        #endregion
    }
}