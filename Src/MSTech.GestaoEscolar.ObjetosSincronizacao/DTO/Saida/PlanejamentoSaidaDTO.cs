using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class PlanejamentoSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }
        public List<CLS_PlanejamentoCiclo> PlanejamentoCiclo { get; set; }
        public List<CLS_TurmaDisciplinaPlanejamentoDTO> PlanejamentoAnual { get; set; }
        public List<CLS_AlunoPlanejamento> PlanejamentoAluno { get; set; }
        public List<CLS_AlunoPlanejamentoRelacionada> PlanejamentoAlunoRelacionada { get; set; }
        public List<ACA_ArquivoArea> PlanejamentoDocumentos { get; set; }
        public List<ACA_TipoAreaDocumento> TipoDocumentos {get; set;}

        public PlanejamentoSaidaDTO()
        {           
            this.Status = 0;
            this.Date = DateTime.Now.ToString(Util.Util.mascaraData);
            this.PlanejamentoCiclo = new List<CLS_PlanejamentoCiclo>();
            this.PlanejamentoAnual = new List<CLS_TurmaDisciplinaPlanejamentoDTO>();
            this.PlanejamentoAluno = new List<CLS_AlunoPlanejamento>();
            this.PlanejamentoAlunoRelacionada = new List<CLS_AlunoPlanejamentoRelacionada>();
            this.PlanejamentoDocumentos = new List<ACA_ArquivoArea>();
            this.TipoDocumentos = new List<ACA_TipoAreaDocumento>();
        }
    }
}
