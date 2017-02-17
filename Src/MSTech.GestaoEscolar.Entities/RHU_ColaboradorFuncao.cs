using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
    [Serializable]
    public class RHU_ColaboradorFuncao : Abstract_RHU_ColaboradorFuncao
    {
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime cof_vigenciaInicio { get; set; }
        [MSValidRange(30, "Matrícula pode conter até 30 caracteres.")]
        public override string cof_matricula { get; set; }              
        [MSNotNullOrEmpty("Unidade administrativa é obrigatório.")]
        public override Guid uad_id { get; set; }
        [MSValidRange(1000, "Observações pode conter até 1000 caracteres.")]
        public override string cof_observacao { get; set; }
        [MSDefaultValue(1)]
        public override byte cof_situacao { get; set; }
        public override DateTime cof_dataCriacao { get; set; }
        public override DateTime cof_dataAlteracao { get; set; }
    }
}
