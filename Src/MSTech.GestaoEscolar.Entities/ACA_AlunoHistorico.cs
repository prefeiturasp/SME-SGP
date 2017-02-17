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
    ///
    /// </summary>
    [Serializable()]
    public class ACA_AlunoHistorico : AbstractACA_AlunoHistorico
    {
        /// <summary>
        /// ID do historico do aluno.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int alh_id { get; set; }

        /// <summary>
        /// Descrição do histórico do resultado do aluno.
        /// </summary>
        [MSValidRange(30, "Resultado pode conter até 30 caracteres.")]
        public override string alh_resultadoDescricao { get; set; }

        /// <summary>
        /// Avaliação do histórico do aluno.
        /// </summary>
        [MSValidRange(100, "Avaliacao pode conter até 100 caracteres.")]
        public override string alh_avaliacao { get; set; }

        /// <summary>
        /// Frequência do histórico do aluno.
        /// </summary>
        [MSValidRange(100, "Frequência pode conter até 100 caracteres.")]
        public override string alh_frequencia { get; set; }

        /// <summary>
        /// Descrição do próximo ano letivo do histórico do aluno.
        /// </summary>
        [MSValidRange(200, "Descrição do próximo período pode conter até 200 caracteres.")]
        public override string alh_descricaoProximoPeriodo { get; set; }

        /// <summary>
        /// Situação do registro (1-Ativo, 3-Excluído).
        /// </summary>
        [MSDefaultValue(1)]
        public override short alh_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime alh_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime alh_dataAlteracao { get; set; }

        public static DataTable TipoTabela_AlunoHistorico()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("alu_id", typeof(Int64));
            dt.Columns.Add("alh_id", typeof(Int32));
            dt.Columns.Add("eco_id", typeof(Int64));
            dt.Columns.Add("mtu_id", typeof(Int32));
            dt.Columns.Add("alh_anoLetivo", typeof(Int32));
            dt.Columns.Add("alh_resultado", typeof(Byte));
            dt.Columns.Add("alh_resultadoDescricao", typeof(String));
            dt.Columns.Add("alh_avaliacao", typeof(String));
            dt.Columns.Add("alh_frequencia", typeof(String));
            dt.Columns.Add("alh_situacao", typeof(Byte));
            //dt.Columns.Add("alh_dataCriacao", typeof(DateTime));
            //dt.Columns.Add("alh_dataAlteracao", typeof(DateTime));
            dt.Columns.Add("cur_id", typeof(Int32));
            dt.Columns.Add("crr_id", typeof(Int32));
            dt.Columns.Add("crp_id", typeof(Int32));
            dt.Columns.Add("esc_id", typeof(Int32));
            dt.Columns.Add("uni_id", typeof(Int32));
            dt.Columns.Add("alh_qtdeFaltas", typeof(Int32));
            dt.Columns.Add("alh_tipoControleNotas", typeof(Byte));
            dt.Columns.Add("aho_id", typeof(Int32));
            dt.Columns.Add("alh_cargaHorariaBaseNacional", typeof(Int32));
            dt.Columns.Add("alh_cargaHorariaBaseDiversificada", typeof(Int32));
            dt.Columns.Add("alh_descricaoProximoPeriodo", typeof(String));
            dt.Columns.Add("alh_idTemp", typeof(Int32));
            return dt;
        }
    }
}