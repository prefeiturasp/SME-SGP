namespace MSTech.GestaoEscolar.DAL
{
	using MSTech.Data.Common;
	using MSTech.GestaoEscolar.DAL.Abstracts;
	using MSTech.GestaoEscolar.Entities;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_EventoLimiteDAO : Abstract_ACA_EventoLimiteDAO
	{
		#region Métodos sobrescritos

		protected override void ParamInserir(Data.Common.QuerySelectStoredProcedure qs, Entities.ACA_EventoLimite entity)
		{
			base.ParamInserir(qs, entity);

			qs.Parameters["@evl_dataCriacao"].Value = DateTime.Now;
			qs.Parameters["@evl_dataAlteracao"].Value = DateTime.Now;
		}

		protected override void ParamAlterar(Data.Common.QueryStoredProcedure qs, Entities.ACA_EventoLimite entity)
		{
			base.ParamAlterar(qs, entity);

			qs.Parameters.RemoveAt("@evl_dataCriacao");
			qs.Parameters["@evl_dataAlteracao"].Value = DateTime.Now;
		}

		protected override bool Alterar(Entities.ACA_EventoLimite entity)
		{
			__STP_UPDATE = "NEW_ACA_EventoLimite_Update";
			return base.Alterar(entity);
		}

        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_EventoLimite entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = entity.cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tev_id";
            Param.Size = 4;
            Param.Value = entity.tev_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@evl_id";
            Param.Size = 4;
            Param.Value = entity.evl_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@evl_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@evl_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

		public override bool Delete(Entities.ACA_EventoLimite entity)
		{
			__STP_DELETE = "NEW_ACA_EventoLimite_Update_Situacao";
			return base.Delete(entity);
		}

		protected override bool ReceberAutoIncremento(Data.Common.QuerySelectStoredProcedure qs, Entities.ACA_EventoLimite entity)
		{
			entity.evl_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.evl_id > 0);
		}

		#endregion

		public List<ACA_EventoLimite> SelectByCalendario(int cal_id)
		{
			var qs = new QuerySelectStoredProcedure("NEW_ACA_EventoLimite_SelectByCalendario", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cal_id";
				Param.Value = cal_id;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				var ret = new List<ACA_EventoLimite>();

				foreach(DataRow row in qs.Return.Rows)
				{
					ret.Add(DataRowToEntity(row, new ACA_EventoLimite(), true));
				}

				return ret;
			}
			catch (Exception)
			{
				throw;
			}
		}

        public List<ACA_EventoLimite> SelectByTipoEvento(int tev_id)
        {
            var qs = new QuerySelectStoredProcedure("NEW_ACA_EventoLimite_SelectByTipoEvento", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_id";
                Param.Value = tev_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                var ret = new List<ACA_EventoLimite>();

                foreach (DataRow row in qs.Return.Rows)
                {
                    ret.Add(DataRowToEntity(row, new ACA_EventoLimite(), true));
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ACA_EventoLimite LoadByTipoEventoAndDre(int tev_id, Guid dre_id)
        {
            var qs = new QuerySelectStoredProcedure("NEW_ACA_EventoLimite_LoadByTipoEvento_and_Dre", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_id";
                Param.Value = tev_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_id";
                Param.Size = 16;
                Param.Value = dre_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if(qs.Return.Rows.Count > 0)
                    return DataRowToEntity(qs.Return.Rows[0], new ACA_EventoLimite(), true);

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}