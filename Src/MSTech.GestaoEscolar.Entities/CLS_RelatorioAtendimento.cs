/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.ComponentModel;
    using Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
	public class CLS_RelatorioAtendimento : Abstract_CLS_RelatorioAtendimento
	{

        /// <summary>
        /// ID do relatório de atendimento.
        /// </summary>
        [DataObjectField(true, true, false)]
        public override int rea_id { get; set; }

        /// <summary>
        /// Título do relatório de atendimento.
        /// </summary>
        [MSValidRange(200, "Título pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("Título é obrigatório.")]
        public override string rea_titulo { get; set; }

        /// <summary>
        /// Tipo do relatório de atendimento (1 - AEE, 2 - NAAPA, 3 - Recuperação Paralela).
        /// </summary>
        [MSNotNullOrEmpty("Tipo é obrigatório.")]
        public override byte rea_tipo { get; set; }

        /// <summary>
        /// Permite editar campo de Raça/Cor.
        /// </summary>
        [MSNotNullOrEmpty("Permite editar campo de Raça/Cor é obrigatório.")]
        public override bool rea_permiteEditarRecaCor { get; set; }

        /// <summary>
        /// Permite editar campo de Hipótese Diagnóstica.
        /// </summary>
        [MSNotNullOrEmpty("Permite editar campo de Hipótese Diagnóstica é obrigatório.")]
        public override bool rea_permiteEditarHipoteseDiagnostica { get; set; }

        /// <summary>
        /// Permite editar campo de Acoes Realizadas.
        /// </summary>
        [MSNotNullOrEmpty("Permite Ações realizadas é obrigatório.")]
        public override bool rea_permiteAcoesRealizadas { get; set; }

        /// <summary>
        /// ID do tipo de disciplina do relatório de atendimento.
        /// </summary>
        public override int tds_id { get; set; }

        /// <summary>
        /// Periodicidade de preenchimento do relatório de atendimento (1 - Periódico, 2 - Encerramento).
        /// </summary>
        public override byte rea_periodicidadePreenchimento { get; set; }
        
        /// <summary>
        /// Título do arquivo anexado no relatório de atendimento.
        /// </summary>
        [MSValidRange(256, "Título do anexo pode conter até 256 caracteres.")]
        public override string rea_tituloAnexo { get; set; }

        /// <summary>
        /// Situação do registro (1 - Ativo, 3 - Excluido).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte rea_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime rea_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime rea_dataAlteracao { get; set; }
    }
}