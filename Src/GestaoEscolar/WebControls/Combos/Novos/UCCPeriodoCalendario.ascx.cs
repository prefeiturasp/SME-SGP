using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using System.Data;
using MSTech.GestaoEscolar.Entities;

namespace GestaoEscolar.WebControls.Combos.Novos
{
    public partial class UCCPeriodoCalendario : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion

        #region Constantes

        private const string valorSelecione = "-1;-1";

        private const string valorMsgSelecione = "-- Selecione uma turma --";

        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor do Tpc_ID selecionado no combo
        /// </summary>
        public int Tpc_ID
        {
            get
            {
                return Valor[0];
            }
            set
            {
                var x = from ListItem list in ddlCombo.Items
                        where (Convert.ToInt32(list.Value.Split(';')[0]) == value)
                        select Convert.ToInt32(list.Value.Split(';')[1]);

                if (x.Count() > 0 && ddlCombo.Items.FindByValue(x.ToList()[0].ToString()) != new ListItem())
                    Valor = new[] { value, x.ToList()[0] };
            }
        }

        /// <summary>
        /// Retorna e seta o valor do Cap_ID selecionado no combo
        /// </summary>
        public int Cap_ID
        {
            get
            {
                return Valor[1];
            }
            set
            {
                var x = from ListItem list in ddlCombo.Items
                        where (Convert.ToInt32(list.Value.Split(';')[1]) == value)
                        select Convert.ToInt32(list.Value.Split(';')[0]);

                if (x.Count() > 0 && ddlCombo.Items.FindByValue(x.ToList()[0].ToString()) != new ListItem())
                    Valor = new[] { x.ToList()[0], value };
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
        /// Coloca na primeira linha a mensagem de selecione anual.
        /// </summary>
        public bool MostrarMensagemSelecioneAnual
        {
            get
            {
                if (ViewState["MostrarMensagemSelecioneAnual"] != null)
                    return Convert.ToBoolean(ViewState["MostrarMensagemSelecioneAnual"]);
                return false;
            }
            set
            {
                ViewState["MostrarMensagemSelecioneAnual"] = value;
            }
        }

        /// <summary>
        /// Adiciona a opção de "Final" no combo.
        /// </summary>
        public bool MostrarOpcaoFinal
        {
            get
            {
                if (ViewState["MostrarOpcaoFinal"] != null)
                    return Convert.ToBoolean(ViewState["MostrarOpcaoFinal"]);
                return false;
            }
            set
            {
                ViewState["MostrarOpcaoFinal"] = value;
            }
        }

        /// <summary>
        /// Propriedade que seta/retorna a label e a validação do combo
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
        /// Propriedade que diz se será carregado automaticamente o período atual ou não.
        /// </summary>
        public bool SelecionaPeriodoAtualAoCarregar
        {
            get
            {
                if (ViewState["SelecionaPeriodoAtualAoCarregar"] != null)
                    return Convert.ToBoolean(ViewState["SelecionaPeriodoAtualAoCarregar"]);
                return false;
            }
            set
            {
                ViewState["SelecionaPeriodoAtualAoCarregar"] = value;
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
        /// Retorna e seta o valor selecionado no combo
        /// Valor[0] = tpc_id
        /// Valor[1] = cap_id
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
        /// Retorna se o item selecionado é o último bimestre disponível no combo
        /// </summary>
        /// <returns></returns>
        public bool SelecaoUltimoBimestre()
        {
            var x = (from ListItem item in ddlCombo.Items
                     orderby Convert.ToInt32(item.Value.Split(';')[0]) descending
                     select item.Value);

            if (x.Count() > 0)
            {
                // Verifica se o valor selecionado no combo é igual ao valor do último bimestre.
                return ddlCombo.SelectedValue == x.FirstOrDefault();
            }

            return false;
        }

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
        /// Carrega a opcao de "Final"
        /// </summary>
        private void CarregarOpcaoFinal()
        {
            if (MostrarOpcaoFinal)
                ddlCombo.Items.Insert(ddlCombo.Items.Count, new ListItem("Final", "-2;-2", true));
            
        }

        /// <summary>
        /// Carrega a mensagem de selecione de acordo com o parâmetro
        /// </summary>
        private void CarregarMensagemSelecione()
        {
            if (MostrarMensagemSelecione && !MostrarMensagemSelecioneAnual)
            {
                if (ddlCombo.Items.FindByValue(valorSelecione) != null)
                {
                    ddlCombo.Items.RemoveAt(0);
                }
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " --", valorSelecione, true));
            }
            else if (MostrarMensagemSelecioneAnual && !MostrarMensagemSelecione)
            {
                if (ddlCombo.Items.FindByValue(valorSelecione) != null)
                {
                    ddlCombo.Items.RemoveAt(0);
                }
                ddlCombo.Items.Insert(0, new ListItem(" Anual ", valorSelecione, true));
            }

            ddlCombo.AppendDataBoundItems = MostrarMensagemSelecione || MostrarMensagemSelecioneAnual;
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
                CarregarOpcaoFinal();
                SelecionaPrimeiroItem();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
                lblMessage.Visible = true;
            }
        }

        /// <summary>
        /// Carrega o período atual
        /// </summary>
        /// <param name="dt">DataTable de tipos de currículo período</param>
        /// <param name="carregar">True - Carregar / False - Não carregar</param>
        private void CarregarPeriodoAtual(List<sTipoPeriodoCalendario> dt, bool carregar)
        {
            if (carregar)
            {
                // Se passou a disciplina, verificar se o período atual está na lista de períodos da
                // disciplina.
                var x = from dr in dt
                        where dr.PeriodoAtual.Equals(1)
                        select dr.tpc_cap_id.ToString();

                if (x.Count() > 0)
                {
                    ddlCombo.SelectedValue = x.First();
                }
            }
        }

        /// <summary>
        /// Carrega o período atual
        /// </summary>
        /// <param name="dt">DataTable de tipos de currículo período</param>
        /// <param name="carregar">True - Carregar / False - Não carregar</param>
        private void CarregarPeriodoAtual(List<sComboPeriodoCalendario> dt, bool carregar)
        {
            if (carregar)
            {
                // Se passou a disciplina, verificar se o período atual está na lista de períodos da
                // disciplina.
                var x = from dr in dt
                        where dr.PeriodoAtual
                        select dr.tpc_cap_id.ToString();

                if (x.Count() > 0)
                {
                    ddlCombo.SelectedValue = x.First();
                }
            }
        }

        /// <summary>
        /// Carrega o período atual
        /// </summary>
        /// <param name="dt">DataTable de tipos de currículo período</param>
        /// <param name="carregar">True - Carregar / False - Não carregar</param>
        private void CarregarPeriodoAtual(DataTable dt, bool carregar)
        {
            if (carregar)
            {
                // Se passou a disciplina, verificar se o período atual está na lista de períodos da
                // disciplina.
                var x = from DataRow dr in dt.Rows
                        where Convert.ToBoolean(dr["PeriodoAtual"])
                        select dr[ddlCombo.DataValueField].ToString();

                if (x.Count() > 0)
                {
                    ddlCombo.SelectedValue = x.First();
                }
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
        /// Carrega todos os tipos de período calendário de períodos vigentes ou com um evento de efetivação vigente ligado ao tpc_id
        /// (se for disciplina eletiva ou eletiva do aluno, traz somente os períodos que a disciplina oferece)
        /// filtrando por calendário e disciplina e turma
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_id">ID da turma</param>
        public void CarregarPorPeriodoEventoEfetivacaoVigentes(int cal_id, long tud_id, long tur_id)
        {
            List<sComboPeriodoCalendario> dt = ACA_TipoPeriodoCalendarioBO.SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente(cal_id, tud_id, tur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, false, ApplicationWEB.AppMinutosCacheLongo);
            CarregarCombo(dt);
            CarregarPeriodoAtual(dt, (tud_id > 0));
        }

        /// <summary>
        /// Carrega todos os tipos de período calendário de períodos vigentes ou com um evento de efetivação vigente ligado ao tpc_id
        /// (se for disciplina eletiva ou eletiva do aluno, traz somente os períodos que a disciplina oferece)
        /// filtrando por calendário e disciplina e turma
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="VerificaEscolaCalendarioPeriodo">Informa se irá selecionar todos os dados conforme os filtros (false) 
        /// ou se irá selecionar apenas os dados que não estão na tabela ESC_EscolaCalendarioPeriodo (true)</param>
        public void CarregarPorPeriodoEventoEfetivacaoVigentes(int cal_id, long tud_id, long tur_id, bool VerificaEscolaCalendarioPeriodo)
        {        
            List<sComboPeriodoCalendario> dt = ACA_TipoPeriodoCalendarioBO.SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente(cal_id, tud_id, tur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, VerificaEscolaCalendarioPeriodo, ApplicationWEB.AppMinutosCacheLongo);            
            // exibe apenas o ultimo periodo aberto,
            // o metodo ja retorna os periodos abertos ordenados do mais recente para o mais antigo
            CarregarCombo(dt);   
        }

        /// <summary>
        /// Carrega todos os tipos de período calendário
        /// (se for disciplina eletiva ou eletiva do aluno, traz somente os períodos que a disciplina oferece)
        /// filtrando por calendário e disciplina
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tud_id">ID da disciplina</param>
        public void CarregarPorPeriodoVigenteAteAtual(int cal_id , long tud_id)
        {
            DataTable dt = ACA_TipoPeriodoCalendarioBO.CarregarPeriodosAteDataAtual(cal_id, tud_id);
            CarregarCombo(dt);
            CarregarPeriodoAtual(dt, (tud_id > 0));
        }

        /// <summary>
        /// Carrega todos os tipos de período calendário não excluídos logicamente
        /// filtrando por turma.
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        public void CarregarPorTurma(long tur_id)
        {
            ddlCombo.DataTextField = "cap_descricao";
            CarregarCombo(ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendario_Tur(tur_id, ApplicationWEB.AppMinutosCacheLongo));
        }
        
        /// <summary>
        /// Carrega todos os tipos de período calendário não excluídos logicamente
        /// filtrando por turma com o coc 0
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        public void CarregarPorTurma_Coc0(long tur_id)
        {
            ddlCombo.DataTextField = "cap_descricao";
            DataTable dt = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendario_Fav_Tur(tur_id);

            try
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = dt;

                CarregarMensagemSelecione();

                // Adiciona um registro com valor 0 no combo
                string nomeInicio = MTR_TipoMomentoBO.GetEntity(new MTR_TipoMomento { tmm_id = (byte)MTR_TipoMomentoNomes.InicioAnoLetivoAposFechamento }).tmm_nome;

                ddlCombo.Items.Insert(1, new ListItem(nomeInicio, "0;0", true));

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
        /// Carrega todos os tipos de período calendário não excluídos logicamente
        /// filtrando por calendário
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        public void CarregarPorCalendario(int cal_id, bool adicionarRecesso = false)
        {
            sTipoPeriodoCalendario sTpc = new sTipoPeriodoCalendario();

            ddlCombo.DataTextField = "cap_descricao";
            List<sTipoPeriodoCalendario> dt = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);
            if (adicionarRecesso)
            {
                int tpc_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (tpc_id > 0)
                {
                    ACA_TipoPeriodoCalendario tpc = new ACA_TipoPeriodoCalendario { tpc_id = tpc_id };
                    ACA_TipoPeriodoCalendarioBO.GetEntity(tpc);
                    sTpc.tpc_id = tpc_id;
                    sTpc.tpc_nome = sTpc.cap_descricao = tpc.tpc_nome;
                    sTpc.tpc_cap_id = string.Format("{0};-1", tpc_id);

                    dt.Add(sTpc);
                }
            }
            CarregarCombo(dt);
            CarregarPeriodoAtual(dt, SelecionaPeriodoAtualAoCarregar);

            if (sTpc.tpc_id > 0)
            {
                dt.Remove(sTpc);
            }
        }

        /// <summary>
        /// Carrega os tipos de período calendário não excluídos logicamente
        /// que estão em fechamento e o atual.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        public void CarregarPeriodosAtualFechamentoPorCalendario(int cal_id)
        {
            ddlCombo.DataTextField = "cap_descricao";
            DataTable dt = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioAtualFechamentoPorCalendario(cal_id);
            CarregarCombo(dt);
            CarregarPeriodoAtual(dt, SelecionaPeriodoAtualAoCarregar);
        }

        /// <summary>
        /// Carrega todos os tipos de período calendário não excluídos logicamente
        /// filtrando por calendário.
        /// Adiciona uma linha a mais antes do primeiro registro, com valor -2;-2, indicando o período
        /// "Início ano letivo após o fechamento de matrícula".
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        public void CarregarPorCalendario_Coc0(int cal_id)
        {
            ddlCombo.DataTextField = "cap_descricao";
            List<sTipoPeriodoCalendario> dt = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);

            try
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = dt;

                CarregarMensagemSelecione();

                // Adiciona um registro com valor 0 no combo
                string nomeInicio = MTR_TipoMomentoBO.GetEntity(new MTR_TipoMomento
                                                {tmm_id = (byte)MTR_TipoMomentoNomes.InicioAnoLetivoAposFechamento}).tmm_nome;
                
                ddlCombo.Items.Insert(1, new ListItem(nomeInicio, "0;0", true));

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

            //Altera o Label para o nome padrão de período no sistema
            lblTitulo.Text = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

            //Altera a mensagem de validação para o nome padrão de curso no sistema
            cpvCombo.ErrorMessage = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " é obrigatório.";
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