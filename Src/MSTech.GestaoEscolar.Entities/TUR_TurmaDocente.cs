/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
	public class TUR_TurmaDocente : Abstract_TUR_TurmaDocente
	{
        [DataObjectField(true, false, false)]
        public override Int64 tud_id { get; set; }
        [DataObjectField(true, false, false)]
        public override int tdt_id { get; set; }

        /// <summary>
        /// ID do colaborador relacionado ao docente.
        /// </summary>
        [MSNotNullOrEmpty("Docente é obrigatório.")]
        public override Int64 col_id { get; set; }

        /// <summary>
        /// ID do cargo do docente relacionado à disciplina.
        /// </summary>
        [MSNotNullOrEmpty("Cargo do docente é obrigatório.")]
        public override int crg_id { get; set; }

        /// <summary>
        /// ID do relacionamento do cargo do colaborador.
        /// </summary>
        [MSNotNullOrEmpty("Cargo do docente é obrigatório.")]
        public override int coc_id { get; set; }


        [MSNotNullOrEmpty("Tipo é obrigatório.")]
        public override byte tdt_tipo { get; set; }   

        /// <summary>
        /// Inicio de vigência do docente
        /// </summary>
        [MSNotNullOrEmpty("Inicio de vigência é obrigatório.")]
        public override DateTime tdt_vigenciaInicio { get; set; }

        [MSNotNullOrEmpty("Posição do docente é obrigatório.")]
        public override byte tdt_posicao { get; set; }

        [MSDefaultValue(1)]
        public override byte tdt_situacao { get; set; }
        public override DateTime tdt_dataCriacao { get; set; }
        public override DateTime tdt_dataAlteracao { get; set; }
	}

    /// <summary>
    /// Estrutura usada para guardar os docentes das disciplinas
    /// Utilizada na tela de cadastro de turmas
    /// </summary>
    [Serializable]
    public struct TUR_Turma_Docentes_Disciplina
    {
        public int indice;
        public string doc_nome;
        public TUR_TurmaDocente entDocente;
    }
}