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
    public abstract class AbstractMTR_Matricula : Abstract_Entity
    {
		
		/// <summary>
        /// Id do processo de fechamento/início do ano letivo.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int pfi_id { get; set; }

		/// <summary>
        /// Id do aluno.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long alu_id { get; set; }

		/// <summary>
		/// Id da renovação (pré-matrícula).
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int mtr_id { get; set; }

		/// <summary>
		/// Id da escola de destino.
		/// </summary>
		public virtual int esc_id { get; set; }

		/// <summary>
		/// Id da unidade de destino.
		/// </summary>
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Id do curso de destino.
		/// </summary>
		public virtual int cur_id { get; set; }

		/// <summary>
		/// Id do curriculo de destino.
		/// </summary>
		public virtual int crr_id { get; set; }

		/// <summary>
		/// Id do período do curso de destino.
		/// </summary>
		public virtual int crp_id { get; set; }

		/// <summary>
		/// Id do tipo de turno de destino.
		/// </summary>
		public virtual int ttn_id { get; set; }

		/// <summary>
		/// Id do tipo de movimentação.
		/// </summary>
		public virtual int tmo_id { get; set; }

		/// <summary>
        /// Id da movimentação gerada no fechamento de matrícula.
		/// </summary>
		public virtual int mov_id { get; set; }

		/// <summary>
		/// Id da turma de destino.
		/// </summary>
		public virtual long tur_id { get; set; }

		/// <summary>
        /// Id do tipo de movimentação do arquivo de importação.
		/// </summary>
		public virtual int tmo_idImportado { get; set; }

		/// <summary>
        /// Id da escola de origem / destino.
		/// </summary>
		public virtual long eco_id { get; set; }

		/// <summary>
        /// Id da cidade de origem / destino.
		/// </summary>
		public virtual Guid cid_id { get; set; }

		/// <summary>
        /// Id da unidade federativa de origem / destino.
		/// </summary>
		public virtual Guid unf_id { get; set; }

		/// <summary>
        /// Avaliação do aluno.
		/// </summary>
		public virtual string mda_avaliacao { get; set; }

		/// <summary>
        /// Observações informadas na movimentação do aluno.
		/// </summary>
		public virtual string mda_observacao { get; set; }

        /// <summary>
        /// Indica se o aluno conclui  nível de ensino
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool mtr_concluiNivelEnsino { get; set; }

		/// <summary>
        /// Número da avaliação de destino.
		/// </summary>
		public virtual int mtr_numeroAvaliacao { get; set; }

		/// <summary>
        /// Número de matrícula do aluno.
		/// </summary>
		[MSValidRange(50)]
		public virtual string mtr_numeroMatricula { get; set; }

		/// <summary>
		/// Tipo de processo (1-Renovação,2-Importação do sistema Matrícula Digital,3-Matrícula de alunos oriundos de creches conveniadas,4-Matrícula de alunos oriundos de creches conveniadas - pelo sistema Inscrição Creche).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short mtr_tipoProcesso { get; set; }

		/// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído, 4- Matriculado, 5-Inativo).
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short mtr_situacao { get; set; }

		/// <summary>
        /// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mtr_dataCriacao { get; set; }

		/// <summary>
        /// Data da última alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime mtr_dataAlteracao { get; set; }

    }
}