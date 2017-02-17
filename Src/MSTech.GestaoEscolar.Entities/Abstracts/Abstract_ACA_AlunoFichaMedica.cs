/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
    public abstract class Abstract_ACA_AlunoFichaMedica : Abstract_Entity
    {
		[MSNotNullOrEmpty()] 
		[DataObjectField(true, true, false)]
		public virtual Int64 alu_id { get; set; }
        [MSValidRange(5)]
        public virtual string afm_tipoSanguineo { get; set; }
        [MSValidRange(5)]
        public virtual string afm_fatorRH { get; set; }
        public virtual string afm_doencasConhecidas { get; set; }
        public virtual string afm_alergias { get; set; }
        public virtual string afm_medicacoesPodeUtilizar { get; set; }
        public virtual string afm_medicacoesUsoContinuo { get; set; }
        [MSValidRange(1000)]
        public virtual string afm_convenioMedico { get; set; }
        [MSValidRange(1000)]
        public virtual string afm_hospitalRemocao { get; set; }
        public virtual string afm_outrasRecomendacoes { get; set; }

    }
}