using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    [Serializable]
    public class RHU_Colaborador : Abstract_RHU_Colaborador
    { 
        [MSNotNullOrEmpty("Data de admissão é obrigatório.")]
        public override DateTime col_dataAdmissao { get; set; }        
        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }
        [MSDefaultValue(1)]
        public override byte col_situacao { get; set; }
        public override DateTime col_dataCriacao { get; set; }
        public override DateTime col_dataAlteracao { get; set; }
    }
}
