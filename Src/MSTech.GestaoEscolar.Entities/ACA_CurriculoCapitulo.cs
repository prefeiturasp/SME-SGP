/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;
    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_CurriculoCapitulo : Abstract_ACA_CurriculoCapitulo
	{
        /// <summary>
        /// Título do capítulo.
        /// </summary>
        [MSNotNullOrEmpty("Título do capítulo é obrigatório.")]
        [MSValidRange(200, "Título do capítulo pode conter até 200 caracteres.")]
        public override string crc_titulo { get; set; }

        /// <summary>
        /// Descrição do capítulo.
        /// </summary>
        [MSValidRange(4000, "Descrição do capítulo pode conter até 4000 caracteres.")]
        public override string crc_descricao { get; set; }

        /// <summary>
        /// Ordem do registro.
        /// </summary>
        [MSNotNullOrEmpty("Ordem do registro é obrigatória.")]
        public override int crc_ordem { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte crc_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime crc_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime crc_dataAlteracao { get; set; }
    }
}