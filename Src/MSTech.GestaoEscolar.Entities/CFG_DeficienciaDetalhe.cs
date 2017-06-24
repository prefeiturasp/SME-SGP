/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;

    [Serializable]
    public class CFG_DeficienciaDetalhe : Abstract_CFG_DeficienciaDetalhe
	{
        /// <summary>
        /// ID da deficiencia no Core - Pes_TipoDeficiencia.
        /// </summary>
        [MSNotNullOrEmpty("[tde_id] é obrigatório.")]
        public override Guid tde_id { get; set; }

        /// <summary>
        /// ID do detalhe, gerado automaticamente.
        /// </summary>
        public override int dfd_id { get; set; }

        /// <summary>
        /// Nome do detalhamento da deficiência.
        /// </summary>
        [MSValidRange(100)]
        [MSNotNullOrEmpty("Nome do detalhe é obrigatório.")]
        public override string dfd_nome { get; set; }

        /// <summary>
        /// Situação do detalhe - padrão é sem 1.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte dfd_situacao { get; set; }

        /// <summary>
        /// Data de criação .
        /// </summary>
        public override DateTime dfd_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração.
        /// </summary>
        public override DateTime dfd_dataAlteracao { get; set; }


        public bool PermiteExcluir { get; set; }
    }
}