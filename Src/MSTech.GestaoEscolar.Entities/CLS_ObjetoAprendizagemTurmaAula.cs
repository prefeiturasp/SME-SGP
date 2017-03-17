/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using Validation;
    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_ObjetoAprendizagemTurmaAula : Abstract_CLS_ObjetoAprendizagemTurmaAula
    {
        /// <summary>
        /// ID da tabela TUR_TurmaDisciplina.
        /// </summary>
        [MSNotNullOrEmpty("ID da tabela é TUR_TurmaDisciplina obrigatório.")]
        public override long tud_id { get; set; }

        /// <summary>
        /// ID da tabela CLS_TurmaAula.
        /// </summary>
        [MSNotNullOrEmpty("ID da tabela é CLS_TurmaAula obrigatório.")]
        public override int tau_id { get; set; }

        /// <summary>
        /// ID da tabela ACA_ObjetoAprendizagem.
        /// </summary>
        [MSNotNullOrEmpty("ID da tabela é ACA_ObjetoAprendizagem obrigatório.")]
        public override int oap_id { get; set; }
    }
}