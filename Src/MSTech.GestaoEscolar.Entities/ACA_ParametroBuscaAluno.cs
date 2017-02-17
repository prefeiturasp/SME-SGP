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
	public class ACA_ParametroBuscaAluno : Abstract_ACA_ParametroBuscaAluno
	{
        [MSNotNullOrEmpty("Tipo é obrigatório.")]
        public override byte pba_tipo { get; set; }
        [MSDefaultValue(1)]
        public override byte pba_situacao { get; set; }        
        public override DateTime pba_dataCriacao { get; set; }
        public override DateTime pba_dataAlteracao { get; set; }
	}
}