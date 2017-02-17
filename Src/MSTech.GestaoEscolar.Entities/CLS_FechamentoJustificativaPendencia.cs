/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.Data;
		
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_FechamentoJustificativaPendencia : AbstractCLS_FechamentoJustificativaPendencia
	{
        [MSDefaultValue(1)]
        public override byte fjp_situacao { get; set; }
        public override DateTime fjp_dataCriacao { get; set; }
        public override DateTime fjp_dataAlteracao { get; set; }

        public static DataTable TipoTabela_FechamentoJustificativaPendencia()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tud_id", typeof(Int64));
            dt.Columns.Add("cal_id", typeof(Int32));
            dt.Columns.Add("tpc_id", typeof(Int32));
            dt.Columns.Add("fjp_id", typeof(Int32));
            dt.Columns.Add("fjp_justificativa", typeof(String));
            dt.Columns.Add("usu_id", typeof(Guid));
            dt.Columns.Add("usu_idAlteracao", typeof(Guid));
            dt.Columns.Add("fjp_situacao", typeof(Byte));
            return dt;
        }
	}
}