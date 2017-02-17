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
    public abstract class Abstract_CLS_ConfiguracaoAtividade : Abstract_Entity
    {
		
		/// <summary>
		/// Id sequencial.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int caa_id { get; set; }

		/// <summary>
		/// Ano letivo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int caa_anoLetivo { get; set; }

		/// <summary>
		/// Id da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Id da unidade da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Id do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// Id do curriculo do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// Id do período do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crp_id { get; set; }

		/// <summary>
		/// Id da disciplina do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int dis_id { get; set; }

		/// <summary>
		/// Indica se a configuração possui atividade automatica.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool caa_possuiAtividadeAutomatica { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short caa_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime caa_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime caa_dataAlteracao { get; set; }

    }
}