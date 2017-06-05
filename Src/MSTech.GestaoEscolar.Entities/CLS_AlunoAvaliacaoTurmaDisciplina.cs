/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;
using System.Data;

namespace MSTech.GestaoEscolar.Entities
{	
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
	public class CLS_AlunoAvaliacaoTurmaDisciplina : Abstract_CLS_AlunoAvaliacaoTurmaDisciplina
	{
        [DataObjectField(true, false, false)]
        public override int atd_id { get; set; }
        [MSNotNullOrEmpty("Formato de avaliação é obrigatório.")]
        public override int fav_id { get; set; }
        [MSNotNullOrEmpty("Avaliação é obrigatório.")]
        public override int ava_id { get; set; }
        [MSValidRange(20, "Avaliação pode conter até 20 caracteres.")]
        public override string atd_avaliacao { get; set; }
        [MSValidRange(1000, "Comentários pode conter até 1000 caracteres.")]
        public override string atd_comentarios { get; set; }
        [MSDefaultValue(1)]
        public override byte atd_situacao { get; set; }
        public override DateTime atd_dataCriacao { get; set; }
        public override DateTime atd_dataAlteracao { get; set; }
        public override Int32 atd_ausenciasCompensadas { get; set; }

        // <summary>
        /// Propriedade atd_avaliacaoPosConselho.
        /// </summary>
        [MSValidRange(20, "Nota pós-conselho pode conter até 20 caracteres.")]
        public override string atd_avaliacaoPosConselho { get; set; }

        public override decimal atd_frequenciaFinalAjustada { get; set; }

        public int tpc_id { get; set; }

        public bool possuiFrequenciaFinal { get; set; }

        public static DataTable TipoTabela_AlunoAvaliacaoTurmaDisciplina()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tud_id", typeof(Int64));
            dt.Columns.Add("alu_id", typeof(Int64));
            dt.Columns.Add("mtu_id", typeof(Int32));
            dt.Columns.Add("mtd_id", typeof(Int32));
            dt.Columns.Add("atd_id", typeof(Int32));
            dt.Columns.Add("fav_id", typeof(Int32));
            dt.Columns.Add("ava_id", typeof(Int32));
            dt.Columns.Add("atd_avaliacao", typeof(String));
            dt.Columns.Add("atd_avaliacaoPosConselho", typeof(String));
            dt.Columns.Add("atd_numeroAulas", typeof(Int32));
            dt.Columns.Add("atd_numeroFaltas", typeof(Int32));
            dt.Columns.Add("atd_ausenciasCompensadas", typeof(Int32));
            dt.Columns.Add("atd_registroexterno", typeof(Boolean));
            dt.Columns.Add("atd_frequencia", typeof(Decimal));
            dt.Columns.Add("atd_comentarios", typeof(String));
            dt.Columns.Add("atd_relatorio", typeof(String));
            dt.Columns.Add("atd_semProfessor", typeof(Boolean));
            dt.Columns.Add("atd_situacao", typeof(Byte));
            dt.Columns.Add("arq_idRelatorio", typeof(Int64));
            dt.Columns.Add("atd_justificativaPosConselho", typeof(String));
            dt.Columns.Add("atd_frequenciaFinalAjustada", typeof(Decimal));
            dt.Columns.Add("atd_numeroFaltasReposicao", typeof(Int32));
            dt.Columns.Add("atd_numeroAulasReposicao", typeof(Int32));
            dt.Columns.Add("atd_numeroAulasExterna", typeof(Int32));
            dt.Columns.Add("atd_numeroFaltasExterna", typeof(Int32));
            dt.Columns.Add("atd_numeroAtividadeExtraclasse", typeof(Int32));
            return dt;
        }
	}
}