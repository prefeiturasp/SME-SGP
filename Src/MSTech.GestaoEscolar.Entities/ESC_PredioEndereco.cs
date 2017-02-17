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
    public class ESC_PredioEndereco : Abstract_ESC_PredioEndereco
    {
        [DataObjectField(true, false, false)]
        public override int prd_id { get; set; }
        [DataObjectField(true, false, false)]
        public override int ped_id { get; set; }
        [MSNotNullOrEmpty("Endereço é obrigatório.")]
        public override Guid end_id { get; set; }
        [MSNotNullOrEmpty("Unidade endereço é obrigatório.")]
        public override Guid uae_id { get; set; }
        [MSValidRange(20, "Número deve conter até 20 caracteres.")]
        [MSNotNullOrEmpty("Número do endereço é obrigatório.")]
        public override string ped_numero { get; set; }
        [MSValidRange(100, "Complemento deve conter até 100 caracteres.")]
        public override string ped_complemento { get; set; }
        [MSDefaultValue(1)]
        public override byte ped_situacao { get; set; }
        public override DateTime ped_dataCriacao { get; set; }
        public override DateTime ped_dataAlteracao { get; set; }
        public override bool ped_enderecoPrincipal { get; set; }
        public override decimal ped_latitude { get; set; }
        public override decimal ped_longitude { get; set; }
    }
}