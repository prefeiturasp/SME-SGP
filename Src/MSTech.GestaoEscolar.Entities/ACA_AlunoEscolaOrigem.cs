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
	public class ACA_AlunoEscolaOrigem : Abstract_ACA_AlunoEscolaOrigem
	{
        [MSNotNullOrEmpty]
        [DataObjectField(true, true, false)]
        public override Int64 eco_id { get; set; }
        [MSValidRange(200, "Nome pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string eco_nome { get; set; }
        [MSValidRange(20, "Código INEP pode conter até 20 caracteres.")]
        public override string eco_codigoInep { get; set; }
        [MSValidRange(10, "Número pode conter até 10 caracteres.")]
        public override string eco_numero { get; set; }
        [MSDefaultValue(1)]
        public override byte eco_situacao { get; set; }
        public override DateTime eco_dataCriacao { get; set; }
        public override DateTime eco_dataAlteracao { get; set; }
	}
}