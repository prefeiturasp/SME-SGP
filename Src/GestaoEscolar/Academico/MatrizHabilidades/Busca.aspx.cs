using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Academico.MatrizHabilidades
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// ID do matricula.
        /// </summary>
        public int Edit_Mat_id
        {
            get
            {
                return Convert.ToInt32(gvMatriz.DataKeys[gvMatriz.EditIndex].Values["mat_id"]);
            }
        }

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }
            try
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                {
                    lblMensagem.Text = message;
                    updMessage.Update();
                }

                if (!IsPostBack)
                {
                    VerificaPermissaoUsuario();     
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "MatrizHabilidades.Busca.lblMensagem.ErrorMessage").ToString(), UtilBO.TipoMensagem.Erro);
                updMessage.Update();
            }

            Page.Form.DefaultButton = btnPesquisar.UniqueID;
        }

        #endregion
        
        #region Métodos

        /// <summary>
        /// Verifica se usuário tem permissão de acesso à página.
        /// </summary>
        private void VerificaPermissaoUsuario()
        {
            if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "MatrizHabilidades.Busca.lblMensagem.ErroPermissao").ToString(), UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            pnlPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
        }

        /// <summary>
        /// Método realiza a pesquisa.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                pnlResultado.Visible = false;

                gvMatriz.PageIndex = 0;

                odsMatriz.SelectParameters.Clear();
                odsMatriz.SelectParameters.Add("mat_nome", txtNome.Text);
                odsMatriz.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                
                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                UCComboQtdePaginacao.Valor = itensPagina;
                gvMatriz.PageIndex = 0;
                gvMatriz.PageSize = itensPagina;

                gvMatriz.DataBind();

                pnlResultado.Visible = true;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "MatrizHabilidades.Busca.lblMensagem.ErroPesquisa").ToString(), UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
                pnlResultado.Visible = false;
            }
            finally
            {
                updMessage.Update();
            }
        }


        #endregion

        #region Eventos

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("~/Academico/MatrizHabilidades/Busca.aspx");
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("~/Academico/MatrizHabilidades/Cadastro.aspx");
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            gvMatriz.PageSize = UCComboQtdePaginacao.Valor;
            gvMatriz.PageIndex = 0;
            // atualiza o grid
            gvMatriz.DataBind();
        }

        protected void gvMatriz_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros.Total = ORC_MatrizHabilidadesBO.GetTotalRecords();
            ConfiguraColunasOrdenacao(gvMatriz);
        }
               

        protected void odsMatriz_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Pesquisar();
        }

        protected void gvMatriz_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e != null && e.CommandName != "Page")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int mat_id = Convert.ToInt32(gvMatriz.DataKeys[index].Values[0]);

                if (e.CommandName == "Deletar")
                {
                    try
                    {                       
                        ORC_MatrizHabilidades entity = new ORC_MatrizHabilidades { mat_id = mat_id};
                        ORC_MatrizHabilidadesBO.GetEntity(entity);

                        if (ORC_MatrizHabilidadesBO.Delete(entity))
                        {
                            Pesquisar();

                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "mat_id: " + mat_id);
                            lblMensagem.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", 
                                                "MatrizHabilidades.Busca.gvMatriz.SucessoExclusao").ToString(), 
                                                UtilBO.TipoMensagem.Sucesso);
                        }
                    }
                    catch (ValidationException ex)
                    {
                        ApplicationWEB._GravaErro(ex);
                        lblMensagem.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico",
                                                "MatrizHabilidades.Busca.gvMatriz.ErroValidaExclusao").ToString(),
                                                UtilBO.TipoMensagem.Erro);
                    }
                    catch (Exception ex)
                    {
                        ApplicationWEB._GravaErro(ex);                        
                        lblMensagem.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico",
                                                "MatrizHabilidades.Busca.gvMatriz.ErroExclusao").ToString(),
                                                UtilBO.TipoMensagem.Erro);
                    }
                }
            }
        }

        protected void gvMatriz_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lkbNome = (LinkButton)e.Row.FindControl("lkbNome");
                if (lkbNome != null)
                    lkbNome.CommandArgument = e.Row.RowIndex.ToString();

                ImageButton _imgExcluir = (ImageButton)e.Row.FindControl("_imgExcluir");
                if (_imgExcluir != null)
                    _imgExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        #endregion
    }
}