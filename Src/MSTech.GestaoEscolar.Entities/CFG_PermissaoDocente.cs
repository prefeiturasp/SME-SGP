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
	public class CFG_PermissaoDocente : Abstract_CFG_PermissaoDocente
	{
        /// <summary>
        /// ID Permissão do docente.
        /// </summary>
        [DataObjectField(true, true, false)]
        public override int pdc_id { get; set; }

        /// <summary>
        /// ID tipo do docente.
        /// </summary>
        [MSNotNullOrEmpty("Tipo docente é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override byte tdc_id { get; set; }

        /// <summary>
        /// ID tipo do docente permissao.
        /// </summary>
        [MSNotNullOrEmpty("Tipo docente permissão é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override byte tdc_idPermissao { get; set; }

        /// <summary>
        /// id do modulo de permissao (EnumModuloPermissao).
        /// </summary>
        [MSNotNullOrEmpty("Módulo de permissão é obrigatório.")]
        public override byte pdc_modulo { get; set; }

        /// <summary>
        /// Indicado permissao de consulta.
        /// </summary>
        [MSNotNullOrEmpty("Permissão de consulta é obrigatório.")]
        [MSDefaultValue(true)]
        public override bool pdc_permissaoConsulta { get; set; }

        /// <summary>
        /// Indicado permissao de edicao.
        /// </summary>
        [MSNotNullOrEmpty("Permissão de edição é obrigatório.")]
        [MSDefaultValue(true)]
        public override bool pdc_permissaoEdicao { get; set; }

        /// <summary>
        /// Situação do registro.
        /// 1-Ativo,
        /// 3-Excluido
        /// </summary>
        [MSDefaultValue(1)]
        public override byte pdc_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro..
        /// </summary>
        public override DateTime pdc_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro..
        /// </summary>
        public override DateTime pdc_dataAlteracao { get; set; }
	}
}