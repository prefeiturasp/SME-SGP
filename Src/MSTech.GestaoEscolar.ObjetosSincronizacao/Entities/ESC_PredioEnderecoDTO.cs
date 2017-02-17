namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ESC_PredioEnderecoDTO : ESC_PredioEndereco
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? prd_id { get; set; }
            public int? ped_id { get; set; }
        }
    }
}
