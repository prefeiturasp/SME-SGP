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
    public partial class Cadastro : MotherPageLogado
    {
        private List<sTipoPeriodoCalendario> LstTipoPeriodoCalendario;

        protected void Page_Load(object sender, EventArgs e)
        {
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;
            UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;
            UCCCurriculoPeriodo.IndexChanged += UCCCurriculoPeriodo_IndexChanged;

            try
            {
                if (!IsPostBack)
                {
                    UCCCursoCurriculo.CarregarPorModalidadeEnsino(ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_MODALIDADE_CIEJA, Ent_ID_UsuarioLogado));
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void CarregarTela()
        {
            pnlCadastro.Visible = false;

            if (UCCCalendario.Valor > 0 && UCCCursoCurriculo.Valor != new[] { -1, -1 } && UCCCurriculoPeriodo.Valor != new[] { -1, -1, -1 })
            {
                LstTipoPeriodoCalendario = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(UCCCalendario.Valor, ApplicationWEB.AppMinutosCacheLongo);

                rptDisciplinas.DataSource = ACA_CurriculoDisciplinaBO.SelecionaDisciplinasParaFormacaoTurmaNormal(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1], UCCCurriculoPeriodo.Valor[2]);
                rptDisciplinas.DataBind();

                pnlCadastro.Visible = true;
            }

            updCadastro.Update();
        }

        private void UCCCursoCurriculo_IndexChanged()
        {
            UCCCalendario.Valor = -1;
            UCCCalendario.PermiteEditar = false;
            UCCCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
            UCCCurriculoPeriodo.PermiteEditar = true;
            pnlCadastro.Visible = false;

            if (UCCCursoCurriculo.Valor[0] > 0 && UCCCursoCurriculo.Valor[1] > 0)
            {
                UCCCalendario.CarregarPorCurso(UCCCursoCurriculo.Valor[0]);
                UCCCalendario.PermiteEditar = true;

                UCCCurriculoPeriodo.CarregarPorCursoCurriculo(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);
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

        protected void rptDisciplinas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header ||
                e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Item)
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    HtmlTableCell thCargaHoraria = e.Item.FindControl("thCargaHoraria") as HtmlTableCell;
                    if (thCargaHoraria != null)
                    {
                        thCargaHoraria.ColSpan = LstTipoPeriodoCalendario.Count;
                    }
                }

                Repeater rptPeriodoCalendario = e.Item.FindControl("rptPeriodoCalendario") as Repeater;
                rptPeriodoCalendario.DataSource = LstTipoPeriodoCalendario;
                rptPeriodoCalendario.DataBind();
            }
        }
    }
}