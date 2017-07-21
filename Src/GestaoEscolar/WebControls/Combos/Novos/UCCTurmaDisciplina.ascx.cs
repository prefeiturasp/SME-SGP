using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.Combos.Novos
{
    public partial class UCCTurmaDisciplina : MotherUserControl
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
        /// valor = tud_id
        /// </summary>
        public long Valor
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlCombo.SelectedValue))
                {
                    long valor = Convert.ToInt64(ddlCombo.SelectedValue);
                    if (valor > 0)
                        return valor;
                }

                return -1;
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

        public bool VS_MostraFilhosRegencia
        {
            get
            {
                if (ViewState["VS_MostraFilhosRegencia"] != null)
                {
                    return Convert.ToBoolean(ViewState["VS_MostraFilhosRegencia"]);
                }

                return true;
            }

            set
            {
                ViewState["VS_MostraFilhosRegencia"] = value;
            }
        }

        public bool VS_MostraRegencia
        {
            get
            {
                if (ViewState["VS_MostraRegencia"] != null)
                {
                    return Convert.ToBoolean(ViewState["VS_MostraRegencia"]);
                }

                return true;
            }

            set
            {
                ViewState["VS_MostraRegencia"] = value;
            }
        }

        /// <summary>
        /// Mostra experiência dos territórios do saber.
        /// </summary>
        public bool VS_MostraExperiencia
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_MostraExperiencia"] ?? (ViewState["VS_MostraExperiencia"] = true));
            }

            set
            {
                ViewState["VS_MostraExperiencia"] = value;
            }
        }

        /// <summary>
        /// Mostra os territórios do saber.
        /// </summary>
        public bool VS_MostraTerritorio
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_MostraTerritorio"] ?? (ViewState["VS_MostraTerritorio"] = false));
            }

            set
            {
                ViewState["VS_MostraTerritorio"] = value;
            }
        }

        /// <summary>
        /// Retorna se o combo está sendo utilizado para mostrar turmas eletivas.
        /// </summary>
        public bool VS_TurmaEletiva
        {
            get
            {
                if (ViewState["VS_TurmaEletiva"] != null)
                    return Convert.ToBoolean(ViewState["VS_TurmaEletiva"]);
                return false;
            }
            set
            {
                ViewState["VS_TurmaEletiva"] = value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Traz o primeiro item selecinado caso seja o único
        /// </summary>
        private void SelecionaPrimeiroItem()
        {
            if (QuantidadeItensCombo == 2 && Valor == -1)
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
            if (MostrarMensagemSelecione && ddlCombo.Items.FindByValue(valorSelecione) == null)
            {
                if (VS_TurmaEletiva)
                {
                    ddlCombo.Items.Insert(0, new ListItem(GetGlobalResourceObject("UserControl", "UCCTurmaDisciplina.cmbCombo.MsgSelecione.TurmaEletiva").ToString(), valorSelecione, true));
                }
                else
                {
                    ddlCombo.Items.Insert(0, new ListItem(GetGlobalResourceObject("UserControl", "UCCTurmaDisciplina.cmbCombo.MsgSelecione").ToString(), valorSelecione, true));
                }
            }
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
        /// Carrega as disciplinas de uma turma.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        public void CarregarTurmaDisciplina(long tur_id, bool exibirDisciplinasSemFrequencia = true, int cap_id = 0, bool exibirApenasDisciplinasPermiteAbonoFalta = false)
        {
            List<sTurmaDisciplina> lista = TUR_TurmaDisciplinaBO.GetSelectBy_tur_id(tur_id,
                __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                VS_MostraFilhosRegencia,
                VS_MostraRegencia,
                VS_MostraExperiencia,
                VS_MostraTerritorio,
                cap_id,
                ApplicationWEB.AppMinutosCacheLongo);

            if (!exibirDisciplinasSemFrequencia)
            {
                lista = lista.FindAll(p => !p.tud_naoLancarFrequencia);
            }

            if (exibirApenasDisciplinasPermiteAbonoFalta)
            {
                lista = lista.FindAll(p => p.tud_permitirLancarAbonoFalta);
            }

            CarregarCombo(lista);
        }

        /// <summary>
        /// Carrega as disciplinas de uma turma. Sem a docencia compartilhada
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="doc_id">id do docente</param>
        public void CarregarTurmaDisciplina_SemCompartilhada
            (long tur_id, long doc_id, bool mostraCompartilhadas = false)
        {
            if (doc_id > 0)
            {
                CarregarCombo(TUR_TurmaDisciplinaBO.GetSelectBy_TurmaDocente
                    (tur_id
                    , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    , VS_MostraFilhosRegencia, VS_MostraRegencia, VS_MostraExperiencia, VS_MostraTerritorio
                    , doc_id
                    , 0
                    , ApplicationWEB.AppMinutosCacheLongo
                    , mostraCompartilhadas)
                    .Where(p => p.tud_tipo != (int)TurmaDisciplinaTipo.DocenciaCompartilhada));
            }
            else
            {
                CarregarCombo(TUR_TurmaDisciplinaBO.GetSelectBy_tur_id(tur_id,
                                                                       __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                                                       VS_MostraFilhosRegencia,
                                                                       VS_MostraRegencia,
                                                                       VS_MostraExperiencia,
                                                                       VS_MostraTerritorio,
                                                                       0
                                                                       , ApplicationWEB.AppMinutosCacheLongo)
                                                                       .Where(p => p.tud_tipo != (int)TurmaDisciplinaTipo.DocenciaCompartilhada));
            }
        }

        /// <summary>
        /// Carrega as disciplinas de uma turma.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="doc_id">id do docente</param>
        public void CarregarTurmaDisciplina(long tur_id, long doc_id, int cap_id = 0, bool exibirApenasDisciplinasPermiteAbonoFalta = false)
        {
            List<sTurmaDisciplina> lista = TUR_TurmaDisciplinaBO.GetSelectBy_TurmaDocente
                (tur_id
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                , VS_MostraFilhosRegencia, VS_MostraRegencia, VS_MostraExperiencia, VS_MostraTerritorio
                , doc_id
                , cap_id
                , ApplicationWEB.AppMinutosCacheLongo);

            if (exibirApenasDisciplinasPermiteAbonoFalta)
            {
                CarregarCombo(lista.Where(p => p.tud_permitirLancarAbonoFalta));
            }
            else
            {
                CarregarCombo(lista);
            }
        }

        /// <summary>
        /// Carrega as disciplinas de uma turma.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="doc_id">id do docente</param>
        public void CarregarTurmaDisciplinaFiltraProjetos(long tur_id, long doc_id, int cap_id = 0, bool exibirApenasDisciplinasPermiteAbonoFalta = false)
        {
            List<sTurmaDisciplina> lista = TUR_TurmaDisciplinaBO.GetSelectBy_TurmaDocente
                (tur_id
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                , VS_MostraFilhosRegencia, VS_MostraRegencia, VS_MostraExperiencia, VS_MostraTerritorio
                , doc_id
                , cap_id
                , ApplicationWEB.AppMinutosCacheLongo).Where(p => p.tud_tipo != (byte)TurmaDisciplinaTipo.DocenciaCompartilhada).ToList();

            if (exibirApenasDisciplinasPermiteAbonoFalta)
            {
                CarregarCombo(lista.Where(p => p.tud_permitirLancarAbonoFalta));
            }
            else
            {
                CarregarCombo(lista);
            }
        }

        /// <summary>
        /// Carrega as turmas de recuperação paralela de acordo com a escola e o calendário.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="cal_id">Id do calendário</param>
        public void CarregarTurmaDisciplinaEletivaAluno(int esc_id, int cal_id)
        {
            CarregarCombo(TUR_TurmaDisciplinaBO.GetSelectEletivaAlunoBy_EscolaCalendario(esc_id, cal_id));
        }

        #endregion

        #region Eventos

        #region Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && VS_TurmaEletiva)
            {
                lblTitulo.Text = GetGlobalResourceObject("UserControl", "UCCTurmaDisciplina.lblTitulo.Text.TurmaEletiva").ToString();
                cpvCombo.ErrorMessage = GetGlobalResourceObject("UserControl", "UCCTurmaDisciplina.cpvCombo.ErrorMessage.TurmaEletiva").ToString();
            }

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