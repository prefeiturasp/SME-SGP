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
    public abstract class Abstract_ACA_CompromissoEstudo : Abstract_Entity
    {
		
		/// <summary>
		/// Campo Id da tabela ACA_Aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Campo Id da tabela ACA_CompromissoEstudo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int cpe_id { get; set; }

		/// <summary>
		/// Atividade feita..
		/// </summary>
		public virtual string cpe_atividadeFeita { get; set; }

		/// <summary>
		/// Atividade que pretende fazer..
		/// </summary>
		public virtual string cpe_atividadePretendeFazer { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short cpe_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cpe_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime cpe_dataAlteracao { get; set; }

		/// <summary>
        /// Ano do compromisso de estudo.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cpe_ano { get; set; }

        /// <summary>
        /// Campo Id da tabela ACA_TipoPeriodoCalendario.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int tpc_id { get; set; }

    }
}