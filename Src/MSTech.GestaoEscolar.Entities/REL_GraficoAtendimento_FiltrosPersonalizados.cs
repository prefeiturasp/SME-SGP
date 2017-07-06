/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using Validation;
    using System;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
	public class REL_GraficoAtendimento_FiltrosPersonalizados : Abstract_REL_GraficoAtendimento_FiltrosPersonalizados
	{
        
        public override int gfp_id { get; set; }

        [MSDefaultValue(1)]
        public override int gfp_situacao { get; set; }
        
        public override DateTime gfp_dataCriacao { get; set; }
        
        public override DateTime gfp_dataAlteracao { get; set; }
    }
}