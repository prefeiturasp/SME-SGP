/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System.Linq;
    using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ORC_OrientacaoCurricularDAO : Abstract_ORC_OrientacaoCurricularDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Seleciona as orientações curriculares por calendario, período e por disciplina.
        /// </summary>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <returns></returns>
        public DataTable SelecionaPorCalendarioPeriodoDisciplina(int cal_id, int cur_id, int crr_id, int crp_id, int tds_id, long mat_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricular_SelecionaPorCalendarioPeriodoDisciplina", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

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
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@mat_id";
                Param.Size = 4;
                Param.Value = mat_id;
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
        /// Seleciona as orientações curriculares por nivel.
        /// <param name="cal_id">ID do nível.</param>     
        /// <returns></returns>
        public DataTable SelecionaPorNivel(int nvl_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaOrientacoesCurricularesPorNivel", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nvl_id";
                Param.Size = 4;
                Param.Value = nvl_id;
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
        /// Busca as orientacoes curriculares pela turmadisciplina e database
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public List<ORC_OrientacaoCurricular> SelecionaPorTurmaDisciplinaDataBase(long tud_id, DateTime dataBase, long ocr_idSuperior)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricular_SelecionaPorTurmaDisciplinaDataBase", _Banco);
            List<ORC_OrientacaoCurricular> lt = new List<ORC_OrientacaoCurricular>();
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 16;
                if (dataBase != new DateTime())
                    Param.Value = dataBase;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_idSuperior";
                Param.Size = 8;
                if (ocr_idSuperior >= 0)
                    Param.Value = ocr_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    ORC_OrientacaoCurricular entity = new ORC_OrientacaoCurricular();
                    lt.Add(DataRowToEntity(dr, entity));
                }
                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        
        /// <summary>
        /// Busca as orientacoes curriculares pela database, entidade e escola
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public DataTable SelecionaOrientacoesPorEntidadeEscolaDataBase(Guid ent_id, int esc_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricular_SelecionaPorEntidadeEscolaDataBase", _Banco);
            
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (!ent_id.Equals(new Guid()))
                    Param.Value = ent_id;
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
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 16;
                if (dataBase != new DateTime())
                    Param.Value = dataBase;
                else
                    Param.Value = DBNull.Value;
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
        /// Busca as orientacoes curriculares pela turmadisciplina e database, ocr_idSuperior e tipo periodo calendario
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="dataBase"></param>
        /// <param name="ocr_idSuperior"></param>
        /// <param name="tpc_id">Id Tipo periodo Calendario</param>
        /// <returns></returns>
        public List<ORC_OrientacaoCurricular> SelecionaPorTurmaDisciplinaDataBaseTpc_id(long tud_id, DateTime dataBase, long ocr_idSuperior, Nullable<long> tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricular_SelecionaPorTurmaDisciplinaDataBaseTpc_id", _Banco);
            List<ORC_OrientacaoCurricular> lt = new List<ORC_OrientacaoCurricular>();
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 16;
                if (dataBase != new DateTime())
                    Param.Value = dataBase;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_idSuperior";
                Param.Size = 8;
                if (ocr_idSuperior >= 0)
                    Param.Value = ocr_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);
                
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tpc_id";
                Param.Size = 16;
                if (tpc_id != null)
                    Param.Value = tpc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                lt = qs.Return.AsEnumerable().Select(x => DataRowToEntity(x, new ORC_OrientacaoCurricular())).ToList();

                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }


        /// <summary>
        /// Retorna registros ativos ou com data de alteração posterior a ultima sincronizacao.
        /// </summary>
        /// <param name="syncDate">Data da última sincronização</param>
        /// <param name="cur_id">Curso</param>
        /// <param name="crr_id">Curriculo</param>
        /// <param name="crp_id">Curriculo periodo</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="tds_id">Tipo de disciplina</param>
        /// <returns></returns>
        public DataTable SelectPorDataSincronizacao(DateTime syncDate, Int64 tur_id, int cur_id, int crr_id, int crp_id, int cal_id, int tds_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacoesCurricularesPorDataSincronizacao", _Banco);
            try
            {
                #region parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;
                if (syncDate != new DateTime())
                    Param.Value = syncDate;
                else
                    Param.Value = DBNull.Value;
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
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
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
        /// Seleciona lista de IDs das orientações curriculares que derivam da orientação passada por parâmetro
        /// e que possuem nível final.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular em que a busca é iniciada.</param>
        /// <returns>Lista de IDs de orientações curriculares.</returns>
        public List<long> SelecionaUltimoNivel(long ocr_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricular_SelecionaUltimoNivel", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                Param.Value = ocr_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                       qs.Return.Rows.Cast<DataRow>().Select(dr => Convert.ToInt64(dr["ocr_id"])).ToList() :
                       new List<long>();
            }
            finally
            {
                qs.Parameters.Clear();
            }

        }

        /// <summary>
        /// Seleciona as orientações curriculares por calendario, período e por disciplina para construção do treeview.
        /// </summary>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <returns>DataTabel com dados</returns>
        public DataTable SelecionaPorCalendarioPeriodoDisciplinaTreeview(int cal_id, int cur_id, int crr_id, int crp_id, int tds_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricular_RecursivaTreeview", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

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
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
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
        /// Seleciona as orientações curriculares por calendario, período e por disciplina para construção do treeview de relacionamento entre matrizes.
        /// </summary>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <param name="mat_id">ID da matriz relacionada ou que se deseja relacionar.</param>
        /// <param name="ocr_id">ID da habilidade da matriz padrão para relacionamento.</param>
        /// <returns>DataTabel com dados</returns>
        public DataTable SelecionaPorCalendarioPeriodoDisciplinaTreeview_ByMatriz(int cal_id, int cur_id, int crr_id, int crp_id, int tds_id, Int64 mat_id, Int64 ocr_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricular_RecursivaTreeview_ByMatriz", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

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
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@mat_id";
                Param.Size = 8;
                Param.Value = mat_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                Param.Value = ocr_id;
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
        /// Retorna se a orientação possui habilidade relacionada.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular.</param>
        /// <returns></returns>
        public bool SelecionaPossuiRelacionamento(Int64 ocr_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricular_SelecionaPossuiRelacionamento", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                Param.Value = ocr_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return Convert.ToBoolean(qs.Return.Rows[0]["Relacionada"]);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Método verifica se existe orientação curricular com o código passado.
        /// </summary>
        /// <param name="entity">Entity da orientação curricular</param>
        /// <returns></returns>
        public bool SelectBy_CodigoOrientacaoCurricular(ORC_OrientacaoCurricular entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricular_SelectBy_Codigo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@ocr_codigo";
                Param.Size = 100;
                Param.Value = entity.ocr_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@mat_id";
                Param.Size = 8;
                Param.Value = entity.mat_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nvl_id";
                Param.Size = 4;
                Param.Value = entity.nvl_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_idSuperior";
                Param.Size = 8;
                Param.Value = entity.ocr_idSuperior;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity, false);
                    return true;
                }
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

        #endregion

        #region Métodos de validação

        /// <summary>
        /// Método verifica se para o mesmo nível e disciplina existe uma orientação curricular com o código passado.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular.</param>
        /// <param name="nvl_id">ID do nível.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <param name="ocr_codigo">Código da orientação curricular.</param>
        /// <returns>True se já existe.</returns>
        public bool VerificaCodigoExistente(long ocr_id, long ocr_idSuperior, int nvl_id, int tds_id, string ocr_codigo)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ORC_OrientacaoCurricular_VerificaCodigoExistente", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                if (ocr_id > 0)
                    Param.Value = ocr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_idSuperior";
                Param.Size = 8;
                if (ocr_idSuperior > 0)
                    Param.Value = ocr_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nvl_id";
                Param.Size = 4;
                Param.Value = nvl_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@ocr_codigo";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(ocr_codigo))
                    Param.Value = ocr_codigo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Método verifica se para o mesmo nível e disciplina existe uma orientação curricular com a descrição passada.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular.</param>
        /// <param name="nvl_id">ID do nível.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <param name="ocr_descricao">Descrição da orientação curricular.</param>
        /// <returns>True se já existe.</returns>
        public bool VerificaDescricaoExistente(long ocr_id, long ocr_idSuperior, int nvl_id, int tds_id, string ocr_descricao)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ORC_OrientacaoCurricular_VerificaDescricaoExistente", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                if (ocr_id > 0)
                    Param.Value = ocr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_idSuperior";
                Param.Size = 8;
                if (ocr_idSuperior > 0)
                    Param.Value = ocr_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nvl_id";
                Param.Size = 4;
                Param.Value = nvl_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@ocr_descricao";
                if (!string.IsNullOrEmpty(ocr_descricao))
                    Param.Value = ocr_descricao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos de exclusão

        /// <summary>
        /// O método deleta uma orientação curricular e todas as outras ligadas a esse.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular inicial.</param>
        /// <returns></returns>
        public bool DeletarHierarquia(long ocr_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ORC_OrientacaoCurricular_UpdateSituacaoHierarquia", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                Param.Value = ocr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@ocr_situacao";
                Param.Size = 1;
                Param.Value = 3;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@ocr_dataAlteracao";
                Param.Size = 16;
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, Entities.ORC_OrientacaoCurricular entity)
        {
            entity.ocr_dataCriacao = entity.ocr_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, Entities.ORC_OrientacaoCurricular entity)
        {
            entity.ocr_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@ocr_dataCriacao");
        }

        protected override bool Alterar(Entities.ORC_OrientacaoCurricular entity)
        {
            __STP_UPDATE = "NEW_ORC_OrientacaoCurricular_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, Entities.ORC_OrientacaoCurricular entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ocr_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ocr_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(Entities.ORC_OrientacaoCurricular entity)
        {
            __STP_DELETE = "NEW_ORC_OrientacaoCurricular_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion

    }
}