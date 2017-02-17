/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// Tabela responsável pela exibição dos documentos acadêmicos dos alunos disponíveis para impressão de acordo com a entidade do sistema acadêmico.
	/// </summary>
	[Serializable]
	public class CFG_RelatorioDocumentoAluno : Abstract_CFG_RelatorioDocumentoAluno
	{
        public override int rda_id { get; set; }

        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }

        [MSNotNullOrEmpty("Relatório é obrigatório.")]
        public override int rlt_id { get; set; }

        [MSValidRange(200)]
        [MSNotNullOrEmpty("Nome do documento é obrigatório.")]
        public override string rda_nomeDocumento { get; set; }

        [MSNotNullOrEmpty("Ordem é obrigatório.")]
        public override int rda_ordem { get; set; }

        [MSDefaultValue(1)]
        public override short rda_situacao { get; set; }

        public override long atg_id { get; set; }

        public override DateTime rda_dataCriacao { get; set; }
        public override DateTime rda_dataAlteracao { get; set; }
	}
}