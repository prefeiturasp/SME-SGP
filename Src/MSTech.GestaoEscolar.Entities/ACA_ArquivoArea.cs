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
    public class ACA_ArquivoArea : Abstract_ACA_ArquivoArea
	{
        /// <summary>
        /// Id do arquivo da área.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int aar_id { get; set; }

        /// <summary>
        /// Descrição do arquivo da área.
        /// </summary>
        [MSValidRange(200, "Descrição do arquivo/link deve possuir até 200 caractesres.")]
        [MSNotNullOrEmpty("Descrição do arquivo/link é obrigatório.")]
        public override string aar_descricao { get; set; }

        /// <summary>
        /// Link do arquivo da área.
        /// </summary>
        [MSValidRange(200, "Link deve possuir até 200 caractesres.")]
        public override string aar_link { get; set; }

        /// <summary>
        /// Id do tipo de área de documento.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de área de documento é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tad_id { get; set; }

        /// <summary>
        /// Situação do registro.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte aar_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime aar_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime aar_dataAlteracao { get; set; }

        /// <summary>
        /// Tipo de documento (1 - Arquivo; 2 - Link).
        /// </summary>
        [MSNotNullOrEmpty("Tipo de documento é obrigatório.")]
        public override byte aar_tipoDocumento { get; set; }

        /// <summary>
        /// ID do registro (propriedade auxiliar utilizado na tela de cadastro de documentos)
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// Nome do arquivo (propriedade auxiliar utilizado na tela de cadastro de documentos)
        /// </summary>
        public string arq_nome { get; set; }
	}
}