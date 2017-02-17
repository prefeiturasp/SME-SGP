/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System.ComponentModel;
    using MSTech.Validation;
	using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class LOG_MovimentacaoAcertoSituacaoAluno : AbstractLOG_MovimentacaoAcertoSituacaoAluno
	{
        /// <summary>
        /// Id do log de acerto de situação do aluno.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int mta_id { get; set; }
	}
}