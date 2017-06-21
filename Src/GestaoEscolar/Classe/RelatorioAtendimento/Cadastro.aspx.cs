namespace GestaoEscolar.Classe.RelatorioAtendimento
{
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;

    public partial class Cadastro : MotherPageLogado
    {
        private long VS_alu_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_alu_id"] ?? -1);
            }

            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        private int VS_cal_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_id"] ?? -1);
            }

            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UCCRelatorioAtendimento.IndexChanged += UCCRelatorioAtendimento_IndexChanged;
            UCCPeriodoCalendario.IndexChanged += UCCPeriodoCalendario_IndexChanged;

            if (!IsPostBack)
            {
                if ((PreviousPage != null) && PreviousPage.IsCrossPagePostBack)
                {
                    VS_alu_id = PreviousPage.EditItemAluId;
                    VS_cal_id = PreviousPage.EditItemCalId;
                }

                UCCRelatorioAtendimento.CarregarPorPermissaoUuarioTipo(CLS_RelatorioAtendimentoTipo.AEE);
                UCCPeriodoCalendario.CarregarPorCalendario(VS_cal_id);
                updFiltros.Update();
            }
        }

        private void CarregarRelatorio()
        {
            pnlLancamento.Visible = btnSalvar.Visible = false;

            if (UCCPeriodoCalendario.Valor[0] > 0 && UCCPeriodoCalendario.Valor[1] > 0 && UCCRelatorioAtendimento.Valor > 0)
            {
                pnlLancamento.GroupingText = UCCRelatorioAtendimento.Texto;
                UCLancamentoRelatorioAtendimento.Carregar(VS_alu_id, UCCRelatorioAtendimento.Valor, false);
                pnlLancamento.Visible = btnSalvar.Visible = true;
                UCCPeriodoCalendario.PermiteEditar = UCCRelatorioAtendimento.PermiteEditar = false; 
            }

            updBotoes.Update();
            updFiltros.Update();
            updLancamento.Update();
        }

        private void UCCPeriodoCalendario_IndexChanged()
        {
            try
            {
                CarregarRelatorio();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
                updMensagem.Update();
            }
        }

        private void UCCRelatorioAtendimento_IndexChanged()
        {
            try
            {
                CarregarRelatorio();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
                updMensagem.Update();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("Busca.aspx");
        }
    }
}