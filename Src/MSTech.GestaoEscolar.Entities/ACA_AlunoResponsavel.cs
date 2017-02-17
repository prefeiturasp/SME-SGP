/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
    public class ACA_AlunoResponsavel : Abstract_ACA_AlunoResponsavel
	{
        [DataObjectField(true, false, false)]
        public override int alr_id { get; set; }
        [MSNotNullOrEmpty("Tipo de filiação / responsável é obrigatório.")]
        public override int tra_id { get; set; }
        public override Guid pes_id { get; set; }
        [MSDefaultValue(0)]
        public override bool alr_moraComAluno { get; set; }
        [MSDefaultValue(1)]
        public override byte alr_situacao { get; set; }
        public override DateTime alr_dataCriacao { get; set; }
        public override DateTime alr_dataAlteracao { get; set; }
	}
}