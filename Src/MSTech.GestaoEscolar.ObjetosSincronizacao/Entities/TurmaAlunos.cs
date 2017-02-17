using System;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TurmaAlunos
    {
        public Int64 Tur_id { get; set; }
        public List<Aluno> Alunos { get; set; }
    }
}