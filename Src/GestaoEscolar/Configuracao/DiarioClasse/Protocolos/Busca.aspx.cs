using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.BLL;

namespace GestaoEscolar.Configuracao.DiarioClasse.Protocolos
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        private const int indiceColunaUASuperior = 0;
        private const int indiceColunaEtapaEnsino = 2;
        private const int indiceColunaGrupamentoEnsino = 3;
        
        #endregion

        #region Propriedades

        /// <summary>
        /// Recebe o valor do textbox de data inicio.
        /// </summary>
        public DateTime _dataInicio
        {
            get
            {
                return Convert.ToDateTime(txtDataInicio.Text);
            }
        }

        /// <summary>
        /// Recebe o valor do textbox de data fim.
        /// </summary>
        public DateTime _dataFim
        {
            get
            {
                if (!string.IsNullOrEmpty(txtDataFim.Text))
                {
                    return Convert.ToDateTime(txtDataFim.Text);
                }
                return DateTime.Now;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Verifica se o usuário tem permissão de acesso a página.
        /// </summary>
        private bool VerificarPermissaoUsuario()
        {
            if (!(__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir ||
                __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar))
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Realiza a busca com os filtros informados na tela.
        /// </summary>
        private void Pesquisar()
        {
            DateTime dt_inicio;
            DateTime dt_fim;
            try
            {
                DateTime.TryParse(txtDataInicio.Text, out dt_inicio);
                DateTime.TryParse(txtDataFim.Text, out dt_fim);
                // verifico se a data inicial e final estao com 00/00/0000
                if (dt_inicio == new DateTime() && dt_fim == new DateTime())
                {
                    if (String.IsNullOrEmpty(txtDataInicio.Text.Trim()) || String.IsNullOrEmpty(txtDataFim.Text.Trim()))
                    {
                        throw new ValidationException("Data início e data fim são campos obrigatórios.");
                    }
                    else
                    {
                        throw new ValidationException("Data início e data fim são inválidas.");
                    }
                }
                else if (dt_inicio == new DateTime())
                {
                    throw new ValidationException("Data início é inválida.");
                }
                else if (dt_fim == new DateTime())
                {
                    throw new ValidationException("Data fim é inválida.");
                }

                if (_dataInicio > _dataFim)
                {
                    throw new ValidationException("Data inicial não pode ser maior que a data final.");
                }
                else
                {
                    Int16 status = 0;
                    Int16 tipoProtocolo = 0;
                    if (Convert.ToInt16(ddlStatus.SelectedValue) > 0)
                    {
                        status = Convert.ToInt16(ddlStatus.SelectedValue);
                    }

                    if (Convert.ToInt16(ddlTipoProtocolo.SelectedValue) > 0)
                    {
                        tipoProtocolo = Convert.ToInt16(ddlTipoProtocolo.SelectedValue);
                    }

                    odsProtocolos.SelectParameters.Clear();
                    odsProtocolos.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                    odsProtocolos.SelectParameters.Add("dtInicio", Convert.ToDateTime(txtDataInicio.Text).ToString("yyyy/MM/dd"));
                    odsProtocolos.SelectParameters.Add("dtFim", Convert.ToDateTime(txtDataFim.Text).ToString("yyyy/MM/dd"));
                    odsProtocolos.SelectParameters.Add("status", status.ToString());

                    odsProtocolos.SelectParameters.Add("tipoProtocolo", tipoProtocolo.ToString());

                    grvProtocolos.Sort("DataCriacaoOrdenar", SortDirection.Descending);

                    grvProtocolos.PageIndex = 0;
                    grvProtocolos.DataBind();

                    fsResultados.Visible = true;


                }
            }
            catch (ValidationException ex)
            {
                lblMensagemErro.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemErro.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os protocolos.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));

                // chamada para os popups que exibem detalhes do tipo do protocolo selecionado.
                sm.Scripts.Add(new ScriptReference("~/Includes/jsDetalhesProtocolo.js"));
            }

            if (!IsPostBack)
            {
                if (VerificarPermissaoUsuario())
                {
                    txtDataInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtDataFim.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                // carrego o nome da coluna de acordo com o que esta cadastrado em parametros do sistema.

                grvDetalhesProtFoto.Columns[indiceColunaUASuperior].HeaderText = GestaoEscolarUtilBO.nomeUnidadeAdministrativaFiltroEscola(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                grvDetalhesProtFoto.Columns[indiceColunaEtapaEnsino].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                grvDetalhesProtFoto.Columns[indiceColunaGrupamentoEnsino].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }

        protected void odsProtocolos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!IsPostBack)
            {
                // Cancela o select se for a primeira entrada na tela.
                e.Cancel = true;
            }
        }

        protected void grvProtocolos_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = DCL_ProtocoloBO.GetTotalRecords();

            // seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvProtocolos);
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvProtocolos.PageSize = UCComboQtdePaginacao1.Valor;
            grvProtocolos.PageIndex = 0;
            // atualiza o grid
            grvProtocolos.DataBind();
        }

        protected void grvProtocolos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnDetalhes_Select")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int pro_tipo = int.Parse(grvProtocolos.DataKeys[index].Values["pro_tipo"].ToString());  // converte para obter o valor do tipo do protocolo 

                if (pro_tipo == (int)DCL_ProtocoloBO.eTipo.Aula)   // protocolo de aula
                {
                    // prepara os dados para serem exibido no pop-up
                    odsDetalhesProtAula.SelectParameters.Clear();
                    odsDetalhesProtAula.SelectParameters.Add("pro_id", grvProtocolos.DataKeys[index].Values["pro_id"].ToString());
                    grvDetalhesProtAula.Sort("", SortDirection.Ascending);
                    grvDetalhesProtAula.PageIndex = 0;
                    grvDetalhesProtAula.DataBind();
                    // chamada ao pop-up
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtAula').dialog('open'); });", true);
                }
                else if (pro_tipo == (int)DCL_ProtocoloBO.eTipo.JustificativaFaltaAluno) // protocolo de justificativa
                {
                    // *** Não sendo utilizada no momento ***
                    //  Foi retirado do Diario de Classes (Aplicativo Android), mas pedido para deixar as rotinas comentadas   
                    
                    odsDetalhesProtJustificativa.SelectParameters.Clear();
                    odsDetalhesProtJustificativa.SelectParameters.Add("pro_id", grvProtocolos.DataKeys[index].Values["pro_id"].ToString());
                    grvDetalhesProtJustificativa.Sort("", SortDirection.Ascending);
                    grvDetalhesProtJustificativa.PageIndex = 0;
                    grvDetalhesProtJustificativa.DataBind();
                    // chamada ao pop-up
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtJustificativa').dialog('open'); });", true);
                }
                else if (pro_tipo == (int)DCL_ProtocoloBO.eTipo.PlanejamentoAnual) // protocolo de planejamento
                {
                    // prepara os dados para serem exibido no pop-up    
                    odsDetalhesProtPlanejamento.SelectParameters.Clear();
                    odsDetalhesProtPlanejamento.SelectParameters.Add("pro_id", grvProtocolos.DataKeys[index].Values["pro_id"].ToString());
                    grvDetalhesProtPlanejamento.Sort("", SortDirection.Ascending);
                    grvDetalhesProtPlanejamento.PageIndex = 0;
                    grvDetalhesProtPlanejamento.DataBind();
                    // chamada ao pop-up
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtPlanejamento').dialog('open'); });", true);
                }
                else if (pro_tipo == (int)DCL_ProtocoloBO.eTipo.Foto) // protocolos de Fotos
                {
                    // prepara os dados para serem exibido no pop-up    
                    odsDetalhesProtFoto.SelectParameters.Clear();
                    odsDetalhesProtFoto.SelectParameters.Add("pro_id", grvProtocolos.DataKeys[index].Values["pro_id"].ToString());
                    odsDetalhesProtFoto.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                    grvDetalhesProtFoto.Sort("", SortDirection.Ascending);
                    grvDetalhesProtFoto.PageIndex = 0;
                    grvDetalhesProtFoto.DataBind();
                    // chamada ao pop-up
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtFoto').dialog('open'); });", true);
                }
                else if (pro_tipo == (int)DCL_ProtocoloBO.eTipo.CompensacaoDeAula) // protocolos de Compensação de ausência da aula
                {
                    // prepara os dados para serem exibido no pop-up    
                    odsDetalhesProtCompensacaoDeAula.SelectParameters.Clear();
                    odsDetalhesProtCompensacaoDeAula.SelectParameters.Add("pro_id", grvProtocolos.DataKeys[index].Values["pro_id"].ToString());
                    grvDetalhesProtCompensacaoDeAula.Sort("", SortDirection.Ascending);
                    grvDetalhesProtCompensacaoDeAula.PageIndex = 0;
                    grvDetalhesProtCompensacaoDeAula.DataBind();
                    // chamada ao pop-up
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtCompensacaoDeAula').dialog('open'); });", true);
                                                                                                                             
                }
            }
        }

        protected void grvProtocolos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnDetalhes = (ImageButton)e.Row.FindControl("btnDetalhes");
                if (btnDetalhes != null)
                {
                    btnDetalhes.CommandArgument = e.Row.RowIndex.ToString();
                    int pro_status_id = int.Parse(grvProtocolos.DataKeys[e.Row.RowIndex].Values["pro_status_id"].ToString());

                    int pro_tipo = int.Parse(grvProtocolos.DataKeys[e.Row.RowIndex].Values["pro_tipo"].ToString());
                    if (pro_tipo == (int)DCL_ProtocoloBO.eTipo.Logs)  // faço isso para inibir o botao detalhes quando o tipo do protocolo for LOG (4)
                    {
                        btnDetalhes.Visible = false;
                    }
                    else
                    {
                        if (pro_status_id == (int)DCL_ProtocoloBO.eStatus.ProcessadoComSucesso)
                        {
                            // somente exibo quando o protocolo esta processado com sucesso
                            btnDetalhes.Visible = true;
                        }
                        else
                        {
                            btnDetalhes.Visible = false;
                        }
                    }
                }
            }
        }

        protected void UCComboQtdePaginacao_ProtAula_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid - detalhes do protocolo de aula
            grvDetalhesProtAula.PageSize = UCComboQtdePaginacao2.Valor;
            grvDetalhesProtAula.PageIndex = 0;
            // atualiza o grid
            grvDetalhesProtAula.DataBind();
        }

        protected void odsDetalhesProtAula_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!IsPostBack)
            {
                // Cancela o select se for a primeira entrada na tela.
                e.Cancel = true;
            }
        }

        protected void odsDetalhesProtPlanejamento_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!IsPostBack)
            {
                // Cancela o select se for a primeira entrada na tela.
                e.Cancel = true;
            }
        }

        protected void odsDetalhesProtJustificativa_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!IsPostBack)
            {
                // Cancela o select se for a primeira entrada na tela.
                e.Cancel = true;
            }
        }

        protected void odsDetalhesProtFoto_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!IsPostBack)
            {
                // Cancela o select se for a primeira entrada na tela.
                e.Cancel = true;
            }
        }

        protected void odsDetalhesProtCompensacaoDeAula_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!IsPostBack)
            {
                // Cancela o select se for a primeira entrada na tela.
                e.Cancel = true;
            }
        }

        protected void UCComboQtdePaginacao_ProtPlanejamento_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid - detalhes do protocolo de planejamento 
            grvDetalhesProtPlanejamento.PageSize = UCComboQtdePaginacao4.Valor;
            grvDetalhesProtPlanejamento.PageIndex = 0;
            // atualiza o grid
            grvDetalhesProtPlanejamento.DataBind();
        }

        protected void UCComboQtdePaginacao_ProtJustificativa_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid - detalhes do protocolo de justificativa 
            grvDetalhesProtJustificativa.PageSize = UCComboQtdePaginacao3.Valor;
            grvDetalhesProtJustificativa.PageIndex = 0;
            // atualiza o grid
            grvDetalhesProtJustificativa.DataBind();
        }

        protected void UCComboQtdePaginacao_ProtFoto_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid - detalhes do protocolo de Foto
            grvDetalhesProtFoto.PageSize = UCComboQtdePaginacao5.Valor;
            grvDetalhesProtFoto.PageIndex = 0;
            // atualiza o grid
            grvDetalhesProtFoto.DataBind();
        }

        protected void UCComboQtdePaginacao_ProtCompensacaoDeAula_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid - detalhes do protocolo de Compensacao ausência
            grvDetalhesProtCompensacaoDeAula.PageSize = UCComboQtdePaginacao6.Valor;
            grvDetalhesProtCompensacaoDeAula.PageIndex = 0;
            // atualiza o grid
            grvDetalhesProtCompensacaoDeAula.DataBind();
        }
        
        protected void btnFecharDetalhesProtAula_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtAula').dialog('close'); });", true);
        }

        protected void btnFecharDetalhesProtJustificativa_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtJustificativa').dialog('close'); });", true);
        }

        protected void btnFecharDetalhesProtPlanejamento_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtPlanejamento').dialog('close'); });", true);
        }

        protected void btnFecharDetalhesProtFoto_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtFoto').dialog('close'); });", true);
        }

        protected void btnFecharDetalhesProtCompensacaoDeAula_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDetalhesProtCompensacaoDeAula').dialog('close'); });", true);
        }

        #endregion

    }
}