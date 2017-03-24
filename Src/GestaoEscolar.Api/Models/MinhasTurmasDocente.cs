using System;
using System.Collections.Generic;

namespace GestaoEscolar.Api.Models
{
    public class MinhasTurmasDocente
    {
        public int escolaId { get; set; }
        public int unidadeId { get; set; }
        public string escolaNome { get; set; }
        public int calendarioId { get; set; }
        public IEnumerable<Turma> Turmas { get; set; }
    }
}