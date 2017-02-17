/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class TUR_ItemAvaliacao : Abstract_TUR_ItemAvaliacao
	{
        [MSValidRange(100)]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string iav_nome { get; set; }        
        [MSNotNullOrEmpty("Situação é obrigatório.")]
        public override byte iav_situacao { get; set; }
	}
}