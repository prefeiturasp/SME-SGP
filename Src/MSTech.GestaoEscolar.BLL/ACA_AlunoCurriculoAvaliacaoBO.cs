using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Data.Common;
using System.Data;
using System;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Situações do AlunoCurriculoAvaliacao
    /// </summary>
    public enum ACA_AlunoCurriculoAvaliacaoSituacao : byte
    {
        Ativo = 1
        ,
        Excluido = 3
        ,
        Inativo = 4
    }

	/// <summary>
	/// ACA_AlunoCurriculoAvaliacao Business Object 
	/// </summary>
	public class ACA_AlunoCurriculoAvaliacaoBO : BusinessBase<ACA_AlunoCurriculoAvaliacaoDAO, ACA_AlunoCurriculoAvaliacao>
	{
        /// <summary>
        /// Retorna o registro do aluno na AlunoCurriculoAvaliacao na ordem:
        /// - Se for encontrado um registro para o lançamento 
        /// já realizado (CLS_AlunoAvaliacaoTurma).
        /// - Se não for encontrado, retorna o registro ativo, em que
        /// não tenha lançamento na CLS_AlunoAvaliacaoTurma.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="mtu_id">ID da matrícula do aluno na turma</param>
        /// <param name="aat_id">ID do lançamento de nota do aluno na CLS_AlunoAvaliacaoTurma</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns></returns>
        public static ACA_AlunoCurriculoAvaliacao GetEntityBy_LancamentoAluno
        (
            long alu_id
            , long tur_id
            , int mtu_id
            , int aat_id
            , TalkDBTransaction banco
        )
        {
            ACA_AlunoCurriculoAvaliacaoDAO dao = new ACA_AlunoCurriculoAvaliacaoDAO {_Banco = banco};
            DataTable dt = dao.LoadBy_LancamentoAluno(alu_id, tur_id, mtu_id, aat_id);

            ACA_AlunoCurriculoAvaliacao entity = new ACA_AlunoCurriculoAvaliacao();

            if (dt.Rows.Count > 0)
            {
                entity = dao.DataRowToEntity(dt.Rows[0], entity);
            }

            return entity;
        }

        /// <summary>
        /// Retorna o currículo avaliação onde o aluno está ativo.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns></returns>
        public static ACA_AlunoCurriculoAvaliacao GetEntityBy_AtualAluno
        (
            long alu_id
            , int cur_id
            , int crr_id
            , int crp_id
            , TalkDBTransaction banco
        )
        {
            ACA_AlunoCurriculoAvaliacaoDAO dao = new ACA_AlunoCurriculoAvaliacaoDAO { _Banco = banco };
            DataTable dt = dao.LoadBy_AtualAluno(alu_id, cur_id, crr_id, crp_id);

            ACA_AlunoCurriculoAvaliacao entity = new ACA_AlunoCurriculoAvaliacao();

            if (dt.Rows.Count > 0)
            {
                entity = dao.DataRowToEntity(dt.Rows[0], entity);
            }

            return entity;
        }

        /// <summary>
        /// Salva a avaliação para o aluno.
        /// </summary>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <param name="entity">Entidade para salvar</param>
        public static new bool Save(ACA_AlunoCurriculoAvaliacao entity, TalkDBTransaction banco)
        {
            if (!entity.Validate())
                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

            ACA_AlunoCurriculoAvaliacaoDAO dao = new ACA_AlunoCurriculoAvaliacaoDAO
                                                     {
                                                         _Banco = banco
                                                     };
            return dao.Salvar(entity);
        }
        
        /// <summary>
        /// Salva a avaliação do aluno ligando ela ao lançamento de notas da efetivação. Não avança o aluno.
        /// </summary>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <param name="entity">Entidade do lançamento de notas da efetivação</param>
        /// <param name="cap_dataInicio">Data de início do período do calendário.</param>
        /// <param name="cap_dataFim">Data de fim do período do calendário.</param>
        public static void SalvarAvaliacaoAlunoSemAvanco(TalkDBTransaction banco, CLS_AlunoAvaliacaoTurma entity, DateTime cap_dataInicio, DateTime cap_dataFim)
        {
            ACA_AlunoCurriculoAvaliacao entAlunoAvaliacao =
                GetEntityBy_LancamentoAluno
                    (entity.alu_id, entity.tur_id, entity.mtu_id, entity.aat_id, banco);

            if (!entAlunoAvaliacao.IsNew)
            {
                // Se existe uma entidade na AlunoCurriculoAvaliacao, salva o registro,
                // ligando ele com a CLS_AlunoAvaliacaoTurma.

                // Configurar valores do lançamento para a AlunoCurriculoAvaliacao.
                entAlunoAvaliacao.mtu_id = entity.mtu_id;
                entAlunoAvaliacao.aat_id = entity.aat_id;
                entAlunoAvaliacao.ala_dataInicio = cap_dataInicio;
                entAlunoAvaliacao.ala_dataFim = cap_dataFim;
                Save(entAlunoAvaliacao, banco);
            }    
        }
	}
}