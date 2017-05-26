using System;
using System.Collections.Generic;

namespace GestaoEscolar.Api.Models
{
    public class MinhasTurmas
    {
        // public Guid diretoriaId { get; set; }
        public int escolaId { get; set; }
        public int unidadeId { get; set; }
        public string escolaNome { get; set; }
        public int calendarioId { get; set; }
        public long cursoId { get; set; }
        public long curriculoId { get; set; }
        public long periodoId { get; set; }
        public long cicloId { get; set; }
        public int qtdRegistros { get; set; }
        public IEnumerable<Turma> Turmas { get; set; }
    }
}