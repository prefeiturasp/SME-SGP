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
    public class ACA_CurriculoCapituloDAO : Abstract_ACA_CurriculoCapituloDAO
	{
        #region Consulta

        /// <summary>
        /// Retorna os capítulos do currículo cadastrados de acordo com o tipo de nível de ensino e disciplina.
        /// </summary>
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tds_id">Id do tipo de disciplina</param>
        /// <returns></returns>
        public DataTable SelecionaPorNivelEnsinoDisciplina(int tne_id, int tds_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoCapitulo_SelecionaPorNivelEnsinoDisciplina", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Value = tne_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                if (tds_id > 0)
                {
                    Param.Value = tds_id;
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

        #endregion Consulta

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CurriculoCapitulo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@crc_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@crc_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CurriculoCapitulo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@crc_dataCriacao");
            qs.Parameters["@crc_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>s
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_CurriculoCapitulo</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(ACA_CurriculoCapitulo entity)
        {
            __STP_UPDATE = "NEW_ACA_CurriculoCapitulo_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CurriculoCapitulo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crc_id";
            Param.Size = 4;
            Param.Value = entity.crc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crc_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@crc_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_CurriculoCapitulo</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(ACA_CurriculoCapitulo entity)
        {
            __STP_DELETE = "NEW_ACA_CurriculoCapitulo_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos
    }
}