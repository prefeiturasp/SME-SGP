/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Data.SqlClient;

namespace MSTech.GestaoEscolar.DAL
{

	/// <summary>
	/// 
	/// </summary>
	public class ACA_CurriculoDisciplinaDAO : Abstract_ACA_CurriculoDisciplinaDAO
	{
		/// <summary>
		/// Retorna as disciplinas cadastradas para o CurriculoPeriodo do curso 
		/// que sejam de qualquer tipo.
		/// </summary>
		public DataTable SelectBy_Curso_TodosTipos
		(
			int cur_id
			, int crr_id
			, int crp_id
			, long tur_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_Curso_TodosTipos", _Banco);

			#region PARAMETROS

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = crp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tur_id";
			Param.Size = 8;
			if (tur_id > 0)
				Param.Value = tur_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			#endregion

			qs.Execute();

			return qs.Return;
		}

		/// <summary>
		/// Retorna as disciplinas cadastradas para o CurriculoPeriodo do curso 
		/// que sejam do tipo informado.
		/// </summary>
		public DataTable SelectBy_Curso_Tipo
		(
			int cur_id
			, int crr_id
			, int crp_id
			, byte crd_tipo
			, long tur_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_Curso_Tipo", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				Param.Value = crr_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				Param.Value = crp_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Byte;
				Param.ParameterName = "@crd_tipo";
				Param.Size = 1;
				Param.Value = crd_tipo;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int64;
				Param.ParameterName = "@tur_id";
				Param.Size = 8;
				if (tur_id > 0)
					Param.Value = tur_id;
				else
					Param.Value = DBNull.Value;
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
		/// Retorna as disciplinas cadastradas para o CurriculoPeriodo do curso        
		/// </summary>
		public DataTable SelecionaDisciplinasParaFormacaoTurmaNormal
		(
			int cur_id
			, int crr_id
			, int crp_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelecionaDisciplinasParaFormacaoTurmaNormal", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				Param.Value = crr_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				Param.Value = crp_id;
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
		/// Retorna um datatable contendo todos as disciplinas do curriculo/curso
		/// que não foram excluídos logicamente, filtrados por 
		/// id do curso, id do curriculo
		/// </summary>
		/// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
		/// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
		/// <param name="paginado">Indica se o datatable será paginado ou não</param>
		/// <param name="currentPage">Página atual do grid</param>
		/// <param name="pageSize">Total de registros por página do grid</param>
		/// <param name="totalRecords">Total de registros retornado na busca</param>
		/// <returns>DataTable com as disciplinas do curriculo</returns>
		public DataTable SelectBy_cur_id_crr_id
		(
			int cur_id
			, int crr_id
			, bool paginado
			, int currentPage
			, int pageSize
			, out int totalRecords
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_cur_id_crr_id", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				if (cur_id > 0)
					Param.Value = cur_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				if (crr_id > 0)
					Param.Value = crr_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				#endregion

				if (paginado)
					totalRecords = qs.Execute(currentPage, pageSize);
				else
				{
					qs.Execute();
					totalRecords = qs.Return.Rows.Count;
				}

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
		/// Retorna os períodos da disciplina eletiva do aluno
		/// </summary>
		/// <param name="cur_id">Id do curso</param>
		/// <param name="crr_id">Id do curriculo</param>
		/// <param name="esc_id">ID da escola</param>
		/// <param name="uni_id">ID da unidade da escola</param>
		/// <param name="dis_id">Id da disciplina</param>
		public DataTable SelectBy_Escola_EletivasAlunos
		(
			int cur_id
			, int crr_id
			, int esc_id
			, int uni_id
			, int dis_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_Escola_EletivasAlunos", _Banco);

			#region PARAMETROS

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
			Param.Value = uni_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_id";
			Param.Size = 4;
			Param.Value = dis_id;
			qs.Parameters.Add(Param);

			#endregion

			qs.Execute();

			return qs.Return;
		}

		/// <summary>
		/// Retorna os períodos da disciplina eletiva do aluno
		/// </summary>
		/// <param name="cur_id">Id do curso</param>
		/// <param name="crr_id">Id do curriculo</param>
		/// <param name="dis_id">Id da disciplina</param>
		public DataTable SelectBy_EletivasAlunos
		(
			int cur_id
			, int crr_id
			, int dis_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_EletivasAlunos", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				Param.Value = crr_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@dis_id";
				Param.Size = 4;
				Param.Value = dis_id;
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
		/// Retorna um datatable contendo todos as disciplinas do curriculo/curso
		/// que não foram excluídos logicamente, filtrados por 
		/// id do curso, id do curriculo
		/// </summary>
		/// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
		/// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
		/// <param name="paginado">Indica se o datatable será paginado ou não</param>
		/// <param name="currentPage">Página atual do grid</param>
		/// <param name="pageSize">Total de registros por página do grid</param>
		/// <param name="totalRecords">Total de registros retornado na busca</param>
		/// <returns>DataTable com as disciplinas do curriculo</returns>
		public DataTable Select_Disciplinas
		(
			int cur_id
			, int crr_id
			, bool paginado
			, int currentPage
			, int pageSize
			, out int totalRecords
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_Select_Disciplinas", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				if (cur_id > 0)
					Param.Value = cur_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				if (crr_id > 0)
					Param.Value = crr_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				#endregion

				if (paginado)
					totalRecords = qs.Execute(currentPage, pageSize);
				else
				{
					qs.Execute();
					totalRecords = qs.Return.Rows.Count;
				}

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
		/// Retorna um datatable contendo todos as disciplinas do curso
		/// que não foram excluídos logicamente, filtrados por 
		/// id do curso
		/// </summary>
		/// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
		/// <param name="paginado">Indica se o datatable será paginado ou não</param>
		/// <param name="currentPage">Página atual do grid</param>
		/// <param name="pageSize">Total de registros por página do grid</param>
		/// <param name="totalRecords">Total de registros retornado na busca</param>
		/// <returns>DataTable com as disciplinas do curso</returns>
		public DataTable SelectBy_cur_id
		(
			int cur_id
			, bool paginado
			, int currentPage
			, int pageSize
			, out int totalRecords
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_cur_id", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				if (cur_id > 0)
					Param.Value = cur_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				#endregion

				if (paginado)
					totalRecords = qs.Execute(currentPage, pageSize);
				else
				{
					qs.Execute();
					totalRecords = qs.Return.Rows.Count;
				}

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

		public DataTable SelectBy_dis_id
		(
			int dis_id
			, bool somenteAtivos = true
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_dis_id", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@dis_id";
				Param.Size = 4;
				if (dis_id > 0)
					Param.Value = dis_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Byte;
				Param.ParameterName = "@crd_situacao";
				if (somenteAtivos)
					Param.Value = 1;
				else
					Param.Value = DBNull.Value;
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
		/// 
		/// </summary>
		/// <param name="cur_id"></param>
		/// <param name="crr_id"></param>
		/// <param name="crp_id"></param>
		/// <param name="paginado"></param>
		/// <param name="currentPage"></param>
		/// <param name="pageSize"></param>
		/// <param name="totalRecords"></param>
		/// <returns></returns>
		public DataTable SelectBy_cur_id_crr_id_crp_id
		(
			int cur_id
			, int crr_id
			, int crp_id
			, bool paginado
			, int currentPage
			, int pageSize
			, out int totalRecords
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_cur_id_crr_id_crp_id", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				if (cur_id > 0)
					Param.Value = cur_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				if (crr_id > 0)
					Param.Value = crr_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				if (crp_id > 0)
					Param.Value = crp_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				#endregion

				if (paginado)
					totalRecords = qs.Execute(currentPage, pageSize);
				else
				{
					qs.Execute();
					totalRecords = qs.Return.Rows.Count;
				}

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
        /// 
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cal_id"></param>
        /// <param name="cap_id"></param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public DataTable SelectBy_cur_id_crr_id_crp_id_esc_id_uni_id
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cap_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_cur_id_crr_id_crp_id_esc_id_uni_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (crr_id > 0)
                    Param.Value = crr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
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
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
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
                Param.ParameterName = "@cap_id";
                Param.Size = 4;
                if (cap_id > 0)
                    Param.Value = cap_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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
		/// Carrega todos os tipos de disciplinas de acordo com os filtros informados
		/// exceto as disciplinas do tipo Eletiva do aluno e as disciplinas do tipo informado
		/// </summary>
		/// <param name="cur_id"></param>
		/// <param name="crr_id"></param>
		/// <param name="crp_id"></param>
		/// <param name="crd_tipo">Tipo de disciplina que NÃO será carregado</param>
		/// <returns>DataTable com os dados</returns>
		public DataTable SelectBy_CursoCurriculoPeriodoTipoDisciplina
		(
			int cur_id
			, int crr_id
			, int crp_id
			, byte crd_tipo
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_CursoCurriculoPeriodoTipoDisciplina", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				if (cur_id > 0)
					Param.Value = cur_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				if (crr_id > 0)
					Param.Value = crr_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				if (crp_id > 0)
					Param.Value = crp_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Byte;
				Param.ParameterName = "@crd_tipo";
				Param.Size = 1;
				Param.Value = crd_tipo;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return qs.Return;
			}
			finally
			{
				qs.Parameters.Clear();
			}
		}

		/// <summary>
		/// Verifica se já existe a disciplina cadastrada para o curriculo/curso/periodo
		/// e excluido logicamente
		/// filtrados por cur_id, crr_id, crp_id, dis_id
		/// </summary>
		/// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
		/// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
		/// <param name="crp_id">Id da tabela ACA_CurriculoPeriodo do bd</param>
		/// <param name="dis_id">Id da tabela ACA_Disciplina do bd</param>
		/// <returns>true ou false</returns>
		public bool SelectBy_Chaves_excluido
		(
			int cur_id
			, int crr_id
			, int crp_id
			, int dis_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_Chaves_excluido", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				if (cur_id > 0)
					Param.Value = cur_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				if (crr_id > 0)
					Param.Value = crr_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				if (crp_id > 0)
					Param.Value = crp_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@dis_id";
				Param.Size = 4;
				if (dis_id > 0)
					Param.Value = dis_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				if (qs.Return.Rows.Count > 0)
					return true;
				else
					return false;
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
		/// Verifica se existe disciplina principal e disciplinas obrigatórias ao mesmo tempo
		/// </summary>
		/// <param name="cur_id">ID do curso</param>
		/// <param name="crr_id">ID do currículo</param>
		/// <param name="crp_id">ID do período do currículo</param>        
		public bool SelectBy_VerificaDisciplinaPrincipalObrigatoria
		(
			int cur_id
			, int crr_id
			, int crp_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_VerificaDisciplinaPrincipalObrigatoria", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				Param.Value = crr_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				Param.Value = crp_id;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return (qs.Return.Rows.Count > 0);
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
		/// Verifica se existe mais de uma disciplina principal no mesmo periodo
		/// </summary>
		/// <param name="cur_id">ID do curso</param>
		/// <param name="crr_id">ID do currículo</param>
		/// <param name="crp_id">ID do período do currículo</param>        
		public bool SelectBy_VerificaDisciplinaPrincipal
		(
			int cur_id
			, int crr_id
			, int crp_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_VerificaDisciplinaPrincipal", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				Param.Value = crr_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				Param.Value = crp_id;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return (qs.Return.Rows.Count > 0);
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
		/// Verifica a soma das carga horária semanal das disciplinas obrigatorias e eletivas
		/// </summary>
		/// <param name="cur_id">ID do curso</param>
		/// <param name="crr_id">ID do currículo</param>
		/// <param name="crp_id">ID do período do currículo</param>        
		public int SelectBy_VerificaCargaHorariaSemanal
		(
			int cur_id
			, int crr_id
			, int crp_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_VerificaCargaHorariaSemanal", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				Param.Value = crr_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				Param.Value = crp_id;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return Convert.ToInt32(qs.Return.Rows[0][0]);
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
		/// Verifica se o curriculo disciplina está sendo utilizado        
		/// </summary>
		/// <param name="cur_id">Id do curso</param>
		/// <param name="crr_id">Id do curriculo do curso</param>
		/// <param name="crp_id">Id do periodo do curriculo</param>        
		/// <param name="dis_id">Id da disciplina</param>
		public bool SelectBy_VerificaCurriculoDisciplina
		(
			int cur_id
			, int crr_id
			, int crp_id
			, int dis_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_VerificaCurriculoDisciplina", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				Param.Value = crr_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				Param.Value = crp_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@dis_id";
				Param.Size = 4;
				Param.Value = dis_id;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return qs.Return.Rows.Count > 0;
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
		/// Retorna as disciplinas cadastradas para o curso, que sejam 
		/// do tipo informado e que possuam uma disciplina equivalente no curso da nova matriz curricular.
		/// </summary>
		/// <param name="cur_idOrigem">ID do curso de origem.</param>
		/// <param name="crr_idOrigem">ID do currículo de origem.</param>
		/// <param name="crp_idOrigem">ID do grupamento de origem.</param>
		/// <param name="crd_tipo">Tipo de currículo disciplina.</param>
		/// <param name="tur_id">ID da turma.</param>
		/// <param name="cur_idDestino">ID do curso da nova matriz curricular.</param>
		/// <param name="crr_idDestino">ID do currículo da nova matriz curricular.</param>
		/// <returns></returns>
		public DataTable SelecionaCursosPorNovaMatrizCurricularTipo
		(
			int cur_idOrigem,
			int crr_idOrigem,
			int crp_idOrigem,
			byte crd_tipo,
			long tur_id,
			int cur_idDestino,
			int crr_idDestino
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_NovaMatrizCurricular_Tipo", _Banco);

			try
			{
				#region Parâmetros

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_idOrigem";
				Param.Size = 4;
				Param.Value = cur_idOrigem;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_idOrigem";
				Param.Size = 4;
				Param.Value = crr_idOrigem;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_idOrigem";
				Param.Size = 4;
				Param.Value = crp_idOrigem;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Byte;
				Param.ParameterName = "@crd_tipo";
				Param.Size = 1;
				Param.Value = crd_tipo;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int64;
				Param.ParameterName = "@tur_id";
				Param.Size = 8;
				Param.Value = tur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_idDestino";
				Param.Size = 4;
				Param.Value = cur_idDestino;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_idDestino";
				Param.Size = 4;
				Param.Value = crr_idDestino;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return qs.Return;
			}
			finally
			{
				qs.Parameters.Clear();
			}
		}

        /// <summary>
        /// Carrega as disciplinas por curso e ciclo
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="tci_id"></param>
        /// <param name="esc_id">Utilizado para trazer apenas as Experiências oferecias pela escola</param>
        /// <returns>Retorna disciplinas por curso e ciclo</returns>
        public DataTable SelectBy_CursoTipoCiclo(int cur_id, int tci_id, int esc_id)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelectBy_Curso_TipoCiclo", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@tci_id";
				Param.Size = 4;
				Param.Value = tci_id;
				qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

				return qs.Return;
			}
			finally
			{
				qs.Parameters.Clear();
			}
		}

        /// <summary>
        /// Seleciona as disciplinas das grades curriculares
        /// </summary>
        /// <param name="dtTipoTabelaCurriculoPeriodo"></param>
        /// <returns></returns>
        public DataTable SelecionaPorGradesCurriculares(DataTable dtTipoTabelaCurriculoPeriodo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_SelecionaPorGradesCurriculares", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@TipoTabela_CurriculoPeriodo";
                sqlParam.TypeName = "TipoTabela_CurriculoPeriodo";
                sqlParam.Value = dtTipoTabelaCurriculoPeriodo;
                qs.Parameters.Add(sqlParam);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

		/// <summary>
		/// Realiza a exclusão lógica do período da disciplina eletiva 
		/// </summary>
		/// <param name="cur_id">Id do curso</param>
		/// <param name="crr_id">Id do curriculo</param>
		/// <param name="crp_id">Id do período do currículo</param>
		/// <param name="dis_id">Id da disciplina</param>
		public bool Update_Situacao_By_EletivasAlunos
		(
			int cur_id
			, int crr_id
			, int crp_id
			, int dis_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_Update_Situacao_EletivasAlunos", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				Param.Value = cur_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				Param.Value = crr_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crp_id";
				Param.Size = 4;
				Param.Value = crp_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@dis_id";
				Param.Size = 4;
				Param.Value = dis_id;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crd_situacao";
				Param.Size = 1;
				Param.Value = 3;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.DateTime;
				Param.ParameterName = "@crd_dataAlteracao";
				Param.Size = 8;
				Param.Value = DateTime.Now;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return qs.Return.Rows.Count > 0;
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
		/// Realiza a exclusão lógica de um tipo de disciplina para todos os periodos do curriculo
		/// </summary>
		public bool Update_SituacaoBy_tds_id
		(
			int cur_id
			, int crr_id
			, int tds_id
			, string tds_nome
			, string dis_nome
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoDisciplina_Update_SituacaoBy_tds_id", _Banco);
			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@cur_id";
				Param.Size = 4;
				if (cur_id > 0)
					Param.Value = cur_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crr_id";
				Param.Size = 4;
				if (crr_id > 0)
					Param.Value = crr_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@tds_id";
				Param.Size = 4;
				if (tds_id > 0)
					Param.Value = tds_id;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.AnsiString;
				Param.ParameterName = "@tds_nome";
				Param.Size = 100;
				if (!string.IsNullOrEmpty(tds_nome))
					Param.Value = tds_nome;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.AnsiString;
				Param.ParameterName = "@dis_nome";
				Param.Size = 200;
				if (!string.IsNullOrEmpty(dis_nome))
					Param.Value = dis_nome;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crd_situacao";
				Param.Size = 1;
				Param.Value = 3;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.DateTime;
				Param.ParameterName = "@crd_dataAlteracao";
				Param.Size = 8;
				Param.Value = DateTime.Now;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return qs.Return.Rows.Count > 0;
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

		public override bool Carregar(ACA_CurriculoDisciplina entity)
		{
			__STP_LOAD = "NEW_ACA_CurriculoDisciplina_LOAD";
			return base.Carregar(entity);
		}

		/// <summary>
		/// Parâmetros para efetuar a inclusão preservando a data de criação
		/// </summary>
		/// <param name="qs"></param>
		/// <param name="entity"></param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CurriculoDisciplina entity)
		{
			base.ParamInserir(qs, entity);

			qs.Parameters["@crd_dataCriacao"].Value = DateTime.Now;
			qs.Parameters["@crd_dataAlteracao"].Value = DateTime.Now;
		}

		/// <summary>
		/// Parâmetros para efetuar a alteração preservando a data de criação
		/// </summary>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CurriculoDisciplina entity)
		{
			base.ParamAlterar(qs, entity);

			qs.Parameters.RemoveAt("@crd_dataCriacao");
			qs.Parameters["@crd_dataAlteracao"].Value = DateTime.Now;
		}

		/// <summary>
		/// Método alterado para que o update não faça a alteração da data de criação
		/// </summary>
		/// <param name="entity"> Entidade ACA_CurriculoDisciplina</param>
		/// <returns>true = sucesso | false = fracasso</returns> 
		protected override bool Alterar(ACA_CurriculoDisciplina entity)
		{
			__STP_UPDATE = "NEW_ACA_CurriculoDisciplina_Update";
			return base.Alterar(entity);
		}

		/// <summary>
		/// Parâmetros para efetuar a exclusão lógica.
		/// </summary>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CurriculoDisciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = entity.crp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@dis_id";
			Param.Size = 4;
			Param.Value = entity.dis_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crd_situacao";
			Param.Size = 1;
			Param.Value = 3;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@crd_dataAlteracao";
			Param.Size = 8;
			Param.Value = DateTime.Now;
			qs.Parameters.Add(Param);
		}

		/// <summary>
		/// Método alterado para que o delete não faça exclusão física e sim lógica (update).
		/// </summary>
		/// <param name="entity"> Entidade ACA_CurriculoDisciplina</param>
		/// <returns>true = sucesso | false = fracasso</returns>        
		public override bool Delete(ACA_CurriculoDisciplina entity)
		{
			__STP_DELETE = "NEW_ACA_CurriculoDisciplina_Update_Situacao";
			return base.Delete(entity);
		}

		///// <summary>
		///// Inseri os valores da classe em um registro ja existente
		///// </summary>
		///// <param name="entity">Entidade com os dados a serem modificados</param>
		///// <returns>True - Operacao bem sucedida</returns>
		//protected override bool Alterar(ACA_CurriculoDisciplina entity)
		//{
		//    return base.Alterar(entity);
		//}
		///// <summary>
		///// Inseri os valores da classe em um novo registro
		///// </summary>
		///// <param name="entity">Entidade com os dados a serem inseridos</param>
		///// <returns>True - Operacao bem sucedida</returns>
		//protected override bool Inserir(ACA_CurriculoDisciplina entity)
		//{
		//    return base.Inserir(entity);
		//}
		///// <summary>
		///// Carrega um registro da tabela usando os valores nas chaves
		///// </summary>
		///// <param name="entity">Entidade com os dados a serem carregados</param>
		///// <returns>True - Operacao bem sucedida</returns>
		//public override bool Carregar(ACA_CurriculoDisciplina entity)
		//{
		//    return base.Carregar(entity);
		//}
		///// <summary>
		///// Exclui um registro do banco
		///// </summary>
		///// <param name="entity">Entidade com os dados a serem apagados</param>
		///// <returns>True - Operacao bem sucedida</returns>
		//public override bool Delete(ACA_CurriculoDisciplina entity)
		//{
		//    return base.Delete(entity);
		//}
		///// <summary>
		///// Configura os parametros do metodo de Alterar
		///// </summary>
		///// <param name="qs">Objeto da Store Procedure</param>
		///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
		//protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CurriculoDisciplina entity)
		//{
		//    base.ParamAlterar(qs, entity);
		//}
		///// <summary>
		///// Configura os parametros do metodo de Carregar
		///// </summary>
		///// <param name="qs">Objeto da Store Procedure</param>
		///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
		//protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_CurriculoDisciplina entity)
		//{
		//    base.ParamCarregar(qs, entity);
		//}
		///// <summary>
		///// Configura os parametros do metodo de Deletar
		///// </summary>
		///// <param name="qs">Objeto da Store Procedure</param>
		///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
		//protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CurriculoDisciplina entity)
		//{
		//    base.ParamDeletar(qs, entity);
		//}
		///// <summary>
		///// Configura os parametros do metodo de Inserir
		///// </summary>
		///// <param name="qs">Objeto da Store Procedure</param>
		///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
		//protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CurriculoDisciplina entity)
		//{
		//    base.ParamInserir(qs, entity);
		//}
		///// <summary>
		///// Salva o registro no banco de dados
		///// </summary>
		///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
		///// <returns>True - Operacao bem sucedida</returns>
		//public override bool Salvar(ACA_CurriculoDisciplina entity)
		//{
		//    return base.Salvar(entity);
		//}
		///// <summary>
		///// Realiza o select da tabela
		///// </summary>
		///// <returns>Lista com todos os registros da tabela</returns>
		//public override IList<ACA_CurriculoDisciplina> Select()
		//{
		//    return base.Select();
		//}
		///// <summary>
		///// Realiza o select da tabela com paginacao
		///// </summary>
		///// <param name="currentPage">Pagina atual</param>
		///// <param name="pageSize">Tamanho da pagina</param>
		///// <param name="totalRecord">Total de registros na tabela original</param>
		///// <returns>Lista com todos os registros da p�gina</returns>
		//public override IList<ACA_CurriculoDisciplina> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
		//{
		//    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
		//}
		///// <summary>
		///// Recebe o valor do auto incremento e coloca na propriedade 
		///// </summary>
		///// <param name="qs">Objeto da Store Procedure</param>
		///// <param name="entity">Entidade com os dados</param>
		///// <returns>True - Operacao bem sucedida</returns>
		//protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_CurriculoDisciplina entity)
		//{
		//    return base.ReceberAutoIncremento(qs, entity);
		//}
		///// <summary>
		///// Passa os dados de um datatable para uma entidade
		///// </summary>
		///// <param name="dr">DataRow do datatable preenchido</param>
		///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
		///// <returns>Entidade preenchida</returns>
		//public override ACA_CurriculoDisciplina DataRowToEntity(DataRow dr, ACA_CurriculoDisciplina entity)
		//{
		//    return base.DataRowToEntity(dr, entity);
		//}
		///// <summary>
		///// Passa os dados de um datatable para uma entidade
		///// </summary>
		///// <param name="dr">DataRow do datatable preenchido</param>
		///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
		///// <param name="limparEntity">Indica se a entidade deve ser limpada antes da transferencia</param>
		///// <returns>Entidade preenchida</returns>
		//public override ACA_CurriculoDisciplina DataRowToEntity(DataRow dr, ACA_CurriculoDisciplina entity, bool limparEntity)
		//{
		//    return base.DataRowToEntity(dr, entity, limparEntity);
		//}
	}
}