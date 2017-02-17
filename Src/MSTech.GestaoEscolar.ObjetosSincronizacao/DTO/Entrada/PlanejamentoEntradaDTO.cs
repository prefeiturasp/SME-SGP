using System;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class PlanejamentoEntradaDTO
    {
        
        public string SyncDate { get; set; }        
        public Int64 tur_id { get; set; }
        public string usu_login { get; set; }
    }
}