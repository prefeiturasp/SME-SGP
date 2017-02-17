namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_CurriculoDisciplinaDTO : ACA_CurriculoDisciplina
    {
        public ACA_DisciplinaDTO disciplina { get; set; }
        public new bool? IsNew { get { return null; } }
        public new int? cur_id { get; set; }
        public new int? crr_id { get; set; }
        public new int? crp_id { get; set; }
        public new int? dis_id { get; set; }
    }
}
