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
	public class RHU_ColaboradorCargoDisciplina : Abstract_RHU_ColaboradorCargoDisciplina
	{
        [MSNotNullOrEmpty("Colaborador é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override Int64 col_id { get; set; }
        [MSNotNullOrEmpty("Cargo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int crg_id { get; set; }
        [MSNotNullOrEmpty("Cargo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int coc_id { get; set; }
        [MSNotNullOrEmpty("[MSG_DISCIPLINA] é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tds_id { get; set; }
	}
}