/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.Validation.Exceptions;
    using MSTech.Data.Common;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    /// <summary>
    /// Description: CLS_AlunoAvaliacaoTurmaDisciplinaObservacao Business Object. 
    /// </summary>
    public class CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoBO : BusinessBase<CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoDAO, CLS_AlunoAvaliacaoTurmaDisciplinaObservacao>
    {
        #region Saves

        /// <summary>
        /// O método salva as observações por disciplina.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="mtd_id">ID da matrícula turma disciplina do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="ltObservacao">Lista de observações.</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvarObservacao(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id, CLS_AlunoAvaliacaoTurDis_Observacao observacao)
        {
            TalkDBTransaction banco = new CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (observacao.entityObservacao != null && observacao.entityObservacao != new CLS_AlunoAvaliacaoTurmaDisciplinaObservacao())
                    Save(observacao.entityObservacao, banco);
                // Limpa cache do fechamento, para atualizar o check.
                GestaoEscolarUtilBO.LimpaCache(MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodo(tud_id, fav_id, ava_id, string.Empty));
                GestaoEscolarUtilBO.LimpaCache(MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(tud_id, fav_id, ava_id, string.Empty));
                return true;
            }
            catch (ValidationException ex)
            {
                banco.Close(ex);
                throw;
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
        /// O método salva um registro CLS_AlunoAvaliacaoTurmaDisciplinaObservacao
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplinaObservacao</param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDisciplinaObservacao entity)
        {
            if (entity.Validate())
            {
                CLS_AlunoAvaliacaoTurmaDisciplinaObservacao entAux = new CLS_AlunoAvaliacaoTurmaDisciplinaObservacao
                {
                    tud_id = entity.tud_id
                    ,
                    alu_id = entity.alu_id
                    ,
                    mtu_id = entity.mtu_id
                    ,
                    mtd_id = entity.mtd_id
                    ,
                    fav_id = entity.fav_id
                    ,
                    ava_id = entity.ava_id
                };
                GetEntity(entAux);

                entity.IsNew = entAux.IsNew;

                if (string.IsNullOrEmpty(entity.ado_qualidade) &&
                    string.IsNullOrEmpty(entity.ado_desempenhoAprendizado) &&
                    string.IsNullOrEmpty(entity.ado_recomendacaoAluno) &&
                    string.IsNullOrEmpty(entity.ado_recomendacaoResponsavel))
                {
                    return entity.IsNew ? true : Delete(entity);
                }
                else
                {
                    return new CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoDAO().Salvar(entity);
                }
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// O método salva um registro CLS_AlunoAvaliacaoTurmaDisciplinaObservacao
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplinaObservacao</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDisciplinaObservacao entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                CLS_AlunoAvaliacaoTurmaDisciplinaObservacao entAux = new CLS_AlunoAvaliacaoTurmaDisciplinaObservacao
                {
                    tud_id = entity.tud_id
                    ,
                    alu_id = entity.alu_id
                    ,
                    mtu_id = entity.mtu_id
                    ,
                    mtd_id = entity.mtd_id
                    ,
                    fav_id = entity.fav_id
                    ,
                    ava_id = entity.ava_id
                };
                GetEntity(entAux, banco);

                entity.IsNew = entAux.IsNew;

                if (string.IsNullOrEmpty(entity.ado_qualidade) &&
                    string.IsNullOrEmpty(entity.ado_desempenhoAprendizado) &&
                    string.IsNullOrEmpty(entity.ado_recomendacaoAluno) &&
                    string.IsNullOrEmpty(entity.ado_recomendacaoResponsavel))
                {
                    return entity.IsNew ? true : Delete(entity, banco);
                }
                else
                {
                    return new CLS_AlunoAvaliacaoTurmaDisciplinaObservacaoDAO { _Banco = banco }.Salvar(entity);
                }
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
    }
}