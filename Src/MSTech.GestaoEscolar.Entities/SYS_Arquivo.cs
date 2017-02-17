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
	public class SYS_Arquivo : Abstract_SYS_Arquivo
	{
        [DataObjectField(true, true, false)]
        public override Int64 arq_id { get; set; }                    
        
        [MSValidRange(256)]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string arq_nome { get; set; }

        [MSNotNullOrEmpty("Tamanho é obrigatório.")]
        public override Int64 arq_tamanhoKB { get; set; }

        [MSValidRange(200)]
        [MSNotNullOrEmpty("Tipo é obrigatório.")]
        public override string arq_typeMime { get; set; }

        [MSNotNullOrEmpty("Arquivo é obrigatório.")]
        public override byte[] arq_data { get; set; }

        [MSDefaultValue(1)]
        public override byte arq_situacao { get; set; }

        public override DateTime arq_dataCriacao { get; set; }
        public override DateTime arq_dataAlteracao { get; set; }
	}
}