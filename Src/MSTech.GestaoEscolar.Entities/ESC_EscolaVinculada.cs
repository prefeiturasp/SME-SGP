/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ESC_EscolaVinculada : Abstract_ESC_EscolaVinculada
	{
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime esv_vigenciaInicio { get; set; }        
        [MSNotNullOrEmpty("Situação é obrigatório.")]
        public override byte esv_situacao { get; set; }
        [MSNotNullOrEmpty("Data de criação é obrigatório.")]
        public override DateTime esv_dataCriacao { get; set; }
        [MSNotNullOrEmpty("Data de alteração é obrigatório.")]
        public override DateTime esv_dataAlteracao { get; set; }        
	}
}