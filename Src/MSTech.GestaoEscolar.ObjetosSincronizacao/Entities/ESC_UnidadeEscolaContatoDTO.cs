namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities; 

    public class ESC_UnidadeEscolaContatoDTO : ESC_UnidadeEscolaContato
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? esc_id { get; set; }
            public int? uni_id { get; set; }
            public int? uec_id { get; set; }
        }
    }
}
