using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Classe.LancamentoSondagem
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        /// <summary>Posição da coluna Responder no grid view da Sondagem.</summary>
        private const int PosicaoResponder = 4;

        #endregion

        #region Enumeradores

        /// <summary>
        /// Situações da sondagem
        /// </summary>
        private enum SituacaoSondagemLancamento : byte
        {
            Vigente = 1
            ,

            VigenteComLançamento = 2
            ,

            VigenteSemLançamento = 3
            ,

            VigenciaEncerrada = 4
        }

        #endregion Enumeradores

        #region Propriedades

        /// <summary>
        /// Retorna o id do registro que está sendo respondido.
        /// </summary>
        public int[] EditItem
        {
            get
            {
                int[] retorno = new int[3];
                if (dgvSondagem.EditIndex >= 0)
                {
                    retorno[0] = Convert.ToInt32(dgvSondagem.DataKeys[dgvSondagem.EditIndex]["snd_id"]);
                    retorno[1] = Convert.ToInt32(dgvSondagem.DataKeys[dgvSondagem.EditIndex]["sda_id"]);
                    retorno[2] = Convert.ToInt32(dgvSondagem.DataKeys[dgvSondagem.EditIndex]["snd_opcaoResposta"]);
                }
                else
                {
                    retorno[0] = -1;
                    retorno[1] = -1;
                    retorno[2] = -1;
                }
                return retorno;
            }
        }

        /// <summary>
        /// Retorna o id do registro que está selecionado.
        /// </summary>
        public int[] SelectedItem
        {
            get
            {
                int[] retorno = new int[3];
                if (dgvSondagem.SelectedIndex >= 0)
                {
                    retorno[0] = Convert.ToInt32(dgvSondagem.DataKeys[dgvSondagem.SelectedIndex]["snd_id"]);
                    retorno[1] = Convert.ToInt32(dgvSondagem.DataKeys[dgvSondagem.SelectedIndex]["sda_id"]);
                    retorno[2] = Convert.ToInt32(dgvSondagem.DataKeys[dgvSondagem.SelectedIndex]["snd_opcaoResposta"]);
                }
                else
                {
                    retorno[0] = -1;
                    retorno[1] = -1;
                    retorno[2] = -1;
                }
                return retorno;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LancamentoSondagem)
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
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LancamentoSondagem)
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

        private bool existeResponder = false;

        #endregion

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            existeResponder = false;
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
                odsSondagem.SelectParameters.Add("sda_dataInicio", string.IsNullOrEmpty(txtDataInicio.Text) ? string.Empty : Convert.ToDateTime(txtDataInicio.Text).ToString("yyyy/MM/dd"));
                odsSondagem.SelectParameters.Add("sda_dataFim", string.IsNullOrEmpty(txtDataFim.Text) ? string.Empty : Convert.ToDateTime(txtDataFim.Text).ToString("yyyy/MM/dd"));
                odsSondagem.SelectParameters.Add("situacao", ddlSituacao.SelectedValue);
                odsSondagem.SelectParameters.Add("doc_id", __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                                                                __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString()
                                                                : "-1");
                odsSondagem.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                odsSondagem.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                odsSondagem.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
                odsSondagem.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                // quantidade de itens por página            
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                dgvSondagem.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                foreach (Parameter param in odsSondagem.SelectParameters)
                {
                    filtros.Add(param.Name, param.DefaultValue);
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.LancamentoSondagem
                    ,
                    Filtros = filtros
                };

                #endregion

                existeResponder = false;
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
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar as sondagens.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LancamentoSondagem)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("snd_titulo", out valor);
                txtTitulo.Text = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("sda_dataInicio", out valor);
                txtDataInicio.Text = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("sda_dataFim", out valor);
                txtDataFim.Text = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("situacao", out valor);
                ddlSituacao.SelectedValue = valor;

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
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
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
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsSondagem.ClientID, string.Format("MsgInformacao('{0}');", string.Concat("#", fdsSondagem.ClientID)), true);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
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

            if (!string.IsNullOrEmpty(dgvSondagem.SortExpression) && __SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.LancamentoSondagem)
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
                    PaginaBusca = PaginaGestao.LancamentoSondagem
                    ,
                    Filtros = filtros
                };
            }

            dgvSondagem.Columns[PosicaoResponder].Visible = existeResponder;
        }

        protected void dgvSondagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                byte situacao = Convert.ToByte(dgvSondagem.DataKeys[e.Row.RowIndex]["situacao"]);

                Label lblRespostas = (Label)e.Row.FindControl("lblRespostas");
                if (lblRespostas != null)
                {
                    // Quando a sondagem está vigente e o usuário tem permissão para responder,
                    // então não é necessário apenas visualizar as respostas.
                    lblRespostas.Visible = (
                                                situacao == (byte)SituacaoSondagemLancamento.Vigente
                                                || situacao == (byte)SituacaoSondagemLancamento.VigenteComLançamento
                                                || situacao == (byte)SituacaoSondagemLancamento.VigenteSemLançamento
                                            )
                                            && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                LinkButton btnRespostas = e.Row.FindControl("btnRespostas") as LinkButton;
                if (btnRespostas != null)
                {
                    // Quando o usuário tem permissão apenas para consultar as respostas,
                    // ou quando a sondagem já foi encerrada e o usuário tinha permissão para responder,
                    // então habilita o botão para visualizar as respostas.
                    btnRespostas.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                            || (situacao == (byte)SituacaoSondagemLancamento.VigenciaEncerrada
                                                && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar);
                }

                ImageButton btnResponder = e.Row.FindControl("btnResponder") as ImageButton;
                if (btnResponder != null)
                {
                    // Quando a sondagem está vigente e o usuário tem permissão para responder,
                    // então habilita o botão para responder.
                    btnResponder.Visible = (
                                                situacao == (byte)SituacaoSondagemLancamento.Vigente
                                                || situacao == (byte)SituacaoSondagemLancamento.VigenteComLançamento
                                                || situacao == (byte)SituacaoSondagemLancamento.VigenteSemLançamento
                                            )
                                            && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                    if (btnResponder.Visible)
                    {
                        existeResponder = true;
                    }
                }
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTitulo.Text)
                || !string.IsNullOrEmpty(txtDataInicio.Text)
                || !string.IsNullOrEmpty(txtDataFim.Text)
                || Convert.ToByte(ddlSituacao.SelectedValue) > 0)
            {
                _Pesquisar();
                fdsResultados.Visible = true;
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Classe", "LancamentoSondagem.Busca.MsgPreencherFiltro").ToString(), UtilBO.TipoMensagem.Alerta);
            }
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