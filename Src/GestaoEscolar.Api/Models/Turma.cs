namespace GestaoEscolar.Api.Models
{
    public class Turma
    {
        public string codigo { get; set; }
        public string nome { get; set; }
        public string curso { get; set; }
        public string turno { get; set; }
        public string tipoDocente { get; set; }
        public long turmaId { get; set; }
        public long turmaDisciplinaId { get; set; }
        public byte turmaDocentePosicao { get; set; }
        public bool AulasDadasVisivel { get; set; }
        public bool AulasDadasOk { get; set; }

    }
}