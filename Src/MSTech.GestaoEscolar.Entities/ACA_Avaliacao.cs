/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ACA_Avaliacao : Abstract_ACA_Avaliacao
    {
        [MSValidRange(100, "Nome pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string ava_nome { get; set; }

        [MSNotNullOrEmpty("Tipo é obrigatório.")]
        public override short ava_tipo { get; set; }

        [MSNotNullOrEmpty("Aparece boletim é obrigatório.")]
        public override bool ava_apareceBoletim { get; set; }

        /// <summary>
        /// Indica se os alunos não avaliados serão exibidos.
        /// </summary>
        [MSNotNullOrEmpty("Exibe não avaliados é obrigatório.")]
        [MSDefaultValue(false)]
        public override bool ava_exibeNaoAvaliados { get; set; }

        /// <summary>
        /// Indica se a opção sem professor será exibida.
        /// </summary>
        [MSNotNullOrEmpty("Exibe sem professor é obrigatório.")]
        [MSDefaultValue(true)]
        public override bool ava_exibeSemProfessor { get; set; }

        /// <summary>
        /// Flag que indica se a observação do aluno para o bimestre será exibida na efetivação (por disciplina).
        /// </summary>
        [MSNotNullOrEmpty("Exibe observação no(a) [MSG_DISCIPLINA] é obrigatório.")]
        [MSDefaultValue(false)]
        public override bool ava_exibeObservacaoDisciplina { get; set; }

        /// <summary>
        /// Flag que indica se a observação do aluno para o bimestre será exibida na efetivação (por turma).
        /// </summary>
        [MSNotNullOrEmpty("Exibe observação para conselho pedagógico é obrigatório.")]
        [MSDefaultValue(false)]
        public override bool ava_exibeObservacaoConselhoPedagogico { get; set; }

        /// <summary>
        /// Exibe a porcentagem de frequência na efetivação de notas.
        /// </summary>
        [MSNotNullOrEmpty("Exibe a porcentagem de frequência é obrigatório.")]
        [MSDefaultValue(0)]
        public override bool ava_exibeFrequencia { get; set; }

        /// <summary>
        /// Indica se o usuário poderá infromar uma nota que substitua a nota original do aluno..
        /// </summary>
        [MSNotNullOrEmpty("Exibe nota pós-conselho é obrigatório.")]
        [MSDefaultValue(0)]
        public override bool ava_exibeNotaPosConselho { get; set; }

        [MSDefaultValue(0)]
        public override bool ava_baseadaConceitoGlobal { get; set; }

        public override bool ava_baseadaNotaDisciplina { get; set; }

        public override bool ava_baseadaAvaliacaoAdicional { get; set; }

        // <summary>
        /// Flag que indica se a frequência do conceito global é obrigatório..
        /// </summary>
        [MSDefaultValue(true)]
        public override bool ava_conceitoGlobalObrigatorioFrequencia { get; set; }

        [MSDefaultValue(1)]
        public override short ava_situacao { get; set; }

        public override DateTime ava_dataCriacao { get; set; }

        public override DateTime ava_dataAlteracao { get; set; }

        /// <summary>
        /// Campo ordem da tabela ACA_TipoPeriodoCalendario.
        /// </summary>
        public int tpc_ordem { get; set; }

        /// <summary>
		/// Indica se irá ocultar botão de atualização de notas e frequência.
		/// </summary>
        [MSDefaultValue(0)]
		public override bool ava_ocultarAtualizacao { get; set; }
    }
}