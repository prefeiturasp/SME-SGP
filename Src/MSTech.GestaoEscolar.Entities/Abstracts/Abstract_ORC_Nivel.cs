/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class Abstract_ORC_Nivel : Abstract_Entity
    {
		
		/// <summary>
		/// ID do nível.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int nvl_id { get; set; }

		/// <summary>
		/// ID do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// ID do curriculo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// ID do período.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crp_id { get; set; }

		/// <summary>
		/// ID do calendário associado ao nível.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// Ordem do nível.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int nvl_ordem { get; set; }

		/// <summary>
		/// Nome do nível.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string nvl_nome { get; set; }

		/// <summary>
		/// Situação do nível (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte nvl_situacao { get; set; }

		/// <summary>
		/// Data de criação do nível.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime nvl_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do nível.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime nvl_dataAlteracao { get; set; }

		/// <summary>
		/// ID do tipo de disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tds_id { get; set; }

		/// <summary>
		/// Nome no plural do parametro de alerta.
		/// </summary>
		[MSValidRange(100)]
		public virtual string nvl_nomePlural { get; set; }

		/// <summary>
		/// Propriedade mat_id.
		/// </summary>
		public virtual long mat_id { get; set; }

    }
}