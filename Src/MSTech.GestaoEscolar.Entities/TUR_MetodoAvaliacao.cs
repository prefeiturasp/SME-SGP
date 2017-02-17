/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class TUR_MetodoAvaliacao : Abstract_TUR_MetodoAvaliacao
	{
        [DataObjectField(true, true, false)]
        public override int mav_id { get; set; }
        [MSNotNullOrEmpty("Padrão é obrigatório.")]
        public override bool mav_padrao { get; set; }
        [MSValidRange(100)]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string mav_nome { get; set; }
        [MSNotNullOrEmpty("Situação é obrigatório.")]
        public override byte mav_situacao { get; set; }
        [MSNotNullOrEmpty("Data de criação é obrigatório.")]
        public override DateTime mav_dataCriacao { get; set; }
        [MSNotNullOrEmpty("Data de alteração é obrigatório.")]
        public override DateTime mav_dataAlteracao { get; set; }
	}
}