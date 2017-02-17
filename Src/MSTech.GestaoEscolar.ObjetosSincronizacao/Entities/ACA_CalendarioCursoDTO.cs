namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ACA_CalendarioCursoDTO : ACA_CalendarioCurso
    {
        public bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? cal_id { get; set; }
            public int? cur_id { get; set; }
        }
    }
}
