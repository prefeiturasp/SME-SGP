namespace GestaoEscolar.Classe.LancamentoFrequenciaExterna
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
        #region Delegates

        private void UCFiltroEscolas__Selecionar()
        {
            try
            {
                if (UCFiltroEscolas._VS_FiltroEscola)
                    UCFiltroEscolas._UnidadeEscola_LoadBy_uad_idSuperior(UCFiltroEscolas._UCComboUnidadeAdministrativa_Uad_ID, false);

                UCFiltroEscolas._ComboUnidadeEscola.Enabled = UCFiltroEscolas._UCComboUnidadeAdministrativa_Uad_ID != Guid.Empty;

                if (UCFiltroEscolas._ComboUnidadeEscola.Enabled)
                    UCFiltroEscolas._ComboUnidadeEscola.Focus();

                updPesquisa.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        private void UCFiltroEscolas__SelecionarEscola()
        {
            try
            {
                UCCCalendario.Valor = -1;
                UCCCalendario.PermiteEditar = false;
                UCCTurma.Valor = new long[] { -1, -1, -1 };
                UCCTurma.PermiteEditar = false;

                if (UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID > 0 && UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID > 0)
                {
                    UCCCalendario.PermiteEditar = true;
                    UCCCalendario.SetarFoco();
                    UCCCalendario.CarregarPorEscola(UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID);
                }

                updPesquisa.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        private void UCCCalendario_IndexChanged()
        {
            try
            {
                UCCTurma.Valor = new long[] { -1, -1, -1 };
                UCCTurma.PermiteEditar = false;

                if (UCCCalendario.Valor > 0)
                {
                    UCCTurma.PermiteEditar = true;
                    UCCTurma.SetarFoco();
                    UCCTurma.CarregarPorEscolaCalendarioSituacao_TurmasNormais(UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID, UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID, UCCCalendario.Valor, TUR_TurmaSituacao.Ativo);
                }

                updPesquisa.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        #endregion Delegates

        #region Page Life Cicle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UCFiltroEscolas._Selecionar += UCFiltroEscolas__Selecionar;
                UCFiltroEscolas._SelecionarEscola += UCFiltroEscolas__SelecionarEscola;
                UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;

                if (!IsPostBack)
                {
                    InicializarTela();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Page Life Cicle

        #region Métodos

        private void InicializarTela()
        {
            UCCCalendario.Valor = -1;
            UCCCalendario.PermiteEditar = false;
            UCCTurma.Valor = new long[] { -1, -1, -1 };
            UCCTurma.PermiteEditar = false;
            UCFiltroEscolas._LoadInicial();
            updPesquisa.Update();
        }

        #endregion Métodos
    }
}