/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using System.ComponentModel;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class ORC_Nivel : Abstract_ORC_Nivel
	{
        /// <summary>
        /// ID do nível.
        /// </summary>
        [DataObjectField(true, true, false)]
        public override int nvl_id { get; set; }

        /// <summary>
        /// ID do curso.
        /// </summary>
        [MSNotNullOrEmpty("Curso é obrigatório.")]
        public override int cur_id { get; set; }

        /// <summary>
        /// ID do curriculo.
        /// </summary>
        [MSNotNullOrEmpty("Currículo é obrigatório.")]
        public override int crr_id { get; set; }

        /// <summary>
        /// ID do período.
        /// </summary>
        [MSNotNullOrEmpty("Currículo período é obrigatório.")]
        public override int crp_id { get; set; }

        /// <summary>
        /// ID do calendário associado ao nível.
        /// </summary>
        [MSNotNullOrEmpty("Calendário é obrigatório.")]
        public override int cal_id { get; set; }

        /// <summary>
        /// ID do tipo de disciplina.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de disciplina é obrigatório.")]
        public override int tds_id { get; set; }

        /// <summary>
        /// Ordem do nível.
        /// </summary>
        [MSNotNullOrEmpty("Ordem é obrigatório.")]
        public override int nvl_ordem { get; set; }

        /// <summary>
        /// Nome do nível.
        /// </summary>
        [MSValidRange(100, "Nome pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string nvl_nome { get; set; }

        /// <summary>
        /// Nome do nível no plural.
        /// </summary>
        [MSValidRange(100, "Nome pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome no plural é obrigatório.")]
        public override string nvl_nomePlural { get; set; }

        /// <summary>
        /// Situação do nível (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte nvl_situacao { get; set; }

        /// <summary>
        /// Data de criação do nível.
        /// </summary>
        public override DateTime nvl_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do nível.
        /// </summary>
        public override DateTime nvl_dataAlteracao { get; set; }
	}
}