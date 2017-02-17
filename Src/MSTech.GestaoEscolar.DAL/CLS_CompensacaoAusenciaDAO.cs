/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Data;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_CompensacaoAusenciaDAO : AbstractCLS_CompensacaoAusenciaDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Seleciona os avisos.
        /// </summary>
        /// <param name="uad_idSuperior">The uad_id superior.</param>
        /// <param name="esc_id">The esc_id.</param>
        /// <param name="uni_id">The uni_id.</param>
        /// <param name="cur_id">The cur_id.</param>
        /// <param name="crr_id">The crr_id.</param>
        /// <param name="cap_id">The cap_id.</param>
        /// <param name="tud_id">The tud_id.</param>
        /// <param name="gru_id">The gru_id.</param>
        /// <param name="usu_id">The usu_id.</param>
        /// <param name="adm">if set to <c>true</c> [adm].</param>
        /// <returns></returns>
        public DataTable SelectByPesquisa(Guid uad_idSuperior, int esc_id, int uni_id, int cur_id, int crr_id, int cap_id, long tud_id, Guid gru_id, Guid usu_id, bool adm, long tur_id, int tev_idEfetivacao, out int totalRecords)
        {
            totalRecords = 0;
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_CompensacaoAusencia_SelectByPesquisa", _Banco);
            try
            {

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (crr_id > 0)
                    Param.Value = crr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cap_id";
                Param.Size = 4;
                if (cap_id > 0)
                    Param.Value = cap_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_id";
                Param.Size = 4;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 16;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_id";
                Param.Size = 4;
                Param.Value = tev_idEfetivacao;
                qs.Parameters.Add(Param);

                #endregion

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
        /// Pesquisa da tela de compensação de ausência.
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior da escola.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="cap_id">ID do período do calendário.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="gru_id">ID do grupo do usuário logado.</param>
        /// <param name="usu_id">ID do usuário logado.</param>
        /// <param name="adm">Flag que indica se o usuário é administrador do sistema.</param>
        /// <param name="tev_idEfetivacao">ID do tipo de evento da efetivação.</param>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tdc_id">ID do tipo de docente.</param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public DataTable SelectByPesquisaFiltroDeficiencia(Guid uad_idSuperior, int esc_id, int uni_id, int cur_id, int crr_id, int cap_id, long tud_id, Guid gru_id, Guid usu_id, bool adm, long tur_id, int tev_idEfetivacao, byte tdc_id, out int totalRecords)
        {
            totalRecords = 0;
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_CompensacaoAusencia_SelectByPesquisaFiltroDeficiencia", _Banco);
            try
            {

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (crr_id > 0)
                    Param.Value = crr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cap_id";
                Param.Size = 4;
                if (cap_id > 0)
                    Param.Value = cap_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_id";
                Param.Size = 4;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 16;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_id";
                Param.Size = 4;
                Param.Value = tev_idEfetivacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
                qs.Parameters.Add(Param);

                #endregion

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
        /// Retorna todos os ids necessários para carregar os combos da tela de cadastro.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="cpa_id"></param>
        /// <returns></returns>
        public DataTable RetornaIdsCadastro(long tud_id, int cpa_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_CompensacaoAusencia_RetornaIdsCadastro", _Banco);
            try
            {

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cpa_id";
                Param.Size = 4;
                Param.Value = cpa_id;
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
        /// Seleciona compensações de ausência para o aluno, segundo parâmetros.
        /// </summary>
        /// <param name="tud_id">ID disciplina da turma</param>
        /// <param name="tpc_id">ID tipo de período do calendário</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula</param>
        /// <param name="mtd_id">ID matrícula-disciplina</param>
        /// <param name="totalRecords">Total de linhas</param>
        /// <returns>Datatable com dados</returns>
        public DataTable SelecionaPorAluno
        (
            long tud_id
            , int tpc_id
            , long alu_id
            , int mtu_id
            , int mtd_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_CompensacaoAusencia_SelecionaPorAluno", _Banco);
            try
            {

                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (tpc_id > 0)
                    Param.Value = tpc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtd_id";
                Param.Size = 4;
                Param.Value = mtd_id;
                qs.Parameters.Add(Param);

                #endregion

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
        /// Retorna compensação de ausência filtrado por disciplina e protocolo de criação.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="pro_id"></param>
        /// <returns></returns>
        public CLS_CompensacaoAusencia SelectByDisciplinaProtocolo(long tud_id, Guid pro_id)
        {
            CLS_CompensacaoAusencia entity = new CLS_CompensacaoAusencia();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_CompensacaoAusencia_SelectByDisciplinaProtocolo", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                Param.Value = pro_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                
                if (qs.Return.Rows.Count > 0)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity, false);
                    entity.IsNew = false;
                }
                else
                {
                    entity.tud_id = tud_id;
                    entity.IsNew = true;
                }

                return entity;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de consulta

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, Entities.CLS_CompensacaoAusencia entity)
        {
            entity.cpa_dataCriacao = DateTime.Now;
            entity.cpa_dataAlteracao = DateTime.Now;
            entity.cpa_situacao = 1;
            base.ParamInserir(qs, entity);

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@pro_id"].Value = DBNull.Value;
            }
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, Entities.CLS_CompensacaoAusencia entity)
        {
            entity.cpa_dataAlteracao = DateTime.Now;
            entity.cpa_situacao = 1;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@cpa_dataCriacao");

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@pro_id"].Value = DBNull.Value;
            }
        }

        protected override bool Alterar(Entities.CLS_CompensacaoAusencia entity)
        {
            __STP_UPDATE = "NEW_CLS_CompensacaoAusencia_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, Entities.CLS_CompensacaoAusencia entity)
        {
            base.ParamDeletar(qs, entity);
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@cpa_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@cpa_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(Entities.CLS_CompensacaoAusencia entity)
        {
            __STP_DELETE = "NEW_CLS_CompensacaoAusencia_UPDATE_Situacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity"></param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, Entities.CLS_CompensacaoAusencia entity)
        {
            entity.cpa_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.cpa_id > 0);
        }

        #endregion Sobrescritos

        #region Métodos Comentados
        
        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // protected override bool Alterar(CLS_CompensacaoAusencia entity)
        // {
        //    return base.Alterar(entity);
        // }
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // protected override bool Inserir(CLS_CompensacaoAusencia entity)
        // {
        //    return base.Inserir(entity);
        // }
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Carregar(CLS_CompensacaoAusencia entity)
        // {
        //    return base.Carregar(entity);
        // }
        ///// <summary>
        ///// Exclui um registro do banco.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Delete(CLS_CompensacaoAusencia entity)
        // {
        //    return base.Delete(entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamAlterar(QueryStoredProcedure qs, CLS_CompensacaoAusencia entity)
        // {
        //    base.ParamAlterar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_CompensacaoAusencia entity)
        // {
        //    base.ParamCarregar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamDeletar(QueryStoredProcedure qs, CLS_CompensacaoAusencia entity)
        // {
        //    base.ParamDeletar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_CompensacaoAusencia entity)
        // {
        //    base.ParamInserir(qs, entity);
        // }
        ///// <summary>
        ///// Salva o registro no banco de dados.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Salvar(CLS_CompensacaoAusencia entity)
        // {
        //    return base.Salvar(entity);
        // }
        ///// <summary>
        ///// Realiza o select da tabela.
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela.</returns>
        // public override IList<CLS_CompensacaoAusencia> Select()
        // {
        //    return base.Select();
        // }
        ///// <summary>
        ///// Realiza o select da tabela com paginacao.
        ///// </summary>
        ///// <param name="currentPage">Pagina atual.</param>
        ///// <param name="pageSize">Tamanho da pagina.</param>
        ///// <param name="totalRecord">Total de registros na tabela original.</param>
        ///// <returns>Lista com todos os registros da p�gina.</returns>
        // public override IList<CLS_CompensacaoAusencia> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        // {
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        // }
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade. 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_CompensacaoAusencia entity)
        // {
        //    return base.ReceberAutoIncremento(qs, entity);
        // }
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade.
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido.</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados.</param>
        ///// <returns>Entidade preenchida.</returns>
        // public override CLS_CompensacaoAusencia DataRowToEntity(DataRow dr, CLS_CompensacaoAusencia entity)
        // {
        //    return base.DataRowToEntity(dr, entity);
        // }
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade.
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido.</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados.</param>
        ///// <param name="limparEntity">Indica se a entidade deve ser limpada antes da transferencia.</param>
        ///// <returns>Entidade preenchida.</returns>
        // public override CLS_CompensacaoAusencia DataRowToEntity(DataRow dr, CLS_CompensacaoAusencia entity, bool limparEntity)
        // {
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        // }

        #endregion Métodos Comentados
    }
}