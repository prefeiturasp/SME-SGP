using System;
using System.Collections.Generic;
using System.Data;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.Data.Common;
using System.Linq;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Web;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estruturas

    /// <summary>
    /// Estrutura utilizada para armazenar a configuração de nova matriz curricular para uma turma.
    /// </summary>
    [Serializable]
    public struct TurmaDisciplinas
    {
        public long tur_id { get; set; }
        public int trn_id { get; set; }
        public int ttn_id { get; set; }
        public bool tur_docenteEspecialista { get; set; }
        public List<CadastroTurmaDisciplina> listaDiscplinas { get; set; }
    }

    #endregion

    public class TUR_TurmaCurriculoBO : BusinessBase<TUR_TurmaCurriculoDAO, TUR_TurmaCurriculo>
    {
        public const string chaveCache_GetSelectBy_Turma = "Cache_GetSelectBy_Turma";

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectBy_Turma(long tur_id)
        {
            return string.Format(chaveCache_GetSelectBy_Turma + "_{0}", tur_id);
        }

        /// <summary>
        /// Retorna as entidades da TurmaCurriculo cadastradas na turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static List<TUR_TurmaCurriculo> GetSelectBy_Turma
        (
            long tur_id
            , TalkDBTransaction banco
            , int appMinutosCacheLongo = 0
        )
        {
            List<TUR_TurmaCurriculo> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectBy_Turma(tur_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        TUR_TurmaCurriculoDAO dao = new TUR_TurmaCurriculoDAO { _Banco = banco };
                        DataTable dtDados = dao.SelectBy_Turma(tur_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select dao.DataRowToEntity(dr, new TUR_TurmaCurriculo())).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<TUR_TurmaCurriculo>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                TUR_TurmaCurriculoDAO dao = new TUR_TurmaCurriculoDAO { _Banco = banco };
                DataTable dtDados = dao.SelectBy_Turma(tur_id);
                dados = (from DataRow dr in dtDados.Rows
                         select dao.DataRowToEntity(dr, new TUR_TurmaCurriculo())).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna as entidades da TurmaCurriculo cadastradas na turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public static List<TUR_TurmaCurriculo> GetSelectBy_Turma
        (
            long tur_id
            , int appMinutosCacheLongo = 0
        )
        {
            TUR_TurmaCurriculoDAO dao = new TUR_TurmaCurriculoDAO();

            return GetSelectBy_Turma(tur_id, dao._Banco, appMinutosCacheLongo);
        }

        /// <summary>
        /// Seleciona a relação de turmas e cursos por escola e curso.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="apenasAtivos">Flag que indica se deve buscar apenas por relações ativas.</param>
        /// <returns></returns>
        public static List<TUR_TurmaCurriculo> SelecionaPorEscolaCurso(int esc_id, int uni_id, int cur_id, int crr_id, byte tcr_situacao, TalkDBTransaction banco = null)
        {
            return banco != null ?
                    new TUR_TurmaCurriculoDAO { _Banco = banco }.SelecionaPorEscolaCurso(esc_id, uni_id, cur_id, crr_id, tcr_situacao) :
                    new TUR_TurmaCurriculoDAO().SelecionaPorEscolaCurso(esc_id, uni_id, cur_id, crr_id, tcr_situacao);
        }
        
        /// <summary>
        /// O método altera a matriz curricular das turmas passadas por parâmetro.
        /// </summary>
        /// <param name="cur_idOrigem">ID do curso de origem.</param>
        /// <param name="crr_idOrigem">ID do currículo de origem.</param>
        /// <param name="cur_idDestino">ID do curso de destino.</param>
        /// <param name="crr_idDestino">ID do currículo de destino.</param>
        /// <param name="tur_ids">IDs das turmas.</param>
        /// <returns></returns>
        public static bool AlterarMatrizCurricularPorTurmas(int cur_idOrigem, int crr_idOrigem, int cur_idDestino, int crr_idDestino, string tur_ids, TalkDBTransaction banco)
        {
            return new TUR_TurmaCurriculoDAO { _Banco = banco }.AlterarMatrizCurricularTurmas(cur_idOrigem, crr_idOrigem, cur_idDestino, crr_idDestino, tur_ids);
        }

        /// <summary>
        /// Método que verifica se o turno selecionado é valido com o 
        /// currículo do curso.
        /// </summary>
        /// <param name="entTurmaCurriculo">entidade com dados Turma currículo</param>
        /// <param name="trn_id">id do turno</param>
        /// <param name="msgErro">Mensagem de erro</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>True - caso é compatível o turno com Curriculo Período / False - caso não seja compatível</returns>
        public static bool VerificaTurno(TUR_TurmaCurriculo entTurmaCurriculo, int trn_id, out string msgErro, Guid ent_id)
        {
            msgErro = "";

            ACA_CurriculoPeriodo entity = new ACA_CurriculoPeriodo
            {
                cur_id = entTurmaCurriculo.cur_id
                ,
                crr_id = entTurmaCurriculo.crr_id
                ,
                crp_id = entTurmaCurriculo.crp_id
            };

            ACA_CurriculoPeriodoBO.GetEntity(entity);

            if (ACA_TurnoHorarioBO.GetSelectDiasSemana(trn_id).Rows.Count !=
                entity.crp_qtdeDiasSemana)
            {
                msgErro = "A quantidade de dias da semana que possui aula do(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id) + " deve ser o mesmo do turno.";
                return true;
            }

            // Verifica o tipo de controle de tempo (TemposAula/Horas).
            if (entity.crp_controleTempo == Convert.ToInt32(ACA_CurriculoPeriodoControleTempo.TemposAula))
            {
                //Compara a quantidade de aulas lançadas no currículo período com a quantidades de aulas lançadas no turno.
                if (!(entity.crp_qtdeTemposSemana == ACA_TurnoBO.QuantidadeTemposAulaTurno(trn_id)))
                {
                    msgErro = "A quantidade de tempos de aula de uma semana do(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id) + " deve ser o mesmo do turno.";
                    return true;
                }
            }
            else
            {
                //Faz o calculo da horas de aula na semana definido no currículo período e compara com as horas lançadas no turno.
                if (!((entity.crp_qtdeHorasDia * 60 + entity.crp_qtdeMinutosDia) * entity.crp_qtdeDiasSemana == ACA_TurnoBO.QuantidadeHorasTurno(trn_id)))
                {
                    msgErro = "A quantidade de tempos de aula de uma semana do(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id) + " deve ser o mesmo do turno.";
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifica se o curso da turma possui avaliação do tipo seriado.
        /// </summary>
        /// <param name="entTurma">Entidade da turma.</param>
        /// <param name="EntFormatoAvaliacao">Entidade do formato de avaliação da turma.</param>
        /// <param name="entCurriculoPeriodo">Entidade do grupamento da turma (parâmatro de sáida)</param>
        /// <param name="Seriado">Flag que indica se o curso ~possui avaliação do tipo seriado (parâmatro de sáida)</param>
        /// <returns></returns>
        public static bool ValidaCursoSeriadoAvaliacao(TUR_Turma entTurma, ACA_FormatoAvaliacao EntFormatoAvaliacao, out ACA_CurriculoPeriodo entCurriculoPeriodo, out bool Seriado)
        {
            TalkDBTransaction banco = new CLS_AlunoAvaliacaoTurmaDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return ValidaCursoSeriadoAvaliacao(entTurma, EntFormatoAvaliacao, banco, out entCurriculoPeriodo, out Seriado);
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// Verifica se o curso da turma possui avaliação do tipo seriado.
        /// </summary>
        /// <param name="entTurma">Entidade da turma.</param>
        /// <param name="EntFormatoAvaliacao">Entidade do formato de avaliação da turma.</param>
        /// <param name="banco">Transação.</param>
        /// <param name="entCurriculoPeriodo">Entidade do grupamento da turma (parâmatro de sáida)</param>
        /// <param name="Seriado">Flag que indica se o curso ~possui avaliação do tipo seriado (parâmatro de sáida)</param>
        /// <returns></returns>
        public static bool ValidaCursoSeriadoAvaliacao(TUR_Turma entTurma, ACA_FormatoAvaliacao EntFormatoAvaliacao, TalkDBTransaction banco, out ACA_CurriculoPeriodo entCurriculoPeriodo, out bool Seriado)
        {
            Seriado = false;

            List<TUR_TurmaCurriculo> listCurriculos = TUR_TurmaCurriculoBO.GetSelectBy_Turma(entTurma.tur_id, banco, GestaoEscolarUtilBO.MinutosCacheLongo);

            if (listCurriculos.Count == 0)
                throw new Exception("A turma (tur_id: " + entTurma.tur_id + ") não possui nenhum curriculoPeriodo cadastrado.");

            ACA_Curriculo entCurriculo = new ACA_Curriculo
            {
                cur_id = listCurriculos[0].cur_id
                ,
                crr_id = listCurriculos[0].crr_id
            };
            ACA_CurriculoBO.GetEntity(entCurriculo, banco);

            // Se curso for seriado por avaliações - EJA.
            if (entCurriculo.crr_regimeMatricula ==
                (byte)ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes)
            {
                Seriado = true;

                if ((EntFormatoAvaliacao.fav_tipo != (byte)ACA_FormatoAvaliacaoTipo.ConceitoGlobal) &&
                    (EntFormatoAvaliacao.fav_tipo != (byte)ACA_FormatoAvaliacaoTipo.GlobalDisciplina))
                {
                    // Curso do EJA não pode efetivar notas por disciplina - não possui ligação
                    // com lançamento por disciplina.
                    throw new ValidationException("O formato de avaliação \"" + EntFormatoAvaliacao.fav_nome +
                                                  "\" deve ser do tipo \"Conceito global\" ou " +
                                                  "\"Conceito global e nota por " + CustomResource.GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") +"\", " +
                                                  "pois o curso da turma é seriado por avaliações.");
                }
            }

            entCurriculoPeriodo = new ACA_CurriculoPeriodo
            {
                cur_id = entCurriculo.cur_id
                ,
                crr_id = entCurriculo.crr_id
                ,
                crp_id = listCurriculos[0].crp_id
            };
            ACA_CurriculoPeriodoBO.GetEntity(entCurriculoPeriodo, banco);

            return true;
        }

        /// <summary>
        /// Retorna as entidades da TurmaCurriculo cadastradas nas turmas.
        /// </summary>
        /// <param name="tur_id">ID das turmas</param>
        /// <returns></returns>
        public static List<TUR_TurmaCurriculo> SelecionaPorTurmas
        (
            string tur_id
        )
        {
            TUR_TurmaCurriculoDAO dao = new TUR_TurmaCurriculoDAO();
            DataTable dtDados = dao.SelecionaPorTurmas(tur_id);
            return (from DataRow dr in dtDados.Rows
                    select dao.DataRowToEntity(dr, new TUR_TurmaCurriculo())).ToList();
        }
    }
}
