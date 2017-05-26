using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class SondagemSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public List<SondagemDTO> sondagens { get; set; }
    }

    public class SondagemDTO
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string descricao { get; set; }
        public List<QuestaoDTO> questoes { get; set; }
        public List<QuestaoDTO> subQuestoes { get; set; }
        public List<RespostaDTO> respostas { get; set; }
        public List<AgendamentoDTO> agendamentos { get; set; }
    }

    public class QuestaoDTO
    {
        public int id { get; set; }
        public string descricao { get; set; }
        public int ordem { get; set; }
    }

    public class RespostaDTO
    {
        public int id { get; set; }
        public string sigla { get; set; }
        public string descricao { get; set; }
        public int ordem { get; set; }
    }

    public class AgendamentoDTO
    {
        public int id { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }
        public List<RespostaAlunoDTO> respostasAluno { get; set; }
    }

    public class RespostaAlunoDTO
    {
        public int idQuestao { get; set; }
        public int idSubQuestao { get; set; }
        public int idResposta { get; set; }
    }
}
