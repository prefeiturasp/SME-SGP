/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CLS_CompensacaoAusencia : AbstractCLS_CompensacaoAusencia
	{
        /// <summary>
        /// Propriedade tud_id.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override long tud_id { get; set; }

        /// <summary>
        /// Propriedade cpa_id.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int cpa_id { get; set; }

        /// <summary>
        /// Propriedade tpc_id.
        /// </summary>
        public override int tpc_id { get; set; }


        /// <summary>
        /// Propriedade cpa_situacao.
        /// </summary>
        [MSDefaultValue(1)]
        public override short cpa_situacao { get; set; }

        /// <summary>
        /// Propriedade cpa_dataCriacao.
        /// </summary>
        public override DateTime cpa_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade cpa_dataAlteracao.
        /// </summary>
        public override DateTime cpa_dataAlteracao { get; set; }

        public int tpc_ordem { get; set; }
	}
}