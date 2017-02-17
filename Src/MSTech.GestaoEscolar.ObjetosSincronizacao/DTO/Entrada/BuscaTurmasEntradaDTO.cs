using System;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaTurmasEntradaDTO
    {
        public string esc_id { get; set; }
        public string SyncDate { get; set; }
        public int cal_ano { get; set; }
        public Int64 tur_id { get; set; }
        public string usu_login { get; set; }
    }
}