/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.ComponentModel;
    /// <summary>
    /// Description: .
    /// </summary>
    public class SYS_ServicosLogExecucao : Abstract_SYS_ServicosLogExecucao
	{
        /// <summary>
		/// Id do log de execução do serviço.
		/// </summary>
        [DataObjectField(true, true, false)]
        public override Guid sle_id { get; set; }
    }
}