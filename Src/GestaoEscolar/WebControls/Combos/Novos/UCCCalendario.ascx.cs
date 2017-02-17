using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Collections.Generic;

namespace GestaoEscolar.WebControls.Combos.Novos
{
    public partial class UCCCalendario : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public SelectedIndexChanged IndexChanged;

        #endregion

        #region Constantes

        private const string valorSelecione = "-1";

        #endregion

        #region Proriedades

        /// <summary>
        /// Retorna o ano selecionado no combo
        /// </summary>
        public int Cal_ano
        {
            get
            {
                int ano = 0;

                if (ddlCombo.SelectedIndex > 0)
                {
                    Int32.TryParse(Texto.Substring(0, 4), out ano);
                    return ano;
                }

                return ano;
            }
        }

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
        /// Quando carrega o combo já traz o ano corrente selecionado
        /// </summary>
        public bool SelecionarAnoCorrente
        {
            get
            {
                if (ViewState["SelecionarAnoCorrente"] != null)
                    return Convert.ToBoolean(ViewState["SelecionarAnoCorrente"]);
                return true;
            }
            set
            {
                ViewState["SelecionarAnoCorrente"] = value;
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
        /// Seleciona uma opção com o ano corrente no combo
        /// </summary>
        private void SelecionaOpcaoAnoCorrente()
        {
            if (SelecionarAnoCorrente)
            {
                var x = from ListItem item in ddlCombo.Items
                        where
                            item.Value != valorSelecione &&
                            Convert.ToInt32(item.Text.Substring(0, 4)) == DateTime.Now.Year
                        select item.Value;

                if (x.Count() == 1)
                {
                    ddlCombo.SelectedValue = x.First();

                    if (IndexChanged != null)
                        IndexChanged();
                }
                else if (ddlCombo.Items.Count == 2)
                {
                    ddlCombo.SelectedValue = ddlCombo.Items[1].Value;

                    if (IndexChanged != null)
                        IndexChanged();
                }
            }
        }

        /// <summary>
        /// Carrega a mensagem de selecione de acordo com o parâmetro
        /// </summary>
        private void CarregarMensagemSelecione()
        {
            if (MostrarMensagemSelecione && (ddlCombo.Items.FindByValue(valorSelecione) == null))
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", valorSelecione, true));

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
                SelecionaOpcaoAnoCorrente();
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
        /// Carrega todos os calendários não excluídos logicamente
        /// </summary>
        public void Carregar()
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnual(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }
        
        /// <summary>
        /// Carrega todos os calendários não excluídos logicamente
        /// filtrados por ano
        /// </summary>
        public void CarregarPorAno(int cal_ano)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnualByAno(cal_ano,__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os calendários não excluídos logicamente
        /// filtrando por escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        public void CarregarPorEscola(int esc_id)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnual_Esc_id(esc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os calendários não excluídos logicamente
        /// filtrando por docente (turmas que o docente está atribuído)
        /// </summary>
        /// <param name="doc_id">ID do docente</param>
        public void CarregarPorDocente(long doc_id)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnualPorDocente(doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os calendários não excluídos logicamente
        /// filtrando por curso
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        public void CarregarPorCurso(int cur_id)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnualPorCurso(cur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os calendários não excluídos logicamente
        /// filtrando por curso com turma Ativa
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        public void CarregarPorCursoTurmaAtiva(int cur_id)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnualPorCursoTurmaAtiva(cur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os calendários não excluídos logicamente
        /// filtrando por ano e escola
        /// </summary>
        /// <param name="anobase">Ano</param>
        /// <param name="esc_id">ID da escola</param>
        public void CarregarPorEscolaAnoBase(int esc_id, int anobase)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnual_AnoBase_Esc_id(anobase, esc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os calendários não excluídos logicamente
        /// filtrando por curso e processo fechamento inicio
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="pfi_id">ID do processo fechamento inicio</param>
        public void CarregarPorCursoAnoInicio(int cur_id, int pfi_id)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnualPorCursoAnoInicio(cur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, pfi_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega todos os calendários não excluídos logicamente
        /// filtrando por curso e quantidade de períodos do calendário
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="qtdePeriodos">Quantidade de períodos</param>
        public void CarregarPorCursoQtdePeriodos(int cur_id, int qtdePeriodos)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnualPorCursoQtdePeriodos(cur_id, qtdePeriodos, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>  
        /// Carrega todos os calendários não excluídos logicamente de cursos que possuem disciplina eletiva
        /// filtrando por curso e escola
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="esc_id">ID da escola </param>
        /// <param name="uni_id">ID da unidadel</param> 
        public void CarregarPorEscolaCursoComDisciplinaEletiva(int esc_id, int uni_id, int cur_id)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaPorCursoComDisciplinaEletiva(cur_id, esc_id, uni_id, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_DISCIPLINA_ELETIVA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id), __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// arrega os calendários anuais por curso a partir de 2012, exceto o ano corrente
        /// </summary>
        /// <param name="cur_id">ID do curso.</param>
        public void CarregarAnosAnterioresPorCurso(int cur_id)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaAnosAnterioresPorCurso(cur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Mostra os dados não excluídos logicamente no dropdownlist    
        /// </summary>
        public void CarregarCalendarioAnual()
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendarioAnual(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo));
        }

        /// <summary>
        /// Carrega os calendarios com bimestres ativos por entidade e escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="VerificaEscolaCalendarioPeriodo">Informa se irá selecionar todos os dados conforme os filtros (false) 
        /// ou se irá selecionar apenas os dados que não estão na tabela ESC_EscolaCalendarioPeriodo (true)</param>
        /// <returns></returns>
        public void CarregarCalendariosComBimestresAtivos(int esc_id, bool VerificaEscolaCalendarioPeriodo)
        {
            CarregarCombo(ACA_CalendarioAnualBO.SelecionaCalendariosComBimestresAberto_Por_EntidadeEscola(__SessionWEB.__UsuarioWEB.Usuario.ent_id, esc_id, VerificaEscolaCalendarioPeriodo));
        }

        /// <summary>
        /// Carrega todos os calendários não excluídos logicamente
        /// </summary>
        public void CarregarCalendarioAnualAnoAtual()
        {
            List<sComboCalendario> lstCalendario = ACA_CalendarioAnualBO.SelecionaCalendarioAnual(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            if (lstCalendario.Any())
            {
                if (lstCalendario.Any(c => Convert.ToInt32(c.cal_ano) >= DateTime.Today.Year))
                    lstCalendario = lstCalendario.Where(c => Convert.ToInt32(c.cal_ano) >= DateTime.Today.Year).ToList();
                else
                    lstCalendario = lstCalendario.Where(c => Convert.ToInt32(c.cal_ano) == Convert.ToInt32(lstCalendario.OrderBy(p => p.cal_ano).LastOrDefault().cal_ano)).ToList();
            }
            CarregarCombo(lstCalendario.AsEnumerable()
                            .Select(p => new
                            {
                                cal_ano_desc = p.cal_ano_desc.ToString()
                                ,
                                cal_id = p.cal_id.ToString()
                            }));
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