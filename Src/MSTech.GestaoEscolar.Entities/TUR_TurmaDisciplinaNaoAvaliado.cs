/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
	public class TUR_TurmaDisciplinaNaoAvaliado : Abstract_TUR_TurmaDisciplinaNaoAvaliado
	{
        /// <summary>
        /// ID da disciplina na turma
        /// </summary>
        [MSNotNullOrEmpty("[MSG_DISCIPLINA] é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override Int64 tud_id { get; set; }

        /// <summary>
        /// ID do formato de avaliação
        /// </summary>
        [MSNotNullOrEmpty("Formato de avaliação é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int fav_id { get; set; }

        /// <summary>
        /// ID da avaliação
        /// </summary>
        [MSNotNullOrEmpty("Avaliação é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int ava_id { get; set; }
	}
}