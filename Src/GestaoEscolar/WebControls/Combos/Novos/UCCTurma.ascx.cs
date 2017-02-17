using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;

namespace GestaoEscolar.WebControls.Combos.Novos
{
    public partial class UCCTurma : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public SelectedIndexChanged IndexChanged;

        #endregion

        #region Constantes

        private const string valorSelecione = "-1;-1;-1";

        #endregion

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
        /// Indica se é para mostrar os dados adicionais da turma dentro do combo quando selecionada no combo.
        /// </summary>
        public bool MostraDadosAdicionaisInternos
        {
            get
            {
                if (ViewState["MostraDadosAdicionaisInternos"] != null)
                    return Convert.ToBoolean(ViewState["MostraDadosAdicionaisInternos"]);
                return false;
            }
            set
            {
                ViewState["MostraDadosAdicionaisInternos"] = value;
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
            get
            {
                return cpvCombo.Visible;
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
        /// Propriedade que seta o SelectedIndex do Combo.       
        /// </summary>
        public int SelectedIndex
        {
            set
            {
                ddlCombo.SelectedValue = ddlCombo.Items[value].Value;
            }
        }

        public DropDownList Combo
        {
            get
            {
                return ddlCombo;
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
        /// Retorna e seta o valor selecionado no combo
        /// valor[0] = tur_id
        /// valor[1] = crp_id
        /// valor[2] = ttn_id
        /// </summary>
        public long[] Valor
        {
            get
            {
                string[] s = ddlCombo.SelectedValue.Split(';');

                if (s.Length == 3)
                    return new[] { Convert.ToInt64(s[0]), Convert.ToInt64(s[1]), Convert.ToInt64(s[2]) };

                return new Int64[] { -1, -1, -1 };
            }
            set
            {
                string s;
                if (value.Length == 3)
                    s = value[0] + ";" + value[1] + ";" + value[2];
                else
                    s = valorSelecione;

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

        #endregion

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
        /// Seta em cada item do combo os valores referentes a turma.
        /// </summary>
        private void SetaDadosAdicionaisInternos()
        {
            try
            {
                if (MostraDadosAdicionaisInternos && ddlCombo.Items.Count > 0)
                {
                    foreach (ListItem item in ddlCombo.Items)
                    {
                        string[] idTurma = item.Value.Split(';');
                        if (Convert.ToInt64(idTurma[0]) > -1)
                        {
                            int qtVagas, qtMatriculados;
                            TUR_TurmaBO.RetornaVagasMatriculadosPor_Turma(Valor[0], out qtVagas, out qtMatriculados);

                            item.Text += " - Capacidade: " + qtVagas;
                            item.Text += " - Matriculados: " + qtMatriculados;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblCapacidade.Text = "";
                lblMatriculados.Text = "";
            }
        }

        /// <summary>
        /// Carrega a mensagem de selecione de acordo com o parâmetro
        /// </summary>
        private void CarregarMensagemSelecione()
        {
            if (MostrarMensagemSelecione && (ddlCombo.Items.FindByValue(valorSelecione) == null))
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma turma --", valorSelecione, true));

            ddlCombo.AppendDataBoundItems = MostrarMensagemSelecione;
        }

        /// <summary>
        /// Carrega o combo
        /// </summary>
        /// <param name="dataSource">Dados a serem inseridos no combo</param>
        public void CarregarCombo(object dataSource)
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
        /// Carrega todos as turmas
        /// filtrando por escola, e ano dos calendários da escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_ano">Ano dos calendários da escola</param>
        public void CarregaPorEscolaAno
        (
            int esc_id,
            int uni_id,
            int cal_ano
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Ano(esc_id, uni_id, cal_ano, __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                 __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id));
        }

        /// <summary>
        /// Carrega todos as turmas do tipo 1 (Normal)
        /// filtrando por escola, curso, currículo, período e calendário
        /// Sem filtrar o usuário
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="cal_id">ID do calendário</param>
        public void CarregaPorEscolaCurriculoPeriodoCalendario_SemUsuario
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(new Guid(), new Guid(), (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao), 
                                                                          esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                                                          0, 0, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos as turmas filtrando por escola, curso, currículo, período e calendário.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_id">ID do período do curso.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tur_situacao">Situacao da turma</param>
        public void CarregarPorEscolaCalendarioEPeriodo(int esc_id, int uni_id, int cal_id, int cur_id, int crr_id, int crp_id, byte tur_situacao)
        {
            CarregarCombo(TUR_TurmaBO.SelecionarPorEscolaCalendarioEPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, tur_situacao, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos as turmas do tipo 1 (Normal)
        /// filtrando por escola, curso, currículo, período e calendário
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="cal_id">ID do calendário</param>
        public void CarregaPorEscolaCurriculoPeriodoCalendario
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                          __SessionWEB.__UsuarioWEB.Grupo.gru_id, 
                                                                          (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                                                                          esc_id, uni_id, cal_id, cur_id, crr_id, crp_id,
                                                                          __SessionWEB.__UsuarioWEB.Usuario.ent_id, 0, 0, 
                                                                          ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos as turmas do tipo 1 (Normal)
        /// filtrando por escola, curso, currículo, período e calendário
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tur_situacao">Situacao da turma</param>
        public void CarregaPorEscolaCurriculoPeriodoCalendario
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id,
            byte tur_situacao,
            bool mostraEletivas = false
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                          __SessionWEB.__UsuarioWEB.Grupo.gru_id, 
                                                                          (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                                                                          esc_id, uni_id, cal_id, cur_id, crr_id, crp_id,
                                                                          __SessionWEB.__UsuarioWEB.Usuario.ent_id, 0, tur_situacao, mostraEletivas,
                                                                          ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos as turmas do tipo 1 (Normal)
        /// filtrando por escola, curso, currículo, período e situação
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="tur_situacao">Situação da turma</param>
        public void CarregaPorEscolaCurriculoPeriodoSituacao
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            byte tur_situacao
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                          __SessionWEB.__UsuarioWEB.Grupo.gru_id, 
                                                                          (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao), 
                                                                          esc_id, uni_id, -1, cur_id, crr_id, crp_id,
                                                                          __SessionWEB.__UsuarioWEB.Usuario.ent_id, 0, tur_situacao, 
                                                                          ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos as turmas ativas com calendario anual existente no MomentoAno 
        /// filtrando por escola, curso, currículo e período
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        public void CarregarPorEscolaCurriculoPeriodoMomentoAno
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Periodo_MomentoAno(esc_id, uni_id, -1, cur_id, crr_id, crp_id,
                __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                __SessionWEB.__UsuarioWEB.Grupo.gru_id));
        }

        /// <summary>
        /// Carrega todos as turmas ativas com calendario anual existente no MomentoAno e que tenha o número da avaliação do currículo 
        /// filtrando por escola, curso, currículo, período e número da avaliação
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="tca_numeroAvaliacao">Número da avaliação</param>
        public void CarregarPorEscolaCurriculoPeriodoMomentoAnoAvaliacao
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int tca_numeroAvaliacao
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Periodo_MomentoAno_Avaliacao(esc_id, uni_id, -1, cur_id, crr_id, crp_id,
                tca_numeroAvaliacao, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                __SessionWEB.__UsuarioWEB.Grupo.gru_id));
        }

        /// <summary>
        /// Carrega todos as turmas
        /// filtrando por escola, curso, currículo, período e calendário
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        public void CarregarPorEscolaCurriculoCalendario
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                          __SessionWEB.__UsuarioWEB.Grupo.gru_id, 
                                                                          (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                                                                          esc_id, uni_id, cal_id, cur_id, crr_id, crp_id,
                                                                          __SessionWEB.__UsuarioWEB.Usuario.ent_id, 0, 0, 
                                                                          ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos as turmas normais (tur_tipo = 1)
        /// filtrando por escola, curso, currículo, período e calendário
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        public void CarregarPorEscolaCurriculoCalendario_TurmasNormais
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id,
            bool mostraEletivas = false
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                __SessionWEB.__UsuarioWEB.Grupo.gru_id, (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, 1, 0, mostraEletivas,
                ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos as turmas normais (tur_tipo = 1)
        /// filtrando por escola, calendário e situação.
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="tur_situacao">Situação da turma.</param>
        public void CarregarPorEscolaCalendarioSituacao_TurmasNormais
        (
            int esc_id,
            int uni_id,
            int cal_id,
            TUR_TurmaSituacao tur_situacao
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Calendario_Situacao(esc_id, uni_id, cal_id, (byte)tur_situacao, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos as turmas ativas
        /// filtrando por escola, curso, currículo e período
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        public void CarregarPorEscolaCurriculoPeriodo
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id
        )
        {
            CarregarCombo(TUR_TurmaBO.RetornaTurmasCalendario(__SessionWEB.__UsuarioWEB.Usuario.usu_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                -1, esc_id, uni_id, -1, cur_id, crr_id, crp_id, -1, __SessionWEB.__UsuarioWEB.Usuario.ent_id, 1));
        }

        /// <summary>
        /// Carrega todos as turmas
        /// filtrando por escola, curso, currículo, período, ano e situação da turma
        /// </summary>
        /// <param name="esc_id">ID da escolar</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="cal_ano">Ano</param>
        /// <param name="tur_situacao">Situação da turma</param>
        public void CarregarPorEscolaCurriculoCalendarioSituacao
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_ano,
            TUR_TurmaSituacao tur_situacao
        )
        {
            CarregarCombo(TUR_TurmaBO.RetornaTurmasCalendario(__SessionWEB.__UsuarioWEB.Usuario.usu_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                -1, esc_id, uni_id, -1, cur_id, crr_id, crp_id, cal_ano, __SessionWEB.__UsuarioWEB.Usuario.ent_id, (byte)tur_situacao));
        }

        /// <summary>
        /// Carrega todos as turmas ativas do tipo 1 (Normal) considerando cursos equivalentes
        /// filtrando por escola, curso, currículo, período e ano
        /// </summary>
        /// <param name="esc_id">ID da escolar</param>
        /// <param name="uni_id">ID da unidade escolar</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="cal_ano">Ano</param>
        public void CarregarPorEscolaCalendarioCursoEquivalentes
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_ano
        )
        {
            CarregarCombo(TUR_TurmaBO.SelecionaPor_Escola_Calendario_CursoPeriodo_Equivalentes(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, esc_id, uni_id, cur_id, crr_id, crp_id, cal_ano));
        }

        /// <summary>
        /// Carrega as turmas eletivas do aluno
        /// filtrando por escola, curso, currículo, disciplina eletiva e calendário
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="dis_id">ID da disciplina eletiva do aluno</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        public void CarregarEletivaAlunoAtiva
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int dis_id,
            int cal_id
        )
        {
            CarregarCombo(TUR_TurmaBO.SelecionaTurmasEletivasAluno(esc_id, uni_id, cur_id, crr_id, dis_id, cal_id,
                __SessionWEB.__UsuarioWEB.Usuario.ent_id, (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id));

            SetaDadosAdicionaisInternos();
        }

        /// <summary>
        /// Carrega todos as turmas do tipo 1 (Normal) e com fav_tipoLancamentoFrequencia = 3 ou 4
        /// filtrando por escola, calendário, curso, currículo e currículo período
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do currículo período.</param>
        public void CarregarComFrequenciaMensalPorEscolaCalendarioCurriculoPeriodo
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            CarregarCombo(TUR_TurmaBO.SelecionaPorEscolaPeriodoCalendarioComFrequenciaMensal(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id,
                __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id));
        }

        /// <summary>
        /// Carrega todos as turmas encerradas
        /// filtrando por escola, curso, currículo, período e calendário
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        public void CarregarPorEscolaCurriculoCalendarioEncerradas
        (
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id
        )
        {
            CarregarCombo(TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                          __SessionWEB.__UsuarioWEB.Grupo.gru_id, 
                                                                          (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                                                                          esc_id, uni_id, cal_id, cur_id, crr_id, crp_id,
                                                                          __SessionWEB.__UsuarioWEB.Usuario.ent_id, 0,
                                                                          (byte)TUR_TurmaSituacao.Encerrada, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega as turmas em que o docente da aula.
        /// </summary>
        /// <param name="doc_id">Código do docente</param>
        /// <param name="esc_id">Código da escola</param>
        /// <param name="mostrarCodigoNome">True - Exibe o código e nome da turma | False - Exibe apenas o código da turma</param>
        public void CarregarPorDocente(long doc_id, int esc_id, bool mostrarCodigoNome, bool turmasNormais = false)
        {
            CarregarPorDocente(doc_id, esc_id, 0, mostrarCodigoNome, turmasNormais);
        }

        /// <summary>
        /// Carrega as turmas em que o docente da aula.
        /// </summary>
        /// <param name="doc_id">Código do docente</param>
        /// <param name="esc_id">Código da escola</param>
        /// <param name="mostrarCodigoNome">True - Exibe o código e nome da turma | False - Exibe apenas o código da turma</param>
        public void CarregarPorDocente(long doc_id, int esc_id, int cal_id, bool mostrarCodigoNome, bool turmasNormais = false, bool mostraEletivas = true)
        {
            int posicaoDocenteCompatilhado = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.POSICAO_DOCENCIA_COMPARTILHADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            try
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = TUR_TurmaBO.GetSelectBy_Docente_TodosTipos_Posicao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, doc_id, posicaoDocenteCompatilhado, esc_id, cal_id, mostrarCodigoNome, turmasNormais, mostraEletivas, ApplicationWEB.AppMinutosCacheLongo);
                ddlCombo.DataTextField = "tur_esc_nome";

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

        #endregion

        #region Eventos

        #region Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            bool obrigatorio = lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio) ||
                               lblTitulo.Text.EndsWith(" *");

            cpvCombo.ValueToCompare = valorSelecione;

            Obrigatorio = obrigatorio;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlCombo.AutoPostBack = (IndexChanged != null);
            CarregarMensagemSelecione();
        }

        #endregion

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();
        }

        #endregion
    }
}