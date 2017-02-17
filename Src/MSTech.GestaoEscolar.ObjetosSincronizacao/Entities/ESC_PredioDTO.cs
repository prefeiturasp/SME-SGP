    namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ESC_PredioDTO : ESC_Predio
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? prd_id { get; set; }
        }
    }
}
