/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System.Collections.Generic;
using System.Linq;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Data;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
	
	/// <summary>
	/// TUR_TurmaDisciplinaNaoAvaliado Business Object 
	/// </summary>
	public class TUR_TurmaDisciplinaNaoAvaliadoBO : BusinessBase<TUR_TurmaDisciplinaNaoAvaliadoDAO,TUR_TurmaDisciplinaNaoAvaliado>
	{
        /// <summary>
        /// Salvar avaliações não avaliadas para a disciplina.
        /// </summary>
        /// <param name="banco">Transação com banco</param>
        /// <param name="cad">Entidade de cadastro de turmaDisciplina</param>
        /// <param name="listaDisciplinaNaoAvaliado_Turma">Lista de avaliações já cadastradas para a turma</param>
        internal static void SalvarAvaliacoesDisciplinas(TalkDBTransaction banco, CadastroTurmaDisciplina cad, List<TUR_TurmaDisciplinaNaoAvaliado> listaDisciplinaNaoAvaliado_Turma)
        {
            foreach (TUR_TurmaDisciplinaNaoAvaliado entDisciplinaNaoAvaliado in cad.listaAvaliacoesNaoAvaliar)
            {
                entDisciplinaNaoAvaliado.tud_id = cad.entTurmaDisciplina.tud_id;

                if (!entDisciplinaNaoAvaliado.Validate())
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entDisciplinaNaoAvaliado));

                TUR_TurmaDisciplinaNaoAvaliado entAux = new TUR_TurmaDisciplinaNaoAvaliado
                {
                    tud_id = entDisciplinaNaoAvaliado.tud_id
                    ,
                    fav_id = entDisciplinaNaoAvaliado.fav_id
                    ,
                    ava_id = entDisciplinaNaoAvaliado.ava_id
                };
                GetEntity(entAux, banco);

                if (entAux.IsNew)
                {
                    Save(entDisciplinaNaoAvaliado, banco);
                }
            }

            // Busca avaliações para a disciplina.
            List<TUR_TurmaDisciplinaNaoAvaliado> listaDisciplinaNaoAvaliado =
                (from TUR_TurmaDisciplinaNaoAvaliado item in listaDisciplinaNaoAvaliado_Turma
                 where item.tud_id == cad.entTurmaDisciplina.tud_id
                 select item).ToList();

            // Excluir as avaliações para a disciplina que não foram inseridas e existiam antes.
            foreach (TUR_TurmaDisciplinaNaoAvaliado entDisciplinaNaoAvaliado in listaDisciplinaNaoAvaliado)
            {
                if (!cad.listaAvaliacoesNaoAvaliar.Exists
                         (
                             p =>
                             p.tud_id == entDisciplinaNaoAvaliado.tud_id
                             && p.fav_id == entDisciplinaNaoAvaliado.fav_id
                             && p.ava_id == entDisciplinaNaoAvaliado.ava_id
                         ))
                {
                    Delete(entDisciplinaNaoAvaliado, banco);
                }
            }
        }

        /// <summary>
        /// Retorna as avaliações que serão desconsideradas para todas as disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public static DataTable GetSelectBy_Turma(long tur_id)
		{
		    return new TUR_TurmaDisciplinaNaoAvaliadoDAO().SelectBy_Turma(tur_id);
		}

	    /// <summary>
	    /// Retorna as avaliações que serão desconsideradas para todas as disciplinas da turma.
	    /// </summary>
	    /// <param name="tur_id">ID da turma</param>
	    /// <param name="banco">Transação com banco</param>
	    /// <returns></returns>
        public static List<TUR_TurmaDisciplinaNaoAvaliado> GetSelectBy_Turma(long tur_id, TalkDBTransaction banco)
	    {
	        TUR_TurmaDisciplinaNaoAvaliadoDAO dao = new TUR_TurmaDisciplinaNaoAvaliadoDAO {_Banco = banco};
	        DataTable dt = dao.SelectBy_Turma(tur_id);

	        List<TUR_TurmaDisciplinaNaoAvaliado> lista =
	            (from DataRow dr in dt.Rows
	             select dao.DataRowToEntity(dr, new TUR_TurmaDisciplinaNaoAvaliado())
	            ).ToList();

            return lista;
        }
    }
}