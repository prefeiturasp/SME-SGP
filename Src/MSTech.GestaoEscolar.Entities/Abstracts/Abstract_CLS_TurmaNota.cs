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
    public abstract class Abstract_CLS_TurmaNota : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 tud_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int tnt_id { get; set; }
		public virtual int tpc_id { get; set; }
		public virtual int tau_id { get; set; }
		public virtual int tav_id { get; set; }
		[MSValidRange(100)]
		public virtual string tnt_nome { get; set; }
		public virtual DateTime tnt_data { get; set; }
		public virtual string tnt_descricao { get; set; }
		public virtual bool tnt_efetivado { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte tnt_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime tnt_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
        public virtual DateTime tnt_dataAlteracao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual byte tdt_posicao { get; set; }
        [MSNotNullOrEmpty()]
        public virtual bool tnt_exclusiva { get; set; }

        /// <summary>
        /// ID do usuário que criou a atividade.
        /// </summary>
        public virtual Guid usu_id { get; set; }

        /// <summary>
        /// ID do protocolo que criou a atividade (Diário de classe).
        /// </summary>
        public virtual Guid pro_id { get; set; }

        /// <summary>
        /// ID gerado pelo Diário de classe para a atividade.
        /// </summary>
        public virtual int tnt_chaveDiario { get; set; }

        /// <summary>
        /// Id do usuário que realizou a última alteração.
        /// </summary>
        public virtual Guid usu_idDocenteAlteracao { get; set; }

    }
}