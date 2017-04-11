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

namespace GestaoEscolar.WebControls.NavegacaoTelaPeriodo
{
    public partial class UCNavegacaoTelaPeriodo : MotherUserControl
    {
        #region Delegates

        public delegate void CarregaDadosTela();

        public CarregaDadosTela OnCarregaDadosTela;

        public delegate void AlteraPeriodo();

        public AlteraPeriodo OnAlteraPeriodo;

        #endregion Delegates

        #region Propriedades

        #region ViewState

        /// <summary>
        /// view state que armeza a pagina de retorno
        /// </summary>
        public string VS_paginaRetorno
        {
            get
            {
                if (ViewState["VS_paginaRetorno"] != null)
                    return ViewState["VS_paginaRetorno"].ToString();
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao ||
                    __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                    return "~/Academico/ControleTurma/Busca.aspx";
                else
                    return "~/Academico/ControleTurma/MinhaEscolaGestor.aspx";
            }
            set
            {
                ViewState["VS_paginaRetorno"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_id
        /// </summary>
        public int VS_cal_id
        {
            get
            {
                if (ViewState["VS_cal_id"] != null)
                    return Convert.ToInt32(ViewState["VS_cal_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_cal_id"] = value;
                hdnCalId.Value = value.ToString();
            }
        }

        /// <summary>
        /// Armazena o ID do tipo do período do calendário em viewstate.
        /// </summary>
        public int VS_tpc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tpc_id"] = value;
            }
        }

        /// <summary>
        /// Armazena a ordem do tipo do período do calendário em viewstate.
        /// </summary>
        public int VS_tpc_ordem
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_ordem"] ?? -1);
            }

            set
            {
                ViewState["VS_tpc_ordem"] = value;
            }
        }

        /// <summary>
        /// view state que armeza a descrição do período selecionado
        /// </summary>
        public string VS_cap_Descricao
        {
            get
            {
                Struct_CalendarioPeriodos per = VS_CalendarioPeriodo.Find(p => p.tpc_id == VS_tpc_id && p.tpc_ordem == VS_tpc_ordem);

                if (per.cap_descricao != null)
                {
                    return per.cap_descricao;
                }

                return "";
            }
        }

        /// <summary>
        /// ViewState que armazena o menu ativo atual.
        /// </summary>
        public eOpcaoAbaMinhasTurmas VS_opcaoAbaAtual
        {
            get
            {
                if (ViewState["VS_opcaoAbaAtual"] != null)
                    return (eOpcaoAbaMinhasTurmas)(ViewState["VS_opcaoAbaAtual"]);

                return eOpcaoAbaMinhasTurmas.DiarioClasse;
            }
            set
            {
                ViewState["VS_opcaoAbaAtual"] = value;
                SetaClassesBotoes();
            }
        }

        /// <summary>
        /// ViewState que armazena os calendarios do período(mesmos itens do repeater mostrado em tela).
        /// </summary>
        private List<Struct_CalendarioPeriodos> VS_CalendarioPeriodo
        {
            get
            {
                return (List<Struct_CalendarioPeriodos>)(ViewState["VS_CalendarioPeriodo"] ?? ((ViewState["VS_CalendarioPeriodo"] = new List<Struct_CalendarioPeriodos>())));
            }

            set
            {
                ViewState["VS_CalendarioPeriodo"] = value;
            }
        }

        /// <summary>
        /// view state que armeza o cap_id do período
        /// </summary>
        public int VS_cap_id
        {
            get
            {
                Struct_CalendarioPeriodos per = VS_CalendarioPeriodo.Find(p => p.tpc_id == VS_tpc_id && p.tpc_ordem == VS_tpc_ordem);

                if (per.cap_id > 0)
                {
                    return per.cap_id;
                }

                return -1;
            }
        }

        /// <summary>
        /// ViewState que armazena se deve ser adicionado o botao Final no final dos períodos.
        /// </summary>
        public bool VS_IncluirPeriodoFinal
        {
            get
            {
                return (bool)(ViewState["VS_IncluirPeriodoFinal"] ?? false);
            }

            set
            {
                ViewState["VS_IncluirPeriodoFinal"] = value;
            }
        }

        #endregion ViewState

        #region Botoes

        /// <summary>
        /// Retorna ou seta a visibilidade do botão planejamento anual.
        /// </summary>
        public bool VisiblePlanejamentoAnual
        {
            set
            {
                btnPlanejamentoAnual.Visible = value;
            }
        }

        /// <summary>
        /// Retorna ou seta a visibilidade do botão diário classe.
        /// </summary>
        private bool VisibleDiarioClasse
        {
            set
            {
                btnDiarioClasse.Visible = value;
            }
        }

        /// <summary>
        /// Retorna ou seta a visibilidade do botão listão.
        /// </summary>
        private bool VisibleListao
        {
            set
            {
                btnListao.Visible = value;
            }
        }

        /// <summary>
        /// Retorna ou seta a visibilidade do botão efetivação.
        /// </summary>
        public bool VisibleEfetivacao
        {
            set
            {
                btnEfetivacao.Visible = value;
            }
        }

        /// <summary>
        /// Retorna ou seta a visibilidade do botão alunos.
        /// </summary>
        private bool VisibleAlunos
        {
            set
            {
                btnAlunos.Visible = value;
            }
        }

        /// <summary>
        /// Retorna ou seta a visibilidade do botão voltar.
        /// </summary>
        private bool VisibleVoltar
        {
            set
            {
                btnVoltar.Visible = value;
            }
        }

        #endregion Botoes

        /// <summary>
        /// Retorna a data inicio do calendário do período
        /// </summary>
        public DateTime cap_dataInicio
        {
            get
            {
                if (VS_tpc_id > 0 && VS_CalendarioPeriodo.Count > 0)
                {
                    return (DateTime)VS_CalendarioPeriodo.Find(p => p.tpc_id == VS_tpc_id && p.tpc_ordem == VS_tpc_ordem).cap_dataInicio;
                }
                else
                {
                    return new DateTime();
                }
            }
        }

        /// <summary>
        /// Retorna a data fim do calendário do período
        /// </summary>
        public DateTime cap_dataFim
        {
            get
            {
                if (VS_tpc_id > 0 && VS_CalendarioPeriodo.Count > 0)
                {
                    return (DateTime)VS_CalendarioPeriodo.Find(p => p.tpc_id == VS_tpc_id && p.tpc_ordem == VS_tpc_ordem).cap_dataFim;
                }
                else
                {
                    return new DateTime();
                }
            }
        }

        /// <summary>
        /// Retorna a data inicio do calendário
        /// </summary>
        public DateTime cal_dataInicio
        {
            get
            {
                if (VS_tpc_id > 0 && VS_CalendarioPeriodo.Count > 0)
                {
                    return (DateTime)VS_CalendarioPeriodo.Find(p => p.tpc_id == VS_tpc_id && p.tpc_ordem == VS_tpc_ordem).cal_dataInicio;
                }
                else
                {
                    return new DateTime();
                }
            }
        }

        /// <summary>
        /// Retorna a data fim do calendário
        /// </summary>
        public DateTime cal_dataFim
        {
            get
            {
                if (VS_tpc_id > 0 && VS_CalendarioPeriodo.Count > 0)
                {
                    return (DateTime)VS_CalendarioPeriodo.Find(p => p.tpc_id == VS_tpc_id && p.tpc_ordem == VS_tpc_ordem).cal_dataFim;
                }
                else
                {
                    return new DateTime();
                }
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_ano
        /// </summary>
        public int VS_cal_ano
        {
            get
            {
                if (ViewState["VS_cal_ano"] != null)
                    return Convert.ToInt32(ViewState["VS_cal_ano"]);
                return -1;
            }
            set
            {
                ViewState["VS_cal_ano"] = value;
            }
        }

        private List<ACA_Avaliacao> listaAvaliacao = null;

        private List<ACA_Avaliacao> ltAvaliacao
        {
            get
            {
                return listaAvaliacao ?? new List<ACA_Avaliacao>();
            }

            set
            {
                listaAvaliacao = value;
            }
        }


        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Redireciona para a página informada dentro da pasta Controle de Turmas.
        /// </summary>
        /// <param name="pagina">Página</param>
        private void RedirecionaTela(string pagina)
        {
            Response.Redirect("~/Academico/ControleTurma/" + pagina + ".aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Adiciona a classe opcao_selecionada ao botão cuja opção está ativa.
        /// </summary>
        public void SetaClassesBotoes()
        {
            //RemoveClass(btnIndicadores, "opcao_selecionada");
            RemoveClass(btnPlanejamentoAnual, "opcao_selecionada");
            RemoveClass(btnDiarioClasse, "opcao_selecionada");
            RemoveClass(btnListao, "opcao_selecionada");
            RemoveClass(btnEfetivacao, "opcao_selecionada");
            RemoveClass(btnAlunos, "opcao_selecionada");
            RemoveClass(btnFrequencia, "opcao_selecionada");
            RemoveClass(btnAvaliacao, "opcao_selecionada");

            switch (VS_opcaoAbaAtual)
            {
                case eOpcaoAbaMinhasTurmas.DiarioClasse:
                    {
                        AddClass(btnDiarioClasse, "opcao_selecionada");
                        lblTitulo.Text = btnDiarioClasse.Text;
                        break;
                    }
                case eOpcaoAbaMinhasTurmas.Alunos:
                    {
                        AddClass(btnAlunos, "opcao_selecionada");
                        lblTitulo.Text = btnAlunos.Text;
                        break;
                    }
                case eOpcaoAbaMinhasTurmas.Efetivacao:
                    {
                        AddClass(btnEfetivacao, "opcao_selecionada");
                        lblTitulo.Text = btnEfetivacao.Text;
                        break;
                    }
                case eOpcaoAbaMinhasTurmas.Listao:
                    {
                        AddClass(btnListao, "opcao_selecionada");
                        lblTitulo.Text = btnListao.Text;
                        break;
                    }
                case eOpcaoAbaMinhasTurmas.PlanejamentoAnual:
                    {
                        AddClass(btnPlanejamentoAnual, "opcao_selecionada");
                        lblTitulo.Text = btnPlanejamentoAnual.Text;
                        break;
                    }
                case eOpcaoAbaMinhasTurmas.Frequencia:
                    {
                        AddClass(btnFrequencia, "opcao_selecionada");
                        lblTitulo.Text = btnFrequencia.Text;
                        break;
                    }
                case eOpcaoAbaMinhasTurmas.Avaliacao:
                    {
                        AddClass(btnAvaliacao, "opcao_selecionada");
                        lblTitulo.Text = btnAvaliacao.Text;
                        break;
                    }
                default:
                    break;
            }

            #region Adiciona classe de mensagem de saída da página

            if (!Convert.ToString(btnDiarioClasse.CssClass).Contains("btnMensagemUnload"))
            {
                btnDiarioClasse.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnAlunos.CssClass).Contains("btnMensagemUnload"))
            {
                btnAlunos.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnEfetivacao.CssClass).Contains("btnMensagemUnload"))
            {
                btnEfetivacao.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnListao.CssClass).Contains("btnMensagemUnload"))
            {
                btnListao.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnPlanejamentoAnual.CssClass).Contains("btnMensagemUnload"))
            {
                btnPlanejamentoAnual.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnVoltar.CssClass).Contains("btnMensagemUnload"))
            {
                btnVoltar.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnFrequencia.CssClass).Contains("btnMensagemUnload"))
            {
                btnFrequencia.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnAvaliacao.CssClass).Contains("btnMensagemUnload"))
            {
                btnAvaliacao.CssClass += " btnMensagemUnload";
            }

            #endregion Adiciona classe de mensagem de saída da página
        }

        /// <summary>
        /// Remove uma classe css ao um controle da página.
        /// Habilita o controle também.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private void RemoveClass(WebControl control, string cssClass)
        {
            control.Enabled = true;
            List<string> classes = control.CssClass.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            classes.Remove(cssClass);

            control.CssClass = string.Join(" ", classes.ToArray());
        }

        /// <summary>
        /// Adiciona uma classe css ao um controle da página.
        /// Desabilita o controle também.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private void AddClass(WebControl control, string cssClass)
        {
            control.Enabled = false;

            List<string> classes = control.CssClass.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            classes.Add(cssClass);

            control.CssClass = string.Join(" ", classes.ToArray());
        }

        /// <summary>
        /// Carregar os períodos e seta a visibilidade dos botões de acordo com a permissão do usuário.
        /// </summary>
        public void CarregarPeriodos
        (
            List<sPermissaoDocente> VS_ltPermissaoFrequencia
            , List<sPermissaoDocente> VS_ltPermissaoEfetivacao
            , List<sPermissaoDocente> VS_ltPermissaoPlanejamentoAnual
            , List<sPermissaoDocente> VS_ltPermissaoAvaliacao
            , TUR_TurmaDisciplina VS_turmaDisciplinaRelacionada
            , int esc_id
            , byte tud_tipo
            , byte tdt_posicao = 0
            , Int64 tur_id = -1
            , Int64 tud_id = -1
            , bool incluirPeriodoRecesso = false
            , int tpcIdPendencia = -1
        )
        {
            // Habilita a visibilidade dos botões alunos e voltar
            VisibleAlunos = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MINHAS_TURMAS_ESCONDER_BOTAO_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            VisibleVoltar = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MINHAS_TURMAS_ESCONDER_BOTAO_VOLTAR, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            int fav_id = -1;

            if (tur_id > 0 && tud_id > 0 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                TUR_Turma entityTurma = new TUR_Turma { tur_id = tur_id };
                TUR_TurmaBO.GetEntity(entityTurma);

                ACA_FormatoAvaliacao entityFormato = new ACA_FormatoAvaliacao { fav_id = entityTurma.fav_id };
                ACA_FormatoAvaliacaoBO.GetEntity(entityFormato);

                ACA_EscalaAvaliacao entityEscala = new ACA_EscalaAvaliacao { esa_id = entityFormato.esa_idPorDisciplina };
                ACA_EscalaAvaliacaoBO.GetEntity(entityEscala);

                ACA_EscalaAvaliacao entityEscalaDocente = new ACA_EscalaAvaliacao { esa_id = entityFormato.esa_idDocente };
                ACA_EscalaAvaliacaoBO.GetEntity(entityEscalaDocente);

                TUR_TurmaDisciplina entityTurmaDisciplina = new TUR_TurmaDisciplina { tud_id = tud_id };
                TUR_TurmaDisciplinaBO.GetEntity(entityTurmaDisciplina);

                // Valor do conceito global ou por disciplina.
                string valorMinimo = tud_id > 0 ?
                    entityFormato.valorMinimoAprovacaoPorDisciplina :
                    entityFormato.valorMinimoAprovacaoConceitoGlobal;

                double notaMinimaAprovacao = 0;
                int ordemParecerMinimo = 0;

                EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo)entityEscala.esa_tipo;

                if (tipoEscala == EscalaAvaliacaoTipo.Numerica)
                {
                    notaMinimaAprovacao = Convert.ToDouble(valorMinimo.Replace(',', '.'));
                }
                else if (tipoEscala == EscalaAvaliacaoTipo.Pareceres)
                {
                    ordemParecerMinimo = ACA_EscalaAvaliacaoParecerBO.RetornaOrdem_Parecer(entityEscala.esa_id, valorMinimo, ApplicationWEB.AppMinutosCacheLongo);
                }

                hdnTudId.Value = tud_id.ToString();
                hdnTurId.Value = entityTurma.tur_id.ToString();
                hdnFavId.Value = entityTurma.fav_id.ToString();
                hdnEsaId.Value = entityEscala.esa_id.ToString();
                hdnTipoEscala.Value = entityEscala.esa_tipo.ToString();
                hdnTipoEscalaDocente.Value = entityEscalaDocente.esa_tipo.ToString();
                hdnNotaMinima.Value = notaMinimaAprovacao.ToString();
                hdnParecerMinimo.Value = ordemParecerMinimo.ToString();
                hdnTipoLancamento.Value = entityFormato.fav_tipoLancamentoFrequencia.ToString();
                hdnCalculoQtAulasDadas.Value = entityFormato.fav_calculoQtdeAulasDadas.ToString();
                hdnTurTipo.Value = entityTurma.tur_tipo.ToString();
                hdnCalId.Value = entityTurma.cal_id.ToString();
                hdnTudTipo.Value = entityTurmaDisciplina.tud_tipo.ToString();
                hdnVariacao.Value = entityFormato.fav_variacao.ToString();
                hdnTipoDocente.Value = (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                            (byte)ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(tdt_posicao, ApplicationWEB.AppMinutosCacheLongo) : (byte)0).ToString();
                hdnDisciplinaEspecial.Value = entityTurmaDisciplina.tud_disciplinaEspecial ? "true" : "false";
                hdnFechamentoAutomatico.Value = entityFormato.fav_fechamentoAutomatico ? "true" : "false";
                hdnProcessarFilaFechamentoTela.Value = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PROCESSAR_FILA_FECHAMENTO_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) ? "true" : "false";

                fav_id = entityFormato.fav_id;
            }

            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.TELA_UNICA_LANCAMENTO_FREQUENCIA_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                VisibleListao = VS_ltPermissaoFrequencia.Any(p => p.pdc_permissaoConsulta) || VS_ltPermissaoAvaliacao.Any(p => p.pdc_permissaoConsulta);
                btnFrequencia.Visible = btnAvaliacao.Visible = false;
            }
            else
            {
                VisibleListao = false;
                btnFrequencia.Visible = VS_ltPermissaoFrequencia.Any(p => p.pdc_permissaoConsulta);
                btnAvaliacao.Visible = VS_ltPermissaoAvaliacao.Any(p => p.pdc_permissaoConsulta);
            }

            VisibleEfetivacao = tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada &&
                                VS_ltPermissaoEfetivacao.Any(p => p.pdc_permissaoConsulta) &&
                                !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.MINHAS_TURMAS_ESCONDER_BOTAO_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            VisiblePlanejamentoAnual = tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada ?
                                        VS_turmaDisciplinaRelacionada != null && VS_turmaDisciplinaRelacionada.tud_naoLancarPlanejamento == false
                                        : VS_ltPermissaoPlanejamentoAnual.Any(p => p.pdc_permissaoConsulta);

            List<Struct_CalendarioPeriodos> lstCalendarioPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            if (incluirPeriodoRecesso)
            {
                VS_CalendarioPeriodo = lstCalendarioPeriodos;
            }
            else
            {
                VS_CalendarioPeriodo = lstCalendarioPeriodos.FindAll(p => p.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                // Se o período selecionado for de recesso,
                // seleciono o bimestre anterior.
                if (VS_tpc_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    int indiceRecesso = lstCalendarioPeriodos.FindIndex(p => p.tpc_id == VS_tpc_id);
                    if (indiceRecesso > 0)
                    {
                        Struct_CalendarioPeriodos periodo = lstCalendarioPeriodos[indiceRecesso - 1];
                        VS_tpc_id = periodo.tpc_id;
                        VS_tpc_ordem = periodo.tpc_ordem;
                    }
                }
            }

            VS_cal_ano = VS_CalendarioPeriodo.Find(p => p.cal_id == VS_cal_id).cal_ano;

            List<ESC_EscolaCalendarioPeriodo> lstEscCalPeriodo = ESC_EscolaCalendarioPeriodoBO.SelectEscolasCalendarioCache(VS_cal_id, ApplicationWEB.AppMinutosCacheCurto);

            VS_CalendarioPeriodo = VS_CalendarioPeriodo.Where(calP => (lstEscCalPeriodo.Where(escP => (escP.esc_id == esc_id && escP.tpc_id == calP.tpc_id)).Count() == 0)).ToList();

            if (VS_IncluirPeriodoFinal)
            {
                Struct_CalendarioPeriodos[] calendarioPeriodosCopy = new Struct_CalendarioPeriodos[VS_CalendarioPeriodo.Count() + 1];
                VS_CalendarioPeriodo.CopyTo(calendarioPeriodosCopy, 0);

                Struct_CalendarioPeriodos periodoFinal = new Struct_CalendarioPeriodos();
                periodoFinal.cap_descricao = periodoFinal.tpc_nomeAbreviado = GetGlobalResourceObject("UserControl", "NavegacaoTelaPeriodo.UCNavegacaoTelaPeriodo.PeriodoFinal").ToString();
                periodoFinal.tpc_id = -1;
                calendarioPeriodosCopy[VS_CalendarioPeriodo.Count()] = periodoFinal;

                rptPeriodo.DataSource = calendarioPeriodosCopy;               
            }
            else
            {
                rptPeriodo.DataSource = VS_CalendarioPeriodo;
            }

            if (fav_id > 0)
            {
                string tpc_id = string.Join(",", VS_CalendarioPeriodo.Select(p => p.tpc_id.ToString()).ToArray());
                ltAvaliacao = ACA_AvaliacaoBO.ConsultaPor_Periodo_Relacionadas(fav_id, tpc_id, ApplicationWEB.AppMinutosCacheLongo);
                if (VS_IncluirPeriodoFinal)
                {
                    ltAvaliacao.AddRange(ACA_AvaliacaoBO.SelectAvaliacaoFinal_PorFormato(fav_id, ApplicationWEB.AppMinutosCacheLongo));
                }
            }

            rptPeriodo.DataBind();

            //Seleciona o ultimo bimestre
            List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo);
            int tpc_idUltimoPeriodo = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;
            int tpc_ordemUltimoPeriodo = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_ordem : 0;

            if (tpcIdPendencia > 0)
            {
                //Busca o bimestre pendente
                Struct_CalendarioPeriodos periodo = VS_CalendarioPeriodo.Where(x => x.tpc_id == tpcIdPendencia).FirstOrDefault();
                VS_tpc_id = periodo.tpc_id;
                VS_tpc_ordem = periodo.tpc_ordem;
            }

            if (VS_tpc_id <= 0 && !VS_IncluirPeriodoFinal)
            {
                //Busca o bimestre corrente
                Struct_CalendarioPeriodos periodo = VS_CalendarioPeriodo.Where(x => (x.cap_dataInicio.Date <= DateTime.Now.Date && x.cap_dataFim.Date >= DateTime.Now.Date)).FirstOrDefault();
                VS_tpc_id = periodo.tpc_id;
                VS_tpc_ordem = periodo.tpc_ordem;

                if (VS_tpc_id <= 0)
                {
                    //Se não tem bimestre selecionado e nem bimestre corrente então seleciona o próximo corrente
                    periodo = VS_CalendarioPeriodo.Where(x => (x.cap_dataInicio.Date >= DateTime.Now.Date)).FirstOrDefault();
                    VS_tpc_id = periodo.tpc_id;
                    VS_tpc_ordem = periodo.tpc_ordem;

                    if (VS_tpc_id <= 0)
                    {
                        //Se não tem bimestre selecionado então seleciona o ultimo
                        VS_tpc_id = tpc_idUltimoPeriodo;
                        VS_tpc_ordem = tpc_ordemUltimoPeriodo;
                    }
                }
            }

            if (VS_tpc_id >= 0 && VS_IncluirPeriodoFinal)
            {
                if (VS_tpc_id == tpc_idUltimoPeriodo)
                {
                    // Se for o ultimo periodo e a avaliacao final estiver aberta,
                    // selecionar a avaliacao final
                    List<ACA_Evento> listaEventos = ACA_EventoBO.GetEntity_Efetivacao_List(VS_cal_id, tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                    if (listaEventos.Exists(p => p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                    {
                        VS_tpc_id = -1;
                        VS_tpc_ordem = 0;
                    }
                }

                if (VS_tpc_id == 0)
                {
                    //Se não tem bimestre selecionado e nem bimestre corrente então seleciona o próximo corrente
                    Struct_CalendarioPeriodos periodo = VS_CalendarioPeriodo.Where(x => (x.cap_dataInicio.Date >= DateTime.Now.Date)).FirstOrDefault();
                    VS_tpc_id = periodo.tpc_id;
                    VS_tpc_ordem = periodo.tpc_ordem;

                    if (VS_tpc_id <= 0)
                    {
                        //Se não tem bimestre selecionado então seleciona o final
                        VS_tpc_id = -1;
                        VS_tpc_ordem = 0;
                    }
                }
            }

            if (VS_tpc_ordem < 0)
            {
                VS_tpc_ordem = 0;
            }

            // Seleciona o botão do bimestre informado (VS_tpc_id)
            rptPeriodo.Items.Cast<RepeaterItem>().ToList()
                        .Select(p => (Button)p.FindControl("btnPeriodo"))
                        .ToList().ForEach(p => RemoveClass(p, "periodo_selecionado"));
            rptPeriodo.Items.Cast<RepeaterItem>().ToList()
                        .Where(p => Convert.ToInt32(((HiddenField)p.FindControl("hdnPeriodo")).Value) == VS_tpc_id
                                && Convert.ToInt32(((HiddenField)p.FindControl("hdnPeriodoOrdem")).Value) == VS_tpc_ordem)
                        .Select(p => (Button)p.FindControl("btnPeriodo"))
                        .ToList()
                        .ForEach
                        (
                            p =>
                            {
                                AddClass(p, "periodo_selecionado");
                                HiddenField hdn = (HiddenField)p.FindControl("hdnIdAvaliacao");
                                if (!string.IsNullOrEmpty(hdn.Value))
                                {
                                    hdnAvaId.Value = hdn.Value;
                                }

                                hdn = (HiddenField)p.FindControl("hdnAvaliacaoTipo");
                                if (!string.IsNullOrEmpty(hdn.Value))
                                {
                                    hdnTipoAvaliacao.Value = hdn.Value;
                                }


                                hdnTpcId.Value = VS_tpc_id.ToString();

                                hdn = (HiddenField)p.FindControl("hdnPeriodoOrdem");
                                if (!string.IsNullOrEmpty(hdn.Value))
                                {
                                    hdnTpcOrdem.Value = hdn.Value;
                                }
                            }
                        );
        }

        /// <summary>
        /// Desabilita todos os períodos.
        /// </summary>
        public void DesabilitarPeriodos()
        {
            rptPeriodo.Items.Cast<RepeaterItem>().ToList()
                        .Select(p => (Button)p.FindControl("btnPeriodo"))
                        .ToList().ForEach(p => AddClass(p, "periodo_selecionado"));
        }

        #endregion Métodos

        #region Eventos

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
            {
                Response.Redirect(VS_paginaRetorno, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect(VS_paginaRetorno, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnPlanejamentoAnual_Click(object sender, EventArgs e)
        {
            if (OnCarregaDadosTela != null)
                OnCarregaDadosTela();

            RedirecionaTela("PlanejamentoAnual");
        }

        protected void btnDiarioClasse_Click(object sender, EventArgs e)
        {
            if (OnCarregaDadosTela != null)
                OnCarregaDadosTela();

            RedirecionaTela("DiarioClasse");
        }

        protected void btnListao_Click(object sender, EventArgs e)
        {
            if (OnCarregaDadosTela != null)
                OnCarregaDadosTela();

            RedirecionaTela("Listao");
        }

        protected void btnEfetivacao_Click(object sender, EventArgs e)
        {
            if (OnCarregaDadosTela != null)
                OnCarregaDadosTela();

            bool fechamentoAutomatico = string.IsNullOrEmpty(hdnFechamentoAutomatico.Value) ? false : 
                                        Convert.ToBoolean(hdnFechamentoAutomatico.Value);
            if (fechamentoAutomatico)
            {
                RedirecionaTela("Fechamento");
            }
            else
            {
                RedirecionaTela("Efetivacao");
            }
        }

        protected void btnAlunos_Click(object sender, EventArgs e)
        {
            if (OnCarregaDadosTela != null)
                OnCarregaDadosTela();

            RedirecionaTela("Alunos");
        }

        protected void btnPeriodo_Click(object sender, EventArgs e)
        {
            Button btnPeriodo = (Button)sender;
            RepeaterItem itemPeriodo = (RepeaterItem)btnPeriodo.NamingContainer;
            Repeater rptPeriodo = (Repeater)itemPeriodo.NamingContainer;
            HiddenField hdnPeriodo = (HiddenField)itemPeriodo.FindControl("hdnPeriodo");
            HiddenField hdnPeriodoOrdem = (HiddenField)itemPeriodo.FindControl("hdnPeriodoOrdem");

            VS_tpc_id = Convert.ToInt32(hdnPeriodo.Value);
            VS_tpc_ordem = Convert.ToInt32(hdnPeriodoOrdem.Value);

            rptPeriodo.Items.Cast<RepeaterItem>().ToList()
                        .Select(p => (Button)p.FindControl("btnPeriodo"))
                        .ToList().ForEach(p => RemoveClass(p, "periodo_selecionado"));

            AddClass(btnPeriodo, "periodo_selecionado");

            // Chamar evento de mudar de período.
            if (OnAlteraPeriodo != null)
                OnAlteraPeriodo();
        }

        protected void btnFrequencia_Click(object sender, EventArgs e)
        {
            if (OnCarregaDadosTela != null)
                OnCarregaDadosTela();

            RedirecionaTela("Frequencia");
        }

        protected void btnAvaliacao_Click(object sender, EventArgs e)
        {
            if (OnCarregaDadosTela != null)
                OnCarregaDadosTela();

            RedirecionaTela("Avaliacao");
        }
        
        protected void rptPeriodo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Item)
            {
                Button btnPeriodo = (Button)e.Item.FindControl("btnPeriodo");
                if (btnPeriodo != null)
                {
                    if (!Convert.ToString(btnPeriodo.CssClass).Contains("btnMensagemUnload"))
                    {
                        btnPeriodo.CssClass += " btnMensagemUnload";
                    }
                }

                if (ltAvaliacao.Any())
                {
                    int tpc_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_id"));

                    ACA_Avaliacao entityAvaliacao = tpc_id > 0 ?
                        ltAvaliacao.Find(p => p.tpc_id == tpc_id) :
                        ltAvaliacao.Find(p => p.ava_tipo == (byte)AvaliacaoTipo.Final);

                    HiddenField hdn = (HiddenField)e.Item.FindControl("hdnIdAvaliacao");
                    hdn.Value = entityAvaliacao == null ? "-1" : entityAvaliacao.ava_id.ToString();

                    hdn = (HiddenField)e.Item.FindControl("hdnAvaliacaoTipo");
                    hdn.Value = entityAvaliacao == null ? "0" : entityAvaliacao.ava_tipo.ToString();
                }
            }
        }

        #endregion Eventos
    }
}