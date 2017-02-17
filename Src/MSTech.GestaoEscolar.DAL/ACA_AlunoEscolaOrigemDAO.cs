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
    public class ACA_AlunoEscolaOrigemDAO : Abstract_ACA_AlunoEscolaOrigemDAO
    {
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Retorna as escolas de origem filtrando por Rede de ensino e/ou nome.
        /// </summary>
        /// <param name="tre_id">ID do tipo de rede de ensino</param>
        /// <param name="eco_nome">Nome da escola de origem</param>        
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public DataTable Select_EscolasPor_RedeEnsino_Nome
        (
             int tre_id
            , string eco_nome
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoEscolaOrigem_SelectBy_RedeEnsino_Nome", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tre_id";
            Param.Size = 4;
            if (tre_id > 0)
                Param.Value = tre_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@eco_nome";
            Param.Size = 200;
            if (!string.IsNullOrEmpty(eco_nome))
                Param.Value = eco_nome;
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

        /// <summary>
        /// Retorna um datatable contendo todas as escolas de origem
        /// que não foram excluídas logicamente, filtrados por 
        /// id, nome, situacao.        
        /// </summary>
        /// <param name="eco_id">Id da tabela ACA_AlunoEscolaOrigem do bd</param>
        /// <param name="tre_id">Id do tipo de rede de ensino</param>
        /// <param name="eco_nome">Campo eco_nome da tabela ACA_AlunoEscolaOrigem do bd</param>        
        /// <param name="eco_situacao">Campo eco_situacao da tabela ACA_AlunoEscolaOrigem do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>   
        /// <returns>DataTable com todas as escolas de origem</returns>
        public DataTable SelectBy_All
        (
            long eco_id
            , int tre_id
            , string eco_nome
            , byte eco_situacao
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoEscolaOrigem_SelectBy_All", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@eco_id";
                Param.Size = 8;
                if (eco_id > 0)
                    Param.Value = eco_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tre_id";
                Param.Size = 4;
                if (tre_id > 0)
                    Param.Value = tre_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@eco_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(eco_nome))
                    Param.Value = eco_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@eco_situacao";
                Param.Size = 1;
                if (eco_situacao > 0)
                    Param.Value = eco_situacao;
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
        /// Override do método inserir
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@eco_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@eco_dataAlteracao"].Value = DateTime.Now;

            if (new Guid(qs.Parameters["@end_id"].Value.ToString()) == Guid.Empty)
                qs.Parameters["@end_id"].Value = DBNull.Value;

            if (new Guid(qs.Parameters["@cid_id"].Value.ToString()) == Guid.Empty)
                qs.Parameters["@cid_id"].Value = DBNull.Value;

            if (new Guid(qs.Parameters["@unf_id"].Value.ToString()) == Guid.Empty)
                qs.Parameters["@unf_id"].Value = DBNull.Value;

        }

        /// <summary>
        /// Override do método alterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@eco_dataCriacao");
            qs.Parameters["@eco_dataAlteracao"].Value = DateTime.Now;

            if (new Guid(qs.Parameters["@end_id"].Value.ToString()) == Guid.Empty)
                qs.Parameters["@end_id"].Value = DBNull.Value;

            if (new Guid(qs.Parameters["@cid_id"].Value.ToString()) == Guid.Empty)
                qs.Parameters["@cid_id"].Value = DBNull.Value;

            if (new Guid(qs.Parameters["@unf_id"].Value.ToString()) == Guid.Empty)
                qs.Parameters["@unf_id"].Value = DBNull.Value;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_AlunoEscolaOrigem</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ACA_AlunoEscolaOrigem entity)
        {
            __STP_UPDATE = "NEW_ACA_AlunoEscolaOrigem_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@eco_id";
            Param.Size = 8;
            Param.Value = entity.eco_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@eco_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@eco_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_AlunoEscolaOrigem</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(ACA_AlunoEscolaOrigem entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoEscolaOrigem_Update_Situacao";
            return base.Delete(entity);
        }



        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_AlunoEscolaOrigem entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_AlunoEscolaOrigem entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_AlunoEscolaOrigem entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_AlunoEscolaOrigem entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_AlunoEscolaOrigem entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_AlunoEscolaOrigem> Select()
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
        //public override IList<ACA_AlunoEscolaOrigem> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_AlunoEscolaOrigem DataRowToEntity(DataRow dr, ACA_AlunoEscolaOrigem entity)
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
        //public override ACA_AlunoEscolaOrigem DataRowToEntity(DataRow dr, ACA_AlunoEscolaOrigem entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}
    }
}