/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using System.Data;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    /// <summary>
    ///
    /// </summary>
    [Serializable()]
    public class ACA_AlunoHistoricoDisciplina : AbstractACA_AlunoHistoricoDisciplina
    {
        [MSNotNullOrEmpty]
        [DataObjectField(true, false, false)]
        public override Int64 alu_id { get; set; }

        [MSNotNullOrEmpty("Histórico é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int alh_id { get; set; }

        [DataObjectField(true, false, false)]
        public override int ahd_id { get; set; }

        [MSValidRange(200, "[MSG_DISCIPLINA] pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("[MSG_DISCIPLINA] é obrigatório.")]
        public override string ahd_disciplina { get; set; }

        public override short ahd_resultado { get; set; }

        [MSValidRange(30, "Resultado pode conter até 30 caracteres.")]
        public override string ahd_resultadoDescricao { get; set; }

        [MSValidRange(100, "Avaliação pode conter até 100 caracteres.")]
        public override string ahd_avaliacao { get; set; }

        [MSValidRange(100, "Frequência pode conter até 100 caracteres.")]
        public override string ahd_frequencia { get; set; }

        [MSDefaultValue(1)]
        public override short ahd_situacao { get; set; }

        public static DataTable TipoTabela_AlunoHistoricoDisciplina()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("alu_id", typeof(Int64));
            dt.Columns.Add("alh_id", typeof(Int32));
            dt.Columns.Add("ahd_id", typeof(Int32));
            dt.Columns.Add("tds_id", typeof(Int32));
            dt.Columns.Add("ahd_disciplina", typeof(String));
            dt.Columns.Add("ahd_resultado", typeof(Byte));
            dt.Columns.Add("ahd_resultadoDescricao", typeof(String));
            dt.Columns.Add("ahd_avaliacao", typeof(String));
            dt.Columns.Add("ahd_frequencia", typeof(String));
            dt.Columns.Add("ahd_indicacaoDependencia", typeof(Boolean));
            dt.Columns.Add("ahd_situacao", typeof(Byte));
            dt.Columns.Add("ahd_qtdeFaltas", typeof(Int32));
            dt.Columns.Add("ahp_id", typeof(Int32));
            dt.Columns.Add("alh_idTemp", typeof(Int32));
            return dt;
        }
    }
}