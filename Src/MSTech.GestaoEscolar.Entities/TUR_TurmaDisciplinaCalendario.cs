/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
	public class TUR_TurmaDisciplinaCalendario : Abstract_TUR_TurmaDisciplinaCalendario
	{
        [DataObjectField(true, false, false)]
        public override Int64 tud_id { get; set; }
        [DataObjectField(true, false, false)]
        public override int tpc_id { get; set; }
	}
}