/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_TurmaAulaGeradaDAO : Abstract_CLS_TurmaAulaGeradaDAO
	{
	    /// <summary>
	    /// Retorna todas as turmas com suas respectivas disciplinas.
	    /// </summary>
	    public DataTable GerarAula(long doc_id, bool ordenaCodigoEscola, int cal_id, int tpc_id)
	    {

	        QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaGerada_TurmaDisciplina", _Banco);

	        #region PARÂMETROS

	        Param = qs.NewParameter();
	        Param.DbType = DbType.Int64;
	        Param.ParameterName = "@doc_id";
	        Param.Size = 8;
	        Param.Value = doc_id;
	        qs.Parameters.Add(Param);

	        Param = qs.NewParameter();
	        Param.DbType = DbType.Boolean;
	        Param.ParameterName = "@ordenaCodigoEscola";
	        Param.Size = 1;
	        Param.Value = ordenaCodigoEscola;
	        qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

	        Param = qs.NewParameter();
	        Param.DbType = DbType.Int32;
	        Param.ParameterName = "@tpc_id";
	        Param.Size = 4;
	        Param.Value = tpc_id;
	        qs.Parameters.Add(Param);

	        #endregion PARÂMETROS

	        qs.Execute();
	        return qs.Return;
	    }

	    /// <summary>
        /// Retorna as entidades das aulas geradas.
        /// </summary>
        public DataTable SelectBy_DisciplinaDocente(string tud_id, long doc_id, int tpc_id)
        {
            try
            {
                QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaGerada_SelectBy_DisicplinaDocente", _Banco);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tud_id";
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                qs.Execute();
                return qs.Return;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, Entities.CLS_TurmaAulaGerada entity)
        {
            entity.tag_dataCriacao = DateTime.Now;
            entity.tag_dataAlteracao = DateTime.Now;

            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, Entities.CLS_TurmaAulaGerada entity)
        {
            entity.tag_dataAlteracao = DateTime.Now;

            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tag_dataCriacao");
        }

        protected override bool Alterar(Entities.CLS_TurmaAulaGerada entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaAulaGerada_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, Entities.CLS_TurmaAulaGerada entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tag_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);
            
            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tag_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.tag_dataAlteracao;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(Entities.CLS_TurmaAulaGerada entity)
        {
            __STP_DELETE = "NEW_CLS_TurmaAulaGerada_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion
    }
}