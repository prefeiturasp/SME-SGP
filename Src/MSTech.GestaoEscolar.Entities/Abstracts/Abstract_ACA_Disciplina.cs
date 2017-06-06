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
    public abstract class Abstract_ACA_Disciplina : Abstract_Entity
    {
		
		/// <summary>
		/// Campo Id da tabela ACA_Disciplina.
		/// </summary>
		[MSNotNullOrEmpty("[dis_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int dis_id { get; set; }

		/// <summary>
		/// Campo Id da tabela ACA_TipoDisciplina.
		/// </summary>
		[MSNotNullOrEmpty("[tds_id] é obrigatório.")]
		public virtual int tds_id { get; set; }

		/// <summary>
		/// Codigo da disciplina eletiva..
		/// </summary>
		[MSValidRange(10)]
		public virtual string dis_codigo { get; set; }

		/// <summary>
		/// Nome da disciplina.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty("[dis_nome] é obrigatório.")]
		public virtual string dis_nome { get; set; }

		/// <summary>
		/// Nome abreviado da disciplina..
		/// </summary>
		[MSValidRange(20)]
		public virtual string dis_nomeAbreviado { get; set; }

		/// <summary>
		/// Ementa da disciplina..
		/// </summary>
		public virtual string dis_ementa { get; set; }

		/// <summary>
		/// Carga horária teórica..
		/// </summary>
		public virtual int dis_cargaHorariaTeorica { get; set; }

		/// <summary>
		/// Carga horária prática..
		/// </summary>
		public virtual int dis_cargaHorariaPratica { get; set; }

		/// <summary>
		/// Carga horária supervisionada..
		/// </summary>
		public virtual int dis_cargaHorariaSupervisionada { get; set; }

		/// <summary>
		/// Carga horária extra..
		/// </summary>
		public virtual int dis_cargaHorariaExtra { get; set; }

		/// <summary>
		/// 1–Ativo, 3–Excluído, 4-Inativo.
		/// </summary>
		[MSNotNullOrEmpty("[dis_situacao] é obrigatório.")]
		public virtual byte dis_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[dis_dataCriacao] é obrigatório.")]
		public virtual DateTime dis_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[dis_dataAlteracao] é obrigatório.")]
		public virtual DateTime dis_dataAlteracao { get; set; }

		/// <summary>
		/// Carga horária anual..
		/// </summary>
		public virtual int dis_cargaHorariaAnual { get; set; }

		/// <summary>
		/// Nome da disciplina para documentações.
		/// </summary>
		[MSValidRange(40)]
		public virtual string dis_nomeDocumentacao { get; set; }

		/// <summary>
		/// Objetivos (disciplinas eletivas).
		/// </summary>
		public virtual string dis_objetivos { get; set; }

		/// <summary>
		/// Competências e Habilidades (disciplinas eletivas) .
		/// </summary>
		public virtual string dis_habilidades { get; set; }

		/// <summary>
		/// Atividades e Metodologias (disciplinas eletivas) .
		/// </summary>
		public virtual string dis_metodologias { get; set; }

		/// <summary>
		/// Carga horária extra classe da disciplina no curso..
		/// </summary>
		public virtual decimal dis_cargaHorariaExtraClasse { get; set; }

    }
}