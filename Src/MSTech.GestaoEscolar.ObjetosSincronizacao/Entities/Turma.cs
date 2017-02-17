using System;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Turma
    {
        public Int64 Tur_id { get; set; }
        public string Tur_codigo { get; set; }
        public string Tur_descricao { get; set; }
        public Int64 Tud_id { get; set; }
        public int Cal_ano { get; set; }
        public string Crp_descricao { get; set; }
        public string Crp_ciclo { get; set; }
        public int tci_id { get; set; }
        public int Crp_ordem { get; set; }
        public byte Crp_controleTempo { get; set; }
        public int Tur_situacao { get; set; }
        public bool Fav_planejamentoAulasNotasConjunto { get; set; }
        public byte Fav_tipoApuracaoFrequencia { get; set; }
        public int esc_id { get; set; }
        public string esc_nome { get; set; }
        public string trn_descricao { get; set; }
        public byte Fav_tipoLancamentoFrequencia { get; set; }
        public int esa_id { get; set; }
        public int Cal_id { get; set; }
        public Guid Ent_id { get; set; }
        public Guid Uad_id { get; set; }
        public int crr_id { get; set; }
        public int crp_id { get; set; }
        public int trn_id { get; set; }
        public int cur_id { get; set; }
        public string cur_nome { get; set; }
        public Int32 tcr_prioridade { get; set; }
        public int tur_tipo { get; set; }
        public byte Ttn_tipo { get; set; }  

        public NivelEnsino NivelEnsino { get; set; }
        public List<TurmaDisciplina> Disciplinas { get; set; }
        public List<TipoDisciplinaDeficiencia> TipoDisciplinaDeficiencia { get; set; }
        public List<TurmaCurriculo> TurmaCurriculo { get; set; }

        public string Fav_tipoApuracaoFrequenciaDescricao
        {
            get
            {
                string descricao = String.Empty;

                switch (Fav_tipoApuracaoFrequencia)
                {
                    case 1:
                        descricao = "Tempos de aula";
                        break;
                    case 2:
                        descricao = "Horas";
                        break;
                }

                return descricao;
            }
        }

        public string Fav_tipoLancamentoFrequenciaDescricao {
            get
            {
                string descricao = String.Empty;
                switch (Fav_tipoLancamentoFrequencia)
                {
                    case 1:
                        descricao = "Aulas planejadas";
                        break;
                    case 2:
                        descricao = "Período";
                        break;
                    case 3:
                        descricao = "Mensal";
                        break;
                    case 4:
                        descricao = "Aulas planejadas e mensal";
                        break;
                }

                return descricao;
            }
        }

        public Turma()
        {
            this.NivelEnsino = new NivelEnsino();
            this.Disciplinas = new List<TurmaDisciplina>();
            this.TurmaCurriculo = new List<TurmaCurriculo>();
        }
    }
}