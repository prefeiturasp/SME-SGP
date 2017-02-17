namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System;

    public class ACA_TipoDisciplinaDeficienciaDTO :  ACA_TipoDisciplinaDeficiencia
    {
        public new bool? IsNew { get { return null; } }

        public class Referencia
        {
            public int? tds_id { get; set; }
            public Guid? tde_id { get; set; }
        }
    }
    /// <summary>
    /// Estrutura utilizada para sincronização com o Diario de Classe (NÂO ALTERAR)
    /// </summary>
    public class TipoDisciplinaDeficiencia
    {
        public int tds_id { get; set; }
        public Guid tde_id { get; set; }
    }
}
