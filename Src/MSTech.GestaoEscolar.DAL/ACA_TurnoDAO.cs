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
    /// <summary>
    /// 
    /// </summary>
    public class ACA_TurnoDAO : Abstract_ACA_TurnoDAO
    {
        /// <summary>
        /// Retorna todos os turnos não excluídos logicamente que possuem previsão de séries.
        /// </summary>
        /// <param name="pfi_id">Id do processo fechamento/início.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_controleTempo">Tipo de controle de tempo do período do curso</param>
        /// <param name="crp_qtdeDiasSemana">Quantidade de dias da semana que tem aula</param>
        /// <param name="crp_qtdeTempoSemana">Quantidade de tempos de aula por semana</param>        
        /// <param name="crp_qtdeHorasDia">Quantidade de horas por dia</param>        
        /// <param name="crp_qtdeMinutosDia">Quantidade de minutos por dia</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>  
        /// <returns>Tabela com os turnos.</returns>
        public DataTable SelecionaTurnosComPrevisao
        (
            int pfi_id
            , int cur_id
            , int crr_id
            , byte crp_controleTempo
            , byte crp_qtdeDiasSemana
            , byte crp_qtdeTempoSemana
            , byte crp_qtdeHorasDia
            , byte crp_qtdeMinutosDia
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_SelecionaTurnosComPrevisao", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                Param.Value = pfi_id;
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_controleTempo";
                Param.Size = 1;
                if (crp_controleTempo > 0)
                    Param.Value = crp_controleTempo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_qtdeDiasSemana";
                Param.Size = 1;
                if (crp_qtdeDiasSemana > 0)
                    Param.Value = crp_qtdeDiasSemana;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_qtdeTempoSemana";
                Param.Size = 1;
                if (crp_qtdeTempoSemana > 0)
                    Param.Value = crp_qtdeTempoSemana;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_qtdeHorasDia";
                Param.Size = 1;
                if (crp_qtdeHorasDia > 0)
                    Param.Value = crp_qtdeHorasDia;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_qtdeMinutosDia";
                Param.Size = 1;
                if (crp_qtdeMinutosDia > 0)
                    Param.Value = crp_qtdeMinutosDia;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
        /// Retorna todos os turnos não excluídos logicamente
        /// </summary>                
        /// <param name="ttn_id">ID do tipo de turno</param>
        /// <param name="trn_descricao">Descrição do turno</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>  
        public DataTable SelectBy_Pesquisa
        (
            int ttn_id
            , string trn_descricao
            , Guid ent_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_SelectBy_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ttn_id";
                Param.Size = 4;
                if (ttn_id > 0)
                    Param.Value = ttn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@trn_descricao";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(trn_descricao))
                    Param.Value = trn_descricao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                totalRecords = qs.Return.Rows.Count;


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
        /// Retorna todos turnos ativos ou de um turno específico
        /// </summary>                       
        /// <param name="trn_id">Id do turno</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_TurnoAtivo
        (
            int trn_id
            , Guid ent_id
            , bool mostrarportipoturno
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_SelectBy_TurnoAtivo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 4;
                Param.Value = trn_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostrarportipoturno";
                Param.Size = 1;
                Param.Value = mostrarportipoturno;
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
        /// Retorna todos turnos ativos ou de um turno específico
        /// E que possui horário cadastrado para o turno
        /// </summary>                       
        /// <param name="trn_id">Id do turno</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_TurnoAtivoHorarioCadastrado
        (
            int trn_id
            , Guid ent_id
            , bool mostrarportipoturno
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_SelectBy_TurnoAtivoHorario", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 4;
                Param.Value = trn_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostrarportipoturno";
                Param.Size = 1;
                Param.Value = mostrarportipoturno;
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
        /// Retorna todos os turnos não excluídos logicamente
        /// de acordo com o controle de tempo do período do curso,
        /// de um turno específico ou que estejam ativos
        /// </summary>                
        /// <param name="trn_id">Id do turno</param>
        /// <param name="crp_controleTempo">Tipo de controle de tempo do período do curso</param>
        /// <param name="crp_qtdeDiasSemana">Quantidade de dias da semana que tem aula</param>
        /// <param name="crp_qtdeTempoSemana">Quantidade de tempos de aula por semana</param>        
        /// <param name="crp_qtdeHorasDia">Quantidade de horas por dia</param>        
        /// <param name="crp_qtdeMinutosDia">Quantidade de minutos por dia</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>        
        public DataTable SelectBy_TurnoPeriodoControleTempoAtivo
        (
            int trn_id
            , byte crp_controleTempo
            , byte crp_qtdeDiasSemana
            , byte crp_qtdeTempoSemana
            , byte crp_qtdeHorasDia
            , byte crp_qtdeMinutosDia
            , Guid ent_id
            , bool mostrarportipoturno
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_SelectBy_TurnoPeriodoControleTempoAtivo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 4;
                if (trn_id > 0)
                    Param.Value = trn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_controleTempo";
                Param.Size = 1;
                Param.Value = crp_controleTempo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_qtdeDiasSemana";
                Param.Size = 1;
                Param.Value = crp_qtdeDiasSemana;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_qtdeTempoSemana";
                Param.Size = 1;
                Param.Value = crp_qtdeTempoSemana;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_qtdeHorasDia";
                Param.Size = 1;
                Param.Value = crp_qtdeHorasDia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crp_qtdeMinutosDia";
                Param.Size = 1;
                Param.Value = crp_qtdeMinutosDia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostrarportipoturno";
                Param.Size = 1;
                Param.Value = mostrarportipoturno;
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
        /// Verifica se já existe um turno cadastrado com o mesmo nome na mesma entidade
        /// </summary>
        /// <param name="trn_id">ID do turno</param>
        /// <param name="trn_descricao">Descrição do turno</param>   
        /// <param name="ent_id">Entidade do usuário logado</param>
        public bool SelectBy_Nome
        (
            int trn_id
            , string trn_descricao
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 4;
                if (trn_id > 0)
                    Param.Value = trn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@trn_descricao";
                Param.Size = 200;
                Param.Value = trn_descricao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
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
        /// Sobrecarga do método SelectBy_Nome.
        /// </summary>
        /// <param name="entity">Entidade ACA_Turno</param>
        /// <returns>True = existe turno | False = Não existe o turno</returns>
        public bool SelectBy_Nome(ref ACA_Turno entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_SelectBy_Nome", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@trn_descricao";
                Param.Size = 200;
                Param.Value = entity.trn_descricao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@trn_id";
                Param.Size = 1;
                if (entity.trn_id > 0)
                    Param.Value = entity.trn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count >= 1)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity, false);
                    return true;
                }
                return false;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// BD:GestaoEscolar / TB:ACA_Turno
        /// Retorna os turnos de acordo com a escola e o docente.
        /// ***Metodo do Quadro de Preferencia
        /// </summary>
        /// <param name="esc_id">ID da escola</param>   
        /// <param name="uni_id">ID do tipo de nível de ensino</param>        
        /// <param name="doc_id">Entidade do usuário logdao</param>
        public DataTable SelectBy_Escola_Docente
        (
            int uni_id,
            int esc_id,
            int doc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_SelectBy_Escola_Docente", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 100;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 100;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@doc_id";
                Param.Size = 100;
                if (doc_id > 0)
                    Param.Value = doc_id;
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
        /// Retorna a quantidade de aulas por semana
        /// </summary>
        /// <param name="trn_id">Id do turno</param>
        /// <returns>Quantidade de aulas por semana</returns>
        public int QuantidadeTemposAulaTurno(int trn_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_QuantidadeTemposSemanaTurno", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@trn_id";
                Param.Value = trn_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return Convert.ToInt32(qs.Return.Rows[0]["soma"].ToString());
                else
                    return -1;
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
        /// Retorna a horas de aula por semana
        /// </summary>
        /// <param name="trn_id">ID do turno</param>
        /// <returns>Quantidade de horas de aula por semana</returns>
        public int QuantidadeHorasTurno(int trn_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Turno_QuantidadeHorasTurno", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@trn_id";
                Param.Value = trn_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return Convert.ToInt32(qs.Return.Rows[0]["soma"].ToString());
                else
                    return -1;
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
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Turno entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@trn_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@trn_dataAlteracao"].Value = DateTime.Now;

            qs.Parameters["@trn_horaInicio"].DbType = DbType.Time;
            qs.Parameters["@trn_horaInicio"].Value = Convert.ToDateTime(entity.trn_horaInicio.ToString());
            qs.Parameters["@trn_horaFim"].DbType = DbType.Time;
            qs.Parameters["@trn_horaFim"].Value = Convert.ToDateTime(entity.trn_horaFim.ToString());
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Turno entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@trn_dataCriacao");
            qs.Parameters["@trn_dataAlteracao"].Value = DateTime.Now;

            qs.Parameters["@trn_horaInicio"].DbType = DbType.Time;
            qs.Parameters["@trn_horaInicio"].Value = Convert.ToDateTime(entity.trn_horaInicio.ToString());
            qs.Parameters["@trn_horaFim"].DbType = DbType.Time;
            qs.Parameters["@trn_horaFim"].Value = Convert.ToDateTime(entity.trn_horaFim.ToString());
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade ACA_Turno</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(ACA_Turno entity)
        {
            __STP_UPDATE = "NEW_ACA_Turno_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Turno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_id";
            Param.Size = 4;
            Param.Value = entity.trn_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@trn_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade ACA_Turno</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(ACA_Turno entity)
        {
            __STP_DELETE = "NEW_ACA_Turno_Update_Situacao";
            return base.Delete(entity);
        }
    }
}