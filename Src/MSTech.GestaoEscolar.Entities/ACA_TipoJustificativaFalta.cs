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
    [Serializable()]
	public class ACA_TipoJustificativaFalta : Abstract_ACA_TipoJustificativaFalta
	{
        /// <summary>
        /// ID do tipo de justificativa de falta
        /// </summary>        
        [DataObjectField(true, true, false)]
        public override int tjf_id { get; set; }

        /// <summary>
        /// Nome do tipo de justificativa de falta
        /// </summary>
        [MSValidRange(100, "Tipo de justificativa de falta pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de justificativa de falta é obrigatório.")]
        public override string tjf_nome { get; set; }

        /// <summary>
        /// Código do tipo de justificativa de falta
        /// </summary>
        [MSValidRange(20, "Código pode conter até 20 caracteres.")]
        public override string tjf_codigo { get; set; }

        /// <summary>
        /// Situação do registro: 1-Ativo, 3-Excluído, 4-Inativo
        /// </summary>
        [MSDefaultValue(1)]
        public override byte tjf_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro
        /// </summary>        
        public override DateTime tjf_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro
        /// </summary>        
        public override DateTime tjf_dataAlteracao { get; set; }                
    }
}
