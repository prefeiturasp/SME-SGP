/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.ComponentModel;
    using Validation;    /// <summary>
                         /// Description: .
                         /// </summary>
    [Serializable]
	public class SYS_UsuarioAPI : Abstract_SYS_UsuarioAPI
	{
        /// <summary>
		/// ID do usuário API.
		/// </summary>
        [DataObjectField(true, true, false)]
        public override int uap_id { get; set; }

        /// <summary>
        /// Situação do registro (1 - Ativo, 3 - Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte uap_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime uap_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime uap_dataAlteracao { get; set; }

    }
}