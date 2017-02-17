namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;

    public class ACA_AreaConhecimentoDTO : ACA_AreaConhecimento
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? aco_id { get; set; }
        }
    }
}
