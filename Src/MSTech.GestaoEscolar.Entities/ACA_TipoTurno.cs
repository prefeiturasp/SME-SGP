/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ACA_TipoTurno : Abstract_ACA_TipoTurno
    {
        [MSValidRange(100, "Tipo de turno pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de turno é obrigatório.")]
        public override string ttn_nome { get; set; }

        [MSDefaultValue(1)]
        public override byte ttn_situacao { get; set; }

        public override DateTime ttn_dataCriacao { get; set; }

        public override DateTime ttn_dataAlteracao { get; set; }

        public override byte ttn_tipo { get; set; }
    }
}