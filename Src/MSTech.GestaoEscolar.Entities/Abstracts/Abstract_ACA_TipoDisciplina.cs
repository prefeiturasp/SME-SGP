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
    public abstract class Abstract_ACA_TipoDisciplina : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int tds_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int tne_id { get; set; }
		public virtual int mds_id { get; set; }
		[MSValidRange(100)]
		[MSNotNullOrEmpty()]
		public virtual string tds_nome { get; set; }
		public virtual int tds_ordem { get; set; }
		[MSNotNullOrEmpty()]
        public virtual byte tds_base { get; set; }
        public virtual int aco_id { get; set; }
        [MSNotNullOrEmpty()]
        public virtual byte tds_tipo { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tds_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tds_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tds_dataAlteracao { get; set; }
        [MSValidRange(200)]
        public virtual string tds_nomeDisciplinaEspecial { get; set; }
        [MSDefaultValue(0)]
        public virtual int tds_qtdeDisciplinaRelacionada { get; set; }
    }
}