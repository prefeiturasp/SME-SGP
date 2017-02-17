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
    public class ACA_RecomendacaoAlunoResponsavel : AbstractACA_RecomendacaoAlunoResponsavel
    {
        [MSDefaultValue(1)]
        public override short rar_situacao { get; set; }
        public override DateTime rar_dataCriacao { get; set; }
        public override DateTime rar_dataAlteracao { get; set; }
    }
}