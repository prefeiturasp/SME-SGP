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
	public class ESC_EscolaDiretor : Abstract_ESC_EscolaDiretor
	{
        [MSNotNullOrEmpty("Colaborador é obrigatório.")]
        public override Int64 col_id { get; set; }
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime esd_vigenciaInicio { get; set; }
        [MSDefaultValue(false)]
        public override bool esd_geralEscola { get; set; }
        [MSDefaultValue(1)]
        public override byte esd_situacao { get; set; }        
        public override DateTime esd_dataCriacao { get; set; }
        public override DateTime esd_dataAlteracao { get; set; }
	}
}