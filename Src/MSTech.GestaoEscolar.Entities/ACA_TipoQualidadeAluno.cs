/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class ACA_TipoQualidadeAluno : AbstractACA_TipoQualidadeAluno
	{
        [MSDefaultValue(1)]
        public override short tqa_situacao { get; set; }
        public override DateTime tqa_dataCriacao { get; set; }
        public override DateTime tqa_dataAlteracao { get; set; }
	}
}