/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;

    [Serializable]
    public class ACA_SondagemAgendamentoPeriodo : Abstract_ACA_SondagemAgendamentoPeriodo
    {
        /// <summary>
        /// ID da tabela ACA_Sondagem.
        /// </summary>
        [MSNotNullOrEmpty("ID da tabela ACA_Sondagem é obrigatório.")]
        public override int snd_id { get; set; }

        /// <summary>
        /// ID da tabela ACA_SondagemAgendamento.
        /// </summary>
        [MSNotNullOrEmpty("ID da tabela ACA_SondagemAgendamento é obrigatório.")]
        public override int sda_id { get; set; }

        /// <summary>
        /// ID da tabela ACA_TipoCurriculoPeriodo.
        /// </summary>
        [MSNotNullOrEmpty("ID da tabela ACA_TipoCurriculoPeriodo é obrigatório.")]
        public override int tcp_id { get; set; }

        /// <summary>
        /// Variável auxiliar do nome do período do curso
        /// </summary>
        public string tcp_descricao { get; set; }

        /// <summary>
        /// Variável auxiliar da ordem do período do curso
        /// </summary>
        public int tcp_ordem { get; set; }

        /// <summary>
        /// Variável auxiliar da ordem do nível de ensino
        /// </summary>
        public int tne_ordem { get; set; }
    }
}