using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.WebControls.Busca
{
    public partial class UCAlunoEducacaoEspecial : MotherUserControl
    {
        protected IDictionary<string, object> returns = new Dictionary<string, object>();

        #region Enumeradores

        /// <summary>
        /// Tipos de busca
        /// </summary>
        public enum eTipoBuscaAluno
        {
            AlunosAtivosSalaRecurso = 1,
            AlunosAtivosClasseEspecial,
            TodosAlunosEducacaoEspecial,
            AlunosComDeficiencia
        }

        #endregion Enumeradores

        #region Propriedades

        /// <summary>
        /// Retorna o tipo de busca
        /// </summary>
        public eTipoBuscaAluno TipoBusca
        {
            get
            {
                if (ViewState["TipoBusca"] != null)
                {
                    return (eTipoBuscaAluno)ViewState["TipoBusca"];
                }

                return eTipoBuscaAluno.AlunosAtivosSalaRecurso;
            }

            set
            {
                ViewState["TipoBusca"] = value;
            }
        }

        #endregion Propriedades

        #region Delegates

        public delegate void OnReturnValues(IDictionary<string, object> parameters);

        public event OnReturnValues ReturnValues;

        #endregion Delegates

        #region Constantes

        private const int INDEX_COLUMN_MATRICULA = 2;
        private const int INDEX_COLUMN_CURSO = 3;
        private const int INDEX_COLUMN_PERIODO = 4;
        private const int INDEX_COLUMN_ESCOLA_RECURSO = 6;
        private const int INDEX_COLUMN_TURMA_RECURSO = 7;

        #endregion Constantes

        #region Eventos Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                grvAluno.PageSize = ApplicationWEB._Paginacao;
                grvAluno.Columns[INDEX_COLUMN_CURSO].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                grvAluno.Columns[INDEX_COLUMN_PERIODO].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        UCComboUAEscola1.Visible = false;
                    }
                }
                else
                {
                    UCComboUAEscola1.Inicializar();
                }

                ConfiguraBusca();
            }

            //atualiza parametro academico TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA
            Atualiza_param_TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA();

            grvAluno.Columns[INDEX_COLUMN_ESCOLA_RECURSO].Visible = grvAluno.Columns[INDEX_COLUMN_TURMA_RECURSO].Visible = false;
            grvAluno.Columns[INDEX_COLUMN_PERIODO].Visible = grvAluno.Columns[INDEX_COLUMN_CURSO].Visible = true;

            switch (TipoBusca)
            {
                case eTipoBuscaAluno.AlunosAtivosSalaRecurso:
                    odsAluno.SelectMethod = "SelecionaAlunosMatriculadosSalaRecurso";
                    grvAluno.Columns[INDEX_COLUMN_ESCOLA_RECURSO].Visible = grvAluno.Columns[INDEX_COLUMN_TURMA_RECURSO].Visible = true;
                    grvAluno.Columns[INDEX_COLUMN_PERIODO].Visible = grvAluno.Columns[INDEX_COLUMN_CURSO].Visible = false;
                    break;
                case eTipoBuscaAluno.AlunosAtivosClasseEspecial:
                    odsAluno.SelectMethod = "SelecionaAlunosMatriculadosClasseEspecial";
                    break;
                case eTipoBuscaAluno.TodosAlunosEducacaoEspecial:
                    odsAluno.SelectMethod = "SelecionaTodosAlunosEducacaoEspecial";
                    grvAluno.Columns[INDEX_COLUMN_ESCOLA_RECURSO].Visible = grvAluno.Columns[INDEX_COLUMN_TURMA_RECURSO].Visible = true;
                    grvAluno.Columns[INDEX_COLUMN_PERIODO].Visible = grvAluno.Columns[INDEX_COLUMN_CURSO].Visible = false;
                    break;
                case eTipoBuscaAluno.AlunosComDeficiencia:
                    odsAluno.SelectMethod = "SelecionaAlunosMatriculadosDeficiencia";
                    break;
            }

            // Seta nomes padrões para os clientes
            grvAluno.Columns[INDEX_COLUMN_MATRICULA].HeaderText = lblMatricula.Text = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            grvAluno.Columns[INDEX_COLUMN_CURSO].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            grvAluno.Columns[INDEX_COLUMN_PERIODO].HeaderText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        #endregion Eventos Page Life Cycle

        #region Eventos

        protected void UCComboUAEscola1_IndexChangedUA()
        {
            try
            {
                if (UCComboUAEscola1.Uad_ID != Guid.Empty)
                {
                    UCComboUAEscola1.CarregaUnidadesEscolaresPorUASuperior(UCComboUAEscola1.Uad_ID);
                }

                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };
                UCComboUAEscola1.EnableEscolas = UCComboUAEscola1.Uad_ID != Guid.Empty;
                UCComboUAEscola1.FocoEscolas = UCComboUAEscola1.Uad_ID != Guid.Empty;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a(s) unidade(s) administrativa(s).", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnPesquisa_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    grvAluno.PageIndex = 0;

                    if (TipoBusca == eTipoBuscaAluno.AlunosComDeficiencia)
                    {
                        // Verifica os filtros, obrigando a informar pelo menos um filtro.
                        if (UCComboUAEscola1.Uad_ID == Guid.Empty
                            && UCComboUAEscola1.Esc_ID <= 0
                            && string.IsNullOrEmpty(txtNome.Text.Trim())
                            && string.IsNullOrEmpty(txtDataNascimento.Text.Trim())
                            && string.IsNullOrEmpty(txtMae.Text.Trim())
                            && string.IsNullOrEmpty(txtMatricula.Text.Trim()))
                        {
                            throw new ValidationException("É necessário informar pelo menos um filtro para pesquisar.");
                        }
                    }

                    odsAluno.SelectParameters.Clear();
                    odsAluno.SelectParameters.Add("uad_idSuperior", UCComboUAEscola1.Uad_ID.ToString());
                    odsAluno.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                    odsAluno.SelectParameters.Add("esc_id", UCComboUAEscola1.Esc_ID.ToString());
                    odsAluno.SelectParameters.Add("uni_id", UCComboUAEscola1.Uni_ID.ToString());
                    odsAluno.SelectParameters.Add("nome_aluno", txtNome.Text);
                    odsAluno.SelectParameters.Add("tipoBusca", rblEscolhaBusca.SelectedValue);
                    odsAluno.SelectParameters.Add("pes_dataNascimento", txtDataNascimento.Text);
                    odsAluno.SelectParameters.Add("pes_nomeMae", txtMae.Text);
                    odsAluno.SelectParameters.Add("alc_matricula", txtMatricula.Text);
                    odsAluno.SelectParameters.Add("matriculaEstadual", ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString());

                    if (TipoBusca.Equals(eTipoBuscaAluno.AlunosAtivosSalaRecurso)
                        || TipoBusca.Equals(eTipoBuscaAluno.AlunosAtivosClasseEspecial)
                        || TipoBusca.Equals(eTipoBuscaAluno.TodosAlunosEducacaoEspecial))
                    {
                        odsAluno.SelectParameters.Add("doc_id", __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual ? __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString() : string.Empty);
                        odsAluno.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
                        odsAluno.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                        odsAluno.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                    }

                    // quantidade de itens por página
                    string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                    int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                    // atribui essa quantidade para o grid
                    grvAluno.PageSize = itensPagina;
                    // atualiza o grid
                    grvAluno.Sort(string.Empty, SortDirection.Ascending);
                    grvAluno.DataBind();

                    fdsResultado.Visible = true;
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAluno_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                returns.Add(grvAluno.DataKeyNames[0], grvAluno.DataKeys[e.NewSelectedIndex].Values["alu_id"]);
                returns.Add(grvAluno.DataKeyNames[1], grvAluno.DataKeys[e.NewSelectedIndex].Values["pes_nome"]);
                returns.Add(grvAluno.DataKeyNames[2], grvAluno.DataKeys[e.NewSelectedIndex].Values["msr_id"]);
                returns.Add(grvAluno.DataKeyNames[3], grvAluno.DataKeys[e.NewSelectedIndex].Values["mtu_id"]);
                returns.Add(grvAluno.DataKeyNames[4], grvAluno.DataKeys[e.NewSelectedIndex].Values["alc_matricula"]);
                returns.Add(grvAluno.DataKeyNames[5], grvAluno.DataKeys[e.NewSelectedIndex].Values["esc_codigo"]);
                returns.Add(grvAluno.DataKeyNames[6], grvAluno.DataKeys[e.NewSelectedIndex].Values["esc_nome"]);
                returns.Add(grvAluno.DataKeyNames[7], grvAluno.DataKeys[e.NewSelectedIndex].Values["cur_nome"]);
                if (TipoBusca == eTipoBuscaAluno.AlunosAtivosClasseEspecial || TipoBusca == eTipoBuscaAluno.AlunosComDeficiencia)
                {
                    returns.Add(grvAluno.DataKeyNames[8], grvAluno.DataKeys[e.NewSelectedIndex].Values["crp_descricao"]);
                    returns.Add(grvAluno.DataKeyNames[9], grvAluno.DataKeys[e.NewSelectedIndex].Values["tur_codigo"]);
                }
                else
                {
                    returns.Add(grvAluno.DataKeyNames[8], grvAluno.DataKeys[e.NewSelectedIndex].Values["tsr_esc_nome"]);
                    returns.Add(grvAluno.DataKeyNames[9], grvAluno.DataKeys[e.NewSelectedIndex].Values["tsr_turma"]);
                }

                if (ReturnValues != null)
                {
                    ReturnValues(returns);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar selecionar o aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAluno_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_AlunoBO.GetTotalRecords();
            ConfiguraColunasOrdenacao(grvAluno);
        }

        protected void odsAluno_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
            {
                e.InputParameters.Clear();
            }
        }

        #endregion Eventos

        #region Métodos

        ///// <summary>
        ///// Atualiza parametro academico TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA
        ///// </summary>
        private void Atualiza_param_TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA()
        {
            grvAluno.Columns[6].HeaderText = "Escola da " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            grvAluno.Columns[7].HeaderText = "Turma da " + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EDUCACAO_ESPECIAL_ALUNO_DEFICIENCIA
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
        }

        /// <summary>
        /// Limpa os campos da pesquisa
        /// </summary>
        public void Limpar()
        {
            UCComboUAEscola1.Inicializar();
            if (UCComboUAEscola1.Esc_ID > 0 && UCComboUAEscola1.QuantidadeItemsComboEscolas > 2)
            {
                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };
            }

            rblEscolhaBusca.SelectedIndex = 0;
            txtNome.Text = string.Empty;
            txtDataNascimento.Text = string.Empty;
            txtMae.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            fdsResultado.Visible = false;
            btnPesquisa.Focus();
        }

        /// <summary>
        /// Configura campos para busca
        /// </summary>
        private void ConfiguraBusca()
        {

                lblNome.Text = "Nome do aluno";
                divNomeAluno.Visible = true;


                lblMae.Text = "Nome da mãe";
                lblMae.Visible = true;
                txtMae.Visible = true;

                
                lblDataNascimento.Text = "Data de nascimento";
                lblDataNascimento.Visible = true;
                txtDataNascimento.Visible = true;
                cvDataNascimento.Visible = true;
        }

        #endregion Métodos
    }
}