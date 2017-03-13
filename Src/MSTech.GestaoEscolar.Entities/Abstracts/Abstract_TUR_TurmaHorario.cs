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
    public abstract class Abstract_TUR_TurmaHorario : Abstract_Entity
    {
		
		/// <summary>
		/// Campo Id da tabela ACA_Turno.
		/// </summary>
		[MSNotNullOrEmpty("[trn_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int trn_id { get; set; }

		/// <summary>
		/// Campo Id da tabela ACA_TurnoHorario.
		/// </summary>
		[MSNotNullOrEmpty("[trh_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int trh_id { get; set; }

		/// <summary>
		/// Campo Id da tabela TUR_TurmaHorario.
		/// </summary>
		[MSNotNullOrEmpty("[thr_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int thr_id { get; set; }

		/// <summary>
		/// Campo Id da tabela TUR_TurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty("[tud_id] é obrigatório.")]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Data inicial da vigencia do horario na turma.
		/// </summary>
		public virtual DateTime thr_vigenciaInicio { get; set; }

		/// <summary>
		/// Data final da vigencia do horario na turma.
		/// </summary>
		public virtual DateTime thr_vigenciaFim { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[thr_situacao] é obrigatório.")]
		public virtual byte thr_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[thr_dataCriacao] é obrigatório.")]
		public virtual DateTime thr_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[thr_dataAlteracao] é obrigatório.")]
		public virtual DateTime thr_dataAlteracao { get; set; }

		/// <summary>
		/// Indica se o registro foi criado por integração com outro sistema.
		/// </summary>
		[MSNotNullOrEmpty("[thr_registroExterno] é obrigatório.")]
		public virtual bool thr_registroExterno { get; set; }

    }
}