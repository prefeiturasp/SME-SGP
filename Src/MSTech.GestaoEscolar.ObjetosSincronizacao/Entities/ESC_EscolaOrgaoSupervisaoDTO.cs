namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ESC_EscolaOrgaoSupervisaoDTO : ESC_EscolaOrgaoSupervisao
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? esc_id { get; set; }
            public int? eos_id { get; set; }
        }
    }
}
