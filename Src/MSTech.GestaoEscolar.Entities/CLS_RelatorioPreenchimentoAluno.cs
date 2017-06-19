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
    public class CLS_RelatorioPreenchimentoAluno : Abstract_CLS_RelatorioPreenchimentoAluno
	{
        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte pal_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime pal_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime pal_dataAlteracao { get; set; }
    }
}