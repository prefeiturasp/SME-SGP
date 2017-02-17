using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    [Serializable]
    public class ACA_TipoPeriodoCalendario : Abstract_ACA_TipoPeriodoCalendario
    {
        [MSValidRange(100, "Tipo de período do calendário pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de período do calendário é obrigatório.")]
        public override string tpc_nome { get; set; }
        public override string tpc_nomeAbreviado { get; set; }
        [MSNotNullOrEmpty("Ordem é obrigatório.")]
        public override int tpc_ordem { get; set; }
        [MSDefaultValue(1)]
        public override byte tpc_situacao { get; set; }
        public override DateTime tpc_dataCriacao { get; set; }
        public override DateTime tpc_dataAlteracao { get; set; }
    }
}
