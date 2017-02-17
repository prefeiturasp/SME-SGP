using System;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Professor
    {
        public Int64 Doc_id { get; set; }
        public string Pes_nome { get; set; }
        public string Psd_numeroCPF { get; set; }
        public int esc_id { get; set; }
        //public Escola Escola { get; set; }
        public List<Turma> Turmas { get; set; }
    }
}