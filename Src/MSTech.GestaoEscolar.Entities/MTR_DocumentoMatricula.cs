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
	public class MTR_DocumentoMatricula : Abstract_MTR_DocumentoMatricula
	{        
        public override int cur_id { get; set; }
        [MSNotNullOrEmpty("Documento é obrigatório.")]
        public override Guid tdo_id { get; set; }
        [MSNotNullOrEmpty("Obrigatoriedade é obrigatório.")]
        public override byte dmt_obrigatoriedade { get; set; }
        [MSNotNullOrEmpty("Apresentação é obrigatório.")]
        public override byte dmt_apresentacao { get; set; }
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime dmt_vigenciaInicio { get; set; }        
        [MSDefaultValue(1)]
        public override byte dmt_situacao { get; set; }        
        public override DateTime dmt_dataCriacao { get; set; }        
        public override DateTime dmt_dataAlteracao { get; set; }
	}
}