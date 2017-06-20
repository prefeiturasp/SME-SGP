using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos.Novos
{
    public partial class UCCCursoCurriculo : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();

        public event SelectedIndexChanged IndexChanged;

        public delegate void SelectedIndexChange_Sender(object sender, EventArgs e);

        public event SelectedIndexChange_Sender IndexChanged_Sender;

        #endregion Delegates

        #region Constantes

        private const string valorSelecione = "-1;-1";

        #endregion Constantes

        #region Proriedades

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
                cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
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
        /// </summary>
        public int[] Valor
        {
            get
            {
                string[] s = ddlCombo.SelectedValue.Split(';');

                if (s.Length == 2)
                    return new[] { Convert.ToInt32(s[0]), Convert.ToInt32(s[1]) };

                return new[] { -1, -1 };
            }
            set
            {
                string s;
                if (value.Length == 2)
                    s = value[0] + ";" + value[1];
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

        #endregion Proriedades

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
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", valorSelecione, true));

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
                lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
                lblMessage.Visible = true;
            }
        }

        /// <summary>
        /// Seta o foco no combo
        /// </summary>
        public void SetarFoco()
        {
            ddlCombo.Focus();
        }

        /// <summary>
        /// Remover do combo por curso e curriculo
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        public void Remover(int cur_id, int crr_id)
        {
            string value = cur_id + ";" + crr_id;
            ListItem li = ddlCombo.Items.FindByValue(value);
            if (li != null)
                ddlCombo.Items.Remove(li);
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// </summary>
        public void Carregar()
        {
            CarregarPorEscolaSituacaoCurso(-1, -1, 0);
        }

        /// <summary>
        /// Carrega cursos/currículos não excluídos logicamente
        /// filtrando por situação do curso
        /// </summary>
        /// <param name="cur_situacao">Situação do curso</param>
        public void CarregarPorSituacaoCurso(byte cur_situacao)
        {
            CarregarPorEscolaSituacaoCurso(-1, -1, cur_situacao);
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// filtrando por escola e situação do curso
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_situacao">Situação do curso</param>
        public void CarregarPorEscolaSituacaoCurso(int esc_id, int uni_id, byte cur_situacao, bool mostraEJAModalidades = false)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculo(esc_id, uni_id, cur_situacao, __SessionWEB.__UsuarioWEB.Usuario.ent_id, mostraEJAModalidades, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// filtrando por escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        public void CarregarPorEscola(int esc_id, int uni_id)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoPorEscola(esc_id, uni_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// e que estão vigentes filtrando por escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        public void CarregarVigentesPorEscola(int esc_id, int uni_id)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoVigentesPorEscola(esc_id, uni_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// filtrando por escola e tipo de ciclo
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        public void CarregarPorEscolaTipoCiclo(int esc_id, int uni_id, int tci_id)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoPorEscolaTipoCiclo(esc_id, uni_id, tci_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// e que estão vigentes filtrando por escola e tipo de ciclo
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        public void CarregarVigentesPorEscolaTipoCiclo(int esc_id, int uni_id, int tci_id)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoVigentesPorEscolaTipoCiclo(esc_id, uni_id, tci_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// filtrando por escola, calendario e tipo de ciclo
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_id">ID do calendario</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        public void CarregarPorEscolaCalendarioTipoCiclo(int esc_id, int uni_id, int cal_id, int tci_id)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoPorEscolaCalendarioTipoCiclo(esc_id, uni_id, cal_id, tci_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// filtrando por escola, ano letivo e tipo de ciclo
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_ano">Ano do calendario</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        public void CarregarPorEscolaCalendarioAnoTipoCiclo(int esc_id, int uni_id, int cal_ano, int tci_id)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoPorEscolaCalendarioAnoTipoCiclo(esc_id, uni_id, cal_ano, tci_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }


        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// filtrando por escola e tipo nivel de ensino
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="tne_id">ID do tipo nivel de ensino</param>
        public void CarregarPorEscolaNivelEnsino(int esc_id, int uni_id, int tne_id)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoPorEscolaNivelEnsino(esc_id, uni_id, tne_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente que possuem disciplina eletiva
        /// filtrando por escola e situação do curso
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_situacao">Situação do curso</param>
        public void CarregarComDisciplinaEletiva(int esc_id, int uni_id, int cur_situacao, bool mostraEJAModalidades = false)
        {
            CarregarCombo(ACA_CursoBO.SelectCursoComDisciplinaEletiva(esc_id, uni_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, cur_situacao, mostraEJAModalidades, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos relacionados ao curso e escola informados.
        /// filtrando por curso, currículo do curso e escola
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="somenteAtivos">True - Trazer os cursos relacionados ativos / False - Trazer os cursos relacionados não excluídos logicamente</param>
        /// <returns></returns>
        public void CarregarCursoRelacionadoPorEscola(int cur_id, int crr_id, int esc_id, int uni_id, bool somenteAtivos)
        {
            CarregarCombo(ACA_CursoBO.Seleciona_CursosRelacionados_Por_Escola(cur_id, crr_id, esc_id, uni_id, somenteAtivos, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// filtrando por escola, calendário e situação do curso
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_situacao">Situação do curso</param>
        public void CarregarPorEscolaCalendarioSituacaoCurso(int esc_id, int uni_id, int cal_id, byte cur_situacao, bool mostraEJAModalidades = false)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoCalendarioEscola(esc_id, uni_id, cur_situacao, __SessionWEB.__UsuarioWEB.Usuario.ent_id, cal_id, mostraEJAModalidades, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// filtrando por escola, calendário, situação do curso e níveis de ensino
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_situacao">Situação do curso</param>
        /// <param name="tne_ids">Arrays de ids de nível de ensino</param>
        public void CarregarPorEscolaCalendarioSituacaoCursoNivelEnsino(int esc_id, int uni_id, int cal_id, byte cur_situacao, int[] tne_ids, bool mostraEJAModalidades = false)
        {
            List<sComboCurso> lstCurso = ACA_CursoBO.SelecionaCursoCurriculoCalendarioEscola(esc_id, uni_id, cur_situacao, __SessionWEB.__UsuarioWEB.Usuario.ent_id, cal_id, mostraEJAModalidades, ApplicationWEB.AppMinutosCacheLongo);
            lstCurso = lstCurso.Where(p => tne_ids.Contains(p.tne_id)).ToList();
            CarregarCombo(lstCurso);
        }

        /// <summary>
        /// Carrega todos os cursos/currículos não excluídos logicamente
        /// filtrando por escola, calendário e situação do curso
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_situacao">Situação do curso</param>
        public void CarregarVigentesPorEscolaCalendarioSituacaoCurso(int esc_id, int uni_id, int cal_id, byte cur_situacao)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoVigentesCalendarioEscola(esc_id, uni_id, cur_situacao, __SessionWEB.__UsuarioWEB.Usuario.ent_id, cal_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        public void CarregarPorTipoNivelEnsino(int tne_id)
        {
            CarregarCombo(ACA_CursoBO.SelecionaCursoCurriculoPorNivelEnsino(tne_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carregar por modalidade de ensino.
        /// </summary>
        /// <param name="tme_id"></param>
        public void CarregarPorModalidadeEnsino(int tme_id)
        {
            CarregarCombo(ACA_CursoBO.Seleciona_Cursos_Por_ModalidadeEnsino(tme_id, -1, -1, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        #endregion Métodos

        #region Eventos

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            bool obrigatorio = lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio) || lblTitulo.Text.EndsWith(" *");

            //Altera o Label para o nome padrão de curso no sistema
            lblTitulo.Text = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            //Altera a mensagem de validação para o nome padrão de curso no sistema
            cpvCombo.ErrorMessage = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " é obrigatório.";
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