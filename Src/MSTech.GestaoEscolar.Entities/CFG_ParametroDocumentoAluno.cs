/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
    /// Tabela responsável por guardar os parametros para exibição de um relatório da entidade do sistema de gestão acadêmica.
	/// </summary>
	[Serializable]
	public class CFG_ParametroDocumentoAluno : Abstract_CFG_ParametroDocumentoAluno
	{
        /// <summary>
        /// id da entidade da tabela CFG_RelatorioDocumentoAluno.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override Guid ent_id { get; set; }

        /// <summary>
        /// id do relatório da tabela CFG_RelatorioDocumentoAluno.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int rlt_id { get; set; }

        /// <summary>
        /// id do parâmentro.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int pda_id { get; set; }

        /// <summary>
        /// nome-chave do parâmetro.
        /// </summary>
        [MSValidRange(100, "Chave pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Chave é obrigatório.")]
        public override string pda_chave { get; set; }

        /// <summary>
        /// valor do parâmetro.
        /// </summary>
        [MSValidRange(1000, "Valor pode conter até 1000 caracteres.")]
        [MSNotNullOrEmpty("Valor é obrigatório.")]
        public override string pda_valor { get; set; }

        /// <summary>
        /// descrição do parâmetro.
        /// </summary>
        [MSValidRange(200, "Descrição pode conter até 200 caracteres.")]
        public override string pda_descricao { get; set; }

        /// <summary>
        /// situação do parametrô: 1-Ativo; 3-Excluído;
        /// </summary>
        [MSDefaultValue(1)]
        public override byte pda_situacao { get; set; }

        /// <summary>
        /// data da criação do registro.
        /// </summary>
        public override DateTime pda_dataCriacao { get; set; }

        /// <summary>
        /// data da alteração do registro.
        /// </summary>
        public override DateTime pda_dataAlteracao { get; set; }

        /// <summary>
        /// Descrição do relatório de acordo com o campo rlt_id.
        /// </summary>
        public string NomeRelatorio { get; set; }
	}
}