using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Collections.Generic;
using MSTech.CoreSSO.BLL;
using System.Linq;

namespace GestaoEscolar.WebControls.Combos.Novos
{
    public partial class UCCPosicaoDocente : MotherUserControl
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
                {
                    return Convert.ToBoolean(ViewState["MostrarMensagemSelecione"]);
                }

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
        public byte Valor
        {
            get
            {
                if (string.IsNullOrEmpty(ddlCombo.SelectedValue))
                {
                    return 0;
                }

                return Convert.ToByte(ddlCombo.SelectedValue);
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
        /// Inclui a opção "Todos" no combo
        /// </summary>
        public bool IncluiOpcaoTodos
        {
            get
            {
                if (ViewState["IncluiOpcaoTodos"] != null)
                {
                    return Convert.ToBoolean(ViewState["IncluiOpcaoTodos"]);
                }

                return false;
            }

            set
            {
                ViewState["IncluiOpcaoTodos"] = value;
            }
        }

        /// <summary>
        /// Lista de permissões do docente.
        /// </summary>
        public List<sPermissaoDocente> VS_ltPermissao
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissao"] ??
                            (
                                new List<sPermissaoDocente>()
                            )
                        );
            }

            set
            {
                ViewState["VS_ltPermissao"] = value;
            }
        } 

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega a mensagem de selecione de acordo com o parâmetro
        /// </summary>
        private void CarregarMensagemSelecione()
        {
            if (MostrarMensagemSelecione && (ddlCombo.Items.FindByValue(valorSelecione) == null))
            {
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", valorSelecione, true));
            }
        }

        /// <summary>
        /// Inclui a opção "Todos" no combo
        /// </summary>
        private void IncluirOpcaoTodos()
        {
            if (IncluiOpcaoTodos)
            {
                ddlCombo.Items.Insert(0, new ListItem("Todos", "0", true));
            }
        }

        /// <summary>
        /// Carrega todas as posições de docente permitidas pelo parâmetro.
        /// </summary>
        /// <param name="tud_id">Id da disciplina.</param>
        /// <param name="doc_id">Id do docente.</param>
        /// <param name="carregarDocentesComAtrib">Verifica se é pra carregar somente os docentes com atribuição.</param>
        public void CarregarPorParametro(long tud_id = -1, long doc_id = -1)
        {
            try
            {
                int qtdPosicoes = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_DOCENTES_VIGENTES_DISCIPLINA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                bool carregarDocentesComAtrib = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.NAO_MOSTRAR_POSICAO_DOCENTES_PLANEJAMENTO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                ddlCombo.Items.Clear();

                if (tud_id > 0)
                {
                    if (doc_id > 0)
                    {
                        List<KeyValuePair<int, string>> docentePosicao = TUR_TurmaDocenteBO.SelecionaDocentesPosicaoPorDisciplina(tud_id)
                                          .Where(p => VS_ltPermissao.Any(q => q.tdt_posicaoPermissao == (byte)p.Key && q.pdc_permissaoConsulta)).ToList();
                        string nomeDocente;
                        foreach (int i in docentePosicao.Select(p => p.Key))
                        {
                            nomeDocente = docentePosicao.Find(p => p.Key == i).Value;

                            if (carregarDocentesComAtrib && (!string.IsNullOrEmpty(nomeDocente)))
                            {
                                ddlCombo.Items.Insert(0, new ListItem(nomeDocente, i.ToString(), true));
                            }
                            else if (!carregarDocentesComAtrib)
                            {
                                ddlCombo.Items.Insert(0, new ListItem(
                                    string.IsNullOrEmpty(nomeDocente) ? string.Format("Docente {0}", i.ToString()) : nomeDocente,
                                    i.ToString(),
                                    true));
                            }
                        }
                    }
                    else
                    {
                        List<KeyValuePair<int, string>> docentePosicao = TUR_TurmaDocenteBO.SelecionaDocentesPosicaoPorDisciplina(tud_id); ;
                        string nomeDocente;

                        for (int i = qtdPosicoes; i > 0; i--)
                        {
                            nomeDocente = docentePosicao.Find(p => p.Key == i).Value;

                            if (carregarDocentesComAtrib && (!string.IsNullOrEmpty(nomeDocente)))
                            {
                                ddlCombo.Items.Insert(0, new ListItem(nomeDocente, i.ToString(), true));
                            }
                            else if (!carregarDocentesComAtrib)
                            {
                                ddlCombo.Items.Insert(0, new ListItem(
                                    string.IsNullOrEmpty(nomeDocente) ? string.Format("Docente {0}", i.ToString()) : nomeDocente,
                                    i.ToString(),
                                    true));
                            }
                        }
                    }
                }
                else
                {
                    for (int i = qtdPosicoes; i > 0; i--)
                    {
                        ddlCombo.Items.Insert(0, new ListItem(
                            string.Format("Docente {0}", i.ToString()),
                            i.ToString(),
                            true));
                    }
                }

                IncluirOpcaoTodos();
                CarregarMensagemSelecione();
                ddlCombo.AppendDataBoundItems = true;
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
            ddlCombo.AutoPostBack = IndexChanged != null;
            CarregarMensagemSelecione();
        }

        #endregion

        protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
            {
                IndexChanged();
            }
        }

        #endregion
    }
}