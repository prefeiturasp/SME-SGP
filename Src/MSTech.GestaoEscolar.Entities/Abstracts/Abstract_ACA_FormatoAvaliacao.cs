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
    public abstract class Abstract_ACA_FormatoAvaliacao : Abstract_Entity
    {
		
		/// <summary>
		/// ID do Formato Avaliação.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, true, false)]
		public virtual int fav_id { get; set; }

		/// <summary>
		/// ID da Entidade.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// ID da Escola que usará o formato.
		/// </summary>
		public virtual int esc_id { get; set; }

		/// <summary>
		/// ID da UnidadeEscolar que usará o formato.
		/// </summary>
		public virtual int uni_id { get; set; }

		/// <summary>
		/// Indica se o formato de avaliação é padrão para a rede toda.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_padrao { get; set; }

		/// <summary>
		/// Nome do Formato de Avaliação.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string fav_nome { get; set; }

		/// <summary>
		/// 1 - Conceito global, 2 - Por disciplina, 3 - Conceito global e por disciplina.
		/// </summary>
        public virtual byte fav_tipo { get; set; }

		/// <summary>
		/// 1-Aulas planejadas, 2-Período, 3-Mensal, 4-Aulas planejadas e mensal, 5 - Aulas dadas, 6 - Aulas previstas do docente.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte fav_tipoLancamentoFrequencia { get; set; }

		/// <summary>
		/// 1-Tempos de aula, 2-Dia.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte fav_tipoApuracaoFrequencia { get; set; }

		/// <summary>
		/// 1-Automático, 2-Manual.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte fav_calculoQtdeAulasDadas { get; set; }

		/// <summary>
		/// ID da EscalaAvaliacao usada no conceito global.
		/// </summary>
		public virtual int esa_idConceitoGlobal { get; set; }

		/// <summary>
		/// ID da EscalaAvaliacao usada por disciplina.
		/// </summary>
		public virtual int esa_idPorDisciplina { get; set; }

		/// <summary>
		/// ID da EscalaAvaliacao usada pelo docente.
		/// </summary>
		public virtual int esa_idDocente { get; set; }

		/// <summary>
		/// Campo Id da tabela ACA_EscalaAvaliacao adicional..
		/// </summary>
		public virtual int esa_idConceitoGlobalAdicional { get; set; }

		/// <summary>
		/// Utilizar avaliação adicional..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_conceitoGlobalAdicional { get; set; }

		/// <summary>
		/// Indica se a efetivação do conceito global é por docentes.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_conceitoGlobalDocente { get; set; }

		/// <summary>
		/// Obrigatório relatório para reprovação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_obrigatorioRelatorioReprovacao { get; set; }

		/// <summary>
		/// Indica se o planejamento e lançamento será em conjunto (caso a turma seja de professor especialista)..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_planejamentoAulasNotasConjunto { get; set; }

		/// <summary>
		/// Indica se a alteração da frequência será bloquada ou não na efetivação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_bloqueiaFrequenciaEfetivacao { get; set; }

		/// <summary>
		/// Indica se a alteração da frequência será bloquada ou não na efetivação da disciplina.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_bloqueiaFrequenciaEfetivacaoDisciplina { get; set; }

		/// <summary>
		/// Valor mínimo para aprovar de acordo com a Escala do ConceitoGlobal.
		/// </summary>
		[MSValidRange(10)]
		public virtual string valorMinimoAprovacaoConceitoGlobal { get; set; }

		/// <summary>
		/// Valor mínimo para aprovar de acordo com a Escala por disciplina.
		/// </summary>
		[MSValidRange(10)]
		public virtual string valorMinimoAprovacaoPorDisciplina { get; set; }

		/// <summary>
		/// Percentual mínimo de frequencia para aprovação.
		/// </summary>
		public virtual decimal percentualMinimoFrequencia { get; set; }

		/// <summary>
		/// 1 - Período de matrícula seguinte, 2 - Anterior ao período de matrícula seguinte, 3 - Sem progressão parcial.
		/// </summary>
        public virtual byte tipoProgressaoParcial { get; set; }

		/// <summary>
		/// Valor mínimo para progressão parcial de acordo com a Escala por disciplina.
		/// </summary>
		[MSValidRange(10)]
		public virtual string valorMinimoProgressaoParcialPorDisciplina { get; set; }

		/// <summary>
		/// Qtde de disciplinas máx que o aluno poderá cursar na progressão parcial.
		/// </summary>
		public virtual short qtdeMaxDisciplinasProgressaoParcial { get; set; }

		/// <summary>
		/// 1 - Ativo, 2 - Bloquedo, 3 - Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
        public virtual byte fav_situacao { get; set; }

		/// <summary>
		/// Data da criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime fav_dataCriacao { get; set; }

		/// <summary>
		/// Data da alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime fav_dataAlteracao { get; set; }

		/// <summary>
		/// Critérios de Avaliação (1-Conceito Global e frequência, 2-Conceito Global, 3-Nota por disciplina, 4-Apenas frequência, 5-Todos aprovados, 6-Frequência final ajustada da disciplina).
		/// </summary>
        public virtual short fav_criterioAprovacaoResultadoFinal { get; set; }

		/// <summary>
		/// Indica se a escola pode inserir uma nota/conceito final ao final do 4º bimestre.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_avaliacaoFinalAnalitica { get; set; }

		/// <summary>
		/// Variação de valor da porcentagem de frequência..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual decimal fav_variacao { get; set; }

		/// <summary>
		/// Indica se o parecer final virá preenchido automaticamente na tela do fechamento final..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_sugerirResultadoFinalDisciplina { get; set; }

		/// <summary>
		/// Percentual mínimo de frequência para seleção automática do parecer final..
		/// </summary>
		public virtual decimal fav_percentualMinimoFrequenciaFinalAjustadaDisciplina { get; set; }

		/// <summary>
		/// Indica se o botão de soma/média irá aparecer.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_exibirBotaoSomaMedia { get; set; }

		/// <summary>
		/// Propriedade valorMinimoAprovacaoDocente.
		/// </summary>
		[MSValidRange(10)]
		public virtual string valorMinimoAprovacaoDocente { get; set; }

		/// <summary>
		/// Percentual mínimo de frequencia para baixa frequencia.
		/// </summary>
		public virtual decimal percentualBaixaFrequencia { get; set; }

		/// <summary>
		/// Define se o formato utilizará o modelo de fechamento automático..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_fechamentoAutomatico { get; set; }

		/// <summary>
		/// Permite que o professor possa lançar nota de recuperação independente da nota que obtiver na avaliação.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_permiteRecuperacaoQualquerNota { get; set; }

		/// <summary>
		/// Permite que a data da recuperação possa ser fora do período do calendário.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_permiteRecuperacaoForaPeriodo { get; set; }

		/// <summary>
		/// Indica se será calculada uma média na avaliação do tipo Final, baseada nas avaliações periódicas..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool fav_calcularMediaAvaliacaoFinal { get; set; }

    }
}