/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MTR_Configuracao : Abstract_MTR_Configuracao
	{
        [MSValidRange(100, "Nome deve conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string cfg_nome { get; set; }                
        [MSDefaultValue(1)]
        public override byte cfg_situacao { get; set; }
        public override DateTime cfg_dataCriacao { get; set; }
        public override DateTime cfg_dataAlteracao { get; set; }
        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }
	}
}