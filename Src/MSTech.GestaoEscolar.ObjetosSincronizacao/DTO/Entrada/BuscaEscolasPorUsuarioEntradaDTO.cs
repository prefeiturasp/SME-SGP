using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaEscolasPorUsuarioEntradaDTO
    {
        public Guid ent_id { get; set; }
        public Guid usu_id { get; set; }
        public Guid gru_id { get; set; }
        public Guid uad_idSuperior { get; set; }
    }
}
