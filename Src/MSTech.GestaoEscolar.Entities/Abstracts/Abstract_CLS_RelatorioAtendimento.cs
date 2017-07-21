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
    public abstract class Abstract_CLS_RelatorioAtendimento : Abstract_Entity
    {
		
		/// <summary>
		/// ID do relatório de atendimento.
		/// </summary>
		[MSNotNullOrEmpty("[rea_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int rea_id { get; set; }

		/// <summary>
		/// Título do relatório de atendimento.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty("[rea_titulo] é obrigatório.")]
		public virtual string rea_titulo { get; set; }

		/// <summary>
		/// Tipo do relatório de atendimento (1 - AEE, 2 - NAAPA, 3 - Recuperação Paralela).
		/// </summary>
		[MSNotNullOrEmpty("[rea_tipo] é obrigatório.")]
		public virtual byte rea_tipo { get; set; }

		/// <summary>
		/// Permite editar campo de Raça/Cor.
		/// </summary>
		[MSNotNullOrEmpty("[rea_permiteEditarRecaCor] é obrigatório.")]
		public virtual bool rea_permiteEditarRecaCor { get; set; }

		/// <summary>
		/// Permite editar campo de Hipótese Diagnóstica.
		/// </summary>
		[MSNotNullOrEmpty("[rea_permiteEditarHipoteseDiagnostica] é obrigatório.")]
		public virtual bool rea_permiteEditarHipoteseDiagnostica { get; set; }

        /// <summary>
		/// Permite editar campo de Ações Realizadas.
		/// </summary>
		[MSNotNullOrEmpty("[rea_permiteAcoesRealizadas] é obrigatório.")]
        public virtual bool rea_permiteAcoesRealizadas { get; set; }

        /// <summary>
        /// Indica se o relatório vai gerar pendência de fechamento
        /// </summary>
        [MSNotNullOrEmpty("[rea_gerarPendenciaFechamento] é obrigatório.")]
        public virtual bool rea_gerarPendenciaFechamento { get; set; }

        /// <summary>
        /// ID do tipo de disciplina do relatório de atendimento.
        /// </summary>
        public virtual int tds_id { get; set; }

		/// <summary>
		/// Periodicidade de preenchimento do relatório de atendimento (1 - Periódico, 2 - Encerramento).
		/// </summary>
		[MSNotNullOrEmpty("[rea_periodicidadePreenchimento] é obrigatório.")]
		public virtual byte rea_periodicidadePreenchimento { get; set; }

		/// <summary>
		/// ID do arquivo de anexo do relatório de atendimento.
		/// </summary>
		public virtual long arq_idAnexo { get; set; }

		/// <summary>
		/// Título do arquivo anexado no relatório de atendimento.
		/// </summary>
		[MSValidRange(256)]
		public virtual string rea_tituloAnexo { get; set; }

		/// <summary>
		/// Situação do registro (1 - Ativo, 3 - Excluido).
		/// </summary>
		[MSNotNullOrEmpty("[rea_situacao] é obrigatório.")]
		public virtual byte rea_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[rea_dataCriacao] é obrigatório.")]
		public virtual DateTime rea_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[rea_dataAlteracao] é obrigatório.")]
		public virtual DateTime rea_dataAlteracao { get; set; }

    }
}