/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;

	/// <summary>
	/// Description: TUR_TurmaDocenteProjeto Business Object. 
	/// </summary>
	public class TUR_TurmaDocenteProjetoBO : BusinessBase<TUR_TurmaDocenteProjetoDAO, TUR_TurmaDocenteProjeto>
	{
		#region Consultas

		/// <summary>
		/// Seleciona os registros por turma, docente, colaborador, cargo e colaborador-cargo 
		/// que não estejam logicamente excluídos.
		/// </summary>
		/// <param name="tur_id">Id da turma</param>
		/// <param name="doc_id">Id do docente</param>
		/// <param name="col_id">Id do colaborador</param>
		/// <param name="crg_id">Id do cargo</param>
		/// <param name="coc_id">Id do colaborador-cargo</param>
		/// <returns>Lista com os registros encontrados</returns>
		public static List<TUR_TurmaDocenteProjeto> SelectBy_TurmaDocenteColaboradorCargo
		(
			long tur_id,
			long doc_id,
			long col_id,
			int crg_id,
			int coc_id
		)
		{
			TUR_TurmaDocenteProjetoDAO dao = new TUR_TurmaDocenteProjetoDAO();
			DataTable dt = dao.SelectBy_TurmaDocenteColaboradorCargo(tur_id, doc_id, col_id, crg_id, coc_id);
			List<TUR_TurmaDocenteProjeto> lt = new List<TUR_TurmaDocenteProjeto>();

			if (dt.Rows.Count > 0)
				lt = (from dr in dt.AsEnumerable()
					  select dao.DataRowToEntity(dr, new TUR_TurmaDocenteProjeto { })).ToList();

			return lt;
		}

		#endregion
	}
}