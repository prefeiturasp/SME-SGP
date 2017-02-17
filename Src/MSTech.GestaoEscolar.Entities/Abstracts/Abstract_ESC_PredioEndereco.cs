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
    public abstract class Abstract_ESC_PredioEndereco : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int prd_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int ped_id { get; set; }
		[MSNotNullOrEmpty()]
        public virtual Guid end_id { get; set; }
        public virtual Guid uae_id { get; set; }
		[MSValidRange(20)]
		[MSNotNullOrEmpty()]
		public virtual string ped_numero { get; set; }
		[MSValidRange(100)]
		public virtual string ped_complemento { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte ped_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime ped_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime ped_dataAlteracao { get; set; }
        public virtual bool ped_enderecoPrincipal { get; set; }
        public virtual decimal ped_latitude { get; set; }
        public virtual decimal ped_longitude { get; set; }
    }
}