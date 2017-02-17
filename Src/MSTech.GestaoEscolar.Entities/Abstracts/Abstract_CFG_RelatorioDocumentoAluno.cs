/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: Tabela responsável pela exibição dos documentos acadêmicos dos alunos disponíveis para impressão de acordo com a entidade do sistema acadêmico..
	/// </summary>
	[Serializable]
    public abstract class Abstract_CFG_RelatorioDocumentoAluno : Abstract_Entity
    {
		
		/// <summary>
		/// id da entidade na tabela SYS_SistemaEntidade do banco de dados CoreSSO..
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// id do relatório na tabela CFG_Relatorio do banco de dados CoreSSO.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int rlt_id { get; set; }

		/// <summary>
		/// nome de exibição do relatório no sistema de gestão acadêmica..
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty]
		public virtual string rda_nomeDocumento { get; set; }

		/// <summary>
		/// define a ordem que deverão ser exibidos a listagem de documentos do aluno..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int rda_ordem { get; set; }

		/// <summary>
		/// 1-ativo; 2- Bloqueado; 3-Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short rda_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rda_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime rda_dataAlteracao { get; set; }

		/// <summary>
		/// Propriedade rda_id.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int rda_id { get; set; }

        /// <summary>
        /// Propriedade atg_id.
        /// </summary>
        [DataObjectField(true, true, false)]
        public virtual long atg_id { get; set; }

    }
}