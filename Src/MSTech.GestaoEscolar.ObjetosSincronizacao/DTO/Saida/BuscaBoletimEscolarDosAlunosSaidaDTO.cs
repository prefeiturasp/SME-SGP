using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaBoletimEscolarDosAlunosSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }

        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int ava_id { get; set; }
        public int fav_id { get; set; }
        public decimal fav_variacao { get; set; }
        public int alc_id { get; set; }
        public long tur_id { get; set; }
        public int mtu_numeroChamada { get; set; }
        public int cal_id { get; set; }
        public int cal_ano { get; set; }
        public string cur_nome { get; set; }
        public long arq_idFoto { get; set; }
        public string ava_nome { get; set; }
        public string uad_nome { get; set; }
        public string esc_nome { get; set; }
        public string pes_nome { get; set; }
        public string pes_nomeOficial { get; set; }
        public string pes_nomeRegistro { get; set; }
        public string pes_nome_abreviado { get; set; }
        public string tur_codigo { get; set; }
        public int tci_id { get; set; }
        public string tci_nome { get; set; }
        public string tci_layout { get; set; }
        public string cicloClass { get; set; }
        public bool tci_exibirBoletim { get; set; }
        public bool fechamentoPorImportacao { get; set; }
        public string cpe_atividadeFeita { get; set; }
        public string cpe_atividadePretendeFazer { get; set; }
        public string recuperacaoParalela { get; set; }
        public string alc_matricula { get; set; }
        public string alc_matriculaEstadual { get; set; }
        public bool mostraConceitoGlobal { get; set; }
        public bool exibeCompensacaoAusencia { get; set; }
        public string nomeNota { get; set; }
        public int QtComponenteRegencia { get; set; }
        public int QtComponentes { get; set; }
        public string parecerConclusivo { get; set; }
        public bool possuiFreqExterna { get; set; }

        public string displayPerfilAluno { get; set; }
        public string displayRecomendacoes { get; set; }
        public string displayResultados { get; set; }
        public string displayParecerConclusivo { get; set; }

        public int cur_id { get; set; }
        public int crr_id { get; set; }
        public int crp_id { get; set; }

        public List<string> qualidade { get; set; }
        public List<string> desempenho { get; set; }
        public List<string> recomendacaoAluno { get; set; }
        public List<string> recomendacaoResponsavel { get; set; }

        public List<BuscaBoletimEscolarDosAlunosSaidaPeriodosDTO> periodos { get; set; }

        public string linhaTerritorioSaber { get; set; }
        public string territorioSaber { get; set; }

        public List<BuscaBoletimEscolarDosAlunosSaidaTodasDisciplinasDTO> todasDisciplinas { get; set; }

        public bool BoletimLiberado { get; set; }

        public string justificativaAbonoFalta { get; set; }

        public bool ensinoInfantil { get; set; }
    }
}
