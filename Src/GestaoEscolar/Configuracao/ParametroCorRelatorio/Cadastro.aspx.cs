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
using System.ComponentModel;

public partial class Configuracao_ParametroCorRelatorio_Cadastro : MotherPageLogado
{

    #region ENUM

    public enum RelatoriosCor
    {
        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.DocDctGraficoAtividadeAvaliativa")]
        DocDctGraficoAtividadeAvaliativa = 244,

        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.GraficoConsolidadoAtividadeAvaliativa")]
        GraficoConsolidadoAtividadeAvaliativa = 252,

        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular")]
        GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular = 262,

        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas")]
        GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas = 263,

        [Description("Configuracao.CorRelatorio.Busca.RelatoriosCor.AcompanhamentoIndividualdeNotas_ConceitoPorComponente")]
        AcompanhamentoIndividualdeNotas_ConceitoPorComponente = 270
    }

    #endregion
    #region Propriedades

    public new Configuracao_ParametroCorRelatorio PreviousPage
    {
        get
        {
            return ((Configuracao_ParametroCorRelatorio)(base.PreviousPage));
        }
    }

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

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference("~/Includes/jscolor.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        if (!IsPostBack)
        {
            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
            {
                VS_rlt_id = PreviousPage.EditItem;                
                Carregar(VS_rlt_id);                
            }

            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                lblMessage.Text = message;
            
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            grvCadastroCor.Columns[1].Visible = controlarOrdem;

            if (grvCadastroCor.Rows.Count > 0)
            {
                ((ImageButton)grvCadastroCor.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                ((ImageButton)grvCadastroCor.Rows[grvCadastroCor.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
            }
            grvCadastroCor.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            UCTotalRegistros1.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            btnNovaCor.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    #endregion

    #region Eventos    

    protected void grvCadastroCor_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = CFG_CorRelatorioBO.GetTotalRecords();

        if (grvCadastroCor.Rows.Count > 0)
        {
            if (grvCadastroCor.Rows[0].FindControl("_btnSubir") != null)//para deixar a seta do primeiro registro do grid só uma seta para baixo             
                ((ImageButton)grvCadastroCor.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
            if (grvCadastroCor.Rows[grvCadastroCor.Rows.Count - 1].FindControl("_btnDescer") != null)//para deixar a seta do último registro do grid só uma seta para cima                
                ((ImageButton)grvCadastroCor.Rows[grvCadastroCor.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");            
        }
    }

    protected void grvCadastroCor_RowCommand(object sender, GridViewCommandEventArgs e)
    {       
        if (e.CommandName == "Subir")        
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());

                int rlt_idDescer = Convert.ToInt32(grvCadastroCor.DataKeys[index]["rlt_id"]);
                int cor_idDescer = Convert.ToInt32(grvCadastroCor.DataKeys[index - 1]["cor_id"]);
                byte cor_ordem = Convert.ToByte(grvCadastroCor.DataKeys[index]["cor_ordem"]);
                CFG_CorRelatorio entityDescer = new CFG_CorRelatorio { cor_id = cor_idDescer, rlt_id = rlt_idDescer };
                CFG_CorRelatorioBO.GetEntity(entityDescer);
                entityDescer.cor_ordem = cor_ordem;

                int rlt_idSubir = Convert.ToInt32(grvCadastroCor.DataKeys[index]["rlt_id"]);
                int cor_idSubir = Convert.ToInt32(grvCadastroCor.DataKeys[index]["cor_id"]);
                byte cor_ordemSubir = Convert.ToByte(grvCadastroCor.DataKeys[index - 1]["cor_ordem"]);
                CFG_CorRelatorio entitySubir = new CFG_CorRelatorio { cor_id = cor_idSubir, rlt_id = rlt_idSubir };
                CFG_CorRelatorioBO.GetEntity(entitySubir);
                entitySubir.cor_ordem = cor_ordemSubir;

                if (CFG_CorRelatorioBO.SaveOrdem(entityDescer, entitySubir))
                {
                    grvCadastroCor.DataBind();
                    Carregar(VS_rlt_id);

                    if (grvCadastroCor.Rows.Count > 0)
                    {
                        ((ImageButton)grvCadastroCor.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)grvCadastroCor.Rows[grvCadastroCor.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "cor_id: " + cor_idSubir);
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "cor_id: " + cor_idDescer);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }

        if (e.CommandName == "Descer")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());

                int rlt_idDescer = Convert.ToInt32(grvCadastroCor.DataKeys[index]["rlt_id"]);
                int cor_idDescer = Convert.ToInt32(grvCadastroCor.DataKeys[index]["cor_id"]);
                byte cor_ordemDescer = Convert.ToByte(grvCadastroCor.DataKeys[index + 1]["cor_ordem"]);
                CFG_CorRelatorio entityDescer = new CFG_CorRelatorio { cor_id = cor_idDescer, rlt_id = rlt_idDescer };
                CFG_CorRelatorioBO.GetEntity(entityDescer);
                entityDescer.cor_ordem = cor_ordemDescer;

                int rlt_idSubir = Convert.ToInt32(grvCadastroCor.DataKeys[index]["rlt_id"]);
                int cor_idSubir = Convert.ToInt32(grvCadastroCor.DataKeys[index + 1]["cor_id"]);
                byte cor_ordemSubir = Convert.ToByte(grvCadastroCor.DataKeys[index]["cor_ordem"]);
                CFG_CorRelatorio entitySubir = new CFG_CorRelatorio { cor_id = cor_idSubir, rlt_id = rlt_idSubir };
                CFG_CorRelatorioBO.GetEntity(entitySubir);
                entitySubir.cor_ordem = cor_ordemSubir;

                if (CFG_CorRelatorioBO.SaveOrdem(entityDescer, entitySubir))
                {
                    grvCadastroCor.DataBind();
                    Carregar(VS_rlt_id);

                    if (grvCadastroCor.Rows.Count > 0)
                    {
                        ((ImageButton)grvCadastroCor.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                        ((ImageButton)grvCadastroCor.Rows[grvCadastroCor.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                    }
                }
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "cor_id: " + cor_idSubir);
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "cor_id: " + cor_idDescer);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void grvCadastroCor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgEditar = (ImageButton)e.Row.FindControl("_imgEditar");
            if (imgEditar != null)
                imgEditar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

            ImageButton imgExcluir = (ImageButton)e.Row.FindControl("_imgExcluir");
            if (imgExcluir != null)
                imgExcluir.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir) &&
                    (Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "cor_situacao")) != (Byte)eSituacao.Interno);

            ImageButton _btnSubir = (ImageButton)e.Row.FindControl("_btnSubir");
            if (_btnSubir != null)
            {
                _btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                _btnSubir.CommandArgument = e.Row.RowIndex.ToString();
                _btnSubir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            ImageButton _btnDescer = (ImageButton)e.Row.FindControl("_btnDescer");
            if (_btnDescer != null)
            {
                _btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                _btnDescer.CommandArgument = e.Row.RowIndex.ToString();
                _btnDescer.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }            
        }
    }    

    protected void grvCadastroCor_RowEditing(object sender, GridViewEditEventArgs e)
    {             
        grvCadastroCor.EditIndex = e.NewEditIndex;
        Carregar(VS_rlt_id);

        int cor_id = Convert.ToInt32(grvCadastroCor.DataKeys[e.NewEditIndex]["cor_id"].ToString());

        ImageButton imgSalvar = (ImageButton)grvCadastroCor.Rows[e.NewEditIndex].FindControl("_imgSalvar");
        if (imgSalvar != null)
            imgSalvar.Visible = true;

        ImageButton imgEditar = (ImageButton)grvCadastroCor.Rows[e.NewEditIndex].FindControl("_imgEditar");
        if (imgEditar != null)
        {
            imgEditar.Visible = false;
            ImageButton imgCancelar = (ImageButton)grvCadastroCor.Rows[e.NewEditIndex].FindControl("_imgCancelar");
            if (imgCancelar != null)
                imgCancelar.Visible = true;
        }
    }

    protected void grvCadastroCor_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {        
        GridView grv = ((GridView)sender);
        try
        {
            CFG_CorRelatorio entity = new CFG_CorRelatorio
            {
                IsNew = Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()),
                cor_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["cor_id"]),                
                cor_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["cor_situacao"].ToString()),
                cor_ordem = Convert.ToByte(grv.DataKeys[e.RowIndex]["cor_ordem"]),
                rlt_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["rlt_id"])
            };
            
            TextBox cor_corPaleta = (TextBox)grvCadastroCor.Rows[e.RowIndex].FindControl("txtCorPaleta");
            if (cor_corPaleta != null)
                entity.cor_corPaleta = cor_corPaleta.Text;

            entity.cor_dataCriacao = DateTime.Now;
            entity.cor_dataAlteracao = DateTime.Now;

            if (CFG_CorRelatorioBO.SalvarCor(entity))
            {
                if (Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "cor_id: " + entity.cor_id + ", rlt_id: " + entity.rlt_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Cor cadastrada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    updMensagem.Update();
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "cor_id: " + entity.cor_id + ", rlt_id: " + entity.rlt_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Cor alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    updMensagem.Update();
                }
                grv.EditIndex = -1;                
                Carregar(VS_rlt_id);
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
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar cor.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvCadastroCor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView grv = ((GridView)sender);
        try
        {
            if (!Boolean.Parse(grv.DataKeys[e.RowIndex]["IsNew"].ToString()))
            {
                CFG_CorRelatorio entity = new CFG_CorRelatorio
                {
                    cor_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["cor_id"]),
                    rlt_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["rlt_id"]),                    
                    cor_situacao = Byte.Parse(grv.DataKeys[e.RowIndex]["cor_situacao"].ToString())
                };
                if (CFG_CorRelatorioBO.Delete(entity))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "cor_id: " + entity.cor_id + ", rlt_id: " + entity.rlt_id);
                    lblMessage.Text = UtilBO.GetErroMessage("Cor excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    updMensagem.Update();
                    Carregar(VS_rlt_id);
                }
            }
        }
        catch (ValidationException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir cor.", UtilBO.TipoMensagem.Erro);
        }
        
    }

    protected void grvCadastroCor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView grv = ((GridView)sender);
        grv.EditIndex = -1;
        Carregar(VS_rlt_id);
    }

    /// <summary>
    /// Botão para incluir nova cor.
    /// </summary>
    protected void btnNovaCor_Click(object sender, EventArgs e)
    {   
        try
        {
            /*** abaixo busca o Seleciona cor passando o id do relatorio para que o grid seja 
                 alimentado somente com as cores referentes ao relatorio passado pela página anterior***/
            List<CFG_CorRelatorio> Cor = CFG_CorRelatorioBO.SelecionaCoresRelatorio(VS_rlt_id);            
            Cor.Add(new CFG_CorRelatorio
                {
                    IsNew = true,
                    cor_id = -1,
                    rlt_id = VS_rlt_id,
                    cor_corPaleta = String.Empty,
                    cor_situacao = 1,
                    /***abaixo pega a última linha do grid e se já tiver cor cadastrada acrescenta +1 para inserir nova ordem abaixo da ultima posição
                         senão ele será o primeiro cadastro e a primeira cor é cadastrada***/
                    cor_ordem = (Convert.ToByte(grvCadastroCor.Rows.Count) > 0 ? Convert.ToByte(Convert.ToByte(grvCadastroCor.DataKeys[Cor.Count - 1]["cor_ordem"]) + 1) : Convert.ToByte('0'))                                    
                });

                int index = (Cor.Count - 1);
                grvCadastroCor.EditIndex = index;
                grvCadastroCor.DataSource = Cor;
                grvCadastroCor.DataBind();

                ImageButton imgEditar = (ImageButton)grvCadastroCor.Rows[index].FindControl("_imgEditar");
                if (imgEditar != null)
                    imgEditar.Visible = false;

                ImageButton imgSalvar = (ImageButton)grvCadastroCor.Rows[index].FindControl("_imgSalvar");
                if (imgSalvar != null)
                    imgSalvar.Visible = true;

                ImageButton imgCancelar = (ImageButton)grvCadastroCor.Rows[index].FindControl("_imgCancelarParametro");
                if (imgCancelar != null)
                    imgCancelar.Visible = true;

                ImageButton imgExcluir = (ImageButton)grvCadastroCor.Rows[index].FindControl("_imgExcluir");
                if (imgExcluir != null)
                    imgExcluir.Visible = false;

                string script = String.Format("SetConfirmDialogLoader('{0}','{1}');", String.Concat("#", imgExcluir.ClientID), "Confirma a exclusão?");
                Page.ClientScript.RegisterStartupScript(GetType(), imgExcluir.ClientID, script, true);

                grvCadastroCor.Rows[index].Focus();
            }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar cor.", UtilBO.TipoMensagem.Erro);
            updMensagem.Update();
        }
    }

    /// <summary>
    /// Botão para incluir nova cor.
    /// </summary>
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Configuracao/ParametroCorRelatorio/Busca.aspx");
        }
        catch
        {
 
        }
    }

    #endregion

    #region Métodos

    private void Carregar(int rlt_id)
    {
        try
        {
            RelatoriosCor value = (RelatoriosCor)VS_rlt_id;
            string description = GetGlobalResourceObject("Enumerador", StringValueAttribute.GetStringDescription(value)).ToString();
            lblVariavel.Text = "Relatório: " + description;    

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            grvCadastroCor.PageSize = itensPagina;
            grvCadastroCor.DataSource = CFG_CorRelatorioBO.SelecionaCoresRelatorio(rlt_id);
            grvCadastroCor.DataBind();
         }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar cores do relatório", UtilBO.TipoMensagem.Erro);
        }
    }
    #endregion
}
