namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_CursoRelacionadoDTO : ACA_CursoRelacionado
    {
        public new bool? IsNew { get { return null; } }
        public new int? cur_id { get; set; }
        public new int? crr_id { get; set; }
    }
}
