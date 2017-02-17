namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_TipoModalidadeEnsinoDTO : ACA_TipoModalidadeEnsino
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? tme_id { get; set; }
        }

    }
}
