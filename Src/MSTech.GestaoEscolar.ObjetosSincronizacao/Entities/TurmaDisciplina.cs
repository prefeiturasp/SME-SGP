using System;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TurmaDisciplina
    {
        public Int64 Tud_id { get; set; }
        public string Tud_nome { get; set; }
        public int Tud_situacao { get; set; }
        public int Tud_disciplinaEspecial { get; set; }
        public string Tds_nomeDisciplinaEspecial { get; set; }
        public int Tud_tipo { get; set; }
        public string Tud_tipoDescricao
        {
            get
            {
                string descricao = String.Empty;

                switch (Tud_tipo)
                {
                    case 1:
                        descricao = "Obrigatória";
                        break;
                    case 3:
                        descricao = "Optativa";
                        break;
                    case 4:
                        descricao = "Eletiva";
                        break;
                    case 5:
                        descricao = "[MSG_DISCIPLINA] principal";
                        break;
                    case 6:
                        descricao = "Docente da turma e docente específico – obrigatória";
                        break;
                    case 7:
                        descricao = "Docente da turma e docente específico – eletiva";
                        break;
                    case 8:
                        descricao = "Depende da disponibilidade de professor – obrigatória";
                        break;
                    case 9:
                        descricao = "Depende da disponibilidade de professor – eletiva";
                        break;
                    case 10:
                        descricao = "[MSG_DISCIPLINA] Eletivo(a) do Aluno";
                        break;
                    default:
                        break;
                }

                return descricao;
            }
        }
        public bool Tud_global { get; set; }

        public int Tud_cargaHorariaSemanal { get; set; }
        public string Tdt_vigenciaInicio { get; set; }
        public string Tdt_vigenciaFim { get; set; }
        public int Tdt_posicao { get; set; }
        public int Tdt_id { get; set; }
        public int Tdt_situacao { get; set; }
        public string Tdt_dataAlteracao { get; set; }
        public int Tds_id { get; set; }
        public int Tds_ordem { get; set; }
        public Int16 crg_tipo { get; set; }
        public bool Tud_naoLancarFrequencia { get; set; }
        public bool Tud_naoLancarNota { get; set; }        
        public bool Tud_naoLancarPlanejamento { get; set; }

        public List<TUR_TurmaDisciplinaRelacionadaDTO> TurmaDisciplinaRelacionada { get; set; }
        public List<TUR_TurmaDisciplinaTerritorioDTO> TurmaDisciplinaTerritorio { get; set; }

        public TurmaDisciplina()
        {

        }
    }
}