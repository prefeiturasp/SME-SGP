using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GestaoEscolar.Api.Models
{
    public class AulasDadas
    {
        public string nomeEscola { get; set; }
        public int escolaId { get; set; }
        public string nomeCalendario { get; set; }
        public int calendarioId { get; set; }
        public bool exibirTipoDocente { get; set; }
        public long turmaId { get; set; }
        public string turmaDisciplinaId { get; set; }
        public int tipoDocenteId { get; set; }
        public bool fechamentoAutomatico { get; set; }
        public IEnumerable<jsonObject> tiposDocente { get; set; }
        public IEnumerable<TurmaCompCur> turmasComponenteCurricular { get; set; }
    }
}