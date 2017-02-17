/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class ACA_AvisoTextoGeral : AbstractACA_AvisoTextoGeral
	{

        
        /// <summary>
        /// Propriedade atg_id.
        /// </summary>
        [DataObjectField(true, true, false)]
        public override long atg_id { get; set; }

        /// <summary>
        /// Titulo do aviso texto geral
        /// </summary>
        [MSValidRange(200, "Título do Aviso/Texto geral pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("Título do Aviso/Texto geral é obrigatório.")]
        public override string atg_titulo { get; set; }

        /// <summary>
        /// Descrição do aviso texto geral
        /// </summary>
        [MSNotNullOrEmpty("Descrição do Aviso/Texto geral é obrigatório.")]
        public override string atg_descricao { get; set; }

        /// <summary>
        /// Propriedade atg_timbreCabecalho.
        /// </summary>
        [MSNotNullOrEmpty("Timbre no cabeçalho é obrigatório.")]
        public override bool atg_timbreCabecalho { get; set; }

        /// <summary>
        /// Propriedade atg_anotacaoAula.
        /// </summary>
        [MSNotNullOrEmpty("Anotação de aula é obrigatório.")]
        public override bool atg_anotacaoAula { get; set; }

        /// <summary>
        /// Propriedade atg_tipo.
        /// </summary>
        public override byte atg_tipo { get; set; }

        /// <summary>
        /// Propriedade atg_situacao.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte atg_situacao { get; set; }

        /// <summary>
        /// Propriedade atg_dataCriacao.
        /// </summary>
        public override DateTime atg_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade atg_dataAlteracao.
        /// </summary>
        public override DateTime atg_dataAlteracao { get; set; }
	}
}