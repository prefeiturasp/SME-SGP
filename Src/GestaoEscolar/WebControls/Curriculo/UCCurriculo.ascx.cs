using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.UI;

namespace GestaoEscolar.WebControls.Curriculo
{
    public partial class UCCurriculo : MotherUserControl
    {
        #region Constantes

        private const int colunaSugestao = 1;
        private const int colunaOrdem = 2;        
        private const int colunaEditar = 3;
        private const int colunaExcluir = 4;
        private const int colunaObjetivos = 5;

        #endregion Constantes

        #region Propriedades

        // Ano do calendário para o currículo,
        // por enquanto será feito apenas para este ano.
        private int cal_ano = 2017;

        /// <summary>
        /// Altera o título da página
        /// </summary>
        public string Titulo
        {
            set
            {
                litLegend.Text = value;
            }
        }

        private int VS_tne_id
        {
            get
            {
                if (ViewState["VS_tne_id"] != null)
                    return Convert.ToInt32(ViewState["VS_tne_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_tne_id"] = value;
            }
        }

        private int VS_tme_id
        {
            get
            {
                if (ViewState["VS_tme_id"] != null)
                    return Convert.ToInt32(ViewState["VS_tme_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_tme_id"] = value;
            }
        }

        private int VS_tds_id
        {
            get
            {
                if (rblDisciplina.Items.Count > 0)
                    return Convert.ToInt32(rblDisciplina.SelectedValue);
                return -1;
            }
        }

        private int VS_tcp_id
        {
            get
            {
                return UCComboTipoCurriculoPeriodo1.Valor;
            }
        }

        public bool VS_permiteIncluir
        {
            get
            {
                if (ViewState["VS_permiteIncluir"] != null)
                    return Convert.ToBoolean(ViewState["VS_permiteIncluir"]);
                return false;
            }
            set
            {
                ViewState["VS_permiteIncluir"] = value;
            }
        }

        public bool VS_permiteEditar
        {
            get
            {
                if (ViewState["VS_permiteEditar"] != null)
                    return Convert.ToBoolean(ViewState["VS_permiteEditar"]);
                return false;
            }
            set
            {
                ViewState["VS_permiteEditar"] = value;
            }
        }

        public bool VS_permiteExcluir
        {
            get
            {
                if (ViewState["VS_permiteExcluir"] != null)
                    return Convert.ToBoolean(ViewState["VS_permiteExcluir"]);
                return false;
            }
            set
            {
                ViewState["VS_permiteExcluir"] = value;
            }
        }

        public bool VS_permiteIncluirSugestao
        {
            get
            {
                if (ViewState["VS_permiteIncluirSugestao"] != null)
                    return Convert.ToBoolean(ViewState["VS_permiteIncluirSugestao"]);
                return false;
            }
            set
            {
                ViewState["VS_permiteIncluirSugestao"] = value;
            }
        }

        public bool VS_permiteConsultarSugestao
        {
            get
            {
                if (ViewState["VS_permiteConsultarSugestao"] != null)
                    return Convert.ToBoolean(ViewState["VS_permiteConsultarSugestao"]);
                return false;
            }
            set
            {
                ViewState["VS_permiteConsultarSugestao"] = value;
            }
        }

        private List<sCurriculoCapitulo> VS_ltCurriculoCapituloGeral
        {
            get
            {
                return (List<sCurriculoCapitulo>)
                        (
                            ViewState["VS_ltCurriculoCapituloGeral"] ??
                            (
                                ViewState["VS_ltCurriculoCapituloGeral"] = ACA_CurriculoCapituloBO.SelecionaPorNivelEnsinoDisciplina(VS_tne_id, VS_tme_id, -1)
                            )
                        );
            }
            set
            {
                ViewState["VS_ltCurriculoCapituloGeral"] = value;
            }
        }

        private List<sCurriculoCapitulo> VS_ltCurriculoCapituloDisciplina
        {
            get
            {
                return (List<sCurriculoCapitulo>)
                        (
                            ViewState["VS_ltCurriculoCapituloDisciplina"] ??
                            (
                                ViewState["VS_ltCurriculoCapituloDisciplina"] = ACA_CurriculoCapituloBO.SelecionaPorNivelEnsinoDisciplina(VS_tne_id, VS_tme_id, VS_tds_id)
                            )
                        );
            }
            set
            {
                ViewState["VS_ltCurriculoCapituloDisciplina"] = value;
            }
        }

        private List<sCurriculoObjetivo> VS_ltCurriculoObjetivo
        {
            get
            {
                return (List<sCurriculoObjetivo>)
                        (
                            ViewState["VS_ltCurriculoObjetivo"] ??
                            (
                                ViewState["VS_ltCurriculoObjetivo"] = ACA_CurriculoObjetivoBO.SelecionaPorNivelEnsinoDisciplinaPeriodo(VS_tne_id, VS_tme_id, VS_tds_id, VS_tcp_id)
                            )
                        );
            }
            set
            {
                ViewState["VS_ltCurriculoObjetivo"] = value;
            }
        }

        private List<sCurriculoSugestaoCapitulo> VS_ltCurriculoSugestaoCapitulo
        {
            get
            {
                return (List<sCurriculoSugestaoCapitulo>)
                        (
                            ViewState["VS_ltCurriculoSugestaoCapitulo"] ??
                            (
                                ViewState["VS_ltCurriculoSugestaoCapitulo"] = ACA_CurriculoCapituloSugestaoBO.SelecionaPorNivelEnsinoDisciplinaUsuario(VS_tne_id, VS_tme_id, -1, VS_permiteIncluirSugestao ? __SessionWEB.__UsuarioWEB.Usuario.usu_id : Guid.Empty)
                            )
                        );
            }
            set
            {
                ViewState["VS_ltCurriculoSugestaoCapitulo"] = value;
            }
        }

        private List<sCurriculoSugestaoCapitulo> VS_ltCurriculoSugestaoCapituloDisciplina
        {
            get
            {
                return (List<sCurriculoSugestaoCapitulo>)
                        (
                            ViewState["VS_ltCurriculoSugestaoCapituloDisciplina"] ??
                            (
                                ViewState["VS_ltCurriculoSugestaoCapituloDisciplina"] = ACA_CurriculoCapituloSugestaoBO.SelecionaPorNivelEnsinoDisciplinaUsuario(VS_tne_id, VS_tme_id, VS_tds_id, VS_permiteIncluirSugestao ? __SessionWEB.__UsuarioWEB.Usuario.usu_id : Guid.Empty)
                            )
                        );
            }
            set
            {
                ViewState["VS_ltCurriculoSugestaoCapituloDisciplina"] = value;
            }
        }

        private List<sCurriculoSugestaoObjetivo> VS_ltCurriculoSugestaoObjetivo
        {
            get
            {
                return (List<sCurriculoSugestaoObjetivo>)
                        (
                            ViewState["VS_ltCurriculoSugestaoObjetivo"] ??
                            (
                                ViewState["VS_ltCurriculoSugestaoObjetivo"] = ACA_CurriculoObjetivoSugestaoBO.SelecionaPorNivelEnsinoDisciplinaPeriodoUsuario(VS_tne_id, VS_tme_id, VS_tds_id, VS_tcp_id, VS_permiteIncluirSugestao ? __SessionWEB.__UsuarioWEB.Usuario.usu_id : Guid.Empty)
                            )
                        );
            }
            set
            {
                ViewState["VS_ltCurriculoSugestaoObjetivo"] = value;
            }
        }

        private bool VS_abertoSugestao
        {
            get
            {
                if (ViewState["VS_abertoSugestao"] == null)
                {
                    int tev_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_ABERTURA_SUGESTOES, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    ViewState["VS_abertoSugestao"] = ACA_EventoBO.VerificaEventoVigentePorUsuario(tev_id, __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao, __SessionWEB.__UsuarioWEB.Usuario.usu_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Docente.doc_id, -1);
                }
                return (bool)ViewState["VS_abertoSugestao"];
            }
            set
            {
                ViewState["VS_abertoSugestao"] = value;
            }
        }

        private List<int> ltEixoAberto = new List<int>();

        private bool comandoExecutado = false;

        #endregion Propriedades

        #region Page life cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCCurriculo.js"));
            }

            btnNovoGeral.Visible = btnNovoDisciplina.Visible = btnNovoEixo.Visible = VS_permiteIncluir;

            UCComboTipoNivelEnsino1.IndexChanged += UCComboTipoNivelEnsino1_IndexChanged;
            UCComboTipoModalidadeEnsino1.IndexChanged += UCComboTipoModalidadeEnsino1_IndexChanged;
            UCComboTipoCurriculoPeriodo1.OnSelectedIndexChanged += UCComboTipoCurriculoPeriodo1_OnSelectedIndexChanged;

            if (!IsPostBack)
            {
                try
                {
                    if (VS_permiteIncluirSugestao && !VS_abertoSugestao)
                    {
                        lblMsgEvento.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemEvento").ToString(), UtilBO.TipoMensagem.Informacao);
                        UCComboTipoNivelEnsino1.Visible = UCComboTipoModalidadeEnsino1.Visible = false;
                    }
                    else
                    {
                        if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                        {
                            UCComboTipoNivelEnsino1.CarregarTipoNivelEnsinoDocente(__SessionWEB.__UsuarioWEB.Docente.doc_id);
                            UCComboTipoModalidadeEnsino1.CarregarTipoModalidadeEnsinoDocente(__SessionWEB.__UsuarioWEB.Docente.doc_id);
                        }
                        else
                        {
                            UCComboTipoNivelEnsino1.CarregarTipoNivelEnsino();
                            UCComboTipoModalidadeEnsino1.CarregarTipoModalidadeEnsino();
                        }
                        grvEixo.CssClass += " accordion-grid";
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion Page life cycle

        #region Eventos

        private void UCComboTipoNivelEnsino1_IndexChanged()
        {
            try
            {
                Limpar();

                VS_tne_id = UCComboTipoNivelEnsino1.Valor;
                pnlCurriculo.Visible = UCComboTipoNivelEnsino1.Valor > 0 && UCComboTipoModalidadeEnsino1.Valor > 0;

                if (VS_tne_id > 0 && VS_tme_id > 0 && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    Carregar(-1);
                    
                    DataTable dtDisciplinas = ACA_TipoDisciplinaBO.SelecionaObrigatoriasPorNivelEnsino(VS_tne_id, VS_tme_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                    rblDisciplina.DataSource = dtDisciplinas;
                    rblDisciplina.DataBind();
                    rblDisciplina.Visible = dtDisciplinas.Rows.Count > 0;
                    if (rblDisciplina.Items.Count == 1)
                    {
                        rblDisciplina.SelectedIndex = 0;
                        rblDisciplina_SelectedIndexChanged(null, null);
                    }

                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        UCComboTipoCurriculoPeriodo1.CarregarPorNivelEnsinoModalidadeDocente(VS_tne_id, VS_tme_id, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                    }
                    else
                    {
                        UCComboTipoCurriculoPeriodo1.CarregarPorNivelEnsinoModalidade(VS_tne_id, VS_tme_id);
                    }

                    if (VS_permiteIncluirSugestao)
                    {
                        // Grava log do acesso
                        LOG_CurriculoSugestao logSugestao = new LOG_CurriculoSugestao
                        {
                            IsNew = true
                            ,
                            lcs_id = -1
                            ,
                            tne_id = VS_tne_id
                            ,
                            tme_id = VS_tme_id
                            ,
                            usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                            ,
                            lcs_data = DateTime.Now
                            ,
                            cal_ano = cal_ano
                        };
                        LOG_CurriculoSugestaoBO.Save(logSugestao);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar currículo.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboTipoModalidadeEnsino1_IndexChanged()
        {
            try
            {
                Limpar();

                VS_tme_id = UCComboTipoModalidadeEnsino1.Valor;
                pnlCurriculo.Visible = UCComboTipoNivelEnsino1.Valor > 0 && UCComboTipoModalidadeEnsino1.Valor > 0;

                if (VS_tne_id > 0 && VS_tme_id > 0 && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    Carregar(-1);

                    DataTable dtDisciplinas = ACA_TipoDisciplinaBO.SelecionaObrigatoriasPorNivelEnsino(VS_tne_id, VS_tme_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                    rblDisciplina.DataSource = dtDisciplinas;
                    rblDisciplina.DataBind();
                    rblDisciplina.Visible = dtDisciplinas.Rows.Count > 0;
                    if (rblDisciplina.Items.Count == 1)
                    {
                        rblDisciplina.SelectedIndex = 0;
                        rblDisciplina_SelectedIndexChanged(null, null);
                    }

                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        UCComboTipoCurriculoPeriodo1.CarregarPorNivelEnsinoModalidadeDocente(VS_tne_id, VS_tme_id, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                    }
                    else
                    {
                        UCComboTipoCurriculoPeriodo1.CarregarPorNivelEnsinoModalidade(VS_tne_id, VS_tme_id);
                    }

                    if (VS_permiteIncluirSugestao)
                    {
                        // Grava log do acesso
                        LOG_CurriculoSugestao logSugestao = new LOG_CurriculoSugestao
                        {
                            IsNew = true
                            ,
                            lcs_id = -1
                            ,
                            tne_id = VS_tne_id
                            ,
                            tme_id = VS_tme_id
                            ,
                            usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                            ,
                            lcs_data = DateTime.Now
                            ,
                            cal_ano = cal_ano
                        };
                        LOG_CurriculoSugestaoBO.Save(logSugestao);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar currículo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rblDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                litDisciplina.Text = rblDisciplina.SelectedItem.Text;
                fsDisciplina.Visible = Convert.ToInt32(rblDisciplina.SelectedValue) > 0;
                VS_ltCurriculoCapituloDisciplina = null;
                VS_ltCurriculoObjetivo = null;
                VS_ltCurriculoSugestaoCapituloDisciplina = null;
                VS_ltCurriculoSugestaoObjetivo = null;
                grvDisciplina.EditIndex = -1;
                grvEixo.EditIndex = -1;
                UCComboTipoCurriculoPeriodo1.SelectedIndex = 0;
                pnlHabilidades.Visible = false;

                if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    Carregar(VS_tds_id);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar currículo.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboTipoCurriculoPeriodo1_OnSelectedIndexChanged()
        {
            try
            {
                pnlHabilidades.Visible = UCComboTipoCurriculoPeriodo1.Valor > 0;
                VS_ltCurriculoObjetivo = null;
                VS_ltCurriculoSugestaoObjetivo = null;
                grvEixo.EditIndex = -1;

                if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    CarregarObjetivos(grvEixo, (byte)ACA_CurriculoObjetivoTipo.Eixo, -1);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar currículo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvGeral_DataBound(object sender, EventArgs e)
        {
            GridView grv = (GridView)sender;
            if (grv.EditIndex >= 0)
            {
                grv.Columns[colunaOrdem].Visible = false;
                grv.Columns[colunaExcluir].Visible = false;
            }
            else
            {
                grv.Columns[colunaExcluir].Visible = VS_permiteExcluir;
                grv.Columns[colunaEditar].Visible = VS_permiteEditar;

                int total = grv.Rows.Count;
                grv.Columns[colunaOrdem].Visible = VS_permiteEditar && total > 1;
                if (total > 1)
                {
                    //para deixar a seta do primeiro registro do grid só uma seta para baixo 
                    if (grv.Rows[0].FindControl("btnSubir") != null)
                        ((ImageButton)grv.Rows[0].FindControl("btnSubir")).Style.Add("visibility", "hidden");
                    //para deixar a seta do último registro do grid só uma seta para cima
                    if (grv.Rows[total - 1].FindControl("btnDescer") != null)
                        ((ImageButton)grv.Rows[total - 1].FindControl("btnDescer")).Style.Add("visibility", "hidden");
                }
            }
            grv.Columns[colunaSugestao].Visible = VS_permiteIncluirSugestao;
        }

        protected void grvGeral_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Subir" || e.CommandName == "Descer")
                {
                    GridView grv = (GridView)sender;
                    int index = int.Parse(e.CommandArgument.ToString());
                    int tds_id = Convert.ToInt32(grv.DataKeys[0]["tds_id"]);
                    int crc_idDescer = -1, crc_idSubir = -1;
                    int crc_ordemDescer = 0, crc_ordemSubir = 0;

                    if (e.CommandName == "Subir")
                    {
                        crc_idDescer = Convert.ToInt32(grv.DataKeys[index - 1]["crc_id"]);
                        crc_ordemDescer = Convert.ToInt32(grv.DataKeys[index]["crc_ordem"]);

                        crc_idSubir = Convert.ToInt32(grv.DataKeys[index]["crc_id"]);
                        crc_ordemSubir = Convert.ToInt32(grv.DataKeys[index - 1]["crc_ordem"]);
                    }
                    else if (e.CommandName == "Descer")
                    {
                        crc_idDescer = Convert.ToInt32(grv.DataKeys[index]["crc_id"]);
                        crc_ordemDescer = Convert.ToInt32(grv.DataKeys[index + 1]["crc_ordem"]);

                        crc_idSubir = Convert.ToInt32(grv.DataKeys[index + 1]["crc_id"]);
                        crc_ordemSubir = Convert.ToInt32(grv.DataKeys[index]["crc_ordem"]);
                    }

                    if (crc_idDescer > 0 && crc_idSubir > 0)
                    {
                        ACA_CurriculoCapitulo entityDescer = new ACA_CurriculoCapitulo { crc_id = crc_idDescer };
                        ACA_CurriculoCapituloBO.GetEntity(entityDescer);
                        entityDescer.crc_ordem = crc_ordemDescer;

                        ACA_CurriculoCapitulo entitySubir = new ACA_CurriculoCapitulo { crc_id = crc_idSubir };
                        ACA_CurriculoCapituloBO.GetEntity(entitySubir);
                        entitySubir.crc_ordem = crc_ordemSubir;

                        if (ACA_CurriculoCapituloBO.SaveOrdem(entityDescer, entitySubir))
                        {
                            grv.EditIndex = -1;
                            if (tds_id <= 0)
                            {
                                VS_ltCurriculoCapituloGeral = null;
                            }
                            else
                            {
                                VS_ltCurriculoCapituloDisciplina = null;
                            }
                            Carregar(tds_id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvGeral_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnSubir = (ImageButton)e.Row.FindControl("btnSubir");
                if (btnSubir != null)
                {
                    btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                    btnSubir.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnDescer = (ImageButton)e.Row.FindControl("btnDescer");
                if (btnDescer != null)
                {
                    btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                    btnDescer.CommandArgument = e.Row.RowIndex.ToString();
                }

                GridView grv = (GridView)sender;
                if (grv.EditIndex >= 0)
                {
                    ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");
                    if (btnEditar != null)
                    {
                        btnEditar.Visible = false;
                    }
                }

                Panel pnlItem = (Panel)e.Row.FindControl("pnlItem");
                if (pnlItem != null)
                {
                    pnlItem.Visible = VS_permiteEditar;
                }

                Panel pnlSugestao = (Panel)e.Row.FindControl("pnlSugestao");
                if (pnlSugestao != null)
                {
                    pnlSugestao.Visible = VS_permiteIncluirSugestao;

                    TextBox txtSugestao = (TextBox)e.Row.FindControl("txtSugestao");
                    if (txtSugestao != null)
                    {
                        int tds_id = Convert.ToInt32(grv.DataKeys[e.Row.RowIndex]["tds_id"]);
                        int crc_id = Convert.ToInt32(grv.DataKeys[e.Row.RowIndex]["crc_id"]);
                        sCurriculoSugestaoCapitulo sugestao = new sCurriculoSugestaoCapitulo();

                        if (tds_id > 0)
                        {
                            if (VS_ltCurriculoSugestaoCapituloDisciplina.Any(p => p.crc_id == crc_id))
                            {
                                sugestao = VS_ltCurriculoSugestaoCapituloDisciplina.Find(p => p.crc_id == crc_id);
                            }
                        }
                        else
                        {
                            if (VS_ltCurriculoSugestaoCapitulo.Any(p => p.crc_id == crc_id))
                            {
                                sugestao = VS_ltCurriculoSugestaoCapitulo.Find(p => p.crc_id == crc_id);
                            }
                        }

                        DropDownList ddlTipoSugestao = (DropDownList)e.Row.FindControl("ddlTipoSugestao");
                        if (sugestao.crs_id > 0)
                        {
                            txtSugestao.Text = sugestao.crs_sugestao;
                            if (ddlTipoSugestao != null)
                            {
                                ddlTipoSugestao.SelectedValue = sugestao.crs_tipo.ToString();
                            }

                            HiddenField hdnCrsId = (HiddenField)e.Row.FindControl("hdnCrsId");
                            if (hdnCrsId != null)
                            {
                                hdnCrsId.Value = sugestao.crs_id.ToString();
                            }
                        }

                        txtSugestao.Enabled = VS_abertoSugestao;
                        if (ddlTipoSugestao != null)
                        {
                            ddlTipoSugestao.Enabled = VS_abertoSugestao;
                        }
                    }
                }

                ImageButton btnIncluirSugestao = (ImageButton)e.Row.FindControl("btnIncluirSugestao");
                if (btnIncluirSugestao != null)
                {
                    int tds_id = Convert.ToInt32(grv.DataKeys[e.Row.RowIndex]["tds_id"]);
                    int crc_id = Convert.ToInt32(grv.DataKeys[e.Row.RowIndex]["crc_id"]);
                    btnIncluirSugestao.CssClass += (tds_id > 0 && VS_ltCurriculoSugestaoCapituloDisciplina.Any(p => p.crc_id == crc_id))
                                                    || (tds_id <= 0 && VS_ltCurriculoSugestaoCapitulo.Any(p => p.crc_id == crc_id)) ? " preenchido" : " vazio";
                }
            }
        }

        protected void grvGeral_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GridView grv = (GridView)sender;
                grv.EditIndex = e.NewEditIndex;
                Carregar(Convert.ToInt32(grv.DataKeys[0]["tds_id"]));

                ImageButton btnSalvar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnSalvar");
                if (btnSalvar != null)
                    btnSalvar.Visible = true;

                ImageButton btnEditar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnEditar");
                if (btnEditar != null)
                {
                    btnEditar.Visible = false;
                    ImageButton btnCancelarEdicao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnCancelarEdicao");
                    if (btnCancelarEdicao != null)
                        btnCancelarEdicao.Visible = true;
                }

                ImageButton btnSalvarSugestao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnSalvarSugestao");
                if (btnSalvarSugestao != null)
                    btnSalvarSugestao.Visible = VS_abertoSugestao;

                ImageButton btnIncluirSugestao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnIncluirSugestao");
                if (btnIncluirSugestao != null)
                {
                    btnIncluirSugestao.Visible = false;
                    ImageButton btnCancelarSugestao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnCancelarSugestao");
                    if (btnCancelarSugestao != null)
                        btnCancelarSugestao.Visible = true;
                }

                HiddenField hdnCrsId = (HiddenField)grv.Rows[e.NewEditIndex].FindControl("hdnCrsId");
                if (hdnCrsId != null)
                {
                    ImageButton btnExcluirSugestao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnExcluirSugestao");
                    if (btnExcluirSugestao != null)
                        btnExcluirSugestao.Visible = Convert.ToInt32(hdnCrsId.Value) > 0;
                }

                grv.Rows[e.NewEditIndex].Focus();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvGeral_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                if (VS_permiteEditar)
                {
                    TextBox txtTitulo = (TextBox)grv.Rows[e.RowIndex].FindControl("txtTitulo");
                    TextBox txtDescricao = (TextBox)grv.Rows[e.RowIndex].FindControl("txtDescricao");
                    if (txtTitulo != null && txtDescricao != null)
                    {
                        DataKey chave = grv.DataKeys[e.RowIndex];
                        int tds_id = Convert.ToInt32(chave["tds_id"]);
                        ACA_CurriculoCapitulo entity = new ACA_CurriculoCapitulo
                        {
                            IsNew = Convert.ToInt32(chave["crc_id"]) <= 0
                            ,
                            crc_id = Convert.ToInt32(chave["crc_id"])
                            ,
                            tne_id = VS_tne_id
                            ,
                            tme_id = VS_tme_id
                            ,
                            tds_id = tds_id
                            ,
                            cal_ano = cal_ano
                            ,
                            crc_titulo = txtTitulo.Text
                            ,
                            crc_descricao = txtDescricao.Text
                            ,
                            crc_ordem = Convert.ToInt32(chave["crc_ordem"])
                            ,
                            crc_situacao = 1
                            ,
                            crc_dataCriacao = DateTime.Now
                            ,
                            crc_dataAlteracao = DateTime.Now
                        };
                        if (ACA_CurriculoCapituloBO.Save(entity))
                        {
                            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoSalvar").ToString(), UtilBO.TipoMensagem.Sucesso);
                            grv.EditIndex = -1;
                            if (tds_id <= 0)
                            {
                                VS_ltCurriculoCapituloGeral = null;
                            }
                            else
                            {
                                VS_ltCurriculoCapituloDisciplina = null;
                            }
                            Carregar(tds_id);
                        }
                    }
                }
                else if (VS_permiteIncluirSugestao)
                {
                    TextBox txtSugestao = (TextBox)grv.Rows[e.RowIndex].FindControl("txtSugestao");
                    DropDownList ddlTipoSugestao = (DropDownList)grv.Rows[e.RowIndex].FindControl("ddlTipoSugestao");
                    HiddenField hdnCrsId = (HiddenField)grv.Rows[e.RowIndex].FindControl("hdnCrsId");
                    if (txtSugestao != null && ddlTipoSugestao != null && hdnCrsId != null)
                    {
                        DataKey chave = grv.DataKeys[e.RowIndex];
                        int tds_id = Convert.ToInt32(chave["tds_id"]);
                        int crc_id = Convert.ToInt32(chave["crc_id"]);
                        int crs_id = Convert.ToInt32(hdnCrsId.Value);
                        ACA_CurriculoSugestao entity = new ACA_CurriculoSugestao
                        {
                            IsNew = crs_id <= 0
                            ,
                            crs_id = crs_id
                            ,
                            crs_sugestao = txtSugestao.Text
                            ,
                            crs_tipo = Convert.ToByte(ddlTipoSugestao.SelectedValue)
                            ,
                            usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                            ,
                            crs_situacao = 1
                            ,
                            crs_dataCriacao = DateTime.Now
                            ,
                            crs_dataAlteracao = DateTime.Now
                        };
                        if (ACA_CurriculoCapituloSugestaoBO.Save(crc_id, entity))
                        {
                            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoSalvarSugestao").ToString(), UtilBO.TipoMensagem.Sucesso);
                            grv.EditIndex = -1;
                            if (tds_id <= 0)
                            {
                                VS_ltCurriculoSugestaoCapitulo = null;
                            }
                            else
                            {
                                VS_ltCurriculoSugestaoCapituloDisciplina = null;
                            }
                            Carregar(tds_id);
                        }
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
                if (VS_permiteEditar)
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar tópico.", UtilBO.TipoMensagem.Erro);
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar sugestão.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvGeral_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                if (Convert.ToInt32(grv.DataKeys[e.RowIndex]["crc_id"]) > 0)
                {
                    if (VS_permiteExcluir)
                    {
                        ACA_CurriculoCapitulo entity = new ACA_CurriculoCapitulo
                        {
                            crc_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["crc_id"])
                        };
                        if (ACA_CurriculoCapituloBO.Delete(entity))
                        {
                            int tds_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["tds_id"]);
                            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoExcluir").ToString(), UtilBO.TipoMensagem.Sucesso);
                            grv.EditIndex = -1;
                            if (tds_id <= 0)
                            {
                                VS_ltCurriculoCapituloGeral = null;
                            }
                            else
                            {
                                VS_ltCurriculoCapituloDisciplina = null;
                            }
                            Carregar(tds_id);
                        }
                    }
                    else if (VS_permiteIncluirSugestao)
                    {
                        HiddenField hdnCrsId = (HiddenField)grv.Rows[e.RowIndex].FindControl("hdnCrsId");
                        if (hdnCrsId != null && Convert.ToInt32(hdnCrsId.Value) > 0)
                        {
                            ACA_CurriculoSugestao entity = new ACA_CurriculoSugestao
                            {
                                crs_id = Convert.ToInt32(hdnCrsId.Value)
                            };
                            if (ACA_CurriculoSugestaoBO.Delete(entity))
                            {
                                int tds_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["tds_id"]);
                                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoExcluirSugestao").ToString(), UtilBO.TipoMensagem.Sucesso);
                                grv.EditIndex = -1;
                                if (tds_id <= 0)
                                {
                                    VS_ltCurriculoSugestaoCapitulo = null;
                                }
                                else
                                {
                                    VS_ltCurriculoSugestaoCapituloDisciplina = null;
                                }
                                Carregar(tds_id);
                            }
                        }
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
                if (VS_permiteExcluir)
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir tópico.", UtilBO.TipoMensagem.Erro);
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir sugestão.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvGeral_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (VS_permiteIncluir && Convert.ToInt32(grv.DataKeys[e.RowIndex]["crc_id"]) <= 0)
                {
                    int tds_id = Convert.ToInt32(grv.DataKeys[e.RowIndex]["tds_id"]);
                    if (tds_id <= 0)
                    {
                        VS_ltCurriculoCapituloGeral.RemoveAll(p => p.crc_id <= 0);
                    }
                    else
                    {
                        VS_ltCurriculoCapituloDisciplina.RemoveAll(p => p.crc_id <= 0);
                    }
                }
                grv.EditIndex = -1;
                Carregar(Convert.ToInt32(grv.DataKeys[0]["tds_id"]));
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvEixo_DataBound(object sender, EventArgs e)
        {
            GridView grv = (GridView)sender;
            if (grv.EditIndex >= 0)
            {
                grv.Columns[colunaOrdem].Visible = false;
                grv.Columns[colunaExcluir].Visible = false;
            }
            else
            {
                grv.Columns[colunaExcluir].Visible = VS_permiteExcluir;
                grv.Columns[colunaEditar].Visible = VS_permiteEditar;

                int total = grv.Rows.Count;
                grv.Columns[colunaOrdem].Visible = VS_permiteEditar && total > 1;
                if (total > 1)
                {
                    //para deixar a seta do primeiro registro do grid só uma seta para baixo 
                    if (grv.Rows[0].FindControl("btnSubir") != null)
                        ((ImageButton)grv.Rows[0].FindControl("btnSubir")).Style.Add("visibility", "hidden");
                    //para deixar a seta do último registro do grid só uma seta para cima
                    if (grv.Rows[total - 1].FindControl("btnDescer") != null)
                        ((ImageButton)grv.Rows[total - 1].FindControl("btnDescer")).Style.Add("visibility", "hidden");
                }
            }
            grv.Columns[colunaSugestao].Visible = VS_permiteIncluirSugestao;
        }

        protected void grvEixo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (!comandoExecutado && (e.CommandName == "Subir" || e.CommandName == "Descer"))
                {
                    // Variável adicionada para não propagar o comando para o grid pai.
                    comandoExecutado = true;

                    GridView grv = (GridView)sender;
                    int index = int.Parse(e.CommandArgument.ToString());
                    int cro_idDescer = -1, cro_idSubir = -1;
                    int cro_ordemDescer = 0, cro_ordemSubir = 0;

                    if (e.CommandName == "Subir")
                    {
                        cro_idDescer = Convert.ToInt32(grv.DataKeys[index - 1]["cro_id"]);
                        cro_ordemDescer = Convert.ToInt32(grv.DataKeys[index]["cro_ordem"]);

                        cro_idSubir = Convert.ToInt32(grv.DataKeys[index]["cro_id"]);
                        cro_ordemSubir = Convert.ToInt32(grv.DataKeys[index - 1]["cro_ordem"]);
                    }
                    else if (e.CommandName == "Descer")
                    {
                        cro_idDescer = Convert.ToInt32(grv.DataKeys[index]["cro_id"]);
                        cro_ordemDescer = Convert.ToInt32(grv.DataKeys[index + 1]["cro_ordem"]);

                        cro_idSubir = Convert.ToInt32(grv.DataKeys[index + 1]["cro_id"]);
                        cro_ordemSubir = Convert.ToInt32(grv.DataKeys[index]["cro_ordem"]);
                    }

                    if (cro_idDescer > 0 && cro_idSubir > 0)
                    {
                        ACA_CurriculoObjetivo entityDescer = new ACA_CurriculoObjetivo { cro_id = cro_idDescer };
                        ACA_CurriculoObjetivoBO.GetEntity(entityDescer);
                        entityDescer.cro_ordem = cro_ordemDescer;

                        ACA_CurriculoObjetivo entitySubir = new ACA_CurriculoObjetivo { cro_id = cro_idSubir };
                        ACA_CurriculoObjetivoBO.GetEntity(entitySubir);
                        entitySubir.cro_ordem = cro_ordemSubir;

                        if (ACA_CurriculoObjetivoBO.SaveOrdem(entityDescer, entitySubir))
                        {
                            grv.EditIndex = -1;
                            VS_ltCurriculoObjetivo = null;
                            GuardarEixosAbertos();
                            CarregarObjetivos(grv, Convert.ToByte(grv.DataKeys[index]["cro_tipo"]), Convert.ToInt32(grv.DataKeys[index]["cro_idPai"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvEixo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnSubir = (ImageButton)e.Row.FindControl("btnSubir");
                if (btnSubir != null)
                {
                    btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                    btnSubir.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnDescer = (ImageButton)e.Row.FindControl("btnDescer");
                if (btnDescer != null)
                {
                    btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                    btnDescer.CommandArgument = e.Row.RowIndex.ToString();
                }

                GridView grv = (GridView)sender;
                if (grv.EditIndex >= 0)
                {
                    ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");
                    if (btnEditar != null)
                    {
                        btnEditar.Visible = false;
                    }
                }

                int cro_id = Convert.ToInt32(grv.DataKeys[e.Row.RowIndex]["cro_id"]);
                Button btnNovoObjetivo = (Button)e.Row.FindControl("btnNovoObjetivo");
                if (btnNovoObjetivo != null)
                {
                    btnNovoObjetivo.Visible = VS_permiteIncluir;
                    btnNovoObjetivo.CommandArgument = cro_id.ToString();
                }

                GridView grvObjetivo = (GridView)e.Row.FindControl("grvObjetivo");
                if (grvObjetivo != null)
                {
                    byte cro_tipo = Convert.ToByte(grv.DataKeys[e.Row.RowIndex]["cro_tipo"]);
                    if (cro_tipo == (byte)ACA_CurriculoObjetivoTipo.Eixo)
                    {
                        CarregarObjetivos(grvObjetivo, (byte)ACA_CurriculoObjetivoTipo.Topico, cro_id);
                    }
                    else if (cro_tipo == (byte)ACA_CurriculoObjetivoTipo.Topico)
                    {
                        CarregarObjetivos(grvObjetivo, (byte)ACA_CurriculoObjetivoTipo.ObjetivoAprendizagem, cro_id);
                    }
                }

                HiddenField hdnAberto = (HiddenField)e.Row.FindControl("hdnAberto");
                if (hdnAberto != null)
                {
                    hdnAberto.Value = ltEixoAberto.Any(p => p == cro_id) ? "1" : "0";
                }

                Panel pnlItem = (Panel)e.Row.FindControl("pnlItem");
                if (pnlItem != null)
                {
                    pnlItem.Visible = VS_permiteEditar;
                }

                Panel pnlSugestao = (Panel)e.Row.FindControl("pnlSugestao");
                if (pnlSugestao != null)
                {
                    pnlSugestao.Visible = VS_permiteIncluirSugestao;

                    TextBox txtSugestao = (TextBox)e.Row.FindControl("txtSugestao");
                    if (txtSugestao != null)
                    {
                        sCurriculoSugestaoObjetivo sugestao = new sCurriculoSugestaoObjetivo();
                        if (VS_ltCurriculoSugestaoObjetivo.Any(p => p.cro_id == cro_id))
                        {
                            sugestao = VS_ltCurriculoSugestaoObjetivo.Find(p => p.cro_id == cro_id);
                        }

                        DropDownList ddlTipoSugestao = (DropDownList)e.Row.FindControl("ddlTipoSugestao");
                        if (sugestao.crs_id > 0)
                        {
                            txtSugestao.Text = sugestao.crs_sugestao;
                            if (ddlTipoSugestao != null)
                            {
                                ddlTipoSugestao.SelectedValue = sugestao.crs_tipo.ToString();
                            }

                            HiddenField hdnCrsId = (HiddenField)e.Row.FindControl("hdnCrsId");
                            if (hdnCrsId != null)
                            {
                                hdnCrsId.Value = sugestao.crs_id.ToString();
                            }
                        }

                        txtSugestao.Enabled = VS_abertoSugestao;
                        if (ddlTipoSugestao != null)
                        {
                            ddlTipoSugestao.Enabled = VS_abertoSugestao;
                        }
                    }
                }

                ImageButton btnIncluirSugestao = (ImageButton)e.Row.FindControl("btnIncluirSugestao");
                if (btnIncluirSugestao != null)
                {
                    btnIncluirSugestao.CssClass += VS_ltCurriculoSugestaoObjetivo.Any(p => p.cro_id == cro_id) ? " preenchido" : " vazio";
                }
            }
        }

        protected void grvEixo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GridView grv = (GridView)sender;
                grv.EditIndex = e.NewEditIndex;
                GuardarEixosAbertos();
                CarregarObjetivos(grv, Convert.ToByte(grv.DataKeys[e.NewEditIndex]["cro_tipo"]), Convert.ToInt32(grv.DataKeys[e.NewEditIndex]["cro_idPai"]));

                ImageButton btnSalvar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnSalvar");
                if (btnSalvar != null)
                    btnSalvar.Visible = true;

                ImageButton btnEditar = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnEditar");
                if (btnEditar != null)
                {
                    btnEditar.Visible = false;
                    ImageButton btnCancelarEdicao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnCancelarEdicao");
                    if (btnCancelarEdicao != null)
                        btnCancelarEdicao.Visible = true;
                }

                ImageButton btnSalvarSugestao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnSalvarSugestao");
                if (btnSalvarSugestao != null)
                    btnSalvarSugestao.Visible = VS_abertoSugestao;

                ImageButton btnIncluirSugestao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnIncluirSugestao");
                if (btnIncluirSugestao != null)
                {
                    btnIncluirSugestao.Visible = false;
                    ImageButton btnCancelarSugestao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnCancelarSugestao");
                    if (btnCancelarSugestao != null)
                        btnCancelarSugestao.Visible = true;
                }

                HiddenField hdnCrsId = (HiddenField)grv.Rows[e.NewEditIndex].FindControl("hdnCrsId");
                if (hdnCrsId != null)
                {
                    ImageButton btnExcluirSugestao = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnExcluirSugestao");
                    if (btnExcluirSugestao != null)
                        btnExcluirSugestao.Visible = Convert.ToInt32(hdnCrsId.Value) > 0;
                }

                grv.Rows[e.NewEditIndex].Focus();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvEixo_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                if (VS_permiteEditar)
                {
                    TextBox txtDescricao = (TextBox)grv.Rows[e.RowIndex].FindControl("txtDescricao");
                    if (txtDescricao != null)
                    {
                        DataKey chave = grv.DataKeys[e.RowIndex];
                        byte cro_tipo = Convert.ToByte(chave["cro_tipo"]);
                        int cro_idPai = Convert.ToInt32(chave["cro_idPai"]);
                        ACA_CurriculoObjetivo entity = new ACA_CurriculoObjetivo
                        {
                            IsNew = Convert.ToInt32(chave["cro_id"]) <= 0
                            ,
                            cro_id = Convert.ToInt32(chave["cro_id"])
                            ,
                            tne_id = VS_tne_id
                            ,
                            tme_id = VS_tme_id
                            ,
                            tds_id = VS_tds_id
                            ,
                            tcp_id = VS_tcp_id
                            ,
                            cal_ano = cal_ano
                            ,
                            cro_descricao = txtDescricao.Text
                            ,
                            cro_ordem = Convert.ToInt32(chave["cro_ordem"])
                            ,
                            cro_tipo = cro_tipo
                            ,
                            cro_idPai = cro_idPai
                            ,
                            cro_situacao = 1
                            ,
                            cro_dataCriacao = DateTime.Now
                            ,
                            cro_dataAlteracao = DateTime.Now
                        };
                        if (ACA_CurriculoObjetivoBO.Save(entity))
                        {
                            if (cro_tipo == (byte)ACA_CurriculoObjetivoTipo.Eixo)
                            {
                                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoSalvarEixo").ToString(), UtilBO.TipoMensagem.Sucesso);
                            }
                            else if (cro_tipo == (byte)ACA_CurriculoObjetivoTipo.Topico)
                            {
                                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoSalvarObjetivo").ToString(), UtilBO.TipoMensagem.Sucesso);
                            }
                            else if (cro_tipo == (byte)ACA_CurriculoObjetivoTipo.ObjetivoAprendizagem)
                            {
                                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoSalvarObjetivoAprendizagem").ToString(), UtilBO.TipoMensagem.Sucesso);
                            }
                            grv.EditIndex = -1;
                            VS_ltCurriculoObjetivo = null;
                            GuardarEixosAbertos();
                            CarregarObjetivos(grv, cro_tipo, cro_idPai);
                        }
                    }
                }
                else if (VS_permiteIncluirSugestao)
                {
                    TextBox txtSugestao = (TextBox)grv.Rows[e.RowIndex].FindControl("txtSugestao");
                    DropDownList ddlTipoSugestao = (DropDownList)grv.Rows[e.RowIndex].FindControl("ddlTipoSugestao");
                    HiddenField hdnCrsId = (HiddenField)grv.Rows[e.RowIndex].FindControl("hdnCrsId");
                    if (txtSugestao != null && ddlTipoSugestao != null && hdnCrsId != null)
                    {
                        DataKey chave = grv.DataKeys[e.RowIndex];
                        int cro_id = Convert.ToInt32(chave["cro_id"]);
                        byte cro_tipo = Convert.ToByte(chave["cro_tipo"]);
                        int cro_idPai = Convert.ToInt32(chave["cro_idPai"]);
                        int crs_id = Convert.ToInt32(hdnCrsId.Value);
                        ACA_CurriculoSugestao entity = new ACA_CurriculoSugestao
                        {
                            IsNew = crs_id <= 0
                            ,
                            crs_id = crs_id
                            ,
                            crs_sugestao = txtSugestao.Text
                            ,
                            crs_tipo = Convert.ToByte(ddlTipoSugestao.SelectedValue)
                            ,
                            usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id
                            ,
                            crs_situacao = 1
                            ,
                            crs_dataCriacao = DateTime.Now
                            ,
                            crs_dataAlteracao = DateTime.Now
                        };
                        if (ACA_CurriculoObjetivoSugestaoBO.Save(cro_id, entity))
                        {
                            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoSalvarSugestao").ToString(), UtilBO.TipoMensagem.Sucesso);
                            grv.EditIndex = -1;
                            VS_ltCurriculoSugestaoObjetivo = null;
                            GuardarEixosAbertos();
                            CarregarObjetivos(grv, cro_tipo, cro_idPai);
                        }
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
                if (VS_permiteEditar)
                { 
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar objetivo.", UtilBO.TipoMensagem.Erro);
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar sugestão.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvEixo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                DataKey chave = grv.DataKeys[e.RowIndex];
                if (Convert.ToInt32(chave["cro_id"]) > 0)
                {
                    if (VS_permiteExcluir)
                    {
                        ACA_CurriculoObjetivo entity = new ACA_CurriculoObjetivo
                        {
                            cro_id = Convert.ToInt32(chave["cro_id"])
                        };
                        if (ACA_CurriculoObjetivoBO.Delete(entity))
                        {
                            byte cro_tipo = Convert.ToByte(chave["cro_tipo"]);
                            int cro_idPai = Convert.ToInt32(chave["cro_idPai"]);
                            if (cro_tipo == (byte)ACA_CurriculoObjetivoTipo.Eixo)
                            {
                                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoExcluirEixo").ToString(), UtilBO.TipoMensagem.Sucesso);
                            }
                            else if (cro_tipo == (byte)ACA_CurriculoObjetivoTipo.Topico)
                            {
                                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoExcluirObjetivo").ToString(), UtilBO.TipoMensagem.Sucesso);
                            }
                            else if (cro_tipo == (byte)ACA_CurriculoObjetivoTipo.ObjetivoAprendizagem)
                            {
                                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoExcluirObjetivoAprendizagem").ToString(), UtilBO.TipoMensagem.Sucesso);
                            }
                            grv.EditIndex = -1;
                            VS_ltCurriculoObjetivo = null;
                            GuardarEixosAbertos();
                            CarregarObjetivos(grv, cro_tipo, cro_idPai);
                        }
                    }
                    else if (VS_permiteIncluirSugestao)
                    {
                        HiddenField hdnCrsId = (HiddenField)grv.Rows[e.RowIndex].FindControl("hdnCrsId");
                        if (hdnCrsId != null && Convert.ToInt32(hdnCrsId.Value) > 0)
                        {
                            ACA_CurriculoSugestao entity = new ACA_CurriculoSugestao
                            {
                                crs_id = Convert.ToInt32(hdnCrsId.Value)
                            };
                            if (ACA_CurriculoSugestaoBO.Delete(entity))
                            {
                                byte cro_tipo = Convert.ToByte(chave["cro_tipo"]);
                                int cro_idPai = Convert.ToInt32(chave["cro_idPai"]);
                                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.MensagemSucessoExcluirSugestao").ToString(), UtilBO.TipoMensagem.Sucesso);
                                grv.EditIndex = -1;
                                VS_ltCurriculoSugestaoObjetivo = null;
                                GuardarEixosAbertos();
                                CarregarObjetivos(grv, cro_tipo, cro_idPai);
                            }
                        }
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
                if (VS_permiteExcluir)
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir objetivo.", UtilBO.TipoMensagem.Erro);
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir sugestão.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvEixo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                DataKey chave = grv.DataKeys[e.RowIndex];
                byte cro_tipo = Convert.ToByte(chave["cro_tipo"]);
                int cro_idPai = Convert.ToInt32(chave["cro_idPai"]);
                if (VS_permiteIncluir && Convert.ToInt32(chave["cro_id"]) <= 0)
                {
                    VS_ltCurriculoObjetivo.RemoveAll(p => p.cro_tipo == cro_tipo && p.cro_idPai == cro_idPai && p.cro_id <= 0);
                }
                grv.EditIndex = -1;
                GuardarEixosAbertos();
                CarregarObjetivos(grv, cro_tipo, cro_idPai);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoGeral_Click(object sender, EventArgs e)
        {
            try
            {
                // Se ainda não existe outro item novo sendo cadastrado...
                if (grvGeral.EditIndex < 0 || Convert.ToInt32(grvGeral.DataKeys[grvGeral.EditIndex]["crc_id"]) > 0)
                {
                    sCurriculoCapitulo entity = new sCurriculoCapitulo
                    {
                        crc_id = -1
                        ,
                        tds_id = -1
                        ,
                        crc_titulo = string.Empty
                        ,
                        crc_descricao = string.Empty
                        ,
                        crc_ordem = grvGeral.Rows.Count > 0 ? Convert.ToInt32(grvGeral.DataKeys[grvGeral.Rows.Count - 1]["crc_ordem"]) + 1 : 1
                    };
                    VS_ltCurriculoCapituloGeral.Add(entity);
                    int index = VS_ltCurriculoCapituloGeral.Count - 1;
                    grvGeral.EditIndex = index;
                    Carregar(-1);

                    ImageButton btnSalvar = (ImageButton)grvGeral.Rows[index].FindControl("btnSalvar");
                    if (btnSalvar != null)
                        btnSalvar.Visible = true;

                    ImageButton btnEditar = (ImageButton)grvGeral.Rows[index].FindControl("btnEditar");
                    if (btnEditar != null)
                    {
                        btnEditar.Visible = false;
                        ImageButton btnCancelarEdicao = (ImageButton)grvGeral.Rows[index].FindControl("btnCancelarEdicao");
                        if (btnCancelarEdicao != null)
                            btnCancelarEdicao.Visible = true;
                    }

                    ImageButton btnExcluir = (ImageButton)grvGeral.Rows[index].FindControl("btnExcluir");
                    if (btnExcluir != null)
                        btnExcluir.Visible = false;

                    grvGeral.Rows[index].Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar tópico.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoDisciplina_Click(object sender, EventArgs e)
        {
            try
            {
                // Se ainda não existe outro item novo sendo cadastrado...
                if (grvDisciplina.EditIndex < 0 || Convert.ToInt32(grvDisciplina.DataKeys[grvDisciplina.EditIndex]["crc_id"]) > 0)
                {
                    sCurriculoCapitulo entity = new sCurriculoCapitulo
                    {
                        crc_id = -1
                        ,
                        tds_id = VS_tds_id
                        ,
                        crc_titulo = string.Empty
                        ,
                        crc_descricao = string.Empty
                        ,
                        crc_ordem = grvDisciplina.Rows.Count > 0 ? Convert.ToInt32(grvDisciplina.DataKeys[grvDisciplina.Rows.Count - 1]["crc_ordem"]) + 1 : 1
                    };
                    VS_ltCurriculoCapituloDisciplina.Add(entity);
                    int index = VS_ltCurriculoCapituloDisciplina.Count - 1;
                    grvDisciplina.EditIndex = index;
                    Carregar(VS_tds_id);

                    ImageButton btnSalvar = (ImageButton)grvDisciplina.Rows[index].FindControl("btnSalvar");
                    if (btnSalvar != null)
                        btnSalvar.Visible = true;

                    ImageButton btnEditar = (ImageButton)grvDisciplina.Rows[index].FindControl("btnEditar");
                    if (btnEditar != null)
                    {
                        btnEditar.Visible = false;
                        ImageButton btnCancelarEdicao = (ImageButton)grvDisciplina.Rows[index].FindControl("btnCancelarEdicao");
                        if (btnCancelarEdicao != null)
                            btnCancelarEdicao.Visible = true;
                    }

                    ImageButton btnExcluir = (ImageButton)grvDisciplina.Rows[index].FindControl("btnExcluir");
                    if (btnExcluir != null)
                        btnExcluir.Visible = false;

                    grvDisciplina.Rows[index].Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar tópico.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoEixo_Click(object sender, EventArgs e)
        {
            try
            {
                // Se ainda não existe outro item novo sendo cadastrado...
                if (grvEixo.EditIndex < 0 || Convert.ToInt32(grvEixo.DataKeys[grvEixo.EditIndex]["cro_id"]) > 0)
                {
                    sCurriculoObjetivo entity = new sCurriculoObjetivo
                    {
                        cro_id = -1
                        ,
                        cro_descricao = string.Empty
                        ,
                        cro_ordem = grvEixo.Rows.Count > 0 ? Convert.ToInt32(grvEixo.DataKeys[grvEixo.Rows.Count - 1]["cro_ordem"]) + 1 : 1
                        ,
                        cro_tipo = (byte)ACA_CurriculoObjetivoTipo.Eixo
                        ,
                        cro_idPai = -1
                    };
                    VS_ltCurriculoObjetivo.Add(entity);
                    int index = VS_ltCurriculoObjetivo.FindAll(p => p.cro_tipo == (byte)ACA_CurriculoObjetivoTipo.Eixo && p.cro_idPai <= 0).Count - 1;
                    grvEixo.EditIndex = index;
                    GuardarEixosAbertos();
                    CarregarObjetivos(grvEixo, (byte)ACA_CurriculoObjetivoTipo.Eixo, -1);

                    ImageButton btnSalvar = (ImageButton)grvEixo.Rows[index].FindControl("btnSalvar");
                    if (btnSalvar != null)
                        btnSalvar.Visible = true;

                    ImageButton btnEditar = (ImageButton)grvEixo.Rows[index].FindControl("btnEditar");
                    if (btnEditar != null)
                    {
                        btnEditar.Visible = false;
                        ImageButton btnCancelarEdicao = (ImageButton)grvEixo.Rows[index].FindControl("btnCancelarEdicao");
                        if (btnCancelarEdicao != null)
                            btnCancelarEdicao.Visible = true;
                    }

                    ImageButton btnExcluir = (ImageButton)grvEixo.Rows[index].FindControl("btnExcluir");
                    if (btnExcluir != null)
                        btnExcluir.Visible = false;

                    grvEixo.Rows[index].Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar eixo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoObjetivo_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnNovoObjetivo = (Button)sender;
                GridView grvObjetivo = (GridView)btnNovoObjetivo.Parent.FindControl("grvObjetivo");
                // Se ainda não existe outro item novo sendo cadastrado...
                if (grvObjetivo.EditIndex < 0 || Convert.ToInt32(grvObjetivo.DataKeys[grvObjetivo.EditIndex]["cro_id"]) > 0)
                {
                    int cro_idPai = Convert.ToInt32(btnNovoObjetivo.CommandArgument);
                    sCurriculoObjetivo entity = new sCurriculoObjetivo
                    {
                        cro_id = -1
                        ,
                        cro_descricao = string.Empty
                        ,
                        cro_ordem = grvObjetivo.Rows.Count > 0 ? Convert.ToInt32(grvObjetivo.DataKeys[grvObjetivo.Rows.Count - 1]["cro_ordem"]) + 1 : 1
                        ,
                        cro_tipo = (byte)ACA_CurriculoObjetivoTipo.Topico
                        ,
                        cro_idPai = cro_idPai
                    };
                    VS_ltCurriculoObjetivo.Add(entity);
                    int index = VS_ltCurriculoObjetivo.FindAll(p => p.cro_tipo == (byte)ACA_CurriculoObjetivoTipo.Topico && p.cro_idPai == cro_idPai).Count - 1;
                    grvObjetivo.EditIndex = index;
                    CarregarObjetivos(grvObjetivo, (byte)ACA_CurriculoObjetivoTipo.Topico, cro_idPai);

                    ImageButton btnSalvar = (ImageButton)grvObjetivo.Rows[index].FindControl("btnSalvar");
                    if (btnSalvar != null)
                        btnSalvar.Visible = true;

                    ImageButton btnEditar = (ImageButton)grvObjetivo.Rows[index].FindControl("btnEditar");
                    if (btnEditar != null)
                    {
                        btnEditar.Visible = false;
                        ImageButton btnCancelarEdicao = (ImageButton)grvObjetivo.Rows[index].FindControl("btnCancelarEdicao");
                        if (btnCancelarEdicao != null)
                            btnCancelarEdicao.Visible = true;
                    }

                    ImageButton btnExcluir = (ImageButton)grvObjetivo.Rows[index].FindControl("btnExcluir");
                    if (btnExcluir != null)
                        btnExcluir.Visible = false;

                    grvObjetivo.Rows[index].Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar objetivo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoObjetivoAprendizagem_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnNovoObjetivo = (Button)sender;
                GridView grvObjetivo = (GridView)btnNovoObjetivo.Parent.FindControl("grvObjetivo");
                // Se ainda não existe outro item novo sendo cadastrado...
                if (grvObjetivo.EditIndex < 0 || Convert.ToInt32(grvObjetivo.DataKeys[grvObjetivo.EditIndex]["cro_id"]) > 0)
                {
                    int cro_idPai = Convert.ToInt32(btnNovoObjetivo.CommandArgument);
                    sCurriculoObjetivo entity = new sCurriculoObjetivo
                    {
                        cro_id = -1
                        ,
                        cro_descricao = string.Empty
                        ,
                        cro_ordem = grvObjetivo.Rows.Count > 0 ? Convert.ToInt32(grvObjetivo.DataKeys[grvObjetivo.Rows.Count - 1]["cro_ordem"]) + 1 : 1
                        ,
                        cro_tipo = (byte)ACA_CurriculoObjetivoTipo.ObjetivoAprendizagem
                        ,
                        cro_idPai = cro_idPai
                    };
                    VS_ltCurriculoObjetivo.Add(entity);
                    int index = VS_ltCurriculoObjetivo.FindAll(p => p.cro_tipo == (byte)ACA_CurriculoObjetivoTipo.ObjetivoAprendizagem && p.cro_idPai == cro_idPai).Count - 1;
                    grvObjetivo.EditIndex = index;
                    CarregarObjetivos(grvObjetivo, (byte)ACA_CurriculoObjetivoTipo.ObjetivoAprendizagem, cro_idPai);

                    ImageButton btnSalvar = (ImageButton)grvObjetivo.Rows[index].FindControl("btnSalvar");
                    if (btnSalvar != null)
                        btnSalvar.Visible = true;

                    ImageButton btnEditar = (ImageButton)grvObjetivo.Rows[index].FindControl("btnEditar");
                    if (btnEditar != null)
                    {
                        btnEditar.Visible = false;
                        ImageButton btnCancelarEdicao = (ImageButton)grvObjetivo.Rows[index].FindControl("btnCancelarEdicao");
                        if (btnCancelarEdicao != null)
                            btnCancelarEdicao.Visible = true;
                    }

                    ImageButton btnExcluir = (ImageButton)grvObjetivo.Rows[index].FindControl("btnExcluir");
                    if (btnExcluir != null)
                        btnExcluir.Visible = false;

                    grvObjetivo.Rows[index].Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar objetivo de aprendizagem.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Limpa os dados da tela.
        /// </summary>
        private void Limpar()
        {
            rblDisciplina.Items.Clear();
            litDisciplina.Text = string.Empty;
            grvGeral.EditIndex = -1;
            grvGeral.DataSource = new List<sCurriculoCapitulo>();
            grvGeral.DataBind();
            grvDisciplina.EditIndex = -1;
            grvDisciplina.DataSource = new List<sCurriculoCapitulo>();
            grvDisciplina.DataBind();
            grvEixo.EditIndex = -1;
            grvEixo.DataSource = new List<sCurriculoObjetivo>();
            grvEixo.DataBind();
            VS_ltCurriculoCapituloGeral = null;
            VS_ltCurriculoCapituloDisciplina = null;
            VS_ltCurriculoObjetivo = null;
            fsDisciplina.Visible = false;
            UCComboTipoCurriculoPeriodo1.SelectedIndex = 0;
            pnlHabilidades.Visible = false;
            VS_ltCurriculoSugestaoCapitulo = null;
            VS_ltCurriculoSugestaoCapituloDisciplina = null;
            VS_ltCurriculoSugestaoObjetivo = null;
        }

        /// <summary>
        /// Carrega os dados de acordo com o tipo de nível de ensino selecionado.
        /// </summary>
        private void Carregar(int tds_id)
        {
            if (tds_id <= 0)
            {
                grvGeral.DataSource = VS_ltCurriculoCapituloGeral;
                grvGeral.DataBind();
            }
            else
            {
                grvDisciplina.DataSource = VS_ltCurriculoCapituloDisciplina;
                grvDisciplina.DataBind();
            }
        }

        /// <summary>
        /// Carrega os objetivos de acordo com o tipo de nível de ensino selecionado.
        /// </summary>
        private void CarregarObjetivos(GridView grvObjetivo, byte tipoObjetivo, int cro_idPai)
        {
            grvObjetivo.DataSource = VS_ltCurriculoObjetivo.FindAll(p => p.cro_tipo == tipoObjetivo && p.cro_idPai == cro_idPai).OrderBy(p => p.cro_ordem);
            grvObjetivo.DataBind();
        }

        /// <summary>
        /// Armazena os eixos abertos (accordion), para manter o estado.
        /// </summary>
        private void GuardarEixosAbertos()
        {
            ltEixoAberto = new List<int>();
            foreach (GridViewRow eixo in grvEixo.Rows)
            {
                HiddenField hdnAberto = (HiddenField)eixo.FindControl("hdnAberto");
                if (hdnAberto != null && hdnAberto.Value == "1")
                {
                    ltEixoAberto.Add(Convert.ToInt32(grvEixo.DataKeys[eixo.DataItemIndex]["cro_id"]));
                }
            }
        }

        #endregion Métodos
    }
}