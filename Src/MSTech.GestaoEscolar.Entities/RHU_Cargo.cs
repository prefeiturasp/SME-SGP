using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    [Serializable]
    public class RHU_Cargo : Abstract_RHU_Cargo
    {
        [MSValidRange(20, "Código do cargo pode conter até 20 caracteres.")]
        public override string crg_codigo { get; set; }
        [MSValidRange(100, "Nome do cargo pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome do cargo é obrigatório.")]
        public override string crg_nome { get; set; }        
        [MSNotNullOrEmpty("Tipo de vínculo é obrigatório.")]
        public override int tvi_id { get; set; }
        [MSValidRange(20, "Código de integração pode conter até 20 caracteres.")]
        public override string crg_codIntegracao { get; set; }
        [MSDefaultValue(1)]
        public override byte crg_situacao { get; set; }
        public override DateTime crg_dataCriacao { get; set; }
        public override DateTime crg_dataAlteracao { get; set; }
        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }

        /// <summary>
		/// Tipo do cargo: 1-Comum, 2-Cargo base, 3-Atribuição esporádica, 4-Indireto.
		/// </summary>
		[MSDefaultValue(1)]
        public override byte crg_tipo { get; set; }
    }
}
