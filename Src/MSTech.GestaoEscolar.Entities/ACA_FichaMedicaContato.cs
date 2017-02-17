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
	public class ACA_FichaMedicaContato : Abstract_ACA_FichaMedicaContato
	{
        [DataObjectField(true, false, false)]
        public override Int64 alu_id { get; set; }        
        [DataObjectField(true, false, false)]
        public override int fmc_id { get; set; }        
        [MSValidRange(100, "Nome pode conter até 100 caracteres.")]
        public override string fmc_nome { get; set; }
        [MSValidRange(100, "Telefone pode conter até 100 caracteres.")]
        public override string fmc_telefone { get; set; }
        public override int fmc_ordem { get; set; }  
	}
}