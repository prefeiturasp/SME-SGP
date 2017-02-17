/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{	
	/// <summary>
	/// 
    /// </summary>
    [Serializable()]
	public class ACA_Curriculo : Abstract_ACA_Curriculo
	{        
        [DataObjectField(true, false, false)]
        public override int cur_id { get; set; }        
        [DataObjectField(true, false, false)]
        public override int crr_id { get; set; }
        [MSValidRange(10, "Código pode conter até 10 caracteres.")]
        public override string crr_codigo { get; set; }
        [MSValidRange(200, "Nome pode conter até 200 caracteres.")]
        public override string crr_nome { get; set; }        
        [MSNotNullOrEmpty("Regime de matrícula é obrigatório.")]
        public override byte crr_regimeMatricula { get; set; }        
        [MSNotNullOrEmpty("Quantidade normal de períodos é obrigatório.")]
        public override byte crr_periodosNormal { get; set; }
        [MSNotNullOrEmpty("Quantidade de dias letivos é obrigatório e deve ser um número inteiro maior que 0 (zero).")]
        public override int crr_diasLetivos { get; set; }
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime crr_vigenciaInicio { get; set; }
        [MSDefaultValue(1)]
        public override byte crr_situacao { get; set; }        
        public override DateTime crr_dataCriacao { get; set; }
        public override DateTime crr_dataAlteracao { get; set; }
	}
}