/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Linq;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: CLS_AlunoAvaliacaoTurmaDesempenho Business Object. 
	/// </summary>
	public class CLS_AlunoAvaliacaoTurmaDesempenhoBO : BusinessBase<CLS_AlunoAvaliacaoTurmaDesempenhoDAO, CLS_AlunoAvaliacaoTurmaDesempenho>
    {
        #region Consultas

        /// <summary>
        /// Seleciona os tipo de desempenho e aprendizado por matrícula do aluno.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <returns></returns>
        public static DataTable SelecionaPorMatriculaTurma(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id)
        {
            return new CLS_AlunoAvaliacaoTurmaDesempenhoDAO().SelecionaPorMatriculaTurma(tur_id, alu_id, mtu_id, fav_id, ava_id);
        }

        /// <summary>
        /// Seleciona uma lista de tipo de desempenho e aprendizado por matrícula do aluno.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <returns></returns>
        public static List<CLS_AlunoAvaliacaoTurmaDesempenho> SelecionaListaPorMatriculaTurma(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id, TalkDBTransaction banco = null)
        {
            CLS_AlunoAvaliacaoTurmaDesempenhoDAO dao = banco == null ?
                                                       new CLS_AlunoAvaliacaoTurmaDesempenhoDAO() :
                                                       new CLS_AlunoAvaliacaoTurmaDesempenhoDAO { _Banco = banco };

            return (from DataRow dr in dao.SelecionaPorMatriculaTurma(tur_id, alu_id, mtu_id, fav_id, ava_id).Rows
                    select dao.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaDesempenho())).ToList();
        }

        #endregion

        #region Saves

        /// <summary>
        /// O método salva os desempenhos cadastradas para o aluno e deleta as que forem desmarcadas.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="lista">Lista de qualidades adicionadas</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool Salvar(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id, List<CLS_AlunoAvaliacaoTurmaDesempenho> lista, TalkDBTransaction banco)
        {
            bool retorno = true;

            List<CLS_AlunoAvaliacaoTurmaDesempenho> listaCadastrados = SelecionaListaPorMatriculaTurma(tur_id, alu_id, mtu_id, fav_id, ava_id, banco);
            if (lista.Any())
            {
                List<CLS_AlunoAvaliacaoTurmaDesempenho> listaExcluir = !listaCadastrados.Any() ?
                    new List<CLS_AlunoAvaliacaoTurmaDesempenho>() : listaCadastrados.Where(p => !lista.Contains(p)).ToList();

                List<CLS_AlunoAvaliacaoTurmaDesempenho> listaSalvar = listaCadastrados.Any() ?
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
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaDesempenho.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDesempenho</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDesempenho entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaDesempenhoDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaDesempenho.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDesempenho</param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDesempenho entity)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaDesempenhoDAO().Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
    }
}