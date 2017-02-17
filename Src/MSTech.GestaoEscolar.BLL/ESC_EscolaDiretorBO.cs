using System.Data;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ESC_EscolaDiretorBO : BusinessBase<ESC_EscolaDiretorDAO, ESC_EscolaDiretor>        
    {
        /// <summary>
        /// Retorna um datatable contendo todos os Diretores da Escola
        /// que não foram excluídos logicamente, filtrados por 
        /// id da escola
        /// </summary>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>        
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com os Diretores da Escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int esc_id            
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            
            ESC_EscolaDiretorDAO dao = new ESC_EscolaDiretorDAO();
            return dao.SelectBy_esc_id(esc_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Inclui um novo diretor para a escola
        /// </summary>
        /// <param name="entity">Entidade ESC_EscolaDiretor</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ESC_EscolaDiretor entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ESC_EscolaDiretorDAO dao = new ESC_EscolaDiretorDAO {_Banco = banco};
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }
    }
}
