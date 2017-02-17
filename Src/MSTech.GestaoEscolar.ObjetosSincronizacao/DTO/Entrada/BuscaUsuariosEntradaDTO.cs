using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaUsuariosEntradaDTO
    {
        public int esc_id { get; set; }
        public string SyncDate { get; set; }
        public string Usu_login { get; set; }
    }
}