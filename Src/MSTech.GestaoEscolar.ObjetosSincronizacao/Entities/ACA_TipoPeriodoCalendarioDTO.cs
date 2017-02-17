namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ACA_TipoPeriodoCalendarioDTO : ACA_TipoPeriodoCalendario
    {
        public new bool? IsNew { get { return null; } }
        
        public class Referencia
        {
            public int? tpc_id { get; set; }
        }
    }
}
