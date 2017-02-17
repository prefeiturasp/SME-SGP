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
	/// 
	/// </summary>
	public class CFG_ParametroDocumentoAlunoDAO : Abstract_CFG_ParametroDocumentoAlunoDAO
	{

        /// <summary>
        /// Retorna todos os parâmetros cadastrados.
        /// </summary>
        /// <returns></returns>
        public IList<CFG_ParametroDocumentoAluno> Seleciona()
        {
            IList<CFG_ParametroDocumentoAluno> lt = new List<CFG_ParametroDocumentoAluno>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_ParametroDocumentoAluno_Select", _Banco);

            qs.Execute();

            foreach (DataRow dr in qs.Return.Rows)
            {
                CFG_ParametroDocumentoAluno entity = new CFG_ParametroDocumentoAluno();
                lt.Add(this.DataRowToEntity(dr, entity));
            }
            return lt;
        }

        /// <summary>
        /// Retorna os dados da tabela CFG_ParametroDocumentoAluno filtrados pelo campo pda_chave, ent_id.
        /// </summary>
        /// <param name="pda_chave">Nome da chave do parâmetro.</param>
        /// <param name="ent_id">Id da entidade da tabela CFG_ParametroDocumentoAluno.</param>
        /// <returns>Lista com objetos CFG_ParametroDocumentoAluno.</returns>
        public virtual IList<CFG_ParametroDocumentoAluno> SelectByChave(string pda_chave, Guid ent_id)
        {
            IList<CFG_ParametroDocumentoAluno> lt = new List<CFG_ParametroDocumentoAluno>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_ParametroDocumentoAluno_SELECTBY_Chave", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pda_chave";
                Param.Size = 100;
                Param.Value = pda_chave;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                foreach (DataRow dr in qs.Return.Rows)
                {
                    CFG_ParametroDocumentoAluno entity = new CFG_ParametroDocumentoAluno();
                    lt.Add(this.DataRowToEntity(dr, entity));
                }
                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        /// <summary>
        /// Retorna os dados da tabela CFG_ParametroDocumentoAluno filtrados pelo campo pda_chave, ent_id.
        /// MÉTODO(S) DEPENDENTE(S):
        /// 1 - Classe: CFG_ParametroDocumentoAlunoBO; Método: ParametroValor
        /// </summary>
        /// <param name="pda_chave">Nome da chave do parâmetro.</param>
        /// <param name="ent_id">Id da entidade da tabela CFG_ParametroDocumentoAluno.</param>
        /// <returns>Instância do objeto carregado com os valores da chave.</returns>        
        public virtual CFG_ParametroDocumentoAluno CarregarPorChave(string pda_chave, Guid ent_id)
        {
            CFG_ParametroDocumentoAluno entity = new CFG_ParametroDocumentoAluno();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_ParametroDocumentoAluno_LOADBY_Chave", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pda_chave";
                Param.Size = 100;
                Param.Value = pda_chave;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                if (qs.Return.Rows.Count > 0)
                    entity = this.DataRowToEntity(qs.Return.Rows[0], entity, false);
                return entity;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public virtual CFG_ParametroDocumentoAluno CarregarPorChaveRelatorio(string pda_chave, Guid ent_id, int rlt_id)
        {
            CFG_ParametroDocumentoAluno entity = new CFG_ParametroDocumentoAluno();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_ParametroDocumentoAluno_LOADBY_ChaveRelatorio", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pda_chave";
                Param.Size = 100;
                Param.Value = pda_chave;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rlt_id";
                Param.Value = rlt_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                if (qs.Return.Rows.Count > 0)
                    entity = this.DataRowToEntity(qs.Return.Rows[0], entity, false);
                return entity;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public string SelectNomeDocumentoPorRelatorio(int rlt_id)
        {
            CFG_ParametroDocumentoAluno entity = new CFG_ParametroDocumentoAluno();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_ParametroDocumentoAluno_CarregaNomeDocumentoPorRelatorio", this._Banco);

            try
            {
                qs.Execute();

                return Convert.ToString(qs.Return.Rows[0]);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_ParametroDocumentoAluno entity)
        {
            entity.pda_dataCriacao = DateTime.Now;
            entity.pda_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CFG_ParametroDocumentoAluno entity)
        {
            entity.pda_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@pda_dataCriacao");
        }

        protected override bool Alterar(CFG_ParametroDocumentoAluno entity)
        {
            __STP_UPDATE = "NEW_CFG_ParametroDocumentoAluno_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CFG_ParametroDocumentoAluno entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pda_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pda_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(CFG_ParametroDocumentoAluno entity)
        {
            __STP_DELETE = "NEW_CFG_ParametroDocumentoAluno_UpdateSituacao";
            return base.Delete(entity);
        }

        public override IList<CFG_ParametroDocumentoAluno> Select()
        {
            __STP_SELECT = "NEW_CFG_ParametroDocumentoAluno_Select";
            return base.Select();
        }

        #endregion

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(CFG_ParametroDocumentoAluno entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(CFG_ParametroDocumentoAluno entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(CFG_ParametroDocumentoAluno entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(CFG_ParametroDocumentoAluno entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, CFG_ParametroDocumentoAluno entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, CFG_ParametroDocumentoAluno entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, CFG_ParametroDocumentoAluno entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_ParametroDocumentoAluno entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(CFG_ParametroDocumentoAluno entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<CFG_ParametroDocumentoAluno> Select()
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
        //public override IList<CFG_ParametroDocumentoAluno> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CFG_ParametroDocumentoAluno entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override CFG_ParametroDocumentoAluno DataRowToEntity(DataRow dr, CFG_ParametroDocumentoAluno entity)
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
        //public override CFG_ParametroDocumentoAluno DataRowToEntity(DataRow dr, CFG_ParametroDocumentoAluno entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}
	}
}