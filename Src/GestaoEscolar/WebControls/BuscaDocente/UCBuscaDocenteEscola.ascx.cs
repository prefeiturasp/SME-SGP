using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.BuscaDocente
{
    public partial class UCBuscaDocenteEscola : MotherUserControl
    {
        protected IDictionary<string, object> returns = new Dictionary<string, object>();

        #region Constantes

        private const int grvDocenteColunaEscola = 2;

        #endregion Constantes

        #region Propriedades

        public int IdEscola
        {
            get
            {
                return Convert.ToInt32(hdnEscola.Value);
            }
            set
            {
                hdnEscola.Value = value.ToString();
                if (value > 0 && divDisciplina.Visible)
                {
                    CarregaComboDisciplinas();
                    Limpar();
                }
            }
        }

        /// <summary>
        /// Propriedade que seta a visibilidade do campo RG
        /// </summary>
        public bool VisibleRG
        {
            set
            {
                divRG.Visible = value;
            }
            get
            {
                return divRG.Visible;
            }
        }

        /// <summary>
        /// Propriedade que seta a visibilidade do campo disciplina
        /// </summary>
        public bool VisibleDisciplina
        {
            set
            {
                divDisciplina.Visible = value;
            }
        }

        /// <summary>
        /// Seta a visibilidade da coluna de escolas.
        /// </summary>
        public bool VisibleColunaEscola
        {
            get
            {
                return Convert.ToBoolean(ViewState["VisibleColunaEscola"] ?? "false");
            }

            set
            {
                ViewState["VisibleColunaEscola"] = value;
            }
        }

        #endregion Propriedades

        #region Delegates

        public delegate void OnReturnValues(IDictionary<string, object> parameters);
        public event OnReturnValues ReturnValues;

        #endregion Delegates

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            UCComboQtdePaginacao1.GridViewRelacionado = grvDocente;
            if (!IsPostBack)
            {
                grvDocente.PageSize = ApplicationWEB._Paginacao;
            }
        }

        protected void btnPesquisa_Click(object sender, EventArgs e)
        {
            grvDocente.PageIndex = 0;
            grvDocente.Columns[grvDocenteColunaEscola].Visible = VisibleColunaEscola;
            PesquisarDocentes();
        }

        private void PesquisarDocentes()
        {
            if (VisibleRG)
                PesquisarDocentes_PorEscolaRede();
            else
                Pesquisar();
        }

        protected void grvDocente_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                returns.Add(grvDocente.DataKeyNames[0], grvDocente.DataKeys[e.NewSelectedIndex].Values["doc_id"]);
                returns.Add(grvDocente.DataKeyNames[1], grvDocente.DataKeys[e.NewSelectedIndex].Values["pes_nome"]);
                returns.Add(grvDocente.DataKeyNames[2], grvDocente.DataKeys[e.NewSelectedIndex].Values["col_id"]);
                returns.Add(grvDocente.DataKeyNames[3], grvDocente.DataKeys[e.NewSelectedIndex].Values["crg_id"]);
                returns.Add(grvDocente.DataKeyNames[4], grvDocente.DataKeys[e.NewSelectedIndex].Values["coc_id"]);

                if (ReturnValues != null)
                    ReturnValues(returns);
                else
                    throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar selecionar o docente.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvDocente_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_DocenteBO.GetTotalRecords();
            ConfiguraColunasOrdenacao(grvDocente);
        }

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvDocente.PageSize = UCComboQtdePaginacao1.Valor;
            //grvDocente.PageIndex = 0;
            // atualiza o grid
            grvDocente.DataBind();
        }

        protected void grvDocente_PageIndexChanged(object source, GridViewPageEventArgs e)
        {
            try
            {
                grvDocente.PageIndex = e.NewPageIndex;
                PesquisarDocentes();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os docentes.", UtilBO.TipoMensagem.Erro);
            }
        }
        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Carrega as disciplinas da escola.
        /// </summary>
        private void CarregaComboDisciplinas()
        {
            ddlDisciplina.Items.Clear();
            ddlDisciplina.DataTextField = "tds_nome";
            ddlDisciplina.DataValueField = "tds_id";
            ddlDisciplina.DataSource = ACA_TipoDisciplinaBO.SelecionaTipoDisciplinaPorEscola(IdEscola);
            ddlDisciplina.Items.Insert(0, new ListItem(GetGlobalResourceObject("WebControls", "BuscaDocente.UCBuscaDocenteEscola.ddlDisciplina.Selecione").ToString(), "-1", true));
            ddlDisciplina.AppendDataBoundItems = true;
            ddlDisciplina.DataBind();
        }

        /// <summary>
        /// Limpa os campos.
        /// </summary>
        public void Limpar()
        {
            txtNome.Text = "";
            txtMatricula.Text = "";
            txtCPF.Text = "";
            if (ddlDisciplina.Items.Count > 0)
            {
                ddlDisciplina.SelectedIndex = 0;
            }
            fdsResultado.Visible = false;
            UCTotalRegistros1.Total = 0;
            txtRG.Text = "";
        }

        /// <summary>
        /// Pesquisa os docentes
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);
                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvDocente.PageSize = itensPagina;
                // atualiza o grid
                int tdsId = -1;
                if (ddlDisciplina.Items.Count > 0 && ddlDisciplina.SelectedIndex > 0)
                {
                    tdsId = Convert.ToInt32(ddlDisciplina.SelectedValue);
                }
                //grvDocente.PageIndex = 0;
                //odsDocente.SelectParameters.Clear();
                //odsDocente.SelectMethod = "SelectBy_PesquisaEscola";
                //odsDocente.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Docente";
                //odsDocente.TypeName = "MSTech.GestaoEscolar.BLL.ACA_DocenteBO";

                //odsDocente.SelectParameters.Add("escId", IdEscola.ToString());
                //odsDocente.SelectParameters.Add("nome", txtNome.Text.Trim());
                //odsDocente.SelectParameters.Add("matricula", txtMatricula.Text.Trim());
                //odsDocente.SelectParameters.Add("cpf", txtCPF.Text.Trim());
                //odsDocente.SelectParameters.Add("tdsId", tdsId.ToString());

                grvDocente.DataSource = ACA_DocenteBO.SelectBy_PesquisaEscola(IdEscola, txtNome.Text, txtMatricula.Text, txtCPF.Text, tdsId);
                grvDocente.DataBind();
                fdsResultado.Visible = true;
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os docentes.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Pesquisa os docentes da Escola ou da Rede (quando o flag evento padrão do cadastro de evento for marcado).
        /// </summary>
        private void PesquisarDocentes_PorEscolaRede()
        {
            try
            {
                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);
                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvDocente.PageSize = itensPagina;

                //odsDocente.SelectMethod = "SelectBy_PesquisaEscolaRede";
                //odsDocente.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Docente";
                //odsDocente.TypeName = "MSTech.GestaoEscolar.BLL.ACA_DocenteBO";
                //odsDocente.SelectParameters.Clear();
                //odsDocente.SelectParameters.Add("escId", IdEscola.ToString());
                //odsDocente.SelectParameters.Add("nome", txtNome.Text.Trim());
                //odsDocente.SelectParameters.Add("matricula", txtMatricula.Text.Trim());
                //odsDocente.SelectParameters.Add("cpf", txtCPF.Text.Trim());
                //odsDocente.SelectParameters.Add("rg", txtRG.Text.Trim());

                grvDocente.DataSource = ACA_DocenteBO.SelectBy_PesquisaEscolaRede(IdEscola, txtNome.Text, txtMatricula.Text, txtCPF.Text, txtRG.Text, __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                                    __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao);
                grvDocente.DataBind();
                fdsResultado.Visible = true;
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os docentes.", UtilBO.TipoMensagem.Erro);
            }
        }
        #endregion Métodos
    }
}