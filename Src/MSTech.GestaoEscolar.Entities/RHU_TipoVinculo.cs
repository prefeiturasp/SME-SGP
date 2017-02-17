using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    [Serializable]
    public class RHU_TipoVinculo : Abstract_RHU_TipoVinculo
    {
        [MSValidRange(100, "Tipo de vínculo pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de vínculo é obrigatório.")]
        public override string tvi_nome { get; set; }
        [MSNotNullOrEmpty("Horas semanais é obrigatório e deve ser um número inteiro maior que 0 (zero).")]
        public override int tvi_horasSemanais { get; set; }
        [MSNotNullOrEmpty("Minutos de almoço é obrigatório e deve ser um número inteiro maior que 0 (zero).")]
        public override int tvi_minutosAlmoco { get; set; }
        [MSValidRange(20, "Código de integração pode conter até 20 caracteres.")]
        public override string tvi_codIntegracao { get; set; }
        [MSDefaultValue(1)]
        public override byte tvi_situacao { get; set; }
        public override DateTime tvi_dataCriacao { get; set; }
        public override DateTime tvi_dataAlteracao { get; set; }
        [MSNotNullOrEmpty("Entidade é obrigatório")]
        public override Guid ent_id { get; set; }
    }
}
