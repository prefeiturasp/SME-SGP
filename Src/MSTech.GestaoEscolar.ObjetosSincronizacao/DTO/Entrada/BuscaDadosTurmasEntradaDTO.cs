using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    public class BuscaDadosTurmasEntradaDTO
    {
        public Guid Ent_id { get; set; }
        public int cal_id { get; set; }
        public Guid uad_idSuperior { get; set; }
        public int esc_id { get; set; }
        public int cur_id { get; set; }
        public int crr_id { get; set; }
        public int crp_id { get; set; }
        public int trn_id { get; set; }
        public string tur_codigo { get; set; }
        public Guid usu_id { get; set; }
        public Guid gru_id { get; set; }
        public long doc_id { get; set; }
        public long tur_id { get; set; }
    }
}
