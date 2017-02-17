using System;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    [Serializable]
    public class RHU_ColaboradorCargo : Abstract_RHU_ColaboradorCargo
    {
        public override int coc_id { get; set; }
        [MSValidRange(30, "Matrícula pode conter até 30 caracteres.")]
        public override string coc_matricula { get; set; }
        [MSValidRange(1000, "Observações pode conter até 1000 caracteres.")]
        public override string coc_observacao { get; set; }
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime coc_vigenciaInicio { get; set; }
        [MSNotNullOrEmpty("Unidade administrativa é obrigatório.")]
        public override Guid uad_id { get; set; }
        [MSDefaultValue(1)]
        public override byte coc_situacao { get; set; }
        public override DateTime coc_dataCriacao { get; set; }
        public override DateTime coc_dataAlteracao { get; set; }
    }
}
