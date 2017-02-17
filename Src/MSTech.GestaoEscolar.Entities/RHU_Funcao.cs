using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    [Serializable]
    public class RHU_Funcao : Abstract_RHU_Funcao
    {
        [MSValidRange(20, "Código pode conter até 20 caracteres.")]
        public override string fun_codigo { get; set; }
        [MSValidRange(100, "Nome da função pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome da função é obrigatório.")]
        public override string fun_nome { get; set; }
        [MSValidRange(20, "Código de integração pode conter até 20 caracteres.")]
        public override string fun_codIntegracao { get; set; }
        [MSDefaultValue(1)]
        public override byte fun_situacao { get; set; }
        public override DateTime fun_dataCriacao { get; set; }
        public override DateTime fun_dataAlteracao { get; set; }
        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }
    }
}
