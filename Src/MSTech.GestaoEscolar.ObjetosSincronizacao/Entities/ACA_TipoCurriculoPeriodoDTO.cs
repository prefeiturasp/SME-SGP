namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_TipoCurriculoPeriodoDTO : ACA_TipoCurriculoPeriodo
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? tcp_id { get; set; }
        }

    }
}
