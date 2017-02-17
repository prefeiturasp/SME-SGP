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
	[Serializable]
	public class ACA_AreaConhecimento : Abstract_ACA_AreaConhecimento
	{
        /// <summary>
        /// Id da área de conhecimento.
        /// </summary>
        [DataObjectField(true, true, false)]
        public override int aco_id { get; set; }

        /// <summary>
        /// Nome da área de conhecimento.
        /// </summary>
        [MSValidRange(150, "Nome pode conter até 150 caracteres.")]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string aco_nome { get; set; }

        /// <summary>
        /// Situacao da área de conhecimento. (1 - Ativo, 3 - Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override short aco_situacao { get; set; }

        /// <summary>
        /// Data de alteração da área de conhecimento.
        /// </summary>
        public override DateTime aco_dataAlteracao { get; set; }

        /// <summary>
        /// Data de cadastro da área de conhecimento.
        /// </summary>
        public override DateTime aco_dataCriacao { get; set; }
	}
}