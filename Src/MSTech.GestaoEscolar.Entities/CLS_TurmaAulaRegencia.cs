/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using System.Data;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CLS_TurmaAulaRegencia : AbstractCLS_TurmaAulaRegencia
	{
        public long idAula { get; set; }

        public static DataTable TipoTabela_TurmaAulaRegencia()
        {
            DataTable dtTurmaAulaRegencia = new DataTable();
            dtTurmaAulaRegencia.Columns.Add("idAula", typeof(Int64));
            dtTurmaAulaRegencia.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaRegencia.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAulaRegencia.Columns.Add("tud_idFilho", typeof(Int64));
            dtTurmaAulaRegencia.Columns.Add("tuf_data", typeof(DateTime));
            dtTurmaAulaRegencia.Columns.Add("tuf_numeroAulas", typeof(Int32));
            dtTurmaAulaRegencia.Columns.Add("tuf_planoAula", typeof(String));
            dtTurmaAulaRegencia.Columns.Add("tuf_diarioClasse", typeof(String));
            dtTurmaAulaRegencia.Columns.Add("tuf_situacao", typeof(Byte));
            dtTurmaAulaRegencia.Columns.Add("tuf_conteudo", typeof(String));
            dtTurmaAulaRegencia.Columns.Add("tuf_atividadeCasa", typeof(String));
            dtTurmaAulaRegencia.Columns.Add("pro_id", typeof(Guid));
            dtTurmaAulaRegencia.Columns.Add("tuf_sintese", typeof(String));
            dtTurmaAulaRegencia.Columns.Add("tuf_dataAlteracao", typeof(DateTime));
            dtTurmaAulaRegencia.Columns.Add("tuf_checadoAtividadeCasa", typeof(Boolean));
            return dtTurmaAulaRegencia;
        }

        /// <summary>
        /// utilizado para guardar o id do usuário que realizou alguma modificação/inclusão nos dados
        /// </summary>
        public virtual Guid usu_idDocenteAlteracao { get; set; }

        /// <summary>
        /// Propriedade tuf_checadoAtividadeCasa.
        /// </summary>
        [MSDefaultValue(false)]
        public override bool tuf_checadoAtividadeCasa { get; set; }
    }
}