namespace GestaoEscolar.Relatorios.GraficoAtendimento
{
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class Busca : MotherPageLogado
    {
        public int gra_id
        {
            get
            {
                return UCCGraficoAtendimento.Valor;
            }
        }

        public string gra_titulo
        {
            get
            {
                return UCCGraficoAtendimento.Texto;
            }
        }

        public string uadSuperior_nome
        {
            get
            {
                return UCCUAEscola.Uad_ID != new Guid() ?
                    UCCUAEscola.TextoComboUA : string.Empty;
            }
        }

        public int esc_id
        {
            get
            {
                return UCCUAEscola.Esc_ID;
            }
        }

        public int uni_id
        {
            get
            {
                return UCCUAEscola.Uni_ID;
            }
        }

        public string esc_nome
        {
            get
            {
                return UCCUAEscola.TextoComboEscola;
            }
        }

        public int cur_id
        {
            get
            {
                return UCCCursoCurriculo.Valor[0];
            }
        }

        public int crr_id
        {
            get
            {
                return UCCCursoCurriculo.Valor[1];
            }
        }

        public string cur_nome
        {
            get
            {
                return UCCCursoCurriculo.Valor[0] > 0 && UCCCursoCurriculo.Valor[1] > 0 ?
                    UCCCursoCurriculo.Texto : string.Empty;
            }
        }

        public int crp_id
        {
            get
            {
                return UCCCurriculoPeriodo.Valor[2];
            }
        }

        public string crp_descricao
        {
            get
            {
                return UCCCurriculoPeriodo.Valor[0] > 0 && UCCCurriculoPeriodo.Valor[1] > 0 && UCCCurriculoPeriodo.Valor[2] > 0 ?
                    UCCCurriculoPeriodo.Texto : string.Empty;
            }
        }

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
                    VerificaBusca();
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
            UCCGraficoAtendimento.PermiteEditar = false;
            UCCCursoCurriculo.PermiteEditar = false;
            UCCCurriculoPeriodo.PermiteEditar = false;
        }

        public void SalvarBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();

            filtros.Add("rae_tipo", UCCTipoRelatorioAtendimento.Valor.ToString());
            filtros.Add("gra_id", UCCGraficoAtendimento.Valor.ToString());
            filtros.Add("uad_idSuperior", UCCUAEscola.Uad_ID.ToString());
            filtros.Add("esc_id", UCCUAEscola.Esc_ID.ToString());
            filtros.Add("uni_id", UCCUAEscola.Uni_ID.ToString());
            filtros.Add("cur_id", UCCCursoCurriculo.Valor[0].ToString());
            filtros.Add("crr_id", UCCCursoCurriculo.Valor[1].ToString());
            filtros.Add("crp_id", UCCCurriculoPeriodo.Valor[2].ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.GraficoAtendimento, Filtros = filtros };
        }

        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.GraficoAtendimento)
            {
                string valor, valor2;

                if(__SessionWEB.BuscaRealizada.Filtros.TryGetValue("rae_tipo", out valor))
                {
                    UCCTipoRelatorioAtendimento.Valor = Convert.ToByte(valor);
                    UCCTipoRelatorioAtendimento_IndexChanged();
                }

                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("gra_id", out valor))
                {
                    UCCGraficoAtendimento.Valor = Convert.ToInt32(valor);
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                if (!string.IsNullOrEmpty(valor) && UCCUAEscola.FiltroEscola)
                {
                    UCCUAEscola.Uad_ID = new Guid(valor);
                    UCCUAEscola.CarregaEscolaPorUASuperiorSelecionada();

                    if (UCCUAEscola.Uad_ID != Guid.Empty)
                    {
                        UCCUAEscola.FocoEscolas = true;
                        UCCUAEscola.PermiteAlterarCombos = true;
                    }
                }

                if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out valor)) &&
                    (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out valor2)))
                {
                    UCCUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                    UCCUAEscola_IndexChangedUnidadeEscola();
                }

                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor2) &&
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor))
                {
                    UCCCursoCurriculo.Valor = new[] { Convert.ToInt32(valor2), Convert.ToInt32(valor) };
                    UCCCursoCurriculo_IndexChanged();
                }

                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor))
                {
                    UCCCurriculoPeriodo.Valor = new[] { UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1], Convert.ToInt32(valor) };
                }
            }
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

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            RedirecionarPagina("Busca.aspx");
        }
    }
}