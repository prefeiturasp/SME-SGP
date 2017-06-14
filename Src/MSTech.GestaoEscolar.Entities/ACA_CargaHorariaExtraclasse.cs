/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.ComponentModel;
    using Validation;
    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_CargaHorariaExtraclasse : Abstract_ACA_CargaHorariaExtraclasse
	{
        /// <summary>
		/// ID da disciplina.
		/// </summary>
		[MSNotNullOrEmpty("Disciplina é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int dis_id { get; set; }

        /// <summary>
        /// ID do calendário anual.
        /// </summary>
        [MSNotNullOrEmpty("Calendário anual é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int cal_id { get; set; }

        /// <summary>
        /// ID do tipo de período do calendário.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de período do calendário é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tpc_id { get; set; }

        /// <summary>
        /// ID da carga horaria extraclasse.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int che_id { get; set; }

        /// <summary>
        /// Carga horária de atividades extraclasse.
        /// </summary>
        [MSNotNullOrEmpty("Carga horária é obrigatório.")]
        public override decimal che_cargaHoraria { get; set; }

        /// <summary>
        /// Situação do registro ( 1 - Ativo, 3 - Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte che_situacao { get; set; }

        /// <summary>
        /// Data de criação do regsitro.
        /// </summary>
        public override DateTime che_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime che_dataAlteracao { get; set; }
    }
}