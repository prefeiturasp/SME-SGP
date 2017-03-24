using GestaoEscolar.Api.Areas.HelpPage.Attributes;
using GestaoEscolar.Api.Controllers.Base;
using GestaoEscolar.Api.Models;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using static MSTech.GestaoEscolar.BLL.Struct_MinhasTurmas;

namespace GestaoEscolar.Api.Areas.v1
{
    [RoutePrefix("api/v1/Turmas")]
    public class TurmasController : BaseApiController
    {
        /// <summary>
        /// Busca dados das minhas turmas de acordo com os filtros
        /// </summary>
        /// <param name="escolaId">(Obrigarório) Id da escola</param>
        /// <param name="unidadeId">(Obrigarório) Id da unidade escolar</param>
        /// <param name="calendarioId">(Obrigarório) Id do calendário escolar</param>
        /// <param name="cursoId">(Obrigarório) Id do curso</param>
        /// <param name="curriculoId">(Obrigarório) Id do currículo</param>
        /// <param name="periodoId">(Opcional) Id do período</param>
        /// <param name="cicloId">(Opcional) Id do ciclo</param>
        /// <param name="codigoTurma">(Opcional) Código da turma</param>
        /// <param name="X-Order">(Opcional - Padrão = nome) Coluna que será feita a ordenação: nome, curso, turma ou tipoDocencia</param>
        /// <param name="X-Order-Asc">(Opcional - Padrão = true) True para crescente e False para decrescente</param>
        /// <param name="X-Pagination-Per-Page">(Opcional - Padrão = 10) Número de itens por página</param>
        /// <param name="X-Pagination-Current-Page">(Opcional - Padrão = 1) Número da página</param>
        /// <returns>Retorna um objeto de minhas turmas</returns>
        [Route("MinhasTurmas")]
        [ResponseType(typeof(MinhasTurmas))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetMinhasTurmas(int escolaId, int unidadeId, int calendarioId,
                                                   int cursoId, int curriculoId, int periodoId = 0,
                                                   int cicloId = 0, string codigoTurma = "",
                                                   [FromHeader] string XOrder = "", [FromHeader] bool XOrderAsc = true,
                                                   [FromHeader] int XPaginationPerPage = 10, [FromHeader] int XPaginationCurrentPage = 1)
        {
            try
            {
                List<Struct_MinhasTurmas> obj = new List<Struct_MinhasTurmas>();
                IEnumerable<Struct_Turmas> turmasOrdenadas = new List<Struct_Turmas>();
                MinhasTurmas ret = new MinhasTurmas();
                ret.Turmas = new List<Turma>();

                obj = TUR_TurmaBO.SelecionaPorFiltrosMinhasTurmas(escolaId, unidadeId, calendarioId, cursoId, curriculoId, periodoId,
                                                               __userLogged.Usuario.ent_id, codigoTurma, cicloId, ApplicationWEB.AppMinutosCacheLongo);
                turmasOrdenadas = obj.First().Turmas;

                int qtdRegistros = turmasOrdenadas.Count();
                if (obj.Count > 0)
                {
                    #region Ordenar

                    Func<Struct_Turmas, Object> orderByFunc = null;
                    switch (XOrder)
                    {
                        case "curso": orderByFunc = item => item.tud_nome; break;
                        case "turno": orderByFunc = item => item.tud_nome; break;
                        case "tipoDocencia": orderByFunc = item => item.tud_nome; break;
                        default:
                            orderByFunc = item => item.tur_codigo; break;
                    }

                    if (XOrderAsc)
                        turmasOrdenadas = turmasOrdenadas.OrderBy(orderByFunc).ThenBy(n => n.tud_nome);
                    else
                        turmasOrdenadas = turmasOrdenadas.OrderByDescending(orderByFunc).ThenBy(n => n.tud_nome); ;

                    #endregion

                    #region Paginar

                    XPaginationCurrentPage = XPaginationCurrentPage > 0 ? (XPaginationCurrentPage - 1) : 0;
                    var turmas = turmasOrdenadas.Skip(XPaginationPerPage * XPaginationCurrentPage)
                        .Take(XPaginationPerPage).Select(p => new Turma
                        {
                            codigo = p.tur_codigo,
                            curso = p.tur_curso,
                            nome = p.tud_nome,
                            tipoDocente = p.TipoDocencia,
                            turmaDisciplinaId = p.tud_id,
                            turmaDocentePosicao = (byte)p.tdt_posicao,
                            turmaId = p.tur_id,
                            turno = p.tur_turno,
                            AulasDadasVisivel = p.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                                   && p.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.Experiencia,
                            AulasDadasOk = p.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                                   && p.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.Experiencia
                                                   && p.aulasPrevistasPreenchida
                        });
                    #endregion

                    ret = new MinhasTurmas
                    {
                        calendarioId = calendarioId,
                        escolaNome = ESC_EscolaBO.GetEntity(new ESC_Escola { esc_id = escolaId}).esc_nome,
                        escolaId = escolaId,
                        unidadeId = unidadeId,
                        curriculoId = curriculoId,
                        cursoId = cursoId,
                        periodoId = periodoId,
                        cicloId = cicloId,
                        Turmas = turmas,
                        qtdRegistros = qtdRegistros
                    };
                }

                return Request.CreateResponse(HttpStatusCode.OK, ret);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Busca dados das minhas turmas do docente
        /// </summary>
        /// <returns>Retorna um lista de minhas turmas por escola</returns>
        [Route("Docente/MinhasTurmas")]
        [ResponseType(typeof(IEnumerable<MinhasTurmas>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized, HttpStatusCode.NoContent)]
        public HttpResponseMessage GetMinhasTurmasDocente()
        {
            try
            {
                var dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__userLogged.Usuario.ent_id, __userLogged.Docente.doc_id, ApplicationWEB.AppMinutosCacheCurto);
                var dadosEscolasAtivas = dados.Where(p => p.Turmas.Any(t => t.tur_situacao == (byte)TUR_TurmaSituacao.Ativo)).ToList();

                if (dadosEscolasAtivas.Count == 0)
                {  // se o docente não possuir nenhuma turma - exibir a mensagem informativa
                    var msg = CustomResource.GetGlobalResourceObject("Academico", "ControleTurma.Busca.DocenteSemTurma");
                    return Request.CreateResponse(HttpStatusCode.NoContent, msg);
                }

                var ret = dadosEscolasAtivas.Select(p => new MinhasTurmasDocente
                {
                    calendarioId = p.cal_id,
                    escolaId = p.esc_id,
                    unidadeId = p.uni_id,
                    escolaNome = p.esc_nome,
                    Turmas = p.Turmas.Select(
                            t => new Turma {
                                codigo = t.tur_codigo,
                                        curso = t.tur_curso,
                                        nome = t.tud_nome,
                                        tipoDocente = t.TipoDocencia,
                                        turmaDisciplinaId = t.tud_id,
                                        turmaDocentePosicao = (byte)t.tdt_posicao,
                                        turmaId = t.tur_id,
                                        turno = t.tur_turno,
                                        AulasDadasVisivel = t.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                                               && t.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.Experiencia,
                                        AulasDadasOk = t.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                                               && t.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.Experiencia
                                                               && t.aulasPrevistasPreenchida
                            })
                });

                return Request.CreateResponse(HttpStatusCode.OK, ret);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Busca os dados das aulas dadas de acordo com o parâmetro
        /// </summary>
        /// <param name="turmaDisciplinaId">(Obrigarório) Id da turma disciplina</param>
        /// <param name="escolaId">(Obrigarório) Id da escola</param>
        /// <param name="unidadeId">(Obrigarório) Id da unidade escolar</param>
        /// <param name="calendarioId">(Opcional para docente) Id do calendário escolar</param>
        /// <param name="cursoId">(Opcional para docente) Id do curso</param>
        /// <param name="curriculoId">(Opcional para docente) Id do currículo</param>
        /// <param name="periodoId">(Opcional para docente) Id do período</param>
        /// <param name="cicloId">(Opcional para docente) Id do ciclo</param>
        /// <param name="turmaId">(Opcional para docente) Id da turma</param>
        /// <param name="turmaDocentePosicao">(Opcional para docente) Posição do docente</param>
        /// <returns>Retorna objeto de aulas dadas</returns>
        [Route("AulasDadas")]
        [ResponseType(typeof(AulasDadas))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetTurmasComponenteCurricular(long turmaDisciplinaId, 
                                                   int escolaId, int unidadeId = 0, int calendarioId = 0,
                                                   int cursoId = 0, int curriculoId = 0, int periodoId = 0,
                                                   int cicloId = 0, int turmaId = 0, 
                                                   byte turmaDocentePosicao = 0)
        {
            try
            {
                long docente = __userLogged.Docente.doc_id;
                Struct_Turmas turmaSelecionada;
                IEnumerable<TurmaCompCur> turmasComponenteCurricular;
                IEnumerable<jsonObject> tiposDocente;
                bool exibirTipoDocente;
                if (docente == 0)
                {
                    List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorFiltrosMinhasTurmas(escolaId, unidadeId, calendarioId, cursoId, curriculoId, periodoId,
                                               __userLogged.Usuario.ent_id, "", cicloId, ApplicationWEB.AppMinutosCacheCurto);

                    var dadosTurmas = dados.Find(p => p.esc_id == escolaId).Turmas;

                    turmaSelecionada = dadosTurmas.Where(p => p.tud_id == turmaDisciplinaId && (turmaDocentePosicao <= 0 || p.tdt_posicao == turmaDocentePosicao)).ToList().FirstOrDefault();

                    turmasComponenteCurricular = dadosTurmas.Where(d => d.cal_id == turmaSelecionada.cal_id).Select(p => new TurmaCompCur
                    {
                        id = p.tur_id.ToString(),
                        turmaDisciplinaId = p.tud_id.ToString(),
                        text = p.EscolaTurmaDisciplina
                    });

                    exibirTipoDocente = true;
                    tiposDocente = EnumToJsonObject<EnumTipoDocente>();
                }
                else
                {
                    exibirTipoDocente = false;
                    tiposDocente = new List<jsonObject>();
                    List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(__userLogged.Usuario.ent_id, docente, ApplicationWEB.AppMinutosCacheCurto, true);

                    var dadosTurmas = dados.SelectMany(p => p.Turmas);

                    turmaSelecionada = dadosTurmas.Where(p => p.tud_id == turmaDisciplinaId).ToList().First();

                    // Filtra as turmas do calendário sendo exibido na popup.
                    dadosTurmas = dadosTurmas.Where(p => p.cal_id == turmaSelecionada.cal_id);

                    List<Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, escolaId, turmaSelecionada.cal_id, true);
                    turmasComponenteCurricular = dadosTurmasAtivas.Select(p => new TurmaCompCur
                    {
                        id = p.tur_id.ToString(),
                        turmaDisciplinaId = p.tud_id.ToString(),
                        turmaDisciplinaTipo = p.tud_tipo,
                        turmaDocentePosicao = p.tdt_posicao.ToString(),
                        text = p.EscolaTurmaDisciplina
                    });
                }

                var aulasDadas = new AulasDadas
                {
                    nomeEscola = turmaSelecionada.tur_escolaUnidade,
                    escolaId = turmaSelecionada.esc_id,
                    nomeCalendario = turmaSelecionada.tur_calendario,
                    calendarioId = turmaSelecionada.cal_id,
                    turmasComponenteCurricular = turmasComponenteCurricular,
                    turmaId = turmaSelecionada.tur_id,
                    turmaDisciplinaId = turmaSelecionada.tud_id.ToString(),
                    tipoDocenteId = turmaSelecionada.tdt_id,
                    fechamentoAutomatico = turmaSelecionada. fav_fechamentoAutomatico,
                    exibirTipoDocente = exibirTipoDocente,
                    tiposDocente = tiposDocente
                };

                return Request.CreateResponse(HttpStatusCode.OK, aulasDadas);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Busca lista dos períodos e aulas de uma turma
        /// </summary>
        /// <param name="calendarioId">(Obrigarório) Id do calendário escolar</param>
        /// <param name="turmaId">(Obrigarório) Id da turma</param>
        /// <param name="turmaDisciplinaId">(Obrigarório) Id da turma disciplina</param>
        /// <param name="turmaDocentePosicao">(Obrigarório) Posição do docente</param>
        /// <returns>Retorna lista de períodos e aulas de uma turma</returns>
        [Route("PeriodosAulas")]
        [ResponseType(typeof(List<PeriodoAula>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetPeriodosAulasTurma(int calendarioId, int turmaId, long turmaDisciplinaId,
                                                   byte turmaDocentePosicao)
        {
            try
            {
                var aulas = ACA_CalendarioPeriodoBO.Seleciona_QtdeAulas_TurmaDiscplina(turmaId, turmaDisciplinaId, calendarioId, turmaDocentePosicao, __userLogged.Docente.doc_id);
                var periodosAulas = aulas.AsEnumerable().Select(p => new PeriodoAula
                {
                    aulasDadas = p.Field<int>("aulasDadas"),
                    aulasPrevistas = p.Field<int?>("aulasPrevistas"),
                    aulasRepostas = p.Field<int>("aulasRepostas"),
                    periodo = p.Field<string>("periodo"),
                    periodoCalendario = p.Field<string>("cap_descricao"),
                    tipoPeriodoCalendarioId = p.Field<int>("tpc_id"),
                    periodoDataFim = p.Field<DateTime>("cap_dataFim"),
                    permitirEditar = PermitirEditarPeriodoAula(turmaDocentePosicao,
                                                               p.Field<int>("tpc_id"),
                                                               calendarioId,
                                                               turmaId,
                                                               p.Field<DateTime>("cap_dataFim")
                                                               )
                });
                return Request.CreateResponse(HttpStatusCode.OK, periodosAulas);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Atualiza os períodos de aulas previstas
        /// </summary>
        /// <param name="turmaDisciplina">Objeto contendo os dados referêntes as turmas previstas que serão alteradas</param>
        /// <returns>Mensagem de sucesso ou falha na alteração</returns>
        [Route("AulasDadas/Previstas")]
        [ResponseType(typeof(List<PeriodoAula>))]
        [ResponseCodes(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized)]
        [HttpPut]
        public HttpResponseMessage PutAulasDadasPrevistasTurma(TurmaDisciplinaAulaPrevista turmaDisciplina)
        {
            try
            {
                if (turmaDisciplina == null)
                    throw new ArgumentException("Objeto do tipo 'TurmaDisciplinaAulaPrevista' está nulo.");

                var permitir = PermitirEditarPeriodoAula(turmaDisciplina.turmaDocentePosicao, turmaDisciplina.aulasPrevistas.First().tipoPeriodoCalendarioId,
                                                         turmaDisciplina.calendarioId, turmaDisciplina.turmaId, turmaDisciplina.periodoDataFim);
                if (!permitir)
                    throw new ArgumentException("O usuário não possui permissão para alterar as aulas previstas.");

                List<TUR_TurmaDisciplinaAulaPrevista> aulasPrevistas = TUR_TurmaDisciplinaAulaPrevistaBO.SelecionaPorDisciplina(turmaDisciplina.turmaDisciplinaId);
                var totalPrevistas = 0;
                List<TUR_TurmaDisciplinaAulaPrevista> listaSalvar = new List<TUR_TurmaDisciplinaAulaPrevista>();
                List<TUR_TurmaDisciplinaAulaPrevista> listaProcessarPend = new List<TUR_TurmaDisciplinaAulaPrevista>();

                foreach (AulaPrevista aula in turmaDisciplina.aulasPrevistas)
                {
                    var qtAulasPrevistas = aula.qtAulasPrevistas;
                    if (qtAulasPrevistas < 1)
                        throw new ArgumentException("Quantidade de aulas previstas deve ser maior que 0.");

                    TUR_TurmaDisciplinaAulaPrevista aulaPrevista = aulasPrevistas.Find(p => p.tpc_id == aula.tipoPeriodoCalendarioId);
                    if (aulaPrevista == null)
                    {
                        aulaPrevista = new TUR_TurmaDisciplinaAulaPrevista();

                        // Seta os dados para uma nova insercao.
                        aulaPrevista.tud_id = turmaDisciplina.turmaDisciplinaId;
                        aulaPrevista.tpc_id = aula.tipoPeriodoCalendarioId;
                        aulaPrevista.tap_registrosCorrigidos = false;
                    }
                    totalPrevistas += qtAulasPrevistas;

                    if ((aulaPrevista.tap_registrosCorrigidos && aulaPrevista.tap_aulasPrevitas == qtAulasPrevistas))
                        continue;

                    aulaPrevista.tap_registrosCorrigidos = aulaPrevista.tap_aulasPrevitas == qtAulasPrevistas;

                    if (!(aulaPrevista.tap_aulasPrevitas == qtAulasPrevistas))
                        listaProcessarPend.Add(aulaPrevista);

                    // Atualiza ou seta as aulas previstas.
                    aulaPrevista.tap_aulasPrevitas = qtAulasPrevistas;

                    aulaPrevista.tud_tipo = turmaDisciplina.turmaDisciplinaTipo;

                    listaSalvar.Add(aulaPrevista);
                }

                Guid ent_id = __userLogged.Usuario.ent_id;
                long doc_id = __userLogged.Docente.doc_id;

                if (TUR_TurmaDisciplinaAulaPrevistaBO.SalvarAulasPrevistas(listaSalvar, listaProcessarPend, ent_id, turmaDisciplina.escolaId, doc_id, turmaDisciplina.fechamentoAutomatico))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Aulas previstas | tud_id: " + turmaDisciplina.turmaDisciplinaId);
                    return Request.CreateResponse(HttpStatusCode.OK, "Aulas previstas alteradas com sucesso.");
                }
                else
                {
                    throw new Exception("Não foi possível salvar as aulas previstas.");
                }
            }
            catch (ArgumentException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private bool PermitirEditarPeriodoAula(byte tdt_posicao, int tpc_id, int cal_id, long tur_id, DateTime cap_dataFim, bool permiteSalvarAulasPrevistas = true)
        {
            if (__userLogged.Docente.doc_id == 0)
                return false;

            bool efetivado = false;

            //Se o bimestre está ativo ou nem começou então não está efetivado
            if (DateTime.Today <= cap_dataFim)
                efetivado = false;
            else
            {
                List<ACA_Evento> lstEventos = ACA_EventoBO.GetEntity_Efetivacao_List(cal_id, tur_id, __userLogged.Grupo.gru_id, __userLogged.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, false);

                efetivado = !lstEventos.Exists(p => p.tpc_id == tpc_id && p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __userLogged.Usuario.ent_id) &&
                                                    DateTime.Today >= p.evt_dataInicio && DateTime.Today <= p.evt_dataFim);
            }

            // Somente os docentes titulares e o especial podem alterar ou salvar e o período não pode estar efetivado
            if ((!(tdt_posicao == (byte)EnumTipoDocente.Titular ||
                       tdt_posicao == (byte)EnumTipoDocente.SegundoTitular ||
                       tdt_posicao == (byte)EnumTipoDocente.Especial) ||
                       !permiteSalvarAulasPrevistas) || efetivado)
            {
                return false;
            }
            return true;
        }
    }
}
