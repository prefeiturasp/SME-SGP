using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_TipoFormacaoDocenteBO : BusinessBase<ACA_TipoFormacaoDocenteDAO, ACA_TipoFormacaoDocente>
    {
        /// <summary>
        /// Retorna todos os tipos de formação do docente não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoFormacaoDocente()
        {
            ACA_TipoFormacaoDocenteDAO dao = new ACA_TipoFormacaoDocenteDAO();
            return dao.SelectBy_Pesquisa(false, 1, 1, out totalRecords);
        }
    }
}
