/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public class CLS_Questionario : Abstract_CLS_Questionario
	{        
        public override int qst_id { get; set; }
        
        [MSValidRange(500)]
        [MSNotNullOrEmpty("Título do questionário é obrigatório.")]
        public override string qst_titulo { get; set; }

        public override DateTime qst_dataCriacao { get; set; }

        public override DateTime qst_dataAlteracao { get; set; }
        
        [MSDefaultValue(1)]
        public override int qst_situacao { get; set; }

    }
}