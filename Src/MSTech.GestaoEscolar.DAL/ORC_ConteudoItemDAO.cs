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
	public class ORC_ConteudoItemDAO : Abstract_ORC_ConteudoItemDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna os itens de conteúdo cadastrados para o conteúdo.
        /// </summary>
        /// <param name="obj_id">ID do objetivo</param>
        /// <param name="ctd_id">ID do conteúdo</param>
        /// <returns></returns>
        public DataTable SelectBy_Conteudo
        (
            int obj_id
            , int ctd_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_ConteudoItem_SelectBy_Conteudo", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@obj_id";
            Param.Size = 4;
            Param.Value = obj_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ctd_id";
            Param.Size = 4;
            Param.Value = ctd_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os itens do conteúdo do objetivo
        /// </summary>                
        /// <param name="obj_id">ID do objetivo</param>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="naoAlcancadasAnoAnterior">Indica se vai trazer os dados do ano atual ou do anterior</param>
        public DataTable SelectBy_obj_id
        (
            int obj_id
            , long tud_id
            , bool naoAlcancadasAnoAnterior
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_ConteudoItem_SelectBy_obj_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@obj_id";
                Param.Size = 4;                
                Param.Value = obj_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@naoAlcancadasAnoAnterior";
                Param.Size = 1;
                Param.Value = naoAlcancadasAnoAnterior;
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
        /// Verifica se existe o item do conteúdo com a descrição passada no mesmo objetivo
        /// com o cti_id diferente do passado.
        /// </summary>                
        /// <param name="obj_id">Id do objetivo</param>
        /// <param name="cti_id">Id do contéudo item.</param>
        /// <param name="cti_descricao">Descrição do item do conteúdo.</param>
        public bool SelectBy_Nome
        (
            int obj_id
            , int cti_id
            , string cti_descricao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_ConteudoItem_SelectBy_Nome_Objetivo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@obj_id";
                Param.Size = 4;
                Param.Value = obj_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cti_id";
                Param.Size = 4;
                if (cti_id > 0)
                    Param.Value = cti_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cti_descricao";
                Param.Size = 4000;
                Param.Value = cti_descricao;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0;
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
        /// Retorna os itens do conteúdos dos objetivos da turma disciplina e currículo período
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="cur_idAtual">ID do curso</param>
        /// <param name="crr_idAtual">ID do curriculo</param>
        /// <param name="crp_idAtual">ID do currículo período atual</param>
        /// <param name="crp_idAnterior">ID do currículo período anterior</param>
        /// <param name="tdt_posicao">Posição do docente responsável</param>
        /// <returns></returns>
        public DataTable SelecionaPorTurmaDisciplinaCurriculoPeriodo
        (
            long tud_id
            , int cur_idAtual
            , int crr_idAtual
            , int crp_idAtual
            , int crp_idAnterior
            , byte tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_ConteudoItem_SelecionaPorTurmaDisciplinaCurriculoPeriodo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_idAtual";
                Param.Size = 4;
                Param.Value = cur_idAtual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_idAtual";
                Param.Size = 4;
                Param.Value = crr_idAtual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_idAtual";
                Param.Size = 4;
                Param.Value = crp_idAtual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_idAnterior";
                Param.Size = 4;
                Param.Value = crp_idAnterior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
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


        #endregion

        #region Métodos Sobrescritos

        /// <summary>
        /// Override do nome da ConnectionString.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Override do método inserir
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ORC_ConteudoItem entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@cti_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@cti_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método alterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ORC_ConteudoItem entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@cti_dataCriacao");
            qs.Parameters["@cti_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ORC_ConteudoItem</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ORC_ConteudoItem entity)
        {
            __STP_UPDATE = "NEW_ORC_ConteudoItem_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ORC_ConteudoItem entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@obj_id";
            Param.Size = 4;
            Param.Value = entity.obj_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ctd_id";
            Param.Size = 4;
            Param.Value = entity.ctd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cti_id";
            Param.Size = 4;
            Param.Value = entity.cti_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@cti_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@cti_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ORC_ConteudoItem</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(ORC_ConteudoItem entity)
        {
            __STP_DELETE = "NEW_ORC_ConteudoItem_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ORC_ConteudoItem entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ORC_ConteudoItem entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ORC_ConteudoItem entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ORC_ConteudoItem entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ORC_ConteudoItem entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ORC_ConteudoItem entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ORC_ConteudoItem entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ORC_ConteudoItem entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ORC_ConteudoItem entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ORC_ConteudoItem> Select()
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
        //public override IList<ORC_ConteudoItem> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ORC_ConteudoItem entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ORC_ConteudoItem DataRowToEntity(DataRow dr, ORC_ConteudoItem entity)
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
        //public override ORC_ConteudoItem DataRowToEntity(DataRow dr, ORC_ConteudoItem entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}