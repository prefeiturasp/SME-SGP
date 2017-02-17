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
    public abstract class Abstract_RHU_CargaHoraria : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, true, false)]
		public virtual int chr_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSValidRange(200)]
		public virtual string chr_descricao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool chr_padrao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual bool? chr_especialista { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int crg_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int chr_cargaHorariaSemanal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int chr_temposAula { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int chr_horasAula { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int chr_horasComplementares { get; set; }

		/// <summary>
		/// 1 – Ativo, 3 – Excluído
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte chr_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime chr_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime chr_dataAlteracao { get; set; }

    }
}