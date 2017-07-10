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
    public abstract class Abstract_CLS_RelatorioPreenchimentoAlunoTurmaDisciplina : Abstract_Entity
    {

        /// <summary>
        /// ID da tabela CLS_RelatorioPreenchimento, referente ao preenchimento de um relatório..
        /// </summary>
        [MSNotNullOrEmpty("[reap_id] é obrigatório.")]
        [DataObjectField(true, false, false)]
        public virtual long reap_id { get; set; }

        /// <summary>
        /// ID da tabela ACA_Aluno..
        /// </summary>
        [MSNotNullOrEmpty("[alu_id] é obrigatório.")]
        public virtual long alu_id { get; set; }

        /// <summary>
        /// ID da tabela TUR_Turma..
        /// </summary>
        [MSNotNullOrEmpty("[tur_id] é obrigatório.")]
        public virtual long tur_id { get; set; }

        /// <summary>
        /// ID da tabela TUR_TurmaDisciplina..
        /// </summary>
        public virtual long tud_id { get; set; }

        /// <summary>
        /// ID do tipo ACA_TipoPeriodoCalendario..
        /// </summary>
        public virtual int tpc_id { get; set; }

        /// <summary>
        /// Data de criação do registro..
        /// </summary>
        [MSNotNullOrEmpty("[ptd_dataCriacao] é obrigatório.")]
        public virtual DateTime ptd_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro..
        /// </summary>
        [MSNotNullOrEmpty("[ptd_dataAlteracao] é obrigatório.")]
        public virtual DateTime ptd_dataAlteracao { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído)..
        /// </summary>
        [MSNotNullOrEmpty("[ptd_situacao] é obrigatório.")]
        public virtual byte ptd_situacao { get; set; }

    }
}