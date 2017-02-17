using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Configuracao_FaixaRelatorio_Cadastro : MotherPageLogado
{
    #region ENUM

    /// <summary>
    /// Enum dos números dos relatórios
    /// </summary>
    public enum RelatoriosFaixa
    {
        [Description("Configuracao.FaixaRelatorio.Busca.RelatoriosFaixa.DocDctGraficoAtividadeAvaliativa")]
        DocDctGraficoAtividadeAvaliativa = 244,

        [Description("Configuracao.FaixaRelatorio.Busca.RelatoriosFaixa.GraficoConsolidadoAtividadeAvaliativa")]
        GraficoConsolidadoAtividadeAvaliativa = 252,

        [Description("Configuracao.FaixaRelatorio.Busca.RelatoriosFaixa.GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular")]
        GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular = 262,

        [Description("Configuracao.FaixaRelatorio.Busca.RelatoriosFaixa.GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas")]
        GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas = 263,
    }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Dados de página anterior para o cadastro
    /// </summary>
    public new Configuracao_ParametroFaixaRelatorio PreviousPage
    {
        get
        {
            return ((Configuracao_ParametroFaixaRelatorio)(base.PreviousPage));
        }
    }    

    /// <summary>
    /// Propriedade em ViewState que armazena valor de rlt_id (ID da faixa relatório)
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private int VS_rlt_id
    {
        get
        {
            if (ViewState["VS_rlt_id"] != null)
                return Convert.ToInt32(ViewState["VS_rlt_id"]);
            return -1;
        }
        set
        {
            ViewState["VS_rlt_id"] = value;
        }
    }

    #endregion

    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference("~/Includes/jscolor.js"));
        }

        if (!IsPostBack)
        {
            if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
            {
            VS_rlt_id = PreviousPage.EditItem;
                RelatoriosFaixa value = (RelatoriosFaixa)VS_rlt_id;
                string description = GetGlobalResourceObject("Enumerador", StringValueAttribute.GetStringDescription(value)).ToString();
                lblRelatorio.Text += description;

                lblLegendaNota.Text = "Faixas de nota";
                lblLegendaConceito.Text = "Faixas de conceito";
                btnNovaFaixaNota.Text = btnNovaFaixaNota.Text + "nota";
                btnNovaFaixaConceito.Text = btnNovaFaixaConceito.Text + "conceito";
                btnNovaFaixaNota.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                btnNovaFaixaConceito.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            Carregar(VS_rlt_id);            
        } 
            else
            {
                Response.Redirect("~/Configuracao/FaixaRelatorio/Busca.aspx");
    }
        }
    }

    #region Faixa Nota

    protected void btnNovaFaixaNota_Click(object sender, EventArgs e)
    {
        try
        {        
            List<sFaixaRelatorioCor> faixa = CFG_FaixaRelatorioBO.SelecionaCoresRelatorio(VS_rlt_id, ApplicationWEB.AppMinutosCacheLongo).Where(p => p.esa_id <= 0).ToList();
            faixa.Add(new sFaixaRelatorioCor
            {
                IsNew = true,
                far_id = -1,
                rlt_id = VS_rlt_id,
                far_descricao = String.Empty,
                far_inicio = String.Empty,
                far_fim = String.Empty,
                esa_id = -1,
                eap_id = -1,
                far_cor = "#FFFFFF",
                far_situacao = 1
            });

            int index = (faixa.Count - 1);
            grvFaixaNota.EditIndex = index;
            grvFaixaNota.DataSource = faixa;
            grvFaixaNota.DataBind();

            ImageButton imgEditar = (ImageButton)grvFaixaNota.Rows[index].FindControl("_imgEditar");
            if (imgEditar != null)
                imgEditar.Visible = false;

            ImageButton imgSalvar = (ImageButton)grvFaixaNota.Rows[index].FindControl("_imgSalvar");
            if (imgSalvar != null)
                imgSalvar.Visible = true;

            ImageButton imgCancelar = (ImageButton)grvFaixaNota.Rows[index].FindControl("_imgCancelarParametro");
            if (imgCancelar != null)
                imgCancelar.Visible = true;

            ImageButton imgExcluir = (ImageButton)grvFaixaNota.Rows[index].FindControl("_imgExcluir");
            if (imgExcluir != null)
                imgExcluir.Visible = false;

            string script = String.Format("SetConfirmDialogLoader('{0}','{1}');", String.Concat("#", imgExcluir.ClientID), "Confirma a exclusão?");
            Page.ClientScript.RegisterStartupScript(GetType(), imgExcluir.ClientID, script, true);

            grvFaixaNota.Rows[index].Focus();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar nova faixa por relatório.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvFaixaNota_DataBinding(object sender, EventArgs e)
    {
        try
        {
           GridView grv = ((GridView)sender);
           if (grv.DataSource == null)
               grv.DataSource = CFG_FaixaRelatorioBO.GetSelect();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar faixa por relatório.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvFaixaNota_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string far_cor = grvFaixaNota.DataKeys[e.Row.RowIndex]["far_cor"] != null ?
                             grvFaixaNota.DataKeys[e.Row.RowIndex]["far_cor"].ToString() : "";
            if (string.IsNullOrEmpty(far_cor))
            {
                TextBox txtCorPaleta = (TextBox)e.Row.FindControl("txtCorPaleta");
                if (txtCorPaleta != null)
                    txtCorPaleta.Visible = false;
            }

            ImageButton imgEditar = (ImageButton)e.Row.FindControl("_imgEditar");
            if (imgEditar != null)
                imgEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

            ImageButton imgExcluir = (ImageButton)e.Row.FindControl("_imgExcluir");
            if (imgExcluir != null)
                imgExcluir.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir) &&
                    (Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "far_situacao")) != (Byte)eSituacao.Interno);                                    
        }
    }

    protected void grvFaixaNota_RowEditing(object sender, GridViewEditEventArgs e)
    {            
        GridView grv = ((GridView)sender);
        grv.EditIndex = e.NewEditIndex;
        Carregar(VS_rlt_id);

        int far_id = Convert.ToInt32(grv.DataKeys[e.NewEditIndex]["far_id"].ToString());

        TextBox txtDescricao = (TextBox)grv.Rows[e.NewEditIndex].FindControl("txtDescricao");
        if (txtDescricao != null)
            txtDescricao.Text = HttpUtility.HtmlDecode(txtDescricao.Text);

        TextBox txtInicio = (TextBox)grv.Rows[e.NewEditIndex].FindControl("txtInicio");
        if (txtInicio != null)
            txtInicio.Text = HttpUtility.HtmlDecode(txtInicio.Text);

        TextBox txtFim = (TextBox)grv.Rows[e.NewEditIndex].FindControl("txtFim");
        if (txtFim != null)
            txtFim.Text = HttpUtility.HtmlDecode(txtFim.Text);

        TextBox cor_corPaleta = (TextBox)grv.Rows[e.NewEditIndex].FindControl("txtCorPaleta");
        if (cor_corPaleta != null)
            cor_corPaleta.ReadOnly = false;

        string far_cor = grv.DataKeys[e.NewEditIndex]["far_cor"] != null ?
                         grv.DataKeys[e.NewEditIndex]["far_cor"].ToString() : "";

        CheckBox chkSemCor = (CheckBox)grv.Rows[e.NewEditIndex].FindControl("chkSemCor");
        if (chkSemCor != null)
        {
            chkSemCor.Checked = string.IsNullOrEmpty(far_cor);
            if (cor_corPaleta != null)
                cor_corPaleta.Visible = !chkSemCor.Checked;
        }

        ImageButton imgSalvar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("_imgSalvar");
        if (imgSalvar != null)
            imgSalvar.Visible = true;
           
        ImageButton imgEditar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("_imgEditar");
        if (imgEditar != null)
        {
            imgEditar.Visible = false;
            ImageButton imgCancelar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("_imgCancelar");
            if (imgCancelar != null)
                imgCancelar.Visible = true;
        }
        grv.Rows[e.NewEditIndex].Focus();
    }

    protected void grvFaixaNota_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView grv = ((GridView)sender);
        try
        {
            CFG_FaixaRelatorio entity = new CFG_FaixaRelatorio
            {
                IsNew = Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()),
                far_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["far_id"]),
                far_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["far_situacao"].ToString())
            };

            entity.rlt_id = VS_rlt_id;

            decimal far_inicio = 0;
            decimal far_fim = 0;

            TextBox txtDescricao = (TextBox)grv.Rows[e.RowIndex].FindControl("txtDescricao");
            if (txtDescricao != null)
                entity.far_descricao = txtDescricao.Text;

            TextBox txtInicio = (TextBox)grv.Rows[e.RowIndex].FindControl("txtInicio");
            if (txtInicio != null && Decimal.TryParse(txtInicio.Text.Replace(".", ","), out far_inicio))
                entity.far_inicio = txtInicio.Text.Replace(".", ",");

            TextBox txtFim = (TextBox)grv.Rows[e.RowIndex].FindControl("txtFim");
            if (txtFim != null && Decimal.TryParse(txtFim.Text.Replace(".", ","), out far_fim))
                entity.far_fim = txtFim.Text.Replace(".", ",");        

            TextBox cor_corPaleta = (TextBox)grv.Rows[e.RowIndex].FindControl("txtCorPaleta");
            if (cor_corPaleta != null)
                entity.far_cor = cor_corPaleta.Text;

            CheckBox chkSemCor = (CheckBox)grv.Rows[e.RowIndex].FindControl("chkSemCor");
            if (chkSemCor != null)
                entity.far_cor = chkSemCor.Checked ? "" : entity.far_cor;

            entity.far_dataCriacao = DateTime.Now;
            entity.far_dataAlteracao = DateTime.Now;

            if (string.IsNullOrEmpty(entity.far_inicio) || string.IsNullOrEmpty(entity.far_fim))
            {
                if ((string.IsNullOrEmpty(entity.far_inicio) && !string.IsNullOrEmpty(entity.far_fim)) ||
                    (!string.IsNullOrEmpty(entity.far_inicio) && string.IsNullOrEmpty(entity.far_fim)))
                    throw new ValidationException("A faixa início e fim devem estar preenchidas ou ambas em branco.");

                foreach (DataKey dataKey in grv.DataKeys)
                    if ((dataKey.Values["far_inicio"] == null || dataKey.Values["far_fim"] == null ||
                         string.IsNullOrEmpty(dataKey.Values["far_inicio"].ToString()) ||
                         string.IsNullOrEmpty(dataKey.Values["far_fim"].ToString())) &&
                        Convert.ToInt32(dataKey.Values["far_id"]) != entity.far_id)
                        throw new ValidationException("Já existe uma faixa em branco para o relatório.");
            }
            else
            {
                foreach (DataKey dataKey in grv.DataKeys)
                    if (dataKey.Values["far_inicio"] != null && dataKey.Values["far_fim"] != null &&
                        !string.IsNullOrEmpty(dataKey.Values["far_inicio"].ToString()) &&
                        !string.IsNullOrEmpty(dataKey.Values["far_fim"].ToString()) &&
                        ((far_inicio >= Convert.ToDecimal(dataKey.Values["far_inicio"].ToString().Replace(".", ",")) &&
                          far_inicio <= Convert.ToDecimal(dataKey.Values["far_fim"].ToString().Replace(".", ","))) ||
                         (far_fim >= Convert.ToDecimal(dataKey.Values["far_inicio"].ToString().Replace(".", ",")) &&
                          far_fim <= Convert.ToDecimal(dataKey.Values["far_fim"].ToString().Replace(".", ","))) ||
                         (Convert.ToDecimal(dataKey.Values["far_inicio"].ToString().Replace(".", ",")) >= far_inicio &&
                          Convert.ToDecimal(dataKey.Values["far_fim"].ToString().Replace(".", ",")) <= far_fim)) &&
                        Convert.ToInt32(dataKey.Values["far_id"]) != entity.far_id)
                        throw new ValidationException("Já existe uma faixa com esse valor para a escala no relatório.");
            }

            if (CFG_FaixaRelatorioBO.Save(entity))
            {
                if (Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "far_id: " + entity.far_id + ", rlt_id: " + entity.rlt_id);
                    lblMensagem.Text = UtilBO.GetErroMessage("Faixa por relatório incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "far_id: " + entity.far_id + ", rlt_id: " + entity.rlt_id);
                    lblMensagem.Text = UtilBO.GetErroMessage("Faixa por relatório alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                ApplicationWEB.RecarregarConfiguracoes();
                grv.EditIndex = -1;
                Carregar(VS_rlt_id);
            }
        }
        catch (ValidationException ex)
        {
            lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar faixa por relatório.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvFaixaNota_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView grv = ((GridView)sender);
        try
        {
            if (!Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
            {
                CFG_FaixaRelatorio entity = new CFG_FaixaRelatorio
                {
                    far_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["far_id"]),
                    rlt_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["rlt_id"]),
                    far_situacao = 3,//Excluido
                    far_dataAlteracao = DateTime.Now
                };

                if (CFG_FaixaRelatorioBO.Delete(entity))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "far_id: " + entity.far_id + ", rlt_id: " + entity.rlt_id);
                    lblMensagem.Text = UtilBO.GetErroMessage("Faixa por relatório excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    Carregar(VS_rlt_id);
                }
            }            
        }
        catch (ValidationException ex)
        {
            lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir faixa por relatório.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvFaixaNota_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView grv = ((GridView)sender);
        grv.EditIndex = -1;
        Carregar(VS_rlt_id);
    }

    protected void chkSemCor_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSemCor = (CheckBox)sender;
        GridViewRow rowGrid = (GridViewRow)chkSemCor.Parent.Parent;

        TextBox txtCorPaleta = (TextBox)rowGrid.FindControl("txtCorPaleta");

        if (txtCorPaleta != null && chkSemCor != null)
            txtCorPaleta.Visible = !chkSemCor.Checked;
    }

    #endregion

    #region Faixa Conceito

    protected void ddlEscalaAvaliacao_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlEscalaAvaliacao = (DropDownList)sender;
            GridViewRow rowGrid = (GridViewRow)ddlEscalaAvaliacao.Parent.Parent;

            int esa_id = Convert.ToInt32(ddlEscalaAvaliacao.SelectedValue);

            WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer uCCEscalaAvaliacaoParecer = (WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer)rowGrid.FindControl("UCCEscalaAvaliacaoParecer1");
            if (uCCEscalaAvaliacaoParecer != null)
            {
                uCCEscalaAvaliacaoParecer.Titulo = "Conceito";
                uCCEscalaAvaliacaoParecer.MostrarMensagemSelecione = uCCEscalaAvaliacaoParecer.AdicionaValorSemParecer =
                    uCCEscalaAvaliacaoParecer.Obrigatorio = true;
                uCCEscalaAvaliacaoParecer.CarregarMensagemSelecione();
                uCCEscalaAvaliacaoParecer.PermiteEditar = false;
                uCCEscalaAvaliacaoParecer.Valor = new int[] { -1, -1, -1 };
                if (esa_id > 0)
                {
                    uCCEscalaAvaliacaoParecer.CarregarPorEscala(esa_id);
                    uCCEscalaAvaliacaoParecer.PermiteEditar = true;
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar escala de avaliação.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnNovaFaixaConceito_Click(object sender, EventArgs e)
    {
        try
        {
            List<sFaixaRelatorioCor> faixa = CFG_FaixaRelatorioBO.SelecionaCoresRelatorio(VS_rlt_id, ApplicationWEB.AppMinutosCacheLongo).Where(p => p.esa_id > 0).ToList();
            faixa.Add(new sFaixaRelatorioCor
            {
                IsNew = true,
                far_id = -1,
                rlt_id = VS_rlt_id,
                far_descricao = String.Empty,
                far_inicio = String.Empty,
                far_fim = String.Empty,
                esa_id = -1,
                eap_id = -1,
                far_cor = "#FFFFFF",
                far_situacao = 1
            });

            int index = (faixa.Count - 1);
            grvFaixaConceito.EditIndex = index;
            grvFaixaConceito.DataSource = faixa;
            grvFaixaConceito.DataBind();

            DropDownList ddlEscalaAvaliacao = (DropDownList)grvFaixaConceito.Rows[index].FindControl("ddlEscalaAvaliacao");
            if (ddlEscalaAvaliacao != null)
            {
                ddlEscalaAvaliacao.Items.Clear();
                ddlEscalaAvaliacao.Items.Add(new ListItem("-- Selecione uma escala --", "-1"));
                ddlEscalaAvaliacao.DataSource = ACA_EscalaAvaliacaoBO.SelecionaEscalaAvaliacaoPorTipo(false, true, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                ddlEscalaAvaliacao.DataBind();

                if (ddlEscalaAvaliacao.Items.Count == 2)
                {
                    ddlEscalaAvaliacao.SelectedIndex = 1;
                    ddlEscalaAvaliacao_SelectedIndexChanged(ddlEscalaAvaliacao, e);
                }
            }

            WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer uCCEscalaAvaliacaoParecer = (WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer)grvFaixaConceito.Rows[index].FindControl("UCCEscalaAvaliacaoParecer1");
            if (uCCEscalaAvaliacaoParecer != null)
            {
                uCCEscalaAvaliacaoParecer.MostrarMensagemSelecione = true;
                uCCEscalaAvaliacaoParecer.CarregarMensagemSelecione();
            }

            ImageButton imgEditar = (ImageButton)grvFaixaConceito.Rows[index].FindControl("_imgEditar");
            if (imgEditar != null)
                imgEditar.Visible = false;

            ImageButton imgSalvar = (ImageButton)grvFaixaConceito.Rows[index].FindControl("_imgSalvar");
            if (imgSalvar != null)
                imgSalvar.Visible = true;

            ImageButton imgCancelar = (ImageButton)grvFaixaConceito.Rows[index].FindControl("_imgCancelarParametro");
            if (imgCancelar != null)
                imgCancelar.Visible = true;

            ImageButton imgExcluir = (ImageButton)grvFaixaConceito.Rows[index].FindControl("_imgExcluir");
            if (imgExcluir != null)
                imgExcluir.Visible = false;

            string script = String.Format("SetConfirmDialogLoader('{0}','{1}');", String.Concat("#", imgExcluir.ClientID), "Confirma a exclusão?");
            Page.ClientScript.RegisterStartupScript(GetType(), imgExcluir.ClientID, script, true);

            grvFaixaConceito.Rows[index].Focus();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar nova faixa por relatório.", UtilBO.TipoMensagem.Erro);
        }
    }
    
    protected void grvFaixaConceito_DataBinding(object sender, EventArgs e)
    {
        try
        {
           GridView grv = ((GridView)sender);
           if (grv.DataSource == null)
               grv.DataSource = CFG_FaixaRelatorioBO.GetSelect();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar faixa por relatório.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvFaixaConceito_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int esa_id = Convert.ToInt32(grvFaixaConceito.DataKeys[e.Row.RowIndex]["esa_id"].ToString());
            int eap_id = Convert.ToInt32(grvFaixaConceito.DataKeys[e.Row.RowIndex]["eap_id"].ToString());
            int eap_ordem = Convert.ToInt32(grvFaixaConceito.DataKeys[e.Row.RowIndex]["eap_ordem"].ToString());
            int esa_tipo = Convert.ToInt32(grvFaixaConceito.DataKeys[e.Row.RowIndex]["esa_tipo"].ToString());
            eap_ordem = eap_id < 0 ? -1 : eap_ordem;
            esa_tipo = esa_id <= 0 ? -1 : esa_tipo;

            string far_cor = grvFaixaConceito.DataKeys[e.Row.RowIndex]["far_cor"] != null ?
                             grvFaixaConceito.DataKeys[e.Row.RowIndex]["far_cor"].ToString() : "";

            if (string.IsNullOrEmpty(far_cor))
            {
                TextBox txtCorPaleta = (TextBox)e.Row.FindControl("txtCorPaleta");
                if (txtCorPaleta != null)
                    txtCorPaleta.Visible = false;
            }

            DropDownList ddlEscalaAvaliacao = (DropDownList)e.Row.FindControl("ddlEscalaAvaliacao");
            if (ddlEscalaAvaliacao != null)
            {
                ddlEscalaAvaliacao.Items.Clear();
                ddlEscalaAvaliacao.Items.Add(new ListItem("-- Selecione uma escala --", "-1"));
                ddlEscalaAvaliacao.DataSource = ACA_EscalaAvaliacaoBO.SelecionaEscalaAvaliacaoPorTipo(false, true, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                ddlEscalaAvaliacao.DataBind();
                ddlEscalaAvaliacao.SelectedValue = esa_id.ToString();
            }

            WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer uCCEscalaAvaliacaoParecer = (WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer)e.Row.FindControl("UCCEscalaAvaliacaoParecer1");
            if (uCCEscalaAvaliacaoParecer != null)
            {
                uCCEscalaAvaliacaoParecer.Titulo = "Conceito";
                uCCEscalaAvaliacaoParecer.MostrarMensagemSelecione = uCCEscalaAvaliacaoParecer.AdicionaValorSemParecer = 
                    uCCEscalaAvaliacaoParecer.Obrigatorio = true;
                uCCEscalaAvaliacaoParecer.PermiteEditar = false;
                if (esa_id > 0)
                {
                    uCCEscalaAvaliacaoParecer.CarregarPorEscala(esa_id);
                    uCCEscalaAvaliacaoParecer.PermiteEditar = true;
                    uCCEscalaAvaliacaoParecer.Valor = new int[] { esa_id, eap_id, eap_ordem };
                }
            }

            ImageButton imgEditar = (ImageButton)e.Row.FindControl("_imgEditar");
            if (imgEditar != null)
                imgEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

            ImageButton imgExcluir = (ImageButton)e.Row.FindControl("_imgExcluir");
            if (imgExcluir != null)
                imgExcluir.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir) &&
                    (Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "far_situacao")) != (Byte)eSituacao.Interno);                                    
        }
    }

    protected void grvFaixaConceito_RowEditing(object sender, GridViewEditEventArgs e)
    {            
        GridView grv = ((GridView)sender);
        grv.EditIndex = e.NewEditIndex;
        Carregar(VS_rlt_id);

        int far_id = Convert.ToInt32(grv.DataKeys[e.NewEditIndex]["far_id"].ToString());

        TextBox cor_corPaleta = (TextBox)grv.Rows[e.NewEditIndex].FindControl("txtCorPaleta");
        if (cor_corPaleta != null)
            cor_corPaleta.ReadOnly = false;

        string far_cor = grv.DataKeys[e.NewEditIndex]["far_cor"] != null ?
                         grv.DataKeys[e.NewEditIndex]["far_cor"].ToString() : "";

        CheckBox chkSemCor = (CheckBox)grv.Rows[e.NewEditIndex].FindControl("chkSemCor");
        if (chkSemCor != null)
        {
            chkSemCor.Checked = string.IsNullOrEmpty(far_cor);
            if (cor_corPaleta != null)
                cor_corPaleta.Visible = !chkSemCor.Checked;
        }

        ImageButton imgSalvar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("_imgSalvar");
        if (imgSalvar != null)
            imgSalvar.Visible = true;
           
        ImageButton imgEditar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("_imgEditar");
        if (imgEditar != null)
        {
            imgEditar.Visible = false;
            ImageButton imgCancelar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("_imgCancelar");
            if (imgCancelar != null)
                imgCancelar.Visible = true;
        }
        grv.Rows[e.NewEditIndex].Focus();
    }

    protected void grvFaixaConceito_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView grv = ((GridView)sender);
        try
        {
            CFG_FaixaRelatorio entity = new CFG_FaixaRelatorio
            {
                IsNew = Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()),
                far_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["far_id"]),
                    far_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["far_situacao"].ToString())
                };

            entity.rlt_id = VS_rlt_id;

            DropDownList ddlEscalaAvaliacao = (DropDownList)grv.Rows[e.RowIndex].FindControl("ddlEscalaAvaliacao");
            if (ddlEscalaAvaliacao != null)
                entity.esa_id = Convert.ToInt32(ddlEscalaAvaliacao.SelectedValue);

            WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer uCCEscalaAvaliacaoParecer = (WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer)grv.Rows[e.RowIndex].FindControl("UCCEscalaAvaliacaoParecer1");
            if (uCCEscalaAvaliacaoParecer != null)
            {
                entity.far_descricao = uCCEscalaAvaliacaoParecer.Texto;
                entity.eap_id = Convert.ToInt32(uCCEscalaAvaliacaoParecer.Valor[1]);
                entity.far_inicio = entity.far_fim = uCCEscalaAvaliacaoParecer.Valor[2].ToString();
            }

            if (entity.far_inicio.Equals("0"))
                entity.far_inicio = entity.far_fim = "";

            TextBox cor_corPaleta = (TextBox)grv.Rows[e.RowIndex].FindControl("txtCorPaleta");
            if (cor_corPaleta != null)
                entity.far_cor = cor_corPaleta.Text;

            CheckBox chkSemCor = (CheckBox)grv.Rows[e.RowIndex].FindControl("chkSemCor");
            if (chkSemCor != null)
                entity.far_cor = chkSemCor.Checked ? "" : entity.far_cor;

            entity.far_dataCriacao = DateTime.Now;
            entity.far_dataAlteracao = DateTime.Now;

            if (string.IsNullOrEmpty(entity.far_inicio) || string.IsNullOrEmpty(entity.far_fim))
            {
                foreach (DataKey dataKey in grv.DataKeys)
                    if ((dataKey.Values["far_inicio"] == null || dataKey.Values["far_fim"] == null ||
                         string.IsNullOrEmpty(dataKey.Values["far_inicio"].ToString()) ||
                         string.IsNullOrEmpty(dataKey.Values["far_fim"].ToString())) &&
                        Convert.ToInt32(dataKey.Values["esa_id"]) == entity.esa_id &&
                        Convert.ToInt32(dataKey.Values["far_id"]) != entity.far_id)
                        throw new ValidationException("Já existe uma faixa em branco para a escala no relatório.");
            }
            else
            {
                foreach (DataKey dataKey in grv.DataKeys)
                    if (dataKey.Values["far_inicio"] != null && entity.far_inicio == dataKey.Values["far_inicio"].ToString() &&
                        dataKey.Values["far_fim"] != null && entity.far_fim == dataKey.Values["far_fim"].ToString() &&
                        Convert.ToInt32(dataKey.Values["esa_id"]) == entity.esa_id &&
                        Convert.ToInt32(dataKey.Values["far_id"]) != entity.far_id)
                        throw new ValidationException("Já existe uma faixa com esse valor para a escala no relatório.");
            }

            if (CFG_FaixaRelatorioBO.Save(entity))
            {
                if (Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "far_id: " + entity.far_id + ", rlt_id: " + entity.rlt_id);
                    lblMensagem.Text = UtilBO.GetErroMessage("Faixa por relatório incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "far_id: " + entity.far_id + ", rlt_id: " + entity.rlt_id);
                    lblMensagem.Text = UtilBO.GetErroMessage("Faixa por relatório alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                ApplicationWEB.RecarregarConfiguracoes();
                grv.EditIndex = -1;
                Carregar(VS_rlt_id);
            }
        }
        catch (ValidationException ex)
        {
            lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar faixa por relatório.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvFaixaConceito_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView grv = ((GridView)sender);
        try
        {
            if (!Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
            {
                CFG_FaixaRelatorio entity = new CFG_FaixaRelatorio
                {
                    far_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["far_id"]),
                    rlt_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["rlt_id"]),
                    far_situacao = 3,//Excluido
                    far_dataAlteracao = DateTime.Now
                };

                if (CFG_FaixaRelatorioBO.Delete(entity))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "far_id: " + entity.far_id + ", rlt_id: " + entity.rlt_id);
                    lblMensagem.Text = UtilBO.GetErroMessage("Faixa por relatório excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    Carregar(VS_rlt_id);
                }
            }            
        }
        catch (ValidationException ex)
        {
            lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir faixa por relatório.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvFaixaConceito_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView grv = ((GridView)sender);
        grv.EditIndex = -1;
        Carregar(VS_rlt_id);
    }

    #endregion

    /// <summary>
    /// Evento voltar para tela anterior.
    /// </summary>
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Configuracao/FaixaRelatorio/Busca.aspx");
        }
        catch
        {
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar voltar para tela de relatorios.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion EVENTOS

    #region MÉTODOS

    /// <summary>
    /// Carrega as faixas do relatório
    /// </summary>
    /// <param name="rlt_id">Id do relatório</param>
    private void Carregar(int rlt_id)
    {
        try
        {
            Array populaGrid = Enum.GetValues(typeof(RelatoriosFaixa));

            DataTable dt = new DataTable();
            dt.Columns.Add("rlt_id");
            dt.Columns.Add("rlt_nome");

            Type objType = typeof(RelatoriosFaixa);
            FieldInfo[] propriedades = objType.GetFields();
            foreach (FieldInfo objField in propriedades)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                    dt.Rows.Add(Convert.ToString(objField.GetRawConstantValue()), GetGlobalResourceObject("Enumerador", attributes[0].Description));
            }

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            List<sFaixaRelatorioCor> lstFaixaRelatorio = CFG_FaixaRelatorioBO.SelecionaCoresRelatorio(rlt_id, ApplicationWEB.AppMinutosCacheLongo);

            grvFaixaNota.PageSize = itensPagina;
            grvFaixaNota.DataSource = lstFaixaRelatorio.Where(p => p.esa_id <= 0);
            grvFaixaNota.DataBind();

            grvFaixaConceito.PageSize = itensPagina;
            grvFaixaConceito.DataSource = lstFaixaRelatorio.Where(p => p.esa_id > 0);
            grvFaixaConceito.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar faixas do relatório", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion
}
