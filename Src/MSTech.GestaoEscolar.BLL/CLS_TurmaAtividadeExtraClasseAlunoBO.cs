/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System;
    using System.Reflection;
    using ObjetosSincronizacao.Util;
    using System.Data;
    using System.Linq;
    public class TipoTabela_TurmaAtividadeExtraClasseALuno : TipoTabela
    {
        private string dataVazia = new DateTime().ToString();

        [Order]
        public long tud_id { get; set; }
        [Order]
        public int tae_id { get; set; }
        [Order]
        public long alu_id { get; set; }
        [Order]
        public int mtu_id { get; set; }
        [Order]
        public int mtd_id { get; set; }
        [Order]
        [DBNullValue(typeof(string))]
        public string aea_avaliacao { get; set; }
        [Order]
        [DBNullValue(typeof(string))]
        public string aea_relatorio { get; set; }
        [Order]
        public bool aea_entregue { get; set; }
        [Order]
        public byte aea_situacao { get; set; }
        [Order]
        [DBNullValue(typeof(DateTime))]
        public DateTime aea_dataAlteracao { get; set; }

        public TipoTabela_TurmaAtividadeExtraClasseALuno(CLS_TurmaAtividadeExtraClasseAluno entity) : base(entity)
        {

        }
    }

    /// <summary>
    /// Description: CLS_TurmaAtividadeExtraClasseAluno Business Object. 
    /// </summary>
    public class CLS_TurmaAtividadeExtraClasseAlunoBO : BusinessBase<CLS_TurmaAtividadeExtraClasseAlunoDAO, CLS_TurmaAtividadeExtraClasseAluno>
	{
        #region Métodos de inclusão/alteração

        /// <summary>
        /// Salva as notas das atividades extraclasse
        /// </summary>
        /// <param name="lstTurmaAtividadeExtraClasseAluno"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(List<CLS_TurmaAtividadeExtraClasseAluno> lstTurmaAtividadeExtraClasseAluno)
        {
            DataTable dt = lstTurmaAtividadeExtraClasseAluno.Select(p => new TipoTabela_TurmaAtividadeExtraClasseALuno(p).ToDataRow()).CopyToDataTable();
            return new CLS_TurmaAtividadeExtraClasseAlunoDAO().SalvarEmLote(dt);
        }

        #endregion Métodos de inclusão/alteração
    }
}