/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
    using System.Data.SqlClient;

    /// <summary>
	/// 
	/// </summary>
	public class CLS_TurmaNotaAlunoDAO : Abstract_CLS_TurmaNotaAlunoDAO
	{
	    /// <summary>
	    /// Retorna todas as entidades da CLS_TurmaNotaAluno
	    /// de todos os alunos matriculados na disicplina, para as atividades informadas (tnt_id).
	    /// </summary>                
	    /// <param name="tud_id">ID da disciplina da turma</param>        
	    /// <param name="tnt_id">ID das atividades da disciplina da turma</param>
	    /// <param name="listaTurmaNota">Lista da entidade CLS_TurmaNota</param>
	    public List<CLS_TurmaNotaAluno> SelectBy_Disciplina_Atividades
        (
            long tud_id
            , string tnt_id
            , out List<CLS_TurmaNota> listaTurmaNota
        )
        {
            if (!string.IsNullOrEmpty(tnt_id))
            {
                QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNotaAluno_SelectBy_Disciplina_Atividades", _Banco);

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tnt_id";
                Param.Value = tnt_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                List<CLS_TurmaNotaAluno> lista =
                    (from DataRow dr in qs.Return.Rows
                     select DataRowToEntity(dr, new CLS_TurmaNotaAluno())).ToList();

                listaTurmaNota =
                    (from DataRow dr in qs.Return.Rows
                     select new CLS_TurmaNotaDAO().DataRowToEntity(dr, new CLS_TurmaNota())
                    ).ToList().Distinct().ToList();

                return lista;
            }
            else
            {
                listaTurmaNota = new List<CLS_TurmaNota>();
                return new List<CLS_TurmaNotaAluno>();
            }
        }

        /// <summary>
        /// Retorna os dados da CLS_TurmaNotaAluno que sejam pela 
        /// "chave" da matrícula do aluno na disciplina.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="mtd_id">Id da matrícula do aluno na disciplina - obrigatório</param>
        /// <returns>Lista de CLS_TurmaNotaAluno</returns>
        public List<CLS_TurmaNotaAluno> SelectBy_Disciplina_Aluno
        (
            Int64 tud_id
            , Int64 alu_id
            , Int32 mtu_id
            , Int32 mtd_id
        )
        {
            List<CLS_TurmaNotaAluno> lista = new List<CLS_TurmaNotaAluno>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNotaAluno_SelectBy_Disciplina_Aluno", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
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

            DataTable dt = qs.Return;

            foreach (DataRow dr in dt.Rows)
            {
                CLS_TurmaNotaAluno entity = new CLS_TurmaNotaAluno();
                entity = DataRowToEntity(dr, entity);

                lista.Add(entity);
            }

            return lista;
        }

       
        /// <summary>
        /// Retorna o lançamento de notas dos alunos da atividade passada
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="tnt_id">ID da atividade da disciplina da turma</param>       
        public DataTable BuscaNotasDaAtividade
        (
            long tud_id
            , int tnt_id            
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaNotasDaAtividade", _Banco);
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
                Param.ParameterName = "@tnt_id";
                Param.Size = 4;
                Param.Value = tnt_id;
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
        /// Retorna o lançamento de notas dos alunos que não foram excluídos logicamente
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="tnt_id">ID da atividade da disciplina da turma</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="ordenacao">Tipo de ordenação dos alunos 0-Número chamada, 1-Nome do aluno</param>
        public DataTable SelectBy_TurmaDisciplina
        (
            long tud_id
            , int tnt_id
            , Guid ent_id
            , byte ordenacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNotaAluno_SelectBy_TurmaDisciplina", _Banco);
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
                Param.ParameterName = "@tnt_id";
                Param.Size = 4;
                Param.Value = tnt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@ordenacao";
                Param.Size = 1;
                Param.Value = ordenacao;
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
	    /// Retorna o lançamento de notas dos alunos, filtrando pelo período.
	    /// </summary>                
	    /// <param name="tud_id">ID da disciplina da turma</param>        
	    /// <param name="tnt_id">ID da avaliativa da disciplina da turma</param>
	    /// <param name="tpc_id"></param>
	    /// <param name="ent_id">Entidade do usuário logado</param>
	    /// <param name="ordenacao">Tipo de ordenação dos alunos 0-Número chamada, 1-Nome do aluno</param>
	    /// <param name="trazerInativos"></param>
	    public DataTable SelectBy_TurmaDisciplinaPeriodo
        (
            long tud_id
            , int tnt_id
            , int tpc_id
            , Guid ent_id
            , byte ordenacao
            , bool trazerInativos
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNotaAluno_SelectBy_TurmaDisciplinaPeriodo", _Banco);
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
                Param.ParameterName = "@tnt_id";
                Param.Size = 4;
                Param.Value = tnt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@ordenacao";
                Param.Size = 1;
                Param.Value = ordenacao;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazerInativos";
                Param.Size = 1;
                Param.Value = trazerInativos;
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
        /// Retorna o lançamento de notas dos alunos, filtrando pelo período.
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="tnt_id">ID da avaliativa da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tdc_id">ID do tipo de docente.</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="ordenacao">Tipo de ordenação dos alunos 0-Número chamada, 1-Nome do aluno</param>
        /// <param name="trazerInativos"></param>
        public DataTable SelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia
        (
            long tud_id
            , int tnt_id
            , int tpc_id
            , byte tdc_id
            , Guid ent_id
            , byte ordenacao
            , bool trazerInativos
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNotaAluno_SelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia", _Banco);
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
                Param.ParameterName = "@tnt_id";
                Param.Size = 4;
                Param.Value = tnt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@ordenacao";
                Param.Size = 1;
                Param.Value = ordenacao;
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazerInativos";
                Param.Size = 1;
                Param.Value = trazerInativos;
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
        /// Salva os dados das notas dos alunos.
        /// </summary>
        /// <param name="dtTurmaNotaAluno">DataTable de dados do listão de avaliação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvaNotaAlunos(DataTable dtTurmaNotaAluno)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaNotaAluno_SalvaNotaAlunos", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaNotaAluno";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaNotaAluno";
                sqlParam.Value = dtTurmaNotaAluno;
                qs.Parameters.Add(sqlParam);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaNotaAluno entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tna_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tna_dataAlteracao"].Value = DateTime.Now;

            qs.Parameters["@tna_relatorio"].DbType = DbType.String;
            qs.Parameters["@tna_relatorio"].Size = Int32.MaxValue;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaNotaAluno entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tna_dataCriacao");
            qs.Parameters["@tna_dataAlteracao"].Value = DateTime.Now;

            qs.Parameters["@tna_relatorio"].DbType = DbType.String;
            qs.Parameters["@tna_relatorio"].Size = Int32.MaxValue;

        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaNotaAluno</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(CLS_TurmaNotaAluno entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaNotaAluno_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaNotaAluno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = entity.tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tnt_id";
            Param.Size = 4;
            Param.Value = entity.tnt_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mtu_id";
            Param.Size = 4;
            Param.Value = entity.mtu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mtd_id";
            Param.Size = 4;
            Param.Value = entity.mtd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tna_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tna_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaNotaAluno</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        public override bool Delete(CLS_TurmaNotaAluno entity)
        {
            __STP_DELETE = "NEW_CLS_TurmaNotaAluno_Update_Situacao";
            return base.Delete(entity);
        }

		///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(CLS_TurmaNotaAluno entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(CLS_TurmaNotaAluno entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(CLS_TurmaNotaAluno entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(CLS_TurmaNotaAluno entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaNotaAluno entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TurmaNotaAluno entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaNotaAluno entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaNotaAluno entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(CLS_TurmaNotaAluno entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<CLS_TurmaNotaAluno> Select()
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
        //public override IList<CLS_TurmaNotaAluno> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaNotaAluno entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override CLS_TurmaNotaAluno DataRowToEntity(DataRow dr, CLS_TurmaNotaAluno entity)
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
        //public override CLS_TurmaNotaAluno DataRowToEntity(DataRow dr, CLS_TurmaNotaAluno entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}
	}
}