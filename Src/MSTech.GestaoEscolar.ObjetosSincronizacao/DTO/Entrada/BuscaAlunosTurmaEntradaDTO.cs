using System;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaAlunosTurmaEntradaDTO
    {
        public Int32 Esc_id { get; set; }
        public Int64 Tur_id { get; set; }
        public string SyncDate { get; set; }
    }
}