using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Web;

namespace GestaoEscolar.Academico.AgendamentoSondagem
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        /// <summary>Posição da coluna de Agendamento no grid view da Sondagem.</summary>
        private const int PosicaoAgendamento = 2;
        
        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna o snd_id do registro que esta sendo editado.
        /// </summary>
        public int EditItem
        {
            get
            {
                return Convert.ToInt32(dgvSondagem.DataKeys[dgvSondagem.EditIndex]["snd_id"]);
            }
        }

        /// <summary>
        /// Retorna o snd_id do registro que esta selecionado.
        /// </summary>
        public int SelectedItem
        {
            get
            {
                return Convert.ToInt32(dgvSondagem.DataKeys[dgvSondagem.SelectedIndex]["snd_id"]);
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Sondagem)
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
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Sondagem)
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

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            dgvSondagem.PageSize = UCComboQtdePaginacao1.Valor;
            dgvSondagem.PageIndex = 0;
            // atualiza o grid
            dgvSondagem.DataBind();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
        /// </summary>
        private void _Pesquisar()
        {
            try
            {
                Dictionary<string, string> filtros = new Dictionary<string, string>();

                dgvSondagem.PageIndex = 0;
                odsSondagem.SelectParameters.Clear();
                odsSondagem.SelectParameters.Add("snd_titulo", txtTitulo.Text);

                // quantidade de itens por página            
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                dgvSondagem.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                //Salvar UA Superior.            
                //  if (UCFiltroEscolas1._VS_FiltroEscola == true)
                //  filtros.Add("ua_superior", UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue);

                foreach (Parameter param in odsSondagem.SelectParameters)
                {
                    filtros.Add(param.Name, param.DefaultValue);
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.Sondagem
                    ,
                    Filtros = filtros
                };

                #endregion

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                dgvSondagem.PageSize = itensPagina;
                // atualiza o grid
                dgvSondagem.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Busca.ErroCarregarSondagens").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Sondagem)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("snd_titulo", out valor);
                txtTitulo.Text = valor;

                _Pesquisar();
            }
            else
            {
                fdsResultados.Visible = false;
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
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            UCComboQtdePaginacao1.GridViewRelacionado = dgvSondagem;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                dgvSondagem.PageIndex = 0;
                dgvSondagem.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    VerificaBusca();

                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsSondagem.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsSondagem.ClientID)), true);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Busca.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = btnPesquisar.UniqueID;
                Page.Form.DefaultFocus = txtTitulo.ClientID;

                divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            }
        }

        protected void odsSondagem_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        protected void dgvSondagem_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_SondagemBO.GetTotalRecords();

            ConfiguraColunasOrdenacao(dgvSondagem);

            if ((!string.IsNullOrEmpty(dgvSondagem.SortExpression)) &&
            (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Sondagem))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = dgvSondagem.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", dgvSondagem.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = dgvSondagem.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", dgvSondagem.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.Sondagem
                    ,
                    Filtros = filtros
                };
            }
        }

        protected void dgvSondagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnCadastrarAgendamento = e.Row.FindControl("btnCadastrarAgendamento") as ImageButton;
                if (btnCadastrarAgendamento != null)
                {
                    bool bloqueado = Convert.ToByte(dgvSondagem.DataKeys[e.Row.RowIndex]["snd_situacao"]) == (byte)ACA_SondagemSituacao.Bloqueado;
                    btnCadastrarAgendamento.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && !bloqueado;
                    btnCadastrarAgendamento.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }
        
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            _Pesquisar();
            fdsResultados.Visible = true;
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
        }

        #endregion
    }
}