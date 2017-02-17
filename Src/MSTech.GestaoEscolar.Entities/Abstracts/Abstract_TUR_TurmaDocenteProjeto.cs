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
    public abstract class AbstractTUR_TurmaDocenteProjeto : Abstract_Entity
    {
		
		/// <summary>
		/// ID da turma..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tur_id { get; set; }

		/// <summary>
		/// ID do turma docente projeto..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tdp_id { get; set; }

		/// <summary>
		/// ID do docente..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual long doc_id { get; set; }

		/// <summary>
		/// ID do colaborador relacionado ao docente..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual long col_id { get; set; }

		/// <summary>
		/// ID do cargo do docente relacionado à disciplina..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crg_id { get; set; }

		/// <summary>
		/// ID do relacionamento do cargo do colaborador..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int coc_id { get; set; }

		/// <summary>
		/// Posição do docente.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tdp_posicao { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3 - Excluído, 4 - Inativo).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short tdp_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tdp_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tdp_dataAlteracao { get; set; }

    }
}