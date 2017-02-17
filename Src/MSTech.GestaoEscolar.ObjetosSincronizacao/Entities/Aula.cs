using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Aula
    {
        public int Tau_id { get; set; }
        public long Tur_id { get; set; }
        public long Tud_id { get; set; }
        public long Tud_idRelacionado { get; set; }
        public long Doc_id { get; set; }
        public int Dis_id { get; set; }
        public int Esc_id { get; set; }
        public byte Tdt_posicao { get; set; }
        public long Pro_protocolo { get; set; }
        public string Tau_data
        {
            get
            {
                return tau_data_bd.ToString("dd/MM/yyyy HH:mm:ss.fff");
            }
        }

        public DateTime tau_data_bd { private get; set; }
        public int Tau_numeroAulas { get; set; }
        public string Tau_diarioClasse { get; set; }

        public string Tau_dataAlteracao
        {
            get
            {
                return tau_dataAlteracao_bd.ToString("dd/MM/yyyy HH:mm:ss.fff");
            }
        }

        public DateTime tau_dataAlteracao_bd { private get; set; }
        
        public byte Tau_situacao { get; set; }
        public string Tau_planoAula { get; set; }
        public string Tau_atividadeCasa { get; set; }
        public int Tau_reposicao { get; set; }
        public string Tau_sintese { get; set; }        
    
        public List<Aluno> Alunos { get; set; }
        public List<Atividade> Atividades { get; set; }
        public List<TurmaAulaRecurso> Recursos { get; set; }
        public List<Regencia> Regencias { get; set; }
        public List<CLS_TurmaAulaPlanoDisciplina> PlanoAulaRegenciaDisciplinas { get; set; }

        public string Tau_dataPrevista { get { return string.Empty; } }

        public long Tud_idExperiencia { get; set; }
        public int Tau_idExperiencia { get; set; }
        public long ProtocoloExperiencia { get; set; }

        public Aula()
        {
            this.Alunos = new List<Aluno>();
            this.Atividades = new List<Atividade>();
            this.Recursos = new List<TurmaAulaRecurso>();
            this.Regencias = new List<Regencia>();
            this.PlanoAulaRegenciaDisciplinas = new List<CLS_TurmaAulaPlanoDisciplina>();
        }
    }
}