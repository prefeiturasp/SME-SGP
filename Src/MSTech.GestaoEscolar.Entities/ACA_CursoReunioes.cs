/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ACA_CursoReunioes : Abstract_ACA_CursoReunioes
    {
        /// <summary>
        /// ID do curso
        /// </summary>
        [MSNotNullOrEmpty("Curso é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int cur_id { get; set; }

        /// <summary>
        /// ID do currículo do curso
        /// </summary>
        [MSNotNullOrEmpty("Currículo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int crr_id { get; set; }

        /// <summary>
        /// ID do calendário
        /// </summary>
        [MSNotNullOrEmpty("Calendário é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int cal_id { get; set; }

        /// <summary>
        /// ID de curso reunião
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int crn_id { get; set; }

        /// <summary>
        /// Quantidade de reuniões por período do calendário
        /// </summary>
        [MSNotNullOrEmpty("Quantidade de reuniões por período do calendário é obrigatório e deve ser um número inteiro maior que 0 (zero).")]
        public override int crn_qtde { get; set; }

        /// <summary>
        /// Situação do registro: 1-Ativo, 3-Excluído
        /// </summary>
        [MSDefaultValue(1)]
        public override byte crn_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro
        /// </summary>
        public override DateTime crn_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro
        /// </summary>
        public override DateTime crn_dataAlteracao { get; set; }
    }
}