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
    using System.ComponentModel;
    using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho Business Object. 
	/// </summary>
	public class CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoBO : BusinessBase<CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoDAO, CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho>
    {
        #region Consultas

        /// <summary>
        /// Seleciona os tipo de desemepenho e aprendizado por disciplina e matrícula do aluno.
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
            return new CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoDAO().SelecionaPorMatriculaTurmaDisciplina(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id);
        }

        /// <summary>
        /// Seleciona uma lista de desemepenho e aprendizado por disciplina e matrícula do aluno.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="mtd_id">ID da matrícula turma disciplina no aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <returns></returns>
        public static List<CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho> SelecionaListaPorMatriculaTurmaDisciplina(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id, TalkDBTransaction banco = null)
        {
            CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoDAO dao = banco == null ?
                                                                new CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoDAO() :
                                                                new CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoDAO { _Banco = banco };

            return (from DataRow dr in dao.SelecionaPorMatriculaTurmaDisciplina(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id).Rows
                    select dao.DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho())).ToList();
        }

        /// <summary>
        /// Retorna os docentes da disciplina da turma
        /// </summary>
        /// <param name="alu_id">ID do aluno</param> 
        /// <param name="mtu_id">Id da matrícula do aluno.</param>
        /// <param name="cur_id">ID do curso</param> 
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="ava_id">ID da avaliação</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDesempenhosAluno
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
            CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoDAO dao = new CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoDAO();
            return dao.SelectBy_AluId_CurId_CrrId_CrpId(alu_id, mtu_id, cur_id, crr_id, crp_id, ava_id, mostraPorDisciplina);
        }

        #endregion

        #region Saves

        /// <summary>
        /// O método salva os desempenhos cadastrados para o aluno e deleta as que forem desmarcados.
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
        public static bool Salvar(long tud_id, long alu_id, int mtu_id, int mtd_id, int fav_id, int ava_id, List<CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho> lista, TalkDBTransaction banco)
        {
            bool retorno = true;

            List<CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho> listaCadastrados = SelecionaListaPorMatriculaTurmaDisciplina(tud_id, alu_id, mtu_id, mtd_id, fav_id, ava_id, banco);
            if (lista.Any())
            {
                List<CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho> listaExcluir = !listaCadastrados.Any() ?
                    new List<CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho>() : listaCadastrados.Where(p => !lista.Contains(p)).ToList();

                List<CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho> listaSalvar = listaCadastrados.Any() ?
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
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// O método salva um registro na tabela CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho</param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho entity)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoAvaliacaoTurmaDisciplinaDesempenhoDAO().Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
    }
}