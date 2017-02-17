/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ESC_EscolaClassificacao : Abstract_ESC_EscolaClassificacao
	{
        /// <summary>
        /// ID da escola.
        /// </summary>
        [MSNotNullOrEmpty("Escola é obrigatorio.")]
        [DataObjectField(true, false, false)]
        public override int esc_id { get; set; }

        /// <summary>
        /// ID do tipo de classificação da escola.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de classificação de escola é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tce_id { get; set; }

        /// <summary>
        /// Id da vigência de classificação.
        /// </summary>
        [MSNotNullOrEmpty("Vigência de classificação de escola é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long ecv_id { get; set; }

        /// <summary>
        /// Nome do tipo de classificação.
        /// </summary>
        public virtual string tce_nome { get; set; }

        /// <summary>
        /// Vigência da classificação (Data início - Data final).
        /// </summary>
        public virtual string vigencia { get; set; }
	
    }
}