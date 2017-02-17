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
    [Serializable()]
    public class ACA_AlunoHistoricoObservacao : Abstract_ACA_AlunoHistoricoObservacao
    {
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public override Int64 alu_id { get; set; }       
        [DataObjectField(true, false, false)]
        public override int aho_id { get; set; }
        [MSValidRange(4000, "Observação do histórico pode conter até 4000 caracteres.")]
        public override string aho_observacao { get; set; }        
        [MSDefaultValue(1)]
        public override byte aho_situacao { get; set; }        
        public override DateTime aho_dataCriacao { get; set; }        
        public override DateTime aho_dataAlteracao { get; set; }
    }
}