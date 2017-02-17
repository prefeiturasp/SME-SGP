using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos.Novos
{
    public partial class UCCCurriculoPeriodo : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();

        public event SelectedIndexChanged IndexChanged;

        public delegate void SelectedIndexChange_Sender(object sender, EventArgs e);

        public event SelectedIndexChange_Sender IndexChanged_Sender;

        #endregion Delegates

        #region Constantes

        private const string valorSelecione = "-1;-1;-1";

        #endregion Constantes

        #region Propriedades

        /// <summary>
        /// ClientID do combo
        /// </summary>
        public string ClientID_Combo
        {
            get
            {
                return ddlCombo.ClientID;
            }
        }

        /// <summary>
        /// ClientID do validator
        /// </summary>
        public string ClientID_Validator
        {
            get
            {
                return cpvCombo.ClientID;
            }
        }

        /// <summary>
        /// ClientID do label
        /// </summary>
        public string ClientID_Label
        {
            get
            {
                return lblTitulo.ClientID;
            }
        }

        /// <summary>
        /// Seta o vísible do Label "Ano/Série"
        /// </summary>
        public bool ExibeFormatoPeriodo
        {
            set
            {
                lblFormatoPeriodo.Visible = value;
            }
        }

        /// <summary>
        /// Coloca na primeira linha a mensagem de selecione um item.
        /// </summary>
        public bool MostrarMensagemSelecione
        {
            get
            {
                if (ViewState["MostrarMensagemSelecione"] != null)
                    return Convert.ToBoolean(ViewState["MostrarMensagemSelecione"]);
                return true;
            }
            set
            {
                ViewState["MostrarMensagemSelecione"] = value;
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
                    AdicionaAsteriscoObrigatorio(lblTitulo);
                else
                    RemoveAsteriscoObrigatorio(lblTitulo);

                cpvCombo.Visible = value;
            }
        }

        /// <summary>
        /// Deixa o combo habilitado de acordo com o valor passado
        /// </summary>
        public bool PermiteEditar
        {
            get
            {
                return ddlCombo.Enabled;
            }
            set
            {
                ddlCombo.Enabled = value;
            }
        }

        /// <summary>
        /// Propriedade que verifica quantos items existem no combo
        /// </summary>
        public int QuantidadeItensCombo
        {
            get
            {
                return ddlCombo.Items.Count;
            }
        }

        /// <summary>
        /// Propriedade que seta o SelectedIndex do Combo.
        /// </summary>
        public int SelectedIndex
        {
            set
            {
                ddlCombo.SelectedValue = ddlCombo.Items[value].Value;
            }
        }

        /// <summary>
        /// Retorna o texto selecionado no combo
        /// </summary>
        public string Texto
        {
            get
            {
                return ddlCombo.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// Seta um titulo diferente do padrão para o combo
        /// </summary>
        public string Titulo
        {
            set
            {
                lblTitulo.Text = value;
                cpvCombo.ErrorMessage = RemoveAsteriscoObrigatorio(value) + " é obrigatório.";
            }
            get
            {
                return lblTitulo.Text;
            }
        }

        /// <summary>
        /// Indica se deve trazer o primeiro item selecinado caso seja o único
        /// (Sem contar a MensagemSelecione)
        /// </summary>
        public bool TrazerComboCarregado
        {
            get
            {
                if (ViewState["TrazerComboCarregado"] != null)
                    return Convert.ToBoolean(ViewState["TrazerComboCarregado"]);
                return true;
            }
            set
            {
                ViewState["TrazerComboCarregado"] = value;
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
        /// Retorna e seta o valor selecionado no combo.
        /// valor[0] = cur_id
        /// valor[1] = crr_id
        /// valor[2] = crp_id
        /// </summary>
        public int[] Valor
        {
            get
            {
                string[] s = ddlCombo.SelectedValue.Split(';');

                if (s.Length == 3)
                    return new[] { Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), Convert.ToInt32(s[2]) };

                return new[] { -1, -1, -1 };
            }
            set
            {
                string s;
                if (value.Length == 3)
                    s = value[0] + ";" + value[1] + ";" + value[2];
                else
                    s = valorSelecione;

                if (ddlCombo.Items.FindByValue(s) != null)
                    ddlCombo.SelectedValue = s;
            }
        }

        /// <summary>
        /// Propriedade visible da label do nome do combo
        /// </summary>
        public bool Visible_Label
        {
            set
            {
                lblTitulo.Visible = value;
            }
        }

        /// <summary>
        /// Propriedade que seta o Width do combo.
        /// </summary>
        public Int32 Width_Combo
        {
            set
            {
                ddlCombo.Width = value;
            }
        }

        /// <summary>
        /// Propriedade que seta o SkinId do combo.
        /// </summary>
        public string SkinId_Combo
        {
            set
            {
                ddlCombo.SkinID = value;
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Traz o primeiro item selecinado caso seja o único
        /// </summary>
        private void SelecionaPrimeiroItem()
        {
            if (TrazerComboCarregado && (QuantidadeItensCombo == 2) && (Valor[0] == -1))
            {
                // Seleciona o primeiro item.
                ddlCombo.SelectedValue = ddlCombo.Items[1].Value;

                if (IndexChanged != null)
                    IndexChanged();
            }
        }

        /// <summary>
        /// Carrega a mensagem de selecione de acordo com o parâmetro
        /// </summary>
        private void CarregarMensagemSelecione()
        {
            if (MostrarMensagemSelecione && (ddlCombo.Items.FindByValue(valorSelecione) == null))
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", valorSelecione, true));

            ddlCombo.AppendDataBoundItems = MostrarMensagemSelecione;
        }

        /// <summary>
        /// Carrega o combo
        /// </summary>
        /// <param name="dataSource">Dados a serem inseridos no combo</param>
        private void CarregarCombo(object dataSource)
        {
            try
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = dataSource;

                CarregarMensagemSelecione();
                ddlCombo.DataBind();
                SelecionaPrimeiroItem();
            }
            catch (Exception)
            {
                lblMessage.Text = "Erro ao tentar carregar " + RemoveAsteriscoObrigatorio(lblTitulo.Text).ToLower() + ".";
                lblMessage.Visible = true;
            }
        }

        /// <summary>
        /// Remover do combo por curso e curriculo e currículo período
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crr_id">ID do currículo período</param>
        public void Remover(int cur_id, int crr_id, int crp_id)
        {
            string value = cur_id + ";" + crr_id + ";" + crp_id;
            ListItem li = ddlCombo.Items.FindByValue(value);
            if (li != null)
                ddlCombo.Items.Remove(li);
        }

        /// <summary>
        /// Seta o foco no combo
        /// </summary>
        public void SetarFoco()
        {
            ddlCombo.Focus();
        }

        /// <summary>
        /// Carrega os curriculo período não excluídos logicamente
        /// filtrados por curso e currículo
        /// </summary>
        /// <param name="cur_id">ID de Curso</param>
        /// <param name="crr_id">ID de curriculo</param>
        public void CarregarPorCursoCurriculo(int cur_id, int crr_id)
        {
            CarregarCombo(ACA_CurriculoPeriodoBO.GetSelect(cur_id, crr_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega os curriculo período equivalentes ao currículo período informado
        /// filtrados por curso, currículo, escola e curso/currículo/período equivalente
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_idEquivalente">ID do curso equivalente</param>
        /// <param name="crr_idEquivalente">ID do currículo equivalente</param>
        /// <param name="crp_idEquivalente">ID do período equivalente</param>
        public void CarregarPorCursoEscolaPeriodoEquivalente
        (
            int cur_id,
            int crr_id,
            int esc_id,
            int uni_id,
            int cur_idEquivalente,
            int crr_idEquivalente,
            int crp_idEquivalente
        )
        {
            CarregarCombo(ACA_CurriculoPeriodoBO.Seleciona_Por_CursoEscola_PeriodoEquivalente(cur_id, crr_id, esc_id, uni_id,
                        cur_idEquivalente, crr_idEquivalente, crp_idEquivalente));
        }

        /// <summary>
        /// Carrega os curriculo período não excluídos logicamente
        /// filtrados por curso, currículo e escola
        /// </summary>
        /// <param name="cur_id">ID de curso</param>
        /// <param name="crr_id">ID de curriculo</param>
        /// <param name="esc_id">ID de escola</param>
        /// <param name="uni_id">ID de unidade</param>
        public void CarregaPorCursoCurriculoEscola(int cur_id, int crr_id, int esc_id, int uni_id)
        {
            CarregarCombo(ACA_CurriculoPeriodoBO.GetSelect(cur_id, crr_id, esc_id, uni_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega os curriculo período
        /// filtrados por curso e currículo e turma disciplina
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="tud_id">ID da turma disciplina</param>
        public void CarregarPorCursoCurriculoTurmaDisciplina(int cur_id, int crr_id, long tud_id)
        {
            CarregarCombo(ACA_CurriculoPeriodoBO.SelecionaPorCursoTurmaDisciplina(cur_id, crr_id, tud_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
        }

        /// <summary>
        /// Carrega os curriculo período
        /// filtrados por curso e currículo e disciplina
        /// </summary>
        /// <param name="dis_id">ID da disciplina</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        public void CarregarPorCursoCurriculoDisciplina(int cur_id, int crr_id, int dis_id)
        {
            CarregarCombo(ACA_CurriculoPeriodoBO.Select_Por_Disciplina(dis_id, cur_id, crr_id));
        }

        /// <summary>
        /// Carrega os curriculo período
        /// filtrados por curso e currículo e tipo ciclo
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="tci_id">ID do tipo do ciclo</param>
        public void CarregarPorCursoCurriculoTipoCiclo(int cur_id, int crr_id, int tci_id)
        {
            CarregarCombo(ACA_CurriculoPeriodoBO.Select_Por_TipoCiclo(cur_id, crr_id, tci_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega os períodos da escola por curso e ciclo
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="tci_id"></param>
        public void CarregarPorCursoCurriculoEscolaCiclo(int cur_id, int crr_id, int esc_id, int uni_id, int tci_id)
        {
            CarregarCombo(ACA_CurriculoEscolaPeriodoBO.SelecionaPorEscolaCursoCiclo(cur_id, crr_id, esc_id, uni_id, tci_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        #endregion Métodos

        #region Eventos

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            bool obrigatorio = lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio) ||
                               lblTitulo.Text.EndsWith(" *");

            //Altera o Label para o nome padrão de período no sistema
            lblTitulo.Text = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

            //Altera a mensagem de validação para o nome padrão de curso no sistema
            cpvCombo.ErrorMessage = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " é obrigatório.";
            cpvCombo.ValueToCompare = valorSelecione;

            Obrigatorio = obrigatorio;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlCombo.AutoPostBack = (IndexChanged != null) || (IndexChanged_Sender != null);
            CarregarMensagemSelecione();
        }

        #endregion Page Life Cycle

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();

            if (IndexChanged_Sender != null)
                IndexChanged_Sender(sender, e);
        }

        #endregion Eventos
    }
}