using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboDisciplina : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();

        public SelectedIndexChanged OnSelectedIndexChanged;

        #endregion Delegates

        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// Campo dis_id.
        /// </summary>
        public int Valor
        {
            get
            {
                if (string.IsNullOrEmpty(ddlCombo.SelectedValue))
                    return -1;

                return Convert.ToInt32(ddlCombo.SelectedValue);
            }
            set
            {
                ddlCombo.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Propriedade que seta a label e a validação do combo
        /// </summary>
        public bool Obrigatorio
        {
            set
            {
                if (value)
                {
                    AdicionaAsteriscoObrigatorio(lblTitulo);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblTitulo);
                }
                cpvCombo.Visible = value;
            }
        }

        /// <summary>
        /// Seta o validationGroup do combo.
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                cpvCombo.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Deixa o combo habilitado de acordo com o valor passado
        /// </summary>
        public bool PermiteEditar
        {
            set
            {
                ddlCombo.Enabled = value;
            }
        }

        /// <summary>
        /// Texto do título ao combo.
        /// </summary>
        public string Texto
        {
            set
            {
                lblTitulo.Text = value;
                cpvCombo.ErrorMessage = value.EndsWith("*") ? value.Remove(value.Length - 2) + " é obrigatório." : value + " é obrigatório.";
            }
            get
            {
                return ddlCombo.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// Coloca na primeira linha a mensagem de selecione um item.
        /// </summary>
        public bool MostrarMensagemSelecione
        {
            set
            {
                if (value)
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " --", "-1", true));
            }
        }

        /// <summary>
        /// Atribui valores para o combo
        /// </summary>
        public DropDownList _Combo
        {
            get
            {
                return ddlCombo;
            }
            set
            {
                ddlCombo = value;
            }
        }


        /// <summary>
        /// Atribui valor para o skin do combo
        /// </summary>
        public string SkinIDCombo
        {
            set
            {
                ddlCombo.SkinID = value;
            }
        }

        /// <summary>
        /// Retorna o título do combo (nome do CurriculoPeriodo no sistema).
        /// </summary>
        public string Titulo
        {
            get
            {
                return lblTitulo.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
            }
        }

        /// <summary>
        /// Propriedade visible da label do nome do combo
        /// </summary>
        public bool LabelVisible
        {
            set
            {
                lblTitulo.Visible = value;
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Seta o foco no combo
        /// </summary>
        public void SetarFoco()
        {
            ddlCombo.Focus();
        }

        /// <summary>
        /// Grava a exception no log e mostra mensagem de erro no label.
        /// </summary>
        /// <param name="ex"></param>
        private void TrataErro(Exception ex)
        {
            // Grava o erro e mostr pro usuário.
            ApplicationWEB._GravaErro(ex.InnerException);

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }

        /// <summary>
        /// Carrega as disciplinas que sejam do tipo Eletiva do aluno. Somente disciplinas Ativas.
        /// </summary>
        public void CarregarDisciplinasEletivasAluno(int cur_id, int crr_id)
        {
            try
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = ACA_DisciplinaBO.SelecionaPor_Tipo_Curso(cur_id, crr_id, ACA_DisciplinaBO.ACA_DisciplinaSituacao.Ativo);
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " --", "-1", true));
                ddlCombo.AppendDataBoundItems = true;
                ddlCombo.DataBind();
            }
            catch (Exception ex)
            {
                TrataErro(ex);
            }
        }

        /// <summary>
        /// Carrega as disciplinas que sejam do tipo Eletiva do aluno. Somente disciplinas Ativas e que tenha
        /// períodos compatíveis com a escola.
        /// </summary>
        public void CarregarDisciplinasEletivasAlunoPeriodo(int cur_id, int crr_id, int esc_id, int uni_id)
        {
            try
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = ACA_DisciplinaBO.SelecionaPor_Tipo_CursoPeriodo(cur_id, crr_id, ACA_DisciplinaBO.ACA_DisciplinaSituacao.Ativo, esc_id, uni_id);
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " --", "-1", true));
                ddlCombo.AppendDataBoundItems = true;
                ddlCombo.DataBind();
            }
            catch (Exception ex)
            {
                TrataErro(ex);
            }
        }

        /// <summary>
        /// Carrega as disciplinas que sejam do tipo Multisseriadas ( Enumerador de tipo de diciplina). 
        /// Somente disciplinas Ativas e que tenham períodos compatíveis com a escola.
        /// </summary>
        public void CarregarDisciplinasMultisseriadas(int cur_id, int crr_id, int crp_id, int esc_id, int uni_id)
        {
            try
            {
                byte crd_tipo = (byte)ACA_CurriculoDisciplinaTipo.DisciplinaMultisseriada;

                CarregarDisciplinasMultisseriadas(cur_id, crr_id, crp_id, esc_id, uni_id, crd_tipo);
            }
            catch (Exception ex)
            {
                TrataErro(ex);
            }
        }

        // <summary>
        /// Carrega as disciplinas que sejam do tipo Multisseriadas ( Enumerador de tipo de diciplina). 
        /// Somente disciplinas Ativas e que tenham períodos compatíveis com a escola.
        /// </summary>
        public void CarregarDisciplinasMultisseriadas(int cur_id, int crr_id, int crp_id, int esc_id, int uni_id, byte crd_tipo)
        {
            try
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = ACA_DisciplinaBO.SelecionaPor_TipoDisciplinaEnum_CursoPeriodo(cur_id, crr_id, crp_id, (byte)ACA_CurriculoDisciplinaTipo.DisciplinaMultisseriada, ACA_DisciplinaBO.ACA_DisciplinaSituacao.Ativo, esc_id, uni_id);
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " --", "-1", true));
                ddlCombo.AppendDataBoundItems = true;
                ddlCombo.DataBind();
            }
            catch (Exception ex)
            {
                TrataErro(ex);
            }
        }

        /// <summary>
        /// Carrega o combo por tipo de disciplina.
        /// </summary>
        /// <param name="tds_id"></param>
        public void CarregarDisciplinasTipoDisciplina(byte crd_tipo)
        {
            try
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = ACA_DisciplinaBO.SelecionaPorTipoGradeCurricular(crd_tipo);
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
                ddlCombo.AppendDataBoundItems = true;
                ddlCombo.DataBind();
            }
            catch (Exception ex)
            {
                TrataErro(ex);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlCombo.AutoPostBack = (OnSelectedIndexChanged != null);
        }

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedIndexChanged != null)
                OnSelectedIndexChanged();
        }

        #endregion Eventos
    }
}