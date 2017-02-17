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
    public abstract class Abstract_MTR_MatriculaTurmaDisciplina : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 alu_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int mtd_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Int64 tud_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime mtd_dataMatricula { get; set; }
		public virtual int mtd_numeroChamada { get; set; }
		[MSValidRange(20)]
		public virtual string mtd_avaliacao { get; set; }
		public virtual decimal mtd_frequencia { get; set; }
		public virtual string mtd_relatorio { get; set; }
		public virtual byte mtd_resultado { get; set; }
		public virtual DateTime mtd_dataSaida { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte mtd_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime mtd_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime mtd_dataAlteracao { get; set; }

    }
}