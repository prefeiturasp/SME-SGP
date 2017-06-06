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
    public abstract class Abstract_CLS_TurmaAtividadeExtraClasse : Abstract_Entity
    {
		
		/// <summary>
		/// Id da turma disciplina.
		/// </summary>
		[MSNotNullOrEmpty("[tud_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Id da atividade extraclasse.
		/// </summary>
		[MSNotNullOrEmpty("[tae_id] é obrigatório.")]
		[DataObjectField(true, false, false)]
		public virtual int tae_id { get; set; }

		/// <summary>
		/// Id do tipo de período do calendário.
		/// </summary>
		[MSNotNullOrEmpty("[tpc_id] é obrigatório.")]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Id do tipo de atividade.
		/// </summary>
		public virtual int tav_id { get; set; }

		/// <summary>
		/// Nome da atividade extraclasse.
		/// </summary>
		[MSValidRange(100)]
		public virtual string tae_nome { get; set; }

		/// <summary>
		/// Descrição da atividade extraclasse.
		/// </summary>
		public virtual string tae_descricao { get; set; }

		/// <summary>
		/// Carga horária da atividade extraclasse.
		/// </summary>
		[MSNotNullOrEmpty("[tae_cargaHoraria] é obrigatório.")]
		public virtual decimal tae_cargaHoraria { get; set; }

        /// <summary>
		/// Posição do docente.
		/// </summary>
		[MSNotNullOrEmpty("[tdt_posicao] é obrigatório.")]
        public virtual byte tdt_posicao { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[tae_situacao] é obrigatório.")]
		public virtual byte tae_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[tae_dataCriacao] é obrigatório.")]
		public virtual DateTime tae_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[tae_dataAlteracao] é obrigatório.")]
		public virtual DateTime tae_dataAlteracao { get; set; }

    }
}