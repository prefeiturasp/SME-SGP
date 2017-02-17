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
using MSTech.GestaoEscolar.Entities;


namespace GestaoEscolar.Configuracao.DiarioClasse.ConsultaEquipamentos
{
    public partial class Busca : MotherPageLogado
    {
        
        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                //sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaAlunosMatriculados.js"));
            }
            if (!IsPostBack)
            {
                try
                {
                    uccUaEscola.Focus();
                    uccUaEscola.EnableEscolas = true;
                    uccUaEscola.FiltroEscola = true;
                    uccUaEscola.CarregarEscolaAutomatico = true;
                    uccUaEscola.Inicializar();
                    this.VerificaBusca();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagemErro.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
                Page.Form.DefaultFocus = uccUaEscola.VisibleUA ? uccUaEscola.ComboUA_ClientID : uccUaEscola.ComboEscola_ClientID;

                btn_pesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            }
        }

        #endregion

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // Atribui nova quantidade de itens por página para o grid.
            grvConsultaEquipamentos.PageSize = UCComboQtdePaginacao1.Valor;
            grvConsultaEquipamentos.PageIndex = 0;

            // Atualiza o grid
            grvConsultaEquipamentos.DataBind();
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
        /// Realiza pesquisa de equipamentos
        /// </summary>
        private void Pesquisar()
        {
            try
            {                                   
                    odsConsultaEquipamentos.SelectParameters.Clear();
                    odsConsultaEquipamentos.SelectParameters.Add("esc_id", uccUaEscola.Esc_ID.ToString());
                    odsConsultaEquipamentos.SelectParameters.Add("uni_id", uccUaEscola.Uni_ID.ToString());                   
                    odsConsultaEquipamentos.SelectParameters.Add("equ_descricao", txtDescricao.Text);
                
                    //grvConsultaEquipamentos.Sort("Escola", SortDirection.Descending);

                    grvConsultaEquipamentos.PageIndex = 0;
                   
                #region Salvar busca realizada com os parâmetros do ODS.
                    
                    Dictionary<string, string> filtros = this.odsConsultaEquipamentos.SelectParameters.Cast<Parameter>().ToDictionary(param => param.Name, param => param.DefaultValue);
                    filtros.Add("ua_superior", uccUaEscola.Uad_ID.ToString());
                    __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.ConsultaEquipamentos, Filtros = filtros };                    

                    #endregion

                    grvConsultaEquipamentos.DataBind();

                    fdsResultados.Visible = true;      
          
            }
            catch (ValidationException ex)
            {
                lblMensagemErro.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemErro.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os equipamentos.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta, 
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConsultaEquipamentos)
            {
                // Recuperar busca realizada e btnPesquisar automaticamente.
                string valor1;

                if (uccUaEscola.FiltroEscola)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ua_superior", out valor1);

                    if (!string.IsNullOrEmpty(valor1))
                    {
                        uccUaEscola.DdlUA.SelectedValue = valor1;
                    }

                    if (valor1 != Guid.Empty.ToString())
                    {
                        uccUaEscola.CarregaEscolaPorUASuperiorSelecionada();
                        SelecionarEscola();
                    }
                }
                else
                {
                    SelecionarEscola();
                }
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("equ_descricao", out valor1);
                txtDescricao.Text = valor1;

                Pesquisar();
            }
            else
            {
                fdsResultados.Visible = false;
            }
        }

        /// <summary>
        /// Seleciona a escola no combo de acordo com o parâmetro salvo na sessão de busca 
        /// realizada.
        /// </summary>
        private void SelecionarEscola()
        {            
            string uni_id;
            string esc_id;

            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)) &&
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id))
            {
                uccUaEscola.SelectedValueEscolas = new int[]{Convert.ToInt32(esc_id), Convert.ToInt32(uni_id)};
                uccUaEscola.EnableEscolas = !(uccUaEscola.VisibleUA && uccUaEscola.Uad_ID == Guid.Empty);
            }
        }
        
        #endregion

        #region Eventos

        protected void odsConsultaEquipamentos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!IsPostBack)
            {
                // Cancela o select se for a primeira entrada na tela.
                e.Cancel = true;
            }
        }

        protected void ConsultaEquipamentos_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = SYS_EquipamentoBO.GetTotalRecords();

            // seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvConsultaEquipamentos);
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }

        #endregion
    }
}