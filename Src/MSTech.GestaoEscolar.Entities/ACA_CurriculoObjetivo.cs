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
    public class ACA_CurriculoObjetivo : Abstract_ACA_CurriculoObjetivo
	{
        /// <summary>
        /// Descrição do objetivo.
        /// </summary>
        [MSValidRange(500, "Descrição do capítulo pode conter até 500 caracteres.")]
        public override string cro_descricao { get; set; }

        /// <summary>
        /// Ordem do registro.
        /// </summary>
        [MSNotNullOrEmpty("Ordem do registro é obrigatória.")]
        public override int cro_ordem { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte cro_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime cro_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime cro_dataAlteracao { get; set; }
    }
}