/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_TipoAnotacaoAluno : Abstract_ACA_TipoAnotacaoAluno
	{
        [DataObjectField(true, true, false)]
        public override int tia_id { get; set; }

        [MSValidRange(50, "Tipo de anotação do aluno pode conter até 50 caracteres.")]
        [MSNotNullOrEmpty("Tipo de anotação do aluno é obrigatório.")]
        public override string tia_nome { get; set; }

        [MSValidRange(50, "Código pode conter até 50 caracteres.")]
        [MSNotNullOrEmpty("Código do Tipo de anotação do aluno é obrigatório.")]
        public override string tia_codigo { get; set; }

        [MSDefaultValue(1)]
        public override short tia_situacao { get; set; }

        public override DateTime tia_dataCriacao { get; set; }

        public override DateTime tia_dataAlteracao { get; set; }

        public override Guid ent_id { get; set; }
	}
}