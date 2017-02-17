/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CLS_CompensacaoAusenciaAluno : AbstractCLS_CompensacaoAusenciaAluno
	{
        /// <summary>
        /// Id da tabela TUR_TurmaDisciplina.
        /// </summary>
        [DataObjectField(true, false, false)]
        public virtual long tud_id { get; set; }

        /// <summary>
        /// Id da tabela CLS_CompensacaoAusencia.
        /// </summary>
        [DataObjectField(true, false, false)]
        public virtual int cpa_id { get; set; }

        /// <summary>
        /// Id da tabela ACA_aluno.
        /// </summary>
        [DataObjectField(true, false, false)]
        public virtual long alu_id { get; set; }

        /// <summary>
        /// ID da tabela MTR_MatriculaTurma.
        /// </summary>
        [DataObjectField(true, false, false)]
        public virtual int mtu_id { get; set; }

        /// <summary>
        /// ID da tabela MTR_MatriculaTurmaDisciplina.
        /// </summary>
        [DataObjectField(true, false, false)]
        public virtual int mtd_id { get; set; }

        /// <summary>
        /// 1-Ativo, 3-Excluído.
        /// </summary>
        [MSDefaultValue(1)]
        public virtual short caa_situacao { get; set; }

        /// <summary>
        /// Data de criação.
        /// </summary>
        public virtual DateTime caa_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração.
        /// </summary>
        public virtual DateTime caa_dataAlteracao { get; set; }
	}
}