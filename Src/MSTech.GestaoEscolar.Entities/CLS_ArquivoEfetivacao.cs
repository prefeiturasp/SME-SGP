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
	public class CLS_ArquivoEfetivacao : AbstractCLS_ArquivoEfetivacao
    {
        /// <summary>
        /// Id do registro.
        /// </summary>
        [DataObjectField(true, true, false)]
        public override long aef_id { get; set; }

        /// <summary>
        /// Id da escola.
        /// </summary>
        [MSNotNullOrEmpty("Escola é obrigatório.")]
        public override int esc_id { get; set; }

        /// <summary>
        /// Id da unidade da escola.
        /// </summary>
        [MSNotNullOrEmpty("Unidade da escola é obrigatório.")]
        public override int uni_id { get; set; }

        /// <summary>
        /// Id do calendário.
        /// </summary>
        [MSNotNullOrEmpty("Calendário é obrigatório.")]
        public override int cal_id { get; set; }

        /// <summary>
        /// Id do tipo período currículo.
        /// </summary>
        [MSNotNullOrEmpty("Período currículo é obrigatório.")]
        public override int tpc_id { get; set; }

        /// <summary>
        /// Id da turma
        /// </summary>
        [MSNotNullOrEmpty("Turma é obrigatório.")]
        public override long tur_id { get; set; }

        /// <summary>
        /// Tipo: 1-Importação, 2-Exportação.
        /// </summary>
        [MSNotNullOrEmpty("Tipo da solicitação é obrigatório.")]
        public override short aef_tipo { get; set; }

        /// <summary>
        /// Situação do registro.
        /// </summary>
        [MSDefaultValue(1)]
        public override short aef_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime aef_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime aef_dataAlteracao { get; set; }

	}
}