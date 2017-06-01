using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.Curriculo
{
    public partial class UCCurriculo : MotherUserControl
    {
        #region Constantes

        private const int colunaOrdem = 1;
        private const int colunaEditar = 2;
        private const int colunaExcluir = 3;

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

        public List<sCurriculoCapitulo> VS_ltCurriculoCapituloGeral
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

        public List<sCurriculoCapitulo> VS_ltCurriculoCapituloDisciplina
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

        #endregion Propriedades

        #region Page life cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    UCComboTipoNivelEnsino1.CarregarTipoNivelEnsino();
                    UCComboTipoModalidadeEnsino1.CarregarTipoModalidadeEnsino();
                    lblEmptyEixo.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Curriculo.Cadastro.lblEmptyEixo.Text").ToString(), UtilBO.TipoMensagem.Nenhuma);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }

            btnNovoGeral.Visible = btnNovoDisciplina.Visible = VS_permiteIncluir;

            UCComboTipoNivelEnsino1.IndexChanged += UCComboTipoNivelEnsino1_IndexChanged;
            UCComboTipoModalidadeEnsino1.IndexChanged += UCComboTipoModalidadeEnsino1_IndexChanged;
            UCComboTipoCurriculoPeriodo1.OnSelectedIndexChanged += UCComboTipoCurriculoPeriodo1_OnSelectedIndexChanged;
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

                if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    UCComboTipoCurriculoPeriodo1.CarregarPorNivelEnsinoModalidade(VS_tne_id, VS_tme_id);
                    Carregar(-1);

                    DataTable dtDisciplinas = ACA_TipoDisciplinaBO.SelecionaObrigatoriasPorNivelEnsino(VS_tne_id, VS_tme_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    rblDisciplina.DataSource = dtDisciplinas;
                    rblDisciplina.DataBind();
                    rblDisciplina.Visible = dtDisciplinas.Rows.Count > 0;
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

                if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    UCComboTipoCurriculoPeriodo1.CarregarPorNivelEnsinoModalidade(VS_tne_id, VS_tme_id);
                    Carregar(-1);

                    DataTable dtDisciplinas = ACA_TipoDisciplinaBO.SelecionaObrigatoriasPorNivelEnsino(VS_tne_id, VS_tme_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    rblDisciplina.DataSource = dtDisciplinas;
                    rblDisciplina.DataBind();
                    rblDisciplina.Visible = dtDisciplinas.Rows.Count > 0;
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
                grvDisciplina.EditIndex = -1;

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
                    byte crc_ordemDescer = 0, crc_ordemSubir = 0;

                    if (e.CommandName == "Subir")
                    {
                        crc_idDescer = Convert.ToInt32(grv.DataKeys[index - 1]["crc_id"]);
                        crc_ordemDescer = Convert.ToByte(grv.DataKeys[index]["crc_ordem"]);

                        crc_idSubir = Convert.ToInt32(grv.DataKeys[index]["crc_id"]);
                        crc_ordemSubir = Convert.ToByte(grv.DataKeys[index - 1]["crc_ordem"]);
                    }
                    else if (e.CommandName == "Descer")
                    {
                        crc_idDescer = Convert.ToInt32(grv.DataKeys[index]["crc_id"]);
                        crc_ordemDescer = Convert.ToByte(grv.DataKeys[index + 1]["crc_ordem"]);

                        crc_idSubir = Convert.ToInt32(grv.DataKeys[index + 1]["crc_id"]);
                        crc_ordemSubir = Convert.ToByte(grv.DataKeys[index]["crc_ordem"]);
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
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar tópico.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvGeral_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grv = ((GridView)sender);
            try
            {
                if (Convert.ToInt32(grv.DataKeys[e.RowIndex]["crc_id"]) > 0)
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
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir tópico.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvGeral_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GridView grv = ((GridView)sender);
                if (Convert.ToInt32(grv.DataKeys[e.RowIndex]["crc_id"]) <= 0)
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

        protected void btnNovoGeral_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar adicionar tópico.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoEixo_Click(object sender, EventArgs e)
        {

        }

        protected void btnNovoObjetivo_Click(object sender, EventArgs e)
        {

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
            VS_ltCurriculoCapituloGeral = null;
            VS_ltCurriculoCapituloDisciplina = null;
            fsDisciplina.Visible = false;
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

        #endregion Métodos
    }
}