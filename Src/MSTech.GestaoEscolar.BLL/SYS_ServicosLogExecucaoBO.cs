/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using Data.Common;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Description: SYS_ServicosLogExecucao Business Object. 
    /// </summary>
    public class SYS_ServicosLogExecucaoBO : BusinessBase<SYS_ServicosLogExecucaoDAO, SYS_ServicosLogExecucao>
	{
        #region Métodos de inclusão/alteração

        public static Guid IniciarServico(eChaveServicos servico)
        {
            SYS_ServicosLogExecucao log = new SYS_ServicosLogExecucao
            {
                ser_id = (short)servico
                ,
                sle_dataInicioExecucao = DateTime.Now
            };

            Save(log);

            return log.sle_id;
        }

        public static bool FinalizarServio(Guid sle_id)
        {
            SYS_ServicosLogExecucao log = new SYS_ServicosLogExecucao
            {
                sle_id = sle_id
            };
            GetEntity(log);
            log.sle_dataFimExecucao = DateTime.Now;

            return Save(log);
        }

        #endregion Métodos de inclusão/alteração

    }
}