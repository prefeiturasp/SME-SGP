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
    public class ACA_CurriculoSugestao : Abstract_ACA_CurriculoSugestao
	{
        /// <summary>
        /// Texto da sugestão.
        /// </summary>
        [MSValidRange(400, "Sugestão pode conter até 400 caracteres.")]
        public override string crs_sugestao { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte crs_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime crs_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime crs_dataAlteracao { get; set; }
    }
}