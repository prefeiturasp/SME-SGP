namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MatriculaTurmaSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtu_numeroChamada { get; set; }
        public string alc_matricula { get; set; }
        public string pes_nome { get; set; }
        public string esc_nome { get; set; }
        public string uad_nome { get; set; }
        public string tur_codigo { get; set; }
        public string tci_nome { get; set; }
        public long arq_idFoto { get; set; }
    }
    
}
