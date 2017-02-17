namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System;

    public class ACA_CalendarioPeriodoDTO : ACA_CalendarioPeriodo
    {
        public bool? IsNew { get { return null; } }
        public ACA_TipoPeriodoCalendarioDTO.Referencia tipoPeriodoCalendario { get; set; }

        public class Referencia
        {
            public int? cal_id { get; set; }
            public int? cap_id { get; set; }
        }       
    }

    /// <summary>
    /// Estrutura utilizada para sincronização com o Diario de Classe (NÂO ALTERAR)
    /// </summary>
    public class CalendarioPeriodo
    {
        public int Cal_id { get; set; }
        public int Cap_id { get; set; }
        public string Cap_descricao { get; set; }
        public int Tpc_id { get; set; }
        public DateTime Cap_dataInicio { get; set; }
        public DateTime Cap_dataFim { get; set; }
        public int Cap_situacao { get; set; }
    }
}