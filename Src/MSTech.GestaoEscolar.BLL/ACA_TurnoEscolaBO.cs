using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using System.Data;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_TurnoEscolaBO : BusinessBase<ACA_TurnoEscolaDAO, ACA_TurnoEscola>
    {
        /// <summary>
        /// Retorna um datatable contendo todas as escolas do turno
        /// que não foram excluídas logicamente, filtrados por 
        /// id do turno
        /// </summary>
        /// <param name="trn_id">Id da tabela ACA_Turno do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com as escolas do turno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int trn_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_TurnoEscolaDAO dao = new ACA_TurnoEscolaDAO();
            return dao.SelectBy_trn_id(trn_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Inclui uma nova escola para o turno
        /// </summary>
        /// <param name="entity">Entidade ACA_TurnoEscola</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_TurnoEscola entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ACA_TurnoEscolaDAO dao = new ACA_TurnoEscolaDAO {_Banco = banco};
                dao.Salvar(entity);
            }
            else
            {
                throw new ValidationException(entity.PropertiesErrorList[0].Message);
            }

            return true;
        }
    }
}
