namespace GestaoEscolar.Api.Models
{
    public class TurmaCompCur : jsonObject
    {
        public string turmaDisciplinaId { get; set; }
        public byte turmaDisciplinaTipo { get; set; }
        public string turmaDocentePosicao { get; set; }
    }
}