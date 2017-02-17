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
    public abstract class Abstract_MTR_MatriculaTurma : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 alu_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int mtu_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual Int64 tur_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int crp_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int alc_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime mtu_dataMatricula { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(20)]
		public virtual string mtu_avaliacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual decimal mtu_frequencia { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string mtu_relatorio { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual byte mtu_resultado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual DateTime mtu_dataSaida { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int mtu_numeroChamada { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte mtu_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime mtu_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime mtu_dataAlteracao { get; set; }

        public virtual Guid usu_idResultado { get; set; }
    }
}