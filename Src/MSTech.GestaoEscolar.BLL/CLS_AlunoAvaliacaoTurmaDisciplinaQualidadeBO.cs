/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;
    using MSTech.Data.Common;
    using System.ComponentModel;
    using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: CLS_AlunoAvaliacaoTurmaDisciplinaQualidade Business Object. 
	/// </summary>
	public class CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeBO : BusinessBase<CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeDAO, CLS_AlunoAvaliacaoTurmaDisciplinaQualidade>
	{
        #region Consultas

        /// <summary>
        /// Seleciona os tipo de qualidade por disciplina e matrícula do aluno.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="mtd_id">ID da matrícula turma disciplina no aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <returns></returns>
        public static DataTable SelecionaPorMatriculaTurmaDisciplina(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id)
        {

            return new CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeDAO().SelecionaPorMatriculaTurmaDisciplina(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id);
        }

        /// <summary>
        /// Seleciona uma lista de tipo de qualidade por disciplina e matrícula do aluno.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="mtd_id">ID da matrícula turma disciplina no aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <returns></returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplinaQualidade> SelecionaListaPorMatriculaTurmaDisciplina(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id, TalkDBTransaction banco = null)
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeDAO dao = banco == null ?
                                                                new CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeDAO() :
                                                                new CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeDAO { _Banco = banco };

            return (from DataRow dr in dao.SelecionaPorMatriculaTurmaDisciplina(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id).Rows
                    select dao.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaDisciplinaQualidade())).ToList();
        }

        /// <summary>
        /// Retorna os docentes da disciplina da turma
        /// </summary>
        /// <param name="alu_id">ID do aluno</param> 
        /// <param name="mtu_id">Id da matrícula do aluno.</param>
        /// <param name="cur_id">ID do curso</param> 
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaQualidadesAluno
        (
            long alu_id,
            int mtu_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int ava_id,
            bool mostraPorDisciplina
        )
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeDAO();
            return dao.SelectBy_AluId_CurId_CrrId_CrpId(alu_id, mtu_id, cur_id, crr_id, crp_id, ava_id, mostraPorDisciplina);
        }


        #endregion
				
        #region Saves

        /// <summary>
        /// O método salva as qualidade cadastradas para o aluno e deleta as que forem desmarcadas.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="mtd_id">ID da matrícula turma disciplina do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="lista">Lista de qualidades adicionadas</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool Salvar(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id, List<CLS_AlunoAvaliacaoTurmaDisciplinaQualidade> lista, TalkDBTransaction banco)
        {
            bool retorno = true;

            List<CLS_AlunoAvaliacaoTurmaDisciplinaQualidade> listaCadastrados = SelecionaListaPorMatriculaTurmaDisciplina(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id, banco);
            if (lista.Any())
            {
                List<CLS_AlunoAvaliacaoTurmaDisciplinaQualidade> listaExcluir = !listaCadastrados.Any() ?
                    new List<CLS_AlunoAvaliacaoTurmaDisciplinaQualidade>() : listaCadastrados.Where(p => !lista.Contains(p)).ToList();

                List<CLS_AlunoAvaliacaoTurmaDisciplinaQualidade> listaSalvar = listaCadastrados.Any() ?
                    lista.Where(p => !listaCadastrados.Contains(p)).ToList() : lista;

                retorno &= !listaExcluir.Any() ? retorno : listaExcluir.Aggregate(true, (excluiu, qualidade) => excluiu & Delete(qualidade, banco));
                retorno &= !listaSalvar.Any() ? retorno : listaSalvar.Aggregate(true, (salvou, qualidade) => salvou & Save(qualidade, banco));
            }
            else
            {
                retorno &= !listaCadastrados.Any() ? retorno : listaCadastrados.Aggregate(true, (excluiu, qualidade) => excluiu & Delete(qualidade, banco));
            }

            return retorno;
        }

        /// <summary>
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaDisciplinaQualidade.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplinaQualidade</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDisciplinaQualidade entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaDisciplinaQualidade.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplinaQualidade</param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDisciplinaQualidade entity)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaDisciplinaQualidadeDAO().Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
	}
}