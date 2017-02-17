namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_CurriculoDTO : ACA_Curriculo
    {
        public new bool? IsNew { get { return null; } }
        public new int? cur_id { get; set; }

    }
}
