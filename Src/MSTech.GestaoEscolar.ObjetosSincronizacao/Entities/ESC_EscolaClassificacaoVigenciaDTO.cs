namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ESC_EscolaClassificacaoVigenciaDTO : ESC_EscolaClassificacaoVigencia
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public long? ecv_id { get; set; }
        }
    }
}
