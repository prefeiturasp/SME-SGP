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
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class Abstract_CLS_TurmaAula : Abstract_Entity
    {
		
		/// <summary>
		/// ID TurmaDisciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual long tud_id { get; set; }

		/// <summary>
		/// ID TurmaAula.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int tau_id { get; set; }

		/// <summary>
		/// ID TipoPeriodoCalendario.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Sequencia de aulas.
		/// </summary>
		public virtual int tau_sequencia { get; set; }

		/// <summary>
		/// Data da aula.
		/// </summary>
		public virtual DateTime tau_data { get; set; }

		/// <summary>
		/// Número de aulas que engloba.
		/// </summary>
		public virtual int tau_numeroAulas { get; set; }

		/// <summary>
		/// Plano da aula.
		/// </summary>
		public virtual string tau_planoAula { get; set; }

		/// <summary>
		/// Diário da aula.
		/// </summary>
		public virtual string tau_diarioClasse { get; set; }

		/// <summary>
		/// Conteúdo da aula.
		/// </summary>
		public virtual string tau_conteudo { get; set; }

		/// <summary>
		/// Indica se o docente finalizou o lançamento da aula.
		/// </summary>
		public virtual bool tau_efetivado { get; set; }

		/// <summary>
		/// Indica se existe atividade para casa na aula.
		/// </summary>
		public virtual string tau_atividadeCasa { get; set; }

		/// <summary>
		/// 1–Aula prevista, 3–Excluído, 4–Aula dada, 6–Aula cancelada.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tau_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tau_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime tau_dataAlteracao { get; set; }

		/// <summary>
		/// Posicao do docente.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tdt_posicao { get; set; }

		/// <summary>
		/// Indica o id do protocolo que gerou essa aula (vindo do diário de classe).
		/// </summary>
		public virtual Guid pro_id { get; set; }

		/// <summary>
		/// Sintese da aula .
		/// </summary>
		public virtual string tau_sintese { get; set; }

		/// <summary>
		/// Indica se a aula e de reposicao.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool tau_reposicao { get; set; }

		/// <summary>
		/// ID do usuário que criou a aula..
		/// </summary>
		public virtual Guid usu_id { get; set; }

		/// <summary>
		/// Id do usuário que realizou a última alteração nos dados.
		/// </summary>
		public virtual Guid usu_idDocenteAlteracao { get; set; }

		/// <summary>
		/// Propriedade tau_statusFrequencia.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual byte tau_statusFrequencia { get; set; }

		/// <summary>
		/// Propriedade tau_statusAtividadeAvaliativa.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte tau_statusAtividadeAvaliativa { get; set; }

		/// <summary>
		/// Propriedade tau_statusAnotacoes.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte tau_statusAnotacoes { get; set; }

		/// <summary>
		/// Propriedade tau_statusPlanoAula.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte tau_statusPlanoAula { get; set; }

		/// <summary>
		/// Propriedade tau_checadoAtividadeCasa.
		/// </summary>
		public virtual bool tau_checadoAtividadeCasa { get; set; }

		/// <summary>
		/// Propriedade tau_dataUltimaSincronizacao.
		/// </summary>
		public virtual DateTime tau_dataUltimaSincronizacao { get; set; }

    }
}