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
using ReportNameDocumentos = MSTech.GestaoEscolar.BLL.ReportNameDocumentos;

namespace GestaoEscolar.Configuracao.RelatorioDocumentoAluno
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        private IDictionary<ReportNameDocumentos, string[]> relatorios;

        private IDictionary<ReportNameDocumentos, string[]> Relatorios
        {
            get { return relatorios ?? (relatorios = CFG_RelatorioDocumentoAlunoBO.SelecionaRelatorios()); }
        }

        #endregion Propriedades

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
                grvDocumentos.DataBind();
        }

        #endregion Page Life Cycle

        #region Eventos

        protected void grvDocumentos_DataBinding(object sender, EventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (grv.DataSource == null)
                    grv.DataSource = CFG_RelatorioDocumentoAlunoBO.SelecionaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os documentos do aluno.", UtilBO.TipoMensagem.Erro);
                updMessage.Update();
            }
        }

        protected void grvDocumentos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                if (Convert.ToInt32((grv.DataKeys[e.RowIndex]["rda_id"].ToString())) > 0)
                {
                    CFG_RelatorioDocumentoAluno entity = new CFG_RelatorioDocumentoAluno
                    {
                        ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                        ,
                        rlt_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["rlt_id"].ToString())
                        ,
                        rda_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["rda_id"].ToString())
                    };

                    if (CFG_RelatorioDocumentoAlunoBO.Delete(entity))
                    {
                        CFG_RelatorioDocumentoAlunoBO.RecarregaDocumentosAtivos();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "ent_id: " + entity.ent_id + ",rlt_id: " + entity.rlt_id + ",rda_id: " + entity.rda_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Documento do aluno excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        grv.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir documento do aluno.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                updMessage.Update();
            }
        }

        protected void grvDocumentos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = e.NewEditIndex;
            grv.DataBind();

            DropDownList ddlRelatorios = (DropDownList)grvDocumentos.Rows[e.NewEditIndex].FindControl("ddlRelatorios");
            if (ddlRelatorios != null)
            {
                ddlRelatorios.Items.Clear();

                ddlRelatorios.Items.Add(new ListItem("-- Selecione um relatório --", "-1"));

                foreach (KeyValuePair<ReportNameDocumentos, string[]> item in Relatorios)
                {
                    ddlRelatorios.Items.Add(new ListItem(item.Value[0], ((int)item.Key).ToString()));
                }

                ddlRelatorios.SelectedValue = grvDocumentos.DataKeys[e.NewEditIndex]["rlt_id"].ToString();
            }

            ImageButton imgSalvar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("imgSalvar");
            if (imgSalvar != null)
                imgSalvar.Visible = true;
            ImageButton imgEditar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("imgEditar");
            if (imgEditar != null)
            {
                imgEditar.Visible = false;
                ImageButton imgCancelar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("imgCancelar");
                if (imgCancelar != null)
                    imgCancelar.Visible = true;
            }
            grv.Rows[e.NewEditIndex].Focus();
        }

        protected void grvDocumentos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                CFG_RelatorioDocumentoAluno entity = new CFG_RelatorioDocumentoAluno
                {
                    IsNew = !(Convert.ToInt32(grv.DataKeys[e.RowIndex]["rda_id"].ToString()) > 0)
                    ,
                    ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    ,
                    rda_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["rda_id"].ToString())
                    ,
                    rda_situacao = Convert.ToByte(grv.DataKeys[e.RowIndex]["rda_situacao"].ToString())
                };

                DropDownList ddlRelatorios = (DropDownList)grvDocumentos.Rows[e.RowIndex].FindControl("ddlRelatorios");
                if (ddlRelatorios != null)
                    entity.rlt_id = Convert.ToInt32(ddlRelatorios.SelectedValue);

                TextBox txtNome = (TextBox)grvDocumentos.Rows[e.RowIndex].FindControl("txtNome");
                if (txtNome != null)
                    entity.rda_nomeDocumento = txtNome.Text;

                TextBox txtOrdem = (TextBox)grvDocumentos.Rows[e.RowIndex].FindControl("txtOrdem");
                if (txtOrdem != null)
                    entity.rda_ordem = Convert.ToInt32(txtOrdem.Text);

                if (CFG_RelatorioDocumentoAlunoBO.Salvar(entity))
                {
                    CFG_RelatorioDocumentoAlunoBO.RecarregaDocumentosAtivos();
                    if (Convert.ToInt32(grv.DataKeys[e.RowIndex]["rda_id"].ToString()) > 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "ent_id: " + entity.ent_id + " ,rda_id: " + entity.rda_id + ",rlt_id: " + entity.rlt_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Documento do aluno alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "ent_id: " + entity.ent_id + " ,rda_id: " + entity.rda_id + ",rlt_id: " + entity.rlt_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Documento do aluno incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    grv.EditIndex = -1;
                    grv.DataBind();
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar documento.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                updMessage.Update();
            }
        }

        protected void grvDocumentos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = -1;
            grv.DataBind();
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = CFG_RelatorioDocumentoAlunoBO.SelecionaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                DataRow dr = dt.NewRow();
                dr["ent_id"] = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                dr["rda_id"] = -1;
                dr["rda_situacao"] = 1;

                dt.Rows.Add(dr);

                int index = (dt.Rows.Count - 1);
                grvDocumentos.EditIndex = index;
                grvDocumentos.DataSource = dt;
                grvDocumentos.DataBind();

                DropDownList ddlRelatorios = (DropDownList)grvDocumentos.Rows[index].FindControl("ddlRelatorios");
                if (ddlRelatorios != null)
                {
                    ddlRelatorios.Items.Clear();

                    ddlRelatorios.Items.Add(new ListItem("-- Selecione um relatório --", "-1"));

                    foreach (KeyValuePair<ReportNameDocumentos, string[]> item in Relatorios)
                    {
                        ddlRelatorios.Items.Add(new ListItem(item.Value[0], ((int)item.Key).ToString()));
                    }

                    ddlRelatorios.SelectedValue = grvDocumentos.DataKeys[index]["rlt_id"].ToString();
                }

                TextBox txtOrdem = (TextBox)grvDocumentos.Rows[index].FindControl("txtOrdem");
                if (txtOrdem != null)
                    txtOrdem.Text = string.Empty;

                ImageButton imgEditar = (ImageButton)grvDocumentos.Rows[index].FindControl("imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = false;
                ImageButton imgSalvar = (ImageButton)grvDocumentos.Rows[index].FindControl("imgSalvar");
                if (imgSalvar != null)
                    imgSalvar.Visible = true;
                ImageButton imgCancelar = (ImageButton)grvDocumentos.Rows[index].FindControl("imgCancelarDoc");
                if (imgCancelar != null)
                    imgCancelar.Visible = true;

                ImageButton imgExcluir = (ImageButton)grvDocumentos.Rows[index].FindControl("imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = false;
                grvDocumentos.Rows[index].Focus();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar novo documento.", UtilBO.TipoMensagem.Erro);
                updMessage.Update();
            }
        }

        #endregion Eventos
    }
}