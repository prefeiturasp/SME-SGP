using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class AlunoProtocoloExcedente
    {
        public byte alu_situacao { get; set; }
        public string esc_nome { get; set; }
        public string cur_nome { get; set; }
        public string crp_descricao { get; set; }
        public string ttn_nome { get; set; }
        public string ale_dataInteresse { get; set; }
        public int posicao { get; set; }
        public byte ale_situacao { get; set; }
        public int idadeMinima { get; set; }
        public int idadeMaxima { get; set; }

        public AlunoProtocoloExcedente()
        {
            this.alu_situacao = 8; //excedente
            this.esc_nome = string.Empty;
            this.cur_nome = string.Empty;
            this.crp_descricao = string.Empty;
            this.ttn_nome = string.Empty;
            this.ale_dataInteresse = string.Empty;
            this.posicao = -1;
            this.ale_situacao = 1; //na lista de espera
            this.idadeMinima = 0;
            this.idadeMaxima = 1000;
        }
    }
}
