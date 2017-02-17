using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Academico.NivelAprendizado
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Id do nível de aprendizado.
        /// </summary>
        private int VS_nap_id
        {
            get
            {
                if (ViewState["VS_nap_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_nap_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_nap_id"] = value;
            }
        }

        public int EditItem_Nap_id
        {
            get
            {
                return Convert.ToInt32(grvNivelAprendizado.DataKeys[grvNivelAprendizado.EditIndex].Values["nap_id"]);
            }
        }
                
        /// <summary>
        /// DataTable dos Níveis de Aprendizado
        /// </summary>
        private DataTable VS_Dt_NivelAprendizado
        {
            get
            {
                if (ViewState["VS_Dt_NivelAprendizado"] != null)
                    return (DataTable)ViewState["VS_Dt_NivelAprendizado"];
                VS_Dt_NivelAprendizado = new DataTable();
                VS_Dt_NivelAprendizado.Columns.Add("nap_id");
                VS_Dt_NivelAprendizado.Columns.Add("nap_descricao");
                VS_Dt_NivelAprendizado.Columns.Add("nap_sigla");
                VS_Dt_NivelAprendizado.Columns.Add("nap_situacao");
                VS_Dt_NivelAprendizado.Columns.Add("nap_dataCriacao");
                return VS_Dt_NivelAprendizado;
            }
            set
            {
                ViewState["VS_Dt_NivelAprendizado"] = value;
            }
        }

        #endregion

        #region Constantes

        private const int INDEX_COLUNA_CURSO = 2;

        #endregion Constantes

        #region Eventos

        #region Page Life Cycle

        protected void Page_init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                grvNivelAprendizado.Columns[INDEX_COLUNA_CURSO].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
            }

            UCComboQtdePaginacao1.GridViewRelacionado = grvNivelAprendizado;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMensagem.Text = message;

                try
                {
                    CarregaNiveisAprendizado();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                // Configura permissões
                grvNivelAprendizado.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnAdicionar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        #endregion

        protected void grvNivelAprendizado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("imgExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void grvNivelAprendizado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int nap_id = Convert.ToInt32(grvNivelAprendizado.DataKeys[index].Value);

                    //Verifica se existe alguma orientação para esse nível de aprendizado
                    DataTable dtNiveisAprendizadoORC = ORC_OrientacaoCurricularNivelAprendizadoBO.SelectNivelAprendizadoByOcrId(0, nap_id);
                    if (dtNiveisAprendizadoORC.Rows.Count > 0)
                    {
                        throw new ValidationException("Não foi possível realizar a exclusão, pois existe uma orientação curricular cadastrada para esse nível de aprendizado.");
                    }

                    ORC_NivelAprendizado entity = new ORC_NivelAprendizado { nap_id = nap_id };
                    ORC_NivelAprendizadoBO.GetEntity(entity);
                    entity.nap_situacao = 3;
                    entity.nap_dataAlteracao = DateTime.Now;
                    entity.IsNew = false;

                    ORC_NivelAprendizadoBO.Save(entity);

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "nap_id: " + entity.nap_id);
                    lblMensagem.Text = UtilBO.GetErroMessage("Nível de aprendizado excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    CarregaNiveisAprendizado();
                }
                catch (ValidationException ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o nível de aprendizado.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void odsNivelAprendizado_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                if (e.Exception.InnerException is ValidationException)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(e.Exception.InnerException.Message, UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    ApplicationWEB._GravaErro(e.Exception.InnerException);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os níveis de aprendizado.", UtilBO.TipoMensagem.Erro);
                }

                e.ExceptionHandled = true;
            }
        }

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Academico/NivelAprendizado/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvNivelAprendizado.PageSize = UCComboQtdePaginacao1.Valor;
            grvNivelAprendizado.PageIndex = 0;
            // atualiza o grid
            grvNivelAprendizado.DataBind();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega dados no grid.
        /// </summary>
        private void CarregaNiveisAprendizado()
        {
            try
            {
                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvNivelAprendizado.PageIndex = 0;
                grvNivelAprendizado.PageSize = itensPagina;
                odsNivelAprendizado.SelectParameters.Clear();
                odsNivelAprendizado.SelectParameters.Add("cur_id", "0");
                odsNivelAprendizado.SelectParameters.Add("crr_id", "0");
                odsNivelAprendizado.SelectParameters.Add("crp_id", "0");
                odsNivelAprendizado.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
               
                grvNivelAprendizado.DataSourceID = odsNivelAprendizado.ID;
                grvNivelAprendizado.DataBind();

                updNivelAprendizado.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os níveis de aprendizado.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

    }
}