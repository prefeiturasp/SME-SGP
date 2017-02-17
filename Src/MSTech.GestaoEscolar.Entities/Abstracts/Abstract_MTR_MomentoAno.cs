/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
    /// <summary>
    ///
    /// </summary>
    [Serializable()]
    public abstract class Abstract_MTR_MomentoAno : Abstract_Entity
    {
        /// <summary>
        /// Ano letivo dos momentos de movimentação
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int mom_ano { get; set; }

        /// <summary>
        /// Id dos momentos de movimentação
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int mom_id { get; set; }

        /// <summary>
        /// ID da entidade
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual Guid ent_id { get; set; }

        /// <summary>
        /// Data de congelamento das movimentações
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime mom_dataCongelamento { get; set; }

        /// <summary>
        /// Flag que indica se o congelamento poderá ser lançado por escola
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual bool mom_congelamentoEscola { get; set; }

        /// <summary>
        /// Data de congelamento das movimentações para o censo
        /// </summary>
        public virtual DateTime mom_dataCongelamentoCenso { get; set; }

        /// <summary>
        /// Prazo em dias para realização da movimentação
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual int mom_prazoMovimentacao { get; set; }

        /// <summary>
        /// Prazo em dias para aprovação de uma ação retroativa por usuários com visão gestão
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual int mom_prazoAprovacaoRetroativa { get; set; }

        /// <summary>
        /// Situação do registro: 1-Ativo, 3-Excluído
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte mom_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime mom_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime mom_dataAlteracao { get; set; }

        /// <summary>
        /// Propriedade mom_dataCalculoIdade.
        /// </summary>
        [MSNotNullOrEmpty]
        public virtual DateTime mom_dataCalculoIdade { get; set; }

        /// <summary>
        /// Se haverá o antigo esquema de controle semestral das disciplinas onde o(a) curso permite o
        /// fechamento/efetivação semestral, feito devido aos registros do histórico.
        /// </summary>
        [MSNotNullOrEmpty()]
        [MSDefaultValue(0)]
        public virtual bool mom_controleSemestral { get; set; }
    }
}