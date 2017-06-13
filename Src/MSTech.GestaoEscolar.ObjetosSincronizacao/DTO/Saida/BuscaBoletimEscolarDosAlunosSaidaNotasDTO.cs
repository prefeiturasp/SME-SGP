using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaBoletimEscolarDosAlunosSaidaNotasDTO
    {
        public int tpc_id { get; set; }
        public BuscaBoletimEscolarDosAlunosSaidaNotaDTO nota { get; set; }
    }

    public class BuscaBoletimEscolarDosAlunosSaidaNotaDTO
    {
        public string Nota { get; set; }
        public string Conceito { get; set; }
        public int tpc_id { get; set; }
        public string tpc_nome { get; set; }
        public string NotaRP { get; set; }
        public string numeroAulas { get; set; }
        public string numeroFaltas { get; set; }
        public byte tud_Tipo { get; set; }
        public bool possuiFreqExterna { get; set; }
    }
}
