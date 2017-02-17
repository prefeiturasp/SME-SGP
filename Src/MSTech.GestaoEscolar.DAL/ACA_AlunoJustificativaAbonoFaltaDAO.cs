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
    public class ACA_AlunoJustificativaAbonoFaltaDAO : Abstract_ACA_AlunoJustificativaAbonoFaltaDAO
	{
        /// <summary>
        /// Inseri os valores da classe em um registro ja existente.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem modificados.</param>
        /// <returns>True - Operacao bem sucedida.</returns>
        protected override bool Alterar(ACA_AlunoJustificativaAbonoFalta entity)
        {
            __STP_UPDATE = "NEW_ACA_AlunoJustificativaAbonoFalta_UPDATE";
            return base.Alterar(entity); ;
        }

        /// <summary>
        /// Exclui um registro do banco.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem apagados.</param>
        /// <returns>True - Operacao bem sucedida.</returns>
        public override bool Delete(ACA_AlunoJustificativaAbonoFalta entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoJustificativaAbonoFalta_Update_Situacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoJustificativaAbonoFalta entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@ajf_dataCriacao");
            qs.Parameters["@ajf_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoJustificativaAbonoFalta entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@ajf_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@ajf_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoJustificativaAbonoFalta entity)
        {
            if (entity != null & qs != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Selecionar as justificativas por aluno e disciplina.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="tud_id">Id da disciplina.</param>
        /// <returns>DataTable com os dados selecionados.</returns>
        public DataTable SelecionarPorAlunoETurmaDisciplina(long alu_id, long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoJustificativaAbonoFalta_SelecionarPorAlunoETurmaDisciplina", _Banco);

            #region Parametros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            #endregion Parametros

            qs.Execute();
            qs.Parameters.Clear();

            return qs.Return;
        }
    }
}