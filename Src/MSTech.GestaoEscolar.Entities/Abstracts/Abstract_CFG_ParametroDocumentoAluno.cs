/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
    /// <summary>
    /// Tabela responsável por guardar os parametros para exibição de um relatório da entidade do sistema de gestão acadêmica.
    /// </summary>
    [Serializable()]
    public abstract class Abstract_CFG_ParametroDocumentoAluno : Abstract_Entity
    {

        /// <summary>
        /// id da entidade da tabela CFG_RelatorioDocumentoAluno.
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual Guid ent_id { get; set; }

        /// <summary>
        /// id do relatório da tabela CFG_RelatorioDocumentoAluno.
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int rlt_id { get; set; }

        /// <summary>
        /// id do parâmentro.
        /// </summary>
        [MSNotNullOrEmpty()]
        [DataObjectField(true, false, false)]
        public virtual int pda_id { get; set; }

        /// <summary>
        /// nome-chave do parâmetro.
        /// </summary>
        [MSValidRange(100)]
        [MSNotNullOrEmpty()]
        public virtual string pda_chave { get; set; }

        /// <summary>
        /// valor do parâmetro.
        /// </summary>
        [MSValidRange(1000)]
        [MSNotNullOrEmpty()]
        public virtual string pda_valor { get; set; }

        /// <summary>
        /// descrição do parâmetro.
        /// </summary>
        [MSValidRange(200)]
        public virtual string pda_descricao { get; set; }

        /// <summary>
        /// situação do parametrô: 1-Ativo; 3-Excluído;
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte pda_situacao { get; set; }

        /// <summary>
        /// data da criação do registro.
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime pda_dataCriacao { get; set; }

        /// <summary>
        /// data da alteração do registro.
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual DateTime pda_dataAlteracao { get; set; }

    }
}