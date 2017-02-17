/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System.ComponentModel;
using MSTech.Validation;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MTR_ParametroFormacaoTurmaTurno : Abstract_MTR_ParametroFormacaoTurmaTurno
	{
        [MSNotNullOrEmpty("Processo fechamento/início ano letivo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int pfi_id { get; set; }

        [MSNotNullOrEmpty("Parâmetro por período é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int pft_id { get; set; }

        [MSNotNullOrEmpty("Turno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int trn_id { get; set; }
	}
}