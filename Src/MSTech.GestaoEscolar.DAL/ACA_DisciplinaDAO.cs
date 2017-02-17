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

    /// <summary>
    /// 
    /// </summary>
    public class ACA_DisciplinaDAO : Abstract_ACA_DisciplinaDAO
    {
        /// <summary>
        /// BD:GestaoEscolar / TB:ACA_Disciplina
        /// -Seleção de todos os registros relacionados com
        /// unidade, escola, docente e turno.
        /// </summary>
        /// <returns></returns>
        public DataTable SelectBy_Escola_Docente_Turno
        (
            int uni_id,
            int esc_id,
            int doc_id,
            int trn_id
        )
        {
            //***Metodo do Quadro de Preferencia            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_SelectBy_escola_docente_turno", _Banco);
            try
            {
                #region PARAMETROS

                //Unidade
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 100;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                //Escola
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 100;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                //Docente
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@doc_id";
                Param.Size = 100;
                if (doc_id > 0)
                    Param.Value = doc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                //Turno
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 100;
                if (trn_id > 0)
                    Param.Value = trn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();

                return qs.Return;
            }//fim try
            catch
            {
                throw;
            }//fim catch
            finally
            {
                qs.Parameters.Clear();
            }//fim finally
        }//fim SelectBy_Escola_Docente_Turno

        ///<sumary>
        /// Retorna as disciplinas pelo tipo
        /// </sumary>
        /// <param name="tds_id"> Id da tabela ACA_TipoDisciplina</param>
        /// <returns>Datatable com as disciplinas</returns>
        public DataTable SelectBy_tds_id
        (
           int tds_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_SelectBy_tds_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                if (tds_id > 0)
                    Param.Value = tds_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna as disciplinas do tipo informado, cadastradas para o curso.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="dis_situacao">Situacao da disciplina</param>
        /// <returns></returns>
        public DataTable SelectBy_Tipo_Curso
        (
            int cur_id
            , int crr_id
            , int tds_id
            , byte dis_situacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_SelectBy_Tipo_Curso", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cur_id";
            Param.Size = 4;
            Param.Value = cur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crr_id";
            Param.Size = 4;
            Param.Value = crr_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tds_id";
            Param.Size = 4;
            Param.Value = tds_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@dis_situacao";
            Param.Size = 1;
            if (dis_situacao > 0)
                Param.Value = dis_situacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as disciplinas do tipo informado, cadastradas para o curso.
        /// E tenha períodos compatíveis com a escola.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="dis_situacao">Situacao da disciplina</param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <returns></returns>
        public DataTable SelectBy_Tipo_CursoPeriodo
        (
            int cur_id
            , int crr_id
            , int tds_id
            , byte dis_situacao
            , int esc_id
            , int uni_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_SelectBy_Tipo_CursoPeriodo", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cur_id";
            Param.Size = 4;
            Param.Value = cur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crr_id";
            Param.Size = 4;
            Param.Value = crr_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tds_id";
            Param.Size = 4;
            Param.Value = tds_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@dis_situacao";
            Param.Size = 1;
            if (dis_situacao > 0)
                Param.Value = dis_situacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            Param.Value = esc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@uni_id";
            Param.Size = 4;
            Param.Value = uni_id;
            qs.Parameters.Add(Param);


            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as disciplinas do tipo informado, cadastradas para o curso.
        /// E tenha períodos compatíveis com a escola.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crd_tipo">Tipo de disciplina vindo do enum (ACA_CurriculoDisciplinaBO)</param>
        /// <param name="dis_situacao">Situacao da disciplina</param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <returns></returns>
        public DataTable SelectBy_TipoDisciplinaEnum_CursoPeriodo
        (
            int cur_id
            , int crr_id
            , int crp_id
            , byte crd_tipo
            , byte dis_situacao
            , int esc_id
            , int uni_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_SelectBy_TipoDisciplinaEnum_CursoPeriodo", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@crp_id";
            Param.Size = 4;
            if (crp_id > 0)
                Param.Value = crp_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@crd_tipo";
            Param.Size = 1;
            Param.Value = crd_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@dis_situacao";
            Param.Size = 1;
            if (dis_situacao > 0)
                Param.Value = dis_situacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            Param.Value = esc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@uni_id";
            Param.Size = 4;
            Param.Value = uni_id;
            qs.Parameters.Add(Param);


            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Verifica se o TipoDisciplina está na tabela Disciplina e CurriculoDisciplina.
        /// </summary>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <returns></returns>
        public DataTable Verifica_TipoDisciplina
        (
            int tds_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_VerificaTipoDisciplina", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tds_id";
            Param.Size = 4;
            Param.Value = tds_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        public List<ACA_Disciplina> GetSelectBy_DisIds(string ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_SelectBy_DisIds", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@ids";
            Param.Value = ids;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return (from DataRow dr in qs.Return.Rows select DataRowToEntity(dr, new ACA_Disciplina())).ToList();
        }

        ///<sumary>
        /// Retorna as disciplinas eletivas do aluno: crd_tipo = 10
        /// </sumary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo do curso</param>
        /// <returns>Datatable com as disciplinas</returns>
        public DataTable SelectBy_EletivasAlunos
        (
            int cur_id
            , int crr_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_SelectBy_EletivasAlunos", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
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
        /// Retorna as disciplinas eletivas dos alunos matriculados na turma selecionada 
        /// (ulitizado na tela de lançamento de frequência mensal)
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_ids"></param>
        /// <param name="tpc_id"></param>
        /// <returns>DataTable de disciplinas eletivas</returns>
        public DataTable SelectEletivasAlunosByTurma(long tur_id, string alu_ids, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_SelectEletivasAlunosByTurma", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_ids";
                Param.Value = alu_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
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
        /// Seleciona as disciplinas por tipo da disciplina na grade curricular.
        /// </summary>
        /// <param name="crd_tipo">Tipo de disciplina.</param>
        /// <returns></returns>
        public List<ACA_Disciplina> SelecionaPorTipoGradeCurricular(byte crd_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_SelecionaPorTipoGradeCurricular", _Banco);

            try
            {
                #region Parametro

                Param = qs.NewParameter();
                Param.ParameterName = "@crd_tipo";
                Param.DbType = DbType.Byte;
                Param.Size = 1;
                Param.Value = crd_tipo;
                qs.Parameters.Add(Param);

                #endregion Parametro

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new ACA_Disciplina())).ToList() :
                    new List<ACA_Disciplina>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }


        /// <summary>
        /// Verifica se já foi cadastrada algum disciplina com o mesmo codigo e tipo
        /// </summary>	    
        /// <param name="dis_id"></param>
        /// <param name="dis_codigo"></param>
        /// <param name="tds_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <returns>True | False</returns>
        public bool VerificaExistentePorCodigoTipo(int dis_id, string dis_codigo, int tds_id, int cur_id, int crr_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Disciplina_VerificaExistentePorCodigoTipo", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@dis_id";
                Param.Size = 4;
                if (dis_id > 0)
                    Param.Value = dis_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@dis_codigo";
                Param.Size = 10;
                Param.Value = dis_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
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

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Disciplina entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@dis_ementa"].DbType = DbType.String;
            qs.Parameters["@dis_ementa"].Size = Int32.MaxValue;

            qs.Parameters["@dis_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@dis_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Disciplina entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@dis_ementa"].DbType = DbType.String;
            qs.Parameters["@dis_ementa"].Size = Int32.MaxValue;

            qs.Parameters.RemoveAt("@dis_dataCriacao");
            qs.Parameters["@dis_dataAlteracao"].Value = DateTime.Now;
        }


        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_Disciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(ACA_Disciplina entity)
        {
            __STP_UPDATE = "NEW_ACA_Disciplina_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Disciplina entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@dis_id";
            Param.Size = 4;
            Param.Value = entity.dis_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@dis_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@dis_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_Disciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(ACA_Disciplina entity)
        {
            __STP_DELETE = "NEW_ACA_Disciplina_Update_Situacao";
            return base.Delete(entity);
        }

        #region Métodos comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_Disciplina entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_Disciplina entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_Disciplina entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_Disciplina entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Disciplina entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Disciplina entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Disciplina entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Disciplina entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_Disciplina entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_Disciplina> Select()
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
        //public override IList<ACA_Disciplina> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Disciplina entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_Disciplina DataRowToEntity(DataRow dr, ACA_Disciplina entity)
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
        //public override ACA_Disciplina DataRowToEntity(DataRow dr, ACA_Disciplina entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion Métodos comentados
    }
}