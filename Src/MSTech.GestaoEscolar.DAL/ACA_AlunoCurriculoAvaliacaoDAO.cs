/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class ACA_AlunoCurriculoAvaliacaoDAO : Abstract_ACA_AlunoCurriculoAvaliacaoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna o currículo avaliação onde o aluno está ativo.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do período do curso</param>
        /// <returns></returns>
        public DataTable LoadBy_AtualAluno
        (
            long alu_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculoAvaliacao_SelectBy_AtualAluno", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
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

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna o registro do aluno na AlunoCurriculoAvaliacao na ordem:
        /// - Se for encontrado um registro para o lançamento 
        /// já realizado (CLS_AlunoAvaliacaoTurma).
        /// - Se não for encontrado, retorna o registro ativo, em que
        /// não tenha lançamento na CLS_AlunoAvaliacaoTurma.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="mtu_id">ID da matrícula do aluno na turma</param>
        /// <param name="aat_id">ID do lançamento de nota do aluno na CLS_AlunoAvaliacaoTurma</param>
        /// <returns></returns>
        public DataTable LoadBy_LancamentoAluno
        (
            long alu_id
            , long tur_id
            , int mtu_id
            , int aat_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoCurriculoAvaliacao_SelectBy_LancamentoAluno", _Banco);

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
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mtu_id";
            Param.Size = 4;
            Param.Value = mtu_id;
            qs.Parameters.Add(Param);
            
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@aat_id";
            Param.Size = 4;
            Param.Value = aat_id;
            qs.Parameters.Add(Param);
            
            #endregion

            qs.Execute();
            
            return qs.Return;
        }

        #endregion

        #region Sobrescritos

        /// <summary>
        /// Override do nome da string de conexão.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Override do método que adiciona os parâmetro de inserir na procedure.
        /// </summary>
        /// <param name="qs">Procedure</param>
        /// <param name="entity">Entidade a ser salva</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoCurriculoAvaliacao entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@ala_dataInicio"].DbType = DbType.Date;
            qs.Parameters["@ala_dataFim"].DbType = DbType.Date;
            
            qs.Parameters["@ala_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@ala_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método que adiciona os parâmetro de alteração na procedure.
        /// </summary>
        /// <param name="qs">Procedure</param>
        /// <param name="entity">Entidade a ser salva</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoCurriculoAvaliacao entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@ala_dataInicio"].DbType = DbType.Date;
            qs.Parameters["@ala_dataFim"].DbType = DbType.Date;

            qs.Parameters.RemoveAt("@ala_dataCriacao");
            qs.Parameters["@ala_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método de alteração - muda o nome da procedure padrão.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <returns></returns>
        protected override bool Alterar(ACA_AlunoCurriculoAvaliacao entity)
        {
            __STP_UPDATE = "NEW_ACA_AlunoCurriculoAvaliacao_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Override do método que adiciona os parâmetro de exclusão na procedure.
        /// </summary>
        /// <param name="qs">Procedure</param>
        /// <param name="entity">Entidade a ser excluída</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoCurriculoAvaliacao entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ala_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ala_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Override do método de exclusão - muda o nome da procedure padrão.
        /// </summary>
        /// <param name="entity">Entidade a ser excluída</param>
        /// <returns></returns>
        public override bool Delete(ACA_AlunoCurriculoAvaliacao entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoCurriculoAvaliacao_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}