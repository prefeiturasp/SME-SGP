/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MTR_MomentoAno : Abstract_MTR_MomentoAno
    {
        /// <summary>
        /// Id dos momentos de movimentação
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int mom_id { get; set; }

        /// <summary>
        /// Prazo em dias para realização da movimentação
        /// </summary>
        [MSNotNullOrEmpty("Quantidade de dias corridos que a escola pode realizar uma movimentação sem ter uma ação retroativa é obrigatório e deve ser um número inteiro maior que 0 (zero).")]
        public override int mom_prazoMovimentacao { get; set; }

        /// <summary>
        /// Prazo em dias para aprovação de uma ação retroativa por usuários com visão gestão
        /// </summary>
        [MSNotNullOrEmpty("Limite de aprovação da ação retroativa pela visão Gestão (em dias corridos) é obrigatório e deve ser um número inteiro maior que 0 (zero).")]
        public override int mom_prazoAprovacaoRetroativa { get; set; }

        /// <summary>
        /// Situação do registro: 1-Ativo, 3-Excluído
        /// </summary>
        [MSDefaultValue(1)]
        public override byte mom_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro
        /// </summary>
        public override DateTime mom_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro
        /// </summary>
        public override DateTime mom_dataAlteracao { get; set; }
    }
}