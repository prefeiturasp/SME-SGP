/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class TUR_TurmaDocenteProjetoDAO : AbstractTUR_TurmaDocenteProjetoDAO
    {
        #region Métodos

        /// <summary>
        /// Seleciona os registros por turma, docente, colaborador, cargo e colaborador-cargo 
        /// que não estejam logicamente excluídos.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="doc_id">Id do docente</param>
        /// <param name="col_id">Id do colaborador</param>
        /// <param name="crg_id">Id do cargo</param>
        /// <param name="coc_id">Id do colaborador-cargo</param>
        /// <returns>Registros encontrados</returns>
        public DataTable SelectBy_TurmaDocenteColaboradorCargo
        (
            long tur_id,
            long doc_id,
            long col_id,
            int crg_id,
            int coc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocenteProjeto_SelectBy_TurmaDocenteColaboradorCargo", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.Size = 16;
            Param.ParameterName = "@tur_id";
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.Size = 16;
            Param.ParameterName = "@doc_id";
            Param.Value = doc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.Size = 16;
            Param.ParameterName = "@col_id";
            Param.Value = col_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.Size = 8;
            Param.ParameterName = "@crg_id";
            Param.Value = crg_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.Size = 8;
            Param.ParameterName = "@coc_id";
            Param.Value = coc_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        #endregion

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaDocenteProjeto entity)
        {
            entity.tdp_dataCriacao = entity.tdp_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaDocenteProjeto entity)
        {
            entity.tdp_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@tdp_dataCriacao");
        }

        protected override bool Alterar(TUR_TurmaDocenteProjeto entity)
        {
            __STP_UPDATE = "NEW_TUR_TurmaDocenteProjeto_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TurmaDocenteProjeto entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdp_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tdp_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(TUR_TurmaDocenteProjeto entity)
        {
            __STP_DELETE = "NEW_TUR_TurmaDocenteProjeto_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion
    }
}