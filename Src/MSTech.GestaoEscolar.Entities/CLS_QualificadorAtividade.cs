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
    public class CLS_QualificadorAtividade : AbstractCLS_QualificadorAtividade
    {
        [MSNotNullOrEmpty]
        [MSDefaultValue(1)]
        public override byte qat_situacao { get; set; }
        public override DateTime qat_dataCriacao { get; set; }
        public override DateTime qat_dataAlteracao { get; set; }
    }
}
