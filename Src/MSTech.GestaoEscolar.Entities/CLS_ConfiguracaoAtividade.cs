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
	public class CLS_ConfiguracaoAtividade : Abstract_CLS_ConfiguracaoAtividade
	{
        /// <summary>
        /// Id da configuração de atividade.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int caa_id { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override short caa_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime caa_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime caa_dataAlteracao { get; set; }
	}
}