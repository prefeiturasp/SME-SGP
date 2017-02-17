/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using System.ComponentModel;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class DCL_ProtocoloHistorico : Abstract_DCL_ProtocoloHistorico
    {
        /// <summary>
        /// Propriedade pro_id.
        /// </summary>
        [MSNotNullOrEmpty("Protocolo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override Guid pro_id { get; set; }

        /// <summary>
        /// Propriedade prh_id.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int prh_id { get; set; }

        /// <summary>
        /// Propriedade pro_status.
        /// </summary>
        [MSNotNullOrEmpty("Status do protocolo é obrigatório.")]
        public override byte pro_status { get; set; }

        /// <summary>
        /// Propriedade prh_dataCriacao.
        /// </summary>
        public override DateTime prh_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade prh_dataAlteracao.
        /// </summary>
        public override DateTime prh_dataAlteracao { get; set; }
	}
}