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
	public class ACA_TipoDesempenhoAprendizado : AbstractACA_TipoDesempenhoAprendizado
    {
        [MSDefaultValue(1)]
        public override short tda_situacao { get; set; }
        public override DateTime tda_dataCriacao { get; set; }
        public override DateTime tda_dataAlteracao { get; set; }
	}
}