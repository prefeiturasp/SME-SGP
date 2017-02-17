/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.ComponentModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao Business Object. 
	/// </summary>
	public class CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoBO : BusinessBase<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoDAO, CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao>
	{
        #region Consultas

        /// <summary>
        /// Seleciona as recomendações por disciplina, matrícula do aluno e tipo de recomendação.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="mtd_id">ID da matrícula turma disciplina no aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="rar_tipo">Tipo de recomendação.</param>
        /// <returns></returns>
        public static DataTable SelecionaPorMatriculaTurmaDisciplinaTipo(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id, byte rar_tipo)
        {
            return new CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoDAO().SelecionaPorMatriculaTurmaDisciplinaTipo(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id, rar_tipo);
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
        public static DataTable SelecionaRecomendacoesPorCurriculoPeriodoAvaliacao
        (
            long alu_id,
            int mtu_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int ava_id,
            Int16 rar_tipo,
            bool mostraPorDisciplina
        )
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoDAO();
            return dao.SelectBy_AluId_CurId_CrrId_CrpId(alu_id, mtu_id, cur_id, crr_id, crp_id, ava_id, rar_tipo, mostraPorDisciplina);
        }

        /// <summary>
        /// Seleciona uma lista de recomendações por disciplina, matrícula do aluno e tipo de recomendação.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="mtd_id">ID da matrícula turma disciplina no aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="rar_tipo">Tipo de recomendação.</param>
        /// <returns></returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao> SelecionaListaPorMatriculaTurmaDisciplinaTipo(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id, byte rar_tipo, TalkDBTransaction banco = null)
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoDAO dao = banco == null ?
                                                                new CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoDAO() :
                                                                new CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoDAO { _Banco = banco };

            DataTable dtRecomendacao = dao.SelecionaPorMatriculaTurmaDisciplinaTipo(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id, rar_tipo);

            if (dtRecomendacao.Rows.Count > 1)
            {
                return (from DataRow dr in dtRecomendacao.Rows
                        select dao.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao())).ToList();
            }
            return new List<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao>();
        }

        #endregion

        #region Saves

        /// <summary>
        /// O método salva as recomendações cadastradas para o aluno e deleta as que forem desmarcadas.
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
        public static bool Salvar(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id, List<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao> lista, TalkDBTransaction banco)
        {
            bool retorno = true;

            List<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao> listaCadastrados = SelecionaListaPorMatriculaTurmaDisciplinaTipo(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id, 0, banco);

            if (lista.Any())
            {
                List<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao> listaExcluir = !listaCadastrados.Any() ?
                    new List<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao>() : listaCadastrados.Where(p => !lista.Contains(p)).ToList();

                List<CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao> listaSalvar = listaCadastrados.Any() ?
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
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao</param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao entity)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacaoDAO().Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
	}
}