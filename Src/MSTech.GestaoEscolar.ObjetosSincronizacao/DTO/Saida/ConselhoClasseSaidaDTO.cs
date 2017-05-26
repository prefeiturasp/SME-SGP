namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ConselhoClasseSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public List<ConselhoClasseDTO> dadosConselho { get; set; }
    }

    public class ConselhoClasseDTO
    {
        public long alu_id { get; set; }

        public int mtu_id { get; set; }

        public int ava_id { get; set; }

        public int fav_id { get; set; }

        public int tpc_id { get; set; }
        public int tpc_ordem { get; set; }
        public string tpc_nome { get; set; }

        public int mtu_numeroChamada { get; set; }

        public string qualidade { get; set; }
        public string desempenho { get; set; }
        public string recomendacaoAluno { get; set; }
        public string recomendacaoResponsavel { get; set; }

        public string cpe_atividadeFeita { get; set; }

        public string cpe_atividadePretendeFazer { get; set; }
    }
}
