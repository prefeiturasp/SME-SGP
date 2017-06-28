/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO : Abstract_CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO
	{
        /// <summary>
        /// Retorna o id dos alunos que possuem registro de anotação da recuperação paralela.
        /// </summary>
        /// <param name="tpc_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public DataTable SelecionaAlunoPreenchimentoPorPeriodoDisciplina(int tpc_id, long tur_id, long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioPreenchimentoAlunoTurmaDisciplina_SelecionaAlunoPreenchimentoPorPeriodoDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
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
        /// Retorna os lançamentos feito para o aluno de acordo com os parâmetros.
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(long alu_id, long tur_id, long tud_id, bool apenasComPreenchimento, int rea_id, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioPreenchimentoAlunoTurmaDisciplina_SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                {
                    Param.Value = tur_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                {
                    Param.Value = tud_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@apenasComPreenchimento";
                Param.Size = 1;
                Param.Value = apenasComPreenchimento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rea_id";
                Param.Size = 4;
                Param.Value = rea_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;  
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (tpc_id > 0)
                {
                    Param.Value = tpc_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
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

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_RelatorioPreenchimentoAlunoTurmaDisciplina entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@ptd_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@ptd_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_RelatorioPreenchimentoAlunoTurmaDisciplina entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@ptd_dataCriacao");
            qs.Parameters["@ptd_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>s
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade CLS_RelatorioPreenchimentoAlunoTurmaDisciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(CLS_RelatorioPreenchimentoAlunoTurmaDisciplina entity)
        {
            __STP_UPDATE = "NEW_CLS_RelatorioPreenchimentoAlunoTurmaDisciplina_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_RelatorioPreenchimentoAlunoTurmaDisciplina entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@reap_id";
            Param.Size = 8;
            Param.Value = entity.reap_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ptd_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ptd_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade CLS_RelatorioPreenchimentoAlunoTurmaDisciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(CLS_RelatorioPreenchimentoAlunoTurmaDisciplina entity)
        {
            __STP_DELETE = "NEW_CLS_RelatorioPreenchimentoAlunoTurmaDisciplina_UpdateSituacao";
            return base.Delete(entity);
        }
        
        #endregion Métodos Sobrescritos
    }
}