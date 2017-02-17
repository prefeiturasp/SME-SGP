namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_TipoCicloDTO : ACA_TipoCiclo
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? tci_id { get; set; }
        }

    }
}
