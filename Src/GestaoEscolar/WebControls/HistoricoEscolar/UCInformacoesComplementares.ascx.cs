using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.HistoricoEscolar
{
    public partial class UCInformacoesComplementares : MotherUserControl
    {

        #region Propriedades

        protected long VS_alu_id
        {
            get
            {
                if (ViewState["VS_alu_id"] != null)
                    return Convert.ToInt64(ViewState["VS_alu_id"]);

                return -1;
            }
            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        protected int VS_aho_id
        {
            get
            {
                if (ViewState["VS_aho_id"] != null)
                    return Convert.ToInt32(ViewState["VS_aho_id"]);

                return -1;
            }
            set
            {
                ViewState["VS_aho_id"] = value;
            }
        }

        protected int VS_alh_id
        {
            get
            {
                if (ViewState["VS_alh_id"] != null)
                    return Convert.ToInt32(ViewState["VS_alh_id"]);

                return -1;
            }
            set
            {
                ViewState["VS_alh_id"] = value;
            }
        }

        public string message
        {
            set
            {
                lblMessage.Text = value;
            }
        }

        #endregion Propriedades

        #region Delegate

        public delegate void onClickVisualizar();
        public event onClickVisualizar clickVisualizar;

        public delegate void onClickVoltar();
        public event onClickVoltar clickVoltar;

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega os dados do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        public void Carregar(Int64 alu_id)
        {
            VS_alu_id = alu_id;

            odsObservacao.SelectParameters.Add("alu_id", alu_id.ToString());
            odsObservacao.SelectParameters.Add("bancoGestao", null);
            grvObservacao.DataBind();

            odsCertificado.SelectParameters.Add("alu_id", alu_id.ToString());
            grvCertificado.DataBind();

            // Mostra a div de certificados se for possível adicionar um novo (existeAnosDisponiveis), ou se ja existem certificados cadastrados
            //bool existeAnosDisponiveis = ACA_AlunoHistoricoCertificadoBO.SelecionaAnosDisponiveis(VS_alu_id).Rows.Count > 0;
            //divCertificado.Visible = existeAnosDisponiveis || grvCertificado.Rows.Count > 0;
            //btnCertificado.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && existeAnosDisponiveis;
        }

        #endregion Métodos

        #region Eventos

        #region Page-Life cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCInformacoesComplementares.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/ckeditor/ckeditor.js"));
            }

            #region Config CKEditor

            string script = string.Empty;

            txtObservacaoHtml.config.toolbar = new object[]
                                        {
                                            new object[]
                                                {
                                                    "Cut", "Copy", "-", "Paste", "PasteText", "-", "Undo",
                                                    "Redo", "-", "Find", "Replace", "-", "SelectAll", "RemoveFormat", "-",
                                                },
                                            new object[]
                                                {
                                                    "Bold", "Italic", "Underline", "Strike"
                                                },
                                            new object[]
                                                {
                                                    "Outdent", "Indent", "-", "JustifyLeft", "JustifyCenter",
                                                    "JustifyRight", "JustifyBlock", "-", "Preview", "-", "About"
                                                },
                                        };

            #endregion Config CKeditor

            btnObservacao.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

            lblMensagemApareceHistorico.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.lblMensagemApareceHistorico.Text").ToString(), UtilBO.TipoMensagem.Informacao);
        }

        #endregion Page-Life cycle

        protected void grvObservacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    long alu_id = Convert.ToInt64(grvObservacao.DataKeys[index].Values[0]);
                    int aho_id = Convert.ToInt32(grvObservacao.DataKeys[index].Values[1]);

                    ACA_AlunoHistoricoObservacao entity = new ACA_AlunoHistoricoObservacao
                    {
                        alu_id = alu_id,
                        aho_id = aho_id
                    };
                    ACA_AlunoHistoricoObservacaoBO.GetEntity(entity);

                    if (ACA_AlunoHistoricoObservacaoBO.Delete(entity))
                    {
                        grvObservacao.PageIndex = 0;
                        grvObservacao.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, string.Format("alu_id: {0} aho_id: {1}", alu_id, aho_id));
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.grvObservacao.ExcluidoSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.grvObservacao.ExcluidoErro").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Alterar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    VS_alu_id = Convert.ToInt64(grvObservacao.DataKeys[index].Values[0]);
                    VS_aho_id = Convert.ToInt32(grvObservacao.DataKeys[index].Values[1]);

                    ACA_AlunoHistoricoObservacao entity = new ACA_AlunoHistoricoObservacao
                    {
                        alu_id = VS_alu_id,
                        aho_id = VS_aho_id
                    };
                    ACA_AlunoHistoricoObservacaoBO.GetEntity(entity);

                    txtObservacaoHtml.Text = entity.aho_observacao;

                    DataTable dtObsPadrao = ACA_HistoricoObservacaoPadraoBO.SelecionaPorTipo(Convert.ToInt32(ACA_HistoricoObservacaoPadraoTipo.Observacao));

                    rptObservacoesPadroes.DataSource = dtObsPadrao;
                    rptObservacoesPadroes.DataBind();

                    divObsPadrao.Visible = dtObsPadrao.Rows.Count > 0;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AbreObservacaoAlteracao", "$(document).ready(function(){ $('.divCadastroObservacao').dialog('open'); });", true);


                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.grvObservacao.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvCertificado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    long alu_id = Convert.ToInt64(grvCertificado.DataKeys[index].Values[0]);
                    int alh_id = Convert.ToInt32(grvCertificado.DataKeys[index].Values[1]);

                    ACA_AlunoHistoricoCertificado entity = new ACA_AlunoHistoricoCertificado
                    {
                        alu_id = alu_id,
                        alh_id = alh_id
                    };
                    ACA_AlunoHistoricoCertificadoBO.GetEntity(entity);

                    if (ACA_AlunoHistoricoCertificadoBO.Delete(entity))
                    {
                        grvCertificado.PageIndex = 0;
                        grvCertificado.DataBind();

                        btnCertificado.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir
                                                    && ACA_AlunoHistoricoCertificadoBO.SelecionaAnosDisponiveis(VS_alu_id).Rows.Count > 0;

                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, string.Format("alu_id: {0} alh_id: {1}", alu_id, alh_id));
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.grvCertificado.ExcluidoSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.grvCertificado.ExcluidoErro").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Alterar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    VS_alu_id = Convert.ToInt64(grvCertificado.DataKeys[index].Values[0]);
                    VS_alh_id = Convert.ToInt32(grvCertificado.DataKeys[index].Values[1]);

                    ACA_AlunoHistoricoCertificado entity = new ACA_AlunoHistoricoCertificado
                    {
                        alu_id = VS_alu_id,
                        alh_id = VS_alh_id
                    };
                    ACA_AlunoHistoricoCertificadoBO.GetEntity(entity);

                    // Limpa campos 
                    txtFolha.Text = entity.ahc_folha;
                    txtGDAE.Text = entity.ahc_gdae;
                    txtLivro.Text = entity.ahc_livro;
                    txtNumero.Text = entity.ahc_numero;

                    ddlAnoHistorico.DataSource = ACA_AlunoHistoricoCertificadoBO.SelecionaAnosDisponiveis(VS_alu_id);
                    ddlAnoHistorico.DataBind();
                    ddlAnoHistorico.Items.Insert(0, new ListItem(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.ddlAnoHistorico.Item0").ToString(), "-1"));
                    ddlAnoHistorico.SelectedValue = entity.alh_id.ToString();
                    ddlAnoHistorico.Enabled = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AbreCertificadoAlteracao", "$(document).ready(function(){ $('.divCadastroCertificado').dialog('open'); });", true);
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.grvCertificado.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvCertificado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }

                LinkButton lnkAlterar = (LinkButton)e.Row.FindControl("lnkAlterar");
                Label lblAlterar = (Label)e.Row.FindControl("lblAlterar");
                if (lnkAlterar != null && lblAlterar != null)
                {
                    lnkAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                    lnkAlterar.CommandArgument = e.Row.RowIndex.ToString();
                    lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void grvObservacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }

                LinkButton lnkAlterar = (LinkButton)e.Row.FindControl("lnkAlterar");
                Label lblAlterar = (Label)e.Row.FindControl("lblAlterar");
                if (lnkAlterar != null && lblAlterar != null)
                {
                    lnkAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                    lnkAlterar.CommandArgument = e.Row.RowIndex.ToString();
                    lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void btnSalvarObservacao_Click(object sender, EventArgs ev)
        {
            if (Page.IsValid)
            {
                try
                {
                    string obs = txtObservacaoHtml.Text.Replace("<p>", string.Empty).Replace("</p>", string.Empty);
                    //bool tag = false;
                    //int tamanho = 0;
                    //foreach (char c in obs.ToCharArray())
                    //{
                    //    if (!tag)
                    //    {
                    //        if (c.Equals("<"))
                    //            tag = true;
                    //        else
                    //            tamanho++;
                    //    }
                    //    else if (c.Equals(">"))
                    //        tag = false;
                    //}
                    //if (tamanho > 450)
                    //    throw new ValidationException(string.Format(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.TamanhoObservacao").ToString(), tamanho.ToString()));
                    //else
                    //{
                    //    DataTable dtObs = ACA_AlunoHistoricoObservacaoBO.SelecionaObservacaoHistoricoAtiva(VS_alu_id, Convert.ToByte(ACA_HistoricoObservacaoPadraoTipo.ObservacaoComplementar));
                    //    string observacoes = "";
                    //    foreach (DataRow row in dtObs.Rows)
                    //    {
                    //        if (Convert.ToInt32(row["aho_id"]) == VS_aho_id)
                    //            continue;
                    //        observacoes += row["aho_observacao"];
                    //    }
                    //    observacoes += obs;

                    //    tag = false;
                    //    tamanho = 0;
                    //    foreach (char c in observacoes.ToCharArray())
                    //    {
                    //        if (!tag)
                    //        {
                    //            if (c.Equals("<") || c.Equals("&"))
                    //                tag = true;
                    //            else
                    //                tamanho++;
                    //        }
                    //        else if (c.Equals(">") || c.Equals(";"))
                    //            tag = false;
                    //    }
                    //    if (tamanho > 450)
                    //        throw new ValidationException(string.Format(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.TamanhoTotalObservacoes").ToString(), tamanho.ToString()));
                    //}

                    ACA_AlunoHistoricoObservacao entity = new ACA_AlunoHistoricoObservacao();

                    entity.alu_id = VS_alu_id;
                    entity.aho_id = VS_aho_id;
                    entity.aho_observacao = obs;
                    entity.aho_tipo = Convert.ToByte(ACA_HistoricoObservacaoPadraoTipo.ObservacaoComplementar);
                    entity.IsNew = VS_aho_id < 0;

                    if (ACA_AlunoHistoricoObservacaoBO.Save(entity))
                    {
                        ApplicationWEB._GravaLogSistema(VS_aho_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, string.Format("alu_id: {0} aho_id: {1} ", entity.alu_id, entity.aho_id));
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", VS_aho_id > 0 ? "UCInformacoesComplementares.btnSalvarObservacao.AlteradoSucesso" : "UCInformacoesComplementares.btnSalvarObservacao.SalvoSucesso").ToString()
                                                                , UtilBO.TipoMensagem.Sucesso);

                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "FechaObservacao", "$(document).ready(function() { $('.divCadastroObservacao').dialog('close'); });", true);

                        // Recarrega as observações
                        grvObservacao.DataBind();
                        updObservacao.Update();
                    }
                }
                catch (ValidationException e)
                {
                    lblMessageObservacao.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (ArgumentException e)
                {
                    lblMessageObservacao.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessageObservacao.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.btnSalvarCertificado.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnSalvarCertificado_Click(object sender, EventArgs ev)
        {
            if (Page.IsValid)
            {
                try
                {

                    ACA_AlunoHistoricoCertificado entity = ACA_AlunoHistoricoCertificadoBO.GetEntity(new ACA_AlunoHistoricoCertificado
                    {
                        alu_id = VS_alu_id
                                                                ,
                        alh_id = VS_alh_id > 0 ? VS_alh_id : Convert.ToInt32(ddlAnoHistorico.SelectedValue)
                    });

                    entity.ahc_numero = txtNumero.Text;
                    entity.ahc_livro = txtLivro.Text;
                    entity.ahc_folha = txtFolha.Text;
                    entity.ahc_gdae = txtGDAE.Text;
                    entity.ahc_situacao = 1;
                    entity.IsNew = entity.IsNew;

                    if (ACA_AlunoHistoricoCertificadoBO.Save(entity))
                    {
                        ApplicationWEB._GravaLogSistema(VS_alh_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, string.Format("alu_id: {0} alh_id: {1} ", entity.alu_id, entity.alh_id));
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", VS_alh_id > 0 ? "UCInformacoesComplementares.btnSalvarCertificado.AlteradoSucesso" : "UCInformacoesComplementares.btnSalvarCertificado.SalvoSucesso").ToString()
                                                                , UtilBO.TipoMensagem.Sucesso);

                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "FechaCertificado", "$(document).ready(function() { $('.divCadastroCertificado').dialog('close'); });", true);

                        btnCertificado.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir
                                                    && ACA_AlunoHistoricoCertificadoBO.SelecionaAnosDisponiveis(VS_alu_id).Rows.Count > 0;

                        // Recarrega os certificados.
                        grvCertificado.DataBind();
                        updCertificado.Update();
                    }
                }
                catch (ValidationException e)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (ArgumentException e)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.btnSalvarCertificado.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCertificado_Click(object sender, EventArgs e)
        {
            // Preenche os anos disponiveis, se tiver somente 1 ano ele é selecionado e não aparece na tela
            DataTable dt = ACA_AlunoHistoricoCertificadoBO.SelecionaAnosDisponiveis(VS_alu_id);

            if (dt.Rows.Count > 0)
            {
                ddlAnoHistorico.DataSource = dt;
                ddlAnoHistorico.DataBind();
                ddlAnoHistorico.Items.Insert(0, new ListItem(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.ddlAnoHistorico.Item0").ToString(), "-1"));

                if (dt.Rows.Count == 1)
                {
                    ddlAnoHistorico.SelectedIndex = 1;
                }
                else
                {
                    ddlAnoHistorico.SelectedIndex = 0;
                }

                // Limpa campos 
                txtFolha.Text = txtGDAE.Text = txtLivro.Text = txtNumero.Text = string.Empty;
                VS_alh_id = -1;
                ddlAnoHistorico.Enabled = true;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "AbreCertificado", "$(document).ready(function(){ $('.divCadastroCertificado').dialog('open'); });", true);
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCInformacoesComplementares.lblMessage.ErroAnoHistorico").ToString(), UtilBO.TipoMensagem.Alerta);
            }
        }

        protected void btnObservacao_Click(object sender, EventArgs e)
        {
            VS_aho_id = -1;
            txtObservacaoHtml.Text = string.Empty;

            DataTable dtObsPadrao = ACA_HistoricoObservacaoPadraoBO.SelecionaPorTipo(Convert.ToInt32(ACA_HistoricoObservacaoPadraoTipo.ObservacaoComplementar));

            rptObservacoesPadroes.DataSource = dtObsPadrao;
            rptObservacoesPadroes.DataBind();

            divObsPadrao.Visible = dtObsPadrao.Rows.Count > 0;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AbreObservacao", "$(document).ready(function(){ $('.divCadastroObservacao').dialog('open'); });", true);
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (clickVoltar != null)
                clickVoltar();
        }

        protected void btnVisualizarHistorico_Click(object sender, EventArgs e)
        {
            if (clickVisualizar != null)
                clickVisualizar();
        }

        protected void rptObservacoesPadroes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ImageButton btnConfirmaObservacoesPadroes = (ImageButton)e.Item.FindControl("btnConfirmaObservacoesPadroes");
            Label lblObservacoesPadroes = (Label)e.Item.FindControl("lblObservacoesPadroes");
            if (btnConfirmaObservacoesPadroes != null && lblObservacoesPadroes != null)
            {
                string script = "CKEDITOR.instances." + txtObservacaoHtml.ClientID + ".insertText("
                     + lblObservacoesPadroes.ClientID + ".innerText); return false;";

                btnConfirmaObservacoesPadroes.OnClientClick = script;
            }
        }

        #endregion Eventos
    }
}