/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// RHU_CargaHoraria Business Object 
    /// </summary>
    public class RHU_CargaHorariaBO : BusinessBase<RHU_CargaHorariaDAO, RHU_CargaHoraria>
    {
        #region Métodos de Seleção

        /// <summary>
        /// Retorna todos os registros de carga horária não excluídas logicamente.
        /// De acordo com os parâmetros passados.
        /// </summary>        
        /// <param name="chr_especialista">Especialista.</param>
        /// <param name="crg_id">Id do cargo.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns>Retorna um data table de registro de carga horária.</returns>
        public static DataTable SelecionaPorPadraoEspecialistaCargo
        (            
            bool chr_especialista
            , int crg_id
            , Guid ent_id
        )
        {
            totalRecords = 0;

            RHU_CargaHorariaDAO dao = new RHU_CargaHorariaDAO();
            return dao.SelectBy_PadraoEspecialistaCargo(chr_especialista, crg_id, ent_id);
        }

        /// <summary>
        /// Retorna todas as cargas horárias não excluídas logicamente
        /// </summary>        
        /// <param name="chr_descricao">escrição da carga horária</param>
        /// <param name="ent_id">Entidade do usuário logado</param>       
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCargaHoraria
        (
            string chr_descricao
            , Guid ent_id
        )
        {
            totalRecords = 0;

            RHU_CargaHorariaDAO chrDao = new RHU_CargaHorariaDAO();
            return chrDao.SelectBy_Pesquisa(chr_descricao, ent_id, out totalRecords);
        }

		/// <summary>
		/// Seleciona carga horária de acordo com os cargos de docentes enviados
		/// </summary>
		/// <param name="idsCargo">Cargos a obter carga horária</param>
		/// <param name="ent_id">Id da Entidade</param>
		public static DataTable SelectBy_CargoDocente(string idsCargo, Guid ent_id)
		{
			RHU_CargaHorariaDAO chrDao = new RHU_CargaHorariaDAO();
			return chrDao.SelectBy_CargoDocente(idsCargo, ent_id);
		}

        #endregion
    }
}