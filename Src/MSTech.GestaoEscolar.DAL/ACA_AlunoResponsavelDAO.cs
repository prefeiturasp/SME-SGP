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
    public class ACA_AlunoResponsavelDAO : Abstract_ACA_AlunoResponsavelDAO
    {
        /// <summary>
        /// Retorna um DataTable com as informações dos responsáveis cadastrados para 
        /// o aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns></returns>
        public DataTable SelectBy_Aluno
        (
            Int64 alu_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoResponsavel_SelectBy_alu_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
                dt = qs.Return;

            return dt;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os contatos do responsavel pelo aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// aluno
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>        
        /// <returns>DataTable com os contatos do responsavel pelo aluno</returns>
        public DataTable SelectContatosBy_alu_id
        (
            long alu_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoResponsavelContato_SelectBy_alu_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                if (alu_id > 0)
                    Param.Value = alu_id;
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

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
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
        /// Atualiza a profissão e a situação da pessoa (Responsável).
        /// Caso for alterado a profissão de um responsável
        /// Caso for do tipo 4 - (Falecido) situação será atualizado 
        /// As atualizações serão feitas para todos a alunos no qual o mesmo é responsável
        /// </summary>
        /// <param name="pes_id">pes_id do responsável do aluno</param>
        /// <param name="alr_profissao">profissão do responsável</param>
        /// <param name="alr_empresa">empresa do responsável</param>
        /// <param name="alr_situacao">situação do responsável</param>
        /// <returns></returns>
        public bool Update_ProfissaoSituacao_AlunoResponsavel
        (
            Guid pes_id
            , string alr_profissao
            , string alr_empresa
            , byte alr_situacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoResponsavel_Update_ProfissaoResponsavel", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pes_id";
                Param.Size = 8;
                Param.Value = pes_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alr_profissao";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(alr_profissao))
                    Param.Value = alr_profissao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alr_empresa";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(alr_empresa))
                    Param.Value = alr_empresa;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@alr_situacao";
                Param.Size = 1;
                Param.Value = alr_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@alr_dataAlteracao";
                Param.Size = 16;
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return true;
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
        /// Retorna a situação e profissão do responsável do aluno.
        /// passando apenas o pes_id por parâmetro
        /// </summary>
        /// <param name="pes_id">pes_id do responsável do aluno</param>
        /// <returns>
        /// retorna a situação do responsável do aluno
        /// caso não exista o responsável é retornado "0"
        /// </returns>
        public DataTable RetornaAlunoResponsavel_Situacao_Profissao
        (
            Guid pes_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoResponsavel_Situacao_Profissao", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pes_id";
                Param.Size = 8;
                Param.Value = pes_id;
                qs.Parameters.Add(Param);
      
                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
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
        /// Retorna todos os alunos que o responsável possui.
        /// </summary>
        /// <param name="pes_id">ID do responsável do aluno</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosPorResponsavel(Guid pes_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoResponsavel_SELECTBY_pes_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pes_id";
                Param.Size = 8;
                Param.Value = pes_id;
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
        /// Retorna um datatable contendo todos os responsaveis do sistema, de acordo com os filtros.
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="nome">nome da pessoa</param>
        /// <param name="cpf">cpf da pessoa</param>
        /// <param name="rg">rg da pessoa</param>
        /// <param name="nis"></param>
        /// <param name="tdo_idnis"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="tdo_idcpf"></param>
        /// <param name="tdo_idrg"></param>
        public DataTable SelectBuscaResponsaveis
        (
            Guid ent_id
            , string nome
            , string cpf
            , string rg
            , string nis
            , Guid tdo_idcpf
            , Guid tdo_idrg
            , Guid tdo_idnis
            , int currentPage
            , int pageSize
            , out int totalRecords

        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoResponsavel_BuscaResponsaveis", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@pes_nome";
                if (!string.IsNullOrEmpty(nome))
                    Param.Value = nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@TIPO_DOCUMENTACAO_RG";
                Param.Size = 50;
                if (!String.IsNullOrEmpty(rg))
                    Param.Value = rg;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@TIPO_DOCUMENTACAO_CPF";
                Param.Size = 50;
                if (!String.IsNullOrEmpty(cpf))
                    Param.Value = cpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@TIPO_DOCUMENTACAO_NIS";
                Param.Size = 50;
                if (!String.IsNullOrEmpty(nis))
                    Param.Value = nis;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_idcpf";
                Param.Size = 16;
                if (tdo_idcpf != Guid.Empty)
                    Param.Value = tdo_idcpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_idrg";
                Param.Size = 16;
                if (tdo_idrg != Guid.Empty)
                    Param.Value = tdo_idrg;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_idnis";
                Param.Size = 16;
                if (tdo_idnis != Guid.Empty)
                    Param.Value = tdo_idnis;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                totalRecords = qs.Execute(currentPage / pageSize, pageSize);

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #region Métodos sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoResponsavel entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@alr_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@alr_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoResponsavel entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@alr_dataCriacao");
            qs.Parameters["@alr_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_AlunoResponsavel</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ACA_AlunoResponsavel entity)
        {
            __STP_UPDATE = "NEW_ACA_AlunoResponsavel_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoResponsavel entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@alr_id";
            Param.Size = 4;
            Param.Value = entity.alr_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pes_id";
            Param.Size = 16;
            Param.Value = entity.pes_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@alr_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@alr_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_AlunoResponsavel</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(ACA_AlunoResponsavel entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoResponsavel_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}