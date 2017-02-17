/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class ESC_EscolaClassificacaoVigencia : AbstractESC_EscolaClassificacaoVigencia
	{
        [MSNotNullOrEmpty("Data de início da vigência é obrigatório.")]
        public override DateTime ecv_dataInicio { get; set; }
        [MSDefaultValue(1)]
		public override short ecv_situacao { get; set; }
        public override DateTime ecv_dataAlteracao { get; set; }
        public override DateTime ecv_dataCriacao { get; set; }
	}
}