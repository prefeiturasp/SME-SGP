namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ACA_TipoMovimentacaoDTO : ACA_TipoMovimentacao
    {
        public bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? tmv_id { get; set; }
        } 
    }
}
