using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Atividade
    {
        public long Tud_id { get; set; }
        public string Tnt_nome { get; set; }
        public string Tnt_descricao { get; set; }
        public int Tav_id { get; set; }
        
        public string Tnt_dataAlteracao
        {
            get
            {
                return tnt_dataAlteracao_bd.ToString("dd/MM/yyyy HH:mm:ss.fff");
            }
        }

        public DateTime tnt_dataAlteracao_bd { private get; set; }

        public byte Tnt_situacao { get; set; }
        public int Tnt_id { get; set; }
        public int Tnt_efetivado { get; set; }
        public int Tnt_exclusiva { get; set; }
        public long Pro_protocolo { get; set; }
        public long Tnt_chaveDiario { get; set; }

        public List<AtividadeAluno> AtividadeAlunos { get; set; }
    }
}
