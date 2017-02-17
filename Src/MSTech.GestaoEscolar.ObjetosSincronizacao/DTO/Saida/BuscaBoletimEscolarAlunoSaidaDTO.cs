using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class BuscaBoletimEscolarAlunoSaidaDTO
    {
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Date { get; set; }

        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public string tur_codigo { get; set; }
        public long tur_id { get; set; }
        public int tpc_id { get; set; }
        public int tpc_ordem { get; set; }
        public int mtd_id { get; set; }
        public long tud_id { get; set; }
        public bool tud_global { get; set; }
        public string Disciplina { get; set; }
        public string DisciplinaEspecial { get; set; }
        public bool tud_disciplinaEspecial { get; set; }
        public string tpc_nome { get; set; }
        public int numeroFaltas { get; set; }
        public string avaliacao { get; set; }
        public string avaliacaoOriginal { get; set; }
        public string avaliacaoSemPosConselho { get; set; }
        public string avaliacaoPosConselho { get; set; }
        public bool NotaNumerica { get; set; }
        public string avaliacaoAdicional { get; set; }
        public bool NotaAdicionalNumerica { get; set; }
        public string NotaRP { get; set; }
        public int NotaIDRP { get; set; }
        public bool mostraConceito { get; set; }
        public bool mostraNota { get; set; }
        public bool ava_mostraConceito { get; set; }
        public bool ava_mostraNota { get; set; }
        public bool mostraFrequencia { get; set; }
        public bool naoExibirNota { get; set; }
        public bool naoExibirFrequencia { get; set; }
        public bool naoExibirBoletim { get; set; }
        public decimal NotaSomar { get; set; }
        public decimal frequenciaAcumulada { get; set; }
        public bool MostrarLinhaDisciplina { get; set; }
        public int NotaID { get; set; }
        public int ava_id { get; set; }
        public byte ava_tipo { get; set; }
        public int fav_id { get; set; }
        public byte fav_tipo { get; set; }
        public bool ava_exibeSemProfessor { get; set; }
        public bool ava_exibeNaoAvaliados { get; set; }
        public bool semProfessor { get; set; }
        public bool naoAvaliado { get; set; }
        public bool naoLancarNota { get; set; }
        public int ava_idRec { get; set; }
        public string ava_nomeRec { get; set; }
        public string esc_codigo { get; set; }
        public string esc_nome { get; set; }
        public string NotaRecEsp { get; set; }
        public int ava_idRecEsp { get; set; }
        public int NotaIDRecEsp { get; set; }
        public decimal NotaTotal { get; set; }
        public string NotaResultado { get; set; }
        public long tud_idResultado { get; set; }
        public int mtu_idResultado { get; set; }
        public int mtd_idResultado { get; set; }
        public int NotaIdResultado { get; set; }
        public int fav_idResultado { get; set; }
        public int ava_idResultado { get; set; }
        public bool notaDisciplinaConceito { get; set; }
        public int dda_id { get; set; }
        public byte tud_tipo { get; set; }
        public int ausenciasCompensadas { get; set; }
        public decimal FrequenciaFinalAjustada { get; set; }
        public decimal FrequenciaGlobal { get; set; }
        public int esa_tipo { get; set; }
        public byte regencia { get; set; }
        public string nomeDisciplina { get; set; }
        public int tds_id { get; set; }
        public int tds_tipo { get; set; }
        public int tds_ordem { get; set; }
        public bool EnriquecimentoCurricular { get; set; }
        public bool Recuperacao { get; set; }
        public string ParecerFinal { get; set; }
        public short mtu_resultado { get; set; }
        public string ParecerConclusivo { get; set; }
        public string usuarioParecerConclusivo { get; set; }
        public DateTime dataAlteracaoParecerConclusivo { get; set; }

        public decimal fav_variacao { get; set; }
        public string frequencia { get; set; }
        public bool PermiteEditar { get; set; }
        public int esa_id { get; set; }
        public byte eap_ordem { get; set; }

        public int cur_id { get; set; }
        public int crr_id { get; set; }
        public int crp_id { get; set; }

        // Variáveis projeto complementar histórico
        public int ProjetoId { get; set; }
        public int NotaProjetoId { get; set; }
        public byte ResultadoProjeto { get; set; }
        public bool ProjetoComplementar { get; set; }

        public int numeroAulas { get; set; }
        public string strFrequenciaFinalAjustada { get; set; }
        public long TudIdRegencia { get; set; }
        public int MtdIdRegencia { get; set; }
        public int AtdIdRegencia { get; set; }
        public bool PermiteEdicaoDocente { get; set; }

        //Disciplinas relacionadas (docencia compartilhada)
        public string disRelacionadas { get; set; }
    }
}
