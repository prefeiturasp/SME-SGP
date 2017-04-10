using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using MSTech.CoreSSO.DAL;
using MSTech.CoreSSO.Entities;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Util;
using MSTech.Validation.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    public class ApiBO
    {
        #region Propriedades

        private static bool? parametroOrientacoesCurricularesAula;

        /// <summary>
        /// Valor do parâmetro acadêmico PLANEJAMENTO_ANUAL_ORIENTACOES_CURRICULARES_AULAS
        /// </summary>
        private static bool ParametroOrientacoesCurricularesAula
        {
            get
            {
                return (bool)
                       (parametroOrientacoesCurricularesAula ??
                       (parametroOrientacoesCurricularesAula =
                       ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_ORIENTACOES_CURRICULARES_AULAS, new Guid())));
            }
        }

        #endregion Propriedades

        #region Métodos

        #region Sistema Diário de classe

        /// <summary>
        /// retorna uma lista com registros pelo ent_id.
        /// quando a dataBase não for passado, apenas registros ativos serão retornados,
        /// caso contrario apenas registros criados ou alterados apos esta data.
        /// </summary>
        /// <param name="ent_id">id da entidade</param>
        /// <param name="dataBase">data base para criação/alteração dos registros</param>
        /// <returns></returns>
        public static List<ACA_TipoAnotacaoAlunoDTO> SelecionarTiposAnotacoesAlunoPorEntidade(int id, Guid ent_id, DateTime dataBase)
        {

            ApiDAO dao = new ApiDAO();

            DataTable dt = dao.SelecionarTiposAnotacoesAlunoPorEntidade(id, ent_id, dataBase);

            if (dt.Rows.Count > 0)
            {
                List<ACA_TipoAnotacaoAlunoDTO> anotacoes = (
                    from DataRow r in dt.Rows
                    select (ACA_TipoAnotacaoAlunoDTO)GestaoEscolarUtilBO.DataRowToEntity(r,
                    new ACA_TipoAnotacaoAlunoDTO())).ToList();

                return anotacoes;
            }

            return null;
        }

        /// <summary>
        /// Retorna uma lista com os tipos de docentes e suas permissões.
        /// </summary>
        /// <returns></returns>
        public static ListagemPermissoesTipoDocenteDTO BuscarPermissoesTipoDocente()
        {
            ListagemPermissoesTipoDocenteDTO dtoSaida = null;

            try
            {
                dtoSaida = new ListagemPermissoesTipoDocenteDTO();
                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.BuscarTiposDocente();

                if (dt.Rows.Count == 0)
                {
                    return dtoSaida;
                }

                dtoSaida.TipoDocente = (
                        from DataRow dr in dt.Rows
                        group dr by dr["tdc_id"] into r
                        select (TipoDocente)GestaoEscolarUtilBO.DataRowToEntity(r.First(), new TipoDocente
                        {
                            PermissaoDocente = (
                                from row in r.AsEnumerable()
                                select (PermissaoDocente)GestaoEscolarUtilBO.DataRowToEntity(row, new PermissaoDocente())
                            ).ToList()
                        })
                    ).ToList();
            }
            catch (Exception e)
            {
                dtoSaida = new ListagemPermissoesTipoDocenteDTO();
                dtoSaida.Status = 1;
                dtoSaida.StatusDescription = e.Message;
            }

            return dtoSaida;
        }

        /// <summary>
        /// Retorna arquivo da biblioteca do gestão.
        /// </summary>
        /// <param name="dtoEntrada"></param>
        /// <returns></returns>
        public static DownloadArquivoSaidaDTO BuscarArquivoDownload(DownloadArquivoEntradaDTO dtoEntrada)
        {
            DownloadArquivoSaidaDTO dtoSaida = null;

            try
            {
                dtoSaida = new DownloadArquivoSaidaDTO();

                SYS_Arquivo arquivo = new SYS_Arquivo
                {
                    arq_id = dtoEntrada.arq_id
                };
                SYS_ArquivoBO.GetEntity(arquivo);

                if (arquivo.IsNew)
                {
                    return dtoSaida;
                }

                dtoSaida.Arq_data = Convert.ToBase64String(arquivo.arq_data);
            }
            catch (Exception e)
            {
                dtoSaida = new DownloadArquivoSaidaDTO();
                dtoSaida.Status = 1;
                dtoSaida.StatusDescription = e.Message;
            }

            return dtoSaida;
        }

        /// <summary>
        /// Retorna listagem com todos os arquivos ativos de biblioteca.
        /// </summary>
        /// <returns></returns>
        public static ListagemArquivoSaidaDTO BuscaArquivosBiblioteca()
        {
            ListagemArquivoSaidaDTO dtoSaida = null;

            try
            {
                dtoSaida = new ListagemArquivoSaidaDTO();

                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.BuscaArquivosBiblioteca();

                if (dt.Rows.Count == 0)
                {
                    return dtoSaida;
                }

                dtoSaida.Arquivos = (
                     from dr in dt.AsEnumerable()
                     select new Arquivo
                     {
                         Arq_id = Convert.ToInt32(dr["arq_id"]),
                         Arq_nome = Convert.ToString(dr["arq_nome"]),
                         Arq_typeMime = Convert.ToString(dr["arq_typeMime"]),
                         Arq_dataAlteracao = Convert.ToDateTime(dr["arq_dataAlteracao"]).ToString("dd/MM/yyyy HH:mm:ss.fff")
                     }).ToList();
            }
            catch (Exception e)
            {
                dtoSaida = new ListagemArquivoSaidaDTO();
                dtoSaida.Status = 1;
                dtoSaida.StatusDescription = e.Message;
            }

            return dtoSaida;
        }


        /// <summary>
        /// Busca os eventos de fechamento de acordo com os parâmetros de entrada
        /// </summary>
        /// <param name="buscaEventosEntrada"></param>
        /// <returns></returns>
        public static BuscaEventosSaidaDTO BuscaEventos(BuscaEventosEntradaDTO buscaEventosEntrada)
        {
            BuscaEventosSaidaDTO dtoSaida = null;

            try
            {
                dtoSaida = new BuscaEventosSaidaDTO();

                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.BuscaEventos(buscaEventosEntrada.esc_id);

                if (dt.Rows.Count == 0)
                {
                    return dtoSaida;
                }

                dtoSaida.ListaEventos = (
                                            from dr in dt.AsEnumerable()
                                            select (ACA_Evento)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_Evento())
                                        ).ToList();
            }
            catch (Exception e)
            {
                dtoSaida = new BuscaEventosSaidaDTO();
                dtoSaida.Status = 1;
                dtoSaida.StatusDescription = e.Message;
            }

            return dtoSaida;
        }

        /// <summary>
        /// Relação de compensações de falta por escola
        /// </summary>
        /// <param name="filtroEntrada">Escola e data da ultima sincronização.</param>
        /// <returns>CLS_CompensacaoAusencia</returns>
        public static ListagemCompensacaoFaltaSaidaDTO BuscaCompensacaoFalta(ListagemCompensacaoFaltaEntradaDTO filtroEntrada)
        {
            ListagemCompensacaoFaltaSaidaDTO dtoSaida = new ListagemCompensacaoFaltaSaidaDTO();
            ApiDAO dao = new ApiDAO();

            List<CompensacaoFalta> list = new List<CompensacaoFalta>();
            try
            {
                if (filtroEntrada.Esc_id <= 0)
                {
                    throw new EscolaVaziaException();
                }

                DataTable dt = dao.BuscaCompensacaoFalta(filtroEntrada.Esc_id, filtroEntrada.Tur_id, Convert.ToDateTime(filtroEntrada.SyncDate));
                if (dt.Rows.Count == 0)
                {
                    return dtoSaida;
                }

                dtoSaida.CompensacaoFalta = (
                        from DataRow row in dt.Rows
                        group row by new { tud_id = row["tud_id"], cap_id = row["cpa_id"] }
                            into r
                        select (CompensacaoFalta)GestaoEscolarUtilBO.DataRowToEntity(r.First(),
                            new CompensacaoFalta
                            {
                                Alunos = (
                                    from DataRow ra in r
                                    group ra by ra["alu_id"] into d
                                    select (AlunoCF)GestaoEscolarUtilBO.DataRowToEntity(d.First(), new AlunoCF())
                                ).ToList()
                            })
                    ).ToList();
            }
            catch (Exception e)
            {
                dtoSaida = new ListagemCompensacaoFaltaSaidaDTO();
                dtoSaida.Status = 1;
                dtoSaida.StatusDescription = e.Message;
            }

            return dtoSaida;
        }

        /// <summary>
        /// Metodo que atualiza os dados do tablet do diario de classe no SGP.
        /// </summary>
        /// <param name="filtroEntrada"></param>
        public static bool AtualizaEquipamento(AssociaEscolaEntradaDTO filtroEntrada)
        {
            try
            {
                return AtualizaEquipamento(filtroEntrada.K1, filtroEntrada.Uad_codigo, filtroEntrada.K4, filtroEntrada.appVersion, filtroEntrada.soVersion, filtroEntrada.sisId);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo que associa o tablet com uma escola.
        /// </summary>
        /// <param name="associaEscolaEntradaDTO">Objeto com as informações da escola e do tablet.</param>
        /// <returns>Retorna um objeto com o status da operação.</returns>
        public static AssociaEscolaSaidaDTO AssociaEscola(AssociaEscolaEntradaDTO associaEscolaEntradaDTO)
        {
            AssociaEscolaSaidaDTO associaEscolaDTO;
            try
            {
                string[] uad_codigos;

                uad_codigos = associaEscolaEntradaDTO.Uad_codigo.Split(';');

                bool status = true;
                associaEscolaDTO = new AssociaEscolaSaidaDTO();
                foreach (string uad_codigo in uad_codigos)
                {
                    if (!String.IsNullOrEmpty(uad_codigo))
                    {
                        status = AssociaEscola(associaEscolaEntradaDTO.K1, uad_codigo, associaEscolaEntradaDTO.K4, associaEscolaEntradaDTO.appVersion, associaEscolaEntradaDTO.soVersion, associaEscolaEntradaDTO.sisId);
                        if (!status)
                            break;
                    }
                }
                associaEscolaDTO.Status = status ? 0 : 1;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, associaEscolaEntradaDTO.GetProperties());

                associaEscolaDTO = new AssociaEscolaSaidaDTO();
                associaEscolaDTO.Status = 1;
                associaEscolaDTO.StatusDescription = exp.Message;
            }
            return associaEscolaDTO;
        }

        /// <summary>
        /// retorna registros de alunos da turma, caso
        /// tenha a data da ultima sincronização vai retornar apenas os alterados/criados
        /// apos esta data.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <returns></returns>
        public static List<AlunoTurma> SelecionarAlunosPorTurma(int tur_id, DateTime dataBase)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.BuscaAlunosTurma(0, tur_id, dataBase);

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                List<AlunoTurma> alunos = (
                    from r in dt.AsEnumerable()
                    select (AlunoTurma)GestaoEscolarUtilBO.DataRowToEntity(r, new AlunoTurma())
                    ).ToList();

                return alunos;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// seleciona uma lista de alunos com os dados de pessoa e usuario por escola
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <returns></returns>
        public static List<AlunoPessoaUsuario> SelecionarAlunosPorEscola(int esc_id, DateTime dataSincronizacao)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.SelecionarAlunosPorEscola(esc_id, dataSincronizacao);

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                List<AlunoPessoaUsuario> alunos = (
                    from DataRow row in dt.Rows
                    group row by row["alu_id"]
                        into r
                    select (AlunoPessoaUsuario)GestaoEscolarUtilBO.DataRowToEntity(r.First(),
                    new AlunoPessoaUsuario
                    {
                        Pessoa = ((PES_PessoaDTO.PessoaDadosBasicos)GestaoEscolarUtilBO.DataRowToEntity(r.First(), new PES_PessoaDTO.PessoaDadosBasicos())),
                        Usuario = ((SYS_UsuarioDTO.UsuarioDadosBasicos)GestaoEscolarUtilBO.DataRowToEntity(r.First(), new SYS_UsuarioDTO.UsuarioDadosBasicos()))
                    })
                    ).ToList();

                return alunos;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Busca todas as informações, que os tablets precisam, sobre os alunos na turma especificada.
        /// </summary>
        /// <param name="buscaAlunosTurmaEntradaDTO">Objeto com as informações de uma turma.</param>
        /// <returns>Retorna um objeto com o status da requisição e uma lista de alunos.</returns>
        public static BuscaAlunosTurmaSaidaDTO BuscaAlunosTurma(BuscaAlunosTurmaEntradaDTO buscaAlunosTurmaEntradaDTO)
        {
            BuscaAlunosTurmaSaidaDTO buscaAlunosTurmaSaidaDTO;
            try
            {
                DateTime syncDate = string.IsNullOrEmpty(buscaAlunosTurmaEntradaDTO.SyncDate) ? new DateTime() : Convert.ToDateTime(buscaAlunosTurmaEntradaDTO.SyncDate);
                DataTable saida = BuscaAlunosTurma
                    (
                        buscaAlunosTurmaEntradaDTO.Esc_id,
                        buscaAlunosTurmaEntradaDTO.Tur_id,
                        syncDate
                    );

                buscaAlunosTurmaSaidaDTO = new BuscaAlunosTurmaSaidaDTO();
                if (saida.Rows.Count == 0)
                {
                    return buscaAlunosTurmaSaidaDTO;
                }

                ACA_AlunoDAO alunoDao = new ACA_AlunoDAO();
                buscaAlunosTurmaSaidaDTO.TurmaAlunos =
                    (
                        from DataRow row in saida.Rows
                        group row by row["tur_id"]
                            into r
                        select new TurmaAlunos
                        {
                            Tur_id = Convert.ToInt64(r.First()["tur_id"]),
                            Alunos = (
                                    from DataRow dr in r
                                    select new Aluno
                                    {
                                        Alu_id = Convert.ToInt64(dr["alu_id"]),
                                        Alu_situacao = Convert.ToByte(dr["alu_situacao"]),
                                        Pes_nome = dr["pes_nome"].ToString(),
                                        Mtu_numeroChamada = (int)dr["mtu_numeroChamada"],
                                        Pes_dataNascimento = Convert.ToDateTime(dr["pes_dataNascimento"]).ToString("dd/MM/yyyy"),
                                        Pes_sexo = Convert.ToByte(dr["pes_sexo"]),
                                        Mtu_dataMatricula = Convert.ToDateTime(dr["mtu_dataMatricula"]).ToString("dd/MM/yyyy"),
                                        Mtu_dataSaida = ((dr["mtu_dataSaida"] == DBNull.Value) ? null : Convert.ToDateTime(dr["mtu_dataSaida"]).ToString()),
                                        temFoto = (int)dr["temFoto"],
                                        arq_dataAlteracao = ((dr["arq_dataAlteracao"] == DBNull.Value) ? null
                                                : Convert.ToDateTime(dr["arq_dataAlteracao"]).ToString("dd/MM/yyyy HH:mm:ss.fff")),

                                        Deficiencias = string.IsNullOrEmpty(dr["tde_id"].ToString()) ?
                                                new List<Deficiencia>() :
                                                new List<Deficiencia> { new Deficiencia { tde_id = new Guid(dr["tde_id"].ToString()) } }
                                    }
                                ).ToList()
                        }
                    ).ToList();
            }
            catch (Exception exp)
            {
                buscaAlunosTurmaSaidaDTO = new BuscaAlunosTurmaSaidaDTO();
                buscaAlunosTurmaSaidaDTO.Status = 1;
                buscaAlunosTurmaSaidaDTO.StatusDescription = exp.Message;
            }

            return buscaAlunosTurmaSaidaDTO;
        }

        /// <summary>
        /// Método que busca os alunos de uma determinada turma
        /// </summary>
        /// <param name="tur_id">Id da turma (Opcional)</param>
        /// <param name="syncDate">Menor data de sincronização dos registros dos Alunos na Turma especificada (Opcional)</param>
        /// <returns>DataTable que contém os alunos da turma pesquisada</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunosTurma(Int32 esc_id, Int64 tur_id, DateTime syncDate)
        {
            ApiDAO dao = new ApiDAO();
            return dao.BuscaAlunosTurma(esc_id, tur_id, syncDate);
        }

        /// <summary>
        /// retorna os dados de aula por turma e data base, retornando registros ativos e excluidos.
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public static List<CLS_TurmaAulaDTO> BuscarAulasPorTurmaDataBase(Int64 tur_id, DateTime dataBase)
        {
            return processarDataSetApi(CLS_TurmaAulaBO.BuscarAulasPorTurmaDataBase(tur_id, dataBase));
        }

        /// <summary>
        /// retorna os dados de aula por escola e data base, retornando registros ativos e excluidos.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public static object BuscarAulasPorEscolaDataBase(Int32 esc_id, DateTime dataBase)
        {
            return processarDataSetApiEnxuto(CLS_TurmaAulaBO.BuscarAulasPorEscolaDataBase(esc_id, dataBase), dataBase);
        }

        /// <summary>
        /// dados da aula por disciplina e um determinado periodo
        /// </summary>
        /// <param name="tud_id">id da turma disciplina</param>
        /// <param name="dataInicio">data de inicio do periodo</param>
        /// <param name="dataFim">data fim do periodo</param>
        /// <param name="usu_id">ID do usuário que criou a aula</param>
        /// <returns></returns>
        public static List<CLS_TurmaAulaDTO> BuscarAulasPorPeriodo(Int64 tud_id, byte tdt_posicao, DateTime dataInicio, DateTime dataFim, Guid usu_id)
        {
            return processarDataSetApi(CLS_TurmaAulaBO.BuscarAulasPorTurmaDisciplinaPeriodo(tud_id, tdt_posicao, dataInicio, dataFim, usu_id));
        }

        /// <summary>
        /// dados da aula por disciplina e um determinado periodo
        /// </summary>
        /// <param name="tud_id">id da turma disciplina</param>
        /// <param name="dataInicio">data de inicio do periodo</param>
        /// <param name="dataFim">data fim do periodo</param>
        /// <returns></returns>
        public static List<CLS_TurmaAulaDTO> BuscarAula(Int64 tud_id, Int32 tau_id)
        {
            return processarDataSetApi(CLS_TurmaAulaBO.BuscarAula(tud_id, tau_id));
        }

        /// <summary>
        /// processa o dataSet gerado para a api de turmas_aulas
        /// </summary>
        /// <param name="dataSetAulas"></param>
        /// <returns></returns>
        private static List<CLS_TurmaAulaDTO> processarDataSetApi(DataSet dataSetAulas)
        {
            try
            {
                DataTable dtAulas = new DataTable();
                DataTable dtAulaAlunos = new DataTable();
                DataTable dtAtividades = new DataTable();
                DataTable dtAtividadeAlunos = new DataTable();
                DataTable dtRecursos = new DataTable();
                DataTable dtHabilidadesPlanoAula = new DataTable();

                if (dataSetAulas.Tables.Count == 6)
                {
                    dtAulas = dataSetAulas.Tables[0];
                    dtAulaAlunos = dataSetAulas.Tables[1];
                    dtAtividades = dataSetAulas.Tables[2];
                    dtAtividadeAlunos = dataSetAulas.Tables[3];
                    dtRecursos = dataSetAulas.Tables[4];
                    dtHabilidadesPlanoAula = dataSetAulas.Tables[5];
                }
                else
                {
                    return null;
                }

                List<CLS_TurmaAulaDTO> aulas = new List<CLS_TurmaAulaDTO>();

                bool permiteAnotacoes = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ANOTACOES_MAIS_DE_UM_ALUNO, new Guid());

                foreach (DataRow dr in dtAulas.Rows)
                {
                    CLS_TurmaAulaDTO aula = (CLS_TurmaAulaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new CLS_TurmaAulaDTO());

                    aula.alunos = (from DataRow item in dtAulaAlunos.Rows
                                   where Convert.ToInt64(item["tud_id"]) == aula.tud_id
                                   && Convert.ToInt32(item["tau_id"]) == aula.tau_id
                                   group item by new
                                   {
                                       tud_id = Convert.ToInt64(item["tud_id"]),
                                       tau_id = Convert.ToInt32(item["tau_id"]),
                                       alu_id = Convert.ToInt64(item["alu_id"]),
                                       mtu_id = Convert.ToInt32(item["mtu_id"]),
                                       mtd_id = Convert.ToInt32(item["mtd_id"]),
                                   } into p
                                   select (CLS_TurmaAulaAlunoDTO)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new CLS_TurmaAulaAlunoDTO
                                   {
                                       listaTurmaAulaAlunoTipoAnotacao = permiteAnotacoes ? (from DataRow r in p
                                                                                             where (!string.IsNullOrEmpty(r["tia_id"].ToString()))
                                                                                             select (CLS_TurmaAulaAlunoTipoAnotacao)GestaoEscolarUtilBO.DataRowToEntity(r,
                                                                                             new CLS_TurmaAulaAlunoTipoAnotacao())
                                                                          ).ToList() : null
                                   })).ToList();
                    aula.atividades = (from DataRow item in dtAtividades.Rows
                                       where Convert.ToInt64(item["tud_id"]) == aula.tud_id
                                       && Convert.ToInt32(item["tau_id"]) == aula.tau_id
                                       select (CLS_TurmaNotaDTO)GestaoEscolarUtilBO.DataRowToEntity(item, new CLS_TurmaNotaDTO())
                                           ).ToList();

                    foreach (CLS_TurmaNotaDTO atividade in aula.atividades)
                    {
                        atividade.alunos = (from DataRow item in dtAtividadeAlunos.Rows
                                            where Convert.ToInt64(item["tud_id"]) == atividade.tud_id
                                            && Convert.ToInt32(item["tnt_id"]) == atividade.tnt_id
                                            select (CLS_TurmaNotaAlunoDTO)GestaoEscolarUtilBO.DataRowToEntity(item, new CLS_TurmaNotaAlunoDTO())
                                                ).ToList();
                    }

                    aula.recursos = (from DataRow item in dtRecursos.Rows
                                     where Convert.ToInt64(item["tud_id"]) == aula.tud_id
                                     && Convert.ToInt32(item["tau_id"]) == aula.tau_id
                                     select (CLS_TurmaAulaRecursoDTO)GestaoEscolarUtilBO.DataRowToEntity(item, new CLS_TurmaAulaRecursoDTO())
                                         ).ToList();

                    aula.habilidadesPlanoAula = (from DataRow item in dtHabilidadesPlanoAula.Rows
                                                 where Convert.ToInt64(item["tud_id"]) == aula.tud_id
                                                 && Convert.ToInt32(item["tau_id"]) == aula.tau_id
                                                 select (CLS_TurmaAulaOrientacaoCurricularDTO)GestaoEscolarUtilBO.DataRowToEntity(item, new CLS_TurmaAulaOrientacaoCurricularDTO())
                                         ).ToList();

                    aulas.Add(aula);
                }

                return aulas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// processa o dataSet gerado para a api de turmas_aulas
        /// </summary>
        /// <param name="dataSetAulas"></param>
        /// <returns></returns>
        private static object processarDataSetApiEnxuto(DataSet dataSetAulas, DateTime dataBase)
        {
            try
            {
                DataTable dtAulas = new DataTable();
                DataTable dtAulaAlunos = new DataTable();
                DataTable dtAtividades = new DataTable();
                DataTable dtAtividadeAlunos = new DataTable();
                DataTable dtRecursos = new DataTable();
                DataTable dtHabilidadesPlanoAula = new DataTable();

                if (dataSetAulas.Tables.Count == 6)
                {
                    dtAulas = dataSetAulas.Tables[0];
                    dtAulaAlunos = dataSetAulas.Tables[1];
                    dtAtividades = dataSetAulas.Tables[2];
                    dtAtividadeAlunos = dataSetAulas.Tables[3];
                    dtRecursos = dataSetAulas.Tables[4];
                    dtHabilidadesPlanoAula = dataSetAulas.Tables[5];
                }
                else
                {
                    return null;
                }

                bool permiteAnotacoes = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ANOTACOES_MAIS_DE_UM_ALUNO, new Guid());

                List<object> retorno = new List<object>();

                foreach (DataRow dr in dtAulas.Rows)
                {
                    CLS_TurmaAulaDTO aula = (CLS_TurmaAulaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new CLS_TurmaAulaDTO());

                    //Adiciona na aula apenas quando alterou os campos (controlado pelas datas dos logs) ou quando criou a aula
                    Dictionary<string, object> objAula = new Dictionary<string, object>();
                    objAula.Add("tur_id", aula.tur_id);
                    objAula.Add("tud_id", aula.tud_id);
                    objAula.Add("tau_id", aula.tau_id);
                    objAula.Add("tpc_id", aula.tpc_id);
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoAula >= dataBase)
                    {
                        objAula.Add("tau_sequencia", aula.tau_sequencia);
                        objAula.Add("tau_data", aula.tau_data);
                        objAula.Add("tau_numeroAulas", aula.tau_numeroAulas);
                    }
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoPlanoAula >= dataBase)
                    {
                        objAula.Add("tau_planoAula", aula.tau_planoAula);
                        objAula.Add("tau_diarioClasse", aula.tau_diarioClasse);
                        objAula.Add("tau_conteudo", aula.tau_conteudo);
                    }
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoAula >= dataBase || aula.dataAlteracaoFrequencia >= dataBase)
                        objAula.Add("tau_efetivado", aula.tau_efetivado);
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoPlanoAula >= dataBase)
                        objAula.Add("tau_atividadeCasa", aula.tau_atividadeCasa);
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoAula >= dataBase || aula.dataExclusaoAula >= dataBase)
                        objAula.Add("tau_situacao", aula.tau_situacao);
                    if (aula.tau_dataCriacao >= dataBase)
                        objAula.Add("tau_dataCriacao", aula.tau_dataCriacao);
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoAula >= dataBase)
                        objAula.Add("tau_dataAlteracao", aula.tau_dataAlteracao);
                    objAula.Add("tdt_posicao", aula.tdt_posicao);
                    objAula.Add("pro_id", aula.pro_id);
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoPlanoAula >= dataBase)
                        objAula.Add("tau_sintese", aula.tau_sintese);
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoAula >= dataBase)
                    {
                        objAula.Add("tau_reposicao", aula.tau_reposicao);
                        objAula.Add("usu_id", aula.usu_id);
                    }
                    objAula.Add("doc_id", aula.doc_id);
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoAula >= dataBase)
                        objAula.Add("usu_idDocenteAlteracao", aula.usu_idDocenteAlteracao);
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoFrequencia >= dataBase)
                        objAula.Add("tau_statusFrequencia", aula.tau_statusFrequencia);
                    //objAula.Add("tau_statusAtividadeAvaliativa", aula.tau_statusAtividadeAvaliativa); 
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoAnotacao >= dataBase)
                        objAula.Add("tau_statusAnotacoes", aula.tau_statusAnotacoes);
                    if (aula.tau_dataCriacao >= dataBase || aula.dataAlteracaoPlanoAula >= dataBase)
                        objAula.Add("tau_statusPlanoAula", aula.tau_statusPlanoAula);
                    objAula.Add("tau_dataUltimaSincronizacao", aula.tau_dataUltimaSincronizacao);
                    objAula.Add("dataAlteracaoAula", aula.dataAlteracaoAula);
                    //objAula.Add("usu_idAlteracaoAula", aula.usu_idAlteracaoAula);
                    objAula.Add("dataAlteracaoPlanoAula", aula.dataAlteracaoPlanoAula);
                    //objAula.Add("usu_idAlteracaoPlanoAula", aula.usu_idAlteracaoPlanoAula);
                    objAula.Add("dataAlteracaoFrequencia", aula.dataAlteracaoFrequencia);
                    //objAula.Add("usu_idAlteracaoFrequencia", aula.usu_idAlteracaoFrequencia);
                    objAula.Add("dataAlteracaoAnotacao", aula.dataAlteracaoAnotacao);
                    //objAula.Add("usu_idAlteracaoAnotacao", aula.usu_idAlteracaoAnotacao);
                    objAula.Add("dataExclusaoAula", aula.dataExclusaoAula);
                    //objAula.Add("usu_idExclusaoAula", aula.usu_idExclusaoAula);
                    objAula.Add("maior_dataAlteracao", aula.maior_dataAlteracao);

                    var alunos = (from DataRow item in dtAulaAlunos.Rows
                                  where Convert.ToInt64(item["tud_id"]) == aula.tud_id
                                  && Convert.ToInt32(item["tau_id"]) == aula.tau_id
                                  group item by new
                                  {
                                      tud_id = Convert.ToInt64(item["tud_id"]),
                                      tau_id = Convert.ToInt32(item["tau_id"]),
                                      alu_id = Convert.ToInt64(item["alu_id"]),
                                      mtu_id = Convert.ToInt32(item["mtu_id"]),
                                      mtd_id = Convert.ToInt32(item["mtd_id"]),
                                  } into p
                                  select (CLS_TurmaAulaAlunoDTO)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new CLS_TurmaAulaAlunoDTO
                                  {
                                      listaTurmaAulaAlunoTipoAnotacao = permiteAnotacoes ? (from DataRow r in p
                                                                                            where (!string.IsNullOrEmpty(r["tia_id"].ToString()))
                                                                                            select (CLS_TurmaAulaAlunoTipoAnotacao)GestaoEscolarUtilBO.DataRowToEntity(r,
                                                                                            new CLS_TurmaAulaAlunoTipoAnotacao())
                                                                          ).ToList() : null
                                  })).ToList();

                    //Adiciona na aula apenas os alunos que lançaram frequencia ou anotacao (controlado pelas datas dos logs) 
                    //  ou quando criou o registro do aluno na aula
                    List<Dictionary<string, object>> lstAlunos = new List<Dictionary<string, object>>();
                    foreach (CLS_TurmaAulaAlunoDTO aluno in alunos)
                    {
                        if (aula.dataAlteracaoAula >= dataBase || aula.dataExclusaoAula >= dataBase ||
                            aula.dataAlteracaoFrequencia >= dataBase || aula.dataAlteracaoAnotacao >= dataBase ||
                            aluno.taa_dataCriacao >= dataBase)
                        {
                            Dictionary<string, object> objAluno = new Dictionary<string, object>();
                            objAluno.Add("alu_id", aluno.alu_id);
                            objAluno.Add("mtu_id", aluno.mtu_id);
                            objAluno.Add("mtd_id", aluno.mtd_id);
                            if (aula.dataAlteracaoFrequencia >= dataBase || aluno.taa_dataCriacao >= dataBase)
                                objAluno.Add("taa_frequencia", aluno.taa_frequencia);
                            if (aula.dataAlteracaoAnotacao >= dataBase || aluno.taa_dataCriacao >= dataBase)
                                objAluno.Add("taa_anotacao", aluno.taa_anotacao);
                            if (aula.dataAlteracaoAula >= dataBase || aula.dataExclusaoAula >= dataBase || aluno.taa_dataCriacao >= dataBase)
                                objAluno.Add("taa_situacao", aluno.taa_situacao);
                            if (aluno.taa_dataCriacao >= dataBase)
                                objAluno.Add("taa_dataCriacao", aluno.taa_dataCriacao);
                            if (aula.dataAlteracaoFrequencia >= dataBase || aula.dataAlteracaoAnotacao >= dataBase || aluno.taa_dataCriacao >= dataBase)
                                objAluno.Add("taa_dataAlteracao", aluno.taa_dataAlteracao);
                            if (aula.dataAlteracaoFrequencia >= dataBase || aluno.taa_dataCriacao >= dataBase)
                                objAluno.Add("taa_frequenciaBitMap", aluno.taa_frequenciaBitMap);
                            if (aula.dataAlteracaoFrequencia >= dataBase || aula.dataAlteracaoAnotacao >= dataBase || aluno.taa_dataCriacao >= dataBase)
                                objAluno.Add("usu_idDocenteAlteracao", aluno.usu_idDocenteAlteracao);
                            if ((aula.dataAlteracaoAnotacao >= dataBase || aluno.taa_dataCriacao >= dataBase) && permiteAnotacoes && aluno.listaTurmaAulaAlunoTipoAnotacao.Any())
                                objAluno.Add("listaTurmaAulaAlunoTipoAnotacao", aluno.listaTurmaAulaAlunoTipoAnotacao);
                            lstAlunos.Add(objAluno);
                        }
                    }

                    if (lstAlunos.Any())
                        objAula.Add("alunos", lstAlunos);

                    var atividades = (from DataRow item in dtAtividades.Rows
                                      where Convert.ToInt64(item["tud_id"]) == aula.tud_id
                                      && Convert.ToInt32(item["tau_id"]) == aula.tau_id
                                      select (CLS_TurmaNotaDTO)GestaoEscolarUtilBO.DataRowToEntity(item, new CLS_TurmaNotaDTO())
                                           ).ToList();

                    var lstAtividades = new List<object>();

                    foreach (CLS_TurmaNotaDTO atividade in atividades)
                    {
                        var alunosAt = (from DataRow item in dtAtividadeAlunos.Rows
                                        where Convert.ToInt64(item["tud_id"]) == atividade.tud_id
                                        && Convert.ToInt32(item["tnt_id"]) == atividade.tnt_id
                                        select (CLS_TurmaNotaAlunoDTO)GestaoEscolarUtilBO.DataRowToEntity(item, new CLS_TurmaNotaAlunoDTO())
                                        ).ToList();

                        //Adiciona na atividade apenas os alunos que lançaram nota (controlado pelas datas dos logs) 
                        //  ou quando criou o registro do aluno na atividade
                        List<Dictionary<string, object>> lstAlunosAt = new List<Dictionary<string, object>>();
                        foreach (CLS_TurmaNotaAlunoDTO aluno in alunosAt)
                        {
                            Dictionary<string, object> objAlunoAt = new Dictionary<string, object>();
                            if (atividade.dataLancamentoNota >= dataBase || aluno.tna_dataCriacao >= dataBase)
                            {
                                objAlunoAt.Add("alu_id", aluno.alu_id);
                                objAlunoAt.Add("mtu_id", aluno.mtu_id);
                                objAlunoAt.Add("mtd_id", aluno.mtd_id);
                                objAlunoAt.Add("tna_avaliacao", aluno.tna_avaliacao);
                                objAlunoAt.Add("tna_naoCompareceu", aluno.tna_naoCompareceu);
                                objAlunoAt.Add("tna_comentarios", aluno.tna_comentarios);
                                objAlunoAt.Add("tna_relatorio", aluno.tna_relatorio);
                                objAlunoAt.Add("tna_situacao", aluno.tna_situacao);
                                if (aluno.tna_dataCriacao >= dataBase)
                                    objAlunoAt.Add("tna_dataCriacao", aluno.tna_dataCriacao);
                                objAlunoAt.Add("tna_dataAlteracao", aluno.tna_dataAlteracao);
                                objAlunoAt.Add("tna_participante", aluno.tna_participante);
                                lstAlunosAt.Add(objAlunoAt);
                            }
                        }

                        //Adiciona na aula apenas as atividades que lançaram nota ou foram alteradas (controlado pelas datas dos logs) 
                        //  ou quando criou a atividade
                        Dictionary<string, object> objAtividade = new Dictionary<string, object>();
                        if (atividade.dataAlteracaoAtividade >= dataBase || atividade.dataExclusaoAtividade >= dataBase ||
                            atividade.tnt_dataCriacao >= dataBase || atividade.dataLancamentoNota >= dataBase ||
                            alunosAt.Any(b => b.tna_dataCriacao >= dataBase))
                        {
                            objAtividade.Add("tnt_id", atividade.tnt_id);
                            objAtividade.Add("tpc_id", atividade.tpc_id);
                            if (atividade.dataAlteracaoAtividade >= dataBase || atividade.tnt_dataCriacao >= dataBase)
                            {
                                objAtividade.Add("tnt_nome", atividade.tnt_nome);
                                objAtividade.Add("tnt_data", atividade.tnt_data);
                                objAtividade.Add("tnt_descricao", atividade.tnt_descricao);
                            }
                            if (atividade.dataAlteracaoAtividade >= dataBase || atividade.dataExclusaoAtividade >= dataBase || atividade.tnt_dataCriacao >= dataBase)
                                objAtividade.Add("tnt_situacao", atividade.tnt_situacao);
                            if (atividade.tnt_dataCriacao >= dataBase)
                                objAtividade.Add("tnt_dataCriacao", atividade.tnt_dataCriacao);
                            if (atividade.dataAlteracaoAtividade >= dataBase || atividade.tnt_dataCriacao >= dataBase)
                                objAtividade.Add("tnt_dataAlteracao", atividade.tnt_dataAlteracao);
                            if (atividade.dataAlteracaoAtividade >= dataBase || atividade.dataLancamentoNota >= dataBase || atividade.tnt_dataCriacao >= dataBase)
                                objAtividade.Add("tnt_efetivado", atividade.tnt_efetivado);
                            if (atividade.dataAlteracaoAtividade >= dataBase || atividade.tnt_dataCriacao >= dataBase)
                            {
                                objAtividade.Add("tav_id", atividade.tav_id);
                                objAtividade.Add("tdt_posicao", atividade.tdt_posicao);
                                objAtividade.Add("tnt_exclusiva", atividade.tnt_exclusiva);
                                objAtividade.Add("usu_id", atividade.usu_id);
                            }
                            objAtividade.Add("pro_id", atividade.pro_id);
                            if (atividade.dataAlteracaoAtividade >= dataBase || atividade.tnt_dataCriacao >= dataBase)
                                objAtividade.Add("tnt_chaveDiario", atividade.tnt_chaveDiario);
                            if (atividade.dataAlteracaoAtividade >= dataBase || atividade.dataLancamentoNota >= dataBase ||
                                atividade.dataExclusaoAtividade >= dataBase || atividade.tnt_dataCriacao >= dataBase)
                                objAtividade.Add("usu_idDocenteAlteracao", atividade.usu_idDocenteAlteracao);
                            if (atividade.dataAlteracaoAtividade >= dataBase || atividade.tnt_dataCriacao >= dataBase)
                                objAtividade.Add("dataAlteracaoAtividade", atividade.dataAlteracaoAtividade);
                            //objAtividade.Add("usu_idAlteracaoAtividade", atividade.usu_idAlteracaoAtividade);
                            objAtividade.Add("dataLancamentoNota", atividade.dataLancamentoNota);
                            //objAtividade.Add("usu_idLancamentoNota", atividade.usu_idLancamentoNota);
                            objAtividade.Add("dataExclusaoAtividade", atividade.dataExclusaoAtividade);
                            objAtividade.Add("maior_dataAlteracao", atividade.maior_dataAlteracao);
                            if ((atividade.dataLancamentoNota >= dataBase || alunosAt.Any(b => b.tna_dataCriacao >= dataBase)) &&
                                lstAlunosAt.Any())
                                objAtividade.Add("alunos", lstAlunosAt);
                            lstAtividades.Add(objAtividade);
                        }

                    }

                    if (lstAtividades.Any())
                        objAula.Add("atividades", lstAtividades);

                    var recursos = (from DataRow item in dtRecursos.Rows
                                    where Convert.ToInt64(item["tud_id"]) == aula.tud_id
                                    && Convert.ToInt32(item["tau_id"]) == aula.tau_id
                                    select (CLS_TurmaAulaRecursoDTO)GestaoEscolarUtilBO.DataRowToEntity(item, new CLS_TurmaAulaRecursoDTO())
                                         ).ToList();

                    List<Dictionary<string, object>> lstRecursos = new List<Dictionary<string, object>>();
                    foreach (CLS_TurmaAulaRecursoDTO recurso in recursos)
                    {
                        if (aula.dataAlteracaoPlanoAula >= @dataBase || aula.tau_dataCriacao >= dataBase || recurso.tar_dataCriacao >= dataBase)
                        {
                            Dictionary<string, object> recursoObj = new Dictionary<string, object>();
                            recursoObj.Add("tar_id", recurso.tar_id);
                            recursoObj.Add("rsa_id", recurso.rsa_id);
                            recursoObj.Add("tar_observacao", recurso.tar_observacao);
                            if (recurso.tar_dataCriacao >= dataBase)
                                recursoObj.Add("tar_dataCriacao", recurso.tar_dataCriacao);
                            recursoObj.Add("tar_dataAlteracao", recurso.tar_dataAlteracao);
                            lstRecursos.Add(recursoObj);
                        }
                    }

                    if (lstRecursos.Any())
                        objAula.Add("recursos", lstRecursos);

                    var habilidadesPlanoAula = (from DataRow item in dtHabilidadesPlanoAula.Rows
                                                where Convert.ToInt64(item["tud_id"]) == aula.tud_id
                                                && Convert.ToInt32(item["tau_id"]) == aula.tau_id
                                                select (CLS_TurmaAulaOrientacaoCurricularDTO)GestaoEscolarUtilBO.DataRowToEntity(item, new CLS_TurmaAulaOrientacaoCurricularDTO())
                                                ).ToList();

                    List<Dictionary<string, object>> lstHabilidadesPlanoAula = new List<Dictionary<string, object>>();
                    foreach (CLS_TurmaAulaOrientacaoCurricularDTO habilidade in habilidadesPlanoAula)
                    {
                        if (aula.dataAlteracaoPlanoAula >= @dataBase || aula.tau_dataCriacao >= dataBase)
                        {
                            Dictionary<string, object> habilidadePlanoAulaObj = new Dictionary<string, object>();
                            habilidadePlanoAulaObj.Add("ocr_id", habilidade.ocr_id);
                            habilidadePlanoAulaObj.Add("tao_pranejado", habilidade.tao_pranejado);
                            habilidadePlanoAulaObj.Add("tao_trabalhado", habilidade.tao_trabalhado);
                            habilidadePlanoAulaObj.Add("tao_alcancado", habilidade.tao_alcancado);
                            lstHabilidadesPlanoAula.Add(habilidadePlanoAulaObj);
                        }
                    }

                    if (lstHabilidadesPlanoAula.Any())
                        objAula.Add("habilidadesPlanoAula", lstHabilidadesPlanoAula);

                    retorno.Add(objAula);
                }

                return retorno;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Busca as últimas aulas conforme a turma e a disciplina passadas.
        /// </summary>
        /// <param name="buscaAulaEntradaDTO"></param>
        /// <returns></returns>
        public static BuscaAulaSaidaDTO BuscaAulas(BuscaAulaEntradaDTO buscaAulaEntradaDTO)
        {
            BuscaAulaSaidaDTO buscaAulaSaidaDTO;

            try
            {
                buscaAulaSaidaDTO = new BuscaAulaSaidaDTO();

                if (buscaAulaEntradaDTO.Tud_id <= 0 && buscaAulaEntradaDTO.Tur_id <= 0)
                {
                    // Tem que passar o tud ou tur_id por parâmetro para a busca.
                    buscaAulaSaidaDTO.Status = 1;
                    buscaAulaSaidaDTO.StatusDescription = "É obrigatório enviar tud_id ou tur_id como paramêtro!";

                    return buscaAulaSaidaDTO;
                }

                // Retorna um DataSet contendo as aulas e seus dados.
                DataSet dadosAulas = CLS_TurmaAulaBO.BuscaUltimasAulasPorTurmaDisciplina(
                   buscaAulaEntradaDTO.Tud_id,
                        buscaAulaEntradaDTO.Tur_id,
                        buscaAulaEntradaDTO.paraTras,
                        buscaAulaEntradaDTO.paraFrente,
                   buscaAulaEntradaDTO.primeiraSincronizacao
                   );

                DataTable dtAulas = new DataTable();
                DataTable dtAtividades = new DataTable();
                DataTable dtAtividadeAlunos = new DataTable();
                DataTable dtRecursos = new DataTable();
                DataTable dtAulaAlunos = new DataTable();
                DataTable dtRegencia = new DataTable();
                DataTable dtRegenciaRecursos = new DataTable();
                DataTable dtPlanoAulaDisciplina = new DataTable();

                if (dadosAulas.Tables.Count > 7)
                {
                    dtAulas = dadosAulas.Tables[0];
                    dtAtividades = dadosAulas.Tables[1];
                    dtAtividadeAlunos = dadosAulas.Tables[2];
                    dtRecursos = dadosAulas.Tables[3];
                    dtAulaAlunos = dadosAulas.Tables[4];
                    dtRegencia = dadosAulas.Tables[5];
                    dtRegenciaRecursos = dadosAulas.Tables[6];
                    dtPlanoAulaDisciplina = dadosAulas.Tables[7];
                }

                List<Aula> aulas = new List<Aula>();

                foreach (DataRow dr in dtAulas.Rows)
                {
                    Aula entAula = (Aula)GestaoEscolarUtilBO.DataRowToEntity(dr, new Aula());

                    entAula.Atividades =
                        (from DataRow item in dtAtividades.Rows
                         where Convert.ToInt64(item["tud_idAula"]) == entAula.Tud_id
                         && Convert.ToInt32(item["tau_idAula"]) == entAula.Tau_id
                         select (Atividade)GestaoEscolarUtilBO.DataRowToEntity(item, new Atividade())
                          ).ToList();

                    foreach (Atividade entAtividade in entAula.Atividades)
                    {
                        // Alimenta as notas dos alunos em cada atividade.
                        entAtividade.AtividadeAlunos =
                            (from DataRow item in dtAtividadeAlunos.Rows
                             where Convert.ToInt64(item["tud_id"]) == entAtividade.Tud_id
                                && Convert.ToInt32(item["tnt_id"]) == entAtividade.Tnt_id
                             select (AtividadeAluno)GestaoEscolarUtilBO.DataRowToEntity(item, new AtividadeAluno())
                          ).ToList();
                    }

                    entAula.Recursos =
                        (from DataRow item in dtRecursos.Rows
                         where Convert.ToInt64(item["tud_idAula"]) == entAula.Tud_id
                         && Convert.ToInt32(item["tau_idAula"]) == entAula.Tau_id
                         select (TurmaAulaRecurso)GestaoEscolarUtilBO.DataRowToEntity(item, new TurmaAulaRecurso())
                          ).ToList();

                    entAula.Alunos =
                        (from DataRow item in dtAulaAlunos.Rows
                         where Convert.ToInt64(item["tud_idAula"]) == entAula.Tud_id
                         && Convert.ToInt32(item["tau_idAula"]) == entAula.Tau_id
                         select (Aluno)GestaoEscolarUtilBO.DataRowToEntity(item, new Aluno())
                          ).ToList();

                    if (ParametroOrientacoesCurricularesAula)
                    {
                        entAula.Regencias =
                            (from DataRow item in dtRegencia.Rows
                             where Convert.ToInt64(item["tud_idAula"]) == entAula.Tud_id
                             && Convert.ToInt32(item["tau_idAula"]) == entAula.Tau_id
                             select (Regencia)GestaoEscolarUtilBO.DataRowToEntity(item, new Regencia())
                              ).ToList();

                        foreach (Regencia entRegencia in entAula.Regencias)
                        {
                            // Alimenta os recursos de cada entidade da regência.
                            entRegencia.Recursos =
                                (from DataRow item in dtRegenciaRecursos.Rows
                                 where Convert.ToInt64(item["tud_idAula"]) == entAula.Tud_id
                                    && Convert.ToInt32(item["tau_idAula"]) == entAula.Tau_id
                                    && Convert.ToInt64(item["tud_idFilho"]) == entRegencia.Tud_idFilho
                                 select (TurmaAulaRecurso)GestaoEscolarUtilBO.DataRowToEntity(item, new TurmaAulaRecurso())
                              ).ToList();
                        }
                    }

                    entAula.PlanoAulaRegenciaDisciplinas =
                       (from DataRow item in dtPlanoAulaDisciplina.Rows
                        where Convert.ToInt64(item["tud_id"]) == entAula.Tud_id
                        && Convert.ToInt32(item["tau_id"]) == entAula.Tau_id
                        select (CLS_TurmaAulaPlanoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(item, new CLS_TurmaAulaPlanoDisciplina())
                         ).ToList();

                    aulas.Add(entAula);
                }

                buscaAulaSaidaDTO.Aulas = aulas;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaEscolaEntradaDTO.GetProperties());

                buscaAulaSaidaDTO = new BuscaAulaSaidaDTO();
                buscaAulaSaidaDTO.Status = 1;
                buscaAulaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaAulaSaidaDTO;
        }

        /// <summary>
        /// Recebe todas as informações necessárias referentes a uma aula.
        /// </summary>
        /// <param name="sincronizacaoDiarioClasseEntradaDTO">Objeto com as informações da aula.</param>
        /// <returns>Retorna um objeto com o status da requisição e um protocolo de atendimento com o status do processamento das informações enviadas.</returns>
        public static PlanejamentoAnualSaidaDTO BuscaPlanejamentoAnual(PlanejamentoAnualEntradaDTO entradaDTO)
        {
            PlanejamentoAnualSaidaDTO saidaDTO;
            try
            {
                saidaDTO = new PlanejamentoAnualSaidaDTO();

                // Só chama os métodos do banco caso tenha passado a turma ou curso (e os outros), ou na configuração esteja setado
                // para configurar geral por escola (sincronização inicial).
                if (entradaDTO.tur_id > 0 || entradaDTO.cur_id > 0 || entradaDTO.sincronizarPorEscola)
                {
                    DateTime syncDate = string.IsNullOrEmpty(entradaDTO.SyncDate) ? new DateTime() : Convert.ToDateTime(entradaDTO.SyncDate);

                    saidaDTO.NiveisAprendizado = (
                            from dr in ORC_NivelAprendizadoBO.BuscarNiveisPorDataSincronizacao(syncDate,
                                            entradaDTO.tur_id,
                                            entradaDTO.cur_id,
                                            entradaDTO.crr_id,
                                            entradaDTO.crp_id).AsEnumerable()
                            select (ORCNivelAprendizado)GestaoEscolarUtilBO.DataRowToEntity(dr, new ORCNivelAprendizado())
                        ).ToList();

                    saidaDTO.Niveis = (
                            from dr in ORC_NivelBO.BuscaNiveisPorDataSincronizacao(syncDate,
                                            entradaDTO.tur_id,
                                            entradaDTO.cur_id,
                                            entradaDTO.crr_id,
                                            entradaDTO.crp_id,
                                            entradaDTO.cal_id,
                                            entradaDTO.tds_id).AsEnumerable()
                            select (ORCNivel)GestaoEscolarUtilBO.DataRowToEntity(dr, new ORCNivel())
                        ).ToList();

                    saidaDTO.OrientacoesCurriculares = (
                            from dr in ORC_OrientacaoCurricularBO.SelecionaPorDataSincronizacao(syncDate,
                                            entradaDTO.tur_id,
                                            entradaDTO.cur_id,
                                            entradaDTO.crr_id,
                                            entradaDTO.crp_id,
                                            entradaDTO.cal_id,
                                            entradaDTO.tds_id).AsEnumerable()
                            select (ORCOrientacaoCurricular)GestaoEscolarUtilBO.DataRowToEntity(dr, new ORCOrientacaoCurricular())
                        ).ToList();

                    saidaDTO.NiveisAprendizadoOrientacao = (
                            from dr in ORC_OrientacaoCurricularNivelAprendizadoBO.BuscarPorDataSincronizacao(syncDate,
                                            entradaDTO.tur_id,
                                            entradaDTO.cur_id,
                                            entradaDTO.crr_id,
                                            entradaDTO.crp_id,
                                            entradaDTO.cal_id,
                                            entradaDTO.tds_id).AsEnumerable()
                            select (ORCOrientacaoCurricularNivelAprendizado)GestaoEscolarUtilBO.DataRowToEntity(dr, new ORCOrientacaoCurricularNivelAprendizado())
                        ).ToList();
                }
            }
            catch (Exception exp)
            {
                saidaDTO = new PlanejamentoAnualSaidaDTO();
                saidaDTO.Status = 1;
                saidaDTO.StatusDescription = exp.Message;
            }

            return saidaDTO;
        }

        /// <summary>
        /// Retorna os períodos do calendário de acordo com os filtros.
        /// </summary>
        /// <param name="entrada">Objeto com os filtros de entrada.</param>
        /// <returns>Retorna um objeto com o status da requisição e um protocolo de atendimento com o status do processamento das informações enviadas.</returns>
        public static CalendarioAnualSaidaDTO BuscaCalendarioAnual(CalendarioAnualEntradaDTO entrada)
        {
            CalendarioAnualSaidaDTO saidaDTO;

            try
            {
                saidaDTO = new CalendarioAnualSaidaDTO();

                DateTime syncDate = string.IsNullOrEmpty(entrada.SyncDate) ? new DateTime() : Convert.ToDateTime(entrada.SyncDate);

                if (entrada.esc_id <= 0)
                {
                    throw new EscolaVaziaException();
                }

                // Calendario periodo.
                DataTable dtCalendario = ACA_CalendarioPeriodoBO.BuscaCalendariosEscola(entrada.esc_id, syncDate);
                if (dtCalendario.Rows.Count == 0)
                {
                    return saidaDTO;
                }

                List<CalendarioPeriodo> listaCalendarioPeriodo = (from DataRow drCalendario in dtCalendario.Rows
                                                                  select
                                                                      new CalendarioPeriodo
                                                                      {
                                                                          Cal_id = Convert.ToInt32(drCalendario["cal_id"]),
                                                                          Cap_id = Convert.ToInt32(drCalendario["cap_id"]),
                                                                          Cap_descricao = Convert.ToString(drCalendario["cap_descricao"]),
                                                                          Tpc_id = Convert.ToInt32(drCalendario["tpc_id"]),
                                                                          Cap_situacao = Convert.ToByte(drCalendario["cap_situacao"]),
                                                                          Cap_dataInicio = Convert.ToDateTime(drCalendario["cap_dataInicio"]),
                                                                          Cap_dataFim = Convert.ToDateTime(drCalendario["cap_dataFim"])
                                                                      }).ToList();

                saidaDTO.CalendarioPeriodo = listaCalendarioPeriodo;
            }
            catch (Exception exp)
            {
                saidaDTO = new CalendarioAnualSaidaDTO { Status = 1, StatusDescription = exp.Message };
            }

            return saidaDTO;
        }

        /// <summary>
        /// Busca informações como endereço da APK e horário de sincronizações.
        /// </summary>
        /// <param name="buscaDadosIniciaisEntradaDTO">Objeto com as informações necessárias para buscar os dados iniciais.</param>
        /// <returns>Retorna um objeto com o status da requisição e os dados iniciais.</returns>
        public static BuscaDadosIniciaisSaidaDTO BuscaDadosIniciais(BuscaDadosIniciaisEntradaDTO buscaDadosIniciaisEntradaDTO)
        {
            BuscaDadosIniciaisSaidaDTO buscaDadosIniciaisSaidaDTO;

            try
            {
                buscaDadosIniciaisSaidaDTO = new BuscaDadosIniciaisSaidaDTO();

                DataTable saida = DadosIniciais(buscaDadosIniciaisEntradaDTO.esc_id);
                if (saida.Rows.Count == 0)
                {
                    return buscaDadosIniciaisSaidaDTO;
                }

                ApiDAO dao = new ApiDAO();
                buscaDadosIniciaisSaidaDTO.AgendasRequisicao =
                    (
                        from DataRow row in saida.Rows
                        group row by row["agh_pacote"]
                            into r
                        select new AgendaRequisicao
                        {
                            Req_id = Convert.ToInt16(r.First()["agh_pacote"])
                            ,
                            Age_periodicidade = Convert.ToInt16(r.First()["age_periodicidade"])
                            ,
                            AgendaHorarios = (
                                 from DataRow row in r
                                 group row by row["agh_horario"]
                                     into o
                                 select new AgendaHorarios
                                 {
                                     AgendaHorario = o.First()["agh_horario"].ToString()
                                 }
                            ).ToList(),
                            Apis = (
                                from DataRow dr in r
                                group dr by dr["api_id"]
                                    into rd
                                select (Api)GestaoEscolarUtilBO.DataRowToEntity(rd.First(), new Api())
                            ).ToList()
                        }
                    ).ToList();
            }
            catch (Exception exp)
            {
                buscaDadosIniciaisSaidaDTO = new BuscaDadosIniciaisSaidaDTO();
                buscaDadosIniciaisSaidaDTO.Status = 1;
                buscaDadosIniciaisSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaDadosIniciaisSaidaDTO;
        }

        /// <summary>
        /// Busca as informações do usuário especifico.
        /// </summary>
        /// <param name="buscaUsuariosEntradaDTO">Objeto com uma lista com o login do usuário.</param>
        /// <returns>Retorna um objeto com o status da requisição os dados do usuário</returns>
        public static BuscaDadosUsuarioSaidaDTO BuscaDadosUsuario(BuscaDadosUsuarioEntradaDTO buscaDadosUsuarioEntradaDTO)
        {
            BuscaDadosUsuarioSaidaDTO buscaDadosUsuarioSaidaDTO;
            try
            {
                buscaDadosUsuarioSaidaDTO = new BuscaDadosUsuarioSaidaDTO();

                ApiDAO dao = new ApiDAO();

                DataTable dados = dao.BuscaDadosUsuario(buscaDadosUsuarioEntradaDTO.usu_login, buscaDadosUsuarioEntradaDTO.esc_id);

                if (dados.Rows.Count == 0)
                {
                    return buscaDadosUsuarioSaidaDTO;
                }

                buscaDadosUsuarioSaidaDTO.Usuario = (
                        from dr in dados.AsEnumerable()
                        select (SYS_UsuarioDTO.Usuario)GestaoEscolarUtilBO.DataRowToEntity(dr, new SYS_UsuarioDTO.Usuario())
                    ).First();

                return buscaDadosUsuarioSaidaDTO;
            }
            catch (Exception exp)
            {
                buscaDadosUsuarioSaidaDTO = new BuscaDadosUsuarioSaidaDTO();
                buscaDadosUsuarioSaidaDTO.Status = 1;
                buscaDadosUsuarioSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaDadosUsuarioSaidaDTO;
        }

        /// <summary>
        /// Busca as fotos dos alunos
        /// </summary>
        /// <param name="buscaFotoAlunoEntradaDTO"></param>
        /// <returns></returns>
        public static BuscaFotoAlunoSaidaDTO BuscaFotoAluno(BuscaFotoAlunoEntradaDTO buscaFotoAlunoEntradaDTO)
        {
            BuscaFotoAlunoSaidaDTO buscaFotoAlunoSaidaDTO;
            try
            {
                buscaFotoAlunoSaidaDTO = new BuscaFotoAlunoSaidaDTO();

                // no diario esta utilizando esta api para retornar o hora do servidor.
                // isto sera modificado.
                if (string.IsNullOrEmpty(buscaFotoAlunoEntradaDTO.Alu_id))
                {
                    return buscaFotoAlunoSaidaDTO;
                }

                DataTable arquivos = BuscaFotoAluno(buscaFotoAlunoEntradaDTO.Alu_id, buscaFotoAlunoEntradaDTO.Fot_dataSincronizacao);

                if (arquivos.Rows.Count == 0)
                {
                    return buscaFotoAlunoSaidaDTO;
                }

                List<Foto> listFotos = new List<Foto>();
                foreach (DataRow dr in arquivos.Rows)
                {
                    byte[] bufferData = (byte[])dr["arq_data"];
                    //entArquivo.arq_data;

                    MemoryStream stream = new MemoryStream(bufferData);
                    Image imgOriginal = Image.FromStream(stream);

                    double widthRatio = (double)imgOriginal.Width / (double)buscaFotoAlunoEntradaDTO.Fot_largura;
                    double heightRatio = (double)imgOriginal.Height / (double)buscaFotoAlunoEntradaDTO.Fot_altura;
                    double ratio = Math.Max(widthRatio, heightRatio);
                    int newWidth = (int)(imgOriginal.Width / ratio);
                    int newHeight = (int)(imgOriginal.Height / ratio);

                    Image imgRedimensionada = (Image)(new Bitmap(imgOriginal, new Size(newWidth, newHeight)));

                    MemoryStream stream2 = new MemoryStream();

                    imgRedimensionada.Save(stream2, System.Drawing.Imaging.ImageFormat.Jpeg);

                    stream2.Seek(0, SeekOrigin.Begin);

                    byte[] arquivo = stream2.ToArray();

                    string base64 = System.Convert.ToBase64String(arquivo,
                            0,
                            arquivo.Length);

                    Foto foto = new Foto();
                    foto.Fot_data = base64;
                    foto.Alu_id = Convert.ToInt64(dr["alu_id"]);
                    foto.Fot_dataAlteracao = Convert.ToDateTime(dr["arq_dataAlteracao"]).ToString("dd/MM/yyyy HH:mm:ss.fff");

                    listFotos.Add(foto);
                }

                buscaFotoAlunoSaidaDTO.Fotos = listFotos;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaEscolaEntradaDTO.GetProperties());

                buscaFotoAlunoSaidaDTO = new BuscaFotoAlunoSaidaDTO();
                buscaFotoAlunoSaidaDTO.Status = 1;
                buscaFotoAlunoSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaFotoAlunoSaidaDTO;
        }

        /// <summary>
        /// retorna um datatable com as fotos dos alunos.
        /// </summary>
        /// <param name="alu_ids">array de alunos por ;</param>
        /// <param name="syncDate">data da ultima sincronizacao</param>
        /// <returns></returns>
        public static DataTable BuscaFotoAluno(string alu_ids, DateTime syncDate)
        {
            ApiDAO dao = new ApiDAO();
            return dao.BuscaFotoAluno(alu_ids, syncDate);
        }

        /// <summary>
        /// Retorna justificativas faltas
        /// </summary>
        public static BuscaJustificativasFaltaSaidaDTO BuscaJustificativasFaltas()
        {
            BuscaJustificativasFaltaSaidaDTO buscaJustificativasFaltaSaidaDTO;
            DataTable justificativas;
            try
            {
                buscaJustificativasFaltaSaidaDTO = new BuscaJustificativasFaltaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                justificativas = dao.BuscaJustificativasFaltas();

                if (justificativas.Rows.Count == 0)
                {
                    buscaJustificativasFaltaSaidaDTO.Status = 1;
                    buscaJustificativasFaltaSaidaDTO.StatusDescription = "A Consulta não retornou resultado!";

                    return buscaJustificativasFaltaSaidaDTO;
                }

                buscaJustificativasFaltaSaidaDTO.JustificativasFaltas = (from dr in justificativas.AsEnumerable() select (JustificativaFalta)GestaoEscolarUtilBO.DataRowToEntity(dr, new JustificativaFalta())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaEscolaEntradaDTO.GetProperties());

                buscaJustificativasFaltaSaidaDTO = new BuscaJustificativasFaltaSaidaDTO();
                buscaJustificativasFaltaSaidaDTO.Status = 1;
                buscaJustificativasFaltaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaJustificativasFaltaSaidaDTO;
        }

        /// <summary>
        /// Busca justificativas faltas de alunos
        /// </summary>
        /// <returns>DataTable contendo as justificativas</returns>
        public static BuscaJustificativasFaltaAlunoSaidaDTO BuscaJustificativasFaltasAlunos(BuscaJustificativasFaltaAlunoEntradaDTO buscaJustificativasFaltaAlunoEntradaDTO)
        {
            BuscaJustificativasFaltaAlunoSaidaDTO buscaJustificativasFaltaAlunoSaidaDTO;
            DataTable justificativas;
            try
            {
                buscaJustificativasFaltaAlunoSaidaDTO = new BuscaJustificativasFaltaAlunoSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DateTime syncDate = String.IsNullOrEmpty(buscaJustificativasFaltaAlunoEntradaDTO.SyncDate) ? new DateTime() : Convert.ToDateTime(buscaJustificativasFaltaAlunoEntradaDTO.SyncDate);

                justificativas = dao.BuscaJustificativasFaltasAlunos(buscaJustificativasFaltaAlunoEntradaDTO.esc_id, syncDate);
                if (justificativas.Rows.Count == 0)
                {
                    return buscaJustificativasFaltaAlunoSaidaDTO;
                }

                buscaJustificativasFaltaAlunoSaidaDTO.JustificativasFaltasAlunos = (from dr in justificativas.AsEnumerable() select (JustificativaFaltaAluno)GestaoEscolarUtilBO.DataRowToEntity(dr, new JustificativaFaltaAluno())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaEscolaEntradaDTO.GetProperties());

                buscaJustificativasFaltaAlunoSaidaDTO = new BuscaJustificativasFaltaAlunoSaidaDTO();
                buscaJustificativasFaltaAlunoSaidaDTO.Status = 1;
                buscaJustificativasFaltaAlunoSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaJustificativasFaltaAlunoSaidaDTO;
        }

        /// <summary>
        /// Metodo que busca os recursos aula
        /// </summary>
        public static BuscaRecursosAulaSaidaDTO BuscaRecursosAula()
        {
            BuscaRecursosAulaSaidaDTO buscaRecursosAulaSaidaDTO;
            DataTable recursos;
            try
            {
                buscaRecursosAulaSaidaDTO = new BuscaRecursosAulaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                recursos = dao.BuscaRecursosAula();

                if (recursos.Rows.Count == 0)
                {
                    return buscaRecursosAulaSaidaDTO;
                }

                buscaRecursosAulaSaidaDTO.Recursos = (from dr in recursos.AsEnumerable() select (RecursosAula)GestaoEscolarUtilBO.DataRowToEntity(dr, new RecursosAula())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaEscolaEntradaDTO.GetProperties());

                buscaRecursosAulaSaidaDTO = new BuscaRecursosAulaSaidaDTO();
                buscaRecursosAulaSaidaDTO.Status = 1;
                buscaRecursosAulaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaRecursosAulaSaidaDTO;
        }

        /// <summary>
        /// Busca os status dos protocolos que estão pendentes.
        /// </summary>
        /// <param name="protocoloEntradaDTO">Objeto com as informações necessárias para busca os protocolos que estão pendentes.</param>
        /// <returns>Retorna um objeto com o status da requisição e uma lista de protocolos com seu status.</returns>
        public static ProtocoloSaidaDTO BuscaStatusProtocolos(ProtocoloEntradaDTO protocoloEntradaDTO)
        {
            ProtocoloSaidaDTO protocoloSaidaDTO;
            try
            {
                protocoloSaidaDTO = new ProtocoloSaidaDTO();

                DataTable saida = BuscaStatusProtocolo(protocoloEntradaDTO.Protocolos);

                if (saida.Rows.Count == 0)
                {
                    return protocoloSaidaDTO;
                }

                protocoloSaidaDTO.Protocolos = saida.Rows.Cast<DataRow>().Select(p => Util.rowToProtocolo(p, new Protocolo())).ToList<Protocolo>();
            }
            catch (Exception exp)
            {
                protocoloSaidaDTO = new ProtocoloSaidaDTO();
                protocoloSaidaDTO.Status = 1;
                protocoloSaidaDTO.StatusDescription = exp.Message;
            }
            return protocoloSaidaDTO;
        }

        /// <summary>
        /// Busca todos os tipos de atividade
        /// </summary>
        /// <returns>DataTable contendo os tipos de atividade</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaTipoAtividade(DateTime syncDate)
        {
            ApiDAO dao = new ApiDAO();
            return dao.BuscaTipoAtividade(syncDate);
        }

        /// <summary>
        /// Busca todas os tipos de atividades avaliativas existentes no gestão escolar.
        /// </summary>
        /// <returns>Retorna um objeto com o status da requisição e uma lista de tipos de atividades avaliativas.</returns>
        public static BuscaTiposAtividadeAvaliativaSaidaDTO BuscaTiposAtividadeAvaliativa(BuscaTiposAtividadeAvaliativaEntradaDTO buscaTiposAtividadeAvaliativaEntradaDTO)
        {
            BuscaTiposAtividadeAvaliativaSaidaDTO buscaTiposAtividadeAvaliativaSaidaDTO;
            try
            {
                buscaTiposAtividadeAvaliativaSaidaDTO = new BuscaTiposAtividadeAvaliativaSaidaDTO();

                DateTime syncDate;
                if (buscaTiposAtividadeAvaliativaEntradaDTO == null)
                {
                    syncDate = new DateTime();
                }
                else
                {
                    syncDate = String.IsNullOrEmpty(buscaTiposAtividadeAvaliativaEntradaDTO.SyncDate) ? new DateTime() : Convert.ToDateTime(buscaTiposAtividadeAvaliativaEntradaDTO.SyncDate);
                }

                DataTable saida = BuscaTipoAtividade(syncDate);
                if (saida.Rows.Count == 0)
                {
                    return buscaTiposAtividadeAvaliativaSaidaDTO;
                }

                buscaTiposAtividadeAvaliativaSaidaDTO.TiposAtividadeAvaliativa =
                    (
                        from DataRow row in saida.Rows
                        select (TipoAtividadeAvaliativa)GestaoEscolarUtilBO.DataRowToEntity(row, new TipoAtividadeAvaliativa())
                    ).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaTiposAtividadeAvaliativaEntradaDTO.GetProperties());

                buscaTiposAtividadeAvaliativaSaidaDTO = new BuscaTiposAtividadeAvaliativaSaidaDTO();
                buscaTiposAtividadeAvaliativaSaidaDTO.Status = 1;
                buscaTiposAtividadeAvaliativaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTiposAtividadeAvaliativaSaidaDTO;
        }

        /// <summary>
        /// Retorna uma lista com as turmas ativas da escola, podendo ser especificamente de um professor.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="doc_id">id do docente (não requerido) </param>
        /// <returns></returns>
        public static List<TUR_TurmaDTO> SelecionarTurmasPorEscolaProfessor(int esc_id, int doc_id)
        {
            try
            {
                if (esc_id == 0)
                {
                    throw new ValidationException("Parâmetro esc_id é requerido!");
                }

                ApiDAO dao = new ApiDAO();

                DataTable dt = dao.SelecionarTurmasPorEscolaProfessor(esc_id, doc_id);

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                List<TUR_TurmaDTO> turmas = (
                        from DataRow r in dt.Rows
                        select (TUR_TurmaDTO)GestaoEscolarUtilBO.DataRowToEntity(r, new TUR_TurmaDTO())
                    ).ToList();

                return turmas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Busca todas as informações, que os tablets precisam, sobre as turmas e suas disciplinas associadas.
        /// Se for passado o tur_id(identificador de uma turma), será buscado somente as informações de uma turma,
        /// caso contrário retornará as informaçõers de todas as turmas de uma escola.
        /// </summary>
        /// <param name="buscaTurmasEntradaDTO">Objeto com as informações necessárias para a busca da turma.</param>
        /// <returns>Retorna um objeto com o status da requisição e as informações da(s) tuma(s).</returns>
        public static BuscaTurmasSaidaDTO BuscaTurmas(BuscaTurmasEntradaDTO buscaTurmasEntradaDTO)
        {
            BuscaTurmasSaidaDTO buscaTurmasSaidaDTO;

            try
            {
                DataTable saida = BuscaTurmas
                    (
                        buscaTurmasEntradaDTO.esc_id,
                        buscaTurmasEntradaDTO.tur_id,
                        string.IsNullOrEmpty(buscaTurmasEntradaDTO.SyncDate) ? new DateTime() : Convert.ToDateTime(buscaTurmasEntradaDTO.SyncDate),
                        Convert.ToInt32(buscaTurmasEntradaDTO.cal_ano),
                        buscaTurmasEntradaDTO.usu_login

                    );

                buscaTurmasSaidaDTO = new BuscaTurmasSaidaDTO();
                if (saida.Rows.Count > 0)
                {
                    buscaTurmasSaidaDTO.Turmas =
                        (
                            from DataRow rowTurma in saida.Rows
                            group rowTurma by rowTurma["tur_id"] into t
                            select (Turma)GestaoEscolarUtilBO.DataRowToEntity(t.First()
                                , new Turma
                                {
                                    NivelEnsino = (NivelEnsino)GestaoEscolarUtilBO.DataRowToEntity(t.First(), new NivelEnsino())
                                        ,
                                    Disciplinas =
                                          (
                                              from DataRow dataRow in t
                                              group dataRow by dataRow["tud_id"] into d
                                              select (TurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(d.First(), new TurmaDisciplina
                                              {
                                                  TurmaDisciplinaRelacionada = (
                                                        from DataRow dataRowd in d
                                                        where !String.IsNullOrEmpty(dataRowd["tud_idRelacionada"].ToString())
                                                        group dataRowd by new { tud_id = dataRowd["tud_id"], tdr_id = dataRowd["tdr_id"], tud_idRelacionada = dataRowd["tud_idRelacionada"] }
                                                            into j
                                                        select (TUR_TurmaDisciplinaRelacionadaDTO)GestaoEscolarUtilBO.DataRowToEntity(j.First(), new TUR_TurmaDisciplinaRelacionadaDTO())
                                                  ).ToList(),
                                                  TurmaDisciplinaTerritorio = (
                                                        from DataRow dataRowd in d
                                                        where !String.IsNullOrEmpty(dataRowd["tte_id"].ToString())
                                                        group dataRowd by dataRowd["tte_id"] into j
                                                        select (TUR_TurmaDisciplinaTerritorioDTO)GestaoEscolarUtilBO.DataRowToEntity(j.First(), new TUR_TurmaDisciplinaTerritorioDTO())
                                                  ).ToList()
                                              })
                                          ).ToList()
                                        ,
                                    TurmaCurriculo = (
                                            from DataRow dr in t
                                            group dr by new { cur_id = t.First()["cur_id"], crr_id = t.First()["crr_id"], crp_id = t.First()["crp_id"] }
                                                into d
                                            select (TurmaCurriculo)GestaoEscolarUtilBO.DataRowToEntity(d.First(), new TurmaCurriculo())
                                        ).ToList()
                                }
                                )
                            ).ToList();
                }

                buscaTurmasSaidaDTO.TiposDisciplinaDeficiencia = (
                        from def in ACA_TipoDisciplinaDeficienciaBO.GetSelect()
                        select new TipoDisciplinaDeficiencia()
                        {
                            tde_id = def.tde_id,
                            tds_id = def.tds_id
                        }
                    ).ToList();
            }
            catch (Exception exp)
            {
                buscaTurmasSaidaDTO = new BuscaTurmasSaidaDTO();
                buscaTurmasSaidaDTO.Status = 1;
                buscaTurmasSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTurmasSaidaDTO;
        }

        /// <summary>
        /// Busca todas as informações, que os tablets precisam, sobre o novo planejamento.
        /// </summary>
        /// <param name="buscaTurmasEntradaDTO">Objeto com as informações necessárias para a busca da turma.</param>
        /// <returns>Retorna um objeto com o status da requisição e as informações do planejamento.</returns>
        public static PlanejamentoSaidaDTO BuscaPlanejamento(PlanejamentoEntradaDTO planejamentoEntradaDTO)
        {
            PlanejamentoSaidaDTO saidaDTO = new PlanejamentoSaidaDTO();
            try
            {

                //DateTime sync_date = string.IsNullOrEmpty(planejamentoEntradaDTO.SyncDate) ? new DateTime() : Convert.ToDateTime(planejamentoEntradaDTO.SyncDate);

                DataSet planejamento = new ApiDAO().SelecionaRetornoPlanejamento(
                    planejamentoEntradaDTO.tur_id,
                    string.IsNullOrEmpty(planejamentoEntradaDTO.SyncDate) ? new DateTime() : Convert.ToDateTime(planejamentoEntradaDTO.SyncDate));

                // PlanejamentoCiclo
                DataTable planejamentoCiclo = planejamento.Tables[0];
                saidaDTO.PlanejamentoCiclo = (from dr in planejamentoCiclo.AsEnumerable()
                                              select (CLS_PlanejamentoCiclo)GestaoEscolarUtilBO.DataRowToEntity(dr, new CLS_PlanejamentoCiclo())).ToList();

                // PlanejamentoAnual
                DataTable planejamentoAnual = planejamento.Tables[1];
                saidaDTO.PlanejamentoAnual = (from dr in planejamentoAnual.AsEnumerable()
                                              select (CLS_TurmaDisciplinaPlanejamentoDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new CLS_TurmaDisciplinaPlanejamentoDTO())).ToList();

                // PlanejamentoAluno
                DataTable planejamentoAluno = planejamento.Tables[2];
                saidaDTO.PlanejamentoAluno = (from dr in planejamentoAluno.AsEnumerable()
                                              select (CLS_AlunoPlanejamento)GestaoEscolarUtilBO.DataRowToEntity(dr, new CLS_AlunoPlanejamento())).ToList();

                // PlanejamentoAlunoRelacionada
                DataTable planejamentoAlunoRelacionada = planejamento.Tables[3];
                saidaDTO.PlanejamentoAlunoRelacionada = (from dr in planejamentoAlunoRelacionada.AsEnumerable()
                                                         select (CLS_AlunoPlanejamentoRelacionada)GestaoEscolarUtilBO.DataRowToEntity(dr, new CLS_AlunoPlanejamentoRelacionada())).ToList();

                // Documentos
                DataTable planejamentoDocumentos = planejamento.Tables[4];
                saidaDTO.PlanejamentoDocumentos = (from dr in planejamentoDocumentos.AsEnumerable()
                                                   select (ACA_ArquivoArea)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_ArquivoArea())).ToList();

                // Tipos de documentos
                DataTable tipoDocumentos = planejamento.Tables[5];
                saidaDTO.TipoDocumentos = (from dr in tipoDocumentos.AsEnumerable()
                                           select (ACA_TipoAreaDocumento)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoAreaDocumento())).ToList();
            }
            catch (Exception exp)
            {
                saidaDTO = new PlanejamentoSaidaDTO();
                saidaDTO.Status = 1;
                saidaDTO.StatusDescription = exp.Message;
            }
            return saidaDTO;
        }

        /// <summary>
        /// Busca todas as informações, que os tablets precisam, sobre o planejamento das turmas.
        /// Se for passado o tur_id(identificador de uma turma), será buscado somente as informações de uma turma,
        /// caso contrário retornará as informações de todas as turmas de uma escola.
        /// </summary>
        /// <param name="buscaTurmasEntradaDTO">Objeto com as informações necessárias para a busca da turma.</param>
        /// <returns>Retorna um objeto com o status da requisição e as informações do planejamento.</returns>
        public static ListagemPlanejamentoTurmaSaidaDTO BuscaPlanejamentoTurma(BuscaTurmasEntradaDTO buscaTurmasEntradaDTO)
        {
            ListagemPlanejamentoTurmaSaidaDTO saidaDTO = new ListagemPlanejamentoTurmaSaidaDTO();
            try
            {
                string esc_id = (buscaTurmasEntradaDTO.esc_id ?? "").Trim();
                if (esc_id.EndsWith(";"))
                    esc_id = esc_id.Remove(esc_id.Length - 1, 1);

                DateTime sync_date = string.IsNullOrEmpty(buscaTurmasEntradaDTO.SyncDate) ? new DateTime() : Convert.ToDateTime(buscaTurmasEntradaDTO.SyncDate);

                DataTable dtPlanejamentoTurma = CLS_TurmaDisciplinaPlanejamentoBO.BuscaPlanejamentoTurmaDisciplinaDT(esc_id, buscaTurmasEntradaDTO.tur_id, sync_date);
                DataTable dtPlanejamentoOrientacao = CLS_TurmaDisciplinaPlanejamentoBO.BuscaPlanejamentoOrientacaoCurricularDT(esc_id, buscaTurmasEntradaDTO.tur_id);

                if (dtPlanejamentoTurma.Rows.Count == 0)
                {
                    return saidaDTO;
                }

                // Buscar as chaves da saída (tud/tdt_posicao).
                saidaDTO.Planejamento = (from DataRow dr in dtPlanejamentoTurma.Rows
                                         group dr by new { tud_id = Convert.ToInt64(dr["tud_id"]), tdt_posicao = Convert.ToByte(dr["tdt_posicao"]) } into p
                                         select new Planejamento
                                         {
                                             tud_id = p.Key.tud_id
                                             ,
                                             tdt_posicao = p.Key.tdt_posicao
                                             ,
                                             TurmaDisciplinaPlanejamento = new List<TurmaDisciplinaPlanejamento>()
                                             ,
                                             PlanejamentoOrientacaoCurricular = new List<PlanejamentoOrientacaoCurricular>()
                                             ,
                                             PlanejamentoOrientacaoCurricularDiagnostico = new List<PlanejamentoOrientacaoCurricularDiagnostico>()
                                         }).ToList();

                TurmaDisciplinaPlanejamento entDisciplinaPlanejamento;

                // Preenche as listas
                foreach (DataRow dr in dtPlanejamentoTurma.Rows)
                {
                    entDisciplinaPlanejamento = new TurmaDisciplinaPlanejamento();
                    entDisciplinaPlanejamento = (TurmaDisciplinaPlanejamento)GestaoEscolarUtilBO.DataRowToEntity(dr, entDisciplinaPlanejamento);

                    Planejamento plan = saidaDTO.Planejamento.Find(p => p.tud_id == Convert.ToInt64(dr["tud_id"]) && p.tdt_posicao == Convert.ToByte(dr["tdt_posicao"]));

                    if (plan != null)
                    {
                        plan.TurmaDisciplinaPlanejamento.Add(entDisciplinaPlanejamento);
                    }
                }

                foreach (DataRow dr in dtPlanejamentoOrientacao.Rows)
                {
                    Planejamento plan = saidaDTO.Planejamento.Find(p => p.tud_id == Convert.ToInt64(dr["tud_id"]) && p.tdt_posicao == Convert.ToByte(dr["tdt_posicao"]));

                    if (plan != null)
                    {
                        if (dr["poc"] != DBNull.Value)
                            plan.PlanejamentoOrientacaoCurricular.Add(new PlanejamentoOrientacaoCurricular { poc = dr["poc"].ToString() });
                        if (dr["ocr"] != DBNull.Value)
                            plan.PlanejamentoOrientacaoCurricularDiagnostico.Add(new PlanejamentoOrientacaoCurricularDiagnostico { ocr = dr["ocr"].ToString() });
                    }
                }

                DataTable dtAluno = CLS_AlunoTurmaDisciplinaOrientacaoCurricularBO.BuscaAlunoOrientacaoCurricular(esc_id, buscaTurmasEntradaDTO.tur_id, sync_date);
                saidaDTO.AlunoTurmaDisciplinaOrientacaoCurricular =
                    (
                        from dr in dtAluno.AsEnumerable()
                        select (AlunoTurmaDisciplinaOrientacaoCurricular)GestaoEscolarUtilBO.DataRowToEntity(dr, new AlunoTurmaDisciplinaOrientacaoCurricular())
                    ).ToList();
            }
            catch (Exception exp)
            {
                saidaDTO = new ListagemPlanejamentoTurmaSaidaDTO();
                saidaDTO.Status = 1;
                saidaDTO.StatusDescription = exp.Message;
            }
            return saidaDTO;
        }

        /// <summary>
        /// Método que busca dados das turmas e disciplinas das turmas
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="syncDate">Menor data de sincronização dos registros da turma (Opcional)</param>
        /// <param name="anoLetivo">Ano letivo</param>
        /// <returns>DataTable contendo os dados das turmas e disciplinas</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaTurmas(string esc_id, Int64 tur_id, DateTime syncDate, int anoLetivo, string usu_login)
        {
            esc_id = (esc_id ?? "").Trim();

            if (esc_id.EndsWith(";"))
                esc_id = esc_id.Remove(esc_id.Length - 1, 1);

            ApiDAO dao = new ApiDAO();
            return dao.BuscaTurmas(esc_id, tur_id, syncDate, anoLetivo, usu_login);
        }

        /// <summary>
        /// retorna registro de aluno por id
        /// </summary>
        /// <param name="alu_id">id do aluno</param>
        /// <returns></returns>
        public static ACA_AlunoDTO SelecionarAlunoPorId(int alu_id)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                ACA_Aluno aluno = dao.SelecionarAlunoPorId(alu_id);

                ACA_AlunoDTO dto = null;

                if (aluno != null)
                {
                    dto = (ACA_AlunoDTO)GestaoEscolarUtilBO.Clone(aluno, new ACA_AlunoDTO());
                }

                return dto;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna registros de TurmaDisciplina por turma
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <returns></returns>
        public static List<TUR_TurmaDisciplinaDTO> SelecionarTurmaDisciplinaPorTurma(long tur_id)
        {
            try
            {
                if (!(tur_id > 0))
                {
                    throw new ValidationException("Id da turma é requerido!");
                }

                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.SelecionarTurmaDisciplinaPorTurma(tur_id);

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                List<TUR_TurmaDisciplinaDTO> lista = (
                    from r in dt.AsEnumerable()
                    select (TUR_TurmaDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(r, new TUR_TurmaDisciplinaDTO())
                    ).ToList();

                return lista;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Busca todas as informações, que os tablets precisam, sobre os Usuários (Administradores e Professores).
        /// </summary>
        /// <param name="buscaUsuariosEntradaDTO">Objeto com uma lista com os identificadores dos usuários professores, e uma lista com o identificador dos usuários administradores.</param>
        /// <returns>Retorna um objeto com o status da requisição e uma lista de usuários dos tipos professores e administradores</returns>
        public static BuscaUsuariosSaidaDTO BuscaUsuarios(BuscaUsuariosEntradaDTO buscaUsuariosEntradaDTO)
        {
            BuscaUsuariosSaidaDTO buscaUsuariosSaidaDTO;
            try
            {
                DataTable saida = BuscaUsuarios
                    (
                        buscaUsuariosEntradaDTO.esc_id,
                        buscaUsuariosEntradaDTO.Usu_login,
                        Convert.ToDateTime(buscaUsuariosEntradaDTO.SyncDate));

                buscaUsuariosSaidaDTO = new BuscaUsuariosSaidaDTO();
                if (saida.Rows.Count == 0)
                {
                    return buscaUsuariosSaidaDTO;
                }

                buscaUsuariosSaidaDTO.Usuarios =
                    (
                        from DataRow row in saida.Rows
                        group row by row["usu_id"]
                            into u
                        select (SYS_UsuarioDTO.Usuario)GestaoEscolarUtilBO.DataRowToEntity(u.First(),
                            new SYS_UsuarioDTO.Usuario
                            {
                                Grupos = (
                                    from DataRow rowGrupo in u
                                    group rowGrupo by rowGrupo["gru_id"] into g
                                    select (UsuarioGrupo)GestaoEscolarUtilBO.DataRowToEntity(g.First(), new UsuarioGrupo())
                                ).ToList(),
                                Professor = (Professor)GestaoEscolarUtilBO.DataRowToEntity(u.First(),
                                    new Professor
                                    {
                                        Turmas =
                                        (
                                            from DataRow rowTurma in u
                                            where (!string.IsNullOrEmpty(rowTurma["tur_id"].ToString()))
                                            group rowTurma by rowTurma["tur_id"] into t
                                            select (Turma)GestaoEscolarUtilBO.DataRowToEntity(t.First(),
                                                new Turma
                                                {
                                                    Disciplinas =
                                                    (
                                                       from DataRow rowDisciplina in u
                                                       where (!string.IsNullOrEmpty(rowDisciplina["tud_id"].ToString()) && t.First()["tur_id"].ToString().Equals(rowDisciplina["tur_id"].ToString()))
                                                       group rowDisciplina by new
                                                       {
                                                           tud_id = rowDisciplina["tud_id"],
                                                           tdt_id = rowDisciplina["tdt_id"]
                                                       } into dis
                                                       select (TurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dis.First(), new TurmaDisciplina())
                                                     ).ToList()
                                                }
                                            )
                                        ).ToList()
                                    }
                                )

                            }
                        )
                    ).ToList();
            }
            catch (Exception exp)
            {
                buscaUsuariosSaidaDTO = new BuscaUsuariosSaidaDTO();
                buscaUsuariosSaidaDTO.Status = 1;
                buscaUsuariosSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaUsuariosSaidaDTO;
        }

        /// <summary>
        /// Método que busca usuários
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="usu_login">Login do usuário (Opcinal)</param>
        /// <param name="syncDate">Menor data de sincronização dos registros dos Alunos na Turma especificada (Opcional)</param>
        /// <param name="dtProfessores">DataTable contendo os doc_id dos professores (Passar apenas doc_id)</param>
        /// <param name="dtAdministradores">DataTable contendo os usu_id dos administradores (Passar apenas usu_id)</param>
        /// <returns>DataTable contendo os usuários</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaUsuarios(int esc_id, string usu_login, DateTime syncDate)
        {
            if (esc_id <= 0)
            {
                throw new EscolaVaziaException();
            }

            ApiDAO dao = new ApiDAO();
            DataTable dtUsuarios = dao.BuscaUsuarios(esc_id, usu_login, syncDate);

            if (dtUsuarios.Rows.Count == 0)
            {
                if (!string.IsNullOrEmpty(usu_login))
                    throw new UsuarioNaoEncontradosException(usu_login);

                throw new UsuariosNaoEncontradosException();
            }

            return dtUsuarios;
        }

        /// <summary>
        /// Método que carrega os dados de configuração
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <returns>DataTable contendo os dados de configuração</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable DadosIniciais(int esc_id)
        {
            if (esc_id <= 0)
            {
                throw new EscolaVaziaException();
            }

            ApiDAO dao = new ApiDAO();

            DataTable dtDadosIniciais = dao.DadosIniciais(esc_id);

            return dtDadosIniciais;
        }

        /// <summary>
        /// Busca as escalas de avaliação ativas para a entidade.
        /// </summary>
        /// <param name="buscaEscalaAvaliacaoEntradaDTO"></param>
        /// <returns></returns>
        public static BuscaEscalaAvaliacaoSaidaDTO ListagemEscalaAvaliacao(BuscaEscalaAvaliacaoEntradaDTO buscaEscalaAvaliacaoEntradaDTO)
        {
            BuscaEscalaAvaliacaoSaidaDTO buscaEscalaAvaliacaoSaidaDTO;
            try
            {
                buscaEscalaAvaliacaoSaidaDTO = new BuscaEscalaAvaliacaoSaidaDTO();

                DataTable dt = ACA_EscalaAvaliacaoBO.BuscaEscalasAvaliacaoPorChaveK1(buscaEscalaAvaliacaoEntradaDTO.K1);

                if (dt.Rows.Count == 0)
                {
                    return buscaEscalaAvaliacaoSaidaDTO;
                }

                buscaEscalaAvaliacaoSaidaDTO.listEscalaAvaliacao = (
                        from DataRow dr in dt.Rows
                        group dr by dr["esa_id"] into r
                        select (EscalaAvaliacao)GestaoEscolarUtilBO.DataRowToEntity(r.First(), new EscalaAvaliacao
                        {
                            escalaAvaliacaoNumerica = (
                                (Convert.ToInt16(r.First()["esa_tipo"]) == (Byte)EscalaAvaliacaoTipo.Numerica)
                                ? (EscalaAvaliacaoNumerica)GestaoEscolarUtilBO.DataRowToEntity(r.First(), new EscalaAvaliacaoNumerica())
                                : null
                            ),
                            listEscalaAvaliacaoParecer = (
                                (Convert.ToInt16(r.First()["esa_tipo"]) == (Byte)EscalaAvaliacaoTipo.Pareceres)
                                ? from DataRow row in r.AsEnumerable()
                                  select (EscalaAvaliacaoParecer)GestaoEscolarUtilBO.DataRowToEntity(row, new EscalaAvaliacaoParecer())
                                : new List<EscalaAvaliacaoParecer>()
                            ).ToList()
                        })
                    ).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaTiposAtividadeAvaliativaEntradaDTO.GetProperties());

                buscaEscalaAvaliacaoSaidaDTO = new BuscaEscalaAvaliacaoSaidaDTO();
                buscaEscalaAvaliacaoSaidaDTO.Status = 1;
                buscaEscalaAvaliacaoSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaEscalaAvaliacaoSaidaDTO;
        }

        /// <summary>
        /// Metodo que busca as informações de uma escola.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com as informações que identifiquem qual escola será buscada.</param>
        /// <returns>Retorna um objeto com o status da requisição e as informações da escola solicitada.</returns>
        public static BuscaEscolaSaidaDTO ListagemEscolas(BuscaEscolaEntradaDTO buscaEscolaEntradaDTO)
        {
            BuscaEscolaSaidaDTO buscaEscolaSaidaDTO;
            DataTable saida;
            try
            {
                buscaEscolaSaidaDTO = new BuscaEscolaSaidaDTO();

                saida = BuscaEscola(buscaEscolaEntradaDTO.K1, buscaEscolaEntradaDTO.Uad_codigo);
                if (saida.Rows.Count == 0)
                {
                    return buscaEscolaSaidaDTO;
                }

                buscaEscolaSaidaDTO.Escola = (ESC_EscolaDTO.EscolaEndereco)GestaoEscolarUtilBO.DataRowToEntity(saida.Rows[0], new ESC_EscolaDTO.EscolaEndereco());
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaEscolaEntradaDTO.GetProperties());

                buscaEscolaSaidaDTO = new BuscaEscolaSaidaDTO();
                buscaEscolaSaidaDTO.Status = 1;
                buscaEscolaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaEscolaSaidaDTO;
        }

        /// <summary>
        /// Busca as escolas de um professor atravez do seu login
        /// </summary>
        /// <param name="buscaEscolasProfessorEntradaDTO"></param>
        /// <returns></returns>
        public static BuscaEscolasProfessorSaidaDTO ListagemEscolasProfessor(BuscaEscolasProfessorEntradaDTO buscaEscolasProfessorEntradaDTO)
        {
            BuscaEscolasProfessorSaidaDTO buscaEscolasProfessorSaidaDTO;
            DataTable saida;
            try
            {
                buscaEscolasProfessorSaidaDTO = new BuscaEscolasProfessorSaidaDTO();

                saida = BuscaEscolasProfessor(buscaEscolasProfessorEntradaDTO.usu_login);
                if (saida.Rows.Count == 0)
                {
                    return buscaEscolasProfessorSaidaDTO;
                }

                List<ESC_EscolaDTO.EscolaEndereco> escolas = new List<ESC_EscolaDTO.EscolaEndereco>();

                foreach (DataRow dr in saida.Rows)
                {
                    escolas.Add((ESC_EscolaDTO.EscolaEndereco)GestaoEscolarUtilBO.DataRowToEntity(dr, new ESC_EscolaDTO.EscolaEndereco()));
                }

                buscaEscolasProfessorSaidaDTO.lstEscola = escolas;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaEscolaEntradaDTO.GetProperties());

                buscaEscolasProfessorSaidaDTO = new BuscaEscolasProfessorSaidaDTO();
                buscaEscolasProfessorSaidaDTO.Status = 1;
                buscaEscolasProfessorSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaEscolasProfessorSaidaDTO;
        }

        /// <summary>
        /// Retorna uma lista de escolas vinculadas ao login do professor.
        /// </summary>
        /// <param name="usu_login">login do professor</param>
        /// <returns></returns>
        public static List<ESC_EscolaDTO.EscolaDadosBasicos> SelecionarEscolasProfessorPorLogin(string usu_login)
        {
            try
            {
                DataTable saida = BuscaEscolasProfessor(usu_login);
                if (saida.Rows.Count == 0)
                {
                    return null;
                }

                List<ESC_EscolaDTO.EscolaDadosBasicos> escolas = new List<ESC_EscolaDTO.EscolaDadosBasicos>();

                foreach (DataRow dr in saida.Rows)
                {
                    escolas.Add((ESC_EscolaDTO.EscolaDadosBasicos)GestaoEscolarUtilBO.DataRowToEntity(dr, new ESC_EscolaDTO.EscolaDadosBasicos()));
                }

                return escolas;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo que busca os parâmetros acadêmicos solicitados.
        /// </summary>
        /// <param name="buscaEntrada">Objeto com as informações que identifiquem quais parâmetros serão buscados</param>
        /// <returns>Retorna um objeto com os parâmetros solicitados</returns>
        public static BuscaParametrosAcademicosSaidaDTO ListagemParametrosAcademicos(BuscaParametrosAcademicosEntradaDTO buscaEntrada)
        {
            BuscaParametrosAcademicosSaidaDTO buscaSaida = new BuscaParametrosAcademicosSaidaDTO();
            try
            {
                if (string.IsNullOrEmpty(buscaEntrada.ChavesParametros))
                {
                    buscaSaida.Status = 1;
                    buscaSaida.StatusDescription = "É necessário informar os filtros de entrada.";

                    return buscaSaida;
                }

                string[] chaves = (buscaEntrada.ChavesParametros ?? string.Empty).Split(';');
                List<string> listaChavesMaiusculo = (from string c in chaves select c.ToUpper()).ToList();

                DataTable dt = ACA_ParametroAcademicoBO.GetSelect(buscaEntrada.ent_id, false, 1, 1);

                if (dt.Rows.Count == 0)
                {
                    return buscaSaida;
                }

                // Busca os parâmetros acadêmicos pelas chaves.
                List<ParametrosAcademicos> listaParametros =
                    (from DataRow dr in dt.Rows
                     where chaves.Contains(dr["pac_chave"].ToString())
                     select (ParametrosAcademicos)GestaoEscolarUtilBO.DataRowToEntity
                     (dr, new ParametrosAcademicos())).ToList();

                List<CFG_ParametroMensagem> listaParametroMensagem = CFG_ParametroMensagemBO.GetSelect().ToList();

                // Busca também nos parâmetros de mensagem.
                listaParametros.AddRange(
                   (from CFG_ParametroMensagem item in listaParametroMensagem
                        // Busca dos parâmetros de mensagem a chave em maiúsculo.
                    where listaChavesMaiusculo.Exists(p => p == item.pms_chave.ToUpper())
                    select new ParametrosAcademicos
                    {
                        pac_chave = item.pms_chave
                         ,
                        pac_id = item.pms_id
                         ,
                        pac_valor = item.pms_valor
                         ,
                        pac_descricao = item.pms_descricao
                    }).ToList());

                List<CFG_ConfiguracaoAcademico> listaConfiguracao = CFG_ConfiguracaoAcademicoBO.Consultar();

                // Busca também nas configurações do sistema.
                listaParametros.AddRange(
                   (from CFG_ConfiguracaoAcademico item in listaConfiguracao
                    where chaves.Contains(item.cfg_chave)
                    select new ParametrosAcademicos
                    {
                        pac_chave = item.cfg_chave
                         ,
                        pac_id = 0
                         ,
                        pac_valor = item.cfg_valor
                         ,
                        pac_descricao = item.cfg_descricao
                    }).ToList());

                RES_ChaveResourceDAO chaveResourceDao = new RES_ChaveResourceDAO();
                DataTable dtResource = chaveResourceDao.SelecionaPorNomeCulturaChaves(buscaEntrada.ChavesParametros, "pt-BR");

                listaParametros.AddRange(
                    (from DataRow r in dtResource.Rows
                     select new ParametrosAcademicos
                     {
                         pac_id = int.Parse(r["rcr_codigo"].ToString()),
                         pac_chave = r["rcr_chave"].ToString(),
                         pac_valor = r["rcr_valor"].ToString(),
                         pac_descricao = r["rcr_NomeResource"].ToString()
                     }).ToList());

                buscaSaida.Parametros = listaParametros;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaEscolaEntradaDTO.GetProperties());

                buscaSaida = new BuscaParametrosAcademicosSaidaDTO();
                buscaSaida.Status = 1;
                buscaSaida.StatusDescription = exp.Message;
            }

            return buscaSaida;
        }

        /// <summary>
        /// Metodo que busca os parâmetros acadêmicos solicitados.
        /// </summary>
        /// <param name="buscaEntrada">Objeto com as informações que identifiquem quais parâmetros serão buscados</param>
        /// <returns>Retorna um objeto com os parâmetros solicitados</returns>
        public static BuscaParametrosAcademicosSaidaDTO ListagemParametrosAcademicosPlataforma(BuscaParametrosAcademicosEntradaDTO buscaEntrada)
        {
            BuscaParametrosAcademicosSaidaDTO buscaSaida = new BuscaParametrosAcademicosSaidaDTO();
            try
            {
                if (string.IsNullOrEmpty(buscaEntrada.ChavesParametros))
                {
                    buscaSaida.Status = 1;
                    buscaSaida.StatusDescription = "É necessário informar os filtros de entrada.";

                    return buscaSaida;
                }

                string[] chaves = (buscaEntrada.ChavesParametros ?? string.Empty).Split(';');

                DataTable dt = ACA_ParametroAcademicoBO.GetSelect(buscaEntrada.ent_id, false, 1, 1);

                if (dt.Rows.Count == 0)
                {
                    buscaSaida.Status = 1;
                    buscaSaida.StatusDescription = "A Consulta não retornou resultado!";

                    return buscaSaida;
                }

                // Busca os parâmetros acadêmicos pelas chaves.
                List<ParametrosAcademicos> listaParametros =
                    (from DataRow dr in dt.Rows
                     where chaves.Contains(dr["pac_chave"].ToString())
                     select (ParametrosAcademicos)GestaoEscolarUtilBO.DataRowToEntity
                     (dr, new ParametrosAcademicos())).ToList();

                List<CFG_ParametroMensagem> listaParametroMensagem = CFG_ParametroMensagemBO.GetSelect().ToList();

                // Busca também nos parâmetros de mensagem.
                listaParametros.AddRange(
                   (from CFG_ParametroMensagem item in listaParametroMensagem
                    where chaves.Contains(item.pms_chave)
                    select new ParametrosAcademicos
                    {
                        pac_chave = item.pms_chave
                         ,
                        pac_id = item.pms_id
                         ,
                        pac_valor = item.pms_valor
                         ,
                        pac_descricao = item.pms_descricao
                    }).ToList());

                buscaSaida.Parametros = listaParametros;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaEscolaEntradaDTO.GetProperties());

                buscaSaida = new BuscaParametrosAcademicosSaidaDTO();
                buscaSaida.Status = 1;
                buscaSaida.StatusDescription = exp.Message;
            }

            return buscaSaida;
        }

        /// <summary>
        /// Busca o ID da escola de acordo com o ID da unidade administrativa e entidades passados.
        /// </summary>
        /// <param name="buscaDadosIniciaisEntradaDTO">Retorna um objeto com esc_id e o status da requisicao</param>
        /// <returns></returns>
        public static RetornaEscolaSaidaDTO RetornaEscola(RetornaEscolaEntradaDTO retornaEscolaEntradaDTO)
        {
            RetornaEscolaSaidaDTO retornaEscolaSaidaDTO;
            try
            {
                retornaEscolaSaidaDTO = new RetornaEscolaSaidaDTO();

                ESC_Escola esc = new ESC_Escola
                {
                    uad_id = retornaEscolaEntradaDTO.Uad_id,
                    ent_id = retornaEscolaEntradaDTO.Ent_id
                };

                ESC_EscolaBO.ConsultarPorUnidadeAdministrativa(esc);

                retornaEscolaSaidaDTO.esc_id = esc.esc_id;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, buscaTiposAtividadeAvaliativaEntradaDTO.GetProperties());

                retornaEscolaSaidaDTO = new RetornaEscolaSaidaDTO();
                retornaEscolaSaidaDTO.Status = 1;
                retornaEscolaSaidaDTO.StatusDescription = exp.Message;
            }
            return retornaEscolaSaidaDTO;
        }

        /// <summary>
        /// Grava o protocolo por post/put na API.
        /// </summary>
        /// <param name="protocolo"></param>
        /// <returns></returns>
        public static DCL_Protocolo SalvarProtocolo(DCL_Protocolo protocolo)
        {
            try
            {
                if (protocolo.pro_status.Equals(0))
                    protocolo.pro_status = 1;

                if (DCL_ProtocoloBO.Save(protocolo))
                {
                    protocolo = DCL_ProtocoloBO.GetEntity(protocolo);
                }
                else
                {
                    throw new Exception("Erro ao gravar protocolo.");
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return protocolo;
        }

        /// <summary>
        /// Salva o protocolo informado, buscando o equipamento pela chave (K4), de acordo com o tipo de protocolo informado.
        /// </summary>
        /// <param name="k4">Chave do equipamento</param>
        /// <param name="pro_protocolo">ID do protocolo - caso seja reeenvio</param>
        /// <param name="pro_pacote">Pacote do protocolo (conteúdo JSon enviado no post)</param>
        /// <param name="versao">Versão do aplicativo</param>
        /// <param name="tipoProtocolo">Tipo de protocolo a ser salvo</param>
        /// <returns></returns>
        public static Int64 SincronizaProtocolo(string k4, long pro_protocolo, string pro_pacote, string versao, DCL_ProtocoloBO.eTipo tipoProtocolo)
        {
            if (string.IsNullOrEmpty(k4))
            {
                throw new k4VaziaException();
            }

            if (string.IsNullOrEmpty(pro_pacote))
            {
                throw new PacoteVazioException();
            }

            SYS_Equipamento equipamento = SYS_EquipamentoBO.CarregarPor_Identificador(k4);

            if (equipamento.IsNew)
            {
                throw new EquipamentoNaoEncontradoException(k4);
            }

            DCL_Protocolo entity = new DCL_Protocolo();

            entity.pro_tipo = (byte)tipoProtocolo;

            if (pro_protocolo > 0)
            {
                entity.pro_protocolo = pro_protocolo;
                entity = DCL_ProtocoloBO.GetEntityBy_Protocolo(pro_protocolo, equipamento.ent_id);

                DCL_ProtocoloBO.Reprocessar(entity.pro_id, pro_pacote, versao);
            }
            else
            {
                if (!string.IsNullOrEmpty(versao))
                {
                    entity.pro_versaoAplicativo = versao;
                }

                entity.equ_id = equipamento.equ_id;
                entity.pro_pacote = pro_pacote;
                entity.pro_status = 1;
                entity.pro_situacao = 1;

                //Save retorna e preenche o campo pro_protocolo
                DCL_ProtocoloBO.Save(entity);
            }

            return entity.pro_protocolo;
        }

        /// <summary>
        /// Salva o protocolo na tabela de acordo com o tipo e usando dados de entrada.
        /// </summary>
        /// <param name="dadosEntrada">Dados do protocolo</param>
        /// <param name="tipoProtocolo">Tipo de protocolo</param>
        /// <returns></returns>
        public static SincronizacaoDiarioClasseSaidaDTO SincronizaDiarioClasse
            (SincronizacaoDiarioClasseEntradaDTO dadosEntrada, DCL_ProtocoloBO.eTipo tipoProtocolo)
        {
            SincronizacaoDiarioClasseSaidaDTO sincronizacaoDiarioClasseSaidaDTO;
            try
            {
                //JavaScriptSerializer js = new JavaScriptSerializer();
                Int64 protocolo = SincronizaProtocolo
                (
                    dadosEntrada.K4
                    , dadosEntrada.Pro_protocolo
                    , dadosEntrada.pro_pacote
                    , dadosEntrada.Versao
                    , tipoProtocolo
                );

                sincronizacaoDiarioClasseSaidaDTO = new SincronizacaoDiarioClasseSaidaDTO();
                sincronizacaoDiarioClasseSaidaDTO.Protocolo.Pro_protocolo = protocolo;
                sincronizacaoDiarioClasseSaidaDTO.Protocolo.Pro_status = 1;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                sincronizacaoDiarioClasseSaidaDTO = new SincronizacaoDiarioClasseSaidaDTO();
                sincronizacaoDiarioClasseSaidaDTO.Status = 1;
                sincronizacaoDiarioClasseSaidaDTO.StatusDescription = exp.Message;
            }
            return sincronizacaoDiarioClasseSaidaDTO;
        }

        /// <summary>
        /// cria ou atualiza o equipamento do diario de classe
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="k1"></param>
        /// <param name="k4"></param>
        /// <param name="appVersion"></param>
        /// <param name="soVersion"></param>
        /// <param name="sisId"></param>
        /// <param name="criaEquipamento"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static SYS_Equipamento CriaEditaEquipamento(Guid ent_id, string k1, string k4, string appVersion, string soVersion, int sisId, bool criaEquipamento)
        {
            ApiDAO dao = new ApiDAO();
            TalkDBTransaction banco = dao._Banco;

            try
            {
                banco.Open();

                if (string.IsNullOrEmpty(k4))
                {
                    throw new k4VaziaException();
                }

                SYS_Equipamento sysEquipamento;
                DataTable dtEquipamentos = SYS_EquipamentoBO.CarregarBy_entidade_indentificador(ent_id, k4);

                if (dtEquipamentos.Rows.Count == 0 && criaEquipamento) //Se equipamento não existe, cadastra
                {
                    sysEquipamento = new SYS_Equipamento
                    {
                        IsNew = true,
                        ent_id = ent_id,
                        equ_dataAlteracao = DateTime.Now,
                        equ_dataCriacao = DateTime.Now,
                        equ_identificador = k4,
                        equ_situacao = 1,
                        equ_soVersion = soVersion,
                        equ_appVersion = appVersion,
                        sis_id = sisId,
                    };
                }
                else
                {
                    sysEquipamento = new SYS_Equipamento
                    {
                        equ_id = new Guid(dtEquipamentos.Rows[0]["equ_id"].ToString())
                    };

                    SYS_EquipamentoBO.GetEntity(sysEquipamento);

                    sysEquipamento.equ_dataAlteracao = DateTime.Now;
                    sysEquipamento.equ_appVersion = appVersion;
                    sysEquipamento.equ_soVersion = soVersion;
                    sysEquipamento.sis_id = sisId;
                }

                if (sysEquipamento != null)
                {
                    SYS_EquipamentoBO.Save(sysEquipamento, banco);
                }

                return sysEquipamento;
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw err;
            }
            finally
            {
                banco.Close();
            }
        }

        /// <summary>
        /// atualiza os dados da versão do diario e versão do android do tablet
        /// </summary>
        /// <param name="k1">chave do sistema para a entidade</param>
        /// <param name="uad_codigo">codigo da unidade administrativa</param>
        /// <param name="k4">identificacao do equipamento no diario</param>
        /// <param name="appVersion">versao do diario de classe</param>
        /// <param name="soVersion">versao do android</param>
        /// <param name="sisId">versao do sistema diario de classe</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        private static bool AtualizaEquipamento(string k1, string uad_codigo, string k4, string appVersion, string soVersion, int sisId)
        {
            ApiDAO dao = new ApiDAO();
            TalkDBTransaction banco = dao._Banco;

            try
            {
                banco.Open();

                if (string.IsNullOrEmpty(k1))
                {
                    throw new k1VaziaException();
                }

                if (string.IsNullOrEmpty(uad_codigo))
                {
                    throw new uadCodigoVazioException();
                }

                DataTable dtEscola = BuscaEscola(k1, uad_codigo);

                if (dtEscola.Rows.Count > 0)
                {
                    Guid ent_id = new Guid(dtEscola.Rows[0]["ent_id"].ToString());

                    DataTable dtEquipamentos = SYS_EquipamentoBO.CarregarBy_entidade_indentificador(ent_id, k4);

                    SYS_Equipamento equipamento = CriaEditaEquipamento(ent_id, k1, k4, appVersion, soVersion, sisId, false);

                    if (equipamento != null)
                        return true;
                }
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw err;
            }
            finally
            {
                banco.Close();
            }

            return false;
        }

        /// <summary>
        /// Método que associa escola informada ao Tablet
        /// </summary>
        /// <param name="k1">Chave do sistema para a entidade</param>
        /// <param name="uad_codigo">Código da unidade administrativa</param>
        /// <param name="k4">Código do equipamento</param>
        /// <returns>True - Escola associada com sucesso; False - Escola não associada</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        private static bool AssociaEscola(string k1, string uad_codigo, string k4, string appVersion, string soVersion, int sisId)
        {
            ApiDAO dao = new ApiDAO();
            TalkDBTransaction banco = dao._Banco;

            try
            {
                banco.Open();

                if (string.IsNullOrEmpty(k1))
                {
                    throw new k1VaziaException();
                }

                if (string.IsNullOrEmpty(uad_codigo))
                {
                    throw new uadCodigoVazioException();
                }

                DataTable dtEscola = BuscaEscola(k1, uad_codigo);

                if (dtEscola.Rows.Count > 0)
                {
                    Guid ent_id = new Guid(dtEscola.Rows[0]["ent_id"].ToString());

                    DataTable dtEquipamentos = SYS_EquipamentoBO.CarregarBy_entidade_indentificador(ent_id, k4);

                    SYS_Equipamento sysEquipamento = CriaEditaEquipamento(ent_id, k1, k4, appVersion, soVersion, sisId, true);
                    SYS_EquipamentoUnidadeAdministrativa sysEquipamentoUnidade = new SYS_EquipamentoUnidadeAdministrativa
                    {
                        ent_id = ent_id,
                        uad_id = new Guid(dtEscola.Rows[0]["uad_id"].ToString()),
                        equ_id = sysEquipamento.equ_id
                    };

                    sysEquipamentoUnidade = SYS_EquipamentoUnidadeAdministrativaBO.GetEntity(sysEquipamentoUnidade);

                    if (sysEquipamentoUnidade.IsNew)
                    {
                        sysEquipamentoUnidade.ent_id = ent_id;
                        sysEquipamentoUnidade.equ_id = sysEquipamento.equ_id;
                        sysEquipamentoUnidade.uad_id = new Guid(dtEscola.Rows[0]["uad_id"].ToString());
                        sysEquipamentoUnidade.eua_dataCriacao = DateTime.Now;
                        sysEquipamentoUnidade.eua_dataAlteracao = DateTime.Now;
                        sysEquipamentoUnidade.eua_situacao = 1;

                        SYS_EquipamentoUnidadeAdministrativaBO.Save(sysEquipamentoUnidade, banco);
                    }

                    return true;
                }

                throw new EscolaNaoEncontradaException(uad_codigo);
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw err;
            }
            finally
            {
                banco.Close();
            }
        }

        /// <summary>
        /// Método que busca escola
        /// </summary>
        /// <param name="k1">Chave do sistema para a entidade</param>
        /// <param name="uad_codigo">Código da unidade administrativa</param>
        /// <returns>DataTable contendo ent_id, uad_id, uad_codigo e uad_nome</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        private static DataTable BuscaEscola(string k1, string uad_codigo)
        {
            if (string.IsNullOrEmpty(k1))
            {
                throw new k1VaziaException();
            }

            if (string.IsNullOrEmpty(uad_codigo))
            {
                throw new uadCodigoVazioException();
            }

            ApiDAO dao = new ApiDAO();

            DataTable dt = dao.BuscaEscola(k1, uad_codigo);

            if (dt.Rows.Count == 0)
            {
                throw new EscolaNaoEncontradaException(uad_codigo);
            }

            return dt;
        }

        /// <summary>
        /// Método que busca escolas a partir do usuario do professor
        /// </summary>
        /// <param name="usu_login"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        private static DataTable BuscaEscolasProfessor(String usu_login)
        {
            ApiDAO dao = new ApiDAO();

            DataTable dt = dao.BuscaEscolasProfessor(usu_login);

            if (dt.Rows.Count == 0)
            {
                throw new NenhumaEscolaEncontradaException(usu_login);
            }

            return dt;
        }

        /// <summary>
        /// Método que busca e retorna os status dos protocolos
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="uad_id">Id da unidade administrativa</param>
        /// <param name="dtProtocolos">DataTable contendo os identificadores dos protocolos (Passar apenas pro_protocolo)</param>
        /// <returns>DataTable conténdo os status do protocolos</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        private static DataTable BuscaStatusProtocolo(String protocolos)
        {
            if (string.IsNullOrEmpty(protocolos))
            {
                throw new ProtocolosNaoEnviadosException();
            }

            ApiDAO dao = new ApiDAO();
            return dao.BuscaStatusProtocolos(protocolos);
        }

        #endregion Sistema Diário de classe

        #region Sistema SIG

        /// <summary>
        /// Retorna a quantidade de alunos aprovados no fechamento do bimestre.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaQuantidadeAlunosAprovados(string anosLetivos)
        {
            ApiDAO dao = new ApiDAO();

            decimal percentualFrequenciaPadrao = 75;
            return dao.SelecionaQuantidadeAlunosAprovados(anosLetivos, percentualFrequenciaPadrao);
        }

        /// <summary>
        /// Retorna a quantidade de alunos por resuldado no fechamento do bimestre.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        public static DataTable SelecionaQuantidadeAlunosResultados(string cal_ano)
        {
            ApiDAO dao = new ApiDAO();
            int ano;
            if (string.IsNullOrEmpty(cal_ano))
            {
                // Fixo 2014 se não enviar.
                ano = 2014;
            }
            else
            {
                ano = Convert.ToInt32(cal_ano);
            }

            return dao.SelecionaQuantidadeAlunosResultados(ano);
        }

        /// <summary>
        /// Retorna a quantidade de alunos que estão na idade ideal
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaQuantidadeAlunoIdadeIdeal()
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaQuantidadeAlunoIdadeIdeal();
        }

        /// <summary>
        /// Retorna a quantidade de alunos que estão na idade ideal no ensino fundamental
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaQuantidadeAlunoIdadeIdealFundamental()
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaQuantidadeAlunoIdadeIdealFundamental();
        }

        /// <summary>
        /// Retorna a quantidade de alunos e turmas por escola
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaQuantidadeAlunoTurmaPorEscola()
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaQuantidadeAlunoTurmaPorEscola();
        }

        /// <summary>
        /// Retorna a quantidade de alunos aprovados, reprovados e que tiveram movimentação de abandono por escola
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTaxaRendimento()
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaTaxaRendimento();
        }

        /// <summary>
        /// Retorna a quantidade de escolas que iniciaram o fechamento.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaEscolaFechamento()
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaEscolaFechamento();
        }

        /// <summary>
        /// Retorna a quantidade de turmas que iniciaram o fechamento.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTurmasFechamento()
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaTurmasFechamento();
        }

        /// <summary>
        /// Retorna a quantidade de alunos de finalizaram o boletim.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAlunosFechamento()
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaAlunosFechamento();
        }

        /// <summary>
        /// Retorna a quantidade de pais/responsáveis que acessaram o boletim online.
        /// </summary>
        /// <returns>DataTable com o resultado</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaResponsaveisFechamento()
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaResponsaveisFechamento();
        }

        #endregion Sistema SIG

        #region Geral - Sistema Gestão Escolar

        #region ACA_Curso

        /// <summary>
        /// Seleciona todos os cursos ativos.
        /// </summary>
        /// <returns>List<ACA_CursoDTO></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_CursoDTO> SelecionarCursos(CursoEntradaDTO filtro)
        {
            try
            {
                using (DataTable dtDados = new ACA_CursoDAO().SelecionaCursoPorIdNome(filtro.cur_id, filtro.cur_nome))
                {
                    return ConvertDataTableToACA_ListACA_CursoDTO(dtDados);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Busca os cusor filtrndo pela escola
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <returns></returns>
        public static List<ACA_CursoDTO> SelecionarCursosByEsc_id(long esc_id)
        {
            try
            {
                DataTable dtDados = new ACA_CursoDAO().SelecionaCursoByEsc_id(esc_id);

                return ConvertDataTableToACA_ListACA_CursoDTO(dtDados);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Convert o dataTable passado para uma Lista de ACA_CursoDTO
        /// foi criado para reaproveitar o codigo em metodos diferentes
        /// </summary>
        /// <param name="dtDados"></param>
        /// <returns></returns>
        private static List<ACA_CursoDTO> ConvertDataTableToACA_ListACA_CursoDTO(DataTable dtDados)
        {
            #region Delegate
            Func<ACA_CursoDTO, ACA_CurriculoDTO, List<ACA_CursoRelacionadoDTO>, List<ACA_CurriculoPeriodoDTO>, ACA_CursoDTO> retornaUsuario =
                delegate (ACA_CursoDTO curso, ACA_CurriculoDTO curriculo, List<ACA_CursoRelacionadoDTO> listaCursoRelacionado, List<ACA_CurriculoPeriodoDTO> listaCurriculoPeriodo)
                {
                    curso.tipoNivelEnsino = new ACA_TipoNivelEnsinoDTO.Referencia { tne_id = curso.tne_id, tne_idProximo = curso.tne_idProximo };
                    curso.tne_id = null;
                    curso.tne_idProximo = null;
                    curso.tme_id = null;
                    curso.curriculo = curriculo;
                    curriculo.cur_id = null;
                    curso.listaCursoRelacionado = listaCursoRelacionado;
                    curso.listaCurriculoPeriodo = listaCurriculoPeriodo;
                    return curso;
                };

            Func<ACA_CursoRelacionadoDTO, ACA_CursoRelacionadoDTO> retornaCursoRelacionado =
                delegate (ACA_CursoRelacionadoDTO cursoRelacionado)
                {
                    cursoRelacionado.cur_id = null;
                    cursoRelacionado.crr_id = null;
                    return cursoRelacionado;
                };

            Func<ACA_CurriculoPeriodoDTO, List<ACA_CurriculoDisciplinaDTO>, List<ACA_CurriculoDisciplinaEletivaDTO>, ACA_CurriculoPeriodoDTO> retornaCurriculoPeriodo =
                delegate (ACA_CurriculoPeriodoDTO curriculoPeriodo, List<ACA_CurriculoDisciplinaDTO> listaCurriculoDisciplina, List<ACA_CurriculoDisciplinaEletivaDTO> listaCurriculoDisciplinaEletiva)
                {
                    curriculoPeriodo.cur_id = null;
                    curriculoPeriodo.crr_id = null;
                    curriculoPeriodo.tipoCiclo = new ACA_TipoCicloDTO.Referencia { tci_id = curriculoPeriodo.tci_id };
                    curriculoPeriodo.tipoCurriculoPeriodo = new ACA_TipoCurriculoPeriodoDTO.Referencia { tcp_id = curriculoPeriodo.tcp_id };
                    curriculoPeriodo.mep_id = null;
                    curriculoPeriodo.tci_id = null;
                    curriculoPeriodo.tcp_id = null;
                    curriculoPeriodo.listaCurriculoDisciplina = listaCurriculoDisciplina;
                    curriculoPeriodo.listaCurriculoDisciplinaEletiva = listaCurriculoDisciplinaEletiva;
                    return curriculoPeriodo;
                };

            Func<ACA_CurriculoDisciplinaDTO, ACA_DisciplinaDTO, ACA_CurriculoDisciplinaDTO> retornaCurriculoDisciplina =
                delegate (ACA_CurriculoDisciplinaDTO curriculoDisciplina, ACA_DisciplinaDTO disciplina)
                {
                    curriculoDisciplina.cur_id = null;
                    curriculoDisciplina.crr_id = null;
                    curriculoDisciplina.crp_id = null;
                    curriculoDisciplina.dis_id = null;
                    disciplina.etapa = new ACA_TipoDisciplinaDTO.Referencia { tds_id = disciplina.tds_id };
                    disciplina.tds_id = null;
                    curriculoDisciplina.disciplina = disciplina;
                    return curriculoDisciplina;
                };

            Func<ACA_CurriculoDisciplinaEletivaDTO, ACA_CurriculoDisciplinaEletivaDTO> retornaCurriculoDisciplinaEletiva =
                delegate (ACA_CurriculoDisciplinaEletivaDTO curriculoDisciplinaEletiva)
                {
                    curriculoDisciplinaEletiva.cur_id = null;
                    curriculoDisciplinaEletiva.crr_id = null;
                    curriculoDisciplinaEletiva.crp_id = null;
                    curriculoDisciplinaEletiva.dis_id = null;
                    return curriculoDisciplinaEletiva;
                };

            #endregion Delegate

            List<ACA_CursoDTO> listaCurso = (from dr in dtDados.AsEnumerable()
                                             group dr by Convert.ToInt32(dr["cur_id"]) into g
                                             select retornaUsuario
                                               (
                                                   (ACA_CursoDTO)GestaoEscolarUtilBO.DataRowToEntity(g.FirstOrDefault(), new ACA_CursoDTO())
                                      ,
                                                   (ACA_CurriculoDTO)GestaoEscolarUtilBO.DataRowToEntity(g.FirstOrDefault(), new ACA_CurriculoDTO())
                                                   ,
                                                   (from dr in g.AsEnumerable()
                                                    where dr["crr_id"] != DBNull.Value
                                                    group dr by Convert.ToInt32(dr["crr_id"]) into g2
                                                    orderby Convert.ToInt32(g2.FirstOrDefault()["crr_id"])
                                                    select retornaCursoRelacionado
                                                    (
                                                      (ACA_CursoRelacionadoDTO)GestaoEscolarUtilBO.DataRowToEntity(g2.FirstOrDefault(), new ACA_CursoRelacionadoDTO())
                                                    )).ToList()
                                      ,
                                                   (from dr in g.AsEnumerable()
                                                    where dr["crp_id"] != DBNull.Value
                                                    group dr by Convert.ToInt32(dr["crp_id"]) into g2
                                                    orderby Convert.ToInt32(g2.FirstOrDefault()["crp_id"])
                                                    select retornaCurriculoPeriodo
                                                    (
                                                      (ACA_CurriculoPeriodoDTO)GestaoEscolarUtilBO.DataRowToEntity(g2.FirstOrDefault(), new ACA_CurriculoPeriodoDTO())
                                      ,
                                                       (from dr in g2.AsEnumerable()
                                                        where dr["dis_id"] != DBNull.Value
                                                        group dr by Convert.ToInt32(dr["dis_id"]) into g3
                                                        orderby Convert.ToInt32(g3.FirstOrDefault()["dis_id"])
                                                        select retornaCurriculoDisciplina
                                                        (
                                                           (ACA_CurriculoDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(g3.FirstOrDefault(), new ACA_CurriculoDisciplinaDTO())
                                                           ,
                                                           (ACA_DisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(g3.FirstOrDefault(), new ACA_DisciplinaDTO())
                                                        )).ToList()
                                                       ,
                                                       (from dr in g2.AsEnumerable()
                                                        where dr["dis_id"] != DBNull.Value
                                                        group dr by Convert.ToInt32(dr["dis_id"]) into g3
                                                        orderby Convert.ToInt32(g3.FirstOrDefault()["dis_id"])
                                                        select retornaCurriculoDisciplinaEletiva
                                                        (
                                                           (ACA_CurriculoDisciplinaEletivaDTO)GestaoEscolarUtilBO.DataRowToEntity(g3.FirstOrDefault(), new ACA_CurriculoDisciplinaEletivaDTO())
                                                        )).ToList()
                                                    )).ToList()
                                               )).ToList();

            return listaCurso;
        }

        /// <summary>
        /// Salva os dados informados no arquivo.
        /// </summary>
        /// <param name="Json">Conteúdo Json enviado no post</param>
        /// <returns>List<ACA_CursoDTO></returns>
        public static List<ACA_CursoDTO> SalvarCurso(string Json)
        {
            List<ACA_CursoDTO> lista = new List<ACA_CursoDTO>();
            TalkDBTransaction bancoGestao = new ACA_CursoDAO()._Banco;
            TalkDBTransaction bancoCore = new SYS_EntidadeDAO()._Banco;

            try
            {
                JArray listaDados = (JArray.Parse(Json) ?? new JArray());

                //List<ACA_CursoDTO> dados = (from item in listaDados.AsEnumerable()
                //                            select new ACA_CursoDTO()
                //                                      {
                //                                          ACA_Curso = (Curso)JsonConvert.DeserializeObject<Curso>(item.SelectToken("ACA_Curso").ToString())
                //                                          ,

                //                                          ACA_Curriculo = (Curriculo)JsonConvert.DeserializeObject<Curriculo>(item.SelectToken("ACA_Curriculo").ToString())

                //                                          //List_ACA_Curriculo = (from item2 in ((JArray)item.SelectToken("List_ACA_Curriculo") ?? new JArray()).AsEnumerable()
                //                                          //                      select new ACA_CurriculoDTO()
                //                                          //                            {
                //                                          //                                ACA_Curriculo = (Curriculo)JsonConvert.DeserializeObject<Curriculo>(item2.SelectToken("ACA_Curriculo").ToString())
                //                                          //                            }).ToList()
                //                                          ,
                //                                          List_ACA_CurriculoPeriodo = (from item2 in ((JArray)item.SelectToken("List_ACA_CurriculoPeriodo") ?? new JArray()).AsEnumerable()
                //                                                                       select new ACA_CurriculoPeriodoDTO()
                //                                                                      {
                //                                                                          ACA_CurriculoPeriodo = (CurriculoPeriodo)JsonConvert.DeserializeObject<CurriculoPeriodo>(item2.SelectToken("ACA_CurriculoPeriodo").ToString())
                //                                                                      }).ToList()
                //                                          ,
                //                                          List_ACA_CurriculoDisciplina = (from item2 in ((JArray)item.SelectToken("List_ACA_CurriculoDisciplina") ?? new JArray()).AsEnumerable()
                //                                                                          select new ACA_CurriculoDisciplinaDTO()
                //                                                                      {
                //                                                                          ACA_CurriculoDisciplina = (CurriculoDisciplina)JsonConvert.DeserializeObject<CurriculoDisciplina>(item2.SelectToken("ACA_CurriculoDisciplina").ToString())
                //                                                                      }).ToList()
                //                                      }).ToList();

                bancoGestao.Open();
                bancoCore.Open();

                DataTable dtCurriculoPeriodo = new DataTable();

                //foreach (ACA_CursoDTO item in dados)
                //{
                //    ACA_Curso cur = (ACA_Curso)GestaoEscolarUtilBO.Clone(item.ACA_Curso, new ACA_Curso());

                //    cur.IsNew = (cur.cur_id <= 0);
                //    cur.cur_id = (cur.cur_id <= 0) ? -1 : cur.cur_id;

                //    //List<ACA_Curriculo> lstCrr = (from dr in item.List_ACA_Curriculo.AsEnumerable()
                //    //                                 select (ACA_Curriculo)GestaoEscolarUtilBO.Clone(dr.ACA_Curriculo, new ACA_Curriculo())
                //    //                   ).ToList();

                //    ACA_Curriculo crr = (ACA_Curriculo)GestaoEscolarUtilBO.Clone(item.ACA_Curriculo, new ACA_Curriculo());

                //    List<ACA_CurriculoPeriodo> lstCrp = (from dr in item.List_ACA_CurriculoPeriodo.AsEnumerable()
                //                                         select (ACA_CurriculoPeriodo)GestaoEscolarUtilBO.Clone(dr.ACA_CurriculoPeriodo, new ACA_CurriculoPeriodo())
                //   ).ToList();

                //    dtCurriculoPeriodo.Clear();
                //    dtCurriculoPeriodo = GestaoEscolarUtilBO.EntityToDataTable<ACA_CurriculoPeriodo>(lstCrp);

                //    //ACA_CursoBO.Save(cur
                //    //                , lstCrr[0]
                //    //                , dtCurriculoPeriodo
                //    //                , _VS_DisciplinasPeriodo
                //    //                , _VS_DisciplinasEletivas
                //    //                , VS_List_ACA_CurriculoDisciplina_Cadastro
                //    //                , CheckListToUpdate()
                //    //                , cur.ent_id
                //    //                , bancoGestao
                //    //                , bancoCore);

                //    #region Atualiza dados da lista para Json de retorno

                //    item.ACA_Curso.cur_id = cur.cur_id;

                //    lista.Add(item);

                //    #endregion Atualiza dados da lista para Json de retorno
                //}

            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                bancoCore.Close(ex);
                throw;
            }
            finally
            {
                bancoGestao.Close();
                bancoCore.Close();
            }

            return lista;
        }

        #endregion ACA_Curso

        #region ACA_CalendarioAnual

        /// <summary>
        /// Seleciona todos os calendários anuais ativos.
        /// </summary>
        /// <returns>List<ACA_CalendarioAnualDTO></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_CalendarioAnualDTO> SelecionarCalendariosAnuaisAtivos(int cal_ano)
        {
            #region Delegate

            Func<ACA_CalendarioAnualDTO
                 , IEnumerable<ACA_CalendarioCursoDTO.Referencia>
                 , IEnumerable<ACA_CalendarioPeriodoDTO>
                 , ACA_CalendarioAnualDTO> retornarCalendarioAnual =
                 delegate (ACA_CalendarioAnualDTO calendarioAnual
                          , IEnumerable<ACA_CalendarioCursoDTO.Referencia> listaCalendarioCurso
                          , IEnumerable<ACA_CalendarioPeriodoDTO> listaCalendarioPeriodo)
                 {
                     calendarioAnual.listaCalendarioCurso = listaCalendarioCurso.ToList();
                     calendarioAnual.listaCalendarioPeriodo = listaCalendarioPeriodo.ToList();
                     return calendarioAnual;
                 };

            #endregion Delegate

            List<ACA_CalendarioAnualDTO> lista = new List<ACA_CalendarioAnualDTO>();

            try
            {
                DataTable dtDados = new ApiDAO().SelecionaCalendarioAnualPorAno(cal_ano);

                lista = (from dr in dtDados.AsEnumerable()
                         group dr by Convert.ToInt32(dr["cal_id"]) into g
                         select retornarCalendarioAnual(
                             (ACA_CalendarioAnualDTO)GestaoEscolarUtilBO.DataRowToEntity(g.FirstOrDefault(), new ACA_CalendarioAnualDTO())
                             ,
                             (from dr in g.AsEnumerable()
                              where dr["cur_id"] != DBNull.Value
                              group dr by Convert.ToInt32(dr["cur_id"]) into g2
                              orderby Convert.ToInt32(g2.FirstOrDefault()["cur_id"])
                              select (ACA_CalendarioCursoDTO.Referencia)GestaoEscolarUtilBO.DataRowToEntity(g2.FirstOrDefault(), new ACA_CalendarioCursoDTO.Referencia()))
                             ,
                             (from dr in g.AsEnumerable()
                              where dr["cap_id"] != DBNull.Value
                              group dr by Convert.ToInt32(dr["cap_id"]) into g2
                              orderby Convert.ToInt32(g2.FirstOrDefault()["cap_id"])
                              select (ACA_CalendarioPeriodoDTO)GestaoEscolarUtilBO.DataRowToEntity(g2.FirstOrDefault(), new ACA_CalendarioPeriodoDTO())))
                         ).ToList();
            }
            catch
            {
                throw;
            }

            return lista;
        }

        /// <summary>
        /// Seleciona os dados do calendario anual por esc_id.
        /// </summary>
        /// <param name="esc_id">Id do Calendario anual</param>
        /// <returns>ACA_CalendarioAnualDTO</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ACA_CalendarioAnualDTO SelecionarCalendarioAnualPorEscId(long esc_id)
        {
            ACA_CalendarioAnualDTO cal = null;

            try
            {
                DataTable dtDados = new ApiDAO().SelecionarCalendarioAnualPorEscId(esc_id);

                cal = ApiBO.ConvertDataTableToACA_CalendarioAnualDTO(dtDados);
            }
            catch
            {
                throw;
            }

            return cal;
        }

        /// <summary>
        /// Seleciona os dados do calendario anual por id.
        /// </summary>
        /// <param name="cal_id">Id do Calendario anual</param>
        /// <returns>ACA_CalendarioAnualDTO</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ACA_CalendarioAnualDTO SelecionarCalendarioAnualPorId(int cal_id, DateTime dataBase)
        {
            ACA_CalendarioAnualDTO cal = null;

            try
            {
                DataTable dtDados = new ApiDAO().SelecionarCalendarioAnualPorId(cal_id, dataBase);

                cal = ApiBO.ConvertDataTableToACA_CalendarioAnualDTO(dtDados);
            }
            catch
            {
                throw;
            }

            return cal;
        }

        private static ACA_CalendarioAnualDTO ConvertDataTableToACA_CalendarioAnualDTO(DataTable dtDados)
        {
            #region Delegate

            Func<ACA_CalendarioAnualDTO
                 , IEnumerable<ACA_CalendarioCursoDTO.Referencia>
                 , IEnumerable<ACA_CalendarioPeriodoDTO>
                 , ACA_CalendarioAnualDTO> retornarCalendarioAnual =
                 delegate (ACA_CalendarioAnualDTO calendarioAnual
                          , IEnumerable<ACA_CalendarioCursoDTO.Referencia> listaCalendarioCurso
                          , IEnumerable<ACA_CalendarioPeriodoDTO> listaCalendarioPeriodo)
                 {
                     calendarioAnual.listaCalendarioCurso = listaCalendarioCurso.ToList();
                     calendarioAnual.listaCalendarioPeriodo = listaCalendarioPeriodo.ToList();
                     return calendarioAnual;
                 };

            #endregion Delegate

            ACA_CalendarioAnualDTO cal = null;

            cal = (from dr in dtDados.AsEnumerable()
                   group dr by Convert.ToInt32(dr["cal_id"]) into g
                   select retornarCalendarioAnual(
                       (ACA_CalendarioAnualDTO)GestaoEscolarUtilBO.DataRowToEntity(g.FirstOrDefault(), new ACA_CalendarioAnualDTO())
                       ,
                       (from dr in g.AsEnumerable()
                        where dr["cur_id"] != DBNull.Value
                        group dr by Convert.ToInt32(dr["cur_id"]) into g2
                        orderby Convert.ToInt32(g2.FirstOrDefault()["cur_id"])
                        select (ACA_CalendarioCursoDTO.Referencia)GestaoEscolarUtilBO.DataRowToEntity(g2.FirstOrDefault(), new ACA_CalendarioCursoDTO.Referencia()))
                       ,
                       (from dr in g.AsEnumerable()
                        where dr["cap_id"] != DBNull.Value
                        group dr by Convert.ToInt32(dr["cap_id"]) into g2
                        orderby Convert.ToInt32(g2.FirstOrDefault()["cap_id"])
                        select (ACA_CalendarioPeriodoDTO)GestaoEscolarUtilBO.DataRowToEntity(g2.FirstOrDefault(), new ACA_CalendarioPeriodoDTO())))
                   ).FirstOrDefault();

            return cal;
        }

        /// <summary>
        /// Salva os dados informados no arquivo.
        /// </summary>
        /// <param name="Json">Conteúdo Json enviado no post</param>
        /// <returns>List<ACA_CalendarioAnualDTO></returns>
        public static List<ACA_CalendarioAnualDTO> SalvarCalendarioAnual(string Json)
        {
            #region Delegate

            Func<ACA_CalendarioAnualDTO
                 , IEnumerable<ACA_CalendarioCursoDTO.Referencia>
                 , IEnumerable<ACA_CalendarioPeriodoDTO>
                 , ACA_CalendarioAnualDTO> retornarCalendarioAnual =
                 delegate (ACA_CalendarioAnualDTO calendarioAnual
                          , IEnumerable<ACA_CalendarioCursoDTO.Referencia> listaCalendarioCurso
                          , IEnumerable<ACA_CalendarioPeriodoDTO> listaCalendarioPeriodo)
                 {
                     calendarioAnual.listaCalendarioCurso = listaCalendarioCurso.ToList();
                     calendarioAnual.listaCalendarioPeriodo = listaCalendarioPeriodo.ToList();
                     return calendarioAnual;
                 };

            #endregion Delegate

            List<ACA_CalendarioAnualDTO> lista = new List<ACA_CalendarioAnualDTO>();
            TalkDBTransaction bancoGestao = new ACA_CalendarioAnualDAO()._Banco;
            TalkDBTransaction bancoCore = new SYS_EntidadeDAO()._Banco;

            try
            {
                JArray listaDados = (JArray.Parse(Json) ?? new JArray());

                lista = (from item in listaDados.AsEnumerable()
                         select retornarCalendarioAnual(
                             (ACA_CalendarioAnualDTO)JsonConvert.DeserializeObject<ACA_CalendarioAnualDTO>(item.ToString())
                             ,
                             (from item2 in ((JArray)item.SelectToken("listaCalendarioCurso") ?? new JArray()).AsEnumerable()
                              select (ACA_CalendarioCursoDTO.Referencia)JsonConvert.DeserializeObject<ACA_CalendarioCursoDTO.Referencia>(item2.ToString()))
                             ,
                             (from item3 in ((JArray)item.SelectToken("listaCalendarioPeriodo") ?? new JArray()).AsEnumerable()
                              select (ACA_CalendarioPeriodoDTO)JsonConvert.DeserializeObject<ACA_CalendarioPeriodoDTO>(item3.ToString())))).ToList();

                bancoGestao.Open();
                bancoCore.Open();

                DataTable dtCalendarioPeriodo = new DataTable();

                foreach (ACA_CalendarioAnualDTO item in lista)
                {
                    ACA_CalendarioAnual cal = (ACA_CalendarioAnual)GestaoEscolarUtilBO.Clone(item, new ACA_CalendarioAnual());

                    cal.IsNew = (cal.cal_id <= 0);
                    cal.cal_id = (cal.cal_id <= 0) ? -1 : cal.cal_id;

                    List<ACA_CalendarioCurso> lstCalCur = (from dr in item.listaCalendarioCurso.AsEnumerable()
                                                           select (ACA_CalendarioCurso)GestaoEscolarUtilBO.Clone(dr, new ACA_CalendarioCurso())
                                                           ).ToList();

                    List<ACA_CalendarioPeriodo> lstCap = (from dr in item.listaCalendarioPeriodo.AsEnumerable()
                                                          select (ACA_CalendarioPeriodo)GestaoEscolarUtilBO.Clone(dr, new ACA_CalendarioPeriodo())
                                                          ).ToList();

                    dtCalendarioPeriodo.Clear();
                    dtCalendarioPeriodo = GestaoEscolarUtilBO.EntityToDataTable<ACA_CalendarioPeriodo>(lstCap);

                    ACA_CalendarioAnualBO.Save(cal
                                               , dtCalendarioPeriodo
                                               , lstCalCur
                                               , cal.ent_id
                                               , bancoGestao
                                               , bancoCore);

                    #region Atualiza dados da lista para Json de retorno

                    item.cal_id = cal.cal_id;
                    item.listaCalendarioCurso.ForEach(p => p.cal_id = cal.cal_id);
                    item.listaCalendarioPeriodo.ForEach(p => p.cal_id = cal.cal_id);

                    #endregion Atualiza dados da lista para Json de retorno
                }
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                bancoCore.Close(ex);
                throw;
            }
            finally
            {
                bancoGestao.Close();
                bancoCore.Close();
            }

            return lista;
        }

        #endregion ACA_CalendarioAnual

        #region ACA_CurriculoPeriodo

        /// <summary>
        /// Retorna uma lista de curriculos do periodo por escola. Se passado a dataBase
        /// irá retornar apenas os registros criados ou alterados após esta data, caso contrario
        /// apenas os registros ativos serão retornados.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns></returns>
        public static List<ACA_CurriculoPeriodoDTO> SelecionarCurriculosPorEscola(int esc_id, DateTime dataBase)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.SelecionarCurriculosPorEscola(esc_id, dataBase);

                if (dt.Rows.Count == 0)
                    return null;

                List<ACA_CurriculoPeriodoDTO> curriculos = (from dr in dt.AsEnumerable()
                                                            select (ACA_CurriculoPeriodoDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_CurriculoPeriodoDTO())
                                                                ).ToList();

                return curriculos;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region ACA_AreaConhecimento

        /// <summary>
        /// Seleciona todas as áreas de conhecimento ativas.
        /// </summary>
        /// <returns>List<ACA_AreaConhecimentoDTO></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_AreaConhecimentoDTO> SelecionarAreasConhecimento()
        {
            List<ACA_AreaConhecimentoDTO> lista = new List<ACA_AreaConhecimentoDTO>();

            try
            {
                ACA_AreaConhecimentoDAO dao = new ACA_AreaConhecimentoDAO();

                lista = (from dr in dao.SelecionaAtivas().AsEnumerable()
                         select (ACA_AreaConhecimentoDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_AreaConhecimentoDTO())).ToList();
            }
            catch
            {
                throw;
            }

            return lista;
        }

        /// <summary>
        /// Retorna a ACA_AreaConhecimento pelo aco_id.
        /// </summary>
        /// <returns>ACA_AreaConhecimentoDTO</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ACA_AreaConhecimentoDTO SelecionarAreaConhecimentoPorId(int aco_id)
        {
            try
            {
                ACA_AreaConhecimento areaConhecimento = new ApiDAO().SelecionarAreaConhecimentoPorId(aco_id);

                return areaConhecimento != null ? (ACA_AreaConhecimentoDTO)GestaoEscolarUtilBO.Clone(areaConhecimento, new ACA_AreaConhecimentoDTO()) : null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Salva os dados informados no arquivo.
        /// </summary>
        /// <param name="dados">Lista ACA_AreaConhecimento(conteúdo JSon enviado no post)</param>
        /// <returns></returns>
        public static List<ACA_AreaConhecimentoDTO> SalvarAreaConhecimento(string Json)
        {
            List<ACA_AreaConhecimentoDTO> lista = new List<ACA_AreaConhecimentoDTO>();
            TalkDBTransaction banco = new ACA_AreaConhecimentoDAO()._Banco;

            try
            {
                JArray listaDados = (JArray.Parse(Json) ?? new JArray());

                lista = (from item in listaDados.AsEnumerable()
                         select (ACA_AreaConhecimentoDTO)JsonConvert.DeserializeObject<ACA_AreaConhecimentoDTO>(item.ToString())).ToList();

                banco.Open();

                lista.ForEach(item => ACA_AreaConhecimentoBO.Save(item, banco));
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }

            return lista;
        }

        #endregion ACA_AreaConhecimento

        #region ACA_FormatoAvaliacao

        /// <summary>
        /// retorna um registro de formato de avaliacao pelo id
        /// </summary>
        /// <param name="fav_id">id do formato de avaliacao</param>
        /// <returns></returns>
        public static ACA_FormatoAvaliacaoDTO BuscarFormatoAvaliacaoPorId(int fav_id)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                ACA_FormatoAvaliacao formatoAvaliacao = dao.BuscarFormatoAvaliacaoPorId(fav_id);

                ACA_FormatoAvaliacaoDTO dto = null;

                if (formatoAvaliacao != null)
                {
                    dto = (ACA_FormatoAvaliacaoDTO)GestaoEscolarUtilBO.Clone(formatoAvaliacao, new ACA_FormatoAvaliacaoDTO());
                }

                return dto;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region ACA_EscalaAvaliacao

        public static ACA_EscalaAvaliacaoDTO BuscarEscalaAvaliacaoPorId(int esa_id)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.BuscarEscalaAvaliacaoPorId(esa_id);

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                List<ACA_EscalaAvaliacaoDTO> dto = (from dr in dt.AsEnumerable()
                                                    group dr by dr["esa_id"] into g
                                                    select (ACA_EscalaAvaliacaoDTO)GestaoEscolarUtilBO.DataRowToEntity(g.First(), new ACA_EscalaAvaliacaoDTO
                                                    {
                                                        escalaAvaliacaoNumerica = (
                                                        from d in g.AsEnumerable()
                                                        where d["ean_situacao"] != DBNull.Value
                                                        select ((ACA_EscalaAvaliacaoNumericaDTO)GestaoEscolarUtilBO.DataRowToEntity(d, new ACA_EscalaAvaliacaoNumericaDTO()))).ToList().FirstOrDefault(),
                                                        listaEscalaAvaliacaoParecer = (from dr in g.AsEnumerable()
                                                                                       where dr["eap_id"] != DBNull.Value
                                                                                       group dr by dr["eap_id"] into p
                                                                                       select (ACA_EscalaAvaliacaoParecerDTO)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new ACA_EscalaAvaliacaoParecerDTO())).ToList()
                                                    })).ToList();

                return dto.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region ACA_TipoTurno

        /// <summary>
        /// Seleciona todos os tipos de turno ativos.
        /// </summary>
        /// <returns>List<ACA_TipoDisciplinaDTO></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_TipoTurnoDTO> SelecionarTiposTurno()
        {

            List<ACA_TipoTurnoDTO> lista = new List<ACA_TipoTurnoDTO>();

            try
            {

                lista = (
                               from dr in ACA_TipoTurnoBO.GetSelect()
                               select (ACA_TipoTurnoDTO)GestaoEscolarUtilBO.Clone(dr, new ACA_TipoTurnoDTO())
                           ).ToList();
            }
            catch
            {
                throw;
            }

            return lista;
        }

        /// <summary>
        /// Retorna a ACA_TipoTurno pelo id.
        /// </summary>
        /// <returns>ACA_TipoTurno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ACA_TipoTurnoDTO SelecionaTiposTurnoPorID(int id)
        {

            ACA_TipoTurnoDTO ttn = new ACA_TipoTurnoDTO();
            ttn.ttn_id = id;

            try
            {

                ttn = (ACA_TipoTurnoDTO)GestaoEscolarUtilBO.Clone(ACA_TipoTurnoBO.GetEntity(ttn), new ACA_TipoTurnoDTO());

            }
            catch
            {
                throw;
            }

            return ttn;
        }


        #endregion

        #region ACA_TipoDisciplina

        /// <summary>
        /// Seleciona todos os tipos de disciplina ativas.
        /// </summary>
        /// <returns>List<ACA_TipoDisciplinaDTO></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_TipoDisciplinaDTO> SelecionarTiposDisciplina(TipoDisciplinaEntradaDTO filtro)
        {
            #region Delegate

            Func<ACA_TipoDisciplinaDTO, IEnumerable<ACA_TipoDisciplinaDeficienciaDTO>, ACA_TipoDisciplinaDTO> retornarTipoDisciplina =
                 delegate (ACA_TipoDisciplinaDTO tipoDisciplina
                          , IEnumerable<ACA_TipoDisciplinaDeficienciaDTO> listaTipoDisciplinaDeficiencia)
                 {
                     tipoDisciplina.tipoNivelEnsino.tne_id = tipoDisciplina.tne_id;
                     tipoDisciplina.areaConhecimento.aco_id = tipoDisciplina.aco_id;
                     tipoDisciplina.tne_id = tipoDisciplina.tne_id;
                     tipoDisciplina.mds_id = tipoDisciplina.mds_id;
                     tipoDisciplina.aco_id = tipoDisciplina.aco_id;
                     tipoDisciplina.listaTiposDisciplinaDeficiencia = listaTipoDisciplinaDeficiencia.ToList();
                     return tipoDisciplina;
                 };

            #endregion Delegate

            List<ACA_TipoDisciplinaDTO> lista = new List<ACA_TipoDisciplinaDTO>();

            try
            {
                DataTable dtDados = new ApiDAO().SelecionarTipoDisciplina(filtro.tne_id, filtro.aco_id);

                lista = (from dr in dtDados.AsEnumerable()
                         group dr by Convert.ToInt32(dr["tds_id"]) into g
                         select retornarTipoDisciplina(
                             (ACA_TipoDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(g.FirstOrDefault(), new ACA_TipoDisciplinaDTO())
                             ,
                             (from dr in g.AsEnumerable()
                              where dr["tde_id"] != DBNull.Value
                              select (ACA_TipoDisciplinaDeficienciaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoDisciplinaDeficienciaDTO())))
                        ).ToList();
            }
            catch
            {
                throw;
            }

            return lista;
        }

        /// <summary>
        /// Retorna a ACA_TipoDisciplina pelo tds_id.
        /// </summary>
        /// <returns>ACA_TipoDisciplina</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ACA_TipoDisciplinaDTO SelecionarTipoDisciplinaPorId(int tds_id)
        {
            #region Delegate

            Func<ACA_TipoDisciplinaDTO, IEnumerable<ACA_TipoDisciplinaDeficienciaDTO>, ACA_TipoDisciplinaDTO> retornarTipoDisciplina =
                 delegate (ACA_TipoDisciplinaDTO tipoDisciplina
                          , IEnumerable<ACA_TipoDisciplinaDeficienciaDTO> listaTipoDisciplinaDeficiencia)
                 {
                     tipoDisciplina.tipoNivelEnsino.tne_id = tipoDisciplina.tne_id;
                     tipoDisciplina.areaConhecimento.aco_id = tipoDisciplina.aco_id;
                     tipoDisciplina.tne_id = tipoDisciplina.tne_id;
                     tipoDisciplina.mds_id = tipoDisciplina.mds_id;
                     tipoDisciplina.aco_id = tipoDisciplina.aco_id;
                     tipoDisciplina.listaTiposDisciplinaDeficiencia = listaTipoDisciplinaDeficiencia.ToList();
                     return tipoDisciplina;
                 };

            #endregion Delegate

            ACA_TipoDisciplinaDTO tds = null;

            try
            {
                DataTable dtDados = new ApiDAO().SelecionarTipoDisciplinaPorId(tds_id);

                tds = (from dr in dtDados.AsEnumerable()
                       group dr by Convert.ToInt32(dr["tds_id"]) into g
                       select retornarTipoDisciplina(
                           (ACA_TipoDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(g.FirstOrDefault(), new ACA_TipoDisciplinaDTO())
                           ,
                           (from dr in g.AsEnumerable()
                            where dr["tde_id"] != DBNull.Value
                            select (ACA_TipoDisciplinaDeficienciaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoDisciplinaDeficienciaDTO())))
                        ).FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return tds;
        }

        /// <summary>
        /// Retorna a ACA_TipoDisciplina pelo tne_id.
        /// </summary>
        /// <returns>ACA_TipoDisciplina</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_TipoDisciplinaDTO> SelecionarTipoDisciplinaPorNivelEnsino(int tne_id)
        {
            #region Delegate

            Func<ACA_TipoDisciplinaDTO, IEnumerable<ACA_TipoDisciplinaDeficienciaDTO>, ACA_TipoDisciplinaDTO> retornarTipoDisciplina =
                 delegate (ACA_TipoDisciplinaDTO tipoDisciplina
                          , IEnumerable<ACA_TipoDisciplinaDeficienciaDTO> listaTipoDisciplinaDeficiencia)
                 {
                     tipoDisciplina.tipoNivelEnsino.tne_id = tipoDisciplina.tne_id;
                     tipoDisciplina.areaConhecimento.aco_id = tipoDisciplina.aco_id;
                     tipoDisciplina.tne_id = tipoDisciplina.tne_id;
                     tipoDisciplina.mds_id = tipoDisciplina.mds_id;
                     tipoDisciplina.aco_id = tipoDisciplina.aco_id;
                     tipoDisciplina.listaTiposDisciplinaDeficiencia = listaTipoDisciplinaDeficiencia.ToList();
                     return tipoDisciplina;
                 };

            #endregion Delegate

            List<ACA_TipoDisciplinaDTO> lstTds = null;

            try
            {
                DataTable dtDados = new ApiDAO().SelecionarTipoDisciplinaPorNivelEnsino(tne_id);

                lstTds = (from dr in dtDados.AsEnumerable()
                          group dr by Convert.ToInt32(dr["tds_id"]) into g
                          select retornarTipoDisciplina(
                              (ACA_TipoDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(g.FirstOrDefault(), new ACA_TipoDisciplinaDTO())
                              ,
                              (from dr in g.AsEnumerable()
                               where dr["tde_id"] != DBNull.Value
                               select (ACA_TipoDisciplinaDeficienciaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoDisciplinaDeficienciaDTO())))
                        ).ToList();
            }
            catch
            {
                throw;
            }

            return lstTds;
        }
        
        #endregion ACA_TipoDisciplina

        #region ACA_TipoModalidadeEnsino

        /// <summary>
        /// Retorna a ACA_TipoModalidadeEnsino ativas.
        /// </summary>
        /// <returns> List<ACA_TipoModalidadeEnsinoDTO></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_TipoModalidadeEnsinoDTO> SelecionarTipoModalidadeEnsinoAtivas()
        {
            #region Delegate

            Func<ACA_TipoModalidadeEnsinoDTO, ACA_TipoModalidadeEnsinoDTO> retornarTipoModalidadeEnsino =
                 delegate (ACA_TipoModalidadeEnsinoDTO tipoModalidadeEnsino)
                 {
                     return tipoModalidadeEnsino;
                 };

            #endregion Delegate

            List<ACA_TipoModalidadeEnsinoDTO> lista = new List<ACA_TipoModalidadeEnsinoDTO>();

            try
            {
                DataTable dtDados = new ApiDAO().SelecionarTipoModalidadeEnsinoAtivas();

                lista = (from dr in dtDados.AsEnumerable()
                         group dr by Convert.ToInt32(dr["tme_id"]) into g
                         select retornarTipoModalidadeEnsino(
                             (ACA_TipoModalidadeEnsinoDTO)GestaoEscolarUtilBO.DataRowToEntity(g.FirstOrDefault(), new ACA_TipoModalidadeEnsinoDTO()))
                        ).ToList();
            }
            catch
            {
                throw;
            }

            return lista;
        }



        #endregion ACA_TipoModalidadeEnsino

        #region ACA_TipoNivelEnsino

        /// <summary>
        /// Retorna a ACA_TipoNivelEnsino pelo tne_id.
        /// </summary>
        /// <returns> List<ACA_TipoNivelEnsinoDTO></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_TipoNivelEnsinoDTO> SelecionarTipoNivelEnsino(int tne_id)
        {
            #region Delegate

            Func<ACA_TipoNivelEnsinoDTO, ACA_TipoNivelEnsinoDTO> retornarTipoNivelEnsino =
                 delegate (ACA_TipoNivelEnsinoDTO tipoNivelEnsino)
                 {
                     return tipoNivelEnsino;
                 };

            #endregion Delegate

            List<ACA_TipoNivelEnsinoDTO> lista = new List<ACA_TipoNivelEnsinoDTO>();

            try
            {
                DataTable dtDados = new ApiDAO().SelecionarTipoNivelEnsinoPorId(tne_id);

                lista = (from dr in dtDados.AsEnumerable()
                         group dr by Convert.ToInt32(dr["tne_id"]) into g
                         select retornarTipoNivelEnsino(
                             (ACA_TipoNivelEnsinoDTO)GestaoEscolarUtilBO.DataRowToEntity(g.FirstOrDefault(), new ACA_TipoNivelEnsinoDTO()))
                        ).ToList();
            }
            catch
            {
                throw;
            }

            return lista;
        }



        #endregion ACA_TipoNivelEnsino

        #region ESC_Escola

        /// <summary>
        /// Retorna uma escola pelo id
        /// </summary>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        public static ESC_EscolaDTO SelecionarEscolaPorId(int esc_id)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                ESC_Escola escola = dao.SelecionarEscolaPorId(esc_id);
                ESC_EscolaDTO dto = null;

                if (escola != null)
                {
                    dto = (ESC_EscolaDTO)GestaoEscolarUtilBO.Clone(escola, new ESC_EscolaDTO());
                }

                return dto;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna as escolas pela entidade, qdo informado a data base o retorno é 
        /// apenas de escolas criadas ou alteradas apos esta data... caso contrario 
        /// apenas escolas ativas serão retornadas.
        /// </summary>
        /// <param name="ent_id">id da entidade</param>
        /// <param name="dataBase">data base para a seleção.</param>
        /// <returns></returns>
        public static List<ESC_EscolaDTO> SelecionarEscolasPorEntidade(Int32 esc_id, string esc_codigo, Guid ent_id, DateTime dataBase)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.SelecionarEscolasPorEntidade(esc_id, esc_codigo, ent_id, dataBase);

                if (dt.Rows.Count == 0)
                    return null;

                List<ESC_EscolaDTO> escolas = (from dr in dt.AsEnumerable()
                                               group dr by dr["esc_id"] into esc
                                               select (ESC_EscolaDTO)GestaoEscolarUtilBO.DataRowToEntity(esc.First(), new ESC_EscolaDTO
                                               {
                                                   listaCurriculoPeriodo = (from cur in esc
                                                                            where cur["cur_id"] != DBNull.Value
                                                                            group cur by new
                                                                            {
                                                                                cur_id = cur["cur_id"],
                                                                                crr_id = cur["crr_id"],
                                                                                crp_id = cur["crp_id"]
                                                                            } into crp
                                                                            select (ACA_CurriculoPeriodoDTO)GestaoEscolarUtilBO.DataRowToEntity(crp.First(), new ACA_CurriculoPeriodoDTO())).ToList()
                                               })
                                              ).ToList();

                return escolas;
            }
            catch
            {
                throw;
            }
        }


        #endregion ESC_Escola

        #region MTR_Movimentacao

        /// <summary>
        /// Seleciona todas as movimentações posterior a data informada.
        /// </summary>
        /// <param name="dataAlteracao">Data de alteração da movimentacao</param>
        /// <returns>List<Movimentacao></returns>
        public static List<Movimentacao> SelecionarMovimentacoesPorDataAlteracao(DateTime dataAlteracao)
        {
            List<Movimentacao> mov = new List<Movimentacao>();

            try
            {
                DataTable dtDados = new ApiDAO().SelecionarMovimentacoesPorDataAlteracao(dataAlteracao);

                mov = (from dr in dtDados.AsEnumerable()
                       select (Movimentacao)GestaoEscolarUtilBO.DataRowToEntity(dr, new Movimentacao())
                       ).ToList();
            }
            catch
            {
                throw;
            }

            return mov;
        }

        /// <summary>
        /// Seleciona todas as movimentações posterior a data informada.
        /// </summary>
        /// <param name="dataAlteracao">Data de alteração da movimentacao</param>
        /// <returns>List<Movimentacao></returns>
        public static List<MTR_MovimentacaoDTO> SelecionarMovimentacoesDTOPorDataAlteracao(DateTime mov_dataAlteracao)
        {
            #region Delegate

            Func<MTR_MovimentacaoDTO, MTR_MovimentacaoDTO> retornarMovimentacao =
                    delegate (MTR_MovimentacaoDTO mov)
                    {
                        mov.aluno.alu_id = mov.alu_id.Value;
                        mov.matriculaTurmaAnterior.mtu_id = mov.mtu_idAnterior;
                        mov.matriculaTurmaAtual.mtu_id = mov.mtu_idAtual;
                        mov.tipoMovimentacaoEntrada.tmv_id = mov.tmv_idEntrada;
                        mov.tipoMovimentacaoSaida.tmv_id = mov.tmv_idSaida;
                        mov.alunoCurriculoAnterior.alc_id = mov.alc_idAnterior;
                        mov.alunoCurriculoAtual.alc_id = mov.alc_idAtual;
                        mov.tipoMovimentacao.tmo_id = mov.tmo_id;
                        mov.mtu_idAnterior = null;
                        mov.mtu_idAtual = null;
                        mov.tmv_idEntrada = null;
                        mov.tmv_idSaida = null;
                        mov.alc_idAnterior = null;
                        mov.alc_idAtual = null;
                        mov.tmo_id = null;
                        return mov;
                    };

            #endregion Delegate

            List<MTR_MovimentacaoDTO> listMov = new List<MTR_MovimentacaoDTO>();

            try
            {
                DataTable dtDados = new ApiDAO().SelecionarMovimentacoesPorDataAlteracao(mov_dataAlteracao);

                listMov = (from dr in dtDados.AsEnumerable()
                           select retornarMovimentacao(
                               (MTR_MovimentacaoDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new MTR_MovimentacaoDTO())
                           )).ToList();
            }
            catch
            {
                throw;
            }

            return listMov;
        }

        /// <summary>
        /// Seleciona a movimentacao pelo alu_id e mov_id.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mov_id">Id da movimentacao.</param>
        /// <returns>MTR_MovimentacaoDTO</returns>
        public static MTR_MovimentacaoDTO SelecionarMovimentacaoPorid(long alu_id, int mov_id)
        {
            #region Delegate

            Func<MTR_MovimentacaoDTO, MTR_MovimentacaoDTO> retornarMovimentacao =
                    delegate (MTR_MovimentacaoDTO mov)
                    {
                        mov.aluno.alu_id = mov.alu_id.Value;
                        mov.matriculaTurmaAnterior.mtu_id = mov.mtu_idAnterior;
                        mov.matriculaTurmaAtual.mtu_id = mov.mtu_idAtual;
                        mov.tipoMovimentacaoEntrada.tmv_id = mov.tmv_idEntrada;
                        mov.tipoMovimentacaoSaida.tmv_id = mov.tmv_idSaida;
                        mov.alunoCurriculoAnterior.alc_id = mov.alc_idAnterior;
                        mov.alunoCurriculoAtual.alc_id = mov.alc_idAtual;
                        mov.tipoMovimentacao.tmo_id = mov.tmo_id;
                        mov.mtu_idAnterior = null;
                        mov.mtu_idAtual = null;
                        mov.tmv_idEntrada = null;
                        mov.tmv_idSaida = null;
                        mov.alc_idAnterior = null;
                        mov.alc_idAtual = null;
                        mov.tmo_id = null;
                        return mov;
                    };

            #endregion Delegate

            try
            {
                MTR_Movimentacao movimentacao = new ApiDAO().SelecionarMovimentacaoPorId(alu_id, mov_id);

                MTR_MovimentacaoDTO mov = null;

                if (movimentacao != null)
                {
                    #region Populando mov

                    mov = (MTR_MovimentacaoDTO)GestaoEscolarUtilBO.Clone(movimentacao, new MTR_MovimentacaoDTO());
                    mov.aluno.alu_id = movimentacao.alu_id;
                    mov.matriculaTurmaAnterior.mtu_id = movimentacao.mtu_idAnterior;
                    mov.matriculaTurmaAtual.mtu_id = movimentacao.mtu_idAtual;
                    mov.tipoMovimentacaoEntrada.tmv_id = movimentacao.tmv_idEntrada;
                    mov.tipoMovimentacaoSaida.tmv_id = movimentacao.tmv_idSaida;
                    mov.alunoCurriculoAnterior.alc_id = movimentacao.alc_idAnterior;
                    mov.alunoCurriculoAtual.alc_id = movimentacao.alc_idAtual;
                    mov.tipoMovimentacao.tmo_id = movimentacao.tmo_id;
                    mov.mtu_idAnterior = null;
                    mov.mtu_idAtual = null;
                    mov.tmv_idEntrada = null;
                    mov.tmv_idSaida = null;
                    mov.alc_idAnterior = null;
                    mov.alc_idAtual = null;
                    mov.tmo_id = null;

                    #endregion Populando mov
                }

                return mov;
            }
            catch
            {
                throw;
            }
        }

        #endregion MTR_Movimentacao

        #region TUR_Turma

        ///// <summary>
        ///// Retorna uma turma pelo id
        ///// </summary>
        ///// <param name="tur_id"></param>
        ///// <returns></returns>
        //public static TUR_TurmaDTO SelecionarTurmaPorId(Int64 tur_id)
        //{
        //    try
        //    {
        //        ApiDAO dao = new ApiDAO();
        //        TUR_Turma turma = dao.SelecionarTurmaPorId(tur_id);

        //        TUR_TurmaDTO dto = null;

        //        if (turma != null)
        //        {
        //            dto = (TUR_TurmaDTO)GestaoEscolarUtilBO.Clone(turma, new TUR_TurmaDTO());

        //            // Delegate que retorna a DTO de matricula.
        //            Func<MTR_MatriculaTurmaDTO, MTR_MatriculaTurmaDTO> matriculaReferenciaAluno =
        //                delegate(MTR_MatriculaTurmaDTO matricula)
        //                {
        //                    matricula.aluno = new ACA_AlunoDTO.Referencia { alu_id = matricula.alu_id.Value };
        //                    matricula.alu_id = null;
        //                    return matricula;
        //                };

        //            dto.listaMatriculaTurma = (from DataRow dr in MTR_MatriculaTurmaBO.BuscaAlunosPorTurma(tur_id).Rows
        //                                       //select (MTR_MatriculaTurmaDTO)GestaoEscolarUtilBO.Clone(mtr, new MTR_MatriculaTurmaDTO())).ToList();
        //                                       select matriculaReferenciaAluno
        //                                (
        //                                    (MTR_MatriculaTurmaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new MTR_MatriculaTurmaDTO())

        //                                )).ToList();

        //            // Delegate que retorna a DTO de TurmaDisciplina.
        //            Func<TUR_TurmaDisciplinaDTO, TUR_TurmaDisciplinaDTO> turmaDisciplina =
        //                delegate(TUR_TurmaDisciplinaDTO tud)
        //                {
        //                    tud.tur_id = null;
        //                    return tud;
        //                };

        //            dto.listaTurmaDisciplina = (from TUR_TurmaDisciplina tud in TUR_TurmaDisciplinaBO.GetSelectBy_Turma(tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo)
        //                                        select turmaDisciplina
        //                                (
        //                                    (TUR_TurmaDisciplinaDTO)GestaoEscolarUtilBO.Clone(tud, new TUR_TurmaDisciplinaDTO())

        //                                )).ToList();

        //            dto.escola = new ESC_EscolaDTO.Referencia { esc_id = turma.esc_id };
        //        }
        //        return dto;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Retorna uma turma pelo id da  escola
        /// </summary>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        public static List<TUR_TurmaDTO> SelecionarTurmasAPI(Int64 tur_id, Int32 esc_id, Int64 doc_id, Int64 tud_id, DateTime dataBase)
        {
            try
            {
                ApiDAO dao = new ApiDAO();

                DataTable dt = TUR_TurmaBO.BuscaTurmasAPI(tur_id, esc_id, doc_id, tud_id, dataBase);

                // Delegate que retorna a DTO de matricula.
                Func<MTR_MatriculaTurmaDTO, MTR_MatriculaTurmaDTO> matriculaReferenciaAluno =
                    delegate (MTR_MatriculaTurmaDTO matricula)
                    {
                        //matricula.aluno = new ACA_AlunoDTO.ReferenciaPesUsuario { alu_id = matricula.alu_id.Value };
                        matricula.alu_id = null;
                        matricula.turma = new TUR_TurmaDTO.Referencia { tur_id = matricula.tur_id.Value };
                        matricula.tur_id = null;
                        return matricula;
                    };

                List<TUR_TurmaDTO> turmas = (from DataRow dr in dt.Rows
                                             group dr by dr["tur_id"] into tur
                                             select (TUR_TurmaDTO)GestaoEscolarUtilBO.DataRowToEntity(tur.First(), new TUR_TurmaDTO
                                             {
                                                 escola = (ESC_EscolaDTO.Referencia)GestaoEscolarUtilBO.DataRowToEntity(tur.First(), new ESC_EscolaDTO.Referencia()),
                                                 listaTurmaDisciplina = (from dis in tur
                                                                         group dis by dis["tud_id"] into tud
                                                                         select (TUR_TurmaDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(tud.First(), new TUR_TurmaDisciplinaDTO())
                                                                        ).ToList(),
                                                 listaMatriculaTurma =
                                                 (from alu in tur
                                                  where alu["alu_id"] != DBNull.Value
                                                  group alu by new
                                                  {
                                                      alu_id = alu["alu_id"],
                                                      mtu_id = alu["mtu_id"]
                                                  } into mtu
                                                  orderby (string.IsNullOrEmpty(mtu.First()["mtu_numeroChamada"].ToString()) ||
                                                           mtu.First()["mtu_numeroChamada"].Equals("-1") ? "1000" :
                                                           mtu.First()["mtu_numeroChamada"])
                                                  select matriculaReferenciaAluno
                                                     ((MTR_MatriculaTurmaDTO)GestaoEscolarUtilBO.DataRowToEntity(mtu.First(), new MTR_MatriculaTurmaDTO
                                                     {
                                                         aluno = (ACA_AlunoDTO.ReferenciaPesUsuario)GestaoEscolarUtilBO.DataRowToEntity(mtu.First(), new ACA_AlunoDTO.ReferenciaPesUsuario
                                                         {
                                                             pessoa = new PES_PessoaDTO.PessoaDadosBasicosTipado
                                                             {
                                                                 pes_id = (string.IsNullOrEmpty(mtu.First()["pes_idAluno"].ToString())
                                                                                                ? Guid.Empty
                                                                                                : new Guid(mtu.First()["pes_idAluno"].ToString())),
                                                                 pes_nome = mtu.First()["pes_nomeAluno"].ToString(),
                                                                 pes_situacao = Convert.ToByte((mtu.First()["pes_situacaoAluno"] == DBNull.Value ? 0 : mtu.First()["pes_situacaoAluno"])),
                                                                 pes_dataCriacao = mtu.First()["pes_dataCriacaoAluno"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(mtu.First()["pes_dataCriacaoAluno"]),
                                                                 pes_dataAlteracao = mtu.First()["pes_dataAlteracaoAluno"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(mtu.First()["pes_dataAlteracaoAluno"]),
                                                                 possuiAlteracaoFoto = mtu.First()["possuiALteracaoFotoAluno"] == DBNull.Value ? false : Convert.ToBoolean(mtu.First()["possuiALteracaoFotoAluno"])
                                                             }
                                                         }),
                                                         listaMatriculaTurmaDisciplina =
                                                         (from mdis in mtu
                                                          where mdis["mtd_id"] != DBNull.Value
                                                          group mdis by new
                                                          {
                                                              alu_id = mdis["alu_id"],
                                                              mtu_id = mdis["mtu_id"],
                                                              mtd_id = mdis["mtd_id"]
                                                          } into mtd
                                                          select ((MTR_MatriculaTurmaDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(mtd.First(),
                                                          new MTR_MatriculaTurmaDisciplinaDTO()))).ToList()
                                                     }))).ToList(),
                                                 listaTurmaCurriculo = (from cur in tur
                                                                        where cur["cur_id"] != DBNull.Value
                                                                        group cur by new
                                                                        {
                                                                            cur_id = cur["cur_id"],
                                                                            crr_id = cur["crr_id"],
                                                                            crp_id = cur["crp_id"]
                                                                        } into crp
                                                                        select (TUR_TurmaCurriculoDTO)GestaoEscolarUtilBO.DataRowToEntity(crp.First(), new TUR_TurmaCurriculoDTO())
                                                                            ).ToList(),
                                                 listaTurmaDocente = (from tdt in tur
                                                                      where tdt["tdt_id"] != DBNull.Value
                                                                      group tdt by new
                                                                      {
                                                                          tud_id = tdt["tud_id"],
                                                                          tdt_id = tdt["tdt_id"]

                                                                      } into td
                                                                      select (TUR_TurmaDocenteDTO)GestaoEscolarUtilBO.DataRowToEntity(td.First(), new TUR_TurmaDocenteDTO
                                                                      {
                                                                          docente = (TUR_TurmaDocenteDTO.ReferenciaPesUsuario)GestaoEscolarUtilBO.DataRowToEntity(td.First(), new TUR_TurmaDocenteDTO.ReferenciaPesUsuario
                                                                          {
                                                                              pessoa = new PES_PessoaDTO.PessoaDadosBasicosTipado
                                                                              {
                                                                                  pes_id = (string.IsNullOrEmpty(td.First()["pes_idDocente"].ToString())
                                                                                                                ? Guid.Empty
                                                                                                                : new Guid(td.First()["pes_idDocente"].ToString())),
                                                                                  pes_nome = td.First()["pes_nomeDocente"].ToString(),
                                                                                  pes_situacao = Convert.ToByte(td.First()["pes_situacaoDocente"] == DBNull.Value ? 0 : td.First()["pes_situacaoDocente"]),
                                                                                  pes_dataCriacao = td.First()["pes_dataCriacaoDocente"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(td.First()["pes_dataCriacaoDocente"]),
                                                                                  pes_dataAlteracao = td.First()["pes_dataAlteracaoDocente"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(td.First()["pes_dataAlteracaoDocente"]),
                                                                                  possuiAlteracaoFoto = false
                                                                              }
                                                                          })
                                                                      })).ToList(),
                                             })).ToList();

                return turmas;

                // old
                //Dictionary<TUR_Turma, List<TUR_TurmaDisciplina>> lstTurma = TUR_TurmaBO.BuscaTurmasAPI(tur_id, esc_id, doc_id, dataBase);

                //List<TUR_TurmaDTO> listDTO = new List<TUR_TurmaDTO>();

                //// Delegate que retorna a DTO de turma.
                //Func<TUR_TurmaDTO, IEnumerable<TUR_TurmaDisciplinaDTO>, IEnumerable<MTR_MatriculaTurmaDTO>, ESC_EscolaDTO.Referencia, TUR_TurmaDTO> retornaTurma =
                //    delegate(TUR_TurmaDTO turma, IEnumerable<TUR_TurmaDisciplinaDTO> listaTurmaDisciplina, IEnumerable<MTR_MatriculaTurmaDTO> listaMatriculaTurma, ESC_EscolaDTO.Referencia escola)
                //    {
                //        turma.listaTurmaDisciplina = listaTurmaDisciplina.ToList();
                //        turma.listaMatriculaTurma = listaMatriculaTurma.ToList(); ;
                //        turma.escola = escola;
                //        return turma;
                //    };

                //// Delegate que retorna a DTO de matricula.
                //Func<MTR_MatriculaTurmaDTO, MTR_MatriculaTurmaDTO> matriculaReferenciaAluno =
                //    delegate(MTR_MatriculaTurmaDTO matricula)
                //    {
                //        matricula.aluno = new ACA_AlunoDTO.Referencia { alu_id = matricula.alu_id.Value };
                //        matricula.alu_id = null;
                //        return matricula;
                //    };

                //// Delegate que retorna a DTO de TurmaDisciplina.
                //Func<TUR_TurmaDisciplinaDTO, TUR_TurmaDisciplinaDTO> turmaDisciplina =
                //    delegate(TUR_TurmaDisciplinaDTO tud)
                //    {
                //        tud.tur_id = null;
                //        return tud;
                //    };

                //listDTO = (from KeyValuePair<TUR_Turma, List<TUR_TurmaDisciplina>> tur in lstTurma
                //           select retornaTurma(
                //            (TUR_TurmaDTO)GestaoEscolarUtilBO.Clone(tur.Key, new TUR_TurmaDTO())

                //            ,
                //             ((from TUR_TurmaDisciplina tud in tur.Value
                //               select turmaDisciplina
                //               (
                //                   (TUR_TurmaDisciplinaDTO)GestaoEscolarUtilBO.Clone(tud, new TUR_TurmaDisciplinaDTO())

                //               )).ToList())
                //             ,
                //            ((from DataRow dr in MTR_MatriculaTurmaBO.BuscaAlunosPorTurma(tur.Key.tur_id).Rows
                //              //select (MTR_MatriculaTurmaDTO)GestaoEscolarUtilBO.Clone(mtr, new MTR_MatriculaTurmaDTO())).ToList();
                //              select matriculaReferenciaAluno
                //               (
                //                   (MTR_MatriculaTurmaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new MTR_MatriculaTurmaDTO())

                //               )).ToList())

                //                        ,
                //            new ESC_EscolaDTO.Referencia { esc_id = tur.Key.esc_id }
                //            )
                //            ).ToList();



                //return listDTO.Count == 0 ? null : listDTO;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Salva a lista informada no parametro.
        /// </summary>
        /// <param name="Json">Conteúdo JSon enviado no post</param>
        /// <returns></returns>
        public static TUR_TurmaDTO SalvarTurma(string Json)
        {
            TalkDBTransaction banco = new TUR_TurmaDAO()._Banco;
            TUR_TurmaDTO tur = new TUR_TurmaDTO();
            //try
            //{


            //    JObject listaDados = JObject.Parse(Json);

            //    tur = JsonConvert.DeserializeObject<TUR_TurmaDTO>(listaDados.ToString());

            //    TUR_Turma entTurma = (TUR_Turma)GestaoEscolarUtilBO.Clone(tur, new TUR_Turma());

            //    List<TUR_TurmaCurriculo> listTurmaCurriculo = (List<TUR_TurmaCurriculo>)GestaoEscolarUtilBO.Clone(tur.List_TUR_TurmaCurriculo, new List<TUR_TurmaCurriculo>());

            //    List<CadastroTurmaDisciplina> listaCadastroTurmaDisciplina = new List<CadastroTurmaDisciplina>();

            //    foreach (TUR_TurmaDisciplinaDTO tud in tur.List_TUR_TurmaDisciplina)
            //    {

            //        TUR_TurmaDisciplina entTurmaDisciplina = (TUR_TurmaDisciplina)GestaoEscolarUtilBO.Clone(tud, new TUR_TurmaDisciplina());

            //        CadastroTurmaDisciplina turmaDisciplina = new CadastroTurmaDisciplina
            //        {
            //            entTurmaDisciplina = entTurmaDisciplina
            //            ,
            //            entTurmaDiscRelDisciplina = tud.entTurmaDiscRelDisciplina
            //            ,
            //            listaTurmaDocente = new List<TUR_Turma_Docentes_Disciplina>()
            //            ,
            //            entTurmaCalendario = tud.List_TurmaCalendario
            //            ,
            //            listaAvaliacoesNaoAvaliar = tud.List_TurmaDisciplinaNaoAvaliado
            //            ,
            //            cde_id = tud.cde_id
            //        };

            //        listaCadastroTurmaDisciplina.Add(turmaDisciplina);
            //    }
            //    banco.Open();

            //    //TUR_TurmaBO.Save(
            //    //        entTurma,
            //    //        listTurmaCurriculo,
            //    //        turmaDisciplina,
            //    //        tur.List_TUR_TurmaCurriculoAvaliacao,
            //    //        banco,
            //    //        false,
            //    //        false,
            //    //        tur.ent_id
            //    //    );

            //}
            //catch (Exception ex)
            //{
            //    banco.Close(ex);
            //    throw;
            //}
            //finally
            //{
            //    banco.Close();
            //}

            return tur;
        }

        #endregion TUR_Turma

        #region TUR_TurmaDisciplina

        /// <summary>
        /// Seleciona uma disciplina pelo id
        /// </summary>
        /// <param name="tud_id">id da turmaDisciplina</param>
        /// <returns>TUR_TurmaDisciplina correspondente ao id</returns>
        public static List<TUR_TurmaDisciplinaDTO> SelecionarTurmaDisciplinaPorId(long tud_id)
        {
            try
            {
                return ProcessarResultadoTurmaDisciplina(TUR_TurmaDisciplinaBO.SelecionaDisciplinaDadosDocenteTurma(tud_id));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona as disciplinas do docente em determinada turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns></returns>
        public static List<TUR_TurmaDisciplinaDTO> SelecionarTurmaDisciplinaPorTurmaDocente(long tur_id, long doc_id)
        {
            try
            {
                return ProcessarResultadoTurmaDisciplina(TUR_TurmaDisciplinaBO.SelecionaDisciplinasPorDocenteTurma(tur_id, doc_id));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna uma listagem de disciplinas por escola quando não for passado a data base
        /// apenas os registros ativos serão retornados, caso passe a data base serão retornados
        /// apenas os registros criados ou alterados apos esta data.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns>dataTable com as disciplinas</returns>
        public static List<TUR_TurmaDisciplinaDTO> SelecionarTurmaDisciplinaPorEscola(int esc_id, DateTime dataBase)
        {
            try
            {
                return ProcessarResultadoTurmaDisciplina(TUR_TurmaDisciplinaBO.SelecionaDisciplinasPorEscola(esc_id, dataBase));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Processa o DataTable de Disciplinas
        /// </summary>
        /// <returns></returns>
        private static List<TUR_TurmaDisciplinaDTO> ProcessarResultadoTurmaDisciplina(DataTable dtDisciplina)
        {
            try
            {
                List<TUR_TurmaDisciplinaDTO> listaRetorno = new List<TUR_TurmaDisciplinaDTO>();

                if (dtDisciplina.Rows.Count > 0)
                {
                    // Método delegate para retornar a DTO de disciplina com os valores de turma e docente carregados.
                    Func<TUR_TurmaDisciplinaDTO, long, long, TUR_TurmaDisciplinaDTO> retornaDisciplina =
                        delegate (TUR_TurmaDisciplinaDTO disciplina, long idTurma, long idDocente)
                        {
                            disciplina.turma = new TUR_TurmaDTO.Referencia { tur_id = idTurma };
                            disciplina.docente = new ACA_DocenteDTO.Referencia { doc_id = idDocente };
                            disciplina.tur_id = null;
                            return disciplina;
                        };

                    listaRetorno = (from DataRow dr in dtDisciplina.Rows
                                    group dr by Convert.ToInt64(dr["tud_id"]) into grupo
                                    select retornaDisciplina
                                    (
                                        (TUR_TurmaDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new TUR_TurmaDisciplinaDTO())
                                        ,
                                        Convert.ToInt64(grupo.First()["tur_id"])
                                        ,
                                        Convert.ToInt64(grupo.First()["doc_id"])
                                    )).ToList();
                }
                return listaRetorno;
            }
            catch
            {
                throw;
            }
        }

        #endregion TUR_TurmaDisciplina

        #region DCL_Protocolo

        /// <summary>
        /// retorna um registro de protocolo pelo id.
        /// </summary>
        /// <param name="pro_id">id do protocolo - Guid</param>
        /// <returns></returns>
        public static DCL_ProtocoloDTO SelecionarProtocoloPorId(string pro_id)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                DCL_Protocolo protocolo = dao.SelecionarProtocoloPorId(new Guid(pro_id));

                DCL_ProtocoloDTO dto = null;

                if (protocolo != null)
                {
                    dto = (DCL_ProtocoloDTO)GestaoEscolarUtilBO.Clone(protocolo, new DCL_ProtocoloDTO());
                }

                return dto;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna os protocolos vinculados a escola a partir de uma data especifica podendo filtrar pelo tipo do protocolo
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos protocolos</param>
        /// <param name="pro_tipo">tipo do protocolo</param>
        /// <returns></returns>
        public static List<DCL_ProtocoloDTO> SelecionarProtocoloPorEscola(Int32 esc_id, DateTime dataBase, int pro_tipo)
        {
            try
            {
                if (esc_id.Equals(0))
                {
                    throw new ValidationException("esc_id: Id da escola é requerido.");
                }

                if (dataBase.Equals(new DateTime()))
                {
                    throw new ValidationException("dataBase: A data base para a consulta é requerida.");
                }

                DataTable dt = DCL_ProtocoloBO.SelectBy_Escola(esc_id, dataBase, pro_tipo);


                if (dt.Rows.Count == 0) return null;

                List<DCL_ProtocoloDTO> protocolos = (
                    from r in dt.AsEnumerable()
                    select (DCL_ProtocoloDTO)GestaoEscolarUtilBO.DataRowToEntity(r, new DCL_ProtocoloDTO())
                    ).ToList();

                return protocolos;
            }
            catch
            {
                throw;
            }
        }

        #endregion DCL_Protocolo

        #region ACA_RecursosAula

        /// <summary>
        /// retorna um recurso pelo id
        /// </summary>
        /// <param name="rsa_id">id do recurso</param>
        /// <returns></returns>
        public static ACA_RecursosAulaDTO SelecionarRecursoPorId(int rsa_id)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                ACA_RecursosAula recurso = dao.SelecionarRecursoPorId(rsa_id);
                ACA_RecursosAulaDTO dto = null;

                if (recurso != null)
                {
                    dto = (ACA_RecursosAulaDTO)GestaoEscolarUtilBO.Clone(recurso, new ACA_RecursosAulaDTO());
                }

                return dto;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna todos os recursos ativos.
        /// </summary>
        /// <returns></returns>
        public static List<ACA_RecursosAulaDTO> SelecionarRecursosAtivos()
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.SelecionarRecursosAtivos();

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                List<ACA_RecursosAulaDTO> recursos = (
                    from r in dt.AsEnumerable()
                    select (ACA_RecursosAulaDTO)GestaoEscolarUtilBO.DataRowToEntity(r, new ACA_RecursosAulaDTO())
                    ).ToList();

                return recursos;
            }
            catch
            {
                throw;
            }
        }

        #endregion ACA_RecursosAula

        #region CLS_TipoAtividadeAvaliativa

        /// <summary>
        /// retorna o tipo de atividade avaliativa por id
        /// </summary>
        /// <param name="tav_id">id do tipo de atividade avaliativa</param>
        /// <returns></returns>
        public static CLS_TipoAtividadeAvaliativaDTO SelecionarTipoAtividadeAvaliativaPorId(int tav_id)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                CLS_TipoAtividadeAvaliativa tipoAtividade = dao.SelecionarTipoAtividadeAvaliativaPorId(tav_id);

                CLS_TipoAtividadeAvaliativaDTO dto = null;

                if (tipoAtividade != null)
                {
                    dto = (CLS_TipoAtividadeAvaliativaDTO)GestaoEscolarUtilBO.Clone(tipoAtividade, new CLS_TipoAtividadeAvaliativaDTO());
                }

                return dto;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  retorna uma lista dos tipos de atividade avaliativas ativos
        /// </summary>
        /// <returns></returns>
        public static List<CLS_TipoAtividadeAvaliativaDTO> SelecionarTipoAtividadeAvaliativaAtivos()
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.SelecionarTipoAtividadeAvaliativaAtivas();

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                List<CLS_TipoAtividadeAvaliativaDTO> tipos = (
                    from r in dt.AsEnumerable()
                    select (CLS_TipoAtividadeAvaliativaDTO)GestaoEscolarUtilBO.DataRowToEntity(r, new CLS_TipoAtividadeAvaliativaDTO())
                    ).ToList();

                return tipos;
            }
            catch
            {
                throw;
            }
        }

        #endregion CLS_TipoAtividadeAvaliativa

        #region ACA_Docente

        /// <summary>
        /// retorna os registros de docente com os dados de pessoa e usuarios, caso
        /// tenha a data da ultima sincronização vai retornar apenas os alterados/criados
        /// apos esta data.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="dataBase">data base para a busca</param>
        /// <returns></returns>
        public static List<ACA_DocenteDTO> SelecionarDocentesPorTurma(long tur_id, DateTime dataBase)
        {
            try
            {
                List<ACA_DocenteDTO> docentes = ProcessarResultadoDocentes(new ApiDAO().SelecionarDocentesPorTurma(tur_id, dataBase));
                if (docentes == null || docentes.Count == 0)
                    return null;

                return docentes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna os registros de docente com os dados de pessoa e usuarios, caso
        /// tenha a data da ultima sincronização vai retornar apenas os alterados/criados
        /// apos esta data.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para a busca</param>
        /// <returns></returns>
        public static List<ACA_DocenteDTO> SelecionarDocentesPorEscola(int esc_id, DateTime dataBase)
        {
            try
            {
                List<ACA_DocenteDTO> docentes = ProcessarResultadoDocentes(new ApiDAO().SelecionarDocentesPorEscola(esc_id, dataBase));
                if (docentes == null || docentes.Count == 0)
                    return null;

                return docentes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna um registro de docente pelo id e suas escolas.
        /// </summary>
        /// <param name="doc_id">id do docente</param>
        /// <returns></returns>
        public static List<ACA_DocenteDTO> SelecionarEscolasDocentePorId(long doc_id)
        {
            try
            {

                return ProcessarResultadoDocentes(ACA_DocenteBO.SelecionaEscolaDocente(doc_id));

                //List<ACA_DocenteDTO> listaRetorno = new List<ACA_DocenteDTO>();

                //using (DataTable dtDocente = ACA_DocenteBO.SelecionaEscolaDocente(doc_id))
                //{
                //    if (dtDocente.Rows.Count > 0)
                //    {
                //        // Delegate que retorna a DTO de docente.
                //        Func<ACA_DocenteDTO, PES_PessoaDTO, IEnumerable<ESC_EscolaDTO>, ACA_DocenteDTO> retornaDocente =
                //            delegate(ACA_DocenteDTO docente, PES_PessoaDTO pessoa, IEnumerable<ESC_EscolaDTO> listaEscola)
                //            {
                //                docente.pessoa = pessoa;
                //                docente.listaEscola = listaEscola.ToList();
                //                return docente;
                //            };

                //        listaRetorno = (from DataRow dr in dtDocente.Rows
                //                        group dr by Convert.ToInt64(dr["doc_id"]) into grupo
                //                        select retornaDocente
                //                        (
                //                            (ACA_DocenteDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new ACA_DocenteDTO())
                //                            ,
                //                            (PES_PessoaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new PES_PessoaDTO())
                //                            ,
                //                            (from esc in grupo
                //                             where !string.IsNullOrEmpty(esc["esc_id"].ToString())
                //                             group esc by Convert.ToInt32(esc["esc_id"]) into gEscola
                //                             select (ESC_EscolaDTO)GestaoEscolarUtilBO.DataRowToEntity(gEscola.First(), new ESC_EscolaDTO()))
                //                        )).ToList();
                //    }
                //}

                //return listaRetorno;
            }
            catch
            {
                throw;
            }
        }

        private static List<ACA_DocenteDTO> ProcessarResultadoDocentes(DataTable dtDocente)
        {
            try
            {
                List<ACA_DocenteDTO> listaRetorno = new List<ACA_DocenteDTO>();
                if (dtDocente.Rows.Count > 0)
                {
                    // Delegate que retorna a DTO de docente.
                    Func<ACA_DocenteDTO, PES_PessoaDTO, IEnumerable<ESC_EscolaDTO>, ACA_DocenteDTO> retornaDocente =
                        delegate (ACA_DocenteDTO docente, PES_PessoaDTO pessoa, IEnumerable<ESC_EscolaDTO> listaEscola)
                        {
                            docente.pessoa = pessoa;
                            docente.listaEscola = listaEscola.ToList();
                            return docente;
                        };

                    listaRetorno = (from DataRow dr in dtDocente.Rows
                                    group dr by Convert.ToInt64(dr["doc_id"]) into grupo
                                    select retornaDocente
                                    (
                                        (ACA_DocenteDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new ACA_DocenteDTO())
                                        ,
                                        (PES_PessoaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new PES_PessoaDTO())
                                        ,
                                        (from esc in grupo
                                         where !string.IsNullOrEmpty(esc["esc_id"].ToString())
                                         group esc by Convert.ToInt32(esc["esc_id"]) into gEscola
                                         select (ESC_EscolaDTO)GestaoEscolarUtilBO.DataRowToEntity(gEscola.First(), new ESC_EscolaDTO
                                         {
                                             listaTurmaDocente = (from tud in gEscola
                                                                  select (TUR_TurmaDocenteDTO)GestaoEscolarUtilBO.DataRowToEntity(tud, new TUR_TurmaDocenteDTO())
                                                                      ).ToList()
                                         }))
                                    )).ToList();
                }
                return listaRetorno;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// retorna um registro de docente pelo id
        /// </summary>
        /// <param name="doc_id">id do docente</param>
        /// <returns></returns>
        public static List<ACA_DocenteDTO> SelecionarDocentePorId(long doc_id)
        {
            try
            {
                List<ACA_DocenteDTO> listaRetorno = new List<ACA_DocenteDTO>();

                using (DataTable dtDocente = ACA_DocenteBO.SelecionaDadosDocente(doc_id))
                {
                    if (dtDocente.Rows.Count > 0)
                    {
                        // Delegate que retorna a DTO de docente.
                        Func<ACA_DocenteDTO, PES_PessoaDTO, IEnumerable<ESC_EscolaDTO>, ACA_DocenteDTO> retornaDocente =
                            delegate (ACA_DocenteDTO docente, PES_PessoaDTO pessoa, IEnumerable<ESC_EscolaDTO> listaEscola)
                            {
                                docente.pessoa = pessoa;
                                docente.listaEscola = listaEscola.ToList();
                                return docente;
                            };

                        // Delegate que retorna a DTO de escola.
                        Func<ESC_EscolaDTO, IEnumerable<TUR_TurmaDTO>, ESC_EscolaDTO> retornaEscola =
                            delegate (ESC_EscolaDTO escola, IEnumerable<TUR_TurmaDTO> listaTurma)
                            {
                                escola.listaTurma = listaTurma.ToList();
                                return escola;
                            };

                        // Delegate que retorna a DTO de turma.
                        Func<TUR_TurmaDTO, IEnumerable<TUR_TurmaDisciplinaDTO>, IEnumerable<TUR_TurmaCurriculoDTO>, TUR_TurmaDTO> retornaTurma =
                            delegate (TUR_TurmaDTO turma, IEnumerable<TUR_TurmaDisciplinaDTO> listaTurmaDisciplina, IEnumerable<TUR_TurmaCurriculoDTO> listaTurmaCurriculo)
                            {
                                turma.listaTurmaDisciplina = listaTurmaDisciplina.ToList();
                                turma.listaTurmaCurriculo = listaTurmaCurriculo.ToList();
                                return turma;
                            };



                        // Docente - lista de escolas
                        // Escola do docente - lista de turmas
                        // Turma do docente - lista de disciplinas do docente
                        listaRetorno = (from DataRow dr in dtDocente.Rows
                                        group dr by Convert.ToInt64(dr["doc_id"]) into grupo
                                        select
                                        retornaDocente((ACA_DocenteDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new ACA_DocenteDTO())
                                   ,
                                                       (PES_PessoaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new PES_PessoaDTO())
                                   ,
                                                       (from esc in grupo
                                                        where !string.IsNullOrEmpty(esc["esc_id"].ToString())
                                                        group esc by Convert.ToInt32(esc["esc_id"]) into gEscola
                                                        select retornaEscola((ESC_EscolaDTO)GestaoEscolarUtilBO.DataRowToEntity(gEscola.First(), new ESC_EscolaDTO())
                                                          ,
                                                                             (from tur in gEscola
                                                                              where !string.IsNullOrEmpty(tur["tur_id"].ToString())
                                                                              group tur by Convert.ToInt64(tur["tur_id"]) into gTurma
                                                                              select retornaTurma((TUR_TurmaDTO)GestaoEscolarUtilBO.DataRowToEntity(gTurma.First(), new TUR_TurmaDTO())
                                                                                ,
                                                                                                  (from tud in gTurma
                                                                                                   where !string.IsNullOrEmpty(tud["tud_id"].ToString())
                                                                                                   group tud by Convert.ToInt64(tud["tud_id"]) into gDisciplina
                                                                                                   select (TUR_TurmaDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(gDisciplina.First(), new TUR_TurmaDisciplinaDTO())).ToList()
                                                                                                   ,
                                                                                                   (from tcr in gTurma
                                                                                                    group tcr by new
                                                                                                    {
                                                                                                        cur_id = Convert.ToInt32(tcr["cur_id"])
                                                                                                        ,
                                                                                                        crr_id = Convert.ToInt32(tcr["crr_id"])
                                                                                                        ,
                                                                                                        crp_id = Convert.ToInt32(tcr["crp_id"])
                                                                                                    } into gCurriculo
                                                                                                    select (TUR_TurmaCurriculoDTO)GestaoEscolarUtilBO.DataRowToEntity(gCurriculo.First(), new TUR_TurmaCurriculoDTO())))))))).ToList();
                    }
                }

                return listaRetorno;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna uma listagem de usuários de docentes na mesma unidade ou abaixo da unidade do usuário/grupo informado.
        /// </summary>
        /// <param name="usu_id">ID do usuário que está pesquisando</param>
        /// <param name="gru_id">ID do grupo do usuário que está pesquisando</param>
        /// <param name="sis_id">ID do sistema</param>
        /// <returns></returns>
        public static List<sUsrDocente> SelecionarUsrDocentesPorUsuarioGrupo(Guid usu_id, Guid gru_id, int sis_id, string usu_login, string usu_email, string pes_nome)
        {
            return ACA_DocenteBO.SelecionarUsrDocentesPorUsuarioGrupo(usu_id, gru_id, sis_id, usu_login, usu_email, pes_nome);
        }

        #endregion ACA_Docente

        #region ACA_Aluno

        /// <summary>
        /// Seleciona os dados da pessoa e da matrícula do aluno por id do aluno ou escola.
        /// </summary>
        /// <param name="alu_ids">IDs do aluno separados por ;</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <param name="esc_id">codigo da escola</param>
        /// <returns></returns>
        public static List<ACA_AlunoDTO> SelecionaDadosAluno(string alu_ids, int esc_id, DateTime dataBase)
        {
            List<ACA_AlunoDTO> listaRetorno = new List<ACA_AlunoDTO>();
            using (DataTable dtAluno = ACA_AlunoBO.SelecionaDadosAlunoMatricula(alu_ids, esc_id, dataBase))
            {
                if (dtAluno.Rows.Count > 0)
                {
                    // Delegate que retorna a DTO de aluno.
                    Func<ACA_AlunoDTO, PES_PessoaDTO, IEnumerable<MTR_MatriculaTurmaDTO>, ACA_AlunoDTO> retornaAluno =
                            delegate (ACA_AlunoDTO aluno, PES_PessoaDTO pessoa, IEnumerable<MTR_MatriculaTurmaDTO> listamatriculaTurma)
                            {
                                aluno.pessoa = pessoa;
                                aluno.pes_id = null;
                                aluno.listaMatriculaTurma = listamatriculaTurma.ToList();
                                return aluno;
                            };

                    Func<MTR_MatriculaTurmaDTO, MTR_MatriculaTurmaDTO> retornaMatriculaTurma =
                        delegate (MTR_MatriculaTurmaDTO matriculaTurma)
                        {
                            matriculaTurma.alu_id = null;
                            matriculaTurma.turma = new TUR_TurmaDTO.Referencia { tur_id = matriculaTurma.tur_id.Value };
                            matriculaTurma.tur_id = null;
                            return matriculaTurma;
                        };

                    // Lista de alunos e suas turmas em que estão matriculadas.
                    listaRetorno = (from DataRow dr in dtAluno.Rows
                                    group dr by Convert.ToInt64(dr["alu_id"]) into grupo
                                    select retornaAluno
                                           (
                                               (ACA_AlunoDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new ACA_AlunoDTO())
                                     ,
                                               (PES_PessoaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new PES_PessoaDTO())
                                     ,
                                               (from mtu in grupo
                                                where !string.IsNullOrEmpty(mtu["mtu_id"].ToString())
                                                group mtu by Convert.ToInt32(mtu["mtu_id"]) into gMatricula
                                                select retornaMatriculaTurma((MTR_MatriculaTurmaDTO)GestaoEscolarUtilBO.DataRowToEntity(gMatricula.First(), new MTR_MatriculaTurmaDTO
                                                {
                                                    listaMatriculaTurmaDisciplina =
                                                    (from mdis in gMatricula
                                                     group mdis by new
                                                     {
                                                         mtu_id = mdis["mtu_id"],
                                                         mtd_id = mdis["mtd_id"]
                                                     } into mtd
                                                     select ((MTR_MatriculaTurmaDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(mtd.First(),
                                                     new MTR_MatriculaTurmaDisciplinaDTO()))).ToList()
                                                })))
                                            )).ToList();
                }
            }

            return listaRetorno;
        }

        /// <summary>
        /// retorna registros de usuário dos alunos pelos ids concatenados com fotos redimensionadas e compactadas
        /// </summary>
        /// <param name="alu_ids">IDs do aluno separados por ;</param>
        /// <param name="largura">largura da foto</param>
        /// <param name="altura">altura da foto</param>
        /// <returns></returns>
        public static List<object> SelecionaDadosFotoAlunos(string alu_ids, int largura, int altura)
        {
            DataTable dtAluno = ACA_AlunoBO.SelecionaDadosFotoAlunos(alu_ids);

            List<object> lstRetorno = new List<object>();

            byte[] arquivo = null;
            var retorno = (from dr in dtAluno.Rows.Cast<DataRow>()
                           select new
                           {
                               usu_id = new Guid(Convert.ToString(dr["usu_id"])),
                               usu_login = Convert.ToString(dr["usu_login"]),
                               usu_email = Convert.ToString(dr["usu_email"]),
                               usu_senha = Convert.ToString(dr["usu_senha"]),
                               usu_criptografia = Convert.ToByte(dr["usu_criptografia"]),
                               usu_situacao = Convert.ToByte(dr["usu_situacao"]),
                               pes_id = string.IsNullOrEmpty(Convert.ToString(dr["pes_id"])) ? Guid.Empty : new Guid(Convert.ToString(dr["pes_id"])),
                               pes_nome = Convert.ToString(dr["pes_nome"]),
                               pes_sexo = string.IsNullOrEmpty(dr["pes_sexo"].ToString()) ? 0 : Convert.ToByte(dr["pes_sexo"]),
                               foto = string.IsNullOrEmpty(Convert.ToString(dr["foto"])) ? arquivo :
                                      altura > 0 && largura > 0 ? Compressor.Compress(ACA_AlunoBO.RedimensionaFoto((byte[])dr["foto"], largura, altura)) :
                                                                  Compressor.Compress((byte[])dr["foto"]),
                               usu_dataCriacao = Convert.ToDateTime(dr["usu_dataCriacao"]),
                               usu_dataAlteracao = Convert.ToDateTime(dr["usu_dataAlteracao"]),
                               pes_dataNascimento = string.IsNullOrEmpty(dr["pes_dataNascimento"].ToString())
                                    ? new DateTime()
                                    : Convert.ToDateTime(dr["pes_dataNascimento"])
                           }).ToList();
            retorno.ForEach(p => lstRetorno.Add(p));

            return lstRetorno;
        }

        // nao esta mais utilizando
        ///// <summary>
        ///// Seleciona os alunos matriculados na disciplina dada por determinado docente.
        ///// </summary>
        ///// <param name="doc_id">ID do docente.</param>
        ///// <param name="tud_id">ID da turma disciplina.</param>
        ///// <returns></returns>
        //public static List<ACA_AlunoDTO> SelecionaAlunosPorTurmaDocente(long doc_id, long tud_id)
        //{
        //    List<ACA_AlunoDTO> ltAluno = new List<ACA_AlunoDTO>();
        //    using (DataTable dtAluno = MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosPorTurmaDocente(doc_id, tud_id))
        //    {
        //        if (dtAluno.Rows.Count > 0)
        //        {
        //            // Delegate que retorna a DTO de aluno.
        //            Func<ACA_AlunoDTO, PES_PessoaDTO, IEnumerable<MTR_MatriculaTurmaDTO>, IEnumerable<ACA_AlunoCurriculoDTO>, ACA_AlunoDTO> retornaAluno =
        //                delegate(ACA_AlunoDTO aluno, PES_PessoaDTO pessoa, IEnumerable<MTR_MatriculaTurmaDTO> listamatriculaTurma, IEnumerable<ACA_AlunoCurriculoDTO> listaAlunoCurriculo)
        //                {
        //                    aluno.pessoa = pessoa;
        //                    aluno.listaMatriculaTurma = listamatriculaTurma.ToList();
        //                    aluno.listaAlunoCurriculo = listaAlunoCurriculo.ToList();
        //                    return aluno;
        //                };


        //            // Dados de matrícula dos alunos.
        //            ltAluno = (from DataRow dr in dtAluno.Rows
        //                       group dr by Convert.ToInt64(dr["alu_id"]) into grupo
        //                       select retornaAluno
        //                              (
        //                                  (ACA_AlunoDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new ACA_AlunoDTO())
        //                                                            ,
        //                                  (PES_PessoaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new PES_PessoaDTO())
        //                                                            ,
        //                                  (from mtu in grupo
        //                                   where !string.IsNullOrEmpty(mtu["mtu_id"].ToString())
        //                                   group mtu by Convert.ToInt32(mtu["mtu_id"]) into gMatricula
        //                                   select (MTR_MatriculaTurmaDTO)GestaoEscolarUtilBO.DataRowToEntity(gMatricula.First(), new MTR_MatriculaTurmaDTO()))
        //                             ,
        //                                  (from alc in grupo
        //                                   where !string.IsNullOrEmpty(alc["alc_id"].ToString())
        //                                   group alc by Convert.ToInt32(alc["alc_id"]) into gCurriculo
        //                                   select (ACA_AlunoCurriculoDTO)GestaoEscolarUtilBO.DataRowToEntity(gCurriculo.First(), new ACA_AlunoCurriculoDTO()))
        //                              )).ToList();
        //        }
        //    }

        //    return ltAluno;
        //}

        #endregion ACA_Aluno

        #region Usuário

        /// <summary>
        /// Seleciona os ids de aluno ou docente por usuário.
        /// </summary>
        /// <param name="usu_id">ID do usuário.</param>
        /// <param name="ent_id">ID da entidade.</param>
        /// <returns></returns>
        public static List<SYS_UsuarioDTO> SelecionaUsuario(string usu_login, Guid ent_id)
        {
            Func<SYS_UsuarioDTO, PES_PessoaDTO, long, long, SYS_UsuarioDTO> retornaUsuario =
                 delegate (SYS_UsuarioDTO usuario, PES_PessoaDTO pessoa, long alu_id, long doc_id)
                 {
                     usuario.pessoa = pessoa;
                     usuario.pes_id = null;
                     usuario.aluno = new ACA_AlunoDTO.Referencia { alu_id = alu_id };
                     usuario.docente = new ACA_DocenteDTO.Referencia { doc_id = doc_id };
                     return usuario;
                 };

            List<SYS_UsuarioDTO> listaRetorno = new List<SYS_UsuarioDTO>();

            using (DataTable dtUsuario = new ApiDAO().SelecionaAlunoDocentePorUsuario(Guid.Empty, usu_login, ent_id, 0, new DateTime()))
            {
                listaRetorno = (from DataRow dr in dtUsuario.Rows
                                group dr by dr["usu_id"].ToString() into grupo
                                select retornaUsuario
                                (
                                    (SYS_UsuarioDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new SYS_UsuarioDTO())
                                   ,
                                    (PES_PessoaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new PES_PessoaDTO())
                                    ,
                                    Convert.ToInt64(grupo.First()["alu_id"])
                                    ,
                                    Convert.ToInt64(grupo.First()["doc_id"])
                                )).ToList();
            }

            return listaRetorno;
        }

        /// <summary>
        /// Seleciona os ids de aluno ou docente por usuário.
        /// </summary>
        /// <param name="usu_id">ID do usuário.</param>
        /// <param name="ent_id">ID da entidade.</param>
        /// <returns></returns>
        public static List<SYS_UsuarioDTO> SelecionaUsuario(Guid usu_id, Int64 esc_id, Guid ent_id, DateTime dataBase)
        {
            Func<SYS_UsuarioDTO, PES_PessoaDTO, long, long, SYS_UsuarioDTO> retornaUsuario =
                delegate (SYS_UsuarioDTO usuario, PES_PessoaDTO pessoa, long alu_id, long doc_id)
                {
                    usuario.pessoa = pessoa;
                    usuario.pes_id = null;

                    if (alu_id > 0)
                        usuario.aluno = new ACA_AlunoDTO.Referencia { alu_id = alu_id };

                    if (doc_id > 0)
                        usuario.docente = new ACA_DocenteDTO.Referencia { doc_id = doc_id };

                    return usuario;
                };

            List<SYS_UsuarioDTO> listaRetorno = new List<SYS_UsuarioDTO>();

            using (DataTable dtUsuario = new ApiDAO().SelecionaAlunoDocentePorUsuario(usu_id, string.Empty, ent_id, esc_id, dataBase))
            {
                listaRetorno = (from DataRow dr in dtUsuario.Rows
                                group dr by dr["usu_id"].ToString() into grupo
                                select retornaUsuario
                                (
                                    (SYS_UsuarioDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new SYS_UsuarioDTO())
                                   ,
                                    (PES_PessoaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new PES_PessoaDTO())
                                    ,
                                    Convert.ToInt64(grupo.First()["alu_id"])
                                    ,
                                    Convert.ToInt64(grupo.First()["doc_id"])
                                )).ToList();
            }

            return listaRetorno;
        }

        #endregion Usuário

        #region ACA_TipoCiclo

        //Método utilizado no padrão novo de retorno da API, comentado pois o diario não está adaptado a ele ainda.

        ///// <summary>
        ///// Retorna os ACA_TipoCiclo ativos
        ///// </summary>
        ///// <returns> List<ACA_TipoCicloDTO></returns>        
        //public static List<ACA_TipoCicloDTO> SelecionarTipoCiclo()
        //{
        //    List<ACA_TipoCicloDTO> lista = new List<ACA_TipoCicloDTO>();

        //    try
        //    {
        //        lista = (
        //                       from dr in ACA_TipoCicloBO.GetSelect()
        //                       select (ACA_TipoCicloDTO)GestaoEscolarUtilBO.Clone(dr, new ACA_TipoCicloDTO())
        //                   ).ToList();
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return lista;
        //}


        /// <summary>
        /// Retorna os ACA_TipoCiclo ativos
        /// </summary>
        /// <returns> List<ACA_TipoCicloDTO></returns>        
        public static BuscaTipoCicloSaidaDTO SelecionarTipoCiclo()
        {
            BuscaTipoCicloSaidaDTO saida = new BuscaTipoCicloSaidaDTO();

            try
            {
                saida.List_TipoCiclo = (
                               from dr in ACA_TipoCicloBO.GetSelect()
                               select (ACA_TipoCicloDTO)GestaoEscolarUtilBO.Clone(dr, new ACA_TipoCicloDTO())
                           ).ToList();


            }
            catch
            {
                throw;
            }

            return saida;
        }

        #endregion

        #region ORC_OrientacaoCurricular

        /// <summary>
        /// Busca a orientacao curricular pelo ID
        /// </summary>
        /// <param name="ocr_id"></param>
        /// <returns></returns>
        public static ORC_OrientacaoCurricularDTO SelecionaOrientacoesPorID(Int64 ocr_id)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                ORC_OrientacaoCurricular orientacao = dao.SelecionarOrientacaoPorId(ocr_id);
                ORC_OrientacaoCurricularDTO dto = null;

                if (orientacao != null)
                {
                    dto = (ORC_OrientacaoCurricularDTO)GestaoEscolarUtilBO.Clone(orientacao, new ORC_OrientacaoCurricularDTO());
                }

                return dto;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Busca as orientacoes curriculares por disciplina e por data base
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public static List<ORC_OrientacaoCurricularDTO> SelecionaOrientacoesPorTurmaDisplinaDataBase(long tud_id, DateTime dataBase, long ocr_idSuperior, Nullable<long> tpc_id)
        {

            List<ORC_OrientacaoCurricular> listOC = ORC_OrientacaoCurricularBO.SelecionaPorTurmaDisciplinaDataBase(tud_id, dataBase, ocr_idSuperior, tpc_id);
            List<ORC_OrientacaoCurricularDTO> list = (from oc in listOC
                                                      select (ORC_OrientacaoCurricularDTO)GestaoEscolarUtilBO.Clone(oc, new ORC_OrientacaoCurricularDTO())).ToList();

            return list;
        }

        /// <summary>
        /// Busca as orientacoes curriculares pela database, entidade e escola
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public static List<object> SelecionaOrientacoesPorEntidadeEscolaDataBase(Guid ent_id, int esc_id, DateTime dataBase)
        {
            DataTable dtOC = ORC_OrientacaoCurricularBO.SelecionaOrientacoesPorEntidadeEscolaDataBase(ent_id, esc_id, dataBase);

            List<object> list = new List<object>();

            int cur_id = 0, crr_id = 0, crp_id = 0, cal_id = 0;
            Dictionary<string, object> itemOC = new Dictionary<string, object>();
            List<ORC_OrientacaoCurricularDTO> lstItemOC = new List<ORC_OrientacaoCurricularDTO>();

            foreach (DataRow row in dtOC.Rows)
            {
                if (cur_id != Convert.ToInt32(row["cur_id"]) || crr_id != Convert.ToInt32(row["crr_id"]) ||
                    crp_id != Convert.ToInt32(row["crp_id"]) || cal_id != Convert.ToInt32(row["cal_id"]))
                {
                    cur_id = Convert.ToInt32(row["cur_id"]);
                    crr_id = Convert.ToInt32(row["crr_id"]);
                    crp_id = Convert.ToInt32(row["crp_id"]);
                    cal_id = Convert.ToInt32(row["cal_id"]);

                    if (itemOC != new Dictionary<string, object>())
                    {
                        if (lstItemOC != new List<ORC_OrientacaoCurricularDTO>())
                            itemOC.Add("orientacoes_curriculares", lstItemOC);

                        list.Add(itemOC);
                        itemOC = new Dictionary<string, object>();
                        lstItemOC = new List<ORC_OrientacaoCurricularDTO>();
                    }

                    itemOC.Add("cur_id", cur_id);
                    itemOC.Add("crr_id", crr_id);
                    itemOC.Add("crp_id", crp_id);
                    itemOC.Add("cal_id", cal_id);
                }
                lstItemOC.Add((ORC_OrientacaoCurricularDTO)GestaoEscolarUtilBO.DataRowToEntity(row, new ORC_OrientacaoCurricularDTO()));
            }

            if (itemOC != new Dictionary<string, object>())
            {
                if (lstItemOC != new List<ORC_OrientacaoCurricularDTO>())
                    itemOC.Add("orientacoes_curriculares", lstItemOC);
                list.Add(itemOC);
            }

            return list;
        }

        #endregion

        /// <summary>
        /// Retorna os alunos que estão ativos na escola ligada à unidade administrativa informada.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="uad_id">ID da unidade administrativa da escola</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAlunosAtivosPorUnidadeAdministrativaEscola(Guid ent_id, Guid uad_id)
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaAlunosAtivosPorUnidadeAdministrativaEscola(ent_id, uad_id);
        }

        /// <summary>
        /// Retorna os alunos que estão ativos na escola ligada à unidade administrativa informada.
        /// Listagem detalhada
        /// </summary>
        /// <param name="buscaAlunosDetalhadoEscolaEntradaDTO">Entidade de filtros informados</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static BuscaAlunosDetalhadoEscolaSaidaDTO SelecionaAlunosDetalhadoPorUnidadeAdministrativaEscola(BuscaAlunosDetalhadoEscolaEntradaDTO buscaAlunosDetalhadoEscolaEntradaDTO)
        {
            BuscaAlunosDetalhadoEscolaSaidaDTO buscaAlunosDetalhadoEscolaSaidaDTO;
            try
            {
                buscaAlunosDetalhadoEscolaSaidaDTO = new BuscaAlunosDetalhadoEscolaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                if (buscaAlunosDetalhadoEscolaEntradaDTO.esc_id <= 0 &&
                    buscaAlunosDetalhadoEscolaEntradaDTO.uad_id.Equals(new Guid()))
                    throw new Exception("Deve-se informar pelo menos um filtro (esc_id ou uad_id).");

                DataTable dadosAlunos = dao.SelecionaAlunosDetalhadoPorUnidadeAdministrativaEscola(buscaAlunosDetalhadoEscolaEntradaDTO.ent_id,
                                                                                                   buscaAlunosDetalhadoEscolaEntradaDTO.esc_id,
                                                                                                   buscaAlunosDetalhadoEscolaEntradaDTO.uad_id);

                List<AlunoDetalhado> listAlunos = new List<AlunoDetalhado>();
                foreach (DataRow dr in dadosAlunos.Rows)
                {
                    AlunoDetalhado alu = new AlunoDetalhado();
                    alu.uad_id = new Guid(dr["uad_id"].ToString());
                    alu.ent_id = new Guid(dr["ent_id"].ToString());
                    alu.esc_id = Convert.ToInt32(dr["esc_id"]);
                    alu.alc_matricula = dr["alc_matricula"].ToString();
                    alu.alu_id = Convert.ToInt64(dr["alu_id"]);
                    alu.pes_id = new Guid(dr["pes_id"].ToString());
                    alu.pes_nome = dr["pes_nome"].ToString();
                    alu.alc_id = Convert.ToInt32(dr["alc_id"]);
                    alu.mtu_id = Convert.ToInt32(dr["mtu_id"]);
                    alu.tur_id = Convert.ToInt64(dr["tur_id"]);
                    alu.tur_codigo = dr["tur_codigo"].ToString();
                    alu.tur_descricao = dr["tur_descricao"].ToString();

                    listAlunos.Add(alu);
                }
                buscaAlunosDetalhadoEscolaSaidaDTO.Alunos = listAlunos;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaAlunosDetalhadoEscolaSaidaDTO = new BuscaAlunosDetalhadoEscolaSaidaDTO();
                buscaAlunosDetalhadoEscolaSaidaDTO.Status = 1;
                buscaAlunosDetalhadoEscolaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaAlunosDetalhadoEscolaSaidaDTO;
        }

        #region RHU_Colaborador

        /// <summary>
        /// Retorna os colaboradores da unidade administrativa e cargo informados.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="uad_id">ID da unidade administrativa da escola</param>
        /// <param name="crg_id">ID do cargo para filtrar colaboradores</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaColaboradoresPorUnidade_Cargo(Guid ent_id, Guid uad_id, int crg_id)
        {
            ApiDAO dao = new ApiDAO();
            return dao.SelecionaColaboradoresPorUnidade_Cargo(ent_id, uad_id, crg_id);
        }

        /// <summary>
        /// retorna os registros de colaborador com os dados de pessoa, docente, colaboradorCargo e colaboradorFuncao
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="esc_id">matricula do docente</param>
        /// <param name="dataBase">data base para a busca</param>
        /// <returns></returns>
        public static List<RHU_ColaboradorDTO> SelecionarColaboradoresPorEscolaMatricula(int esc_id, string matricula)
        {
            try
            {
                List<RHU_ColaboradorDTO> colaboradores = ProcessarResultadoColaboradores(new ApiDAO().SelecionarColaboradoresPorEscolaMatricula(esc_id, matricula));
                if (colaboradores == null || colaboradores.Count == 0)
                    return null;

                return colaboradores;
            }
            catch
            {
                throw;
            }
        }

        private static List<RHU_ColaboradorDTO> ProcessarResultadoColaboradores(DataTable dtColaborador)
        {
            try
            {
                List<RHU_ColaboradorDTO> listaRetorno = new List<RHU_ColaboradorDTO>();
                if (dtColaborador.Rows.Count > 0)
                {
                    Func<RHU_ColaboradorCargoDTO
                         , IEnumerable<RHU_ColaboradorCargoDisciplina>
                         , RHU_ColaboradorCargoDTO> retornarColaboradorCargo =
                         delegate (RHU_ColaboradorCargoDTO colaboradorCargo
                                , IEnumerable<RHU_ColaboradorCargoDisciplina> colaboradorCargoDisciplina)
                         {
                             colaboradorCargo.colaboradorCargoDisciplina = colaboradorCargoDisciplina.ToList();
                             if (colaboradorCargo.colaboradorCargoDisciplina != null)
                                 colaboradorCargo.colaboradorCargoDisciplina.ForEach(p => p.IsNew = false);
                             return colaboradorCargo;
                         };

                    // Delegate que retorna a DTO de colaborador.
                    Func<RHU_ColaboradorDTO, PES_PessoaDTO, IEnumerable<PES_PessoaDocumento>, ACA_Docente, IEnumerable<RHU_ColaboradorCargoDTO>, IEnumerable<RHU_ColaboradorFuncao>, RHU_ColaboradorDTO> retornaDocente =
                        delegate (RHU_ColaboradorDTO colaborador, PES_PessoaDTO pessoa, IEnumerable<PES_PessoaDocumento> documentos, ACA_Docente docente, IEnumerable<RHU_ColaboradorCargoDTO> listaCargo, IEnumerable<RHU_ColaboradorFuncao> listaFuncao)
                        {
                            colaborador = colaborador ?? new RHU_ColaboradorDTO();
                            colaborador.pessoa = pessoa;
                            colaborador.documentos = documentos.ToList();
                            if (colaborador.documentos != null)
                                colaborador.documentos.ForEach(p => p.IsNew = false);
                            colaborador.docente = docente;
                            colaborador.docente.IsNew = false;
                            colaborador.colaboradorCargo = listaCargo.ToList();
                            if (colaborador.colaboradorCargo != null)
                                colaborador.colaboradorCargo.ForEach(p => p.IsNew = false);
                            colaborador.colaboradorFuncao = listaFuncao.ToList();
                            if (colaborador.colaboradorFuncao != null)
                                colaborador.colaboradorFuncao.ForEach(p => p.IsNew = false);
                            return colaborador;
                        };

                    listaRetorno = (from DataRow dr in dtColaborador.Rows
                                    group dr by Convert.ToInt64(dr["col_id"]) into grupo
                                    select retornaDocente
                                    (
                                        (RHU_ColaboradorDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.FirstOrDefault(), new RHU_ColaboradorDTO())
                                        ,
                                        (PES_PessoaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.FirstOrDefault(), new PES_PessoaDTO())
                                        ,
                                        (from coc in grupo
                                         where !string.IsNullOrEmpty(coc["tdo_id"].ToString())
                                         group coc by new { tdo_id = new Guid(coc["tdo_id"].ToString()) } into gDoc
                                         select (PES_PessoaDocumento)GestaoEscolarUtilBO.DataRowToEntity(gDoc.First(), new PES_PessoaDocumento()))
                                         ,
                                        (ACA_Docente)GestaoEscolarUtilBO.DataRowToEntity(grupo.FirstOrDefault(), new ACA_Docente())
                                        ,
                                        (from coc in grupo
                                         where !string.IsNullOrEmpty(coc["coc_id"].ToString())
                                         group coc by new { crg_id = Convert.ToInt32(coc["crg_id"]), coc_id = Convert.ToInt32(coc["coc_id"]) } into gCargo
                                         select retornarColaboradorCargo((RHU_ColaboradorCargoDTO)GestaoEscolarUtilBO.DataRowToEntity(gCargo.FirstOrDefault(), new RHU_ColaboradorCargoDTO()),
                                                                         (from ccd in gCargo
                                                                          where !string.IsNullOrEmpty(ccd["tds_id"].ToString())
                                                                          group ccd by new { tds_id = Convert.ToInt32(ccd["tds_id"]) } into gCardoDis
                                                                          select (RHU_ColaboradorCargoDisciplina)GestaoEscolarUtilBO.DataRowToEntity(gCardoDis.FirstOrDefault(), new RHU_ColaboradorCargoDisciplina()))))
                                         ,
                                        (from cfn in grupo
                                         where !string.IsNullOrEmpty(cfn["cof_id"].ToString())
                                         group cfn by new { crg_id = Convert.ToInt32(cfn["fun_id"]), coc_id = Convert.ToInt32(cfn["cof_id"]) } into gFuncao
                                         select (RHU_ColaboradorFuncao)GestaoEscolarUtilBO.DataRowToEntity(gFuncao.FirstOrDefault(), new RHU_ColaboradorFuncao()))
                                    )).ToList();
                }
                return listaRetorno;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Salva os dados informados no arquivo.
        /// </summary>
        /// <param name="Json">Conteúdo Json enviado no post</param>
        /// <returns>List<ACA_CalendarioAnualDTO></returns>
        public static List<RHU_ColaboradorDTO> SalvarColaboradores(string Json)
        {
            #region Delegate

            Func<RHU_ColaboradorCargoDTO
                 , IEnumerable<RHU_ColaboradorCargoDisciplina>
                 , RHU_ColaboradorCargoDTO> retornarColaboradorCargo =
                 delegate (RHU_ColaboradorCargoDTO colaboradorCargo
                        , IEnumerable<RHU_ColaboradorCargoDisciplina> colaboradorCargoDisciplina)
                 {
                     colaboradorCargo.colaboradorCargoDisciplina = colaboradorCargoDisciplina.ToList();
                     return colaboradorCargo;
                 };

            Func<RHU_ColaboradorDTO
                 , IEnumerable<PES_PessoaDocumento>
                 , JArray
                 , IEnumerable<RHU_ColaboradorFuncao>
                 , RHU_ColaboradorDTO> retornarColaborador =
                 delegate (RHU_ColaboradorDTO colaborador
                        , IEnumerable<PES_PessoaDocumento> documentos
                        , JArray colaboradorCargo
                        , IEnumerable<RHU_ColaboradorFuncao> colaboradorFuncao)
                 {
                     colaborador.documentos = documentos.ToList();
                     colaborador.colaboradorCargo = (from item in colaboradorCargo.AsEnumerable()
                                                     select retornarColaboradorCargo((RHU_ColaboradorCargoDTO)JsonConvert.DeserializeObject<RHU_ColaboradorCargoDTO>(item.ToString())
                                                                                     ,
                                                                                     (from item2 in ((JArray)item.SelectToken("colaboradorCargoDisciplina") ?? new JArray()).AsEnumerable()
                                                                                      select (RHU_ColaboradorCargoDisciplina)JsonConvert.DeserializeObject<RHU_ColaboradorCargoDisciplina>(item2.ToString())))).ToList();
                     colaborador.colaboradorFuncao = colaboradorFuncao.ToList();
                     return colaborador;
                 };

            #endregion Delegate

            List<RHU_ColaboradorDTO> lista = new List<RHU_ColaboradorDTO>();
            TalkDBTransaction bancoGestao = new ACA_CalendarioAnualDAO()._Banco;
            TalkDBTransaction bancoCore = new SYS_EntidadeDAO()._Banco;

            try
            {
                JArray listaDados = (JArray.Parse(Json) ?? new JArray());

                lista = (from item in listaDados.AsEnumerable()
                         select retornarColaborador(
                             (RHU_ColaboradorDTO)JsonConvert.DeserializeObject<RHU_ColaboradorDTO>(item.ToString())
                             ,
                             (from item2 in ((JArray)item.SelectToken("documentos") ?? new JArray()).AsEnumerable()
                              select (PES_PessoaDocumento)JsonConvert.DeserializeObject<PES_PessoaDocumento>(item2.ToString()))
                             ,
                             (JArray)item.SelectToken("colaboradorCargo") ?? new JArray()
                             ,
                             (from item5 in ((JArray)item.SelectToken("colaboradorFuncao") ?? new JArray()).AsEnumerable()
                              select (RHU_ColaboradorFuncao)JsonConvert.DeserializeObject<RHU_ColaboradorFuncao>(item5.ToString())))).ToList();

                bancoGestao.Open();
                bancoCore.Open();

                foreach (RHU_ColaboradorDTO item in lista)
                {
                    RHU_Colaborador entityColaborador = new RHU_Colaborador
                    {
                        col_id = item.col_id,
                        ent_id = item.ent_id,
                        col_dataAdmissao = item.col_dataAdmissao,
                        col_dataDemissao = item.col_dataDemissao,
                        col_situacao = item.col_situacao,
                        col_controladoIntegracao = item.col_controladoIntegracao,
                        IsNew = (item.col_id > 0) ? false : true
                    };

                    RHU_ColaboradorBO.SaveAPI(item.pessoa, entityColaborador, item.docente, item.documentos, item.colaboradorCargo, item.colaboradorFuncao, bancoGestao, bancoCore);

                    #region Atualiza dados da lista para Json de retorno

                    item.col_id = entityColaborador.col_id;
                    item.pes_id = entityColaborador.pes_id;
                    item.pessoa.pes_id = entityColaborador.pes_id;
                    item.documentos.ForEach(p => p.pes_id = entityColaborador.pes_id);
                    item.colaboradorCargo.ForEach(p =>
                    {
                        p.col_id = entityColaborador.col_id;
                        p.colaboradorCargoDisciplina.ForEach(d => d.col_id = entityColaborador.col_id);
                    });
                    item.colaboradorFuncao.ForEach(p => p.col_id = entityColaborador.col_id);

                    #endregion Atualiza dados da lista para Json de retorno
                }
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                bancoCore.Close(ex);
                throw;
            }
            finally
            {
                bancoGestao.Close();
                bancoCore.Close();
            }

            return lista;
        }

        #endregion

        #region RHU_Cargo
        /// <summary>
		/// Retorna os Cargos de acordo com os filtros passados.
		/// </summary>
		/// <param name="crg_situacao">Flag da situação do cargo</param>
		/// <param name="crg_cargoDocente">Flag se é cargo de docente</param>
		/// <returns></returns>
		public static List<RHU_CargoDTO> SelecionaCargosPorDocenteSituacao(bool docente, bool bloqueado, string ent_id, bool cargaHoraria)
        {
            return (from DataRow dr in RHU_CargoBO.SelectBy_CargoDocente_Situacao(bloqueado ? 2 : 1, docente, new Guid(ent_id)).Rows
                    group dr by Convert.ToInt32(dr["crg_id"]) into g
                    select (RHU_CargoDTO)GestaoEscolarUtilBO.DataRowToEntity(g.First(), new RHU_CargoDTO()
                    {
                        CargaHoraria = !cargaHoraria ? null :
                           (
                               from DataRow linhaPeriodo in g
                               select (RHU_CargaHorariaDTO)GestaoEscolarUtilBO.DataRowToEntity(linhaPeriodo, new RHU_CargaHorariaDTO())
                            ).ToList()
                    })).ToList();

        }
        #endregion RHU_Cargo

        #region RHU_CargaHoraria
        /// <summary>
        /// Seleciona carga horária de acordo com os cargos de docentes enviados
        /// </summary>
        /// <param name="idsCargo">Cargos a obter carga horária</param>
        /// <param name="ent_id">Id da Entidade</param>
        public static List<RHU_CargaHorariaDTO> SelecionaCargaHorariaPorCargoDocente(string idsCargo, string ent_id)
        {
            return (from c in RHU_CargaHorariaBO.SelectBy_CargoDocente(idsCargo, new Guid(ent_id)).AsEnumerable()
                    select (RHU_CargaHorariaDTO)GestaoEscolarUtilBO.DataRowToEntity(c, new RHU_CargaHorariaDTO())).ToList();
        }
        #endregion RHU_CargaHoraria

        #region BoletimEscolar

        /// <summary>
        /// Seleciona boletim escolar do aluno
        /// </summary>
        /// <param name="buscaBoletimEscolarAlunoEntradaDTO"></param>
        /// <returns></returns>
        public static List<BuscaBoletimEscolarAlunoSaidaDTO> BuscaBoletimEscolarAluno(BuscaBoletimEscolarAlunoEntradaDTO buscaBoletimEscolarAlunoEntradaDTO)
        {
            List<BuscaBoletimEscolarAlunoSaidaDTO> list = new List<BuscaBoletimEscolarAlunoSaidaDTO>();
            BuscaBoletimEscolarAlunoSaidaDTO buscaBoletimEscolarAlunoSaidaDTO = new BuscaBoletimEscolarAlunoSaidaDTO();

            try
            {
                ApiDAO dao = new ApiDAO();
                DataTable BoletimDados = dao.SelecionaBoletimAluno(buscaBoletimEscolarAlunoEntradaDTO.alu_id, buscaBoletimEscolarAlunoEntradaDTO.mtu_id);

                foreach (DataRow dr in BoletimDados.Rows)
                {

                    buscaBoletimEscolarAlunoSaidaDTO.alu_id = Convert.ToInt64(dr["alu_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.mtu_id = Convert.ToInt32(dr["mtu_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.tur_codigo = dr["tur_codigo"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.tur_id = Convert.ToInt64(dr["tur_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.tpc_id = Convert.ToInt32(dr["tpc_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.tpc_ordem = Convert.ToInt32(dr["tpc_ordem"]);
                    buscaBoletimEscolarAlunoSaidaDTO.mtd_id = Convert.ToInt32(dr["mtd_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.tud_id = Convert.ToInt64(dr["tud_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.tud_global = Convert.ToBoolean(dr["tud_global"]);
                    buscaBoletimEscolarAlunoSaidaDTO.Disciplina = dr["Disciplina"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.DisciplinaEspecial = dr["DisciplinaEspecial"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.tud_disciplinaEspecial = Convert.ToBoolean(dr["tud_disciplinaEspecial"]);
                    buscaBoletimEscolarAlunoSaidaDTO.tpc_nome = dr["tpc_nome"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.numeroFaltas = Convert.ToInt32(dr["numeroFaltas"]);
                    buscaBoletimEscolarAlunoSaidaDTO.avaliacao = dr["avaliacao"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.avaliacaoOriginal = dr["avaliacaoOriginal"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.NotaNumerica = Convert.ToBoolean(dr["NotaNumerica"]);
                    buscaBoletimEscolarAlunoSaidaDTO.avaliacaoAdicional = dr["avaliacaoAdicional"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.NotaAdicionalNumerica = Convert.ToBoolean(dr["NotaAdicionalNumerica"]);
                    buscaBoletimEscolarAlunoSaidaDTO.NotaRP = dr["NotaRP"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.NotaIDRP = (!string.IsNullOrEmpty(dr["NotaIDRP"].ToString()) ? Convert.ToInt32(dr["NotaIDRP"]) : -1);
                    buscaBoletimEscolarAlunoSaidaDTO.mostraConceito = Convert.ToBoolean(dr["mostraConceito"]);
                    buscaBoletimEscolarAlunoSaidaDTO.mostraNota = Convert.ToBoolean(dr["mostraNota"]);
                    buscaBoletimEscolarAlunoSaidaDTO.ava_mostraConceito = Convert.ToBoolean(dr["ava_mostraConceito"]);
                    buscaBoletimEscolarAlunoSaidaDTO.ava_mostraNota = Convert.ToBoolean(dr["ava_mostraNota"]);
                    buscaBoletimEscolarAlunoSaidaDTO.mostraFrequencia = Convert.ToBoolean(dr["mostraFrequencia"]);
                    buscaBoletimEscolarAlunoSaidaDTO.naoExibirNota = Convert.ToBoolean(dr["naoExibirNota"]);
                    buscaBoletimEscolarAlunoSaidaDTO.naoExibirFrequencia = Convert.ToBoolean(dr["naoExibirFrequencia"]);
                    buscaBoletimEscolarAlunoSaidaDTO.NotaSomar = Convert.ToDecimal(dr["NotaSomar"]);
                    buscaBoletimEscolarAlunoSaidaDTO.frequenciaAcumulada = Convert.ToDecimal(dr["frequenciaAcumulada"]);
                    buscaBoletimEscolarAlunoSaidaDTO.MostrarLinhaDisciplina = Convert.ToBoolean(dr["MostrarLinhaDisciplina"]);
                    buscaBoletimEscolarAlunoSaidaDTO.NotaID = Convert.ToInt32(dr["NotaID"]);
                    buscaBoletimEscolarAlunoSaidaDTO.ava_id = Convert.ToInt32(dr["ava_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.ava_tipo = Convert.ToByte(dr["ava_tipo"]);
                    buscaBoletimEscolarAlunoSaidaDTO.fav_tipo = Convert.ToByte(dr["fav_tipo"]);
                    buscaBoletimEscolarAlunoSaidaDTO.ava_exibeSemProfessor = Convert.ToBoolean(dr["ava_exibeSemProfessor"]);
                    buscaBoletimEscolarAlunoSaidaDTO.ava_exibeNaoAvaliados = Convert.ToBoolean(dr["ava_exibeNaoAvaliados"]);
                    buscaBoletimEscolarAlunoSaidaDTO.semProfessor = Convert.ToBoolean(dr["semProfessor"]);
                    buscaBoletimEscolarAlunoSaidaDTO.naoAvaliado = Convert.ToBoolean(dr["naoAvaliado"]);
                    buscaBoletimEscolarAlunoSaidaDTO.naoLancarNota = Convert.ToBoolean(dr["naoLancarNota"]);
                    buscaBoletimEscolarAlunoSaidaDTO.ava_idRec = (!string.IsNullOrEmpty(dr["ava_idRec"].ToString()) ? Convert.ToInt32(dr["ava_idRec"]) : -1);
                    buscaBoletimEscolarAlunoSaidaDTO.ava_nomeRec = dr["ava_nomeRec"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.esc_codigo = dr["esc_codigo"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.esc_nome = dr["esc_nome"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.NotaRecEsp = dr["NotaRecEsp"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.ava_idRecEsp = (!string.IsNullOrEmpty(dr["ava_idRecEsp"].ToString()) ? Convert.ToInt32(dr["ava_idRecEsp"]) : -1);
                    buscaBoletimEscolarAlunoSaidaDTO.NotaIDRecEsp = (!string.IsNullOrEmpty(dr["NotaIDRecEsp"].ToString()) ? Convert.ToInt32(dr["NotaIDRecEsp"]) : -1);
                    buscaBoletimEscolarAlunoSaidaDTO.NotaTotal = Convert.ToDecimal(dr["NotaTotal"]);
                    buscaBoletimEscolarAlunoSaidaDTO.NotaResultado = dr["NotaResultado"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.notaDisciplinaConceito = Convert.ToBoolean(dr["notaDisciplinaConceito"]);
                    buscaBoletimEscolarAlunoSaidaDTO.dda_id = (!string.IsNullOrEmpty(dr["dda_id"].ToString()) ? Convert.ToInt32(dr["dda_id"]) : -1);
                    buscaBoletimEscolarAlunoSaidaDTO.tud_tipo = Convert.ToByte(dr["tud_tipo"]);
                    buscaBoletimEscolarAlunoSaidaDTO.ausenciasCompensadas = Convert.ToInt32(dr["ausenciasCompensadas"]);
                    buscaBoletimEscolarAlunoSaidaDTO.FrequenciaFinalAjustada = Convert.ToDecimal(dr["FrequenciaFinalAjustada"]);
                    buscaBoletimEscolarAlunoSaidaDTO.esa_tipo = Convert.ToInt32(dr["esa_tipo"]);
                    buscaBoletimEscolarAlunoSaidaDTO.nomeDisciplina = dr["nomeDisciplina"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.tds_id = Convert.ToInt32(dr["tds_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.tds_ordem = Convert.ToInt32(dr["tds_ordem"]);
                    buscaBoletimEscolarAlunoSaidaDTO.EnriquecimentoCurricular = Convert.ToBoolean(dr["EnriquecimentoCurricular"]);
                    buscaBoletimEscolarAlunoSaidaDTO.Recuperacao = Convert.ToBoolean(dr["Recuperacao"]);
                    buscaBoletimEscolarAlunoSaidaDTO.ParecerFinal = dr["ParecerFinal"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.ParecerConclusivo = dr["ParecerConclusivo"].ToString();
                    buscaBoletimEscolarAlunoSaidaDTO.fav_variacao = Convert.ToDecimal(dr["fav_variacao"]);
                    buscaBoletimEscolarAlunoSaidaDTO.cur_id = Convert.ToInt32(dr["cur_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.crr_id = Convert.ToInt32(dr["crr_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.crp_id = Convert.ToInt32(dr["crp_id"]);
                    buscaBoletimEscolarAlunoSaidaDTO.disRelacionadas = dr["disRelacionadas"].ToString();

                    list.Add(buscaBoletimEscolarAlunoSaidaDTO);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// Retorna se é tipo da disciplina de componente de regência do enumerador.
        /// </summary>
        private static bool tipoComponenteRegencia(byte tud_tipo)
        {
            return ((byte)TurmaDisciplinaTipo.ComponenteRegencia == tud_tipo || (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia == tud_tipo);
        }

        /// <summary>
        /// Seleciona boletim escolar dos alunos
        /// </summary>
        /// <param name="buscaBoletimEscolarAlunoEntradaDTO"></param>
        /// <returns></returns>
        public static List<BuscaBoletimEscolarDosAlunosSaidaDTO> BuscaBoletimEscolarDosAlunos(BuscaBoletimEscolarDosAlunosEntradaDTO buscaBoletimEscolarDosAlunosEntradaDTO)
        {
            List<BuscaBoletimEscolarDosAlunosSaidaDTO> list = new List<BuscaBoletimEscolarDosAlunosSaidaDTO>();
            BuscaBoletimEscolarDosAlunosSaidaDTO buscaBoletimEscolarDosAlunosSaidaDTO = new BuscaBoletimEscolarDosAlunosSaidaDTO();

            BuscaBoletimEscolarAlunoSaidaDTO boletimEscolarAluno = new BuscaBoletimEscolarAlunoSaidaDTO();
            List<BuscaBoletimEscolarAlunoSaidaDTO> listBoletim = new List<BuscaBoletimEscolarAlunoSaidaDTO>();

            try
            {
                List<string> listaAlunos = buscaBoletimEscolarDosAlunosEntradaDTO.alu_ids.Split(',').ToList();
                int tpc_id = buscaBoletimEscolarDosAlunosEntradaDTO.tpc_id;

                Guid ent_id = new Guid();
                if (listaAlunos.Any())
                    ent_id = ACA_AlunoBO.GetEntity(new ACA_Aluno { alu_id = Convert.ToInt64(listaAlunos.First()) }).ent_id;
                else
                    return list;

                bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);
                int tne_idInfantil = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, ent_id);

                List<ACA_AlunoBO.BoletimDadosAluno> lBoletins = ACA_AlunoBO.BuscaBoletimAlunos(buscaBoletimEscolarDosAlunosEntradaDTO.alu_ids, buscaBoletimEscolarDosAlunosEntradaDTO.mtu_ids, tpc_id, ent_id);



                List<TurmasBoletimLiberado> lstTurmasPeriodoLiberado = new List<TurmasBoletimLiberado>();

                foreach (var turmaCalendario in lBoletins.GroupBy(g => new { g.tur_id, g.cal_id }).Select(s => new { s.Key.tur_id, s.Key.cal_id }))
                {
                    DataTable dtEventoCalendario = ACA_EventoBO.Select_EventoLiberacao(turmaCalendario.cal_id, turmaCalendario.tur_id, tpc_id);

                    if (dtEventoCalendario.Rows.Count > 0)
                    {
                        lstTurmasPeriodoLiberado.Add(
                            new TurmasBoletimLiberado
                            {
                                cal_id = turmaCalendario.cal_id
                                ,
                                tur_id = turmaCalendario.tur_id
                                ,
                                tpc_id = tpc_id
                            });
                    }
                }

                foreach (var aluno in lBoletins.GroupBy(b => new { b.alu_id, b.mtu_id })
                                               .Select(b => new { b.Key.alu_id, b.Key.mtu_id }))
                {

                    List<ACA_AlunoBO.BoletimDadosAluno> dadosBoletim = lBoletins.Any(p => p.alu_id == aluno.alu_id) ? lBoletins.Where(p => p.alu_id == aluno.alu_id).ToList() : new List<ACA_AlunoBO.BoletimDadosAluno>();

                    List<ACA_AlunoBO.BoletimDadosAluno> lBoletimAluno;
                    if (dadosBoletim == null || dadosBoletim.Count() == 0)
                    {
                        lBoletimAluno = ACA_AlunoBO.BuscaBoletimAlunos(aluno.alu_id.ToString(), aluno.mtu_id.ToString(), tpc_id, ent_id);
                    }
                    else
                    {
                        lBoletimAluno = dadosBoletim.FindAll(p => p.alu_id == aluno.alu_id);
                    }


                    ACA_AlunoBO.BoletimDadosAluno boletimAluno = new ACA_AlunoBO.BoletimDadosAluno();
                    if (lBoletimAluno != null && lBoletimAluno.Any())
                        boletimAluno = lBoletimAluno.First();
                    else
                        continue;
                                        
                    List<BoletimAluno> BoletimDados = boletimAluno.listaNotasEFaltas;

                    List<ACA_CurriculoPeriodo> lstCurriculoPeriodo = ACA_CurriculoPeriodoBO.Seleciona_PeriodosRelacionados_Equivalentes(boletimAluno.cur_id, boletimAluno.crr_id, boletimAluno.crp_id);

                    decimal FrequenciaFinalAjustadaRegencia = BoletimDados.LastOrDefault(p => ((p.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia ||
                                                                                                p.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia) &&
                                                                                               (p.FrequenciaFinalAjustada > 0 && p.tpc_id == tpc_id))).FrequenciaFinalAjustada;
                    int tpc_ordem = BoletimDados.FirstOrDefault(p => p.tpc_id == tpc_id).tpc_ordem;
                    bool ultimoBimestre = BoletimDados.OrderByDescending(i => i.tpc_ordem).FirstOrDefault().tpc_id.Equals(tpc_id);
                    decimal variacao = BoletimDados.FirstOrDefault().fav_variacao;
                    string formatacaoPorcentagemFrequencia = GestaoEscolarUtilBO.CriaFormatacaoDecimal(variacao > 0 ? GestaoEscolarUtilBO.RetornaNumeroCasasDecimais(variacao) : 2);

                    string mensagemJustificativaAbonoFalta = "Justificativa de abono em: {0}";

                    buscaBoletimEscolarDosAlunosSaidaDTO = new BuscaBoletimEscolarDosAlunosSaidaDTO
                    {
                        alu_id = boletimAluno.alu_id,
                        alc_id = boletimAluno.alc_id,
                        alc_matricula = boletimAluno.alc_matricula,
                        alc_matriculaEstadual = boletimAluno.alc_matriculaEstadual,
                        mtu_id = boletimAluno.mtu_id,
                        mtu_numeroChamada = boletimAluno.mtu_numeroChamada,
                        arq_idFoto = boletimAluno.arq_idFoto,
                        uad_nome = boletimAluno.uad_nome,
                        esc_nome = boletimAluno.esc_nome,
                        pes_nome = boletimAluno.pes_nome,
                        pes_nomeOficial = boletimAluno.pes_nomeOficial,
                        pes_nomeRegistro = boletimAluno.pes_nomeRegistro,
                        pes_nome_abreviado = boletimAluno.pes_nome_abreviado,
                        tur_id = boletimAluno.tur_id,
                        tur_codigo = boletimAluno.tur_codigo,
                        fav_id = boletimAluno.fav_id,
                        fav_variacao = BoletimDados.First().fav_variacao,
                        cur_id = boletimAluno.cur_id,
                        crr_id = boletimAluno.crr_id,
                        crp_id = boletimAluno.crp_id,
                        cur_nome = boletimAluno.cur_nome,
                        tci_id = boletimAluno.tci_id,
                        tci_nome = boletimAluno.tci_nome,
                        tci_layout = boletimAluno.tci_layout,
                        tci_exibirBoletim = boletimAluno.tci_exibirBoletim,
                        ava_id = boletimAluno.ava_id,
                        ava_nome = boletimAluno.ava_nome,
                        cal_id = boletimAluno.cal_id,
                        cal_ano = boletimAluno.cal_ano,
                        cpe_atividadeFeita = boletimAluno.cpe_atividadeFeita,
                        cpe_atividadePretendeFazer = boletimAluno.cpe_atividadePretendeFazer,
                        fechamentoPorImportacao = boletimAluno.fechamentoPorImportacao,
                        displayPerfilAluno = "",
                        displayRecomendacoes = "",
                        displayResultados = "",
                        displayParecerConclusivo = ultimoBimestre && !string.IsNullOrEmpty(boletimAluno.ParecerConclusivo) ? "" : "none",
                        parecerConclusivo = boletimAluno.ParecerConclusivo,
                        cicloClass = !string.IsNullOrEmpty(boletimAluno.tci_layout) ? boletimAluno.tci_layout : "cicloLayoutPadrao",
                        qualidade = lBoletimAluno.Where(p => !string.IsNullOrEmpty(p.qualidade)).Select(p => p.qualidade).Distinct().ToList(),
                        desempenho = lBoletimAluno.Where(p => !string.IsNullOrEmpty(p.desempenho)).Select(p => p.desempenho).Distinct().ToList(),
                        recomendacaoAluno = lBoletimAluno.Where(p => !string.IsNullOrEmpty(p.recomendacaoAluno)).Select(p => p.recomendacaoAluno).Distinct().ToList(),
                        recomendacaoResponsavel = lBoletimAluno.Where(p => !string.IsNullOrEmpty(p.recomendacaoResponsavel)).Select(p => p.recomendacaoResponsavel).Distinct().ToList(),
                        mostraConceitoGlobal = BoletimDados.Count(p => p.tud_global && p.mtu_id > 0) > 0,
                        exibeCompensacaoAusencia = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, ent_id),
                        nomeNota = (BoletimDados.Any(p => p.esa_tipo == 1) ? "Nota" : "Conceito"),
                        possuiFreqExterna = boletimAluno.possuiFreqExterna,
                        periodos = (from BoletimAluno item in BoletimDados
                                    orderby item.tpc_ordem
                                    group item by item.tpc_id into g
                                    select new BuscaBoletimEscolarDosAlunosSaidaPeriodosDTO
                                    {
                                        tpc_id = g.Key,
                                        tpc_nome = g.First().tpc_nome,
                                        tpc_ordem = g.First().tpc_ordem,
                                        ava_idRec = g.First().ava_idRec,
                                        ava_nomeRec = g.First().ava_nomeRec,
                                        MatriculaPeriodo = g.First().mtu_id > 0
                                                           ? "Responsável pelo lançamento no " + g.First().tpc_nome + ": Turma " + g.First().tur_codigo + " (" +
                                                             (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id)
                                                              ? g.First().esc_codigo + " - " + g.First().esc_nome
                                                              : g.First().esc_nome) + ")"
                                                           : "Aluno não matriculado no " + g.First().tpc_nome
                                    }).ToList(),
                        linhaTerritorioSaber = boletimAluno.linhaTerritorioSaber,
                        territorioSaber = boletimAluno.territorioSaber,
                        BoletimLiberado = true,
                        justificativaAbonoFalta = string.IsNullOrEmpty(boletimAluno.justificativaAbonoFalta) ? string.Empty : string.Format(mensagemJustificativaAbonoFalta, boletimAluno.justificativaAbonoFalta),
                        ensinoInfantil = boletimAluno.tne_id == tne_idInfantil
                    };
                    buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas =
                       (from BoletimAluno item in BoletimDados
                        where item.tur_id > 0
                        orderby item.tud_tipo, item.tud_global descending, item.Disciplina
                        group item by item.Disciplina into g
                        select new BuscaBoletimEscolarDosAlunosSaidaTodasDisciplinasDTO
                        {
                            tud_id = g.First().tud_id
                            ,
                            Disciplina = g.First().nomeDisciplina
                            ,
                            tds_ordem = g.First().tds_ordem
                            ,
                            totalAulas = g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada || g.Any(p => p.naoExibirFrequencia) ? "-" :
                                            g.Any(p => p.NotaID > 0) ? (g.Sum(p => (p.mostraFrequencia && !p.naoExibirFrequencia && (p.NotaID > 0 || tipoComponenteRegencia(p.tud_tipo))
                                                                        && p.tpc_ordem <= tpc_ordem) ? p.numeroAulas : 0)).ToString() : "-"
                            ,
                            totalFaltas = g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada || g.Any(p => p.naoExibirFrequencia) ? "-" :
                                            g.Any(p => p.NotaID > 0) ? (g.Sum(p => (p.mostraFrequencia && !p.naoExibirFrequencia && (p.NotaID > 0 || tipoComponenteRegencia(p.tud_tipo))
                                                                        && p.tpc_ordem <= tpc_ordem) ? p.numeroFaltas : 0)).ToString() : "-"
                            ,
                            ausenciasCompensadas = g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada || g.Any(p => p.naoExibirFrequencia) ? "-" :
                                                    g.Any(p => p.NotaID > 0) ? (g.Sum(p => p.tpc_ordem <= tpc_ordem ? p.ausenciasCompensadas : 0)).ToString() : "-"
                            ,
                            FrequenciaFinalAjustada = g.Any(p => p.naoExibirFrequencia) ? "-" :
                                                        g.Any(p => p.NotaID > 0 && p.tpc_id == tpc_id) ? ((FrequenciaFinalAjustadaRegencia > 0) ? FrequenciaFinalAjustadaRegencia :
                                                        g.LastOrDefault(p => p.FrequenciaFinalAjustada > 0 && p.tpc_id == tpc_id).FrequenciaFinalAjustada).ToString(formatacaoPorcentagemFrequencia) + (boletimAluno.possuiFreqExterna ? "*" : "") : "-"
                            ,
                            tud_Tipo = g.First().tud_tipo
                            ,
                            tipoComponenteRegencia = tipoComponenteRegencia(g.First().tud_tipo)
                            ,
                            tipoDocenciaCompartilhada = g.First().tud_tipo == (byte)TurmaDisciplinaTipo.DocenciaCompartilhada
                            ,
                            tud_global = g.First().tud_global
                            ,
                            mostrarDisciplina = g.Count(p => p.MostrarLinhaDisciplina)
                            ,
                            NotaTotal = g.First().NotaTotal
                            ,
                            MediaFinal = (g.Any(p => p.naoExibirNota) || !ultimoBimestre) ? "-" : (!string.IsNullOrEmpty(g.Last().NotaResultado) ? g.Last().NotaResultado : "-")
                            ,
                            regencia = (byte)(g.First().tud_tipo == (byte)TurmaDisciplinaTipo.Regencia
                                              || g.First().tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia
                                              || (g.First().tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia
                                                  && controleOrdemDisciplinas) ? 1 : 2)
                            ,
                            enriquecimentoCurricular = g.First().EnriquecimentoCurricular
                            ,
                            parecerFinal = g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada ? "-" :
                                                               // Se for experiência, vai exibir o menor resultado do aluno (F, NF e nulo nessa ordem)
                                                               g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.Experiencia ? g.OrderBy(p => p.ParecerFinal ?? "z").FirstOrDefault().ParecerFinal :
                                                               // Caso contrário, mantem o parecer final atual
                                                               g.Last().ParecerFinal
                            ,
                            parecerConclusivo = g.Last().ParecerConclusivo
                            ,
                            recuperacao = g.First().Recuperacao
                            ,
                            disRelacionadas = g.First().disRelacionadas
                            ,
                            notas = (
                                        from per in buscaBoletimEscolarDosAlunosSaidaDTO.periodos.ToList()
                                        orderby per.tpc_ordem
                                        select new BuscaBoletimEscolarDosAlunosSaidaNotasDTO
                                        {
                                            tpc_id = per.tpc_id
                                            ,
                                            nota = (
                                                        from BoletimAluno bNota in BoletimDados
                                                        where
                                                            bNota.Disciplina == g.Key
                                                            && bNota.tpc_id == per.tpc_id
                                                        select new BuscaBoletimEscolarDosAlunosSaidaNotaDTO
                                                        {
                                                            Nota = (
                                                                            bNota.dda_id > 0 ? "-"
                                                                            :
                                                                            !bNota.mostraNota || bNota.naoExibirNota || (tpc_id > 0 && bNota.tpc_id > tpc_id)
                                                                                ? "-"
                                                                                : (bNota.NotaNumerica
                                                                                        ? bNota.avaliacao ??
                                                                                            "-"
                                                                                        : (bNota.
                                                                                                NotaAdicionalNumerica
                                                                                                ? bNota.
                                                                                                    avaliacaoAdicional ??
                                                                                                "-"
                                                                                                : bNota.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres
                                                                                                    ? bNota.avaliacao ?? "-"
                                                                                                    : "-")
                                                                                    )
                                                                        ).Replace(".", ",")
                                                                ,
                                                            Conceito =
                                                                    (
                                                                    bNota.dda_id > 0 ? "-"
                                                                    :
                                                                    bNota.mostraConceito
                                                                        ? (bNota.NotaNumerica
                                                                                ? "-"
                                                                                : bNota.avaliacao)
                                                                        : "-")
                                                            ,
                                                            tpc_id = bNota.tpc_id
                                                            ,
                                                            NotaRP = bNota.NotaRP
                                                            ,
                                                            numeroAulas = bNota.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada ? "-" :

                                                                                            // Se for "Experiência", faz contagem específica
                                                                                            bNota.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.Experiencia ?

                                                                                            ((((bNota.cur_id == boletimAluno.cur_id && bNota.crr_id == boletimAluno.crr_id && bNota.crp_id == boletimAluno.crp_id) || lstCurriculoPeriodo.Any(p => p.cur_id == bNota.cur_id && p.crr_id == bNota.crr_id && p.crp_id == bNota.crp_id))
                                                                                                && bNota.mostraFrequencia && !bNota.naoExibirFrequencia && bNota.tpc_id <= tpc_id && (bNota.NotaID > 0 || bNota.numeroAulas > 0))
                                                                                                    ? (g.Sum(p => (p.tpc_id == bNota.tpc_id) ? p.numeroAulas : 0)).ToString() + (g.First().possuiFreqExterna ? "*" : "") : "-")

                                                                                                    :

                                                                                            // Caso contrário, mantem a contagem atual                                                                                                
                                                                                            ((((bNota.cur_id == boletimAluno.cur_id && bNota.crr_id == boletimAluno.crr_id && bNota.crp_id == boletimAluno.crp_id) || lstCurriculoPeriodo.Any(p => p.cur_id == bNota.cur_id && p.crr_id == bNota.crr_id && p.crp_id == bNota.crp_id))
                                                                                                && bNota.mostraFrequencia && !bNota.naoExibirFrequencia && bNota.tpc_id <= tpc_id && (bNota.NotaID > 0 || bNota.numeroAulas > 0)) ? bNota.numeroAulas.ToString() + (g.First().possuiFreqExterna ? "*" : "") : "-")
                                                            ,
                                                            numeroFaltas = bNota.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada ? "-" :

                                                                                            // Se for "Experiência", faz contagem específica
                                                                                            bNota.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.Experiencia ?

                                                                                            ((((bNota.cur_id == boletimAluno.cur_id && bNota.crr_id == boletimAluno.crr_id && bNota.crp_id == boletimAluno.crp_id) || lstCurriculoPeriodo.Any(p => p.cur_id == bNota.cur_id && p.crr_id == bNota.crr_id && p.crp_id == bNota.crp_id))
                                                                                                && bNota.mostraFrequencia && !bNota.naoExibirFrequencia && bNota.tpc_id <= tpc_id && (bNota.NotaID > 0 || bNota.numeroFaltas > 0))
                                                                                                    ? (g.Sum(p => (p.tpc_id == bNota.tpc_id) ? p.numeroFaltas : 0)).ToString() + (g.First().possuiFreqExterna ? "*" : "") : "-")

                                                                                                    :

                                                                                            // Caso contrário, mantem a contagem atual                                                                                                
                                                                                            ((((bNota.cur_id == boletimAluno.cur_id && bNota.crr_id == boletimAluno.crr_id && bNota.crp_id == boletimAluno.crp_id) || lstCurriculoPeriodo.Any(p => p.cur_id == bNota.cur_id && p.crr_id == bNota.crr_id && p.crp_id == bNota.crp_id))
                                                                                                && bNota.mostraFrequencia && !bNota.naoExibirFrequencia && bNota.tpc_id <= tpc_id && (bNota.NotaID > 0 || bNota.numeroFaltas > 0)) ? bNota.numeroFaltas.ToString() + (g.First().possuiFreqExterna ? "*" : "") : "-")
                                                            ,
                                                            tud_Tipo = g.First().tud_tipo
                                                            ,
                                                            possuiFreqExterna = g.First().possuiFreqExterna
                                                        }).FirstOrDefault()
                                        }).ToList()
                        }).ToList();

                    buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas.ForEach(d => d.totalAulas = d.totalAulas + (d.notas.Any(n => n.nota.possuiFreqExterna) ? "*" : ""));
                    buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas.ForEach(d => d.totalFaltas = d.totalFaltas + (d.notas.Any(n => n.nota.possuiFreqExterna) ? "*" : ""));

                    if (controleOrdemDisciplinas)
                    {
                        buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas = buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas.OrderBy(d => d.regencia).ThenBy(d => d.tds_ordem).ToList();
                    }
                    else
                    {
                        buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas = buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas.OrderBy(d => d.regencia).ThenBy(d => d.Disciplina).ToList();
                    }

                    buscaBoletimEscolarDosAlunosSaidaDTO.QtComponenteRegencia = buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas
                        .Where(d => !d.enriquecimentoCurricular && //Retira as que são de enriquecimento curricular
                                    !d.recuperacao) //Retira as recuperacoes
                        .Count(p => (tipoComponenteRegencia(p.tud_Tipo)) && p.mostrarDisciplina > 0);
                    buscaBoletimEscolarDosAlunosSaidaDTO.QtComponentes = buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas
                        .Where(d => !d.enriquecimentoCurricular && //Retira as que são de enriquecimento curricular
                                    !d.recuperacao) //Retira as recuperacoes
                        .Count(p => (p.mostrarDisciplina > 0));

                    var x = buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas
                        .Where(p => p.tud_Tipo != (byte)ACA_CurriculoDisciplinaTipo.DisciplinaEletivaAluno);

                    if (ultimoBimestre && x != null && x.Count() > 0 && !string.IsNullOrEmpty(x.LastOrDefault().parecerConclusivo))
                    {
                        buscaBoletimEscolarDosAlunosSaidaDTO.parecerConclusivo = x.LastOrDefault().parecerConclusivo;
                    }

                    if (!lstTurmasPeriodoLiberado.Any(p => p.cal_id == boletimAluno.cal_id && p.tur_id == boletimAluno.tur_id && p.tpc_id == tpc_id))
                    {
                        buscaBoletimEscolarDosAlunosSaidaDTO.BoletimLiberado = false;
                    }

                    //Se houver alguma disciplina de territórios do saber (experiencia) então vai mostrar o boletim dividido em 2
                    if (buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas.Any(t => t.tud_Tipo == (byte)TurmaDisciplinaTipo.Experiencia))
                    {
                        //Mostra primeiro os resultados
                        buscaBoletimEscolarDosAlunosSaidaDTO.displayResultados = "";
                        //Esconde o perfil do aluno e as recomendações
                        buscaBoletimEscolarDosAlunosSaidaDTO.displayPerfilAluno = "none";
                        buscaBoletimEscolarDosAlunosSaidaDTO.displayRecomendacoes = "none";

                        //Adiciona o boletim do aluno na lista
                        list.Add(buscaBoletimEscolarDosAlunosSaidaDTO);

                        //Cria uma segunda variável copiando a que foi adicionada na lista de boletim
                        BuscaBoletimEscolarDosAlunosSaidaDTO buscaBoletimEscolarDosAlunosSaidaDTO2 = new BuscaBoletimEscolarDosAlunosSaidaDTO
                        {
                            alc_id = buscaBoletimEscolarDosAlunosSaidaDTO.alc_id,
                            alc_matricula = buscaBoletimEscolarDosAlunosSaidaDTO.alc_matricula,
                            alc_matriculaEstadual = buscaBoletimEscolarDosAlunosSaidaDTO.alc_matriculaEstadual,
                            alu_id = buscaBoletimEscolarDosAlunosSaidaDTO.alu_id,
                            arq_idFoto = buscaBoletimEscolarDosAlunosSaidaDTO.arq_idFoto,
                            ava_id = buscaBoletimEscolarDosAlunosSaidaDTO.ava_id,
                            ava_nome = buscaBoletimEscolarDosAlunosSaidaDTO.ava_nome,
                            cal_ano = buscaBoletimEscolarDosAlunosSaidaDTO.cal_ano,
                            cal_id = buscaBoletimEscolarDosAlunosSaidaDTO.cal_id,
                            cicloClass = buscaBoletimEscolarDosAlunosSaidaDTO.cicloClass,
                            cpe_atividadeFeita = buscaBoletimEscolarDosAlunosSaidaDTO.cpe_atividadeFeita,
                            cpe_atividadePretendeFazer = buscaBoletimEscolarDosAlunosSaidaDTO.cpe_atividadePretendeFazer,
                            crp_id = buscaBoletimEscolarDosAlunosSaidaDTO.crp_id,
                            crr_id = buscaBoletimEscolarDosAlunosSaidaDTO.crr_id,
                            cur_id = buscaBoletimEscolarDosAlunosSaidaDTO.cur_id,
                            cur_nome = buscaBoletimEscolarDosAlunosSaidaDTO.cur_nome,
                            Date = buscaBoletimEscolarDosAlunosSaidaDTO.Date,
                            desempenho = buscaBoletimEscolarDosAlunosSaidaDTO.desempenho,
                            //Mostra o perfil do aluno e as recomendações
                            displayPerfilAluno = "",
                            displayRecomendacoes = "",
                            //Esconde os resultados
                            displayResultados = "none",
                            esc_nome = buscaBoletimEscolarDosAlunosSaidaDTO.esc_nome,
                            exibeCompensacaoAusencia = buscaBoletimEscolarDosAlunosSaidaDTO.exibeCompensacaoAusencia,
                            fav_id = buscaBoletimEscolarDosAlunosSaidaDTO.fav_id,
                            fav_variacao = buscaBoletimEscolarDosAlunosSaidaDTO.fav_variacao,
                            fechamentoPorImportacao = buscaBoletimEscolarDosAlunosSaidaDTO.fechamentoPorImportacao,
                            linhaTerritorioSaber = buscaBoletimEscolarDosAlunosSaidaDTO.linhaTerritorioSaber,
                            mostraConceitoGlobal = buscaBoletimEscolarDosAlunosSaidaDTO.mostraConceitoGlobal,
                            mtu_id = buscaBoletimEscolarDosAlunosSaidaDTO.mtu_id,
                            mtu_numeroChamada = buscaBoletimEscolarDosAlunosSaidaDTO.mtu_numeroChamada,
                            nomeNota = buscaBoletimEscolarDosAlunosSaidaDTO.nomeNota,
                            parecerConclusivo = buscaBoletimEscolarDosAlunosSaidaDTO.parecerConclusivo,
                            periodos = buscaBoletimEscolarDosAlunosSaidaDTO.periodos,
                            pes_nome = buscaBoletimEscolarDosAlunosSaidaDTO.pes_nome,
                            pes_nomeOficial = buscaBoletimEscolarDosAlunosSaidaDTO.pes_nomeOficial,
                            pes_nomeRegistro = buscaBoletimEscolarDosAlunosSaidaDTO.pes_nomeRegistro,
                            pes_nome_abreviado = buscaBoletimEscolarDosAlunosSaidaDTO.pes_nome_abreviado,
                            QtComponenteRegencia = buscaBoletimEscolarDosAlunosSaidaDTO.QtComponenteRegencia,
                            QtComponentes = buscaBoletimEscolarDosAlunosSaidaDTO.QtComponentes,
                            qualidade = buscaBoletimEscolarDosAlunosSaidaDTO.qualidade,
                            recomendacaoAluno = buscaBoletimEscolarDosAlunosSaidaDTO.recomendacaoAluno,
                            recomendacaoResponsavel = buscaBoletimEscolarDosAlunosSaidaDTO.recomendacaoResponsavel,
                            recuperacaoParalela = buscaBoletimEscolarDosAlunosSaidaDTO.recuperacaoParalela,
                            Status = buscaBoletimEscolarDosAlunosSaidaDTO.Status,
                            StatusDescription = buscaBoletimEscolarDosAlunosSaidaDTO.StatusDescription,
                            tci_exibirBoletim = buscaBoletimEscolarDosAlunosSaidaDTO.tci_exibirBoletim,
                            tci_id = buscaBoletimEscolarDosAlunosSaidaDTO.tci_id,
                            tci_layout = buscaBoletimEscolarDosAlunosSaidaDTO.tci_layout,
                            tci_nome = buscaBoletimEscolarDosAlunosSaidaDTO.tci_nome,
                            territorioSaber = buscaBoletimEscolarDosAlunosSaidaDTO.territorioSaber,
                            todasDisciplinas = buscaBoletimEscolarDosAlunosSaidaDTO.todasDisciplinas,
                            tur_codigo = buscaBoletimEscolarDosAlunosSaidaDTO.tur_codigo,
                            tur_id = buscaBoletimEscolarDosAlunosSaidaDTO.tur_id,
                            uad_nome = buscaBoletimEscolarDosAlunosSaidaDTO.uad_nome,
                            justificativaAbonoFalta = string.IsNullOrEmpty(buscaBoletimEscolarDosAlunosSaidaDTO.justificativaAbonoFalta) ? string.Empty : string.Format(mensagemJustificativaAbonoFalta, buscaBoletimEscolarDosAlunosSaidaDTO.justificativaAbonoFalta),
                            ensinoInfantil = buscaBoletimEscolarDosAlunosSaidaDTO.ensinoInfantil
                        };

                        //Adiciona a segunda variável na lista para "duplicar" o boletim do aluno, dividido em 2
                        list.Add(buscaBoletimEscolarDosAlunosSaidaDTO2);
                    }
                    else
                    {
                        list.Add(buscaBoletimEscolarDosAlunosSaidaDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                list.Clear();

                throw ex;
            }
            return list;
        }

        #endregion BoletimEscolar

        #region Servicos

        /// <summary>
        /// Seleciona a fila de alunoFechamentoPendencia do serviço
        /// </summary>
        /// <returns></returns>
        public static List<FilaAlunoFechamentoPendenciaDTO> SelecionarFilaAlunoFechamentoPendenciaAPI()
        {
            List<FilaAlunoFechamentoPendenciaDTO> lst = new List<FilaAlunoFechamentoPendenciaDTO>();
            DataTable dt = CLS_AlunoFechamentoPendenciaBO.SelecionaFila_PorSituacao();
            lst = dt.AsEnumerable().Select(r => new FilaAlunoFechamentoPendenciaDTO
                                                {
                                                    Situacao = r["Situacao"].ToString(),
                                                    QtDisciplinas = Convert.ToInt32(r["QtDisciplinas"])
                                                }).ToList();
            return lst;
        }

        /// <summary>
        /// Seleciona o status dos serviços configurados
        /// </summary>
        /// <returns></returns>
        public static List<StatusServicosDTO> SelecionarStatusServicosAPI()
        {
            List<StatusServicosDTO> lst = new List<StatusServicosDTO>();
            DataTable dt = SYS_ServicosBO.SelectStatus();
            lst = dt.AsEnumerable().Select(r => new StatusServicosDTO
                                                {
                                                    ser_nomeProcedimento = r["ser_nomeProcedimento"].ToString(),
                                                    Situacao = r["Situacao"].ToString(),
                                                    PREV_FIRE_TIME = r["PREV_FIRE_TIME"].ToString(),
                                                    NEXT_FIRE_TIME = r["NEXT_FIRE_TIME"].ToString(),
                                                    ser_nome = r["ser_nome"].ToString()
                                                }).ToList();
            return lst;
        }

        #endregion

        #region TurmaHorario

        /// <summary>
        /// Salva os dados informados no arquivo.
        /// </summary>
        /// <param name="dados">Lista TUR_TurmaHorario(conteúdo JSon enviado no post)</param>
        /// <returns></returns>
        public static List<TUR_TurmaHorarioDTO> SalvarTurmaHorario(string Json)
        {
            List<TUR_TurmaHorarioDTO> lista = new List<TUR_TurmaHorarioDTO>();
            TalkDBTransaction banco = new TUR_TurmaHorarioDAO()._Banco;

            try
            {
                JArray listaDados = (JArray.Parse(Json) ?? new JArray());

                lista = (from item in listaDados.AsEnumerable()
                         select (TUR_TurmaHorarioDTO)JsonConvert.DeserializeObject<TUR_TurmaHorarioDTO>(item.ToString()))
                        .Where(t => t.tud_id > 0 && t.trh_id > 0 && t.trn_id > 0).ToList();

                banco.Open();

                TUR_TurmaHorarioBO.SalvarTurmaHorarioAPI(lista, banco);
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }

            return lista;
        }


        #endregion

        #endregion Geral - Sistema Gestão Escolar

        #region Plataforma de Itens e Avaliações

        /// <summary>
        /// Retorna lista de escola que o usuario tem permissao e sao da UAD
        /// </summary>
        /// <returns></returns>
        public static BuscaAlunosMatriculadosTurmaSaidaDTO BuscaAlunosMatriculadosTurma(BuscaAlunosMatriculadosTurmaEntradaDTO buscaAlunosMatriculadosTurmaEntradaDTO)
        {
            BuscaAlunosMatriculadosTurmaSaidaDTO buscaAlunosMatriculadosTurmaSaidaDTO;
            try
            {
                buscaAlunosMatriculadosTurmaSaidaDTO = new BuscaAlunosMatriculadosTurmaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable alunosMatriculados = dao.BuscaAlunosMatriculadosTurma(buscaAlunosMatriculadosTurmaEntradaDTO.tur_id);

                List<MatriculaTurma> ListMatriculados = new List<MatriculaTurma>();

                foreach (DataRow dr in alunosMatriculados.Rows)
                {
                    MatriculaTurma mtu = new MatriculaTurma();
                    mtu.mtu_id = Convert.ToInt32(dr["mtu_id"]);
                    mtu.mtr_numeroMatricula = (dr["mtr_numeroMatricula"]).ToString();

                    PES_PessoaDTO.PessoaDadosBasicos pes = new PES_PessoaDTO.PessoaDadosBasicos();

                    pes.pes_id = new Guid((dr["pes_id"]).ToString());
                    pes.pes_nome = (dr["pes_nome"]).ToString();

                    mtu.Pessoa = pes;

                    ListMatriculados.Add(mtu);
                }

                buscaAlunosMatriculadosTurmaSaidaDTO.MatriculaTurma = ListMatriculados;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaAlunosMatriculadosTurmaSaidaDTO = new BuscaAlunosMatriculadosTurmaSaidaDTO();
                buscaAlunosMatriculadosTurmaSaidaDTO.Status = 1;
                buscaAlunosMatriculadosTurmaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaAlunosMatriculadosTurmaSaidaDTO;
        }

        /// <summary>
        /// Retorna a ultima movimentacao dos alunos que passaram por uma determinada escola em um determinado ano
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="matriculaEstadual">Se é matricula estadual ou não</param>
        /// <param name="tme_id">Id da modalidade de ensino no PEJA</param>
        /// <returns>Movimentacoes</returns>
        public static BuscaCalendariosSaidaDTO BuscaCalendarios(BuscaCalendarioEntidadeEntradaDTO buscaCalendarioEntidadeEntradaDTO)
        {
            BuscaCalendariosSaidaDTO buscaCalendariosSaidaDTO;
            try
            {
                buscaCalendariosSaidaDTO = new BuscaCalendariosSaidaDTO();

                DataTable calendarios = ACA_CalendarioPeriodoBO.BuscaCalendariosEntidade(buscaCalendarioEntidadeEntradaDTO.Ent_id);
                List<Calendario> listCalendarios = new List<Calendario>();

                List<Calendario> listAux =
                    (from DataRow dr in calendarios.Rows
                     group dr by Convert.ToInt32(dr["cal_id"]) into g
                     select new Calendario
                     {
                         Cal_id = g.Key
                          ,
                         Tpc_dataSincronizacao = DateTime.Now
                          ,
                         Periodos =
                             (
                                from DataRow linhaPeriodo in g
                                select (ACA_CalendarioPeriodoDTO)
                                    GestaoEscolarUtilBO.DataRowToEntity(linhaPeriodo, new ACA_CalendarioPeriodoDTO())
                             ).ToList()
                     }).ToList();

                foreach (Calendario linha in listAux)
                {
                    DataTable niveis = ORC_NivelBO.BuscaNiveisCalendario(linha.Cal_id);
                    List<Nivel> listNiveis = new List<Nivel>();
                    foreach (DataRow drNivel in niveis.Rows)
                    {
                        Nivel nivel = new Nivel();
                        nivel.Cal_id = linha.Cal_id;
                        nivel.Crp_id = Convert.ToInt32(drNivel["crp_id"]);
                        nivel.Crr_id = Convert.ToInt32(drNivel["crr_id"]);
                        nivel.Cur_id = Convert.ToInt32(drNivel["cur_id"]);
                        nivel.Crp_ordem = Convert.ToInt32(drNivel["crp_ordem"]);
                        nivel.Nvl_id = Convert.ToInt32(drNivel["nvl_id"]);
                        nivel.Nvl_nome = drNivel["nvl_nome"].ToString();
                        nivel.Nvl_ordem = Convert.ToInt32(drNivel["nvl_ordem"]);
                        nivel.Nvl_dataSincronizacao = DateTime.Now;

                        List<OrientacaoCurricular> listOrientacaoCurricular = new List<OrientacaoCurricular>();
                        DataTable orientacoesCurriculares = ORC_OrientacaoCurricularBO.SelecionaPorNivel(nivel.Nvl_id);
                        foreach (DataRow drOC in orientacoesCurriculares.Rows)
                        {
                            OrientacaoCurricular oc = new OrientacaoCurricular();
                            oc.Nvl_id = nivel.Nvl_id;
                            oc.Ocr_dataSincronizacao = DateTime.Now;
                            oc.Ocr_descricao = drOC["ocr_descricao"].ToString();
                            oc.Ocr_id = Convert.ToInt32(drOC["ocr_id"]);
                            oc.Ocr_codigo = drOC["ocr_codigo"].ToString();
                            if (!String.IsNullOrEmpty(drOC["ocr_idSuperior"].ToString()))
                                oc.Ocr_idSuperior = Convert.ToInt32(drOC["ocr_idSuperior"]);
                            oc.Tds_id = Convert.ToInt32(drOC["tds_id"]);

                            listOrientacaoCurricular.Add(oc);
                        }

                        nivel.OrientacoesCurriculares = listOrientacaoCurricular;
                        listNiveis.Add(nivel);
                    }
                    linha.Niveis = listNiveis;

                    listCalendarios.Add(linha);
                }
                buscaCalendariosSaidaDTO.Calendarios = listCalendarios;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaCalendariosSaidaDTO = new BuscaCalendariosSaidaDTO();
                buscaCalendariosSaidaDTO.Status = 1;
                buscaCalendariosSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaCalendariosSaidaDTO;
        }

        /// <summary>
        /// Retorna os curriculo periodos(serie) de acordo com entidade, curso e curriculo
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cur_id">Curso</param>
        /// <param name="crr_id">Curriculo</param>
        public static BuscaCurriculoPeriodoSaidaDTO BuscaCurriculoPeriodoPorEntidadeCursoCurriculo(BuscaCurriculoPeriodoEntradaDTO buscaCurriculoPeriodoEntradaDTO)
        {
            BuscaCurriculoPeriodoSaidaDTO buscaCurriculoPeriodoSaidaDTO;
            try
            {
                buscaCurriculoPeriodoSaidaDTO = new BuscaCurriculoPeriodoSaidaDTO();

                DataTable curriculoPeriodo = ACA_CurriculoPeriodoBO.BuscaCurriculoPeriodoPorEntidadeCursoCurriculo(
                    buscaCurriculoPeriodoEntradaDTO.Ent_id,
                    buscaCurriculoPeriodoEntradaDTO.cur_id,
                    buscaCurriculoPeriodoEntradaDTO.crr_id
                    );
                List<CurriculoPeriodo> listCurriculoPeriodo = new List<CurriculoPeriodo>();
                foreach (DataRow dr in curriculoPeriodo.Rows)
                {
                    CurriculoPeriodo crp = new CurriculoPeriodo();
                    crp.cur_id = Convert.ToInt32(dr["cur_id"]);
                    crp.crr_id = Convert.ToInt32(dr["crr_id"]);
                    crp.crp_id = Convert.ToInt32(dr["crp_id"]);
                    crp.crp_descricao = dr["crp_descricao"].ToString();

                    listCurriculoPeriodo.Add(crp);
                }
                buscaCurriculoPeriodoSaidaDTO.CurriculoPeriodo = listCurriculoPeriodo;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaCurriculoPeriodoSaidaDTO = new BuscaCurriculoPeriodoSaidaDTO();
                buscaCurriculoPeriodoSaidaDTO.Status = 1;
                buscaCurriculoPeriodoSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaCurriculoPeriodoSaidaDTO;
        }

        /// <summary>
        /// Retorna os curriculo periodos(serie) de acordo com entidade, curso e curriculo
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cur_id">Curso</param>
        /// <param name="crr_id">Curriculo</param>
        /// <param name="esc_id">Escola</param>
        public static BuscaCurriculoPeriodoSaidaDTO BuscaCurriculoPeriodoPorEntidadeCursoCurriculoEscola(BuscaCurriculoPeriodoEntradaDTO buscaCurriculoPeriodoEntradaDTO)
        {
            BuscaCurriculoPeriodoSaidaDTO buscaCurriculoPeriodoSaidaDTO;
            try
            {
                buscaCurriculoPeriodoSaidaDTO = new BuscaCurriculoPeriodoSaidaDTO();

                DataTable curriculoPeriodo = ACA_CurriculoPeriodoBO.BuscaCurriculoPeriodoPorEntidadeCursoCurriculoEscola(
                    buscaCurriculoPeriodoEntradaDTO.Ent_id,
                    buscaCurriculoPeriodoEntradaDTO.cur_id,
                    buscaCurriculoPeriodoEntradaDTO.crr_id,
                    buscaCurriculoPeriodoEntradaDTO.esc_id
                    );
                List<CurriculoPeriodo> listCurriculoPeriodo = new List<CurriculoPeriodo>();
                foreach (DataRow dr in curriculoPeriodo.Rows)
                {
                    CurriculoPeriodo crp = new CurriculoPeriodo();
                    crp.cur_id = Convert.ToInt32(dr["cur_id"]);
                    crp.crr_id = Convert.ToInt32(dr["crr_id"]);
                    crp.crp_id = Convert.ToInt32(dr["crp_id"]);
                    crp.crp_descricao = dr["crp_descricao"].ToString();

                    listCurriculoPeriodo.Add(crp);
                }
                buscaCurriculoPeriodoSaidaDTO.CurriculoPeriodo = listCurriculoPeriodo;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaCurriculoPeriodoSaidaDTO = new BuscaCurriculoPeriodoSaidaDTO();
                buscaCurriculoPeriodoSaidaDTO.Status = 1;
                buscaCurriculoPeriodoSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaCurriculoPeriodoSaidaDTO;
        }

        /// <summary>
        /// Retorna os cursos de acordo com entidade e calendario
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cal_id">Calendario</param>
        public static BuscaCursosSaidaDTO BuscaCursosPorEntidadeCalendario(BuscaCursosEntradaDTO buscaCursosEntCalEntradaDTO)
        {
            BuscaCursosSaidaDTO buscaCursosSaidaDTO;
            try
            {
                buscaCursosSaidaDTO = new BuscaCursosSaidaDTO();

                DataTable cursos = ACA_CursoBO.BuscaCursoPorEntidadeCalendario(buscaCursosEntCalEntradaDTO.Ent_id, buscaCursosEntCalEntradaDTO.cal_id);
                List<Curso> listCursos = new List<Curso>();
                foreach (DataRow dr in cursos.Rows)
                {
                    Curso curso = new Curso();
                    curso.cur_id = Convert.ToInt32(dr["cur_id"]);
                    curso.cur_nome = dr["cur_nome"].ToString();
                    curso.crr_id = Convert.ToInt32(dr["crr_id"]);

                    listCursos.Add(curso);
                }
                buscaCursosSaidaDTO.Cursos = listCursos;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaCursosSaidaDTO = new BuscaCursosSaidaDTO();
                buscaCursosSaidaDTO.Status = 1;
                buscaCursosSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaCursosSaidaDTO;
        }

        /// <summary>
        /// Retorna os cursos de acordo com entidade, calendario e escola
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="esc_id">Escola</param>
        public static BuscaCursosSaidaDTO BuscaCursosPorEntidadeCalendarioEscola(BuscaCursosEntradaDTO buscaCursosEntradaDTO)
        {
            BuscaCursosSaidaDTO buscaCursosSaidaDTO;
            try
            {
                buscaCursosSaidaDTO = new BuscaCursosSaidaDTO();

                DataTable cursos = ACA_CursoBO.BuscaCursoPorEntidadeCalendarioEscola(
                    buscaCursosEntradaDTO.Ent_id,
                    buscaCursosEntradaDTO.cal_id,
                    buscaCursosEntradaDTO.esc_id
                    );
                List<Curso> listCursos = new List<Curso>();
                foreach (DataRow dr in cursos.Rows)
                {
                    Curso curso = new Curso();
                    curso.cur_id = Convert.ToInt32(dr["cur_id"]);
                    curso.cur_nome = dr["cur_nome"].ToString();
                    curso.crr_id = Convert.ToInt32(dr["crr_id"]);

                    listCursos.Add(curso);
                }
                buscaCursosSaidaDTO.Cursos = listCursos;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaCursosSaidaDTO = new BuscaCursosSaidaDTO();
                buscaCursosSaidaDTO.Status = 1;
                buscaCursosSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaCursosSaidaDTO;
        }

        /// <summary>
        /// Retorna dados do docente pelo pes_id
        /// </summary>
        /// <param name="ent_id">ID da pessoa</param>
        /// <returns></returns>
        public static BuscaDadosDocenteSaidaDTO BuscaDadosDocente(BuscaDadosDocenteEntradaDTO buscaDadosDocenteEntradaDTO)
        {
            BuscaDadosDocenteSaidaDTO buscaDadosDocenteSaidaDTO;
            try
            {
                buscaDadosDocenteSaidaDTO = new BuscaDadosDocenteSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable dadosDocente = dao.BuscaDadosDocente(buscaDadosDocenteEntradaDTO.pes_id);

                buscaDadosDocenteSaidaDTO.Docente = (from dr in dadosDocente.AsEnumerable() select (Docente)GestaoEscolarUtilBO.DataRowToEntity(dr, new Docente())).ToList().FirstOrDefault();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaDadosDocenteSaidaDTO = new BuscaDadosDocenteSaidaDTO();
                buscaDadosDocenteSaidaDTO.Status = 1;
                buscaDadosDocenteSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaDadosDocenteSaidaDTO;
        }

        /// <summary>
        /// Retorna usuario e nome de docentes da turma
        /// </summary>
        /// <returns></returns>
        public static BuscaDocentesTurmaSaidaDTO BuscaDadosDocentePorTurma(BuscaDocentesTurmaEntradaDTO buscaDocentesTurmaEntradaDTO)
        {
            BuscaDocentesTurmaSaidaDTO buscaDocentesTurmaSaidaDTO;
            try
            {
                buscaDocentesTurmaSaidaDTO = new BuscaDocentesTurmaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable docenteUsu = dao.BuscaDadosDocentePorTurma(
                    buscaDocentesTurmaEntradaDTO.tur_id
                    );

                buscaDocentesTurmaSaidaDTO.Docentes = (from dr in docenteUsu.AsEnumerable() select (Docente)GestaoEscolarUtilBO.DataRowToEntity(dr, new Docente())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaDocentesTurmaSaidaDTO = new BuscaDocentesTurmaSaidaDTO();
                buscaDocentesTurmaSaidaDTO.Status = 1;
                buscaDocentesTurmaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaDocentesTurmaSaidaDTO;
        }

        /// <summary>
        /// Retorna dados da turma filtrada
        /// </summary>
        /// <returns></returns>
        public static BuscaTurmasPlataformaSaidaDTO BuscaDadosTurma(BuscaDadosTurmasEntradaDTO buscaDadosTurmaEntradaDTO)
        {
            BuscaTurmasPlataformaSaidaDTO buscaTurmasPlataformaSaidaDTO;
            try
            {
                buscaTurmasPlataformaSaidaDTO = new BuscaTurmasPlataformaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable dadosTurmas = dao.BuscaDadosTurma(
                    buscaDadosTurmaEntradaDTO.tur_id
                    );

                Turma tur = new Turma();
                tur.Ent_id = new Guid(dadosTurmas.Rows[0]["ent_id"].ToString());
                tur.Tur_id = Convert.ToInt64(dadosTurmas.Rows[0]["tur_id"]);
                tur.Tur_codigo = (dadosTurmas.Rows[0]["tur_codigo"]).ToString();
                tur.esc_id = Convert.ToInt32(dadosTurmas.Rows[0]["esc_id"]);
                tur.esc_nome = (dadosTurmas.Rows[0]["esc_nome"]).ToString();
                tur.trn_id = Convert.ToInt32(dadosTurmas.Rows[0]["trn_id"]);
                tur.trn_descricao = (dadosTurmas.Rows[0]["trn_descricao"]).ToString();
                tur.cur_nome = (dadosTurmas.Rows[0]["cur_nome"]).ToString();
                tur.cur_id = Convert.ToInt32(dadosTurmas.Rows[0]["cur_id"]);
                tur.crr_id = Convert.ToInt32(dadosTurmas.Rows[0]["crr_id"]);
                tur.crp_id = Convert.ToInt32(dadosTurmas.Rows[0]["crp_id"]);
                tur.Crp_descricao = (dadosTurmas.Rows[0]["crp_descricao"]).ToString();

                buscaTurmasPlataformaSaidaDTO.Turma = tur;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaTurmasPlataformaSaidaDTO = new BuscaTurmasPlataformaSaidaDTO();
                buscaTurmasPlataformaSaidaDTO.Status = 1;
                buscaTurmasPlataformaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTurmasPlataformaSaidaDTO;
        }

        /// <summary>
        /// Retorna turma com ou nao permissao de usuario
        /// </summary>
        /// <returns></returns>
        public static BuscaTurmasSaidaDTO BuscaDadosTurmas(BuscaDadosTurmasEntradaDTO buscaDadosTurmaEntradaDTO)
        {
            BuscaTurmasSaidaDTO buscaTurmasSaidaDTO;
            try
            {
                buscaTurmasSaidaDTO = new BuscaTurmasSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable dadosTurmas = dao.BuscaDadosTurmas(
                    buscaDadosTurmaEntradaDTO.Ent_id,
                    buscaDadosTurmaEntradaDTO.cal_id,
                    buscaDadosTurmaEntradaDTO.uad_idSuperior,
                    buscaDadosTurmaEntradaDTO.esc_id,
                    buscaDadosTurmaEntradaDTO.cur_id,
                    buscaDadosTurmaEntradaDTO.crr_id,
                    buscaDadosTurmaEntradaDTO.crp_id,
                    buscaDadosTurmaEntradaDTO.trn_id,
                    buscaDadosTurmaEntradaDTO.tur_codigo,
                    buscaDadosTurmaEntradaDTO.usu_id,
                    buscaDadosTurmaEntradaDTO.gru_id
                    );

                List<Turma> ListDadosTurmas = new List<Turma>();

                foreach (DataRow dr in dadosTurmas.Rows)
                {
                    Turma tur = new Turma();
                    tur.Ent_id = new Guid(dr["ent_id"].ToString());
                    tur.Tur_id = Convert.ToInt64(dr["tur_id"]);
                    tur.Tur_codigo = (dr["tur_codigo"]).ToString();
                    tur.esc_id = Convert.ToInt32(dr["esc_id"]);
                    tur.esc_nome = (dr["esc_nome"]).ToString();
                    tur.Uad_id = new Guid(dr["uad_id"].ToString());
                    tur.trn_id = Convert.ToInt32(dr["trn_id"]);
                    tur.trn_descricao = (dr["trn_descricao"]).ToString();
                    tur.cur_nome = (dr["cur_nome"]).ToString();
                    tur.cur_id = Convert.ToInt32(dr["cur_id"]);
                    tur.crr_id = Convert.ToInt32(dr["crr_id"]);
                    tur.crp_id = Convert.ToInt32(dr["crp_id"]);
                    tur.Crp_descricao = (dr["crp_descricao"]).ToString();

                    ListDadosTurmas.Add(tur);
                }

                buscaTurmasSaidaDTO.Turmas = ListDadosTurmas;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaTurmasSaidaDTO = new BuscaTurmasSaidaDTO();
                buscaTurmasSaidaDTO.Status = 1;
                buscaTurmasSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTurmasSaidaDTO;
        }

        /// <summary>
        /// Retorna turma com permissao do docente
        /// </summary>
        /// <returns></returns>
        public static BuscaTurmasSaidaDTO BuscaDadosTurmasDocente(BuscaDadosTurmasEntradaDTO buscaDadosTurmaEntradaDTO)
        {
            BuscaTurmasSaidaDTO buscaTurmasSaidaDTO;
            try
            {
                buscaTurmasSaidaDTO = new BuscaTurmasSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable dadosTurmas = dao.BuscaDadosTurmasDocente(
                    buscaDadosTurmaEntradaDTO.Ent_id,
                    buscaDadosTurmaEntradaDTO.doc_id
                    );

                List<Turma> ListDadosTurmas = new List<Turma>();

                foreach (DataRow dr in dadosTurmas.Rows)
                {
                    Turma tur = new Turma();
                    tur.Ent_id = new Guid(dr["ent_id"].ToString());
                    tur.Tur_id = Convert.ToInt64(dr["tur_id"]);
                    tur.Tur_codigo = (dr["tur_codigo"]).ToString();
                    tur.esc_id = Convert.ToInt32(dr["esc_id"]);
                    tur.esc_nome = (dr["esc_nome"]).ToString();
                    tur.trn_id = Convert.ToInt32(dr["trn_id"]);
                    tur.trn_descricao = (dr["trn_descricao"]).ToString();
                    tur.cur_nome = (dr["cur_nome"]).ToString();
                    tur.cur_id = Convert.ToInt32(dr["cur_id"]);
                    tur.crr_id = Convert.ToInt32(dr["crr_id"]);
                    tur.crp_id = Convert.ToInt32(dr["crp_id"]);
                    tur.Crp_descricao = (dr["crp_descricao"]).ToString();

                    ListDadosTurmas.Add(tur);
                }

                buscaTurmasSaidaDTO.Turmas = ListDadosTurmas;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaTurmasSaidaDTO = new BuscaTurmasSaidaDTO();
                buscaTurmasSaidaDTO.Status = 1;
                buscaTurmasSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTurmasSaidaDTO;
        }

        /// <summary>
        /// Retorna disciplinas do docente
        /// </summary>
        /// <returns></returns>
        public static BuscaTurmaDisciplinaSaidaDTO BuscaDocenteDisciplinas(BuscaTurmaDisciplinaEntradaDTO buscaTurmaDisciplinaEntradaDTO)
        {
            BuscaTurmaDisciplinaSaidaDTO buscaTurmaDisciplinaSaidaDTO;
            try
            {
                buscaTurmaDisciplinaSaidaDTO = new BuscaTurmaDisciplinaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable turmaDisciplina = dao.BuscaDocenteDisciplinas(
                    buscaTurmaDisciplinaEntradaDTO.doc_id
                    );

                buscaTurmaDisciplinaSaidaDTO.TipoDisciplinas = (from dr in turmaDisciplina.AsEnumerable() select (ACA_TipoDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoDisciplinaDTO())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaTurmaDisciplinaSaidaDTO = new BuscaTurmaDisciplinaSaidaDTO();
                buscaTurmaDisciplinaSaidaDTO.Status = 1;
                buscaTurmaDisciplinaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTurmaDisciplinaSaidaDTO;
        }

        /// <summary>
        /// Retorna lista de escola que o usuario tem permissao
        /// </summary>
        /// <returns></returns>
        public static BuscaEscolasPorUsuarioSaidaDTO BuscaEscolasPorPermissaoUsuario(BuscaEscolasPorUsuarioEntradaDTO buscaEscolasPorUsuarioEntradaDTO)
        {
            BuscaEscolasPorUsuarioSaidaDTO buscaEscolasPorUsuarioSaidaDTO;
            try
            {
                buscaEscolasPorUsuarioSaidaDTO = new BuscaEscolasPorUsuarioSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable dadosDocente = dao.BuscaEscolasPorPermissaoUsuario(
                    buscaEscolasPorUsuarioEntradaDTO.ent_id,
                    buscaEscolasPorUsuarioEntradaDTO.usu_id,
                    buscaEscolasPorUsuarioEntradaDTO.gru_id);

                List<ESC_EscolaDTO.EscolaEndereco> listEscola = new List<ESC_EscolaDTO.EscolaEndereco>();
                foreach (DataRow dr in dadosDocente.Rows)
                {
                    ESC_EscolaDTO.EscolaEndereco esc = new ESC_EscolaDTO.EscolaEndereco();
                    esc.esc_id = Convert.ToInt32(dr["esc_id"]);
                    esc.esc_nome = (dr["esc_nome"]).ToString();
                    esc.esc_codigo = (dr["esc_codigo"]).ToString();

                    listEscola.Add(esc);
                }
                buscaEscolasPorUsuarioSaidaDTO.Escolas = listEscola;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaEscolasPorUsuarioSaidaDTO = new BuscaEscolasPorUsuarioSaidaDTO();
                buscaEscolasPorUsuarioSaidaDTO.Status = 1;
                buscaEscolasPorUsuarioSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaEscolasPorUsuarioSaidaDTO;
        }

        /// <summary>
        /// Retorna lista de escolas por entidade
        /// </summary>
        /// <returns></returns>
        public static BuscaEscolasEntidadeSaidaDTO BuscaEscolasEntidade(BuscaEscolasEntidadeEntradaDTO buscaEscolasEntidadeEntradaDTO)
        {
            BuscaEscolasEntidadeSaidaDTO buscaEscolasEntidadeSaidaDTO;
            try
            {
                buscaEscolasEntidadeSaidaDTO = new BuscaEscolasEntidadeSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable dadosEscolas = dao.BuscaEscolasEntidade(
                    buscaEscolasEntidadeEntradaDTO.ent_id,
                    buscaEscolasEntidadeEntradaDTO.uad_id);

                List<EscolaEntidade> listEscola = new List<EscolaEntidade>();
                foreach (DataRow dr in dadosEscolas.Rows)
                {
                    EscolaEntidade esc = new EscolaEntidade();
                    esc.ent_id = buscaEscolasEntidadeEntradaDTO.ent_id;
                    esc.uad_id = new Guid(dr["uad_id"].ToString());
                    esc.esc_id = Convert.ToInt32(dr["esc_id"]);
                    esc.esc_nome = (dr["esc_nome"]).ToString();
                    esc.esc_codigo = (dr["esc_codigo"]).ToString();
                    esc.tre_id = Convert.ToInt32(dr["tre_id"]);
                    esc.tre_nome = (dr["tre_nome"]).ToString();

                    listEscola.Add(esc);
                }
                buscaEscolasEntidadeSaidaDTO.Escolas = listEscola;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaEscolasEntidadeSaidaDTO = new BuscaEscolasEntidadeSaidaDTO();
                buscaEscolasEntidadeSaidaDTO.Status = 1;
                buscaEscolasEntidadeSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaEscolasEntidadeSaidaDTO;
        }

        /// <summary>
        /// Retorna lista de escola que o usuario tem permissao e sao da UAD
        /// </summary>
        /// <returns></returns>
        public static BuscaEscolasPorUsuarioSaidaDTO BuscaEscolasPorPermissaoUsuarioUnidadeAdm(BuscaEscolasPorUsuarioEntradaDTO buscaEscolasPorUsuarioEntradaDTO)
        {
            BuscaEscolasPorUsuarioSaidaDTO buscaEscolasPorUsuarioSaidaDTO;
            try
            {
                buscaEscolasPorUsuarioSaidaDTO = new BuscaEscolasPorUsuarioSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable dadosDocente = dao.BuscaEscolasPorPermissaoUsuarioUnidadeAdm(
                    buscaEscolasPorUsuarioEntradaDTO.ent_id,
                    buscaEscolasPorUsuarioEntradaDTO.usu_id,
                    buscaEscolasPorUsuarioEntradaDTO.gru_id,
                    buscaEscolasPorUsuarioEntradaDTO.uad_idSuperior);

                List<ESC_EscolaDTO.EscolaEndereco> listEscola = new List<ESC_EscolaDTO.EscolaEndereco>();
                foreach (DataRow dr in dadosDocente.Rows)
                {
                    ESC_EscolaDTO.EscolaEndereco esc = new ESC_EscolaDTO.EscolaEndereco();
                    esc.esc_id = Convert.ToInt32(dr["esc_id"]);
                    esc.esc_nome = (dr["esc_nome"]).ToString();
                    esc.esc_codigo = (dr["esc_codigo"]).ToString();

                    listEscola.Add(esc);
                }
                buscaEscolasPorUsuarioSaidaDTO.Escolas = listEscola;
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaEscolasPorUsuarioSaidaDTO = new BuscaEscolasPorUsuarioSaidaDTO();
                buscaEscolasPorUsuarioSaidaDTO.Status = 1;
                buscaEscolasPorUsuarioSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaEscolasPorUsuarioSaidaDTO;
        }

        /// <summary>
        /// Retorna todos os tipos de justificativa falta
        /// </summary>
        /// <returns></returns>
        public static BuscaTipoJustificativaFaltaSaidaDTO BuscarTiposJustificativaFalta()
        {
            BuscaTipoJustificativaFaltaSaidaDTO buscaTipoJustificativaFaltaSaidaDTO;
            try
            {
                buscaTipoJustificativaFaltaSaidaDTO = new BuscaTipoJustificativaFaltaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable tiposJustificativaFalta = dao.BuscarTiposJustificativaFalta();

                buscaTipoJustificativaFaltaSaidaDTO.TipoJustificativaFalta = (from dr in tiposJustificativaFalta.AsEnumerable() select (TipoJustificativaFalta)GestaoEscolarUtilBO.DataRowToEntity(dr, new TipoJustificativaFalta())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaTipoJustificativaFaltaSaidaDTO = new BuscaTipoJustificativaFaltaSaidaDTO();
                buscaTipoJustificativaFaltaSaidaDTO.Status = 1;
                buscaTipoJustificativaFaltaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTipoJustificativaFaltaSaidaDTO;
        }

        /// <summary>
        /// Retorna todos os tipos de justificativa falta
        /// </summary>
        /// <returns></returns>
        public static BuscaTipoNivelEnsinoSaidaDTO BuscarTiposNivelEnsino()
        {
            BuscaTipoNivelEnsinoSaidaDTO buscaTipoNivelEnsinoSaidaDTO;
            try
            {
                buscaTipoNivelEnsinoSaidaDTO = new BuscaTipoNivelEnsinoSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable tiposNivelEnsino = dao.BuscarTiposNivelEnsino();

                buscaTipoNivelEnsinoSaidaDTO.TipoNivelEnsino = (from dr in tiposNivelEnsino.AsEnumerable() select (TipoNivelEnsino)GestaoEscolarUtilBO.DataRowToEntity(dr, new TipoNivelEnsino())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaTipoNivelEnsinoSaidaDTO = new BuscaTipoNivelEnsinoSaidaDTO();
                buscaTipoNivelEnsinoSaidaDTO.Status = 1;
                buscaTipoNivelEnsinoSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTipoNivelEnsinoSaidaDTO;
        }

        /// <summary>
        /// Retorna disciplinas da turma
        /// </summary>
        /// <returns></returns>
        public static BuscaTurmaDisciplinaSaidaDTO BuscaTurmaDisciplinas(BuscaTurmaDisciplinaEntradaDTO buscaTurmaDisciplinaEntradaDTO)
        {
            BuscaTurmaDisciplinaSaidaDTO buscaTurmaDisciplinaSaidaDTO;
            try
            {
                buscaTurmaDisciplinaSaidaDTO = new BuscaTurmaDisciplinaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable turmaDisciplina = dao.BuscaTurmaDisciplinas(
                    buscaTurmaDisciplinaEntradaDTO.tur_id
                    );

                buscaTurmaDisciplinaSaidaDTO.TipoDisciplinas = (from dr in turmaDisciplina.AsEnumerable() select (ACA_TipoDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoDisciplinaDTO())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaTurmaDisciplinaSaidaDTO = new BuscaTurmaDisciplinaSaidaDTO();
                buscaTurmaDisciplinaSaidaDTO.Status = 1;
                buscaTurmaDisciplinaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTurmaDisciplinaSaidaDTO;
        }

        /// <summary>
        /// Retorna disciplinas da turma e docente
        /// </summary>
        /// <returns></returns>
        public static BuscaTurmaDisciplinaSaidaDTO BuscaTurmaDocenteDisciplinas(BuscaTurmaDisciplinaEntradaDTO buscaTurmaDisciplinaEntradaDTO)
        {
            BuscaTurmaDisciplinaSaidaDTO buscaTurmaDisciplinaSaidaDTO;
            try
            {
                buscaTurmaDisciplinaSaidaDTO = new BuscaTurmaDisciplinaSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable turmaDisciplina = dao.BuscaTurmaDocenteDisciplinas(
                    buscaTurmaDisciplinaEntradaDTO.tur_id
                    , buscaTurmaDisciplinaEntradaDTO.doc_id
                    );

                buscaTurmaDisciplinaSaidaDTO.TipoDisciplinas = (from dr in turmaDisciplina.AsEnumerable() select (ACA_TipoDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoDisciplinaDTO())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaTurmaDisciplinaSaidaDTO = new BuscaTurmaDisciplinaSaidaDTO();
                buscaTurmaDisciplinaSaidaDTO.Status = 1;
                buscaTurmaDisciplinaSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTurmaDisciplinaSaidaDTO;
        }

        /// <summary>
        /// Retorna turnos filtrado pela entidade
        /// </summary>
        /// <returns></returns>
        public static BuscaTurnosSaidaDTO BuscaTurnos(BuscaTurnosEntradaDTO buscaTurnosEntradaDTO)
        {
            BuscaTurnosSaidaDTO buscaTurnosSaidaDTO;
            try
            {
                buscaTurnosSaidaDTO = new BuscaTurnosSaidaDTO();
                ApiDAO dao = new ApiDAO();

                DataTable turnos = dao.BuscaTurnos(
                    buscaTurnosEntradaDTO.Ent_id
                    );

                buscaTurnosSaidaDTO.Turnos = (from dr in turnos.AsEnumerable() select (Turno)GestaoEscolarUtilBO.DataRowToEntity(dr, new Turno())).ToList();
            }
            catch (Exception exp)
            {
                //_GravarErro(exp, sincronizacaoDiarioClasseEntradaDTO.GetProperties());

                buscaTurnosSaidaDTO = new BuscaTurnosSaidaDTO();
                buscaTurnosSaidaDTO.Status = 1;
                buscaTurnosSaidaDTO.StatusDescription = exp.Message;
            }
            return buscaTurnosSaidaDTO;
        }

        /// <summary>
        /// Busca dados de disciplinas mediante o tipo de nível de ensino
        /// </summary>
        /// <returns>Objeto BuscaTiposDisciplinaSaidaDTO</returns>
        public static BuscaDadosDisciplinasSaidaDTO BuscaDadosDisciplinas(BuscaDadosDisciplinasEntradaDTO buscaTiposDisciplinaEntradaDTO)
        {
            BuscaDadosDisciplinasSaidaDTO buscaDadosDisciplinaSaidaDTO;
            try
            {
                buscaDadosDisciplinaSaidaDTO = new BuscaDadosDisciplinasSaidaDTO();

                DataSet dados = new ApiDAO().BuscaDadosDisciplinas(buscaTiposDisciplinaEntradaDTO.tne_id);

                // Disciplinas
                DataTable disciplinas = dados.Tables[0];
                buscaDadosDisciplinaSaidaDTO.Disciplinas = (from dr in disciplinas.AsEnumerable()
                                                            select (Disciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new Disciplina())).ToList();

                // Tipos de disciplina
                DataTable tiposDisciplinas = dados.Tables[1];
                buscaDadosDisciplinaSaidaDTO.TiposDisciplina = (from dr in tiposDisciplinas.AsEnumerable()
                                                                select (ACA_TipoDisciplinaDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoDisciplinaDTO())).ToList();
            }
            catch (Exception ex)
            {
                buscaDadosDisciplinaSaidaDTO = new BuscaDadosDisciplinasSaidaDTO();
                buscaDadosDisciplinaSaidaDTO.Status = 1;
                buscaDadosDisciplinaSaidaDTO.StatusDescription = ex.Message;
            }

            return buscaDadosDisciplinaSaidaDTO;
        }

        /// <summary>
        /// Retorna todos os níveis de ensino, cursos e currículos mediante a entidade informada
        /// </summary>
        /// <returns>Objeto BuscaTiposDisciplinaSaidaDTO</returns>
        public static BuscaDadosCursosEntidadeSaidaDTO BuscaDadosCursosEntidade(BuscaDadosCursosEntidadeEntradaDTO buscaDadosEntidadeEntradaDTO)
        {
            BuscaDadosCursosEntidadeSaidaDTO buscaDadosEntidadeSaidaDTO;
            try
            {
                buscaDadosEntidadeSaidaDTO = new BuscaDadosCursosEntidadeSaidaDTO();

                DataSet dados = new ApiDAO().BuscaDadosCursosEntidade(buscaDadosEntidadeEntradaDTO.ent_id);

                // Cursos
                DataTable cursos = dados.Tables[0];
                buscaDadosEntidadeSaidaDTO.Cursos = (from dr in cursos.AsEnumerable()
                                                     select (Curso)GestaoEscolarUtilBO.DataRowToEntity(dr, new Curso())).ToList();
                // Curriculos
                DataTable curriculos = dados.Tables[1];
                buscaDadosEntidadeSaidaDTO.Curriculos = (from dr in curriculos.AsEnumerable()
                                                         select (Curriculo)GestaoEscolarUtilBO.DataRowToEntity(dr, new Curriculo())).ToList();

                // Curriculos Periodo
                DataTable curriculosPeriodo = dados.Tables[2];
                buscaDadosEntidadeSaidaDTO.CurriculosPeriodo = (from dr in curriculosPeriodo.AsEnumerable()
                                                                select (CurriculoPeriodo)GestaoEscolarUtilBO.DataRowToEntity(dr, new CurriculoPeriodo())).ToList();

                // Tipos de nível de ensino
                DataTable tiposNivelEnsino = dados.Tables[3];
                buscaDadosEntidadeSaidaDTO.TiposNivelEnsino = (from dr in tiposNivelEnsino.AsEnumerable()
                                                               select (TipoNivelEnsino)GestaoEscolarUtilBO.DataRowToEntity(dr, new TipoNivelEnsino())).ToList();
            }
            catch (Exception ex)
            {
                buscaDadosEntidadeSaidaDTO = new BuscaDadosCursosEntidadeSaidaDTO();
                buscaDadosEntidadeSaidaDTO.Status = 1;
                buscaDadosEntidadeSaidaDTO.StatusDescription = ex.Message;
            }

            return buscaDadosEntidadeSaidaDTO;
        }

        /// <summary>
        /// Retorna lista de descrição da série
        /// </summary>
        /// <returns></returns>
        public static List<ACA_TipoCurriculoPeriodoDTO> SelecionarTipoCurriculoPeriodo(int tme_id, int tne_id)
        {
            try
            {
                ApiDAO dao = new ApiDAO();
                DataTable dt = dao.SelecionarTipoCurriculoPeriodo(tme_id, tne_id);

                if (dt.Rows.Count == 0)
                    return null;

                List<ACA_TipoCurriculoPeriodoDTO> curriculos = (from dr in dt.AsEnumerable()
                                                                select (ACA_TipoCurriculoPeriodoDTO)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_TipoCurriculoPeriodoDTO())
                                                                ).ToList();

                return curriculos;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna lista de docentes por vínculo
        /// </summary>
        /// <returns></returns>
        public static List<ACA_DocenteDTO> SelecionarDocentesPorVinculoDeTrabalho(string psd_numeroCPF, string psd_numeroRG, Guid ent_id, string coc_matricula, bool coc_vinculoSede)
        {
            try
            {
                DataTable dtDocente = new ApiDAO().SelecionarDocentesPorVinculoDeTrabalho(psd_numeroCPF, psd_numeroRG, ent_id, coc_matricula, coc_vinculoSede);
                List<ACA_DocenteDTO> listaRetorno = new List<ACA_DocenteDTO>();
                if (dtDocente.Rows.Count > 0)
                {
                    // Delegate que retorna a DTO de docente.
                    Func<ACA_DocenteDTO, PES_PessoaDTO, IEnumerable<ESC_EscolaDTO>, ACA_DocenteDTO> retornaDocente =
                        delegate (ACA_DocenteDTO docente, PES_PessoaDTO pessoa, IEnumerable<ESC_EscolaDTO> listaEscola)
                        {
                            docente.pessoa = pessoa;
                            docente.listaEscola = listaEscola.ToList();
                            return docente;
                        };

                    listaRetorno = (from DataRow dr in dtDocente.Rows
                                    group dr by Convert.ToInt64(dr["doc_id"]) into grupo
                                    select retornaDocente
                                    (
                                        (ACA_DocenteDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new ACA_DocenteDTO())
                                        ,
                                        (PES_PessoaDTO)GestaoEscolarUtilBO.DataRowToEntity(grupo.First(), new PES_PessoaDTO())
                                        ,
                                        (from esc in grupo
                                         where !string.IsNullOrEmpty(esc["chr_id"].ToString())
                                         group esc by Convert.ToInt32(esc["chr_id"]) into gEscola
                                         select (ESC_EscolaDTO)GestaoEscolarUtilBO.DataRowToEntity(gEscola.First(), new ESC_EscolaDTO()
                                       ))
                                    )).ToList();
                }


                if (listaRetorno == null || listaRetorno.Count == 0)
                    return null;

                return listaRetorno;
            }
            catch
            {
                throw;
            }
        }

        #endregion Plataforma de Itens e Avaliações

        #endregion Métodos
    }
}