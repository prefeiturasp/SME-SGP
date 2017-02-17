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
	public class ACA_TipoDisciplina : Abstract_ACA_TipoDisciplina
	{
        [MSValidRange(100, "Tipo de [MSG_DISCIPLINA] pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de [MSG_DISCIPLINA] é obrigatório.")]
        public override string tds_nome { get; set; }
        [MSNotNullOrEmpty("Base é obrigatório.")]
        public override byte tds_base { get; set; }
        [MSDefaultValue(1)]
        public override byte tds_situacao { get; set; }        
        public override DateTime tds_dataCriacao { get; set; }
        public override DateTime tds_dataAlteracao { get; set; }

        [MSValidRange(100, "Tipo especial de [MSG_DISCIPLINA] pode conter até 200 caracteres.")]
        public override string tds_nomeDisciplinaEspecial { get; set; }
        public override byte tds_tipo { get; set; }
	}
}