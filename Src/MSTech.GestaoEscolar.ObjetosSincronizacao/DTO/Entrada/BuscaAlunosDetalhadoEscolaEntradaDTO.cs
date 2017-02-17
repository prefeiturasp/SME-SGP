using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaAlunosDetalhadoEscolaEntradaDTO
    {
        public Guid ent_id { get; set; }
        public Int32 esc_id { get; set; }
        public Guid uad_id { get; set; }
    }
}
