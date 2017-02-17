using System;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class FormatoAvaliacao
    {
        public int Fav_id { get; set; }
        public Guid Ent_id { get; set; }
        public int Esc_id { get; set; }
        public int Uni_id { get; set; }
        public bool Fav_padrao { get; set; }
        public string Fav_nome { get; set; }
        public Int16 Fav_tipo { get; set; }
        public byte Fav_tipoLancamentoFrequencia { get; set; }
        public bool Fav_bloqueiaFrequenciaEfetivacao { get; set; }
        public bool Fav_planejamentoAulasNotasConjunto { get; set; }
        public bool Fav_bloqueiaFrequenciaEfetivacaoDisciplina { get; set; }
        public bool Fav_conceitoGlobalDocente { get; set; }
        public bool Fav_obrigatorioRelatorioReprovacao { get; set; }
        public byte Fav_tipoApuracaoFrequencia { get; set; }
        public byte Fav_calculoQtdeAulasDadas { get; set; }
    }
}
