using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaBoletimEscolarDosAlunosSaidaTodasDisciplinasDTO
    {
        public long tud_id { get; set; }
        public string Disciplina { get; set; }
        public int tds_ordem { get; set; }
        public string totalAulas { get; set; }
        public string totalFaltas { get; set; }
        public string ausenciasCompensadas { get; set; }
        public string FrequenciaFinalAjustada { get; set; }
        public byte tud_Tipo { get; set; }
        public bool tipoComponenteRegencia { get; set; }
        public bool tipoDocenciaCompartilhada { get; set; }
        public bool tud_global { get; set; }
        public int mostrarDisciplina { get; set; }
        public decimal NotaTotal { get; set; }
        public string MediaFinal { get; set; }
        public byte regencia { get; set; }
        public bool enriquecimentoCurricular { get; set; }
        public string parecerFinal { get; set; }
        public string parecerConclusivo { get; set; }
        public bool recuperacao { get; set; }
        public string disRelacionadas { get; set; }

        public List<BuscaBoletimEscolarDosAlunosSaidaNotasDTO> notas { get; set; }
    }
}
