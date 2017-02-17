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
    public abstract class Abstract_ACA_Avaliacao : Abstract_Entity
    {
		
		/// <summary>
		/// ID do Formato de Avaliação.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int fav_id { get; set; }

		/// <summary>
		/// ID da avaliação.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int ava_id { get; set; }

		/// <summary>
		/// Nome da avaliação.
		/// </summary>
		[MSValidRange(100)]
		[MSNotNullOrEmpty]
		public virtual string ava_nome { get; set; }

		/// <summary>
		/// 1 - Periódica, 2 - Recuperação, 3 - Final, 4 - Conselho de Classe, 5 - Periódica + Final, 6-Prova periódica secretaria , 7 - Recuperação final.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short ava_tipo { get; set; }

		/// <summary>
		/// ID do TipoPeriodoCalendario quando periódica.
		/// </summary>
		public virtual int tpc_id { get; set; }

		/// <summary>
		/// Ordem da avaliação periódica.
		/// </summary>
		public virtual int ava_ordemPeriodo { get; set; }

		/// <summary>
		/// indicador se aparece no boletim.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_apareceBoletim { get; set; }

		/// <summary>
		/// 1 - Ativo, 3 - Excluído.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short ava_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ava_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime ava_dataAlteracao { get; set; }

		/// <summary>
		/// Conceito Global Obrigatorio (0 - Não, 1 - Sim) .
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_conceitoGlobalObrigatorio { get; set; }

		/// <summary>
		/// Avaliação de conceito global...
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_baseadaConceitoGlobal { get; set; }

		/// <summary>
		/// Nota por diciplina na avaliação..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_baseadaNotaDisciplina { get; set; }

		/// <summary>
		/// Avaliação adicional..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_baseadaAvaliacaoAdicional { get; set; }

		/// <summary>
		/// Indica se a nota de conceito global será ou não mostrada no boletim.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_mostraBoletimConceitoGlobalNota { get; set; }

		/// <summary>
		/// Indica se a frequência de conceito global será ou não mostrada no boletim.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_mostraBoletimConceitoGlobalFrequencia { get; set; }

		/// <summary>
		/// Indica se a nota da avaliação adicional de conceito global será ou não mostrada no boletim.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_mostraBoletimConceitoGlobalAvaliacaoAdicional { get; set; }

		/// <summary>
		/// Indica se a nota das disciplinas será ou não mostrada no boletim.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_mostraBoletimDisciplinaNota { get; set; }

		/// <summary>
		/// Indica se a frequência das disciplinas será ou não mostrada no boletim.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_mostraBoletimDisciplinaFrequencia { get; set; }

		/// <summary>
		/// Conceito/nota máxima da recuperação final: (1-Qualquer um; 2-Conceito/nota mínimo para aprovação).
		/// </summary>
		public virtual short ava_recFinalConceitoMaximoAprovacao { get; set; }

		/// <summary>
		/// Indicará que os alunos que não atingirem conceito mínimo deverão fazer recuperação..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_recFinalConceitoGlobalMinimoNaoAtingido { get; set; }

		/// <summary>
		/// Indicará que os alunos que não atingirem a frequência mínima acumulada até a avaliação final deverão fazer a recuperação, independente do conceito..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_recFinalFrequenciaMinimaFinalNaoAtingida { get; set; }

		/// <summary>
		/// Indicará que apenas alunos já de recuperação devido a regra do conceito global farão recuperação apenas das disciplinas que possuem média baixa..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido { get; set; }

		/// <summary>
		/// Indica se disciplina é obrigatória..
		/// </summary>
		public virtual bool ava_disciplinaObrigatoria { get; set; }

		/// <summary>
		/// Exibir alunos não avaliados na avaliação..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_exibeNaoAvaliados { get; set; }

		/// <summary>
		/// Exibir opção sem professor na avaliação..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_exibeSemProfessor { get; set; }

		/// <summary>
		/// Flag que indica se a observação do aluno para o bimestre será exibida na efetivação (por disciplina)..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_exibeObservacaoDisciplina { get; set; }

		/// <summary>
		/// Flag que indica se a observação do aluno para o bimestre será exibida na efetivação (por turma)..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_exibeObservacaoConselhoPedagogico { get; set; }

		/// <summary>
		/// Exibe a porcentagem de frequência na efetivação de notas..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_exibeFrequencia { get; set; }

		/// <summary>
		/// Indica se o usuário poderá infromar uma nota que substitua a nota original do aluno..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_exibeNotaPosConselho { get; set; }

		/// <summary>
		/// Flag que indica se a frequência do conceito global é obrigatório..
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual bool ava_conceitoGlobalObrigatorioFrequencia { get; set; }

		/// <summary>
		/// Peso aplicado em porcentagem na avaliação para compor a média final (utilizado apenas para avaliações do tipo periódicas)..
		/// </summary>
		public virtual decimal ava_peso { get; set; }

		/// <summary>
		/// Indica se irá ocultar botão de atualização de notas e frequência.
		/// </summary>
		public virtual bool ava_ocultarAtualizacao { get; set; }

    }
}