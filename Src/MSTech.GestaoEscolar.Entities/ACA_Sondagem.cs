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
    public class ACA_Sondagem : Abstract_ACA_Sondagem
	{
        /// <summary>
        /// Título da sondagem.
        /// </summary>
        [MSNotNullOrEmpty("Título da sondagem é obrigatório.")]
        [MSValidRange(200, "Título da sondagem pode conter até 200 caracteres.")]
        public override string snd_titulo { get; set; }

        /// <summary>
        /// Descrição da sondagem.
        /// </summary>
        [MSValidRange(4000, "Descrição da sondagem pode conter até 4000 caracteres.")]
        public override string snd_descricao { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 2-Bloqueado, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override byte snd_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime snd_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime snd_dataAlteracao { get; set; }
    }
}