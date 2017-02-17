using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.MotivoInfrequencia
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Eventos Page Life Cycle
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroMotivoInfrequencia.js"));
            }

            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                        lblMessage.Text = message;

                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        uppPrincipal.Visible = uppPrincipal.Visible = false;
                        lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                    }

                    // atualiza accordion
                    carregaAreas();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o cadastro de motivos de infrequência.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion

        #region Eventos
        protected void btnAdicionaNovaArea_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                AdicionarNovaArea();
            }
        }

        /// <summary>
        /// Método utilizado para adicionar uma nova área e invocar o método para salvar.
        /// </summary>
        private void AdicionarNovaArea()
        {
            // monto entidade para incluir área
            ACA_MotivoBaixaFrequencia entity = new ACA_MotivoBaixaFrequencia
            {
                mbf_descricao = txtNome.Text,
                mbf_tipo = 1,
                IsNew = true
            };
            SalvarMotivo(entity);

            // movo -1 para iniciar com todos os accords fechados
            hdnAreaSelecionada.Value = "-1";

            // atualiza accordion
            carregaAreas();

            txtNome.Text = string.Empty;
        }

        // Áreas
        protected void rptAreasInfrequencia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int mbf_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mbf_id"));
                GridView gvItensArea = (GridView)e.Item.FindControl("gvItensArea");
                Button btnExcluirArea = (Button)e.Item.FindControl("btnExcluirArea");

                if (btnExcluirArea != null)
                {
                    string script = String.Format("SetConfirmDialogButton('{0}','{1}');", String.Concat("#", btnExcluirArea.ClientID),
                            "Confirma a exclusão?");
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), btnExcluirArea.ClientID, script, true);
                }
                // carrega itens da área
                gvItensArea.DataSource = ACA_MotivoBaixaFrequenciaBO.Seleciona_Entidades_ItensMotivoInfrequencia(mbf_id);
                gvItensArea.DataBind();
            }
        }

        protected void rptAreasInfrequencia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Button btnExcluir = rptAreasInfrequencia.Items[e.Item.ItemIndex].FindControl("btnExcluirArea") as Button;

            if (e.CommandName == "Deletar")
            {
                try
                {
                    HiddenField hdnMbf_id = rptAreasInfrequencia.Items[e.Item.ItemIndex].FindControl("hdnMbf_id") as HiddenField;
                    int mbf_id = Convert.ToInt32(hdnMbf_id.Value);

                    ACA_MotivoBaixaFrequencia entity = new ACA_MotivoBaixaFrequencia
                    {
                        mbf_id = mbf_id
                    };
                    ACA_MotivoBaixaFrequenciaBO.GetEntity(entity);

                    if (ACA_MotivoBaixaFrequenciaBO.Delete(entity))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "mbf_id: " + mbf_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Área excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

                        // movo -1 para iniciar com todos os accords fechados
                        hdnAreaSelecionada.Value = "-1";

                        // atualiza accordion
                        carregaAreas();
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a área.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnAdicionaNovoItem_Click(object sender, EventArgs e)
        {
            Button btnAdiciona = (Button)sender;
            RepeaterItem rpt = (RepeaterItem)btnAdiciona.NamingContainer;
            if (Page.IsValid)
            {
                AdicionarNovoItem(rpt);
            }
        }

        /// <summary>
        /// Método utilizado para adicionar um novo item na área selecionada.
        /// </summary>
        /// <param name="rpt"></param>
        private void AdicionarNovoItem(RepeaterItem rpt)
        {
            try
            {
                TextBox txtSiglaItem = (TextBox)rpt.FindControl("txtSiglaItem");
                TextBox txtDescricaoItem = (TextBox)rpt.FindControl("txtDescricaoItem");
                HiddenField hdnMbf_id = (HiddenField)rpt.FindControl("hdnMbf_id");
                GridView gvItensArea = (GridView)rpt.FindControl("gvItensArea");

                int mbf_id = Convert.ToInt32(hdnMbf_id.Value);

                hdnAreaSelecionada.Value = rpt.ItemIndex.ToString();

                if (String.IsNullOrEmpty(txtSiglaItem.Text))
                {
                    throw new ValidationException("Código do item é obrigatório.");
                }

                if (String.IsNullOrEmpty(txtDescricaoItem.Text))
                {
                    throw new ValidationException("Descrição do item é obrigatório.");
                }

                // monto entidade para gravar o item da área selecionada.    
                ACA_MotivoBaixaFrequencia entity = new ACA_MotivoBaixaFrequencia
                {
                    mbf_sigla = txtSiglaItem.Text,
                    mbf_descricao = txtDescricaoItem.Text,
                    mbf_idPai = mbf_id,
                    mbf_tipo = 2,
                    IsNew = true
                };

                SalvarMotivo(entity);

                // carrega itens da área
                gvItensArea.DataSource = ACA_MotivoBaixaFrequenciaBO.Seleciona_Entidades_ItensMotivoInfrequencia(mbf_id);
                gvItensArea.DataBind();

                // inicializo os campos para não ficar preenchido após a inclusão.
                txtSiglaItem.Text = txtDescricaoItem.Text = String.Empty;

            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }

        }

        /// <summary>
        /// Método utilizado para solicitar a gravação das informações no banco de dados.
        /// </summary>
        /// <param name="entity"></param>
        private void SalvarMotivo(ACA_MotivoBaixaFrequencia entity)
        {
            try
            {
                ACA_MotivoBaixaFrequenciaDAO dao = new ACA_MotivoBaixaFrequenciaDAO();
                if (dao.Salvar(entity))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "mbf_id: " + entity.mbf_id);

                    if (entity.IsNew)
                    {
                        lblMessage.Text = UtilBO.GetErroMessage(
                            (entity.mbf_tipo == 1 ? "Motivo " : "Item do motivo") + " de infrequência incluído(a) com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        if (entity.mbf_tipo == 2)  // itens da área
                            lblMessage.Text = UtilBO.GetErroMessage("Item do motivo de infrequência alterado(a) com sucesso.", UtilBO.TipoMensagem.Erro);
                    }
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o(a) motivo de infrequência.", UtilBO.TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gravar o motivos de infrequência.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void gvItensArea_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void gvItensArea_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeletarItem")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    GridView gvItens = (GridView)sender;
                    int mbf_id = Convert.ToInt32(gvItens.DataKeys[index].Values["mbf_id"].ToString());
                    int mbf_idPai = Convert.ToInt32(gvItens.DataKeys[index].Values["mbf_idPai"].ToString());
                    ACA_MotivoBaixaFrequencia entity = new ACA_MotivoBaixaFrequencia
                    {
                        mbf_id = mbf_id
                    };
                    ACA_MotivoBaixaFrequenciaBO.GetEntity(entity);

                    if (ACA_MotivoBaixaFrequenciaBO.Delete(entity))  // executa a exclusao 
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "mbf_id: " + mbf_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Item excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

                        // carrega itens da área
                        gvItens.DataSource = ACA_MotivoBaixaFrequenciaBO.Seleciona_Entidades_ItensMotivoInfrequencia(mbf_idPai);
                        gvItens.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o item.", UtilBO.TipoMensagem.Erro);
                }
            }

        }

        /// <summary>
        /// Método utilizado para carregar/exibir todas as áreas cadastradas.
        /// </summary>
        private void carregaAreas()
        {
            DataTable dtResultados = ACA_MotivoBaixaFrequenciaBO.SelecionarAtivos();
            rptAreasInfrequencia.DataSource = dtResultados.Select("mbf_tipo = 1").Any() ? dtResultados.Select("mbf_tipo = 1").CopyToDataTable() : dtResultados.Clone();

            rptAreasInfrequencia.DataBind();
        }

        #endregion
    }
}