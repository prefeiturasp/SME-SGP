/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_AlunoJustificativaAbonoFalta : Abstract_ACA_AlunoJustificativaAbonoFalta
	{
        /// <summary>
        /// Id da tabela.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int ajf_id { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime ajf_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime ajf_dataAlteracao { get; set; }

        /// <summary>
        /// Situação.
        /// </summary>
        public enum Situacao : byte
        {
            Ativo = 1,
            Excluido = 3
        }

        /// <summary>
        /// Status.
        /// </summary>
        public enum Status : byte
        {
            AguardandoProcessamento = 1,
            EmProcessamento = 2,
            Processado = 3
        }
    }
}