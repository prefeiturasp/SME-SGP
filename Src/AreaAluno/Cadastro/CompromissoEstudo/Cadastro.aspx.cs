using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace AreaAluno.Cadastro.CompromissoEstudo
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Id do compromisso de estudo do aluno
        /// </summary>
        public int VS_Cpe_Id
        {
            get
            {
                if (ViewState["VS_Cpe_Id"] == null)
                    return -1;
                return (int)ViewState["VS_Cpe_Id"];
            }
            set
            {
                ViewState["VS_Cpe_Id"] = value;
            }
        }

        /// <summary>
        /// Id da turma
        /// </summary>
        public Int64 VS_Tur_Id
        {
            get
            {
                if (ViewState["VS_Tur_Id"] == null)
                    return -1;
                return (Int64)ViewState["VS_Tur_Id"];
            }
            set
            {
                ViewState["VS_Tur_Id"] = value;
            }
        }

        /// <summary>
        /// Id do calendário
        /// </summary>
        public int VS_Cal_Id
        {
            get
            {
                if (ViewState["VS_Cal_Id"] == null)
                    return -1;
                return (int)ViewState["VS_Cal_Id"];
            }
            set
            {
                ViewState["VS_Cal_Id"] = value;
            }
        }

        /// <summary>
        /// Ano do calendário
        /// </summary>
        public int VS_Cpe_Ano
        {
            get
            {
                if (ViewState["VS_Cpe_Ano"] == null)
                    return -1;
                return (int)ViewState["VS_Cpe_Ano"];
            }
            set
            {
                ViewState["VS_Cpe_Ano"] = value;
            }
        }

        public int VSAnoLetivo
        {
            get
            {
                return Convert.ToInt32((ViewState["VSAnoLetivo"] ?? (ViewState["VSAnoLetivo"] = ACA_CalendarioAnualBO.SelecionaAnoLetivoCorrente(__SessionWEB.__UsuarioWEB.Usuario.ent_id))));
            }
        }

        #endregion

        #region Delegates

        private void UCConfirmacaoOperacao1_ConfimaOperacao()
        {
            Salvar(true);
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Int64 alu_id = __SessionWEB.__UsuarioWEB.alu_id;

                // somente terá acesso se o flag exibirBoletim do cadastro de "Tipo de ciclo" estiver marcado
                if (!ACA_TipoCicloBO.VerificaSeExibeCompromissoAluno(alu_id))
                {
                    lblMensagem.Text = UtilBO.GetErroMessage("Este usuário não tem permissão de acesso a esta página.", UtilBO.TipoMensagem.Alerta);
                    fdsCompromissoEstudo.Visible = false;
                    btnSalvar.Visible = false;
                    return;
                }

                // Carrega combo Bimestre
                string tipo = ((byte)AvaliacaoTipo.Periodica).ToString() + "," + ((byte)AvaliacaoTipo.PeriodicaFinal).ToString();
                string parametroPeriodo = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                DataTable dtCurriculo = ACA_AlunoCurriculoBO.SelecionaDadosUltimaMatricula(alu_id);
                UCComboTipoPeriodoCalendario1.CarregarTipoPeriodoCalendario(Convert.ToInt64(dtCurriculo.Rows[0]["tur_id"]));
                UCComboTipoPeriodoCalendario1._Validator.ValidationGroup = "CompromissoEstudo";
                UCComboTipoPeriodoCalendario1._Validator.Visible = true;
                UCComboTipoPeriodoCalendario1._Label.Text = parametroPeriodo + " *";

                VS_Tur_Id = Convert.ToInt64(dtCurriculo.Rows[0]["tur_id"]);
                VS_Cal_Id = Convert.ToInt32(dtCurriculo.Rows[0]["cal_id"]);
                VS_Cpe_Ano = VSAnoLetivo;

                // Carrega dados para alteração do registro
                if ((PreviousPage != null) && PreviousPage.IsCrossPagePostBack)
                {
                    VS_Cpe_Id = PreviousPage.EditItem;

                    ACA_CompromissoEstudo compromisso = new ACA_CompromissoEstudo
                    {
                        alu_id = alu_id,
                        cpe_id = VS_Cpe_Id
                    };
                    ACA_CompromissoEstudoBO.GetEntity(compromisso);

                    txtOqTenhoFeito.Text = compromisso.cpe_atividadeFeita;
                    txtOqPretendoFazer.Text = compromisso.cpe_atividadePretendeFazer;
                    
                    // Seleciona o bimestre
                    ListItem itemBimestre = UCComboTipoPeriodoCalendario1._Combo.Items.FindByValue(compromisso.tpc_id.ToString());

                    if (itemBimestre != null)
                    {
                        UCComboTipoPeriodoCalendario1._Combo.SelectedValue = itemBimestre.Value;
                        UCComboTipoPeriodoCalendario1._Combo.Enabled = false;
                    }
                    else
                    {
                        ACA_TipoPeriodoCalendario tpc = ACA_TipoPeriodoCalendarioBO.GetEntity(new ACA_TipoPeriodoCalendario { tpc_id = compromisso.tpc_id });
                        itemBimestre = new ListItem(tpc.tpc_nome, tpc.tpc_id.ToString());
                        UCComboTipoPeriodoCalendario1._Combo.Items.Add(itemBimestre);
                        UCComboTipoPeriodoCalendario1._Combo.SelectedValue = itemBimestre.Value;
                        UCComboTipoPeriodoCalendario1._Combo.Enabled = false;
                    }
                }
            }
            UCConfirmacaoOperacao1.ConfimaOperacao += UCConfirmacaoOperacao1_ConfimaOperacao;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar(false);
            }
        }

        #endregion

        #region Métodos

        private void Salvar(bool confirmacao)
        {
            try
            {
                if (VS_Cpe_Ano != VSAnoLetivo)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.SalvarApenasAnoCorrente"), UtilBO.TipoMensagem.Alerta);
                    return;
                }
                if (!confirmacao)
                {
                    UCConfirmacaoOperacao1.Mensagem = "Estas informações serão compartilhadas com a Escola e impressas no boletim. Confirmar?";
                    UCConfirmacaoOperacao1.ObservacaoVisivel = false;
                    UCConfirmacaoOperacao1.ObservacaoObrigatorio = false;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmaMovimentacao", "$(document).ready(function(){ $('#divConfirmacao').dialog('open'); });", true);
                }
                else
                {
                    // Busca todos os eventos do calendário
                    DataTable dtEventos = ACA_EventoBO.Select_TodosEventosPorTipo_CalendarioPeriodo(VS_Cal_Id, VS_Tur_Id, UCComboTipoPeriodoCalendario1.Valor, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    // Verifica se existe um evento cadastrado
                    if (dtEventos.Rows.Count == 0)
                    {
                        lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.BimetreNaoAbertoLancamento"), UtilBO.TipoMensagem.Alerta);
                        return;
                    }
                    else
                    {
                        // Se não exister nenhum calendário cadastrado, ou exister e a data atual é menor que a data de início
                        bool existeBimestreFuturo = dtEventos.Rows.Cast<DataRow>().Any(row => DateTime.Now.Date < Convert.ToDateTime(row["evt_dataInicio"].ToString()));

                        if (existeBimestreFuturo)
                        {
                            lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.BimetreNaoAbertoLancamento"), UtilBO.TipoMensagem.Alerta);
                            return;
                        }

                        // Verifica se já passou a data final do evento do calendário para fechamento do bimestre vigente
                        bool existeBimestreFechado = dtEventos.Rows.Cast<DataRow>().Any(row => DateTime.Now.Date >= Convert.ToDateTime(row["evt_dataInicio"].ToString()) && DateTime.Now.Date <= Convert.ToDateTime(row["evt_dataFim"].ToString()) );

                        if (!existeBimestreFechado)
                        {
                            lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.BimetreFechadoLancamento"), UtilBO.TipoMensagem.Alerta);
                            return;
                        }
                    }

                    Int64 alu_id = __SessionWEB.__UsuarioWEB.alu_id;

                    // Se for um compromisso de estudo que estava excluído e agora foi incluído novamente, altera situação e os dados
                    bool compromissoEstudoAlteraSituacao = false;

                    // Verifica se já existe um compromisso de estudo pra esse bimestre desse aluno
                    // Busca todos os compromissos de estudo do aluno
                    DataTable dtTodosCompromissosAluno = ACA_CompromissoEstudoBO.SelectSituacaoTodosCompromissoAlunoBy_alu_id(alu_id);

                    bool existeBimestreAtivo = dtTodosCompromissosAluno.Rows.Cast<DataRow>().Any(row => row["tpc_id"].ToString().Equals(UCComboTipoPeriodoCalendario1.Valor.ToString()) &&
                                                                                                    !row["cpe_id"].ToString().Equals(VS_Cpe_Id.ToString()) &&
                                                                                                    row["cpe_ano"].ToString().Equals(VS_Cpe_Ano.ToString()) &&
                                                                                                    !row["cpe_situacao"].ToString().Equals("3")
                                                                                    );

                    // Se existir e tiver com situação ativo, mostra a msg!
                    if (existeBimestreAtivo)
                    {
                        lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.IncluirExisteRegistro"), UtilBO.TipoMensagem.Alerta);
                        return;
                    }
                    else if (VS_Cpe_Id <= 0) // Senão verifica se existe com situação excluído, pra alterar a situação pra ativo, ao invés de salvar um novo registro
                    {
                        foreach (DataRow row in dtTodosCompromissosAluno.Rows)
                        {
                            if (Convert.ToInt32(row["tpc_id"]) == UCComboTipoPeriodoCalendario1.Valor &&
                                Convert.ToInt32(row["cpe_ano"]) == VS_Cpe_Ano &&
                                Convert.ToInt32(row["cpe_situacao"]) == 3)
                            {
                                compromissoEstudoAlteraSituacao = true;
                                VS_Cpe_Id = Convert.ToInt32(row["cpe_id"]);
                                break;
                            }
                        }
                    }

                    ACA_CompromissoEstudo compromisso = new ACA_CompromissoEstudo
                    {
                        alu_id = __SessionWEB.__UsuarioWEB.alu_id,
                        cpe_id = -1,
                        cpe_atividadeFeita = txtOqTenhoFeito.Text,
                        cpe_atividadePretendeFazer = txtOqPretendoFazer.Text,
                        cpe_situacao = 1,
                        cpe_dataAlteracao = DateTime.Now,
                        cpe_dataCriacao = DateTime.Now,
                        cpe_ano = VS_Cpe_Ano,
                        tpc_id = UCComboTipoPeriodoCalendario1.Valor,
                        IsNew = true
                    };

                    // Se já existir, altera
                    if (VS_Cpe_Id > 0)
                    {
                        compromisso.cpe_id = VS_Cpe_Id;
                        compromisso.IsNew = false;
                    }

                    if (ACA_CompromissoEstudoBO.Save(compromisso))
                    {
                        if (VS_Cpe_Id <= 0 || compromissoEstudoAlteraSituacao)
                        {
                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "Área do aluno. cpe_id: " + Convert.ToString(compromisso.cpe_id));
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.IncluirSucesso"), UtilBO.TipoMensagem.Sucesso);
                        }
                        else
                        {
                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Área do aluno. cpe_id: " + Convert.ToString(compromisso.cpe_id));
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.AlterarErro"), UtilBO.TipoMensagem.Sucesso);
                        }

                        Response.Redirect("~/Cadastro/CompromissoEstudo/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("AreaAluno", "Cadastro.CompromissoEstudo.Cadastro.SalvarSucesso"), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}