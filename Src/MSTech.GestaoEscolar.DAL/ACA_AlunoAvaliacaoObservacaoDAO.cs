/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
	
	/// <summary>
	/// Description: .
	/// </summary>
    public class ACA_AlunoAvaliacaoObservacaoDAO : AbstractACA_AlunoAvaliacaoObservacaoDAO
    {
        /// <summary>
        /// Retorna um datatable contendo todas as Observações de Avaliações do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>   
        /// <returns>DataTable com as Observações de Avaliações do Aluno</returns>
        public List<ACA_AlunoAvaliacaoObservacao> SelectBy_alu_id(long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoAvaliacaoObservacao_SelectBy_alu_id", _Banco);
            List<ACA_AlunoAvaliacaoObservacao> lst = new List<ACA_AlunoAvaliacaoObservacao>();

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    ACA_AlunoAvaliacaoObservacao entity = new ACA_AlunoAvaliacaoObservacao();
                    entity = DataRowToEntity(dr, entity);
                    lst.Add(entity);
                }

                return lst;
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

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, Entities.ACA_AlunoAvaliacaoObservacao entity)
        {
            entity.aao_dataCriacao = DateTime.Now;
            entity.aao_dataAlteracao = DateTime.Now;
            entity.aao_situacao = 1;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, Entities.ACA_AlunoAvaliacaoObservacao entity)
        {

            entity.aao_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@aao_dataCriacao");
        }

        protected override bool Alterar(Entities.ACA_AlunoAvaliacaoObservacao entity)
        {
            __STP_UPDATE = "NEW_ACA_AlunoAvaliacaoObservacao_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, Entities.ACA_AlunoAvaliacaoObservacao entity)
        {
            base.ParamDeletar(qs, entity);
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@aao_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@aao_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(Entities.ACA_AlunoAvaliacaoObservacao entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoAvaliacaoObservacao_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion Sobrescritos
    }
}