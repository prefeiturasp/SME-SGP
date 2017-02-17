/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using System.ComponentModel;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class TUR_TurmaDocenteProjeto : AbstractTUR_TurmaDocenteProjeto
	{
        /// <summary>
        /// ID da turma.
        /// </summary>
        [MSNotNullOrEmpty("ID da turma é obrigatório")]
        [DataObjectField(true, false, false)]
        public override long tur_id { get; set; }

        /// <summary>
        /// ID do turma docente projeto.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override long tdp_id { get; set; }

        /// <summary>
        /// ID do docente.
        /// </summary>
        [MSNotNullOrEmpty("ID do docente é obrigatório")]
        public override long doc_id { get; set; }

        /// <summary>
        /// ID do colaborador relacionado ao docente.
        /// </summary>
        [MSNotNullOrEmpty("ID do colaborador relacionado ao docente é obrigatório")]
        public override long col_id { get; set; }

        /// <summary>
        /// ID do cargo do docente relacionado à disciplina.
        /// </summary>
        [MSNotNullOrEmpty("ID do cargo do docente relacionado à disciplina é obrigatório")]
        public override int crg_id { get; set; }

        /// <summary>
        /// ID do relacionamento do cargo do colaborador.
        /// </summary>
        [MSNotNullOrEmpty("ID do relacionamento do cargo do colaborador é obrigatório")]
        public override int coc_id { get; set; }

        /// <summary>
        /// Posição do docente.
        /// </summary>
        [MSNotNullOrEmpty("Posição do docente é obrigatório")]
        public override short tdp_posicao { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3 - Excluído, 4 - Inativo).
        /// </summary>
        [MSDefaultValue(1)]
        public override short tdp_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime tdp_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime tdp_dataAlteracao { get; set; }

        /// <summary>
        /// Variável auxiliar que armazena o ID do tipo de docente.
        /// </summary>
        public byte tdc_id { get; set; }
	}
}