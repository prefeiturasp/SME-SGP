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
	public class CLS_ConfiguracaoAtividadeQualificador : Abstract_CLS_ConfiguracaoAtividadeQualificador
	{
        /// <summary>
        /// Quantidade de atividade do qualificador.
        /// </summary>
        [MSDefaultValue(0)]
        public override int caq_quantidade { get; set; }

        /// <summary>
        /// Id do qualificador de atividade.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int qat_id { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override short caq_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime caq_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime caq_dataAlteracao { get; set; }
	}
}