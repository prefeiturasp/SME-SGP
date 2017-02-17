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
    public class ACA_FormatoAvaliacao : Abstract_ACA_FormatoAvaliacao
    {
        /// <summary>
        /// ID do Formato Avaliação
        /// </summary>
        [DataObjectField(true, true, false)]
        public override int fav_id { get; set; }

        /// <summary>
        /// ID da Entidade
        /// </summary>
        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }

        /// <summary>
        /// Indica se o formato de avaliação é padrão para a rede toda
        /// </summary>
        [MSDefaultValue(1)]
        public override bool fav_padrao { get; set; }

        /// <summary>
        /// Nome do Formato de Avaliação
        /// </summary>
        [MSValidRange(100, "Nome do formato de avaliação pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome do formato de avaliação é obrigatório.")]
        public override string fav_nome { get; set; }

        /// <summary>
        /// 1 - Conceito global, 2 - Por disciplina, 3 - Conceito global e por disciplina
        /// </summary>
        [MSNotNullOrEmpty("Tipo do formato de avalição é obrigatório.")]
        public override byte fav_tipo { get; set; }

        /// <summary>
        /// 1-Aulas planejadas, 2-Período, 3-Mensal, 4-Aulas planejadas e mensal
        /// </summary>
        [MSNotNullOrEmpty("Tipo de lançamento de frequência é obrigatório.")]
        public override byte fav_tipoLancamentoFrequencia { get; set; }

        /// <summary>
        /// Valor mínimo para aprovar de acordo com a Escala do ConceitoGlobal
        /// </summary>
        [MSValidRange(10, "Valor mínimo para aprovação do conceito global pode conter até 10 caracteres.")]
        public override string valorMinimoAprovacaoConceitoGlobal { get; set; }

        /// <summary>
        /// Valor mínimo para aprovar de acordo com a Escala por disciplina
        /// </summary>
        [MSValidRange(10, "Valor mínimo para aprovação por [MSG_DISCIPLINA] pode conter até 10 caracteres.")]
        public override string valorMinimoAprovacaoPorDisciplina { get; set; }

        /// <summary>
        /// Valor mínimo para progressão parcial de acordo com a Escala por disciplina
        /// </summary>
        [MSValidRange(10, "Valor mínimo de progressão parcial por [MSG_DISCIPLINA] pode conter até 10 caracteres.")]
        public override string valorMinimoProgressaoParcialPorDisciplina { get; set; }

        /// <summary>
        /// Indica se a alteração da frequência será bloquada ou não na efetivação
        /// </summary>
        [MSDefaultValue(1)]
        public override bool fav_bloqueiaFrequenciaEfetivacao { get; set; }

        /// <summary>
        /// Indica se a alteração da frequência será bloquada ou não na efetivação da disciplina
        /// </summary>
        [MSDefaultValue(1)]
        public override bool fav_bloqueiaFrequenciaEfetivacaoDisciplina { get; set; }

        /// <summary>
        /// Indica se o planejamento e lançamento será em conjunto (caso a turma seja de professor especialista).
        /// </summary>
        [MSDefaultValue(1)]
        public override bool fav_planejamentoAulasNotasConjunto { get; set; }

        /// <summary>
        /// Indica se a efetivação do conceito global é por docentes
        /// </summary>
        [MSDefaultValue(0)]
        public override bool fav_conceitoGlobalDocente { get; set; }

        /// <summary>
        /// indica a obrigatoriedade do relatorio para reprovação
        /// </summary>
        [MSDefaultValue(0)]
        public override bool fav_obrigatorioRelatorioReprovacao { get; set; }

        /// <summary>
        /// 1 - Ativo, 2 - Bloquedo, 3 - Excluído
        /// </summary>
        [MSDefaultValue(1)]
        public override byte fav_situacao { get; set; }

        /// <summary>
        /// Data da criação do registro
        /// </summary>
        public override DateTime fav_dataCriacao { get; set; }

        /// <summary>
        /// Data da alteração do registro
        /// </summary>
        public override DateTime fav_dataAlteracao { get; set; }

        /// <summary>
        /// 1-Tempos de aula, 2-Dia
        /// </summary>
        [MSNotNullOrEmpty("Tipo de apuração de frequência é obrigatório.")]
        public override byte fav_tipoApuracaoFrequencia { get; set; }

        /// <summary>
        /// 1-Automático, 2-Manual
        /// </summary>
        [MSNotNullOrEmpty("Cálculo de quantidade de aulas dadas é obrigatório")]
        public override byte fav_calculoQtdeAulasDadas { get; set; }

        /// <summary>
        /// Indica se a escola pode inserir uma nota/conceito final ao final do 4º bimestre
        /// </summary>
        [MSDefaultValue(0)]
        public override bool fav_avaliacaoFinalAnalitica { get; set; }

        /// <summary>
        /// Variação de valor da porcentagem de frequência
        /// </summary>
        [MSDefaultValue(0)]
        [MSNotNullOrEmpty("Variação é obrigatório")]
        public override decimal fav_variacao { get; set; }

        /// <summary>
        /// Indica se o parecer final virá preenchido automaticamente na tela do fechamento final
        /// </summary>
        [MSDefaultValue(0)]
        public override bool fav_sugerirResultadoFinalDisciplina { get; set; }

        /// <summary>
        /// Indica se o botão de soma/média irá aparecer
        /// </summary>
        [MSDefaultValue(0)]
        public override bool fav_exibirBotaoSomaMedia { get; set; }

        /// <summary>
        /// Valor mínimo para aprovar de acordo com a Escala por docente
        /// </summary>
        [MSValidRange(10, "Valor mínimo para aprovação por docente pode conter até 10 caracteres.")]
        public override string valorMinimoAprovacaoDocente { get; set; }

        /// <summary>
        /// Define se o formato utilizará o modelo de fechamento automático.
        /// </summary>
        [MSDefaultValue(false)]
        public override bool fav_fechamentoAutomatico { get; set; }

        /// <summary>
        /// Permite que o professor possa lançar nota de recuperação independente da nota que obtiver na avaliação.
        /// </summary>
        [MSDefaultValue(false)]
        public override bool fav_permiteRecuperacaoQualquerNota { get; set; }

        /// <summary>
        /// Permite que a data da recuperação possa ser fora do período do calendário.
        /// </summary>
        [MSDefaultValue(false)]
        public override bool fav_permiteRecuperacaoForaPeriodo { get; set; }
    }
}