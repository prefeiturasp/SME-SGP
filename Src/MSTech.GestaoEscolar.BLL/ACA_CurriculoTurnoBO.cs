using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Business.Common;
using System.ComponentModel;
using System.Data;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_CurriculoTurnoBO : BusinessBase<ACA_CurriculoTurnoDAO, ACA_CurriculoTurno>
    {
        /// <summary>
        /// Retorna um datatable contendo todos os tipos de turnos do curriculo/curso
        /// que não foram excluídos logicamente, filtrados por 
        /// id do curso, id do curriculo
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com os tipos de turnos do curriculo</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int cur_id
            , int crr_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CurriculoTurnoDAO dao = new ACA_CurriculoTurnoDAO();
            return dao.SelectBy_cur_id_crr_id(cur_id, crr_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

       [DataObjectMethod(DataObjectMethodType.Select, false)]
       public static DataTable SelectPorEscolas
       (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CurriculoTurnoDAO dao = new ACA_CurriculoTurnoDAO();
            return dao.SelectBy_cur_id_crr_id_esc_id(cur_id, crr_id, esc_id, uni_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectPorEscolas
        (
            int esc_id
            , int uni_id
        )
        {
            totalRecords = 0;
            ACA_CurriculoTurnoDAO dao = new ACA_CurriculoTurnoDAO();
            return dao.SelectBy_esc_id_uni_id(esc_id, uni_id, out totalRecords);
        }

        /// <summary>
        /// Inclui um novo tipo de turno para o curriculo/curso
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoTurno</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_CurriculoTurno entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ACA_CurriculoTurnoDAO dao = new ACA_CurriculoTurnoDAO {_Banco = banco};
                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }
    }
}
