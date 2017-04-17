using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Exceções

    /// <summary>
    /// Classe de excessão, utilizada quando foi incluido uma nota para avaliação da secretaria
    /// recentemente por outro usuário
    /// </summary>
    public class IncluirTurmaNotaAluno_ValidationException : ValidationException
    {
        public IncluirTurmaNotaAluno_ValidationException(string message)
            : base(message)
        {
        }
    }

    #endregion Exceções

    public class CLS_TurmaNotaAlunoBO : BusinessBase<CLS_TurmaNotaAlunoDAO, CLS_TurmaNotaAluno>
    {
        #region Métodos de Validação.

        /// <summary>
        /// Valida os dados da atividade avaliativa e alunos participantes.
        /// </summary>
        /// <param name="listTurmaNota">Lista de avaliações</param>
        /// <param name="listTurmaNotaAluno">Lista de alunos participantes</param>
        /// <param name="errorMSG">Mensagem de erro</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>True : dados válidos || False : inválidos</returns>
        public static bool ValidaParticipantesAvaliacao(List<CLS_TurmaNota> listTurmaNota, List<CLS_TurmaNotaAluno> listTurmaNotaAluno, out string errorMSG, Guid ent_id)
        {
            errorMSG = string.Empty;

            var tntIdsExclusivas = from CLS_TurmaNota item in listTurmaNota
                                   where item.tnt_exclusiva == true
                                   select item.tnt_id;

            if (tntIdsExclusivas.Any())
            {
                var alunosParticipantes = from CLS_TurmaNotaAluno item in listTurmaNotaAluno
                                          where item.tna_participante == true
                                          select item.tnt_id;

                var avaliacaoSemParticipantes = from item in tntIdsExclusivas
                                                where !alunosParticipantes.Contains(item)
                                                select item;

                #region Validação - Ao menos um aluno participante para atividade avaliativa exclusiva.

                if (avaliacaoSemParticipantes.Any())
                    errorMSG = string.Format("Lançamento de nota para a {0} deve ter ao menos um aluno participante.", GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(ent_id).ToLower());

                #endregion Validação - Ao menos um aluno participante para atividade avaliativa exclusiva.
            }

            return string.IsNullOrEmpty(errorMSG);
        }

        /// <summary>
        /// Valida os dados da atividade avaliativa e alunos participantes.
        /// </summary>
        /// <param name="listTurmaNota">Lista de avaliações</param>
        /// <param name="listTurmaNotaAluno">Lista de alunos participantes</param>
        /// <param name="errorMSG">Mensagem de erro</param>
        /// <returns>True : dados válidos || False : inválidos</returns>
        public static bool ValidaParticipantesAvaliacaoDiarioClasse(List<CLS_TurmaNota> listTurmaNota, List<CLS_TurmaNotaAluno> listTurmaNotaAluno, out string errorMSG)
        {
            errorMSG = string.Empty;

            foreach (CLS_TurmaNota atividade in listTurmaNota.Where(p => p.tnt_exclusiva))
            {
                List<CLS_TurmaNotaAluno> notas = listTurmaNotaAluno.Where(p => p.idAtividade == atividade.idAtividade).ToList();

                if (notas.Any())
                {
                    var alunosParticipantes = from CLS_TurmaNotaAluno item in notas
                                              where item.tna_participante
                                              select item.idAtividade;

                    if (!alunosParticipantes.Any())
                        errorMSG = string.Format("Lançamento de nota para a {0} deve ter ao menos um aluno participante.", GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(new Guid()).ToLower());
                }
            }

            return string.IsNullOrEmpty(errorMSG);
        }

        /// <summary>
        /// Verifica se os valores mínimos estão dentro dos limites setados.
        /// </summary>
        /// <param name="ObjAvaliacaoNum">Campo a ser verificado.</param>
        /// <param name="valorMinimo">Valores dos limites para o campo passado por parametro.</param>
        /// <param name="pes_nome">Nome do aluno para exibir para o usuário na mensagem de validação</param>
        public static bool VerificaValoresNotas
        (
            ACA_EscalaAvaliacaoNumerica ObjAvaliacaoNum
            , string valorMinimo
            , string pes_nome
        )
        {
            if (ObjAvaliacaoNum.IsNew)
                return true;

            if (string.IsNullOrEmpty(valorMinimo))
                return true;

            Decimal valorMin;
            if (!Decimal.TryParse(valorMinimo, out valorMin))
                throw new ArgumentException("O valor para a nota do aluno " + pes_nome + " deve ser um número decimal.");

            if (Convert.ToDecimal(valorMinimo) < ObjAvaliacaoNum.ean_menorValor)
            {
                decimal numero = ObjAvaliacaoNum.ean_menorValor != Decimal.Truncate(ObjAvaliacaoNum.ean_menorValor) ? ObjAvaliacaoNum.ean_menorValor : Decimal.Truncate(ObjAvaliacaoNum.ean_menorValor);
                throw new ArgumentException("O valor para a nota do aluno " + pes_nome + " deve ser maior ou igual a " + numero + ".");
            }
            if (Convert.ToDecimal(valorMinimo) > ObjAvaliacaoNum.ean_maiorValor)
            {
                decimal maiorValor = ObjAvaliacaoNum.ean_maiorValor != Decimal.Truncate(ObjAvaliacaoNum.ean_maiorValor) ? ObjAvaliacaoNum.ean_maiorValor : Decimal.Truncate(ObjAvaliacaoNum.ean_maiorValor);
                throw new ArgumentException("As notas dos alunos não podem ser maiores que " + maiorValor + ".");
            }
            if (Convert.ToDecimal(valorMinimo) % ObjAvaliacaoNum.ean_variacao != 0)
            {
                decimal numero = ObjAvaliacaoNum.ean_variacao != Decimal.Truncate(ObjAvaliacaoNum.ean_variacao) ? ObjAvaliacaoNum.ean_variacao : Decimal.Truncate(ObjAvaliacaoNum.ean_variacao);

                throw new ArgumentException("O valor para a nota do aluno " + pes_nome + " deve ter variação de " + Convert.ToDouble(numero) + ".");
            }

            return true;
        }

        #endregion Métodos de Validação.

        #region Métodos de Consulta/Busca.

        /// <summary>
        /// Retorna os dados da CLS_TurmaNotaAluno que sejam pela
        /// "chave" da matrícula do aluno na disciplina.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="mtd_id">Id da matrícula do aluno na disciplina - obrigatório</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>Lista de  CLS_TurmaNotaAluno</returns>
        public static List<CLS_TurmaNotaAluno> GetSelectBy_Disciplina_Aluno
        (
            Int64 tud_id
            , Int64 alu_id
            , Int32 mtu_id
            , Int32 mtd_id
            , TalkDBTransaction banco
        )
        {
            CLS_TurmaNotaAlunoDAO dao = new CLS_TurmaNotaAlunoDAO
            {
                _Banco = banco
            };

            return dao.SelectBy_Disciplina_Aluno(tud_id, alu_id, mtu_id, mtd_id);
        }

        /// <summary>
        /// Retorna o lançamento de notas dos alunos.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tnt_id">ID da atividade da disciplina da turma</param>
        /// <param name="tpc_id"></param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="ordenacao">Tipo de ordenação dos alunos 0-Número chamada, 1-Nome do aluno</param>
        /// <param name="trazerInativos"></param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaFrequenciaPorNotaTurmaDisciplinaPeriodo
        (
            long tud_id
            , int tnt_id
            , int tpc_id
            , Guid ent_id
            , byte ordenacao
            , bool trazerInativos
        )
        {
            CLS_TurmaNotaAlunoDAO dao = new CLS_TurmaNotaAlunoDAO();
            return dao.SelectBy_TurmaDisciplinaPeriodo(tud_id, tnt_id, tpc_id, ent_id, ordenacao, trazerInativos);
        }

        /// <summary>
        /// Retorna o lançamento de notas dos alunos.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tnt_id">ID da atividade da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tipoDocente">Tipo de docente.</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="ordenacao">Tipo de ordenação dos alunos 0-Número chamada, 1-Nome do aluno</param>
        /// <param name="trazerInativos"></param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaFrequenciaPorNotaTurmaDisciplinaPeriodoFiltroDeficiencia
        (
            long tud_id
            , int tnt_id
            , int tpc_id
            , EnumTipoDocente tipoDocente
            , Guid ent_id
            , byte ordenacao
            , bool trazerInativos
        )
        {
            return new CLS_TurmaNotaAlunoDAO().SelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(tud_id, tnt_id, tpc_id, (byte)tipoDocente, ent_id, ordenacao, trazerInativos);
        }

        /// <summary>
        /// Retorna o lançamento de notas dos alunos da atividade passada
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tnt_id">ID da atividade da disciplina da turma</param>
        public static DataTable BuscaNotasDaAtividade
        (
            long tud_id
            , int tnt_id
        )
        {
            CLS_TurmaNotaAlunoDAO dao = new CLS_TurmaNotaAlunoDAO();
            return dao.BuscaNotasDaAtividade(tud_id, tnt_id);
        }

        #endregion Métodos de Consulta/Busca.

        #region Métodos de Inclusão/Alteração.

        /// <summary>
        /// Salva as notas das atividades dos alunos e a propriedade "tnt_efetivado" das
        /// atividades.
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        /// </summary>
        public static bool Save
        (
            List<CLS_TurmaNotaAluno> listTurmaNotaAluno
            , List<CLS_TurmaNota> listTurmaNota
            , List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> listAlunoAvaliacaoTurmaDisciplinaMedia
            , long tur_id
            , long tud_id
            , int tpc_id
            , int fav_id
            , int tdt_posicao
            , Guid ent_id
            , bool fechamentoAutomatico
            , Guid usu_id = new Guid()
            , byte origemLogMedia = 0
            , byte origemLogNota = 0
            , byte tipoLogNota = 0
            , long tud_idRegencia = -1
        )
        {
            TalkDBTransaction banco = new CLS_TurmaNotaAlunoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return Save(listTurmaNotaAluno, listTurmaNota, listAlunoAvaliacaoTurmaDisciplinaMedia, tur_id, tud_id, tpc_id, fav_id, tdt_posicao, banco, ent_id, fechamentoAutomatico, usu_id, origemLogMedia, origemLogNota, tipoLogNota, tud_idRegencia);
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        /// <summary>
        /// Salva as notas das atividades dos alunos e a propriedade "tnt_efetivado" das
        /// atividades.
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        /// </summary>
        public static bool Save
        (
            List<CLS_TurmaNotaAluno> listTurmaNotaAluno
            , List<CLS_TurmaNota> listTurmaNota
            , List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> listAlunoAvaliacaoTurmaDisciplinaMedia
            , long tur_id
            , long tud_id
            , int tpc_id
            , int fav_id
            , int tdt_posicao
            , Guid ent_id
            , List<CLS_TurmaAula> listTurmaAula
            , bool fechamentoAutomatico
            , Guid usu_id = new Guid()
            , byte origemLogMedia = 0
            , byte origemLogNota = 0
            , byte tipoLogNota = 0
            , long tud_idRegencia = -1
        )
        {
            TalkDBTransaction banco = new CLS_TurmaNotaAlunoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (!CLS_TurmaAulaBO.AtualizarStatusAtividadeAvaliativa(listTurmaAula, banco))
                {
                    throw new ValidationException("Erro ao salvar as notas.");
                }

                return Save(listTurmaNotaAluno, listTurmaNota, listAlunoAvaliacaoTurmaDisciplinaMedia, tur_id, tud_id, tpc_id, fav_id, tdt_posicao, banco, ent_id, fechamentoAutomatico, usu_id, origemLogMedia, origemLogNota, tipoLogNota, tud_idRegencia);
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        /// <summary>
        /// Salva os dados das notas dos alunos.
        /// </summary>
        /// <param name="dtTurmaNotaAluno">DataTable de dados do listão de avaliação.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvaNotaAlunos(DataTable dtTurmaNotaAluno, TalkDBTransaction banco = null)
        {
            return banco == null ?
                   new CLS_TurmaNotaAlunoDAO().SalvaNotaAlunos(dtTurmaNotaAluno) :
                   new CLS_TurmaNotaAlunoDAO { _Banco = banco }.SalvaNotaAlunos(dtTurmaNotaAluno);
        }

        /// <summary>
        /// Salva as notas das atividades dos alunos e a propriedade "tnt_efetivado" das
        /// atividades.
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        /// <param name="origemLogMedia">Se enviar a origem do log então será salvo o LOG_AvaliacaoMedia_Alteracao</param>
        /// <param name="origemLogNota">Se enviar a origem do log então será salvo o LOG_TurmaNota_Alteracao</param>
        /// </summary>
        internal static bool Save
        (
            List<CLS_TurmaNotaAluno> listTurmaNotaAluno
            , List<CLS_TurmaNota> listTurmaNota
            , List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> listAlunoAvaliacaoTurmaDisciplinaMedia
            , long tur_id
            , long tud_id
            , int tpc_id
            , int fav_id
            , int tdt_posicao
            , TalkDBTransaction banco
            , Guid ent_id
            , bool fechamentoAutomatico
            , Guid usu_id = new Guid()
            , byte origemLogMedia = 0
            , byte origemLogNota = 0
            , byte tipoLogNota = 0
            , long tud_idRegencia = -1
        )
        {
            string errorMSG = string.Empty;

            if (!ValidaParticipantesAvaliacao(listTurmaNota, listTurmaNotaAluno, out errorMSG, ent_id))
                throw new ValidationException(errorMSG);

            object lockObject = new object();

            // Salva os dados de todos os alunos na tabela CLS_TurmaNotaAluno.
            DataTable dtTurmaNotaAluno = CLS_TurmaNotaAluno.TipoTabela_TurmaNotaAluno();
            if (listTurmaNotaAluno.Any())
            {
                Parallel.ForEach
                (
                    listTurmaNotaAluno,
                    turmaNotaAluno =>
                    {
                        lock (lockObject)
                        {
                            DataRow dr = dtTurmaNotaAluno.NewRow();
                            dtTurmaNotaAluno.Rows.Add(TurmaNotaAlunoToDataRow(turmaNotaAluno, dr));
                        }
                    }
                );
            }
            SalvaNotaAlunos(dtTurmaNotaAluno, banco);

            GestaoEscolarUtilBO.LimpaCache(String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY, tur_id, tpc_id));
            GestaoEscolarUtilBO.LimpaCache(ModelCache.FECHAMENTO_BIMESTRE_COMPONENTES_REGENCIA_PATTERN_KEY, tur_id.ToString());

            string tnt_ids = string.Join(",", (from CLS_TurmaNota item in listTurmaNota select item.tnt_id.ToString()).ToArray());
            List<CLS_TurmaNota> listaTurmaNota;
            List<LOG_TurmaNota_Alteracao> listLogNota = new List<LOG_TurmaNota_Alteracao>();
            // Recupera a lista de entidades CLS_TurmaNotaAluno para verificar se ela já existe.
            CLS_TurmaNotaAlunoDAO tnaDao = new CLS_TurmaNotaAlunoDAO { _Banco = banco };
            tnaDao.SelectBy_Disciplina_Atividades(tud_id, tnt_ids, out listaTurmaNota);

            DateTime dataLogNota = DateTime.Now;
            foreach (CLS_TurmaNota entTurNota in listTurmaNota)
            {
                // Busca registro - deve existir.
                CLS_TurmaNota entAux = listaTurmaNota.Find
                    (p => p.tud_id == entTurNota.tud_id
                          && p.tnt_id == entTurNota.tnt_id);

                if (entAux != null)
                {
                    // Só altera o tnt_efetivado.
                    entAux.tnt_efetivado = entTurNota.tnt_efetivado;
                    entAux.usu_idDocenteAlteracao = entTurNota.usu_idDocenteAlteracao;

                    if (!entAux.Validate())
                    {
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entAux));
                    }

                    if ((tdt_posicao <= 0) || (tdt_posicao == entTurNota.tdt_posicao))
                    {
                        CLS_TurmaNotaBO.Save(entAux, banco);

                        if (origemLogNota > 0)
                        {
                            LOG_TurmaNota_Alteracao entLogNota = new LOG_TurmaNota_Alteracao
                            {
                                tud_id = entAux.tud_id,
                                tnt_id = entAux.tnt_id,
                                usu_id = usu_id,
                                ltn_origem = origemLogNota,
                                ltn_tipo = tipoLogNota,
                                ltn_data = dataLogNota
                            };

                            listLogNota.Add(entLogNota);
                        }
                    }
                }
            }

            //Salva os logs de alteração de nota
            LOG_TurmaNota_AlteracaoBO.SalvarEmLote(listLogNota, banco);

            // Salva os dados da média.
            if (listAlunoAvaliacaoTurmaDisciplinaMedia != null)
            {
                List<CLS_AlunoAvaliacaoTurmaDisciplinaMedia> listaMediasBimestreBD = CLS_AlunoAvaliacaoTurmaDisciplinaMediaBO.BuscaNotasFinaisTurma(tud_id, tpc_id, banco);
                bool alteracaoMedia = listAlunoAvaliacaoTurmaDisciplinaMedia.Exists(media => 
                    listaMediasBimestreBD.Any() ?
                        !listaMediasBimestreBD.Any
                            (
                                p => p.tud_id == media.tud_id && 
                                     p.alu_id == media.alu_id && 
                                     p.mtu_id == media.mtu_id && 
                                     p.mtd_id == media.mtd_id && 
                                     (string.IsNullOrEmpty(p.atm_media) ? "" : p.atm_media) == (string.IsNullOrEmpty(media.atm_media) ? "" : media.atm_media)
                            ) :
                        !string.IsNullOrEmpty(media.atm_media));

                CLS_AlunoAvaliacaoTurmaDisciplinaMediaBO.SalvarEmLote(tur_id, tud_id, tpc_id, fav_id, listAlunoAvaliacaoTurmaDisciplinaMedia, banco);

                // Salva na fila de processamento e o log caso exista alguma alteração.
                if (alteracaoMedia && listAlunoAvaliacaoTurmaDisciplinaMedia.Count > 0)
                {
                    // Caso o fechamento seja automático, grava na fila de processamento.
                    if (fechamentoAutomatico && tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                    {
                        CLS_AlunoFechamentoPendenciaBO.SalvarFilaNota(tud_id, tpc_id, banco);
                        if (tud_idRegencia > 0)
                        {
                            CLS_AlunoFechamentoPendenciaBO.SalvarFilaNota(tud_idRegencia, tpc_id, banco);
                        }
                    }

                    if (origemLogMedia > 0)
                    {
                        LOG_AvaliacaoMedia_Alteracao log = new LOG_AvaliacaoMedia_Alteracao
                                                            {
                                                                tud_id = tud_id,
                                                                tpc_id = tpc_id,
                                                                usu_id = usu_id,
                                                                lam_data = DateTime.Now,
                                                                lam_tipo = (byte)LOG_AvaliacaoMedia_Alteracao_Tipo.AlteracaoMedia,
                                                                lam_origem = origemLogMedia
                                                            };
                        LOG_AvaliacaoMedia_AlteracaoBO.Save(log, banco);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// O método converte um registro da CLS_TurmaNotaAluno em um DataRow.
        /// </summary>
        /// <param name="turmaNotaAluno">Registro da CLS_TurmaNotaAluno.</param>
        /// <param name="dr">Layout do DataTable.</param>
        /// <returns>DataRow.</returns>
        public static DataRow TurmaNotaAlunoToDataRow(CLS_TurmaNotaAluno turmaNotaAluno, DataRow dr, DateTime tna_dataAlteracao = new DateTime())
        {
            if (turmaNotaAluno.idAtividade > 0)
                dr["idAtividade"] = turmaNotaAluno.idAtividade;
            else
                dr["idAtividade"] = DBNull.Value;

            dr["tud_id"] = turmaNotaAluno.tud_id;
            dr["tnt_id"] = turmaNotaAluno.tnt_id;
            dr["alu_id"] = turmaNotaAluno.alu_id;
            dr["mtu_id"] = turmaNotaAluno.mtu_id;
            dr["mtd_id"] = turmaNotaAluno.mtd_id;

            if (!string.IsNullOrEmpty(turmaNotaAluno.tna_avaliacao))
                dr["tna_avaliacao"] = turmaNotaAluno.tna_avaliacao;
            else
                dr["tna_avaliacao"] = DBNull.Value;

            if (!string.IsNullOrEmpty(turmaNotaAluno.tna_comentarios))
                dr["tna_comentarios"] = turmaNotaAluno.tna_comentarios;
            else
                dr["tna_comentarios"] = DBNull.Value;

            if (!string.IsNullOrEmpty(turmaNotaAluno.tna_relatorio))
                dr["tna_relatorio"] = turmaNotaAluno.tna_relatorio;
            else
                dr["tna_relatorio"] = DBNull.Value;

            dr["tna_naoCompareceu"] = turmaNotaAluno.tna_naoCompareceu;
            dr["tna_situacao"] = 1; // Sempre ativo
            dr["tna_participante"] = turmaNotaAluno.tna_participante;

            if (tna_dataAlteracao != new DateTime())
                dr["tna_dataAlteracao"] = tna_dataAlteracao;
            else
                dr["tna_dataAlteracao"] = DBNull.Value;

            return dr;
        }

        #endregion Métodos de Inclusão/Alteração.
    }
}