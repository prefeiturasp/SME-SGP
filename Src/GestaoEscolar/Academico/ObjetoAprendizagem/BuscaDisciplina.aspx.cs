using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;

namespace GestaoEscolar.Academico.ObjetoAprendizagem
{
    public partial class BuscaDisciplina : MotherPageLogado
    {
        #region PROPRIEDADES

        public int tds_id
        {
            get
            {
                return Convert.ToInt32(_grvTipoDisciplina.DataKeys[_grvTipoDisciplina.EditIndex].Value);
            }
        }

        #endregion

        #region EVENTOS

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string message = __SessionWEB.PostMessages;
                if (!string.IsNullOrEmpty(message))
                    _lblMessage.Text = message;

                if (!IsPostBack)
                {
                    UCComboTipoNivelEnsino1.MostrarMessageSelecione = true;
                    UCComboTipoNivelEnsino1.CarregarTipoNivelEnsino();
                    fdsResultados.Visible = false;

                    VerificaBusca();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void _btnPesquisar_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }

        protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ObjetoAprendizagem/BuscaDisciplina.aspx", false);
        }

        protected void _grvTipoDisciplina_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = _grvTipoDisciplina.Rows.Count;
        }

        protected void _grvTipoDisciplina_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            _grvTipoDisciplina.EditIndex = e.NewEditIndex;
        }

        #endregion

        #region MÉTODOS

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ObjetoAprendizagem)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("base", out valor);
                _ddlBase.SelectedValue = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tne_id", out valor);
                UCComboTipoNivelEnsino1.Valor = Convert.ToInt32(valor);

                LoadGridView();
            }
            else
            {
                fdsResultados.Visible = false;
            }
        }

        private void LoadGridView()
        {
            try
            {
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                Dictionary<string, string> filtros = new Dictionary<string, string>();

                filtros.Add("base", _ddlBase.SelectedValue);
                filtros.Add("tne_id", UCComboTipoNivelEnsino1.Valor.ToString());

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.ObjetoAprendizagem
                    ,
                    Filtros = filtros
                };

                _grvTipoDisciplina.PageSize = itensPagina;
                _grvTipoDisciplina.DataSource = ACA_TipoDisciplinaBO.SelecionaTipoDisciplinaPorNivelEnsinoBase(UCComboTipoNivelEnsino1.Valor, Convert.ToInt32(_ddlBase.SelectedValue), __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                _grvTipoDisciplina.DataBind();

                fdsResultados.Visible = true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao carregar {0}.", GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN")), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}