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
    public abstract class Abstract_CLS_FrequenciaReuniaoResponsaveis : Abstract_Entity
    {

		/// <summary>
		/// ID do aluno
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 alu_id { get; set; }

		/// <summary>
		/// ID do calendário
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// ID do período do calendário
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cap_id { get; set; }

		/// <summary>
		/// ID sequêncial da reunião por período do calendário
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int frp_id { get; set; }

		/// <summary>
		/// Flag que indica se o aluno compareceu ou não à reunião
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool frp_frequencia { get; set; }

		/// <summary>
		/// Data de criação
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime frp_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime frp_dataAlteracao { get; set; }

    }
}