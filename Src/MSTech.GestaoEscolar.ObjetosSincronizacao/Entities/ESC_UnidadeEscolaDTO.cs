namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ESC_UnidadeEscolaDTO : ESC_UnidadeEscola
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? esc_id { get; set; }
            public int? uni_id { get; set; }
        }
    }
}
