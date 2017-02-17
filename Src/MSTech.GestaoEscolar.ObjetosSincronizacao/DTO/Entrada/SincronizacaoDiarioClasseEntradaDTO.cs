using System;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class SincronizacaoDiarioClasseEntradaDTO
    {
        public string K4 { get; set; }
        public Int64 Pro_protocolo { get; set; }
        public string Versao { get; set; }

        public string pro_pacote;
    }
}