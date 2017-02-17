/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.Data.Common.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
	
	/// <summary>
	/// Classe abstrata de ACA_AlunoCurriculo
	/// </summary>
    public abstract class Abstract_ACA_AlunoFichaMedicaDAO : Abstract_DAL<ACA_AlunoFichaMedica>
	{
	
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar
		/// </ssummary>
		/// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoFichaMedica entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoFichaMedica entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@afm_tipoSanguineo";
			Param.Size = 5;
            if (!string.IsNullOrEmpty(entity.afm_tipoSanguineo))
                Param.Value = entity.afm_tipoSanguineo;
            else
                Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_fatorRH";
            Param.Size = 5;
            if (!string.IsNullOrEmpty(entity.afm_fatorRH))
                Param.Value = entity.afm_fatorRH;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_doencasConhecidas";
            if (!string.IsNullOrEmpty(entity.afm_doencasConhecidas))
                Param.Value = entity.afm_doencasConhecidas;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_alergias";
            if (!string.IsNullOrEmpty(entity.afm_alergias))
                Param.Value = entity.afm_alergias;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_medicacoesPodeUtilizar";
            if (!string.IsNullOrEmpty(entity.afm_medicacoesPodeUtilizar))
                Param.Value = entity.afm_medicacoesPodeUtilizar;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_medicacoesUsoContinuo";
            if (!string.IsNullOrEmpty(entity.afm_medicacoesUsoContinuo))
                Param.Value = entity.afm_medicacoesUsoContinuo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_convenioMedico";
            Param.Size = 1000;
            if (!string.IsNullOrEmpty(entity.afm_convenioMedico))
                Param.Value = entity.afm_convenioMedico;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_hospitalRemocao";
            Param.Size = 1000;
            if (!string.IsNullOrEmpty(entity.afm_hospitalRemocao))
                Param.Value = entity.afm_hospitalRemocao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_outrasRecomendacoes";
            if (!string.IsNullOrEmpty(entity.afm_outrasRecomendacoes))
                Param.Value = entity.afm_outrasRecomendacoes;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoFichaMedica entity)
		{
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_tipoSanguineo";
            Param.Size = 5;
            if (!string.IsNullOrEmpty(entity.afm_tipoSanguineo))
                Param.Value = entity.afm_tipoSanguineo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_fatorRH";
            Param.Size = 5;
            if (!string.IsNullOrEmpty(entity.afm_fatorRH))
                Param.Value = entity.afm_fatorRH;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_doencasConhecidas";
            if (!string.IsNullOrEmpty(entity.afm_doencasConhecidas))
                Param.Value = entity.afm_doencasConhecidas;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_alergias";
            if (!string.IsNullOrEmpty(entity.afm_alergias))
                Param.Value = entity.afm_alergias;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_medicacoesPodeUtilizar";
            if (!string.IsNullOrEmpty(entity.afm_medicacoesPodeUtilizar))
                Param.Value = entity.afm_medicacoesPodeUtilizar;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_medicacoesUsoContinuo";
            if (!string.IsNullOrEmpty(entity.afm_medicacoesUsoContinuo))
                Param.Value = entity.afm_medicacoesUsoContinuo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_convenioMedico";
            Param.Size = 1000;
            if (!string.IsNullOrEmpty(entity.afm_convenioMedico))
                Param.Value = entity.afm_convenioMedico;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_hospitalRemocao";
            Param.Size = 1000;
            if (!string.IsNullOrEmpty(entity.afm_hospitalRemocao))
                Param.Value = entity.afm_hospitalRemocao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@afm_outrasRecomendacoes";
            if (!string.IsNullOrEmpty(entity.afm_outrasRecomendacoes))
                Param.Value = entity.afm_outrasRecomendacoes;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoFichaMedica entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoFichaMedica entity)
		{
            entity.alu_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.alu_id > 0);
        }		
	}
}

