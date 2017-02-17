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
    public abstract class Abstract_TUR_TurmaDocente : Abstract_Entity
    {

		/// <summary>
		/// ID do relacionamento da disciplina com a turma.
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual Int64 tud_id { get; set; }

		/// <summary>
		/// ID da turma docente.
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int tdt_id { get; set; }

		/// <summary>
		/// ID do docente.
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual Int64 doc_id { get; set; }

		/// <summary>
		/// ID do colaborador relacionado ao docente.
		/// </summary>
		public virtual Int64 col_id { get; set; }

		/// <summary>
		/// ID do cargo do docente relacionado à disciplina.
		/// </summary>
		public virtual int crg_id { get; set; }

		/// <summary>
		/// ID do relacionamento do cargo do colaborador.
		/// </summary>
		public virtual int coc_id { get; set; }

		/// <summary>
		/// Tipo (1 - Regular)
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte tdt_tipo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool tdt_horarioSobreposto { get; set; }

		/// <summary>
		/// Inicio de vigência do docente
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime tdt_vigenciaInicio { get; set; }

		/// <summary>
		/// Fim de vigência do docente
		/// </summary>
		public virtual DateTime tdt_vigenciaFim { get; set; }

		/// <summary>
		/// Posição do docente (1 ou 2)
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte tdt_posicao { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3 - Excluído, 4 - Inativo)
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte tdt_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime tdt_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime tdt_dataAlteracao { get; set; }

    }
}