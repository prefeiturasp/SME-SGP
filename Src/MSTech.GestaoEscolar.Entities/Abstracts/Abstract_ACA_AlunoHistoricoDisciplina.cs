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
    public abstract class AbstractACA_AlunoHistoricoDisciplina : Abstract_Entity
    {
        /// <summary>
        /// ID Aluno.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual long alu_id { get; set; }

        /// <summary>
        /// ID AlunoHistorico.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int alh_id { get; set; }

        /// <summary>
        /// ID AlunoHistoricoDisciplina.
        /// </summary>
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public virtual int ahd_id { get; set; }

        /// <summary>
        /// ID AlunoHistoricoProjeto.
        /// </summary>
        public virtual int ahp_id { get; set; }

        /// <summary>
        /// ID TipoDisciplina.
        /// </summary>
        public virtual int tds_id { get; set; }

        /// <summary>
        /// Nome da disciplina.
        /// </summary>
        [MSValidRange(200)]
        [MSNotNullOrEmpty]
        public virtual string ahd_disciplina { get; set; }

        /// <summary>
        /// 1 - aprovado, 2 - reprovado, 3 - reprovado por frequência.
        /// </summary>
        public virtual short ahd_resultado { get; set; }

        /// <summary>
        /// Avaliação do aluno na disciplina do histórico.
        /// </summary>
        [MSValidRange(100)]
        public virtual string ahd_avaliacao { get; set; }

        /// <summary>
        /// Frequência do aluno na disciplina do histórico.
        /// </summary>
        [MSValidRange(100)]
        public virtual string ahd_frequencia { get; set; }

        /// <summary>
        /// 1 - Ativo, 3 - Excluído.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual short ahd_situacao { get; set; }

        /// <summary>
        /// Propriedade ahd_resultadoDescricao.
        /// </summary>
        [MSValidRange(30)]
        public virtual string ahd_resultadoDescricao { get; set; }

        /// <summary>
        /// Propriedade ahd_indicacaoDependencia.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual bool ahd_indicacaoDependencia { get; set; }

        /// <summary>
        /// Quantidade de faltas no ano para a disciplina
        /// </summary>
        public virtual int ahd_qtdeFaltas { get; set; }
    }
}