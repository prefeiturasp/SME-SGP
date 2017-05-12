/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System.ComponentModel;
    using Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_ConfiguracaoServicoPendencia : Abstract_ACA_ConfiguracaoServicoPendencia
	{
        /// <summary>
        /// Data de alteração do registro..
        /// </summary>
        public override DateTime csp_dataAlteracao { get; set; }

        /// <summary>
        /// Data de criação do registro..
        /// </summary>
        public override DateTime csp_dataCriacao { get; set; }

        /// <summary>
		/// Id do registro..
		/// </summary>
        [DataObjectField(true, true, false)]
        [MSDefaultValue(-1)]
        public override int csp_id { get; set; }

        /// <summary>
		/// Situação do registro..
		/// </summary>
		[MSDefaultValue(1)]
        public override int csp_situacao { get; set; }
    }
}