using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using System.Data.SqlClient;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DCL_ProtocoloDAO : Abstract_DCL_ProtocoloDAO
    {
        #region Consultas

        /// <summary>
        /// Retorna a turma se ela pertencer ao ano letivo corrente e estiver ativa.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="tud_id">id da turmaDisciplina</param>
        /// <returns></returns>
        public DataTable SelecionarTurmaAtivaAnoCorrente(long tur_id, long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_Protocolo_SelectTurmaAnoCorrente", this._Banco);

            try
            {

                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.ParameterName = "@tur_id";

                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;

                qs.Parameters.Add(Param);


                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.ParameterName = "@tud_id";

                if (tud_id > 0)
                    Param.Value = tud_id;
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
        /// Carrega a lista de protocolos segundo os números de protocolo.
        /// </summary>
        /// <param name="pro_protocolo">Número do Protocolo</param>
        /// <returns></returns>
        public DataTable SelectBy_Protocolos(string pro_protocolo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_Protocolo_SelectBy_Protocolo", this._Banco);
            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@pro_protocolo";
            Param.Value = pro_protocolo;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Carrega o protocolo segundo a entidade e o número de protocolo
        /// </summary>
        /// <param name="ent_id">Id da Entidade</param>
        /// <param name="pro_protocolo">Número do Protocolo</param>
        /// <returns></returns>
        public DCL_Protocolo CarregarBy_Protocolo(long pro_protocolo, Guid ent_id)
        {
            DCL_Protocolo entity = new DCL_Protocolo();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("STP_DCL_Protocolo_SelectBy_Protocolo", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@pro_protocolo";
                Param.Size = 8;
                Param.Value = pro_protocolo;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity);
                }

                return (entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna os protocolos pendentes de processamento.
        /// </summary>
        /// <returns></returns>
        public DataTable SelectNaoProcessados()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_Protocolo_SelecionaNaoProcessados", this._Banco);

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os protocolos de acordo com a entidade e o período.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="dtInicio">Data de início</param>
        /// <param name="dtFim">Data de fim</param>
        /// <param name="status">Situação do protocolo</param>
        /// <param name="tipoProtocolo">Tipo do Protocolo</param>
        /// <returns></returns>
        public DataTable SelectBy_EntidadeData(Guid ent_id, DateTime dtInicio, DateTime dtFim, byte status, byte tipoProtocolo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_Protocolo_SelectBy_ent_id_data", this._Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pro_dataInicio";
            Param.Size = 16;
            Param.Value = dtInicio;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pro_dataFim";
            Param.Size = 16;
            Param.Value = dtFim;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@status";
            Param.Size = 2;
            if (status > 0)
                Param.Value = status;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pro_tipo";
            Param.Size = 2;
            if (tipoProtocolo > 0)
                Param.Value = tipoProtocolo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// retorna os protocolos vinculados a escola a partir de uma data especifica podendo filtrar pelo tipo do protocolo
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos protocolos</param>
        /// <param name="pro_tipo">tipo do protocolo</param>
        /// <returns></returns>
        public DataTable SelectBy_Escola(Int32 esc_id, DateTime dataBase, int pro_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_Protocolo_SelectBy_Escola", this._Banco);

            #region parametros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            Param.Value = esc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@dataBase";
            Param.Size = 16;
            Param.Value = dataBase;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pro_tipo";
            Param.Size = 2;
            if (pro_tipo > 0)
                Param.Value = pro_tipo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona uma quantidade de protocolos filtrados pelo tipo.
        /// </summary>
        /// <param name="pro_tipo">Tipo do protocolo.</param>
        /// <param name="qtdeProtocolo">Quantidade máxima de protocolos.</param>
        /// <returns></returns>
        public List<DCL_Protocolo> SelecionaNaoProcessadosPorTipo(byte pro_tipo, int qtdeProtocolo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_Protocolo_SelecionaNaoProcessadosPorTipo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@pro_tipo";
                Param.Size = 1;
                Param.Value = pro_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qtdeProtocolo";
                Param.Size = 4;
                Param.Value = qtdeProtocolo;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Cast<DataRow>()
                                     .Select(dr => DataRowToEntity(dr, new DCL_Protocolo())).ToList();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Salvar

        /// <summary>
        /// Atualiza os protocolos passados por parâmetro em uma tabela.
        /// </summary>
        /// <param name="dtProtocolo">Tabela de protocolos.</param>
        /// <returns></returns>
        public bool AtualizaProtocolos(DataTable dtProtocolo)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_DCL_Protocolo_AtualizaProtocolos", _Banco);

            try
            {
                #region Parametro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@tbProtocolo";
                sqlParam.TypeName = "TipoTabela_Protocolo";
                sqlParam.Value = dtProtocolo;
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

        #endregion

        #region Sobrescritos

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// Não passa o pro_protocolo, pois ele é autoincremento.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, DCL_Protocolo entity)
        {
            // o protocolo pode ter sido criado em outro lugar e esta sincronizando com o SGP pela API
            // quando isto ocorrer o id e as datas vao vir preenchido.
            if (entity.pro_id.Equals(Guid.Empty))
                entity.pro_id = Guid.NewGuid();

            if (entity.pro_dataCriacao.Equals(new DateTime()))
                entity.pro_dataCriacao = DateTime.Now;

            if (entity.pro_dataalteracao.Equals(new DateTime()))
                entity.pro_dataalteracao = DateTime.Now;


            base.ParamInserir(qs, entity);

            qs.Parameters.RemoveAt("@pro_protocolo");

            if (entity.equ_id == Guid.Empty)
            {
                qs.Parameters["@equ_id"].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// Altera a procedure de insert, pois a procedure não tem o parâmetro pro_protocolo
        /// (campo é autoincremento).
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override bool Inserir(DCL_Protocolo entity)
        {
            __STP_INSERT = "NEW_DCL_Protocolo_INSERT";
            return base.Inserir(entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, DCL_Protocolo entity)
        {
            entity.pro_dataalteracao = DateTime.Now;

            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@pro_protocolo");
            qs.Parameters.RemoveAt("@pro_dataCriacao");
        }

        protected override bool Alterar(DCL_Protocolo entity)
        {
            __STP_UPDATE = "NEW_DCL_Protocolo_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, DCL_Protocolo entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pro_situacao";
            Param.Size = 1;
            Param.Value = entity.pro_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pro_dataalteracao";
            Param.Size = 16;
            Param.Value = entity.pro_dataalteracao;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(DCL_Protocolo entity)
        {
            __STP_DELETE = "NEW_DCL_Protocolo_UpdateSituacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, DCL_Protocolo entity)
        {
            if (entity != null & qs != null)
            {
                entity.pro_protocolo = Convert.ToInt64(qs.Return.Rows[0][0]);
                return entity.pro_protocolo > 0;
            }

            return false;
        }


        /// <summary>
        /// Retorna inf. detalhadas da dt. aula/escola/turma/professor vinculadas ao protocolo (pro_id)
        /// </summary>
        /// <param name="pro_id">ID do protocolo</param>
        /// <returns></returns>
        public DataTable SelectBy_Protocolo_TurmaAula(Guid pro_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_DCL_Protocolo", this._Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 36;
            Param.Value = pro_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna inf. detalhadas da Escola/turma/Disciplina vinculadas ao protocolo (pro_id)
        /// </summary>
        /// <param name="pro_id">ID do protocolo</param>
        /// <returns></returns>
        public DataTable SelectBy_Protocolo_TurmaDisciplinaPlanejamento(Guid pro_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_SelectBy_DCL_Protocolo", this._Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 36;
            Param.Value = pro_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna inf. detalhadas da Nome aluno/Matricula/Escola/Turma/Grupamento de ensino vinculadas ao protocolo (pro_id)
        /// </summary>
        /// <param name="pro_id">ID do protocolo</param>
        /// <returns></returns>
        public DataTable SelectBy_Protocolo_ProtocoloAluno(Guid pro_id, bool matriculaEstadual)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_ProtocoloAluno_SelectBy_DCL_Protocolo", this._Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 36;
            Param.Value = pro_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@matriculaEstadual";
            Param.Size = 1;
            Param.Value = matriculaEstadual;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna inf. detalhadas Nome Professor, Disciplina, Qtde de aulas compensadas, ativ. desenvolvida no vinculadas 
        /// ao protocolo (pro_id)
        /// </summary>
        /// <param name="pro_id">ID do protocolo</param>
        /// <returns></returns>
        public DataTable SelectBy_Protocolo_CompensacaoDeAula(Guid pro_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_CompensacaoAusencia_SelectBy_DCL_Protocolo", this._Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 36;
            Param.Value = pro_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        #endregion
    }
}
