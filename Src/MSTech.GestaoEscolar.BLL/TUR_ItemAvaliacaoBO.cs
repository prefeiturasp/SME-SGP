using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using System.Data;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class TUR_ItemAvaliacaoBO : BusinessBase<TUR_ItemAvaliacaoDAO, TUR_ItemAvaliacao> 
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        static public DataTable Seleciona_mav_id
        (
            int mav_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            TUR_ItemAvaliacaoDAO dao = new TUR_ItemAvaliacaoDAO();
            return dao.SelectBy_mav_id(mav_id, 1, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new bool Save
        (
            TUR_ItemAvaliacao entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                TUR_ItemAvaliacaoDAO dao = new TUR_ItemAvaliacaoDAO {_Banco = banco};
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }
    }
}
