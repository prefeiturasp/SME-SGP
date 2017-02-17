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
    public abstract class Abstract_ACA_Disciplina : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int dis_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int tds_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(10)]
		public virtual string dis_codigo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string dis_nome { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(20)]
		public virtual string dis_nomeAbreviado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(40)]
		public virtual string dis_nomeDocumentacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string dis_ementa { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string dis_objetivos { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string dis_habilidades { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string dis_metodologias { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int dis_cargaHorariaTeorica { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int dis_cargaHorariaPratica { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int dis_cargaHorariaSupervisionada { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int dis_cargaHorariaExtra { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int dis_cargaHorariaAnual { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte dis_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime dis_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime dis_dataAlteracao { get; set; }

    }
}