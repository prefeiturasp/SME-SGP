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
    public abstract class Abstract_TUR_TurmaDisciplinaAulaPrevista : Abstract_Entity
    {
		
		/// <summary>
		/// Campo Id da tabela TUR_TurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// Campo Id da tabela ACA_TipoPeriodoCalendario.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Quantidade de aulas previstas..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tap_aulasPrevitas { get; set; }

        /// <summary>
        /// Indica se os registros de frequência dos alunos foram recalculados utilizando aulas previstas.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool tap_registrosCorrigidos { get; set; }

		/// <summary>
		/// 1 - Ativo, 3 - Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tap_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tap_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tap_dataAlteracao { get; set; }

    }
}