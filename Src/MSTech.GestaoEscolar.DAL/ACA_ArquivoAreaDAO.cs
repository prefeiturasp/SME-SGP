/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using System.Collections.Generic;
    using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL.Abstracts;
    using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_ArquivoAreaDAO : Abstract_ACA_ArquivoAreaDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Retorna um datatable contendo todos os documentos cadastrados para a area
        /// </summary>
        /// <param name="tad_id">ID da Area</param>
        /// <returns>DataTable com os dados</returns>
        public List<ACA_ArquivoArea> Select_By_tad_id
        (
            int tad_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ArquivoArea_Select_By_tad_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tad_id";
                Param.Size = 4;
                Param.Value = tad_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new ACA_ArquivoArea())).ToList() :
                    new List<ACA_ArquivoArea>();
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo os documentos cadastrados para uma area filtrando DRE e Escola
        /// </summary>
        /// <param name="tad_id">ID da area</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uad_idSuperior">ID da DRE</param>
        /// <returns>DataTable com os dados</returns>
        public DataTable SelectBy_Id_Dre_Escola
        (
            int tad_id,
            int esc_id,
            int tne_id,
            bool incluirPpp,
            bool apenasPpp,
            Guid uad_idSuperior
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ArquivoArea_Select_By_tad_id_Dre_Escola", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tad_id";
                Param.Size = 4;
                if (tad_id > 0)
                    Param.Value = tad_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                if (tne_id > 0)
                    Param.Value = tne_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@incluirPPP";
                Param.Size = 1;
                Param.Value = incluirPpp;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@apenasPPP";
                Param.Size = 1;
                Param.Value = apenasPpp;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna ultimo aar_id do documentos da area
        /// </summary>
        /// <param name="tad_id">ID da area</param>
        /// <returns>ID do Documento da area</returns>
        public int GetSelectUltimo_aar_idBy_tad_id
        (
            int tad_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ArquivoArea_SelectUltimo_aar_idBy_tad_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tad_id";
                Param.Size = 1;
                if (tad_id > 0)
                    Param.Value = tad_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return Convert.ToInt32(qs.Return.Rows[0][0]);

                return -1;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de consulta

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ArquivoArea entity)
        {
            entity.aar_dataCriacao = entity.aar_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);

            if (qs.Parameters["@uad_idSuperior"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@uad_idSuperior"].Value = DBNull.Value;
            }
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ArquivoArea entity)
        {
            entity.aar_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@aar_dataCriacao");
            if (qs.Parameters["@uad_idSuperior"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@uad_idSuperior"].Value = DBNull.Value;
            }
        }

        protected override bool Alterar(ACA_ArquivoArea entity)
        {
            __STP_UPDATE = "NEW_ACA_ArquivoArea_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ArquivoArea entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@aar_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@aar_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(ACA_ArquivoArea entity)
        {
            __STP_DELETE = "NEW_ACA_ArquivoArea_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_ArquivoArea entity)
        {
            if (entity != null & qs != null)
            {
                entity.aar_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.aar_id > 0;
            }

            return false;
        }

        #endregion Métodos sobrescritos
	}
}