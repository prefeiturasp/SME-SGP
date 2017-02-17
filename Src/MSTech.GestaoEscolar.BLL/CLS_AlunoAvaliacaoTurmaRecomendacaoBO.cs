/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using MSTech.Data.Common;
    using System.Collections.Generic;
    using System.Linq;
    using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: CLS_AlunoAvaliacaoTurmaRecomendacao Business Object. 
	/// </summary>
	public class CLS_AlunoAvaliacaoTurmaRecomendacaoBO : BusinessBase<CLS_AlunoAvaliacaoTurmaRecomendacaoDAO, CLS_AlunoAvaliacaoTurmaRecomendacao>
	{
        #region Consultas 

	    /// <summary>
        /// Seleciona as recomendações por matrícula do aluno e tipo de recomendação.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="rar_tipo">Tipo de recomendação.</param>
        /// <returns></returns>
        public static DataTable SelecionaPorMatriculaTurmaTipo(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id, byte rar_tipo)
        {
            return new CLS_AlunoAvaliacaoTurmaRecomendacaoDAO().SelecionaPorMatriculaTurmaTipo(tur_id, alu_id, mtu_id, fav_id, ava_id, rar_tipo);
        }

        /// <summary>
        /// Seleciona uma lista de recomendações por matrícula do aluno e tipo de recomendação.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="rar_tipo">Tipo de recomendação.</param>
        /// <returns></returns>
        public static List<CLS_AlunoAvaliacaoTurmaRecomendacao> SelecionaListaPorMatriculaTurmaTipo(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id, byte rar_tipo, TalkDBTransaction banco = null)
        {
            CLS_AlunoAvaliacaoTurmaRecomendacaoDAO dao = banco == null ?
                                                         new CLS_AlunoAvaliacaoTurmaRecomendacaoDAO() :
                                                         new CLS_AlunoAvaliacaoTurmaRecomendacaoDAO { _Banco = banco };

            return (from DataRow dr in dao.SelecionaPorMatriculaTurmaTipo(tur_id, alu_id, mtu_id, fav_id, ava_id, rar_tipo).Rows
                    select dao.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaRecomendacao())).ToList();
        }

        #endregion

        #region Saves

        /// <summary>
        /// O método salva as recomendações cadastradas para o aluno e deleta as que forem desmarcadas.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="lista">Lista de recomendações adicionadas</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool Salvar(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id, List<CLS_AlunoAvaliacaoTurmaRecomendacao> lista, TalkDBTransaction banco)
        {
            bool retorno = true;

            List<CLS_AlunoAvaliacaoTurmaRecomendacao> listaCadastrados = SelecionaListaPorMatriculaTurmaTipo(tur_id, alu_id, mtu_id, fav_id, ava_id, 0, banco);
            if (lista.Any())
            {
                List<CLS_AlunoAvaliacaoTurmaRecomendacao> listaExcluir = !listaCadastrados.Any() ?
                    new List<CLS_AlunoAvaliacaoTurmaRecomendacao>() : listaCadastrados.Where(p => !lista.Contains(p)).ToList();

                List<CLS_AlunoAvaliacaoTurmaRecomendacao> listaSalvar = listaCadastrados.Any() ?
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
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaRecomendacao.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaRecomendacao</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaRecomendacao entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaRecomendacaoDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaRecomendacao.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaRecomendacao</param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaRecomendacao entity)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaRecomendacaoDAO().Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
    }
}