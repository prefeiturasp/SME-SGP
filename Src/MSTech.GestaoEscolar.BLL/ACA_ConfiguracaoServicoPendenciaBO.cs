/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Collections.Generic;
    /// <summary>
    /// Description: ACA_ConfiguracaoServicoPendencia Business Object. 
    /// </summary>
    public class ACA_ConfiguracaoServicoPendenciaBO : BusinessBase<ACA_ConfiguracaoServicoPendenciaDAO, ACA_ConfiguracaoServicoPendencia>
	{
        /// <summary>
        /// Retorna as configurações de serviço de pendência não excluídas logicamente, de acordo com tipo de nível de ensino,
        /// tipo de modalidade de ensino e tipo de turma.
        /// </summary>   
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tme_id">Id do tipo de modalidade de ensino</param>
        /// <param name="tur_tipo">Enum do tipo de turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBy_tne_id_tme_id_tur_tipo
        (
            int tne_id
            , int tme_id
            , int tur_tipo
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            ACA_ConfiguracaoServicoPendenciaDAO dao = new ACA_ConfiguracaoServicoPendenciaDAO();
            return dao.SelectBy_tne_id_tme_id_tur_tipo(tne_id, tme_id, tur_tipo, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna as configurações de serviço de pendência não excluídas logicamente, de acordo com tipo de nível de ensino,
        /// tipo de modalidade de ensino e tipo de turma.
        /// </summary>   
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tme_id">Id do tipo de modalidade de ensino</param>
        /// <param name="tur_tipo">Enum do tipo de turma</param>
        public static List<ACA_ConfiguracaoServicoPendencia> SelectTodasBy_tne_id_tme_id_tur_tipo
        (
            int tne_id
            , int tme_id
            , int tur_tipo
        )
        {
            ACA_ConfiguracaoServicoPendenciaDAO dao = new ACA_ConfiguracaoServicoPendenciaDAO();
            DataTable dt = dao.SelectTodasBy_tne_id_tme_id_tur_tipo(tne_id, tme_id, tur_tipo, false, 1, 0, out totalRecords);

            return (from DataRow dr in dt.Rows
                    select (ACA_ConfiguracaoServicoPendencia)GestaoEscolarUtilBO.DataRowToEntity(dr, new ACA_ConfiguracaoServicoPendencia())).ToList();
        }
    }
}