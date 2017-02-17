using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class ListagemCompensacaoFaltaEntradaDTO
    {
        public Int32 Esc_id { get; set; }
        public Int64 Tur_id { get; set; }
        public string SyncDate { get; set; }
    }
}
