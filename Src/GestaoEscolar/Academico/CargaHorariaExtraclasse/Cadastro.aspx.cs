namespace GestaoEscolar.Academico.CargaHorariaExtraclasse
{
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using MSTech.GestaoEscolar.BLL;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using MSTech.CoreSSO.BLL;
    using System.Web.UI.HtmlControls;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.Validation.Exceptions;
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        private int Cur_id
        {
            get
            {
                return UCCCursoCurriculo.Valor[0];
            }
        }

        private int Crr_id
        {
            get
            {
                return UCCCursoCurriculo.Valor[1];
            }
        }

        private int Crp_id
        {
            get
            {
                return UCCCurriculoPeriodo.Valor[2];
            }
        }

        private int Cal_id
        {
            get
            {
                return UCCCalendario.Valor;
            }
        }

        private List<sTipoPeriodoCalendario> LstTipoPeriodoCalendario;

        private List<ACA_CargaHorariaExtraclasse> LstCargaHoraria;

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;
            UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;
            UCCCurriculoPeriodo.IndexChanged += UCCCurriculoPeriodo_IndexChanged;

            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                }

                if (!IsPostBack)
                {
                    string msg = __SessionWEB.PostMessages;
                    if (!string.IsNullOrEmpty(msg))
                    {
                        lblMensagem.Text = msg;
                    }

                    UCCCursoCurriculo.CarregarPorModalidadeEnsino(ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_MODALIDADE_CIEJA, Ent_ID_UsuarioLogado));
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Métodos

        private void CarregarTela()
        {
            try
            {
                rptDisciplinas.Visible = btnSalvar.Visible = false;

                if (Cur_id > 0 && Crr_id > 0 && Crp_id > 0 && Cal_id > 0)
                {
                    LstTipoPeriodoCalendario = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(UCCCalendario.Valor, ApplicationWEB.AppMinutosCacheLongo);

                    LstCargaHoraria = ACA_CargaHorariaExtraclasseBO.SelecionaPorCurriculoPeriodoCalendario(Cur_id, Crr_id, Crp_id, Cal_id);

                    rptDisciplinas.DataSource = ACA_CurriculoDisciplinaBO.SelecionaDisciplinasParaFormacaoTurmaNormal(Cur_id, Crr_id, Crp_id);
                    rptDisciplinas.DataBind();

                    rptDisciplinas.Visible = btnSalvar.Visible = rptDisciplinas.Items.Count > 0;
                }

                updCadastro.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
                updMensagem.Update();
            }

        }

        private void Salvar()
        {
            try
            {
                List<ACA_CargaHorariaExtraclasse> lstSalvar = (from RepeaterItem itemDisciplina in rptDisciplinas.Items
                                                               let rptPeriodoCalendario = itemDisciplina.FindControl("rptPeriodoCalendario") as Repeater
                                                               let hdnDisId = itemDisciplina.FindControl("hdnDisId") as HiddenField
                                                               where (itemDisciplina.ItemType == ListItemType.Item || itemDisciplina.ItemType == ListItemType.AlternatingItem) &&
                                                                     rptPeriodoCalendario != null && hdnDisId != null
                                                               from RepeaterItem itemPeriodo in rptPeriodoCalendario.Items
                                                               let hdnTpcId = itemPeriodo.FindControl("hdnTpcId") as HiddenField
                                                               let hdnCheId = itemPeriodo.FindControl("hdnCheId") as HiddenField
                                                               let txtCargaHoraria = itemPeriodo.FindControl("txtCargaHoraria") as TextBox
                                                               where hdnTpcId != null && hdnCheId != null
                                                               let tpc_id = Convert.ToInt32(hdnTpcId.Value)
                                                               let che_id = Convert.ToInt32(hdnCheId.Value)
                                                               let dis_id = Convert.ToInt32(hdnDisId.Value)
                                                               select new ACA_CargaHorariaExtraclasse
                                                               {
                                                                   dis_id = dis_id
                                                                   ,
                                                                   cal_id = Cal_id
                                                                   ,
                                                                   tpc_id = tpc_id
                                                                   ,
                                                                   che_id = che_id
                                                                   ,
                                                                   che_cargaHoraria = txtCargaHoraria != null && !string.IsNullOrEmpty(txtCargaHoraria.Text) ?
                                                                                            Convert.ToInt32(txtCargaHoraria.Text) : 0
                                                                   ,
                                                                   IsNew = che_id <= 0
                                                               }).ToList();

                if (ACA_CargaHorariaExtraclasseBO.Salvar(lstSalvar))
                {
                    ApplicationWEB._GravaLogSistema(lstSalvar.Any(p => p.che_id > 0) ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, string.Format("Cadastro carga horária de atividade extraclasse | cur_id: {0}, crr_id: {1}, crp_id: {2}, cal_id: {3}", Cur_id, Crr_id, Crp_id, Cal_id));
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Carga horária de atividades extraclasse salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    RedirecionarPagina("Cadastro.aspx");
                }
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                updMensagem.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a carga horária de atividades extraclasse.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
                updMensagem.Update();
            }
        }

        #endregion

        #region Delegates

        private void UCCCursoCurriculo_IndexChanged()
        {
            UCCCalendario.Valor = -1;
            UCCCalendario.PermiteEditar = false;
            UCCCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
            UCCCurriculoPeriodo.PermiteEditar = true;
            rptDisciplinas.Visible = btnSalvar.Visible = false;

            if (Cur_id > 0 && Crr_id > 0)
            {
                UCCCalendario.CarregarPorCurso(Cur_id);
                UCCCalendario.PermiteEditar = true;

                UCCCurriculoPeriodo.CarregarPorCursoCurriculo(Cur_id, Crr_id);
                UCCCurriculoPeriodo.PermiteEditar = true;
            }

            updFiltros.Update();
        }

        private void UCCCurriculoPeriodo_IndexChanged()
        {
            CarregarTela();
        }

        private void UCCCalendario_IndexChanged()
        {
            CarregarTela();
        }

        #endregion

        #region Eventos

        protected void rptDisciplinas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header ||
                e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Item)
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    HtmlTableCell thCargaHoraria = e.Item.FindControl("thCargaHoraria") as HtmlTableCell;
                    if (thCargaHoraria != null && LstTipoPeriodoCalendario != null)
                    {
                        thCargaHoraria.ColSpan = LstTipoPeriodoCalendario.Count;
                    }
                }

                Repeater rptPeriodoCalendario = e.Item.FindControl("rptPeriodoCalendario") as Repeater;
                if (rptPeriodoCalendario != null && LstTipoPeriodoCalendario != null)
                {
                    rptPeriodoCalendario.DataSource = LstTipoPeriodoCalendario;
                    rptPeriodoCalendario.DataBind();
                }
            }
        }


        protected void rptPeriodoCalendario_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int dis_id = -1;
                int tpc_id = -1;
                Repeater rptPeriodoCalendario = e.Item.NamingContainer as Repeater;
                if (rptPeriodoCalendario != null)
                {
                    RepeaterItem rptItemDisciplina = rptPeriodoCalendario.NamingContainer as RepeaterItem;
                    if (rptItemDisciplina != null)
                    {
                        HiddenField hdnDisId = rptItemDisciplina.FindControl("hdnDisId") as HiddenField;
                        if (hdnDisId != null)
                        {
                            int.TryParse(hdnDisId.Value, out dis_id);
                        }
                    }
                }

                HiddenField hdnTpcId = e.Item.FindControl("hdnTpcId") as HiddenField;
                if (hdnTpcId != null)
                {
                    int.TryParse(hdnTpcId.Value, out tpc_id);
                }

                ACA_CargaHorariaExtraclasse entity = LstCargaHoraria.Find(p => p.dis_id == dis_id && p.tpc_id == tpc_id);

                if (entity != null && entity.che_id > 0)
                {
                    HiddenField hdnCheId = e.Item.FindControl("hdnCheId") as HiddenField;
                    if (hdnCheId != null)
                    {
                        hdnCheId.Value = entity.che_id.ToString();
                    }

                    TextBox txtCargaHoraria = e.Item.FindControl("txtCargaHoraria") as TextBox;
                    if (txtCargaHoraria != null)
                    {
                        txtCargaHoraria.Text = entity.che_cargaHoraria > 0 ?
                            Convert.ToInt32(entity.che_cargaHoraria).ToString() : string.Empty;
                    }
                }
            }
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("Cadastro.aspx");
        }

        #endregion
    }
}