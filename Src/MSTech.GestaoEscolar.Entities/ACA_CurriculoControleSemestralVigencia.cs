/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;

namespace MSTech.GestaoEscolar.Entities
{

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public class ACA_CurriculoControleSemestralVigencia : AbstractACA_CurriculoControleSemestralVigencia
    {

        /// <summary>
        /// ID CurriculoPeriodoVigencia.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int vig_id { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime vig_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime vig_dataAlteracao { get; set; }
    }
}