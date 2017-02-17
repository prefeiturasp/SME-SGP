/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
    /// ACA_TipoEquipamentoDeficienteDAO
	/// </summary>
	public class ACA_TipoEquipamentoDeficienteDAO : Abstract_ACA_TipoEquipamentoDeficienteDAO
	{
        /// <summary>
        /// Retorna todos os tipos de equipamentos de deficientes .
        /// </summary>        
        /// <param name="totalRecords">Total de registros retornado na busca</param> 
        /// <returns>DataTable contendo os tipos de equipamentos de deficientes  </returns>  
        public DataTable SelectBy_Pesquisa(out int totalRecords)
        {
            totalRecords = 0;

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoEquipamentoDeficiente_SelectBy_Pesquisa", _Banco);
            try
            {
                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se já existe um tipo de equipamento de deficiente cadastrado com o mesmo nome.
        /// </summary>
        /// <param name="ted_id">ID do tipo de equipamento de deficiente</param>
        /// <param name="ted_nome">Nome do tipo de equipamento de deficienete</param>   
        /// <returns>True | False</returns>     
        public bool SelectBy_Nome(int ted_id, string ted_nome)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoEquipamentoDeficiente_SelectBy_Nome", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@ted_nome";
                Param.Size = 100;
                Param.Value = ted_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ted_id";
                Param.Size = 4;
                if (ted_id > 0)
                    Param.Value = ted_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
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
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade ACA_TipoEquipamentoDeficiente</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoEquipamentoDeficiente entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@ted_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@ted_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade ACA_TipoEquipamentoDeficiente</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoEquipamentoDeficiente entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@ted_dataCriacao");
            qs.Parameters["@ted_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoEquipamentoDeficiente</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ACA_TipoEquipamentoDeficiente entity)
        {
            __STP_UPDATE = "NEW_ACA_TipoEquipamentoDeficiente_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoEquipamentoDeficiente</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(ACA_TipoEquipamentoDeficiente entity)
        {
            __STP_DELETE = "NEW_ACA_TipoEquipamentoDeficiente_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}