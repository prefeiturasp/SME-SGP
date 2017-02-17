namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_TipoNivelEnsinoDTO : ACA_TipoNivelEnsino
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? tne_id { get; set; }
            public int? tne_idProximo { get; set; }
        }

    }
}
