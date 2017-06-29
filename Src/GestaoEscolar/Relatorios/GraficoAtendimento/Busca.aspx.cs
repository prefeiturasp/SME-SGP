namespace GestaoEscolar.Relatorios.GraficoAtendimento
{
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class Busca : MotherPageLogado
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UCCTipoRelatorioAtendimento.IndexChanged += UCCTipoRelatorioAtendimento_IndexChanged;
            UCCUAEscola.IndexChangedUA += UCCUAEscola_IndexChangedUA;
            UCCUAEscola.IndexChangedUnidadeEscola += UCCUAEscola_IndexChangedUnidadeEscola;
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
            }

            if (!IsPostBack)
            {
                try
                {
                    InicializarTela();
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                    ApplicationWEB._GravaErro(ex);
                }
            }
        }

      
        private void InicializarTela()
        {
            UCCUAEscola.Inicializar();
            UCCTipoRelatorioAtendimento.CarregarTipos();
        }

        private void UCCCursoCurriculo_IndexChanged()
        {
            try
            {
                UCCCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                UCCCurriculoPeriodo.PermiteEditar = false;

                if (UCCCursoCurriculo.Valor[0] > 0)
                {
                    UCCCurriculoPeriodo.CarregarPorCursoCurriculo(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);
                    UCCCurriculoPeriodo.PermiteEditar = true;
                    UCCCurriculoPeriodo.Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCCUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                UCCCursoCurriculo.Valor = new[] { -1, -1 };
                UCCCursoCurriculo.PermiteEditar = false;

                if (UCCUAEscola.Esc_ID > 0 && UCCUAEscola.Uni_ID > 0)
                {
                    UCCCursoCurriculo.CarregarVigentesPorEscola(UCCUAEscola.Esc_ID, UCCUAEscola.Uni_ID);
                    UCCCursoCurriculo.SetarFoco();
                    UCCCursoCurriculo.PermiteEditar = true;
                }

                UCCCursoCurriculo_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCCUAEscola_IndexChangedUA()
        {
            try
            {

                if (UCCUAEscola.Uad_ID == Guid.Empty)
                    UCCUAEscola.SelectedValueEscolas = new[] { -1, -1 };

                UCCUAEscola_IndexChangedUnidadeEscola();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCCTipoRelatorioAtendimento_IndexChanged()
        {
            try
            {
                UCCGraficoAtendimento.Valor = -1;
                UCCGraficoAtendimento.PermiteEditar = false;

                if (UCCTipoRelatorioAtendimento.Valor > 0)
                {
                    UCCGraficoAtendimento.CarregarPorTipo(UCCTipoRelatorioAtendimento.Valor);
                    UCCGraficoAtendimento.PermiteEditar = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }


        protected void btnGerar_Click(object sender, EventArgs e)
        {

        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {

        }
    }
}