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
    public class REL_GraficoAtendimento : Abstract_REL_GraficoAtendimento
	{
        public override int gra_id { get; set; }

        [MSDefaultValue(1)]
        public override byte gra_situacao { get; set; }
        
        public override DateTime gra_dataCriacao { get; set; }
        
        public override DateTime gra_dataAlteracao { get; set; }
    }
}