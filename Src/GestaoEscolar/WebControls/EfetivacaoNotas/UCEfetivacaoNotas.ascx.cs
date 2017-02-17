using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using ReportNameGestaoAcademica = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica;

namespace GestaoEscolar.WebControls.EfetivacaoNotas
{
    public partial class UCEfetivacaoNotas : MotherUserControl
    {
        #region Estruturas

        /// <summary>
        /// Estrutura usada para guardar as justificativas da nota pos-conselho e da nota final.
        /// </summary>
        [Serializable]
        public struct Justificativa
        {
            public string Id;
            public string Valor;
        }

        /// <summary>
        /// Estrutura usada para guardar as notas de relatório.
        /// </summary>
        [Serializable]
        public struct NotasRelatorio
        {
            public string Id;
            public string Valor;
            public string arq_idRelatorio;
        }

        #endregion Estruturas

        #region Propriedades

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tur_id
        /// </summary>
        public long _VS_tur_id
        {
            get
            {
                if (ViewState["_VS_tur_id"] != null)
                {
                    return Convert.ToInt64(ViewState["_VS_tur_id"]);
                }

                return -1;
            }
            set
            {
                ViewState["VS_Turma"] = null;
                ViewState["VS_turma_Peja"] = null;
                ViewState["VS_FormatoAvaliacao"] = null;

                ViewState["_VS_tur_id"] = value;                
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tud_id
        /// </summary>
        public string[] TurmaDisciplina_Ids
        {
            get
            {
                return ddlTurmaDisciplina.SelectedValue.Split(';');
            }
        }

        /// <summary>
        /// Retorna o Tud_ID selecionado no combo.
        /// </summary>
        public long Tud_id
        {
            get
            {
                string[] ids = TurmaDisciplina_Ids;

                if (ids.Length > 1)
                {
                    return Convert.ToInt64(ids[1]);
                }

                return -1;
            }
        }

        /// <summary>
        /// ID do período selecionado na tela que chamou.
        /// </summary>
        private int VS_Tpc_IdSelecionado
        {
            get
            {
                if (ViewState["VS_Tpc_IdSelecionado"] == null)
                {
                    return -1;
                }

                return Convert.ToInt32(ViewState["VS_Tpc_IdSelecionado"]);
            }
            set
            {
                ViewState["VS_Tpc_IdSelecionado"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de fav_id
        /// </summary>
        public int _VS_fav_id
        {
            get
            {
                if (ViewState["_VS_fav_id"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_fav_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["_VS_fav_id"] = value;
                ViewState["VS_NomeAvaliacaoRecuperacaoFinal"] = null;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de ava_id
        /// </summary>
        public int _VS_ava_id
        {
            get
            {
                if (ViewState["_VS_ava_id"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_ava_id"]);
                }

                return -1;
            }
            set
            {
                ViewState["VS_Avaliacao"] = null;
                ViewState["VS_EscalaAvaliacao"] = null;
                ViewState["VS_EscalaNumerica"] = null;
                ViewState["VS_EscalaAvaliacaoDocente"] = null;
                ViewState["VS_EscalaAvaliacaoAdicional"] = null;
                ViewState["VS_EscalaNumericaAdicional"] = null;

                ViewState["_VS_ava_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade que guarda se a janela para o cadastro de observação do aluno está aberta
        /// </summary>
        private bool _VS_JanelaObservacaoAberta
        {
            get
            {
                if (ViewState["_VS_JanelaObservacaoAberta"] != null)
                {
                    return Convert.ToBoolean(ViewState["_VS_JanelaObservacaoAberta"]);
                }

                return false;
            }
            set
            {
                ViewState["_VS_JanelaObservacaoAberta"] = value;
            }
        }

        /// <summary>
        /// Propriedade com o nome do modulo.
        /// </summary>
        public string NomeModulo
        {
            get
            {
                if (ViewState["nomemodulo"] == null)
                {
                    SYS_Modulo entModulo;
                    if (Modulo.IsNew)
                    {
                        entModulo = new SYS_Modulo
                        {
                            mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                            sis_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.sis_id
                        };
                        entModulo = GestaoEscolarUtilBO.GetEntityModuloCache(entModulo);
                    }
                    else
                    {
                        entModulo = Modulo;
                    }

                    ViewState["nomemodulo"] = string.IsNullOrEmpty(entModulo.mod_nome) ? "Efetivação de notas" : entModulo.mod_nome;
                }

                return ViewState["nomemodulo"].ToString();
            }
        }

        /// <summary>
        /// Retorna uma flag informando se existe uma avaliação de recuperação final para o formato da turma.
        /// </summary>
        public string VS_NomeAvaliacaoRecuperacaoFinal
        {
            get
            {
                if (ViewState["VS_NomeAvaliacaoRecuperacaoFinal"] == null)
                {
                    DataTable dt = ACA_AvaliacaoBO.GetSelectBy_TipoAvaliacao(AvaliacaoTipo.RecuperacaoFinal, _VS_fav_id);
                    if (dt.Rows.Count > 0)
                        ViewState["VS_NomeAvaliacaoRecuperacaoFinal"] = dt.Rows[0]["ava_nome"].ToString();
                    else
                        ViewState["VS_NomeAvaliacaoRecuperacaoFinal"] = "";
                }

                return ViewState["VS_NomeAvaliacaoRecuperacaoFinal"].ToString();
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando a matriz curricular da turma
        /// </summary>
        public List<ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular> VS_MatrizCurricular
        {
            get
            {
                if (ViewState["VS_MatrizCurricular"] != null)
                {
                    return (List<ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular>)ViewState["VS_MatrizCurricular"];
                }

                return new List<ACA_CurriculoControleSemestralDisciplinaPeriodoBO.MatrizCurricular>();
            }

            set
            {
                ViewState["VS_MatrizCurricular"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando se a turma possui efetivação semestral
        /// </summary>
        public bool VS_EfetivacaoSemestral
        {
            get
            {
                if (ViewState["VS_EfetivacaoSemestral"] != null)
                {
                    return Convert.ToBoolean(ViewState["VS_EfetivacaoSemestral"]);
                }

                return false;
            }

            set
            {
                ViewState["VS_EfetivacaoSemestral"] = value;
            }
        }

        /// <summary>
        /// Retorna se a turma selecionada no combo é do PEJA (currículo seriado por avaliações).
        /// </summary>
        public bool VS_turma_Peja
        {
            get
            {
                if (ViewState["VS_turma_Peja"] == null)
                {
                    if (_VS_tur_id > 0)
                    {
                        List<TUR_TurmaCurriculo> lTurmaCurriculo = TUR_TurmaCurriculoBO.GetSelectBy_Turma(_VS_tur_id, ApplicationWEB.AppMinutosCacheLongo);
                        ACA_Curriculo curriculo = ACA_CurriculoBO.GetEntity(new ACA_Curriculo
                        {
                            cur_id = lTurmaCurriculo.First().cur_id
                            ,
                            crr_id = lTurmaCurriculo.First().crr_id
                        });
                        ViewState["VS_turma_Peja"] = (curriculo.crr_regimeMatricula == (byte)ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes).ToString();
                    }
                }

                return Convert.ToBoolean((ViewState["VS_turma_Peja"] ?? "False"));
            }
        }
        
        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando a url de retorno
        /// </summary>
        public byte _VS_URL_Retorno_Efetivacao
        {
            get
            {
                if (ViewState["_VS_URL_Retorno_Efetivacao"] != null)
                {
                    return Convert.ToByte(ViewState["_VS_URL_Retorno_Efetivacao"]);
                }

                return 0;
            }

            set
            {
                ViewState["_VS_URL_Retorno_Efetivacao"] = value;
            }
        }

        /// <summary>
        /// Guarda a mensagem que deve aparecer antes de salvar o log
        /// </summary>
        public string VS_MensagemLogEfetivacao
        {
            get
            {
                if (ViewState["VS_MensagemLogEfetivacao"] == null)
                {
                    return "";
                }

                return ViewState["VS_MensagemLogEfetivacao"].ToString();
            }
            set
            {
                ViewState["VS_MensagemLogEfetivacao"] = value;
            }
        }

        /// <summary>
        /// Guarda a posição do docente.
        /// </summary>
        public byte VS_posicao
        {
            get
            {
                if (ViewState["VS_posicao"] == null)
                {
                    string posicaoDocente = hiddenPosicaoDocente.Value;
                    string situacaoTud = hiddenTudSituacao.Value;
                    if ((__SessionWEB.__UsuarioWEB.Docente.doc_id == 0 || situacaoTud.Equals("4")) && !string.IsNullOrEmpty(posicaoDocente))
                    {
                        ViewState["VS_posicao"] = Convert.ToByte(posicaoDocente);
                    }
                    else
                    {
                        ViewState["VS_posicao"] = TUR_TurmaDocenteBO.SelecionaPosicaoPorDocenteTurma(__SessionWEB.__UsuarioWEB.Docente.doc_id,
                                                    Tud_id, ApplicationWEB.AppMinutosCacheLongo);
                    }
                }

                return Convert.ToByte(ViewState["VS_posicao"]);
            }
        }

        /// <summary>
        /// Lista de permissões do docente para cadastro de planejamento anual.
        /// </summary>
        public List<sPermissaoDocente> VS_ltPermissaoEfetivacao
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoEfetivacao"] ??
                            (
                                ViewState["VS_ltPermissaoEfetivacao"] =
                                CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(VS_posicao, (byte)EnumModuloPermissao.Efetivacao)
                            )
                        );
            }
        }

        /// <summary>
        /// Lista de permissões do docente para consultar boletim.
        /// </summary>
        public List<sPermissaoDocente> VS_ltPermissaoBoletim
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoBoletim"] ??
                            (
                                ViewState["VS_ltPermissaoBoletim"] =
                                CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(VS_posicao, (byte)EnumModuloPermissao.Boletim)
                            )
                        );
            }
        }

        /// <summary>
        /// Indica se o docente logado tem permissão para editar a disciplina selecionada.
        /// no combo.
        /// </summary>
        public bool DocentePodeEditar
        {
            get
            {
                string[] ids = TurmaDisciplina_Ids;

                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 && !VS_ltPermissaoEfetivacao.Any(p => p.pdc_permissaoEdicao))
                    return false;

                if (ids.Length > 2)
                {
                    return Convert.ToBoolean(Convert.ToByte(ids[2]));
                }

                return true;
            }
        }

        /// <summary>
        /// Retorna se o usuario logado tem permissao para visualizar os botoes de salvar
        /// </summary>
        public bool usuarioPermissao
        {
            get
            {
                string situacaoTud = hiddenTudSituacao.Value;

                //bloquear botoes -> se for disciplina de docencia compartilhada ou 
                // se o docente nao possuir mais essa turma e se nao for turma extinta
                if (VS_turmaDisciplinaCompartilhada != null
                    || (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 
                        && (!VS_ltPermissaoEfetivacao.Any(p => p.pdc_permissaoEdicao))
                        || (situacaoTud == ((byte)TUR_TurmaDocenteSituacao.Inativo).ToString()
                                && VS_Turma.tur_situacao != (byte)TUR_TurmaSituacao.Encerrada
                                && VS_Turma.tur_situacao != (byte)TUR_TurmaSituacao.Extinta)))
                    return false;

                return __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }
        }

        /// <summary>
        /// Carrega as configurações para a disciplina compartilhada, relacionada com a disciplina atual.
        /// </summary>
        public TUR_TurmaDisciplina VS_turmaDisciplinaCompartilhada
        {
            get
            {
                if (ViewState["VS_turmaDisciplinaCompartilhada"] != null)
                    return (TUR_TurmaDisciplina)ViewState["VS_turmaDisciplinaCompartilhada"];
                return null;
            }

            set
            {
                ViewState["VS_turmaDisciplinaCompartilhada"] = value;
            }
        }

        /// <summary>
        /// Carrega a turma disciplina relacionada (para as disciplinas de docencia compartilhada).
        /// </summary>
        public sTurmaDisciplinaRelacionada VS_turmaDisciplinaRelacionada
        {
            get
            {
                if (ViewState["VS_turmaDisciplinaRelacionada"] != null)
                    return (sTurmaDisciplinaRelacionada)ViewState["VS_turmaDisciplinaRelacionada"];
                return new sTurmaDisciplinaRelacionada();
            }

            set
            {
                ViewState["VS_turmaDisciplinaRelacionada"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o tipo de docente logado.
        /// </summary>
        public EnumTipoDocente VS_tipoDocente
        {
            get
            {
                return (EnumTipoDocente)(ViewState["VS_tipoDocente"] ?? 0);
            }

            set
            {
                ViewState["VS_tipoDocente"] = value;
            }
        }
        
        /// <summary>
        /// Armazena o tud_id que será enviado para a tela de retorno.
        /// </summary>
        public long _VS_tud_id_Retorno
        {
            get
            {
                if (ViewState["_VS_tud_id_Retorno"] != null)
                {
                    return Convert.ToInt64(ViewState["_VS_tud_id_Retorno"]);
                }

                return -1;
            }

            set
            {
                ViewState["_VS_tud_id_Retorno"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena se deve ser adicionado o botao Final no final dos períodos.
        /// </summary>
        private bool VS_IncluirPeriodoFinal
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
        
        /// <summary>
        /// Seta a propriedade Visible da navegação entre telas.
        /// </summary>
        public bool VisibleNavegacao
        {
            set
            {
                UCNavegacaoLancamentoClasse1.Visible = value &&
                    !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ESCONDER_NAVEGACAO_MENU_CLASSES, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Seta a propriedade Visible do botao cancelar.
        /// </summary>
        public bool VisibleBotaoCancelar
        {
            set
            {
                _btnCancelar.Visible = _btnCancelar2.Visible = value;
            }
        }

        /// <summary>
        /// Seta a propriedade Text do botao cancelar.
        /// </summary>
        public string TextBotaoCancelar
        {
            set
            {
                _btnCancelar.Text = _btnCancelar2.Text = value;
            }
        }

        /// <summary>
        /// Seta a propriedade Visible do botao salvar.
        /// </summary>
        public bool VisibleBotaoSalvar
        {
            set
            {
                _btnSalvar.Visible = _btnSalvar2.Visible = value;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairEfetivacao", "var exibeMensagemSair=" + _btnSalvar.Visible.ToString().ToLower() + ";", true);
            }
        }

        /// <summary>
        /// Seta a propriedade Visible para os botões de imprimir e importar anotações dos alunos no relatório.
        /// </summary>
        public bool VisibleImprimirEImportarAnotacoesRelatorio
        {
            set
            {
                btnImportarAnotacoesAluno.Visible = btnImprimirRelatorio.Visible = value;
            }
        }

        /// <summary>
        /// Seta a mensagem no label principal da tela.
        /// </summary>
        public string MensagemTela
        {
            set
            {
                _lblMessage.Text = value;
            }
        }
        
        /// <summary>
        /// Entidade da turma selecionada no combo.
        /// </summary>
        public TUR_Turma VS_Turma
        {
            get
            {
                return (TUR_Turma)(ViewState["VS_Turma"] ?? (ViewState["VS_Turma"] = TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = _VS_tur_id })));
            }
        }

        /// <summary>
        /// Entidade do calendário da turma selecionada no combo.
        /// </summary>
        public ACA_CalendarioAnual VS_CalendarioAnual
        {
            get
            {
                return (ACA_CalendarioAnual)
                    (ViewState["VS_CalendarioAnual"] ??
                    (ViewState["VS_CalendarioAnual"] = ACA_CalendarioAnualBO.GetEntity
                        (new ACA_CalendarioAnual { cal_id = VS_Turma.cal_id })));
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        public ACA_FormatoAvaliacao VS_FormatoAvaliacao
        {
            get
            {
                if (ViewState["VS_FormatoAvaliacao"] == null)
                {
                    ACA_FormatoAvaliacao formatoAvaliacao = new ACA_FormatoAvaliacao
                    {
                        fav_id = _VS_fav_id
                    };
                    ACA_FormatoAvaliacaoBO.GetEntity(formatoAvaliacao);

                    ViewState["VS_FormatoAvaliacao"] = formatoAvaliacao;
                }

                return (ACA_FormatoAvaliacao)ViewState["VS_FormatoAvaliacao"];
            }
        }

        /// <summary>
        /// Retorna a avaliação selecionada na tela de busca.
        /// </summary>
        public ACA_Avaliacao VS_Avaliacao
        {
            get
            {
                return (ACA_Avaliacao)(ViewState["VS_Avaliacao"] ?? (ViewState["VS_Avaliacao"] = ACA_AvaliacaoBO.GetEntity(new ACA_Avaliacao { fav_id = _VS_fav_id, ava_id = _VS_ava_id })));
            }
        }

        /// <summary>
        /// Retorna a escala de avaliação do formato. Se for lançamento na disciplina,
        /// retorna de acordo com o esa_idPorDisciplina, se for global, retorna
        /// o esa_idConceitoGlobal.
        /// </summary>
        public ACA_EscalaAvaliacao VS_EscalaAvaliacao
        {
            get
            {
                if (ViewState["VS_EscalaAvaliacao"] == null)
                {
                    ACA_EscalaAvaliacao escala = new ACA_EscalaAvaliacao
                    {
                        esa_id = Tud_id > 0
                            ? VS_FormatoAvaliacao.esa_idPorDisciplina
                            : VS_FormatoAvaliacao.esa_idConceitoGlobal
                    };

                    if (escala.esa_id > 0)
                    {
                        // Só chama o método de carregar se o id da escala for > 0.
                        ACA_EscalaAvaliacaoBO.GetEntity(escala);
                    }

                    ViewState["VS_EscalaAvaliacao"] = escala;
                }

                return (ACA_EscalaAvaliacao)ViewState["VS_EscalaAvaliacao"];
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        public ACA_EscalaAvaliacaoNumerica VS_EscalaNumerica
        {
            get
            {
                if (ViewState["VS_EscalaNumerica"] == null)
                {
                    ACA_EscalaAvaliacaoNumerica entEscalaNumerica = new ACA_EscalaAvaliacaoNumerica
                    {
                        esa_id = VS_EscalaAvaliacao.esa_id
                    };
                    ACA_EscalaAvaliacaoNumericaBO.GetEntity(entEscalaNumerica);

                    ViewState["VS_EscalaNumerica"] = entEscalaNumerica;
                }

                return (ACA_EscalaAvaliacaoNumerica)ViewState["VS_EscalaNumerica"];
            }
        }

        /// <summary>
        /// Retorna a escala de avaliação do docente que está configurado no formato (esa_idDocente).
        /// </summary>
        public ACA_EscalaAvaliacao VS_EscalaAvaliacaoDocente
        {
            get
            {
                if (ViewState["VS_EscalaAvaliacaoDocente"] == null)
                {
                    ACA_EscalaAvaliacao escalaDocente = new ACA_EscalaAvaliacao
                    {
                        esa_id = VS_FormatoAvaliacao.esa_idDocente
                    };

                    if (escalaDocente.esa_id > 0)
                    {
                        // Só chama o método de carregar se o id da escala for > 0.
                        ACA_EscalaAvaliacaoBO.GetEntity(escalaDocente);
                    }

                    ViewState["VS_EscalaAvaliacaoDocente"] = escalaDocente;
                }

                return (ACA_EscalaAvaliacao)ViewState["VS_EscalaAvaliacaoDocente"];
            }
        }

        /// <summary>
        /// Retorna a escala de avaliação da avaliação adicional do Conceito Global.
        /// </summary>
        public ACA_EscalaAvaliacao VS_EscalaAvaliacaoAdicional
        {
            get
            {
                if (ViewState["VS_EscalaAvaliacaoAdicional"] == null)
                {
                    ACA_EscalaAvaliacao escalaAvaliacaoAdicional = new ACA_EscalaAvaliacao
                    {
                        esa_id = VS_FormatoAvaliacao.esa_idConceitoGlobalAdicional
                    };

                    if (PossuiAvaliacaoAdicional && escalaAvaliacaoAdicional.esa_id > 0)
                    {
                        // Só chama o método de carregar se o formato permitir avaliação adicional.
                        ACA_EscalaAvaliacaoBO.GetEntity(escalaAvaliacaoAdicional);
                    }

                    ViewState["VS_EscalaAvaliacaoAdicional"] = escalaAvaliacaoAdicional;
                }

                return (ACA_EscalaAvaliacao)ViewState["VS_EscalaAvaliacaoAdicional"];
            }
        }

        /// <summary>
        /// Retorna o formato de avaliação de acordo com o fav_id do ViewState.
        /// </summary>
        public ACA_EscalaAvaliacaoNumerica VS_EscalaNumericaAdicional
        {
            get
            {
                if (ViewState["VS_EscalaNumericaAdicional"] == null)
                {
                    ACA_EscalaAvaliacaoNumerica entEscalaNumerica = new ACA_EscalaAvaliacaoNumerica
                    {
                        esa_id = VS_EscalaAvaliacaoAdicional.esa_id
                    };
                    ACA_EscalaAvaliacaoNumericaBO.GetEntity(entEscalaNumerica);

                    ViewState["VS_EscalaNumericaAdicional"] = entEscalaNumerica;
            }

                return (ACA_EscalaAvaliacaoNumerica)ViewState["VS_EscalaNumericaAdicional"];
        }
        }

        /// <summary>
        /// Retorna uma flag, caso o formato de avaliação possua o conceito global e esteja selecionado
        /// o conceito global na tela, se está marcado para ter avaliação adicional no conceito global.
        /// </summary>
        public bool PossuiAvaliacaoAdicional
        {
            get
            {
                return VS_FormatoAvaliacao.fav_tipo != (byte)ACA_FormatoAvaliacaoTipo.Disciplina &&
                    Tud_id <= 0 &&
                    VS_FormatoAvaliacao.fav_conceitoGlobalAdicional;
            }
        }

        private TUR_TurmaDisciplina _entTurmaDisciplina;

        /// <summary>
        /// Entidade da disciplina selecionada no combo.
        /// </summary>
        private TUR_TurmaDisciplina EntTurmaDisciplina
        {
            get
            {
                return _entTurmaDisciplina ??
                    (_entTurmaDisciplina = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = Tud_id }));
            }
        }

        public Control FdsRelatorio
        {
            get
            {
                return fdsRelatorio;
            }
        }

        public Label LblMessage
        {
            get
            {
                return _lblMessage;
            }
        }

        public Label LblMessage2
        {
            get
            {
                return _lblMessage2;
            }
        }

        public Label LblMessage3
        {
            get
            {
                return _lblMessage3;
            }
        }

        public Button BtnSalvar
        {
            get
            {
                return _btnSalvar;
            }
        }

        public Panel PnlAlunos
        {
            get
            {
                return _pnlAlunos;
            }
        }

        /// <summary>
        /// Retorna/Guarda a posição do docente.
        /// </summary>
        public HiddenField hiddenPosicaoDocente
        {
            get
            {
                return hdnPosicaoDocente;
            }
            set
            {
                hdnPosicaoDocente = value;
                ViewState["VS_posicao"] = null;
                ViewState["VS_ltPermissaoEfetivacao"] = null;
                ViewState["VS_ltPermissaoBoletim"] = null;
            }
        }
        
        /// <summary>
        /// Retorna/Guarda a situação da turma disciplina do docente.
        /// </summary>
        public HiddenField hiddenTudSituacao
        {
            get
            {
                return hdnTudSituacao;
            }
            set
            {
                hdnTudSituacao = value;
                ViewState["VS_posicao"] = null;
                ViewState["VS_ltPermissaoEfetivacao"] = null;
                ViewState["VS_ltPermissaoBoletim"] = null;
            }
        }

        /// <summary>
        /// Variavel de controle para não carregar a tela duas vezes 
        /// </summary>
        private bool indexChangedAvaliacao = false;

        public bool HabilitaBoletimAluno
        {
            set
            {
                UCEfetivacaoNotasPadrao.HabilitaBoletimAluno = value;
                UCEfetivacaoNotasFinal.HabilitaBoletimAluno = value;
            }
        }

        /// <summary>
        /// Lista de IDs das turmas normais dos alunos matriculados em turmas multisseriadas do docente.
        /// </summary>
        public List<long> VS_listaTur_ids
        {
            get
            {
                return (List<long>)(ViewState["VS_listaTur_ids"] ?? new List<long>());
            }

            set
            {
                ViewState["VS_listaTur_ids"] = value;
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Seta a disciplina e turma selecionados em tela que chamou o user control.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">ID do período do calendário</param>
        public void SetaTurmaDisciplina(long tud_id, long tur_id, int tpc_id, bool incluirPeriodoFinal = false, List<long> listaTur_ids = null)
        {
            this._VS_tur_id = tur_id;
            this.VS_Tpc_IdSelecionado = tpc_id;
            this.VS_IncluirPeriodoFinal = incluirPeriodoFinal;
            this.VS_listaTur_ids = listaTur_ids ?? new List<long>();

            if (_VS_tur_id > 0)
            {
                // Efetivação semestral
                bool efetivacaoSemestral;
                VS_MatrizCurricular = ACA_CurriculoControleSemestralDisciplinaPeriodoBO.SelecionaMatrizCurricularTurma(_VS_tur_id, out efetivacaoSemestral);
                VS_EfetivacaoSemestral = efetivacaoSemestral;
            }

            UCEfetivacaoNotasPadrao.SetaTurmaDisciplina();
            UCEfetivacaoNotasFinal.SetaTurmaDisciplina();

            if (tur_id > 0)
            {
                // Carrega o combo de disciplinas.
                CarregarDisciplinas(tud_id, tpc_id);
                UCComboAvaliacao1.Titulo = "Avaliação / Tipo de avaliação";

                if (UCNavegacaoLancamentoClasse1.Visible)
                {
                    // Mostra os botões de navegação entre telas.
                    UCNavegacaoLancamentoClasse1.Inicializar(PaginaGestao.EfetivacaoNotas, VS_FormatoAvaliacao, (TUR_TurmaTipo)VS_Turma.tur_tipo, EntTurmaDisciplina);

                    UCNavegacaoLancamentoClasse1._VS_tud_id = Tud_id;
                    UCNavegacaoLancamentoClasse1._VS_fav_id = VS_Turma.fav_id;
                    UCNavegacaoLancamentoClasse1._VS_tur_id = VS_Turma.tur_id;

                    // Verifica se vai exibir o botão de frequência ou não
                    UCNavegacaoLancamentoClasse1.ExibirFrequencia(Tud_id, PaginaGestao.Efetivacao, VS_Turma, VS_FormatoAvaliacao);
                }
            }
        }

        /// <summary>
        /// Verifica o formato de avaliação e carrega as disciplinas de acordo
        /// com o conceito de avaliação.
        /// </summary>
        /// <param name="tud_idSelecionado">Id da disciplina selecionada</param>
        private void CarregarDisciplinas(long tud_idSelecionado, int tpc_idSelecionado)
        {
            _VS_fav_id = VS_Turma.fav_id;

            ESC_Escola esc = new ESC_Escola { esc_id = VS_Turma.esc_id };
            ESC_EscolaBO.GetEntity(esc);

            long doc_id = -1;

            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 && VS_turmaDisciplinaCompartilhada == null)
            {
                doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
            }

            hdnVariacaoFrequencia.Value = VS_FormatoAvaliacao.fav_variacao.ToString();

            DataTable dt = new DataTable();
            switch ((ACA_FormatoAvaliacaoTipo)VS_FormatoAvaliacao.fav_tipo)
            {
                case ACA_FormatoAvaliacaoTipo.ConceitoGlobal:
                    {
                        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && !VS_FormatoAvaliacao.fav_conceitoGlobalDocente)
                        {
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage("Docentes não possuem permissão para efetivar notas da turma " + VS_Turma.tur_codigo + " (" + esc.esc_nome + ").", UtilBO.TipoMensagem.Alerta);
                            Response.Redirect("Busca.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }

                        // Carrega apenas uma disciplina "fake" para o lançamento global.
                        dt = TUR_TurmaDisciplinaBO.CarregaLancamentoGlobal(_VS_tur_id, VS_Turma.tur_codigo, doc_id);

                        if (dt.Rows.Count <= 0)
                        {
                            // Se é docente, e ele não tem permissão de ver a efetivação, redireciona pra
                            // tela de busca.
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage(
                                                            "Você não tem permissão para visualizar a efetivação dessa turma.",
                                                            UtilBO.TipoMensagem.Alerta);

                            Response.Redirect("Busca.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();

                            return;
                        }

                        break;
                    }

                case ACA_FormatoAvaliacaoTipo.Disciplina:
                    {
                        ACA_Avaliacao avaliacao = new ACA_Avaliacao
                        {
                            fav_id = _VS_fav_id,
                            ava_id = _VS_ava_id
                        };
                        ACA_AvaliacaoBO.GetEntity(avaliacao);

                        AvaliacaoTipo tipoAvaliacao = (AvaliacaoTipo)avaliacao.ava_tipo;

                        string disciplinaAdicional = (tipoAvaliacao == AvaliacaoTipo.Final ||
                                                       tipoAvaliacao == AvaliacaoTipo.PeriodicaFinal)
                                                       && !VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica
                                                          ? "Resultado final"
                                                          : string.Empty;

                        // Se avaliação for Final ou Per+Final, mostra linha a mais "Res.Final".
                        dt = TUR_TurmaDisciplinaBO.GetSelect_Efetivacao_By_TurmaDisciplinaAdicional(_VS_tur_id, doc_id, VS_Turma.tur_codigo, disciplinaAdicional, VS_Turma, 0, 0, 0);

                        break;
                    }

                case ACA_FormatoAvaliacaoTipo.GlobalDisciplina:
                    {
                        // Mostra disciplinas + "Conceito global".
                        // Só exibe o conceito global se a visão for diferente de docente
                        // ou se a visão for igual a docente e o formato estiver configurado
                        // para o docente lançar conceito global.
                        string disciplinaAdicional = string.Empty;
                        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
                        {
                            disciplinaAdicional = "Conceito global";
                        }
                        else
                        {
                            if (VS_FormatoAvaliacao.fav_conceitoGlobalDocente)
                            {
                                disciplinaAdicional = "Conceito global";
                            }
                        }

                        dt = TUR_TurmaDisciplinaBO.GetSelect_Efetivacao_By_TurmaDisciplinaAdicional(_VS_tur_id, doc_id, VS_Turma.tur_codigo, disciplinaAdicional, VS_Turma, 0, 0);

                        break;
                    }
            }

            ddlTurmaDisciplina.Items.Clear();

            var turmaDisciplina = VS_turmaDisciplinaCompartilhada != null ? 
                                    (from DataRow dr in dt.Rows
                                     where Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                         && VS_turmaDisciplinaRelacionada.tud_id.ToString() == Convert.ToString(dr["tur_tud_id"].ToString().Split(';')[1])
                                         // a disciplina de docência compartilhada nao terá fechamento
                                         && Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                                     select dr)
                                     : 
                                        tud_idSelecionado > 0 &&
                                    dt.Rows.Cast<DataRow>().Any(p => (Convert.ToString(p["tur_tud_id"].ToString().Split(';')[1]).Equals(tud_idSelecionado.ToString()))) &&
                                    ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_CARREGAR_APENAS_DISCIPLINA_SELECIONADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) ?
                                    (from DataRow dr in dt.Rows
                                     where Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                         && (Convert.ToString(dr["tur_tud_id"].ToString().Split(';')[1]).Equals(tud_idSelecionado.ToString()))
                                             // a disciplina de docência compartilhada nao terá fechamento
                                             && Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                                     select dr)
                                    :
                                    (from DataRow dr in dt.Rows
                                     where Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                             // a disciplina de docência compartilhada nao terá fechamento
                                            && Convert.ToByte(dr["tud_tipo"].ToString()) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                                     select dr);

            if (turmaDisciplina.Count<DataRow>() > 0)
            {
                //Carrega as disciplinas da turma caso a visão do usuário não seja individual, exceto as componentes de regência.
                ddlTurmaDisciplina.DataSource = turmaDisciplina.CopyToDataTable();
                ddlTurmaDisciplina.DataBind();
            }

            if (tud_idSelecionado > 0)
            {
                // Busca o tud_id nos itens do combo.
                var x = from ListItem item in ddlTurmaDisciplina.Items
                        where item.Value.Split(';')[1].Equals(tud_idSelecionado.ToString())
                        select item.Value;

                ddlTurmaDisciplina.SelectedValue =
                    x.Count() > 0
                    ? x.First()
                    : ddlTurmaDisciplina.Items[0].Value;
            }

            ddlTurmaDisciplina_SelectedIndexChanged(ddlTurmaDisciplina, new EventArgs());
        }

        /// <summary>
        /// Redireciona para a página de busca.
        /// </summary>
        /// <param name="msg"></param>
        public void RedirecionaBusca(string msg)
        {
            __SessionWEB.PostMessages = msg;

            if (_VS_URL_Retorno_Efetivacao == Convert.ToByte(URL_Retorno_Efetivacao.PlanejamentoCadastroAula))
            {
                // Envia de volta os ids.
                Session["tur_idPlanejamento"] = _VS_tur_id;
                Session["tud_idPlanejamento"] = _VS_tud_id_Retorno;

                Response.Redirect("~/Classe/Planejamento/CadastroAula.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (_VS_URL_Retorno_Efetivacao == Convert.ToByte(URL_Retorno_Efetivacao.RenovacaoCadastro))
            {
                Response.Redirect("~/Matricula/FechamentoAnoLetivo/Renovacao/Cadastro.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (_VS_URL_Retorno_Efetivacao == Convert.ToByte(URL_Retorno_Efetivacao.EnturmacaoCadastro))
            {
                Response.Redirect("~/Matricula/FechamentoAnoLetivo/Enturmacao/Cadastro.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("~/Classe/Efetivacao/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Método para passar os parametros para o rdl e chamá-lo em seguida
        /// </summary>
        /// <param name="alu_id"></param>
        private void ImprimirRelatorio(long alu_id, EscalaAvaliacaoTipo tipoEscala, int tpc_id)
        {
            string nomeNota = "Nota";
            if (tipoEscala == EscalaAvaliacaoTipo.Pareceres)
            {
                nomeNota = "Conceito";
            }
            else if (tipoEscala == EscalaAvaliacaoTipo.Relatorios)
            {
                nomeNota = "Relatório";
            }

            string parametros =
                "ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                "&tur_id=" + _VS_tur_id +
                "&ava_id=" + _VS_ava_id +
                "&fav_id=" + _VS_fav_id +
                "&alu_id=" + alu_id +
                "&tpc_id=" + tpc_id +
                "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                "&nomeTipoNota=" + nomeNota;

            string report = ((int)ReportNameGestaoAcademica.GestaoAcademicaRegistroAlunoReprovado).ToString();
            
            lkbImprimir.NavigateUrl = CFG_RelatorioBO.LinkReport("Relatorios", report, parametros);
        }

        public void AtualizarStatusEfetivacao(bool pendente)
        {
            divStatusFechamento.Visible = true;
            if (pendente)
            {
                imgStatusFechamento.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "excluir.png";
                lblStatusFechamento.Text = GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.lblStatusFechamento.Text.Pendente").ToString();               
            }
            else
            {
                imgStatusFechamento.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "confirmar.png";
                lblStatusFechamento.Text = GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.lblStatusFechamento.Text.Concluido").ToString();
            }
        }

        public void AlterarTituloJustificativa(string titulo)
        {
            divJustificativa.Attributes["title"] = lblJustificativa.Text = titulo;
        }

        #endregion Métodos

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jquery.PrintBoletim.js"));
                    //recarrega sempre pare pegar os campos que antes estavam invisiveis na tela
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroEfetivacao.js"));

                    if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        if (!Convert.ToString(_btnCancelar.CssClass).Contains("btnMensagemUnload"))
                        {
                            _btnCancelar.CssClass += " btnMensagemUnload";
                        }

                        if (!Convert.ToString(_btnCancelar2.CssClass).Contains("btnMensagemUnload"))
                        {
                            _btnCancelar2.CssClass += " btnMensagemUnload";
                        }
                    }
                }

                UCComboAvaliacao1.IndexChanged += UCComboAvaliacao1_IndexChanged;
                UCAlunoEfetivacaoObservacao.ReturnValues += UCAlunoEfetivacaoObservacao_ReturnValues;

                UCEfetivacaoNotasPadrao.AbrirObservacaoDisciplina += UCEfetivacaoNotas_AbrirObservacaoDisciplina;
                UCEfetivacaoNotasPadrao.AbrirObservacaoConselho += UCEfetivacaoNotas_AbrirObservacaoConselho;
                UCEfetivacaoNotasFinal.AbrirObservacaoConselho += UCEfetivacaoNotas_AbrirObservacaoConselho;

                UCEfetivacaoNotasPadrao.AbrirBoletim += UCEfetivacaoNotas_AbrirBoletim;
                UCEfetivacaoNotasFinal.AbrirBoletim += UCEfetivacaoNotas_AbrirBoletim;

                UCEfetivacaoNotasPadrao.AbrirRelatorio += UCEfetivacaoNotas_AbrirRelatorio;
                UCEfetivacaoNotasFinal.AbrirRelatorio += UCEfetivacaoNotas_AbrirRelatorio;
                UCAlunoEfetivacaoObservacao.AbrirRelatorio += UCAlunoEfetivacaoObservacao_AbrirRelatorio;

                UCEfetivacaoNotasPadrao.AbrirJustificativa += UCEfetivacaoNotas_AbrirJustificativa;
                UCEfetivacaoNotasFinal.AbrirJustificativa += UCEfetivacaoNotas_AbrirJustificativa;
                UCAlunoEfetivacaoObservacao.AbrirJustificativa += UCAlunoEfetivacaoObservacao_AbrirJustificativa;
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                // Adiciona JavaScripts da tela.
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));


                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                    string script = "permiteAlterarResultado=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, ent_id) ? "1" : "0") + ";" +
                        "exibirNotaFinal=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, ent_id) ? "1" : "0") + ";" +
                        "ExibeCompensacao=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, ent_id) ? "1" : "0") + ";" +
                        "MinutosCacheFechamento=" + ApplicationWEB.AppMinutosCacheFechamento + ";";

                    if (sm.IsInAsyncPostBack)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Parametros", script, true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Parametros", script, true);
                    }

                    sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmerEfetivacao.js"));
                }
                else
                {
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                }
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsAlunoEfetivacaoObservacao.js"));
            }

            string script2 = String.Format(
                            "SetConfirmDialogButton('{0}','{1}');",
                            String.Concat(
                                "#",
                                btnImprimirRelatorio.ClientID),
                            "Essa operação irá salvar a efetivação desse aluno.<br /><br />Confirma a operação?");
            Page.ClientScript.RegisterStartupScript(GetType(), btnImprimirRelatorio.ClientID, script2, true);
        }

        #endregion Eventos Page Life Cycle

        #region Eventos

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionaBusca(string.Empty);
        }

        protected void _btnSalvar_Click(object sender, EventArgs e)
        {
            if (_btnSalvar.Visible && Page.IsValid)
            {
                if (UCEfetivacaoNotasPadrao.Visible)
                {
                    UCEfetivacaoNotasPadrao.Salvar();
                }
                else if (UCEfetivacaoNotasFinal.Visible)
                {
                    UCEfetivacaoNotasFinal.Salvar();
                }
            }
        }

        protected void btnSalvarRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Relatório salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                if (UCEfetivacaoNotasPadrao.Visible)
                {
                    UCEfetivacaoNotasPadrao.SalvarRelatorio(hdnIndices.Value, _txtArea.Text, fupAnexoRelatorio.PostedFile, hplAnexoRelatorio.Visible);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('close'); });", true);
            }
            catch (ValidationException err)
            {
                _lblMessageRelatorio.Text = UtilBO.GetErroMessage(err.Message, UtilBO.TipoMensagem.Alerta);
                ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o relatório.", UtilBO.TipoMensagem.Alerta);
            }
        }

        protected void btnImportarAnotacoesAluno_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_VS_JanelaObservacaoAberta && UCEfetivacaoNotasPadrao.Visible)
                {
                    string id = hdnIndices.Value.Split(';')[2];
                    long alu_id = string.IsNullOrEmpty(id) ? 0 : Convert.ToInt64(id);
                    string anotacao = UCEfetivacaoNotasPadrao.btnImportarAnotacoesAluno_Click(alu_id);

                    if (!string.IsNullOrEmpty(_txtArea.Text.Trim()))
                        _txtArea.Text = _txtArea.Text.Trim() + "\r\n\n";
                    _txtArea.Text += anotacao;

                    if (string.IsNullOrEmpty(anotacao))
                    {
                        _lblMessageRelatorio.Text = UtilBO.GetErroMessage("Não há anotações cadastradas para o aluno(a).", UtilBO.TipoMensagem.Alerta);
                    }
                }
            }
            catch (ValidationException err)
            {
                _lblMessageRelatorio.Text = UtilBO.GetErroMessage(err.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                _lblMessageRelatorio.Text = UtilBO.GetErroMessage("Erro ao tentar importar as anotações do aluno.", UtilBO.TipoMensagem.Alerta);
            }
        }

        protected void btnImprimirRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                string id = hdnIndices.Value;
                long alu_idSalvar = _VS_JanelaObservacaoAberta ? UCAlunoEfetivacaoObservacao.VS_alu_id : string.IsNullOrEmpty(id.Split(';')[2]) ? 0 : Convert.ToInt64(id.Split(';')[2]);

                if (alu_idSalvar > 0)
                {
                    string arq_idRelatorio = String.Empty;
                    EscalaAvaliacaoTipo esa_tipo = EscalaAvaliacaoTipo.Numerica;
                    int tpc_id = -1;

                    if (_VS_JanelaObservacaoAberta)
                    {
                        arq_idRelatorio = UCAlunoEfetivacaoObservacao.ImprimirRelatorio(alu_idSalvar, id, _txtArea.Text, fupAnexoRelatorio.PostedFile, hplAnexoRelatorio.Visible, out esa_tipo);
                    }
                    else if (UCEfetivacaoNotasPadrao.Visible)
                    {
                        arq_idRelatorio = UCEfetivacaoNotasPadrao.ImprimirRelatorio(alu_idSalvar, id, _txtArea.Text, fupAnexoRelatorio.PostedFile, hplAnexoRelatorio.Visible, out esa_tipo, out tpc_id);
                    }
                    else if (UCEfetivacaoNotasFinal.Visible)
                    {
                        arq_idRelatorio = UCEfetivacaoNotasFinal.ImprimirRelatorio(alu_idSalvar, id, _txtArea.Text, fupAnexoRelatorio.PostedFile, hplAnexoRelatorio.Visible, out esa_tipo);
                    }

                    //// Carrega o relatorio caso ele tenha sido salvo
                    if (!string.IsNullOrEmpty(arq_idRelatorio) && Convert.ToInt64(arq_idRelatorio) > 0)
                    {
                        SYS_Arquivo arq = new SYS_Arquivo { arq_id = Convert.ToInt64(arq_idRelatorio) };
                        SYS_ArquivoBO.GetEntity(arq);
                        btnExcluirAnexoRelatorio.Visible = true;
                        hplAnexoRelatorio.NavigateUrl = "~/FileHandler.ashx?file=" + arq_idRelatorio;
                        hplAnexoRelatorio.Target = "_blank";
                        hplAnexoRelatorio.Text = arq.arq_nome;
                        hplAnexoRelatorio.Visible = true;
                    }
                    else
                    {
                        btnExcluirAnexoRelatorio.Visible = false;
                        hplAnexoRelatorio.Visible = false;
                    }

                    ImprimirRelatorio(alu_idSalvar, esa_tipo, tpc_id);
                }
                else
                {
                    throw new ValidationException("Erro ao tentar imprimir o relatório do aluno.");
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "RegistroClasse", "$(document).ready(function(){ $('#divRegistroClasse').dialog('open'); });", true);
            }
            catch (ValidationException err)
            {
                _lblMessageRelatorio.Text = UtilBO.GetErroMessage(err.Message, UtilBO.TipoMensagem.Alerta);
                ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar imprimir o relatório do aluno.", UtilBO.TipoMensagem.Alerta);
            }
        }

        protected void ddlTurmaDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            _VS_tur_id = Convert.ToInt64(ddlTurmaDisciplina.SelectedValue.Split(';')[0]);

            // ReCarrega o combo de avaliações.
            UCComboAvaliacao1.CarregarAvaliacao(VS_Turma, Tud_id, VS_Tpc_IdSelecionado, this.VS_IncluirPeriodoFinal, __SessionWEB.__UsuarioWEB.Docente.doc_id);
            
            VS_tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(VS_posicao, ApplicationWEB.AppMinutosCacheLongo);
            UCEfetivacaoNotasPadrao.ddlTurmaDisciplina_SelectedIndexChanged(UCComboAvaliacao1.Count);
            UCEfetivacaoNotasFinal.ddlTurmaDisciplina_SelectedIndexChanged(UCComboAvaliacao1.Count);
            indexChangedAvaliacao = false;

            if (UCComboAvaliacao1.Count > 1)
            {
                if (_VS_ava_id > 0 && UCComboAvaliacao1.ExisteValor(_VS_ava_id))
                {
                    UCComboAvaliacao1.Valor = _VS_ava_id;
                }
                else
                {
                    UCComboAvaliacao1.SelecionaPrimeiroItem();
                }

                if (!indexChangedAvaliacao)
                {
                    UCComboAvaliacao1_IndexChanged();
                }
            }
            
            if (UCNavegacaoLancamentoClasse1.Visible)
            {
                UCNavegacaoLancamentoClasse1._VS_tud_id = Tud_id;

                // Verifica se vai exibir o botão de frequência ou não
                UCNavegacaoLancamentoClasse1.ExibirFrequencia(Tud_id, PaginaGestao.Efetivacao, VS_Turma, VS_FormatoAvaliacao);
            }
        }

        protected void UCComboAvaliacao1_IndexChanged()
        {
            indexChangedAvaliacao = true;
            try
            {
                divStatusFechamento.Visible = false;
                if (UCComboAvaliacao1.Valor > 0)
                {
                    _VS_tur_id = Convert.ToInt64(ddlTurmaDisciplina.SelectedValue.Split(';')[0]);
                    _VS_ava_id = UCComboAvaliacao1.Valor; 
                }
                else
                {
                    _VS_ava_id = -1;    
                }

                if (_VS_ava_id > 0)
                {
                    ACA_Avaliacao avaliacao = new ACA_Avaliacao
                    {
                        fav_id = _VS_fav_id,
                        ava_id = _VS_ava_id
                    };
                    ACA_AvaliacaoBO.GetEntity(avaliacao);

                    if (VS_FormatoAvaliacao.fav_avaliacaoFinalAnalitica && (AvaliacaoTipo)avaliacao.ava_tipo == AvaliacaoTipo.Final)
                    {
                        UCEfetivacaoNotasPadrao.Visible = false;
                        UCEfetivacaoNotasFinal.Visible = true;
                        UCEfetivacaoNotasPadrao.EscondeTelaAlunos(String.Empty);
                        PnlAlunos.Visible = true;
                        UCEfetivacaoNotasFinal.UCComboAvaliacao1_IndexChanged();
                    }
                    else
                    {
                        UCEfetivacaoNotasPadrao.Visible = true;
                        UCEfetivacaoNotasFinal.Visible = false;
                        UCEfetivacaoNotasFinal.EscondeTelaAlunos(String.Empty);
                        PnlAlunos.Visible = true;
                        UCEfetivacaoNotasPadrao.UCComboAvaliacao1_IndexChanged();
                    }
                }
                else
                {
                    UCEfetivacaoNotasPadrao.Visible = true;
                    UCEfetivacaoNotasFinal.Visible = false;
                    UCEfetivacaoNotasFinal.EscondeTelaAlunos(String.Empty);
                    PnlAlunos.Visible = true;
                    UCEfetivacaoNotasPadrao.UCComboAvaliacao1_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                TrataErro(ex, _lblMessage, "carregar os dados");
            }
        }

        protected void btnExcluirAnexoRelatorio_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                btnExcluirAnexoRelatorio.Visible = false;
                hplAnexoRelatorio.Visible = false;
                if (_VS_JanelaObservacaoAberta)
                {
                    UCAlunoEfetivacaoObservacao.UppNotaDisciplinas.Update();
                }
                // Atualiza o gvAlunos.
                else if (UCEfetivacaoNotasPadrao.Visible)
                {
                    UCEfetivacaoNotasPadrao.UppAlunos.Update();
                }
                else if (UCEfetivacaoNotasFinal.Visible)
                {
                    UCEfetivacaoNotasFinal.UppAlunos.Update();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o anexo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvarJustificativa_Click(object sender, EventArgs e)
        {
            try
            {
                if (_VS_JanelaObservacaoAberta)
                {
                    UCAlunoEfetivacaoObservacao.SalvarJustificativaNotaFinal(hdnIndiceJustificativa.Value, txtJustificativa.Text);
                    if (UCEfetivacaoNotasFinal.Visible)
                    {
                        UCEfetivacaoNotasFinal.UppAlunos.Update();
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Justificativa", "$(document).ready(function(){ $('.divJustificativa').dialog('close'); });", true);
                }
                else
                {
                    if (UCEfetivacaoNotasPadrao.Visible)
                    {
                        UCEfetivacaoNotasPadrao.SalvarJustificativaPosConselho(hdnIndiceJustificativa.Value, txtJustificativa.Text);
                    }
                    else if (UCEfetivacaoNotasFinal.Visible)
                    {
                        UCEfetivacaoNotasFinal.SalvarJustificativaNotaFinal(hdnIndiceJustificativa.Value, txtJustificativa.Text);
                    }

                    _lblMessage.Text = UtilBO.GetErroMessage(lblJustificativa.Text + " do aluno salva com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Justificativa", "$(document).ready(function(){ $('.divJustificativa').dialog('close'); scrollToTop(); });", true);
                }
            }
            catch (ValidationException ex)
            {
                lblMsgJustificativa.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Justificativa", "$(document).ready(function(){ $('.divJustificativa').dialog('open'); });", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a " + lblJustificativa.Text.ToLower() + " do aluno.", UtilBO.TipoMensagem.Alerta);
            }
        }

        protected void UCAlunoEfetivacaoObservacao_ReturnValues(IDictionary<string, object> parameters, bool sucessoSalvarNotaFinal, List<CLS_AlunoAvaliacaoTurmaDisciplina> listaAtualizacaoEfetivacao, byte resultado, List<MTR_MatriculaTurmaDisciplina> listaMatriculaTurmaDisciplina)
        {
            try
            {
                if (UCEfetivacaoNotasPadrao.Visible)
                {
                    UCEfetivacaoNotasPadrao.UCAlunoEfetivacaoObservacao_ReturnValues(Convert.ToInt32(hdnIndiceObservacao.Value), parameters["observacao"], (eTipoObservacao)parameters["tipoObservacao"], resultado);
                }
                else if (UCEfetivacaoNotasFinal.Visible)
                {
                    byte parecerFinal = 0;
                    if (listaMatriculaTurmaDisciplina.Count > 0 && listaMatriculaTurmaDisciplina.Any(p => p.tud_id == Tud_id))
                    {
                        parecerFinal = listaMatriculaTurmaDisciplina.Find(p => p.tud_id == Tud_id).mtd_resultado;
                    }
                    UCEfetivacaoNotasFinal.UCAlunoEfetivacaoObservacao_ReturnValues(Convert.ToInt32(hdnIndiceObservacao.Value), parameters["observacao"], (eTipoObservacao)parameters["tipoObservacao"], sucessoSalvarNotaFinal, listaAtualizacaoEfetivacao, resultado, parecerFinal);
                }

                if (sucessoSalvarNotaFinal)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharObservacao", "var exibirMensagemConfirmacao=false;$('#divCadastroObservacao').dialog('close'); scrollToTop();", true);
                    _VS_JanelaObservacaoAberta = false;
                }
                updObservacao.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a observação.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCEfetivacaoNotas_AbrirObservacaoDisciplina(int indiceAluno, long tud_id, long alu_id, int mtu_id, int mtd_id, string dadosAluno, string titulo, bool periodoFechado)
        {
            hdnIndiceObservacao.Value = indiceAluno.ToString();
            UCAlunoEfetivacaoObservacao.CarregarObservacoes(tud_id, alu_id, mtu_id, mtd_id, _VS_fav_id, _VS_ava_id, dadosAluno, periodoFechado);
            UCAlunoEfetivacaoObservacao.VS_MensagemLogEfetivacaoObservacao = titulo + " | ";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "divCadastroObservacao",
                "$('#divCadastroObservacao').dialog('option', 'title', '" + titulo + "');" +
                "var exibirMensagemConfirmacao=false;$('#divCadastroObservacao').dialog('open'); ", true);
            _VS_JanelaObservacaoAberta = true;
            updObservacao.Update();
        }

        private void UCEfetivacaoNotas_AbrirObservacaoConselho(int indiceAluno, long tur_id, long alu_id, int mtu_id, string dadosAluno, string titulo, bool fav_avaliacaoFinalAnalitica, AvaliacaoTipo ava_tipo, int cal_id, int tpc_id, bool efetivacaoSemestral, bool periodoFechado,  int ava_idUltimoBimestre = 0)
        {
            hdnIndiceObservacao.Value = indiceAluno.ToString();

            int ava_id = fav_avaliacaoFinalAnalitica && ava_tipo == AvaliacaoTipo.Final ? ava_idUltimoBimestre : _VS_ava_id;
            UCAlunoEfetivacaoObservacao.CarregarObservacoes(tur_id, alu_id, mtu_id, _VS_fav_id, ava_id, dadosAluno, fav_avaliacaoFinalAnalitica, ava_tipo, cal_id, tpc_id, efetivacaoSemestral, periodoFechado);
            UCAlunoEfetivacaoObservacao.VS_MensagemLogEfetivacaoObservacao = titulo + " | ";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "divCadastroObservacao",
                "$('#divCadastroObservacao').dialog('option', 'title', '" + titulo + "');" +
                "var exibirMensagemConfirmacao=false;$('#divCadastroObservacao').dialog('open'); ", true);
            _VS_JanelaObservacaoAberta = true;
            updObservacao.Update();
        }

        private void UCEfetivacaoNotas_AbrirBoletim(long alu_id, int tpc_id, int mtu_id)
        {
            UCBoletimCompletoAluno.CarregarDadosBoletim(alu_id, tpc_id, mtu_id);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "BoletimCompletoAluno", "$(document).ready(function(){ $('#divBoletimCompleto').dialog('open'); });", true);
            updBoletimCompleto.Update();
        }

        private void UCAlunoEfetivacaoObservacao_AbrirRelatorio(string idRelatorio, string nota, string arq_idRelatorio, string dadosAluno)
        {
            lblDadosRelatorio.Text = dadosAluno;
            hdnIndices.Value = idRelatorio;
            _txtArea.Text = nota;

            if (!string.IsNullOrEmpty(arq_idRelatorio) && Convert.ToInt64(arq_idRelatorio) > 0)
            {
                SYS_Arquivo arq = new SYS_Arquivo { arq_id = Convert.ToInt64(arq_idRelatorio) };
                SYS_ArquivoBO.GetEntity(arq);
                if (!arq.IsNew)
                {
                    btnExcluirAnexoRelatorio.Visible = true;
                    hplAnexoRelatorio.NavigateUrl = "~/FileHandler.ashx?file=" + arq_idRelatorio;
                    hplAnexoRelatorio.Target = "_blank";
                    hplAnexoRelatorio.Text = arq.arq_nome;
                    hplAnexoRelatorio.Visible = true;
                }
                else
                {
                    btnExcluirAnexoRelatorio.Visible = false;
                    hplAnexoRelatorio.Visible = false;
                }
            }
            else
            {
                btnExcluirAnexoRelatorio.Visible = false;
                hplAnexoRelatorio.Visible = false;
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "RelatorioNota", "$(document).ready(function(){ $('#divRelatorio').dialog('open'); });", true);
        }

        private void UCEfetivacaoNotas_AbrirRelatorio(string idRelatorio, string nota, string arq_idRelatorio, string dadosAluno)
        {
            _VS_JanelaObservacaoAberta = false;
            UCAlunoEfetivacaoObservacao_AbrirRelatorio(idRelatorio, nota, arq_idRelatorio, dadosAluno);
        }

        private void UCAlunoEfetivacaoObservacao_AbrirJustificativa(string id, string textoJustificativa, string pes_nome, string dis_nome)
        {
            string dadosAluno;
            if (pes_nome.IndexOf("Nome do aluno") < 0)
                dadosAluno = "<b>Nome do aluno:</b> " + pes_nome;
            else
                dadosAluno = pes_nome;

            if (!String.IsNullOrEmpty(dis_nome))
                dadosAluno += "<br /><b>" + GetGlobalResourceObject("UserControl", "EfetivacaoNotas.UCEfetivacaoNotas.litHeadNomeDisciplina.Text") + ":</b> " + dis_nome;

            lblDadosJustificativa.Text = dadosAluno;
            hdnIndiceJustificativa.Value = id;
            txtJustificativa.Text = textoJustificativa;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "divJustificativa", "$(document).ready(function(){ $('.divJustificativa').dialog('open'); });", true);

            updJustificativa.Update();
        }

        private void UCEfetivacaoNotas_AbrirJustificativa(string id, string textoJustificativa, string pes_nome, string dis_nome)
        {
            _VS_JanelaObservacaoAberta = false;
            UCAlunoEfetivacaoObservacao_AbrirJustificativa(id, textoJustificativa, pes_nome, dis_nome);
        }

        #endregion Eventos
    }
}