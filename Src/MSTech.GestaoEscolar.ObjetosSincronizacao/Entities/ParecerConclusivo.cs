namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class ParecerConclusivo
    {
        public string CodigoEOLTurma { get; set; }
        public string CodigoEOLAluno { get; set; }
        public string Resultado { get; set; }
    }

    public class ErrorMessage
    {
        public string message { get; set; }
    }
}
