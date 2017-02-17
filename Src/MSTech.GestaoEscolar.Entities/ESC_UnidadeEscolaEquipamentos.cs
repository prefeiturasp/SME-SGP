/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class ESC_UnidadeEscolaEquipamentos : AbstractESC_UnidadeEscolaEquipamentos
    {
        /// <summary>
        /// Id da tabela ESC_UnidadeEscolaEquipamentos.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int ueq_id { get; set; }

        /// <summary>
        /// Aparelho de Televisão.
        /// </summary>
        [MSNotNullOrEmpty("Aparelho de Televisão é obrigatório.")]
        public override bool ueq_aparelhoTelevisao { get; set; }

        /// <summary>
        /// Videocassete.
        /// </summary>
        [MSNotNullOrEmpty("Videocassete é obrigatório.")]
        public override bool ueq_videocassete { get; set; }

        /// <summary>
        /// DVD.
        /// </summary>
        [MSNotNullOrEmpty("DVD é obrigatório.")]
        public override bool ueq_dvd { get; set; }

        /// <summary>
        /// Antena parabólica.
        /// </summary>
        [MSNotNullOrEmpty("Antena parabólica é obrigatório.")]
        public override bool ueq_antenaParabolica { get; set; }

        /// <summary>
        /// Copiadora.
        /// </summary>
        [MSNotNullOrEmpty("Copiadora é obrigatório.")]
        public override bool ueq_copiadora { get; set; }

        /// <summary>
        /// Retroprojetor.
        /// </summary>
        [MSNotNullOrEmpty("Retroprojetor é obrigatório.")]
        public override bool ueq_retroprojetor { get; set; }

        /// <summary>
        /// Impressora.
        /// </summary>
        [MSNotNullOrEmpty("Impressora é obrigatório.")]
        public override bool ueq_impressora { get; set; }

        /// <summary>
        /// Aparelho de som.
        /// </summary>
        [MSNotNullOrEmpty("Aparelho de som é obrigatório.")]
        public override bool ueq_aparelhoSom { get; set; }

        /// <summary>
        /// Projetor Multimídia (Data show).
        /// </summary>
        [MSNotNullOrEmpty("Projetor Multimídia (Data show) é obrigatório.")]
        public override bool ueq_projetorMultimidia { get; set; }

        /// <summary>
        /// Fax.
        /// </summary>
        [MSNotNullOrEmpty("Fax é obrigatório.")]
        public override bool ueq_fax { get; set; }

        /// <summary>
        /// Máquina Fotográfica/Filmadora.
        /// </summary>
        [MSNotNullOrEmpty("Máquina Fotográfica/Filmadora é obrigatório.")]
        public override bool ueq_maquinaFotografica { get; set; }

        /// <summary>
        /// Computadores.
        /// </summary>
        [MSNotNullOrEmpty("Computadores é obrigatório.")]
        public override bool ueq_computadores { get; set; }

        /// <summary>
        /// Situação do registro: 1 - Ativo, 3 - Excluído.
        /// </summary>
        [MSDefaultValue(1)]
        public override short ueq_situacao { get; set; }

        /// <summary>
        /// Data de Criação de registro.
        /// </summary>
        public override DateTime ueq_dataCriacao { get; set; }

        /// <summary>
        /// Data de Alteração do registro.
        /// </summary>
        public override DateTime ueq_dataAlteracao { get; set; }
	}
}