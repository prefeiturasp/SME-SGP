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
    public class CLS_TurmaAtividadeExtraClasseAluno : Abstract_CLS_TurmaAtividadeExtraClasseAluno
	{
        /// <summary>
		/// Indica se a atividade foi entregue pelo aluno.
		/// </summary>
		[MSDefaultValue(false)]
        public override bool aea_entregue { get; set; }

        /// <summary>
        /// Situação do registro.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte aea_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime aea_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime aea_dataAlteracao { get; set; }
    }
}