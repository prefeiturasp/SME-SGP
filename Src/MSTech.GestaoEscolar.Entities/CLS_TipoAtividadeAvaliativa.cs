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
	public class CLS_TipoAtividadeAvaliativa : Abstract_CLS_TipoAtividadeAvaliativa
	{        
        public override int tav_id { get; set; }
        [MSValidRange(100, "Nome do tipo de atividade avaliativa pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome do tipo de atividade avaliativa é obrigatório.")]
        public override string tav_nome { get; set; }
        [MSDefaultValue(1)]
        public override byte tav_situacao { get; set; }
        public override DateTime tav_dataAlteracao { get; set; }
        public override DateTime tav_dataCriacao { get; set; }
        public override int qat_id { get; set; }
        public string qat_nome { get; set; }
	}
}