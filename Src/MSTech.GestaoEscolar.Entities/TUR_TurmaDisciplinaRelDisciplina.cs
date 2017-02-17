/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
	public class TUR_TurmaDisciplinaRelDisciplina : Abstract_TUR_TurmaDisciplinaRelDisciplina
	{
        [DataObjectField(true, false, false)]
        public override Int64 tud_id { get; set; }        
        [DataObjectField(true, false, false)]
        public override int dis_id { get; set; }        
	}
}