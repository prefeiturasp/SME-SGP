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
    public abstract class Abstract_CLS_PlanejamentoProjeto : Abstract_Entity
    {
		
		/// <summary>
		/// ID da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// ID da unidade escolar.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int uni_id { get; set; }

		/// <summary>
		/// ID do calendario anual.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// ID do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// ID do planejamento do projeto.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int plp_id { get; set; }

		/// <summary>
		/// ID do tipo de disciplina que criou o projeto.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
		public virtual int tds_id { get; set; }

        /// <summary>
        /// ID do tipo de turma disciplina que criou o projeto.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual byte tud_tipo { get; set; }

		/// <summary>
		/// Nome do projeto.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		public virtual string plp_nome { get; set; }

		/// <summary>
		/// Descrição do projeto.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual string plp_descricao { get; set; }

		/// <summary>
		/// Data inicio da duração do planejamento do projeto.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime plp_dataInicio { get; set; }

		/// <summary>
		/// Data fim da duração do planejamento do projeto.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime plp_dataFim { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short plp_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime plp_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime plp_dataAlteracao { get; set; }

    }
}