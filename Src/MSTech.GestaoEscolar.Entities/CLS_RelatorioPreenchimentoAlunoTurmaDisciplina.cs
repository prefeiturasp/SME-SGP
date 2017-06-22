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
    public class CLS_RelatorioPreenchimentoAlunoTurmaDisciplina : Abstract_CLS_RelatorioPreenchimentoAlunoTurmaDisciplina
	{
        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte ptd_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime ptd_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime ptd_dataAlteracao { get; set; }
    }
}