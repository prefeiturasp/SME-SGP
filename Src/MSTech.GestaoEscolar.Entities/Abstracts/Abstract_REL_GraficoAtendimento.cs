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
    public abstract class Abstract_REL_GraficoAtendimento : Abstract_Entity
    {
		
		/// <summary>
		/// Id do gráfico gerado automaticamente.
		/// </summary>
		[MSNotNullOrEmpty("[gra_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int gra_id { get; set; }

		/// <summary>
		/// ID do relatorio de atendimento relacionado - CLS_RelatorioAtendimento.
		/// </summary>
		[MSNotNullOrEmpty("[rea_id] é obrigatório.")]
		public virtual int rea_id { get; set; }

		/// <summary>
		/// Titulo do grafico.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty("[gra_titulo] é obrigatório.")]
		public virtual string gra_titulo { get; set; }

		/// <summary>
		/// Tipo do grafico, enumerador, 1 - Barra.
		/// </summary>
		[MSNotNullOrEmpty("[gra_tipo] é obrigatório.")]
		public virtual byte gra_tipo { get; set; }

		/// <summary>
		/// Eixo de agrupamento, enumerador, 1- Curso, 2- Ciclo, 3- Periodo do Curso.
		/// </summary>
		[MSNotNullOrEmpty("[gra_eixo] é obrigatório.")]
		public virtual byte gra_eixo { get; set; }

		/// <summary>
		/// Situacao do grafico, 1- Ativo, 3 - Excluido.
		/// </summary>
		[MSNotNullOrEmpty("[gra_situacao] é obrigatório.")]
		public virtual byte gra_situacao { get; set; }

		/// <summary>
		/// Data da criacao do grafico.
		/// </summary>
		[MSNotNullOrEmpty("[gra_dataCriacao] é obrigatório.")]
		public virtual DateTime gra_dataCriacao { get; set; }

		/// <summary>
		/// Data da ultima alteracao do grafico.
		/// </summary>
		[MSNotNullOrEmpty("[gra_dataAlteracao] é obrigatório.")]
		public virtual DateTime gra_dataAlteracao { get; set; }

    }
}