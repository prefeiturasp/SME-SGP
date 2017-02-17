/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using MSTech.Data.Common.Abstracts;
    using MSTech.Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
    public abstract class AbstractACA_AlunoHistorico : Abstract_Entity
    {
        /// <summary>
        /// Id do aluno.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual long alu_id { get; set; }

        /// <summary>
        /// ID do historico do aluno.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int alh_id { get; set; }

        /// <summary>
        /// Id do curso do aluno.
        /// </summary>
        public virtual int cur_id { get; set; }

        /// <summary>
        /// Id do curriculo do curso.
        /// </summary>
        public virtual int crr_id { get; set; }

        /// <summary>
        /// Id do período do curso.
        /// </summary>
        public virtual int crp_id { get; set; }

        /// <summary>
        /// Id da escola.
        /// </summary>
        public virtual int esc_id { get; set; }

        /// <summary>
        /// Id da unidade da escola.
        /// </summary>
        public virtual int uni_id { get; set; }

        /// <summary>
        /// Id da matrícula turma do aluno.
        /// </summary>
        public virtual int mtu_id { get; set; }

        /// <summary>
        /// Id da observação do histórico.
        /// </summary>
        public virtual int aho_id { get; set; }

        /// <summary>
        /// Id da escola de origem.
        /// </summary>
        public virtual long eco_id { get; set; }

        /// <summary>
        /// Ano letivo do histórico do aluno.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual int alh_anoLetivo { get; set; }

        /// <summary>
        /// Resultado do histórico do aluno (1-Aprovado, 2-Reprovado, 3-Reprovado por frequência).
        /// </summary>
        public virtual short alh_resultado { get; set; }

        /// <summary>
        /// Descrição do histórico do resultado do aluno.
        /// </summary>
        [MSValidRange(30)]
        public virtual string alh_resultadoDescricao { get; set; }

        /// <summary>
        /// Avaliação do histórico do aluno.
        /// </summary>
        [MSValidRange(100)]
        public virtual string alh_avaliacao { get; set; }

        /// <summary>
        /// Frequência do histórico do aluno.
        /// </summary>
        [MSValidRange(100)]
        public virtual string alh_frequencia { get; set; }

        /// <summary>
        /// Quantidade de faltas do histórico do aluno.
        /// </summary>
        public virtual int alh_qtdeFaltas { get; set; }

        /// <summary>
        /// Controle de notas (1-Global, 2-Por disciplina,  3-Global e por disciplina).
        /// </summary>
        public virtual short alh_tipoControleNotas { get; set; }

        /// <summary>
        /// Carga horária da base nacional.
        /// </summary>
        public virtual int alh_cargaHorariaBaseNacional { get; set; }

        /// <summary>
        /// Carga horária da base diversificada.
        /// </summary>
        public virtual int alh_cargaHorariaBaseDiversificada { get; set; }

        /// <summary>
        /// Descrição do próximo ano letivo do histórico do aluno.
        /// </summary>
        [MSValidRange(200)]
        public virtual string alh_descricaoProximoPeriodo { get; set; }
        
        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual short alh_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime alh_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime alh_dataAlteracao { get; set; }
    }
}