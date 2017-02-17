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
    public abstract class AbstractACA_AlunoCurriculo : Abstract_Entity
    {
		
		/// <summary>
		/// Id do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// ID do currículo do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int alc_id { get; set; }

		/// <summary>
		/// Id da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Id da unidade da escola.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Id do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// Id do currículo do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// Id do período do curso.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int crp_id { get; set; }

		/// <summary>
		/// Número de matrícula do aluno.
		/// </summary>
		[MSValidRange(50)]
		public virtual string alc_matricula { get; set; }

		/// <summary>
		/// Código INEP do histórico do aluno.
		/// </summary>
		[MSValidRange(20)]
		public virtual string alc_codigoInep { get; set; }

        /// <summary>
        /// Registro geral do aluno.
        /// </summary>
        [MSValidRange(50)]
        public virtual string alc_registroGeral { get; set; }

		/// <summary>
		/// Data da matrícula do aluno.
		/// </summary>
		public virtual DateTime alc_dataPrimeiraMatricula { get; set; }

		/// <summary>
		/// Data de saída do aluno.
		/// </summary>
		public virtual DateTime alc_dataSaida { get; set; }

		/// <summary>
		/// Data da colação do aluno.
		/// </summary>
		public virtual DateTime alc_dataColacao { get; set; }

		/// <summary>
		/// Matrícula estadual do aluno.
		/// </summary>
		[MSValidRange(50)]
		public virtual string alc_matriculaEstadual { get; set; }

		/// <summary>
		/// Quantidade de impressões do histórico escolar do aluno.
		/// </summary>
		public virtual int alc_qtdeImpressoesHistorico { get; set; }

		/// <summary>
		/// Situação do registro (1-Ativo, 3-Excluído, 4-Inativo, 5-Formado, 6-Cancelado, 7- Em matrícula, 8- Excedente, 9-Evadido, 10- Em movimentação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short alc_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime alc_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime alc_dataAlteracao { get; set; }

    }
}