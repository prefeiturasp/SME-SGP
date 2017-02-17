using System;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Data;
using MSTech.GestaoEscolar.Entities;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.DAL
{
    public class ACA_TipoPeriodoCalendarioDAO : Abstract_ACA_TipoPeriodoCalendarioDAO
    {
        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente
        /// </summary>                
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>   
        public DataTable SelectBy_Pesquisa
        (
            bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectBy_Pesquisa", _Banco);
            try
            {
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
        /// Retorna os tipos de período do calendário cadastrados no calendário.
        /// Quando informada a turma (tur_id), traz todas as marcações por tud_id
        /// em cada período do calendário.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tur_id">ID da turma</param>
        public DataTable SelectBy_Calendario_MarcacaoTurmas
        (
            int cal_id
            , long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectBy_Calendario_MarcacaoTurmas", _Banco);

            #region PARAMETROS
            
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            if (tur_id > 0)
                Param.Value = tur_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Carrega os tipos de período calendário não excluídos logicamente
        /// que estão em fechamento e o atual.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        public DataTable Select_AtualFechamento_By_Calendario(int cal_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_Select_AtualFechamento_By_cal_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente por calendário
        /// </summary>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>      
        /// <param name="cal_id">ID do calendário</param>      
        public DataTable SelectBy_cal_id
        (
            int tpc_id
            , int cal_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectBy_cal_id", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
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

        public DataTable SelecionaCalendarioPorTipoPeriodoCalendario(int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectCalendarioBy_tpc_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (tpc_id > 0)
                    Param.Value = tpc_id;
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
        /// Retorna os tipos de períodos do calendário que possuem alguma avaliação cadastrada para o tipo 
        /// período dele no formato de avaliação selecionado
        /// </summary>
        /// <param name="tpc_id">ID do formato de avaliação</param>      
        /// <param name="cal_id">ID do calendário</param>      
        public DataTable SelectByCalendarioComAvaliacao
        (
            int cal_id
            , int fav_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectByCalendarioComAvaliacao", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
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
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente que estão no formato de avaliação
        /// </summary>
        /// <param name="tur_id">ID do turma</param>      
        public DataTable SelectBy_FAV_Tur
        (
            long tur_id 
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_Formato_Ava", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            if (tur_id > 0)
                Param.Value = tur_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
            
            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente por turma.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <returns></returns>
        public DataTable SelectBy_Tur(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_tur_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tev_idEfetivacao">Tipo de evento de efetivação de notas</param>
        /// <param name="tur_id">ID da turma - obrigatório</param>
        public DataTable SelecionaTodos_EventoEfetivacao
        (
            int cal_id
            , long tud_id
            , int tev_idEfetivacao
            , long tur_id
            , long doc_id = -1
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelecionaTodos_EventoEfetivacao", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

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
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tev_idEfetivacao";
            Param.Size = 4;
            if (tev_idEfetivacao > 0)
                Param.Value = tev_idEfetivacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            if (doc_id > 0)
                Param.Value = doc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente até a data atual.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tev_idEfetivacao">Tipo de evento de efetivação de notas</param>
        /// <param name="tur_id">ID da turma - obrigatório</param>
        /// <param name="VerificaEscolaCalendarioPeriodo">Informa se irá selecionar todos os dados conforme os filtros (false) 
        /// ou se irá selecionar apenas os dados que não estão na tabela ESC_EscolaCalendarioPeriodo (true)</param>
        public DataTable SelectBy_PeriodoVigente_EventoEfetivacaoVigente
        (
            int cal_id
            , long tud_id
            , int tev_idEfetivacao
            , long tur_id
            , bool VerificaEscolaCalendarioPeriodo
            , long doc_id = -1
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectBy_Periodo_EventoEfetivacao_Vigentes", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

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
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tev_idEfetivacao";
            Param.Size = 4;
            if (tev_idEfetivacao > 0)
                Param.Value = tev_idEfetivacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@VerificaEscolaCalendarioPeriodo";
            Param.Size = 1;
            Param.Value = VerificaEscolaCalendarioPeriodo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            if (doc_id > 0)
                Param.Value = doc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente até a data atual.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        public DataTable SelecionarPeriodosAteDataAtual
        (
            int cal_id
            , long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelecionarPeriodosAteDataAtual", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            if (tud_id > 0)
                Param.Value = tud_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a data final do período do calendário atual
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>                
        public DateTime SelectBy_Calendario
        (
            int cal_id,
            DateTime dataMov
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectBy_Calendario", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Date;
            Param.ParameterName = "@dataMov";
            Param.Size = 20;
            Param.Value = dataMov;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return.Rows.Count > 0 ? Convert.ToDateTime(qs.Return.Rows[0]["cap_dataFim"].ToString()) : new DateTime();
        }

        /// <summary>
        /// Verifica se já existe um tipo de período do calendário cadastrado com o mesmo nome
        /// </summary>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tpc_nome">Nome do tipo de período do calendário</param>   
        public bool SelectBy_Nome
        (
            int tpc_id
            , string tpc_nome
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tpc_nome";
                Param.Size = 100;
                Param.Value = tpc_nome;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
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
        /// Verifica se já existe um tipo de período do calendário cadastrado com o mesmo nome abreviado
        /// </summary>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tpc_nomeAbreviado">Nome do tipo de período do calendário</param>   
        public bool SelectBy_NomeAbreviado
        (
            int tpc_id
            , string tpc_nomeAbreviado
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectBy_NomeAbreviado", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tpc_nomeAbreviado";
                Param.Size = 50;
                Param.Value = tpc_nomeAbreviado;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
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
        /// Verifica o maior número de ordem cadastado de tipo de período do calendário
        /// </summary>     
        public int Select_MaiorOrdem()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_Select_MaiorOrdem", _Banco);
            try
            {
                qs.Execute();

                return qs.Return.Rows.Count > 0 ? Convert.ToInt32(qs.Return.Rows[0][0]) : 0;
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
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente até a data atual por turma.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>        
        /// <param name="tur_id">ID da turma</param>
        public DataTable SelecionarPeriodosAteDataAtualPorTurma(int cal_id, long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelecionarPeriodosAteDataAtualPorTurma", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna o tpc_id vigente, caso não exista o último.
        /// </summary>
        /// <returns>Tpc_id</returns>
        public int SelectBy_PeriodoVigente()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelectBy_tpc_idVigente", _Banco);
            try
            {
                qs.Execute();

                return qs.Return.Rows.Count > 0 ? Convert.ToInt32(qs.Return.Rows[0][0]) : 0;
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
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente até a data atual,
        /// com alunos matriculados no último dia do período.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        public DataTable SelecionarPeriodosComMatricula
        (
            int cal_id
            , long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoPeriodoCalendario_SelecionarPeriodosComMatricula", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            if (tud_id > 0)
                Param.Value = tud_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoPeriodoCalendario entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tpc_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tpc_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoPeriodoCalendario entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tpc_dataCriacao");
            qs.Parameters["@tpc_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoPeriodoCalendario</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(ACA_TipoPeriodoCalendario entity)
        {
            __STP_UPDATE = "NEW_ACA_TipoPeriodoCalendario_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_TipoPeriodoCalendario entity)
        {

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = entity.tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tpc_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tpc_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoPeriodoCalendario</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(ACA_TipoPeriodoCalendario entity)
        {
            __STP_DELETE = "NEW_ACA_TipoPeriodoCalendario_UPDATE_Situacao";
            return base.Delete(entity);
        }
    }
}
