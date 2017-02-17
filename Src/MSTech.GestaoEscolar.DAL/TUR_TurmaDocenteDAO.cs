/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class TUR_TurmaDocenteDAO : Abstract_TUR_TurmaDocenteDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna as atribuições de docentes criadas pelo cargo especificado (atribuiçao esporádica).
        /// </summary>
        /// <param name="col_id"></param>
        /// <param name="crg_id"></param>
        /// <param name="coc_id"></param>
        /// <returns></returns>
        public DataTable PesquisaPor_AtribuicaoEsporadica
        (
            long col_id
            , int crg_id
            , int coc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_PesquisaPor_AtribuicaoEsporadica", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@col_id";
            Param.Size = 8;
            Param.Value = col_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crg_id";
            Param.Size = 4;
            Param.Value = crg_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@coc_id";
            Param.Size = 4;
            Param.Value = coc_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os docentes da turma
        /// </summary>
        /// <param name="tud_id">ID da disciplina.</param>
        public void TransferirLancamentos_Posicao
        (
            long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_TransferirLancamentos_Posicao", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();
        }

        /// <summary>
        /// Retorna os docentes da turma
        /// </summary>
        /// <param name="tur_id">ID da disciplina da turma</param>
        public DataTable SelectBy_Turma
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_SelectBy_Turma", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os nomes dos docentes para cada posição da disciplina.
        /// </summary>
        /// <param name="tud_id">Ids da. disciplina.</param>
        public DataTable SelecionaDocentePosicaoPorDisciplina
        (
            long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_DocentePosicaoPorDisciplina", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os docentes das disciplinas.
        /// </summary>
        /// <param name="tud_id">String contendo os ids das disciplinas</param>
        public DataTable SelecionaDocentesDisciplinas
        (
            string tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_DocentesDisciplinas", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@tud_id";
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os docentes das disciplinas.
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        /// <param name="posicao"></param>
        public DataTable SelecionaVigenciasDocentesPorDisciplina(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_SelecionaVigenciasDocenteDisciplinas", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a posição do docente nas TurmaDisciplinas.
        /// </summary>
        /// <param name="tud_ids">Ids das TurmaDisciplinas.</param>
        /// <returns>Posição do docente nas TurmaDisciplinas.</returns>
        public List<TUR_TurmaDocente> SelecionaPosicaoPorTudIds(string tud_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_SelecionaPosicaoPorTudIds", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tud_ids";
            Param.Value = tud_ids;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();
            DataTable dt = qs.Return;

            List<TUR_TurmaDocente> lista = new List<TUR_TurmaDocente>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TUR_TurmaDocente turmaDocente = new TUR_TurmaDocente();
                turmaDocente = DataRowToEntity(dt.Rows[i], turmaDocente);
                lista.Add(turmaDocente);
            }

            return lista;
        }

        /// <summary>
        /// Exclui logicamente a ligação do docente com a disciplina da turma
        /// </summary>
        /// <param name="col_id">Id do colaborador</param>
        /// <param name="crg_id">Id do cargo</param>
        /// <param name="coc_id">Id do colaboradorcargo</param>
        /// <param name="tds_id">Id do tipo de disciplina</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="uad_id">Id da unidade administrativa</param>
        /// <returns>Verdadeiro se conseguiu fazer a exclusão lógica.</returns>
        public bool AtualizarTurmaDocentePorColaboradorDocente
        (
            long col_id
            , int crg_id
            , int coc_id
            , int tds_id
            , Guid ent_id
            , Guid uad_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_Update_SituacaoPorDocente", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@col_id";
                Param.Size = 8;
                Param.Value = col_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                Param.Value = crg_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@coc_id";
                Param.Size = 4;
                Param.Value = coc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                if (tds_id > 0)
                    Param.Value = tds_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_id";
                Param.Size = 16;
                Param.Value = uad_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a última posição do docente na disciplina, como ativo ou inativo.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tud_id">ID da disciplina</param>
        /// <returns></returns>
        public KeyValuePair<byte, bool> SelectPosicaoByDocenteTurma_ComInativos(long doc_id, long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_SelectPosicaoByDocenteTurma_ComInativos", _Banco);
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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();
                KeyValuePair<byte, bool> retorno = new KeyValuePair<byte, bool>(0,false);
                
                if (qs.Return.Rows.Count > 0)
                {
                    retorno = new KeyValuePair<byte, bool>(Convert.ToByte(qs.Return.Rows[0]["tdt_posicao"])
                        , Convert.ToBoolean(qs.Return.Rows[0]["AtribuicaoAtiva"]));
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a posição do docente na turma disciplina
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns>Posição do docente na turma disciplina</returns>
        public byte SelectPosicaoByDocenteTurma(long doc_id, long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_SelectPosicaoByDocenteTurma", _Banco);
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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? Convert.ToByte(qs.Return.Rows[0]["tdt_posicao"]) : (Byte)0;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona as turmas em que o professor leciona determinada disciplina, exceto na turma passa por parâmetro.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="tdt_posicao">Posição do docente.</param>
        /// <returns></returns>
        public DataTable SelecionaPorTurmaDisciplinaPosicao(long tur_id, int cal_id, int cur_id, int crr_id, int crp_id, long tud_id, byte tdt_posicao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_SelecionaTurmasPorTurmaDisciplinaPosicao", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os docentes das disciplinas para a tela de atribuição de docentes
        /// </summary>
        /// <param name="tur_id">ID da disciplina da turma</param>
        public DataTable SelecionaAtribuicaoDocentes
        (
            long tur_id,
            long doc_id,
            long tud_id_Compartilhada
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_SelectAtribuicaoDocentesBy_Turma", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
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

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id_Compartilhada";
            Param.Size = 8;
            Param.Value = tud_id_Compartilhada;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna o id da turma disciplina de docencia compartilhada,
        /// cujo docente é o titular.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        public Int64 SelectTitularDisciplinaDocenciaCompartilhada(long tur_id, long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_SelectTitularDisciplinaDocenciaCompartilhada", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
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

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return.Rows.Count > 0 ? Convert.ToInt64(qs.Return.Rows[0]["tud_id"]) : -1;
        }

        /// <summary>
        /// Retorna todos os docentes ativos filtrados pelos parametros
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        public DataTable SelecionaDocentesAtivos(int esc_id, int cur_id, int crr_id, int crp_id, long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDocente_SelecionaDocentesAtivos", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                //Param = qs.NewParameter();
                //Param.DbType = DbType.Guid;
                //Param.ParameterName = "@uad_id";
                //Param.Size = 16;
                //Param.Value = uad_id;
                //qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos

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
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaDocente entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tdt_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tdt_dataAlteracao"].Value = DateTime.Now;
            qs.Parameters["@tdt_vigenciaInicio"].DbType = DbType.Date;
            qs.Parameters["@tdt_vigenciaFim"].DbType = DbType.Date;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaDocente entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tdt_dataCriacao");
            qs.Parameters["@tdt_dataAlteracao"].Value = DateTime.Now;
            qs.Parameters["@tdt_vigenciaInicio"].DbType = DbType.Date;
            qs.Parameters["@tdt_vigenciaFim"].DbType = DbType.Date;
        }

        /// <summary>
        /// Méotodo de update alterado para que não modificasse o valor do campo data da criação;
        /// </summary>
        /// <param name="entity">Entidade com dados preenchidos</param>
        /// <returns>True para alteração realizado com sucesso.</returns>
        protected override bool Alterar(TUR_TurmaDocente entity)
        {
            __STP_UPDATE = "NEW_TUR_TurmaDocente_Update";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TurmaDocente entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = entity.tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tdt_id";
            Param.Size = 4;
            Param.Value = entity.tdt_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tdt_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Delete(TUR_TurmaDocente entity)
        {
            __STP_DELETE = "NEW_TUR_TurmaDocente_Update_Situacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity"></param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, TUR_TurmaDocente entity)
        {
            entity.tdt_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.tdt_id > 0);
        }

        #endregion Métodos Sobrescritos
    }
}