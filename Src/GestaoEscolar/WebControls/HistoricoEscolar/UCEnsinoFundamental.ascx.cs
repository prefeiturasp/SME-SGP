using GestaoEscolar.WebControls.EscolaOrigem;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.HistoricoEscolar
{
    public partial class UCEnsinoFundamental : MotherUserControl
    {
        #region Propiedades

        private SortedList<int, ACA_AlunoHistoricoBO.StructHistorico> HistoricosAluno;
        private DataTable dtHistAluno;
        private object dtTipoCurriculoPeriodo;
        private List<ACA_AlunoHistoricoBO.StructResFinalHistorico> lsStructResFinalHistorico;
        private string resultadoText;
        private short resultado;

        ACA_FormatoAvaliacao fav = new ACA_FormatoAvaliacao();
        ACA_EscalaAvaliacao esa = new ACA_EscalaAvaliacao();

        private DataTable _dtPareceresFinal;
        /// <summary>
        /// DataTable de pareceres final.
        /// </summary>
        private DataTable DtPareceresFinal
        {
            get
            {
                return _dtPareceresFinal ??
                       (_dtPareceresFinal = ACA_TipoResultadoBO.SELECT_By_TipoLancamento((byte)EnumTipoLancamento.HistoricoEscolar));
            }
        }

        private DataTable dtHistoricoAluno
        {
            get
            {
                if (dtHistAluno != null)
                    return dtHistAluno;
                dtHistAluno = new DataTable();
                dtHistAluno.Columns.Add("alh_id");
                dtHistAluno.Columns.Add("alh_anoLetivo");
                dtHistAluno.Columns.Add("alh_resultadoDescricaoText");
                dtHistAluno.Columns.Add("eco_nome");
                dtHistAluno.Columns.Add("crp_descricao");
                dtHistAluno.Columns.Add("permiteEditar");

                return dtHistAluno;
            }
            set 
            { 
                dtHistAluno = value; 
            }
        }

        /// <summary>
        /// Retorna se deve-se validar o histórico escolar.
        /// </summary>
        protected bool RealizaValidacaoHistoricoEscolar
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.REALIZAR_VALIDACOES_HISTORICO_ESCOLAR, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        private List<CFG_PermissaoModuloOperacao> VS_Permissoes
        {
            get
            {
                if (ViewState["VS_Permissoes"] != null)
                    return (List<CFG_PermissaoModuloOperacao>)ViewState["VS_Permissoes"];

                return new List<CFG_PermissaoModuloOperacao>();
            }
            set
            {
                ViewState["VS_Permissoes"] = value;
            }
        }

        public bool VS_permiteEditar
        {
            get
            {
                return ViewState["VS_permiteEditar"] == null ? false : Convert.ToBoolean(ViewState["VS_permiteEditar"]);
            }
            set
            {
                ViewState["VS_permiteEditar"] = value;
            }
        }

        public int VS_esc_id
        {
            get
            {
                return ViewState["VS_esc_id"] == null ? 0 : Convert.ToInt32(ViewState["VS_esc_id"]);
            }
            set
            {
                ViewState["VS_esc_id"] = value;
            }
        }

        public int VS_uni_id
        {
            get
            {
                return ViewState["VS_uni_id"] == null ? 0 : Convert.ToInt32(ViewState["VS_uni_id"]);
            }
            set
            {
                ViewState["VS_uni_id"] = value;
            }
        }

        public Int64 VS_alu_id
        {
            get
            {
                return ViewState["VS_alu_id"] == null ? 0 : Convert.ToInt64(ViewState["VS_alu_id"]);
            }
            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        public int VS_alh_id
        {
            get
            {
                return ViewState["VS_alh_id"] == null ? 0 : Convert.ToInt32(ViewState["VS_alh_id"]);
            }
            set
            {
                ViewState["VS_alh_id"] = value;
            }
        }

        public int VS_ahp_id
        {
            get
            {
                return ViewState["VS_ahp_id"] == null ? 0 : Convert.ToInt32(ViewState["VS_ahp_id"]);
            }
            set
            {
                ViewState["VS_ahp_id"] = value;
            }
        }

        public int VS_qtdMaxProjetos
        {
            get
            {
                if (ViewState["VS_qtdMaxProjetos"] == null)
                    ViewState["VS_qtdMaxProjetos"] = CFG_HistoricoPedagogicoBO.SelecionaUltimoAno().chp_qtdMaxProjeto;
                return Convert.ToInt32(ViewState["VS_qtdMaxProjetos"]);
            }
            set
            {
                ViewState["VS_qtdMaxProjetos"] = value;
            }
        }

        /// <summary>
        /// ID da escola origem
        /// </summary>
        private string VS_eco_id
        {
            get
            {
                if (ViewState["VS_eco_id"] != null)
                {
                    return ViewState["VS_eco_id"].ToString();
                }

                return "-1";
            }

            set
            {
                ViewState["VS_eco_id"] = value;
            }
        }

        /// <summary>
        /// Retorna o valor da escola origem.
        /// </summary>
        public bool VS_novaEscolaOrigem
        {
            get
            {
                if (ViewState["VS_novaEscolaOrigem"] != null)
                {
                    return Convert.ToBoolean(ViewState["VS_novaEscolaOrigem"]);
                }

                return false;
            }

            set
            {
                ViewState["VS_novaEscolaOrigem"] = value;
            }
        }

        /// <summary>
        /// List de Histórico do aluno.
        /// </summary>
        private ACA_AlunoHistoricoBO.StructHistorico VS_Historico
        {
            get
            {
                if (ViewState["VS_Historico"] == null)
                    ViewState["VS_Historico"] = new ACA_AlunoHistoricoBO.StructHistorico();

                return (ACA_AlunoHistoricoBO.StructHistorico)ViewState["VS_Historico"];
            }

            set
            {
                ViewState["VS_Historico"] = value;
            }
        }

        public string message
        {
            set 
            {
                lblMessage.Text = value;
            }
        }

        #endregion
        
        #region Delegate

        public delegate void onClickVisualizar();
        public event onClickVisualizar clickVisualizar;

        public delegate void onClickVoltar();
        public event onClickVoltar clickVoltar;

        #endregion

        #region Métodos

        private void VerificaPermissoes()
        {
            List<CFG_PermissaoModuloOperacaoBO.Operacao> list = new List<CFG_PermissaoModuloOperacaoBO.Operacao> {
                CFG_PermissaoModuloOperacaoBO.Operacao.HistoricoEscolarEnsinoFundamental,
            };
            VS_Permissoes = CFG_PermissaoModuloOperacaoBO.VerificaPermissao(__SessionWEB.__UsuarioWEB.GrupoPermissao.sis_id,
                                                                            __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                                                                            //__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                            __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                                                                            list);

            btnAddNovoHistorico.Visible = VS_Permissoes.FirstOrDefault().pmo_permissaoInclusao;
            btnSalvarAddHistorico.Visible = btnSalvarProjAtivCompl.Visible = VS_Permissoes.FirstOrDefault().pmo_permissaoEdicao ||
                                                                             VS_Permissoes.FirstOrDefault().pmo_permissaoInclusao;
        }

        public void CarregarAluno()
        {
            try
            {
                VerificaPermissoes();
                HistoricosAluno = ACA_AlunoHistoricoBO.CarregarPorAluno(VS_alu_id);
                CarregarHistoricos();
                CarregarResultadoFinal();
                updEnsinoFundamental.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroCarregar"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        private void CarregarProjetos()
        {
            DataTable dtProjAluno = ACA_AlunoHistoricoProjetoBO.SelectBy_Aluno(VS_alu_id);

            grvProjAtivComplementar.DataSource = dtProjAluno;
            grvProjAtivComplementar.DataBind();

            btnAddProjAtivCompl.Visible = VS_Permissoes.FirstOrDefault().pmo_permissaoInclusao && dtProjAluno.Rows.Count < VS_qtdMaxProjetos;
        }

        public void CarregarHistoricos()
        {
            foreach (var hist in HistoricosAluno)
            {
                DataRow add = dtHistoricoAluno.NewRow();
                add["alh_id"] = hist.Value.entHistorico.alh_id;
                add["alh_anoLetivo"] = hist.Value.entHistorico.alh_anoLetivo;
                add["alh_resultadoDescricaoText"] = hist.Value.alh_resultadoDescricaoText;
                add["eco_nome"] = hist.Value.eco_nome;
                add["crp_descricao"] = hist.Value.crp_descricao;
                add["permiteEditar"] = true;
                dtHistoricoAluno.Rows.Add(add);
            }

            grvHistorico.DataSource = dtHistoricoAluno;
            grvHistorico.DataBind();
        }

        public void CarregarResultadoFinal()
        {
            lsStructResFinalHistorico = ACA_AlunoHistoricoBO.CarregarResFinal(VS_alu_id, DateTime.Today.Year, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_FUNDAMENTAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

            fdsResFinal.Visible = lsStructResFinalHistorico.Any();

            List<ACA_AlunoHistoricoBO.StructResFinalHistorico> lstTCP = new List<ACA_AlunoHistoricoBO.StructResFinalHistorico>();
            foreach (var tcp in (from resultado in lsStructResFinalHistorico
                                 group resultado by new { 
                                                            resultado.tcp_id,
                                                            resultado.tcp_descricao,
                                                            resultado.fav_id
                                                        }
                                 into res
                                 select res))
                lstTCP.Add(new ACA_AlunoHistoricoBO.StructResFinalHistorico
                            {
                                tcp_id = tcp.Key.tcp_id,
                                tcp_descricao = tcp.Key.tcp_descricao,
                                fav_id = tcp.Key.fav_id
                            });
            dtTipoCurriculoPeriodo = lstTCP;

            List<ACA_AlunoHistoricoBO.StructResFinalHistorico> lstTDS = new List<ACA_AlunoHistoricoBO.StructResFinalHistorico>();
            foreach (var tcp in (from resultado in lsStructResFinalHistorico
                                 where resultado.tds_tipo == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.Disciplina &&
                                 resultado.tds_id > 0
                                 group resultado by new
                                 {
                                     resultado.tds_nome,
                                     resultado.tds_id,
                                 }
                                     into res
                                     select res))
                lstTDS.Add(new ACA_AlunoHistoricoBO.StructResFinalHistorico
                {
                    tds_id = tcp.Key.tds_id,
                    tds_nome = tcp.Key.tds_nome,
                });
            foreach (var tcp in (from resultado in lsStructResFinalHistorico
                                 where resultado.tds_id == 0
                                 group resultado by new
                                 {
                                     resultado.alh_id,
                                     resultado.ahd_id,
                                     resultado.tds_nome
                                 }
                         into res
                                 select res))
                if (!String.IsNullOrEmpty(tcp.Key.tds_nome))
                {
                    lstTDS.Add(new ACA_AlunoHistoricoBO.StructResFinalHistorico
                    {
                        alh_id = tcp.Key.alh_id,
                        ahd_id = tcp.Key.ahd_id,
                        tds_nome = tcp.Key.tds_nome,
                    });
                }

            rptResFinal.DataSource = lstTDS;
            rptResFinal.DataBind();

            List<ACA_AlunoHistoricoBO.StructResFinalHistorico> lstEC = new List<ACA_AlunoHistoricoBO.StructResFinalHistorico>();
            foreach (var tcp in (from resultado in lsStructResFinalHistorico
                                 where resultado.tds_tipo == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.EnriquecimentoCurricular
                                 group resultado by new
                                 {
                                     resultado.tds_id,
                                     resultado.tds_nome
                                 }
                                     into res
                                     select res))
                lstEC.Add(new ACA_AlunoHistoricoBO.StructResFinalHistorico
                {
                    tds_id = tcp.Key.tds_id,
                    tds_nome = tcp.Key.tds_nome,
                });

            rptEnrCurricular.DataSource = lstEC;
            rptEnrCurricular.DataBind();

            List<ACA_AlunoHistoricoBO.StructResFinalHistorico> lstPAC = new List<ACA_AlunoHistoricoBO.StructResFinalHistorico>();
            foreach (var tcp in (from resultado in lsStructResFinalHistorico
                                 where resultado.ahp_id > 0
                                 group resultado by new
                                 {
                                     resultado.ahp_id,
                                     resultado.ahp_nome
                                 }
                                     into res
                                     select res))
                lstPAC.Add(new ACA_AlunoHistoricoBO.StructResFinalHistorico
                {
                    ahp_id = tcp.Key.ahp_id,
                    ahp_nome = tcp.Key.ahp_nome,
                });

            rptProjeto.Visible = lstPAC.Any();

            rptProjeto.DataSource = lstPAC;
            rptProjeto.DataBind();

            List<string> lstP = new List<string>();
            lstP.Add("");

            rptParecer.DataSource = lstP;
            rptParecer.DataBind();

            rptResFinal.Visible = rptResFinal.Items.Count > 0;
        }

        private void BloqueiaCampos()
        {
            UCComboTipoCurriculoPeriodo1.PermiteEditar = txtCargaHoraria.Enabled =
                rblOrigemEscola.Enabled = UCComboUAEscola.PermiteAlterarCombos = btnEscolaOrigem.Enabled = false;
        }

        public void CarregarResultadoFinalAddHistorico(SortedList<int, ACA_AlunoHistoricoBO.StructHistorico> alh, int anoLetivo)
        {
            try
            {
                VS_alh_id = alh.FirstOrDefault().Value.entHistorico.alh_id;
                int tcp_id = alh.FirstOrDefault().Value.tcp_id;

                if (UCComboTipoCurriculoPeriodo1.Combo.Items.FindByValue(tcp_id.ToString()) == null)
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.AnoHistoricoInvalido"));

                UCComboTipoCurriculoPeriodo1.Valor = tcp_id;
                UCComboTipoCurriculoPeriodo1.PermiteEditar = false;
                divResFinal.Visible = true;
                
                txtCargaHoraria.Text = alh.FirstOrDefault().Value.entHistorico.alh_cargaHorariaBaseNacional.ToString();

                if (alh.FirstOrDefault().Value.entHistorico.esc_id > 0)
                {
                    rblOrigemEscola.SelectedValue = "1";
                    CarregaCamposDentroRede();
                    ESC_Escola esc = new ESC_Escola
                                         {
                                             esc_id = alh.FirstOrDefault().Value.entHistorico.esc_id
                                         };
                    ESC_EscolaBO.GetEntity(esc);
                    if (UCComboUAEscola.VisibleUA)
                    {
                    UCComboUAEscola.Uad_ID = esc.uad_idSuperiorGestao;
                    }
                    UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();
                    UCComboUAEscola.SelectedValueEscolas = new[] 
                                                           { 
                                                               alh.FirstOrDefault().Value.entHistorico.esc_id, 
                                                               alh.FirstOrDefault().Value.entHistorico.uni_id 
                                                           };
                    UCComboUAEscola.PermiteAlterarCombos = true;
                }
                else
                {
                    rblOrigemEscola.SelectedValue = "2";
                    CarregaCamposForaRede();
                    txtEscolaOrigem.Text = alh.FirstOrDefault().Value.eco_nome;
                }

                if (!VS_permiteEditar)
                    BloqueiaCampos();

                UCAddResultadoFinal1.VS_alu_id = VS_alu_id;
                UCAddResultadoFinal1.Carregar(anoLetivo, tcp_id, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_FUNDAMENTAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                UCAddResultadoFinal1.CarregarDados(alh, VS_permiteEditar);
            }
            catch (ValidationException ex)
            {
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroCarregarHistoricoAluno"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        private void CarregaCamposDentroRede()
        {
            try
            {
                divDentroRede.Visible = true;
                divForaRede.Visible = false;

                UCComboUAEscola.Inicializar();

                UCComboUAEscola.SelectedIndexUa = 0;
                UCComboUAEscola.SelectedIndexEscolas = 0;

                UCComboUAEscola.ObrigatorioUA = UCComboUAEscola.VisibleUA; 
                UCComboUAEscola.ObrigatorioEscola = UCComboUAEscola.VisibleEscolas;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroCarregarDentroRede"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        private void CarregaCamposForaRede()
        {
            try
            {
                divForaRede.Visible = true;
                divDentroRede.Visible = false;
                btnEscolaOrigem.Enabled = true;

                txtEscolaOrigem.Text = "";

                UCComboUAEscola.ObrigatorioUA = UCComboUAEscola.ObrigatorioEscola = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroCarregarForaRede"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        private short ResHistorico()
        {
            if (resultado == (short)ResultadoHistorico.Aprovado)
                return (short)TipoResultado.Aprovado;
            else if (resultado == (short)ResultadoHistorico.Reprovado)
                return (short)TipoResultado.Reprovado;
            else if (resultado == (short)ResultadoHistorico.ReprovadoFrequencia)
                return (short)TipoResultado.ReprovadoFrequencia;
            else
                return 0;
        }

        private short ResHistoricoDisc()
        {
            if (resultado == (short)ResultadoHistoricoDisciplina.Aprovado)
                return (short)TipoResultado.Aprovado;
            else if (resultado == (short)ResultadoHistoricoDisciplina.Reprovado)
                return (short)TipoResultado.Reprovado;
            else if (resultado == (short)ResultadoHistoricoDisciplina.ReprovadoFrequencia)
                return (short)TipoResultado.ReprovadoFrequencia;
            else
                return 0;
        }

        private void CarregarDadosDisciplinas()
        {
            resultado = UCAddResultadoFinal1.RetornaParecerConclusivo();
            resultadoText = UCAddResultadoFinal1.RetornaParecerConclusivoText();

        }

        /// <summary>
        /// Adiciona disciplina no histórico do aluno.
        /// </summary>
        private void AdicionaHistoricoDisciplina(int anoLetivo, bool dentroRede)
        {
            ACA_AlunoHistoricoBO.StructHistorico historico = VS_Historico;
            List<ACA_AlunoHistoricoBO.StructHistoricoDisciplina> lt = new List<ACA_AlunoHistoricoBO.StructHistoricoDisciplina>();
           
            foreach (ACA_AlunoHistoricoBO.StructCompCurricular resFinal in UCAddResultadoFinal1.RetornaResultadoFinal())
            {
                ACA_AlunoHistoricoBO.StructHistoricoDisciplina historicoDisciplina = new ACA_AlunoHistoricoBO.StructHistoricoDisciplina();
                historicoDisciplina.entDisciplina = new ACA_AlunoHistoricoDisciplina();
                historicoDisciplina.entDisciplina.alu_id = VS_alu_id;
                historicoDisciplina.entDisciplina.alh_id = VS_alh_id;
                historicoDisciplina.entDisciplina.ahd_id = resFinal.ahd_id;

                // Atualiza disciplina no histórico do aluno
                historicoDisciplina.tds_id = resFinal.tds_id.ToString();
                historicoDisciplina.entDisciplina.tds_id = resFinal.tds_id;
                historicoDisciplina.entDisciplina.ahd_disciplina = resFinal.tds_nome;
                historicoDisciplina.entDisciplina.ahd_avaliacao = resFinal.nota.Replace('.', ',');
                historicoDisciplina.entDisciplina.ahd_frequencia = resFinal.frequencia.Equals("-1") ? "" : resFinal.frequencia;
                historicoDisciplina.entDisciplina.ahd_indicacaoDependencia = false;

                lt.Add(historicoDisciplina);
            }

            foreach (ACA_AlunoHistoricoBO.StructEnrCurricular enrCurricular in UCAddResultadoFinal1.RetornaEnriquecimentoCurricular())
            {
                ACA_AlunoHistoricoBO.StructHistoricoDisciplina historicoDisciplina = new ACA_AlunoHistoricoBO.StructHistoricoDisciplina();
                historicoDisciplina.entDisciplina = new ACA_AlunoHistoricoDisciplina();
                historicoDisciplina.entDisciplina.alu_id = VS_alu_id;
                historicoDisciplina.entDisciplina.alh_id = VS_alh_id;
                historicoDisciplina.entDisciplina.ahd_id = enrCurricular.ahd_id;

                // Atualiza disciplina no histórico do aluno
                historicoDisciplina.tds_id = enrCurricular.tds_id.ToString();
                historicoDisciplina.entDisciplina.tds_id = enrCurricular.tds_id;
                historicoDisciplina.entDisciplina.ahd_disciplina = enrCurricular.tds_nome;

                historicoDisciplina.entDisciplina.ahd_resultado = 0;
                if (!string.IsNullOrEmpty(enrCurricular.frequencia) && !enrCurricular.frequencia.Equals("-1"))
                    historicoDisciplina.entDisciplina.ahd_resultado = Convert.ToByte(enrCurricular.frequencia);
                historicoDisciplina.entDisciplina.ahd_indicacaoDependencia = false;

                lt.Add(historicoDisciplina);
            }

            foreach (ACA_AlunoHistoricoBO.StructProjAtivComplementar projAtivComplementar in UCAddResultadoFinal1.RetornaProjAtivComplementares())
            {
                if (projAtivComplementar.frequencia.Equals("-1"))
                    continue;

                ACA_AlunoHistoricoBO.StructHistoricoDisciplina historicoDisciplina = new ACA_AlunoHistoricoBO.StructHistoricoDisciplina();
                historicoDisciplina.entDisciplina = new ACA_AlunoHistoricoDisciplina();
                historicoDisciplina.entDisciplina.alu_id = VS_alu_id;
                historicoDisciplina.entDisciplina.alh_id = VS_alh_id;
                historicoDisciplina.entDisciplina.ahd_id = projAtivComplementar.ahd_id;

                // Atualiza disciplina no histórico do aluno
                historicoDisciplina.entDisciplina.ahp_id = projAtivComplementar.ahp_id;
                historicoDisciplina.entDisciplina.ahd_disciplina = projAtivComplementar.ahp_nome;
                //historicoDisciplina.entDisciplina.ahd_frequencia = projAtivComplementar.frequencia.Equals("-1") ? "" : projAtivComplementar.frequencia;
                historicoDisciplina.entDisciplina.ahd_resultado = 0;
                if (!string.IsNullOrEmpty(projAtivComplementar.frequencia) && !projAtivComplementar.frequencia.Equals("-1"))
                    historicoDisciplina.entDisciplina.ahd_resultado = Convert.ToByte(projAtivComplementar.frequencia);
                historicoDisciplina.entDisciplina.ahd_indicacaoDependencia = false;

                lt.Add(historicoDisciplina);
            }

            if (RealizaValidacaoHistoricoEscolar)
            {
                List<string> listaErros = new List<string>();

                ACA_AlunoHistoricoBO.ValidarDisciplinas(lt, anoLetivo, ref listaErros);

                if (dentroRede)
                {
                    ACA_AlunoHistoricoBO.ValidarDisciplinasDentroRede(lt, anoLetivo, ref listaErros, null);
                }

                if (listaErros.Count > 0)
                {
                    throw new ValidationException(string.Join("<BR/>", listaErros.ToArray()));
                }
            }

            historico.ltDisciplina = new List<ACA_AlunoHistoricoBO.StructHistoricoDisciplina>();
            historico.ltDisciplina.AddRange(lt);

            VS_Historico = historico;
        }

        /// <summary>
        /// Inclui histórico do aluno.
        /// </summary>
        private void IncluiHistorico(int anoLetivo, bool dentroRede)
        {
            ACA_AlunoHistoricoBO.StructHistorico historico = new ACA_AlunoHistoricoBO.StructHistorico();
            bool existeEndereco = false;

            CFG_HistoricoPedagogico cfg = CFG_HistoricoPedagogicoBO.SelecionaByAno(anoLetivo);
            int[] dadosCurso = ACA_CurriculoPeriodoBO.SelecionaPorCursoHistoricoTipoCurriculoPeriodo(cfg.cur_idFundamentalPadrao, UCComboTipoCurriculoPeriodo1.Valor, anoLetivo);

            // Configura entidade ACA_AlunoHistorico
            ACA_AlunoHistorico entityAlunoHistorico = new ACA_AlunoHistorico
            {
                alh_id = VS_alh_id,
                mtu_id = -1,
                alh_anoLetivo = anoLetivo,
                esc_id = dentroRede && UCComboUAEscola.Esc_ID > 0 ? UCComboUAEscola.Esc_ID : -1,
                uni_id = dentroRede && UCComboUAEscola.Uni_ID > 0 ? UCComboUAEscola.Uni_ID : -1,
                cur_id = dadosCurso[0],
                crr_id = dadosCurso[1],
                crp_id = dadosCurso[2],
                alh_resultado = ResHistorico(),
                alh_resultadoDescricao = resultadoText,
                alh_tipoControleNotas = UCAddResultadoFinal1.VS_fav_tipo,
                alh_cargaHorariaBaseNacional = Convert.ToInt32(txtCargaHoraria.Text),
                IsNew = true
            };

            if (RealizaValidacaoHistoricoEscolar)
            {
                List<string> listaErros = new List<string>();

                ACA_AlunoHistoricoBO.ValidaGlobal(entityAlunoHistorico, ref listaErros);

                if (dentroRede)
                {
                    ACA_AlunoHistoricoBO.ValidaGlobalDentroRede(entityAlunoHistorico, ref listaErros, null);
                }

                if (listaErros.Count > 0)
                {
                    throw new ValidationException(string.Join("<BR/>", listaErros.ToArray()));
                }
            }

            ACA_AlunoEscolaOrigem entityAlunoEscolaOrigem = new ACA_AlunoEscolaOrigem();
            if (!dentroRede)
            {
                // Configura entidade ACA_AlunoEscolaOrigem
                entityAlunoEscolaOrigem = new ACA_AlunoEscolaOrigem
                {
                    eco_id = string.IsNullOrEmpty(VS_eco_id) ? -1 : Convert.ToInt64(VS_eco_id),
                    tre_id = UCComboTipoRedeEnsino1.Valor,
                    eco_nome = txtEscolaOrigem.Text,
                    eco_codigoInep = txtCodigoInepEscolaOrigem.Text,
                    eco_numero = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["numero"].ToString() : string.Empty,
                    eco_complemento = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["complemento"].ToString() : string.Empty,
                    cid_id = string.IsNullOrEmpty(txtCid_idMunicipioAluno.Value) ? Guid.Empty : new Guid(txtCid_idMunicipioAluno.Value),
                    unf_id = ddlEstado.SelectedValue == "-1" ? Guid.Empty : new Guid(ddlEstado.SelectedValue)
                };
            }

            historico.entHistorico = entityAlunoHistorico;
            historico.entEscolaOrigem = entityAlunoEscolaOrigem;
            historico.eco_nome = dentroRede ? UCComboUAEscola.ValorComboEscola : txtEscolaOrigem.Text;
            historico.alh_resultadoDescricao = resultadoText;

            VS_Historico = historico;
        }

        /// <summary>
        /// Altera histórico do aluno.
        /// </summary>
        private void AlteraHistorico(int anoLetivo, bool dentroRede)
        {
            bool existeEndereco = false;

            ACA_AlunoHistoricoBO.StructHistorico historico = VS_Historico;

            // Configura entidade ACA_AlunoHistorico
            historico.entHistorico.alh_anoLetivo = anoLetivo;
            historico.entHistorico.alh_resultado = ResHistorico();
            historico.entHistorico.alh_resultadoDescricao = resultadoText;

            CFG_HistoricoPedagogico cfg = CFG_HistoricoPedagogicoBO.SelecionaByAno(anoLetivo);
            int[] dadosCurso = ACA_CurriculoPeriodoBO.SelecionaPorCursoHistoricoTipoCurriculoPeriodo(cfg.cur_idFundamentalPadrao, UCComboTipoCurriculoPeriodo1.Valor, anoLetivo);

            historico.entHistorico.cur_id = dadosCurso[0];
            historico.entHistorico.crr_id = dadosCurso[1];
            historico.entHistorico.crp_id = dadosCurso[2];

            if (!dentroRede)
            {
                // Configura entidade ACA_AlunoEscolaOrigem
                if (historico.entHistorico.esc_id > 0)
                {
                    historico.entHistorico.esc_id = -1;
                    historico.entHistorico.uni_id = -1;
                }

                historico.entEscolaOrigem.eco_id = string.IsNullOrEmpty(VS_eco_id) ? -1 : Convert.ToInt64(VS_eco_id);
                historico.entEscolaOrigem.tre_id = UCComboTipoRedeEnsino1.Valor;
                historico.entEscolaOrigem.eco_nome = txtEscolaOrigem.Text;
                historico.entEscolaOrigem.eco_codigoInep = txtCodigoInepEscolaOrigem.Text;
                historico.entEscolaOrigem.eco_numero = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["numero"].ToString() : string.Empty;
                historico.entEscolaOrigem.eco_complemento = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["complemento"].ToString() : string.Empty;
                historico.entEscolaOrigem.cid_id = string.IsNullOrEmpty(txtCid_idMunicipioAluno.Value) ? Guid.Empty : new Guid(txtCid_idMunicipioAluno.Value);
                historico.entEscolaOrigem.unf_id = ddlEstado.SelectedValue == "-1" ? Guid.Empty : new Guid(ddlEstado.SelectedValue);
                historico.eco_nome = txtEscolaOrigem.Text;
            }
            else
            {
                if (historico.entEscolaOrigem.eco_id > 0)
                {
                    historico.entEscolaOrigem.eco_id = -1;
                    historico.entEscolaOrigem.tre_id = -1;
                    historico.entEscolaOrigem.eco_nome = string.Empty;
                    historico.entEscolaOrigem.eco_codigoInep = string.Empty;
                    historico.entEscolaOrigem.eco_numero = string.Empty;
                    historico.entEscolaOrigem.eco_complemento = string.Empty;
                }

                historico.entHistorico.esc_id = UCComboUAEscola.Esc_ID;
                historico.entHistorico.uni_id = UCComboUAEscola.Uni_ID;
                historico.eco_nome = UCComboUAEscola.ValorComboEscola;
            }

            historico.entHistorico.alh_tipoControleNotas = UCAddResultadoFinal1.VS_fav_tipo;
            historico.entHistorico.alh_cargaHorariaBaseNacional = Convert.ToInt32(txtCargaHoraria.Text);
            historico.alh_resultadoDescricao = resultadoText;

            if (RealizaValidacaoHistoricoEscolar)
            {
                List<string> listaErros = new List<string>();
                ACA_AlunoHistorico entityAlunoHistorico = historico.entHistorico;

                ACA_AlunoHistoricoBO.ValidaGlobal(entityAlunoHistorico, ref listaErros);

                if (dentroRede)
                {
                    ACA_AlunoHistoricoBO.ValidaGlobalDentroRede(entityAlunoHistorico, ref listaErros, null);
                }

                if (listaErros.Count > 0)
                {
                    throw new ValidationException(string.Join("<BR/>", listaErros.ToArray()));
                }
            }

            VS_Historico = historico;
        }

        private void SalvarHistorico()
        {
            try
            {
                bool edicao = VS_alh_id > 0;

                int anoLetivo = 0;
                //if (txtAnoConclusao.Text.Length != 4 || !int.TryParse(txtAnoConclusao.Text, out anoLetivo))
                //{
                //    txtAnoConclusao.Focus();
                //    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.AnoInvalido"));
                //}
                anoLetivo = Convert.ToInt32(ddlAnoConclusao.SelectedValue);

                if (UCComboTipoCurriculoPeriodo1.Valor <= 0)
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.AnoObrigatorio"));

                Decimal cargaHoraria = 0;
                if (!Decimal.TryParse(txtCargaHoraria.Text.Replace(",", "."), out cargaHoraria))
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.CargaInvalida"));

                if (rblOrigemEscola.SelectedValue.Equals(""))
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.EscolaOrigemObrigatoria"));
                else if (rblOrigemEscola.SelectedValue.Equals("1") && UCComboUAEscola.Esc_ID <= 0)
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.EscolaOrigemObrigatoria"));
                else if (rblOrigemEscola.SelectedValue.Equals("2") && string.IsNullOrEmpty(txtEscolaOrigem.Text))
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.EscolaOrigemObrigatoria"));

                CarregarDadosDisciplinas();

                if (VS_alh_id <= 0)
                {
                    IncluiHistorico(anoLetivo, rblOrigemEscola.SelectedValue.Equals("1"));
                    AdicionaHistoricoDisciplina(anoLetivo, rblOrigemEscola.SelectedValue.Equals("1"));
                }
                else
                {
                    AlteraHistorico(anoLetivo, rblOrigemEscola.SelectedValue.Equals("1"));
                    AdicionaHistoricoDisciplina(anoLetivo, rblOrigemEscola.SelectedValue.Equals("1"));
                }

                string param = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.SALVAR_SEMPRE_MAIUSCULO);
                bool salvar_sempre_maiusculo = !string.IsNullOrEmpty(param) && Convert.ToBoolean(param);

                List<ACA_AlunoHistoricoBO.StructHistorico> lstHistorico = new List<ACA_AlunoHistoricoBO.StructHistorico>();
                lstHistorico.Add(VS_Historico);

                ACA_AlunoHistoricoBO.SalvarInclusaoAlteracaoHistoricoAluno(VS_alu_id, lstHistorico, new List<ACA_AlunoHistoricoObservacao>(),
                                                                           salvar_sempre_maiusculo);

                CarregarAluno();
                ApplicationWEB._GravaLogSistema(edicao ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, 
                                                "HistoricoEscolar" + " anoLetivo:" + anoLetivo.ToString() + " alu_id:" + VS_alu_id.ToString() + 
                                                (edicao ? " alh_id:" + VS_alh_id : ""));
                ScriptManager.RegisterStartupScript(Page, GetType(), "AddHistorico", "$(document).ready(function(){ $('.divAddHistorico').dialog('close'); });", true);
            }
            catch (ValidationException ex)
            {
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroSalvar"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        private void LimparPopUp(bool limpaAno)
        {
            VS_Historico = new ACA_AlunoHistoricoBO.StructHistorico();
            VS_alh_id = -1;
            if (limpaAno)
                ddlAnoConclusao.SelectedIndex = 0;
                //txtAnoConclusao.Text = "";
            txtCargaHoraria.Text = "";
            if (rblOrigemEscola.SelectedItem != null)
                rblOrigemEscola.SelectedItem.Selected = false;
            divDentroRede.Visible = divForaRede.Visible = divResFinal.Visible = false;
            btnEscolaOrigem.Enabled = txtCargaHoraria.Enabled = rblOrigemEscola.Enabled = UCComboUAEscola.EnableEscolas = true;
            UCComboTipoCurriculoPeriodo1.Titulo = (string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.UCComboTipoCurriculoPeriodo1.Titulo") + "*";
            UCComboTipoCurriculoPeriodo1.MostrarMessageSelecione = true;
            UCComboTipoCurriculoPeriodo1.PermiteEditar = false;
            UCComboTipoCurriculoPeriodo1.SelectedIndex = 0;
            btnSalvarAddHistorico.Visible = true;
        }

        /// <summary>
        /// Atualiza as informações da tela.
        /// </summary>
        private void AtualizaTela()
        {
            // Cadastro de cidades
            UCCadastroCidade1.Inicialize(ApplicationWEB._Paginacao, "divCadastroCidade");
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlAnoConclusao.Items.Add(new ListItem((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ddlAnoConclusao.Selecione"), "-1"));
                for (int i = DateTime.Today.Year; i > DateTime.Today.Year - 50; i--)
                    ddlAnoConclusao.Items.Add(new ListItem(i.ToString(), i.ToString()));

                // Escola de origem - Tipo rede de ensino / Endereço
                UCComboTipoRedeEnsino1.Obrigatorio = true;
                UCComboTipoRedeEnsino1.ValidationGroup = "EscolaOrigem";
                UCComboTipoCurriculoPeriodo1.Obrigatorio = true;
                UCComboTipoCurriculoPeriodo1.ValidationGroup = "AddHistorico";
                UCComboTipoRedeEnsino1.CarregarTipoRedeEnsino();
                UCEnderecos2.Inicializar(false, true, "EscolaOrigem", false, false);

                ddlEstado.Items.Clear();
                ddlEstado.Items.Add(new ListItem((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ddlEstado.Selecione"), "-1", true));
                ddlEstado.DataSource = END_UnidadeFederativaBO.GetSelect(
                    Guid.Empty,
                    Guid.Empty,
                    String.Empty,
                    String.Empty,
                    1,
                    false,
                    -1,
                    -1);
                ddlEstado.DataBind();
            }
            AtualizaTela();

            UCComboTipoCurriculoPeriodo1.OnSelectedIndexChanged += UCComboTipoCurriculoPeriodo1_OnSelectedIndexChanged; 
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            string mensagem = __SessionWEB.PostMessages;
            if (!string.IsNullOrEmpty(mensagem))
            {
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage(mensagem, UtilBO.TipoMensagem.Alerta); 
            }
        }

        protected void btnAddNovoHistorico_Click(object sender, EventArgs e)
        {
            try
            {
                LimparPopUp(true);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddHistorico", "$(document).ready(function(){ $('.divAddHistorico').dialog('open'); });", true);
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AddHistorico", "$(document).ready(function(){ $('.divAddHistorico').dialog('close'); });", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AddHistorico", "$(document).ready(function(){ $('.divAddHistorico').dialog('close'); });", true);
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroPopUpNovoHist"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnAddrProjAtivComplementar_Click(object sender, EventArgs e)
        {
            try
            {
                divBuscaProjAtivComplementar.Visible = true;
                divCadastroProjAtivComplementar.Visible = false;

                CarregarProjetos();

                txtNomeProjeto.Text = "";
                VS_ahp_id = -1;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ProjAtivComplementar", "$(document).ready(function(){ $('.divProjAtivComplementar').dialog('open'); });", true);
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "ProjAtivComplementar", "$(document).ready(function(){ $('.divProjAtivComplementar').dialog('close'); });", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "ProjAtivComplementar", "$(document).ready(function(){ $('.divProjAtivComplementar').dialog('close'); });", true);
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroPopUpNovoHist"),
                                                        UtilBO.TipoMensagem.Erro);
            }
        }

        #region Resultado final
        protected void rptResFinal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                Repeater rptDadosResFinal = (Repeater)e.Item.FindControl("rptDadosResFinal");
                object dados = new object();

                if (e.Item.ItemType == ListItemType.Header)
                {
                    dados = dtTipoCurriculoPeriodo;
                }
                else
                {
                    int tds_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tds_id"));
                    int ahd_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ahd_id"));
                    int alh_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "alh_id"));

                    if (tds_id > 0)
                    {
                        dados = lsStructResFinalHistorico.Where(hist => hist.tds_tipo == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.Disciplina &&
                                                                        hist.ahp_id <= 0 && hist.tds_id == tds_id);
                    }
                    else
                    {
                        dados = lsStructResFinalHistorico.Where(hist => hist.ahp_id <= 0 && hist.ahd_id == ahd_id && hist.alh_id == alh_id).Distinct();
                    }
                }

                rptDadosResFinal.DataSource = dados;
                rptDadosResFinal.DataBind();
            }
        }

        protected void rptDadosResFinalHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Label lblNota = (Label)e.Item.FindControl("lblNota");
                Label lblFav = (Label)e.Item.FindControl("lblFav");

                int fav_id = lblFav == null || string.IsNullOrEmpty(lblFav.Text) ? 0 : Convert.ToInt32(lblFav.Text);

                if (fav.fav_id != fav_id)
                {
                    fav = new ACA_FormatoAvaliacao { fav_id = fav_id };
                    ACA_FormatoAvaliacaoBO.GetEntity(fav);

                    if (esa.esa_id != (fav.esa_idPorDisciplina > 0 ? fav.esa_idPorDisciplina : fav.esa_idConceitoGlobal))
                    {
                        esa = new ACA_EscalaAvaliacao { esa_id = fav.esa_idPorDisciplina > 0 ? fav.esa_idPorDisciplina : fav.esa_idConceitoGlobal };
                        ACA_EscalaAvaliacaoBO.GetEntity(esa);
            }
            }
                if (esa.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                    lblNota.Text = (string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.lblNota.Text.Conceito");
                else
                    lblNota.Text = (string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.lblNota.Text.Nota");
        }
        }

        protected void rptDadosResFinal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Label lblNota = (Label)e.Item.FindControl("lblNota");
                Label lblFreq = (Label)e.Item.FindControl("lblFreq");

                if (lblNota.Text.Equals(""))
                    lblNota.Text = "-";
                if (lblFreq.Text.Equals(""))
                    lblFreq.Text = "-";
            }
        }

        protected void rptEnrCurricular_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                Repeater rptDadosEnrCurricular = (Repeater)e.Item.FindControl("rptDadosEnrCurricular");
                object dados = new object();

                if (e.Item.ItemType == ListItemType.Header)
                {
                    dados = dtTipoCurriculoPeriodo;
                }
                else
                {
                    int tds_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tds_id"));

                    dados = lsStructResFinalHistorico.Where(hist => hist.tds_tipo == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.EnriquecimentoCurricular &&
                                                                    hist.ahp_id <= 0 && hist.tds_id == tds_id);
                }

                rptDadosEnrCurricular.DataSource = dados;
                rptDadosEnrCurricular.DataBind();
            }
        }

        protected void rptDadosEnrCurricularHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {

            }
        }

        protected void rptDadosEnrCurricular_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Label lblFreq = (Label)e.Item.FindControl("lblFreq");

                if (lblFreq.Text.Equals(((short)ResultadoHistorico.Aprovado).ToString()))
                    lblFreq.Text = "F";
                else if (lblFreq.Text.Equals(((short)ResultadoHistorico.ReprovadoFrequencia).ToString()))
                    lblFreq.Text = "NF";
                else
                    lblFreq.Text = "-";
            }
        }

        protected void rptProjeto_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                Repeater rptDadosProjeto = (Repeater)e.Item.FindControl("rptDadosProjeto");
                object dados = new object();

                if (e.Item.ItemType == ListItemType.Header)
                {
                    dados = dtTipoCurriculoPeriodo;
                }
                else
                {
                    int ahp_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ahp_id"));

                    dados = lsStructResFinalHistorico.Where(hist => hist.ahp_id > 0 && hist.ahp_id == ahp_id);
                }

                rptDadosProjeto.DataSource = dados;
                rptDadosProjeto.DataBind();
            }
        }

        protected void rptDadosProjetoHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {
            }
        }

        protected void rptDadosProjeto_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Label lblFreq = (Label)e.Item.FindControl("lblFreq");

                if (lblFreq.Text.Equals(((short)ResultadoHistorico.Aprovado).ToString()))
                    lblFreq.Text = "F";
                else if (lblFreq.Text.Equals(((short)ResultadoHistorico.ReprovadoFrequencia).ToString()))
                    lblFreq.Text = "NF";
                else
                    lblFreq.Text = "-";
            }
        }

        protected void rptParecer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                Repeater rptDadosParecer = (Repeater)e.Item.FindControl("rptDadosParecer");
                object dados = new object();

                if (e.Item.ItemType == ListItemType.Header)
                {
                    dados = dtTipoCurriculoPeriodo;
                }
                else
                {
                    List<ACA_AlunoHistoricoBO.StructResFinalHistorico> lstP = new List<ACA_AlunoHistoricoBO.StructResFinalHistorico>();
                    foreach (var tcp in (from resultado in lsStructResFinalHistorico
                                         group resultado by new
                                         {
                                             resultado.tcp_id
                                         }
                                             into res
                                             select res))
                        lstP.Add(new ACA_AlunoHistoricoBO.StructResFinalHistorico
                        {
                            tcp_id = tcp.Key.tcp_id,
                            alh_resultado = lsStructResFinalHistorico.Where(r => r.tcp_id == tcp.Key.tcp_id &&
                                                                                 !string.IsNullOrEmpty(r.alh_resultado) &&
                                                                                 !r.alh_resultado.Equals("0"))
                                                                     .FirstOrDefault().alh_resultado,
                        });

                    dados = lstP;
                }

                rptDadosParecer.DataSource = dados;
                rptDadosParecer.DataBind();
            }
        }

        protected void rptDadosParecerHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {

            }
        }

        protected void rptDadosParecer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Label lblPar = (Label)e.Item.FindControl("lblPar");

                if (string.IsNullOrEmpty(lblPar.Text) || lblPar.Text.Equals("") || lblPar.Text.Equals("0"))
                    lblPar.Text = "-";
                else
                {
                    lblPar.Text = (from par in DtPareceresFinal.AsEnumerable()
                                   where Convert.ToByte(par.Field<object>("tpr_resultado")) == ACA_AlunoHistoricoBO.HistoricoRes(Convert.ToByte(lblPar.Text))
                                   select par.Field<object>("tpr_nomenclatura").ToString()).FirstOrDefault();
                    lblPar.Text = !lblPar.Text.Contains(" (") ? lblPar.Text :
                                  lblPar.Text.Substring(0, lblPar.Text.IndexOf(" ("));
                }
            }
        }
        #endregion

        #region Adicionar Historico

        #region Escola de Origem

        protected void btnEscolaOrigem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!UCComboTipoRedeEnsinoBusca.Carregado())
                {
                    UCComboTipoRedeEnsinoBusca.CarregarTipoRedeEnsino();
                }

                UCComboTipoRedeEnsinoBusca.Valor = -1;
                txtEscolaOrigem.Text = string.Empty;

                UCComboTipoRedeEnsinoBusca.SetarFoco();
                updBuscaEscolaOrigem.Update();

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BuscaEscolaOrigem", "$('#divBuscaEscolaOrigem').dialog('open');", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void odsEscola_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
            {
                e.InputParameters.Clear();
            }
        }

        protected void AbreJanelaCadastroCidade()
        {
            if (!UCCadastroCidade1.Visible)
            {
                UCCadastroCidade1.CarregarCombos();
            }

            UCCadastroCidade1.Visible = true;
            UCCadastroCidade1.SetaFoco();

            updCadastroCidade.Update();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroCidadeNovo", "$('#divCadastroCidade').dialog('open');", true);
        }

        protected void btnPesquisarEscolaOrigem_Click(object sender, EventArgs e)
        {
            try
            {
                if (UCComboTipoRedeEnsinoBusca.Valor <= 0
                    && string.IsNullOrEmpty(txtBuscaEscolaOrigem.Text.Trim()))
                {
                    lblMessageBuscaEscolaOrigem.Text = UtilBO.GetErroMessage("É necessário informar ao menos um filtro para pesquisar."
                        , UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    grvEscolaOrigem.PageIndex = 0;

                    odsEscolaOrigem.SelectParameters.Clear();
                    odsEscolaOrigem.SelectParameters.Add("tre_id", UCComboTipoRedeEnsinoBusca.Valor.ToString());
                    odsEscolaOrigem.SelectParameters.Add("eco_nome", txtBuscaEscolaOrigem.Text.Trim());
                    odsEscolaOrigem.SelectParameters.Add("paginado", "true");

                    grvEscolaOrigem.DataBind();

                    fdsResultadosEscolaOrigem.Visible = true;
                    txtBuscaEscolaOrigem.Focus();
                }

                updBuscaEscolaOrigem.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage("Erro ao tentar carregar escolas de origem.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoEscolaOrigem_Click(object sender, EventArgs e)
        {
            LimpaCadastroEscolaOrigem();

            UCComboTipoRedeEnsino1.SetarFoco();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "CadastroEscolaOrigemNovo", "$('#divCadastroEscolaOrigem').dialog('open');", true);
        }

        protected void btnIncluirEscolaOrigem_Click(object sender, EventArgs e)
        {
            if (ValidaCadastroEscolaOrigem())
            {
                VS_novaEscolaOrigem = true;

                txtEscolaOrigem.Text = txtNomeEscolaOrigem.Text;

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "CadastroEscolaOrigemFechar", "$('#divCadastroEscolaOrigem').dialog('close');", true);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BuscaEscolaOrigemFechar", "$('#divBuscaEscolaOrigem').dialog('close');", true);

                //updCadastroHistorico.Update();
            }
            else
            {
                lblMessageEscolaOrigem.Visible = true;
            }
        }

        protected void grvEscolaOrigem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            VS_eco_id = grvEscolaOrigem.DataKeys[e.NewSelectedIndex].Values["eco_id"].ToString();
            txtEscolaOrigem.Text = grvEscolaOrigem.DataKeys[e.NewSelectedIndex].Values["eco_nome"].ToString();

            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BuscaEscolaOrigemFechar", "$('#divBuscaEscolaOrigem').dialog('close');", true);

            //updCadastroHistorico.Update();
        }

        protected void odsEscolaOrigem_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
            {
                e.InputParameters.Clear();
            }
        }

        protected void btnCadastraCidadeAluno_Click(object sender, ImageClickEventArgs e)
        {
            AbreJanelaCadastroCidade();
        }

        /// <summary>
        /// Limpa os campos do cadastro de escola de origem do
        /// histórico do aluno.
        /// </summary>
        private void LimpaCadastroEscolaOrigem()
        {
            UCComboTipoRedeEnsino1.Valor = -1;
            txtNomeEscolaOrigem.Text = string.Empty;
            txtCodigoInepEscolaOrigem.Text = string.Empty;
            txtCid_idMunicipioAluno.Value = Guid.Empty.ToString();
            txtMunicipioAlunoDaEscola.Text = string.Empty;

            VS_eco_id = "-1";
            VS_novaEscolaOrigem = false;
        }

        /// <summary>
        /// Limpa os campos da busca de escola de origem do
        /// histórico do aluno.
        /// </summary>
        private void LimpaBuscaEscolaOrigem()
        {
            //foreach (ListItem item in rblRedeEnsino.Items)
            //{
            //    item.Selected = false;
            //}

            //divDentroRede.Visible = false;
            //divForaRede.Visible = false;

            ////UCFiltroEscolas1._LoadInicial();

            //fdsResultadosEscolaDentroRede.Visible = false;
            //fdsResultadosEscolaOrigem.Visible = false;
        }

        /// <summary>
        /// Valida os campos do cadastro de escola de origem do aluno.
        /// </summary>
        /// <returns></returns>
        private bool ValidaCadastroEscolaOrigem()
        {
            if (String.IsNullOrEmpty(txtNomeEscolaOrigem.Text.Trim()))
            {
                lblMessageEscolaOrigem.Text = UtilBO.GetErroMessage("Escola de origem é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            lblMessageEscolaOrigem.Visible = false;
            return true;
        }

        #endregion Escola de Origem

        protected void grvHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAno = (Label)e.Row.FindControl("lblAno");
                LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");

                //bool permiteEditar = Convert.ToBoolean(grvHistorico.DataKeys[Convert.ToInt32(e.Row.RowIndex)]["permiteEditar"]) &&
                //                     VS_Permissoes.FirstOrDefault().pmo_permissaoEdicao;

                lblAno.Visible = false;// !permiteEditar;
                btnAlterar.Visible = true;// permiteEditar;
                btnExcluir.Visible = Convert.ToBoolean(grvHistorico.DataKeys[Convert.ToInt32(e.Row.RowIndex)]["permiteEditar"]) &&
                                     VS_Permissoes.FirstOrDefault().pmo_permissaoExclusao;

                btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void grvHistorico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Alterar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    //if (!Convert.ToBoolean(grvHistorico.DataKeys[index]["permiteEditar"]))
                    //    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.NaoPermiteEditar"));

                    LimparPopUp(true);

                    VS_alh_id = Convert.ToInt32(grvHistorico.DataKeys[index]["alh_id"]);
                    VS_permiteEditar = Convert.ToBoolean(grvHistorico.DataKeys[index]["permiteEditar"]);

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddHistorico", "$(document).ready(function(){ $('.divAddHistorico').dialog('open'); });", true);

                    int anoLetivo = Convert.ToInt32(grvHistorico.DataKeys[index]["alh_anoLetivo"]);
                    if (ddlAnoConclusao.Items.FindByValue(anoLetivo.ToString()) == null)
                        ddlAnoConclusao.Items.Add(new ListItem(anoLetivo.ToString(), anoLetivo.ToString()));
                    ddlAnoConclusao.SelectedValue = anoLetivo.ToString();// txtAnoConclusao.Text = anoLetivo.ToString();

                    UCComboTipoCurriculoPeriodo1.PermiteEditar = true;
                    UCComboTipoCurriculoPeriodo1.CarregarPorAnoNivelEnsino(anoLetivo, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_FUNDAMENTAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                    SortedList<int, ACA_AlunoHistoricoBO.StructHistorico> histAno = ACA_AlunoHistoricoBO.CarregarPorAlunoHistorico(VS_alu_id, VS_alh_id);
                    
                    VS_Historico = histAno.FirstOrDefault().Value;
                    CarregarResultadoFinalAddHistorico(histAno, anoLetivo);
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroEditar"),
                                                                        UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    if (!Convert.ToBoolean(grvHistorico.DataKeys[index]["permiteEditar"]))
                        throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.NaoPermiteExcluir"));

                    int alh_id = Convert.ToInt32(grvHistorico.DataKeys[index]["alh_id"]);

                    ACA_AlunoHistorico alh = new ACA_AlunoHistorico
                                                {
                                                    alu_id = VS_alu_id,
                                                    alh_id = alh_id
                                                };
                    ACA_AlunoHistoricoBO.GetEntity(alh);

                    alh.alh_situacao = 3;//Excluido
                    alh.alh_dataAlteracao = DateTime.Now;

                    if (!ACA_AlunoHistoricoBO.Save(alh))
                        throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroExcluir"));
                    
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "HistoricoEscolar" + " alh_id:" + alh_id.ToString() + " alu_id:" + VS_alu_id.ToString());
                    CarregarAluno();
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroExcluir"),
                                                                        UtilBO.TipoMensagem.Erro);
                }
            }
        }
        
        protected void UCComboTipoCurriculoPeriodo1_OnSelectedIndexChanged()
        {
            try
            {
                divResFinal.Visible = false;

                if (UCComboTipoCurriculoPeriodo1.Valor > 0)
                {
                    int anoLetivo = 0;
                    //if (txtAnoConclusao.Text.Length != 4 || !int.TryParse(txtAnoConclusao.Text, out anoLetivo))
                    //{
                    //    txtAnoConclusao.Focus();
                    //    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.AnoInvalido"));
                    //}
                    anoLetivo = Convert.ToInt32(ddlAnoConclusao.SelectedValue);

                    SortedList<int, ACA_AlunoHistoricoBO.StructHistorico> histAno = ACA_AlunoHistoricoBO.CarregarPorAlunoSerie(VS_alu_id, UCComboTipoCurriculoPeriodo1.Valor);

                    if (histAno.Any())
                    {
                        anoLetivo = histAno.FirstOrDefault().Value.entHistorico.alh_anoLetivo;
                        if (ddlAnoConclusao.Items.FindByValue(anoLetivo.ToString()) == null)
                            ddlAnoConclusao.Items.Add(new ListItem(anoLetivo.ToString(), anoLetivo.ToString()));
                        ddlAnoConclusao.SelectedValue = anoLetivo.ToString();// txtAnoConclusao.Text = histAno.FirstOrDefault().Value.entHistorico.alh_anoLetivo.ToString();

                        if (histAno.Any(h => h.Value.entHistorico.mtu_id > 0))
                        {
                            VS_permiteEditar = false;
                            BloqueiaCampos();
                            lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.SerieJaLancadaConsulta"),
                                                                                UtilBO.TipoMensagem.Alerta);
                        }
                        else
                        {
                            lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.SerieJaLancadaEdicao"),
                                                                                UtilBO.TipoMensagem.Alerta);
                        }

                        VS_Historico = histAno.FirstOrDefault().Value;
                        CarregarResultadoFinalAddHistorico(histAno, histAno.FirstOrDefault().Value.entHistorico.alh_anoLetivo);
                    }
                    else
                    {
                    divResFinal.Visible = true;
                    UCAddResultadoFinal1.VS_alu_id = VS_alu_id;
                    UCAddResultadoFinal1.Carregar(anoLetivo, UCComboTipoCurriculoPeriodo1.Valor, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_FUNDAMENTAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                        UCAddResultadoFinal1.CarregarNovo(VS_permiteEditar, new List<ACA_AlunoHistoricoBO.StructHistoricoDisciplina>());
                }
            }
            }
            catch (ValidationException ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ddlAnoConclusao.SelectedIndex = 0;//txtAnoConclusao.Text = "";
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroCarregarResFinal"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboUAEscola_IndexChangedUA()
        {
            if (UCComboUAEscola.Uad_ID == Guid.Empty)
                UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                {
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroCarregar"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rblOrigemEscola_SelectedIndexChanged(object sender, EventArgs e)
        {
            divForaRede.Visible = divDentroRede.Visible = false;
            LimpaCadastroEscolaOrigem();

            if (rblOrigemEscola.SelectedValue.Equals("1"))
                CarregaCamposDentroRede();
            if (rblOrigemEscola.SelectedValue.Equals("2"))
                CarregaCamposForaRede();
        }

        //protected void txtAnoConclusao_TextChanged(object sender, EventArgs e)
        protected void ddlAnoConclusao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimparPopUp(false);

                int anoLetivo = 0;
                //if (txtAnoConclusao.Text.Length != 4 || !int.TryParse(txtAnoConclusao.Text, out anoLetivo))
                //{
                //    txtAnoConclusao.Focus();
                //    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.AnoInvalido"));
                //}
                anoLetivo = Convert.ToInt32(ddlAnoConclusao.SelectedValue);

                if (anoLetivo > DateTime.Now.Year)
                {
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.AnoMaiorAtual"));
                }

                if (anoLetivo > 0)
                {
                    VS_permiteEditar = true;
                    UCComboTipoCurriculoPeriodo1.PermiteEditar = true;
                    UCComboTipoCurriculoPeriodo1.CarregarPorAnoNivelEnsino(anoLetivo, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_FUNDAMENTAL,
                                                                                                                                              __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                    UCComboTipoCurriculoPeriodo1.Focus();

                    SortedList<int, ACA_AlunoHistoricoBO.StructHistorico> histAno = ACA_AlunoHistoricoBO.CarregarPorAlunoAnoLetivo(VS_alu_id, anoLetivo);

                    if (histAno.Any())
                    {
                        if (histAno.Any(h => h.Value.entHistorico.mtu_id > 0))
                        {
                            VS_permiteEditar = false;
                            BloqueiaCampos();
                            lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.AnoJaLancadoConsulta"),
                                                                                UtilBO.TipoMensagem.Alerta);
                        }
                        else
                        {
                            lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.AnoJaLancadoEdicao"),
                                                                                UtilBO.TipoMensagem.Alerta);
                        }

                        VS_Historico = histAno.FirstOrDefault().Value;
                        CarregarResultadoFinalAddHistorico(histAno, anoLetivo);
                    }
                }
            }
            catch (ValidationException ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ddlAnoConclusao.SelectedIndex = 0;//txtAnoConclusao.Text = "";
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroCarregarAnoConclusao"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvarAddHistorico_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SalvarHistorico();
            }
        }

        protected void btnCancelarAddHistorico_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "AddHistorico", "$(document).ready(function(){ $('.divAddHistorico').dialog('close'); });", true);
        }
        #endregion

        #region Projeto/Atividade complementar
        protected void btnAddProjAtivCompl_Click(object sender, EventArgs e)
        {
            txtNomeProjeto.Text = "";
            VS_ahp_id = -1;

            divCadastroProjAtivComplementar.Visible = true;
            divBuscaProjAtivComplementar.Visible = false;
        }

        protected void btnSalvarProjAtivCompl_Click(object sender, EventArgs e)
        {
            try
            {
                if (VS_ahp_id <= 0)
                {
                    if (grvProjAtivComplementar.Rows.Count >= VS_qtdMaxProjetos)
                        throw new ValidationException(String.Format((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.QuantidadeMaximaProjetosUltrapassada"), VS_qtdMaxProjetos.ToString()));
                }

                ACA_AlunoHistoricoProjeto ahp = new ACA_AlunoHistoricoProjeto { alu_id = VS_alu_id, ahp_id = VS_ahp_id };
                ACA_AlunoHistoricoProjetoBO.GetEntity(ahp);

                ahp.ahp_nome = txtNomeProjeto.Text;
                ahp.ahp_situacao = 1;
                ahp.ahp_dataAlteracao = DateTime.Now;

                if (ahp.IsNew)
                    ahp.ahp_dataCriacao = DateTime.Now;

                ACA_AlunoHistoricoProjetoBO.Save(ahp);
                ApplicationWEB._GravaLogSistema(VS_ahp_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert,
                                                "ACA_AlunoHistoricoProjeto" + " alu_id:" + VS_alu_id.ToString() + " ahp_id:" + ahp.ahp_id.ToString());

                CarregarAluno();

                divCadastroProjAtivComplementar.Visible = false;
                divBuscaProjAtivComplementar.Visible = true;
                CarregarProjetos();

                txtNomeProjeto.Text = "";
                VS_ahp_id = -1;
            }
            catch (ValidationException ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ddlAnoConclusao.SelectedIndex = 0;//txtAnoConclusao.Text = "";
                ApplicationWEB._GravaErro(ex);
                lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroSalvarProjeto"),
                                                                    UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelarBuscaProjAtivCompl_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "AddHistorico", "$(document).ready(function(){ $('.divProjAtivComplementar').dialog('close'); });", true);
        }

        protected void btnCancelarProjAtivCompl_Click(object sender, EventArgs e)
        {
            txtNomeProjeto.Text = "";
            VS_ahp_id = -1;

            divCadastroProjAtivComplementar.Visible = false;
            divBuscaProjAtivComplementar.Visible = true;
        }

        protected void grvProjAtivComplementar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblNome = (Label)e.Row.FindControl("lblNome");
                LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");

                lblNome.Visible = !VS_Permissoes.FirstOrDefault().pmo_permissaoEdicao;
                btnAlterar.Visible = VS_Permissoes.FirstOrDefault().pmo_permissaoEdicao;
                btnExcluir.Visible = VS_Permissoes.FirstOrDefault().pmo_permissaoExclusao;

                btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void grvProjAtivComplementar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Alterar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    VS_ahp_id = Convert.ToInt32(grvProjAtivComplementar.DataKeys[index]["ahp_id"]);

                    txtNomeProjeto.Text = grvProjAtivComplementar.DataKeys[index]["ahp_nome"].ToString();

                    divBuscaProjAtivComplementar.Visible = false;
                    divCadastroProjAtivComplementar.Visible = true;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AddHistorico", "$(document).ready(function(){ $('.divProjAtivComplementar').dialog('open'); });", true);
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessageAddHistorico.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroEditarProjeto"),
                                                                        UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int ahp_id = Convert.ToInt32(grvProjAtivComplementar.DataKeys[index]["ahp_id"]);

                    ACA_AlunoHistoricoProjeto ahp = new ACA_AlunoHistoricoProjeto
                    {
                        alu_id = VS_alu_id,
                        ahp_id = ahp_id
                    };
                    ACA_AlunoHistoricoProjetoBO.GetEntity(ahp);

                    ahp.ahp_situacao = 3;//Excluido
                    ahp.ahp_dataAlteracao = DateTime.Now;

                    if (!ACA_AlunoHistoricoProjetoBO.Excluir(ahp))
                        throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroExcluirProjeto"));

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "AlunoHistoricoEscolarProjeto" + " ahp_id:" + ahp_id.ToString() + " alu_id:" + VS_alu_id.ToString());
                    CarregarAluno();
                    CarregarProjetos();
                }
                catch (ValidationException ex)
                {
                    lblMessageProjAtivComplementar.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessageProjAtivComplementar.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCEnsinoFundamental.ErroExcluirProjeto"),
                                                                        UtilBO.TipoMensagem.Erro);
                }
            }
        }
        #endregion

        protected void btnVisualizarHistorico_Click(object sender, EventArgs e)
        {
            if (clickVisualizar != null)
                clickVisualizar();
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (clickVoltar != null)
                clickVoltar();
        }

        #endregion
    }
}