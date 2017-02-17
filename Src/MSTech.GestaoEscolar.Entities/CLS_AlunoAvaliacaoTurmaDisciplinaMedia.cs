/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using System.ComponentModel;
    using System.Data;

    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CLS_AlunoAvaliacaoTurmaDisciplinaMedia : Abstract_CLS_AlunoAvaliacaoTurmaDisciplinaMedia
	{
        /// <summary>
        /// Propriedade tud_id.
        /// </summary>
        [MSNotNullOrEmpty("Disciplina é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long tud_id { get; set; }

        /// <summary>
        /// Propriedade alu_id.
        /// </summary>
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override long alu_id { get; set; }

        /// <summary>
        /// Propriedade mtu_id.
        /// </summary>
        [MSNotNullOrEmpty("Matrícula turma é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int mtu_id { get; set; }

        /// <summary>
        /// Propriedade mtd_id.
        /// </summary>
        [MSNotNullOrEmpty("Natrícula turma disciplina é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int mtd_id { get; set; }

        /// <summary>
        /// Propriedade tpc_id.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de período do calendário é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tpc_id { get; set; }

        /// <summary>
        /// Propriedade atm_media.
        /// </summary>
        public override string atm_media { get; set; }

        /// <summary>
        /// Propriedade atm_situacao.
        /// </summary>
        [MSDefaultValue(1)]
        public override short atm_situacao { get; set; }

        /// <summary>
        /// Propriedade atm_dataCriacao.
        /// </summary>
        public override DateTime atm_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade atm_dataAlteracao.
        /// </summary>
        public override DateTime atm_dataAlteracao { get; set; }

        /// <summary>
        /// Retorna o DataTable no formato do TipoTabela_AlunoAvaliacaoTurmaDisciplinaMedia.
        /// </summary>
        /// <returns>DataTable no formato do TipoTabela_AlunoAvaliacaoTurmaDisciplinaMedia.</returns>
        public static DataTable TipoTabela_AlunoAvaliacaoTurmaDisciplinaMedia()
        {
            DataTable dtAlunoAvaliacaoTurmaDisciplinaMedia = new DataTable();
            dtAlunoAvaliacaoTurmaDisciplinaMedia.Columns.Add("tud_id", typeof(Int64));
            dtAlunoAvaliacaoTurmaDisciplinaMedia.Columns.Add("alu_id", typeof(Int64));
            dtAlunoAvaliacaoTurmaDisciplinaMedia.Columns.Add("mtu_id", typeof(Int32));
            dtAlunoAvaliacaoTurmaDisciplinaMedia.Columns.Add("mtd_id", typeof(Int32));
            dtAlunoAvaliacaoTurmaDisciplinaMedia.Columns.Add("tpc_id", typeof(Int32));
            dtAlunoAvaliacaoTurmaDisciplinaMedia.Columns.Add("atm_media", typeof(string));
            dtAlunoAvaliacaoTurmaDisciplinaMedia.Columns.Add("atm_situacao", typeof(Int16));

            return dtAlunoAvaliacaoTurmaDisciplinaMedia;
        }

        public int tpc_ordem { get; set; }

	}
}