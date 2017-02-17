/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
    public class ACA_AlunoFichaMedica : Abstract_ACA_AlunoFichaMedica
	{
        [MSValidRange(5, "Tipo sanguíneo pode conter até 5 caracteres.")]
        public override string afm_tipoSanguineo { get; set; }
        [MSValidRange(5, "Fator RH pode conter até 5 caracteres.")]
        public override string afm_fatorRH { get; set; }
        [MSValidRange(1000, "Convênio médico pode conter até 1000 caracteres.")]
        public override string afm_convenioMedico { get; set; }
        [MSValidRange(1000, "Hospital para remoção pode conter até 1000 caracteres.")]
        public override string afm_hospitalRemocao { get; set; }
	}
}