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
	public class ACA_TipoCurriculoPeriodo : Abstract_ACA_TipoCurriculoPeriodo
	{
        /// <summary>
        /// Campo id da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de período do curso é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tcp_id { get; set; }

        /// <summary>
        /// Campo id da tabela ACA_TipoNivelEnsino.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de nível de ensino é obrigatório.")]
        public override int tne_id { get; set; }

        /// <summary>
        /// Campo id da tabela ACA_TipoModalidadeEnsino.
        /// </summary>
        [MSNotNullOrEmpty("Tipo da modalidade de ensino é obrigatório.")]
        public override int tme_id { get; set; }

        /// <summary>
        /// Campo descrição da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        [MSValidRange(100, "Descrição pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Descrição do tipo de período do curso é obrigatorio.")]
        public override string tcp_descricao { get; set; }

        /// <summary>
        /// Campo ordem da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        [MSNotNullOrEmpty("Ordem é obrigatório.")]
        public override short tcp_ordem { get; set; }

        /// <summary>
        /// Campo que define se o tipo de periodo do curso pode possuir objeto de aprendizagem
        /// </summary>
        public override bool tcp_objetoAprendizagem { get; set; }

        /// <summary>
        /// Campo situação da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        [MSDefaultValue(1)]
        public override short tcp_situacao { get; set; }

        /// <summary>
        /// Campo data de criação da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        public override DateTime tcp_dataCriacao { get; set; }

        /// <summary>
        /// Campo data de alteração da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        public override DateTime tcp_dataAlteracao { get; set; }
	}
}