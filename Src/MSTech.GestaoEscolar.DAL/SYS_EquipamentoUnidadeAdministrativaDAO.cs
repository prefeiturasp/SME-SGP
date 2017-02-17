using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.Data.Common;
using System.Data;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class SYS_EquipamentoUnidadeAdministrativaDAO : Abstract_SYS_EquipamentoUnidadeAdministrativaDAO
    {
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }
    }
}
