using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações da movimentação dos tipos de movimentação
    /// </summary>
    public enum eDiasSemana : byte
    {
        [Description("ACA_TurnoBO.DiasSemana.Domingo")]
        Domingo = 1,

        [Description("ACA_TurnoBO.DiasSemana.Segunda")]
        Segunda = 2,

        [Description("ACA_TurnoBO.DiasSemana.Terca")]
        Terca = 3,

        [Description("ACA_TurnoBO.DiasSemana.Quarta")]
        Quarta = 4,

        [Description("ACA_TurnoBO.DiasSemana.Quinta")]
        Quinta = 5,

        [Description("ACA_TurnoBO.DiasSemana.Sexta")]
        Sexta = 6,

        [Description("ACA_TurnoBO.DiasSemana.Sabado")]
        Sabado = 7
    }

    #endregion Enumeradores

    public class ACA_TurnoBO : BusinessBase<ACA_TurnoDAO, ACA_Turno>
    {
        /// <summary>
        /// Retorna todos os turnos não excluídos logicamente que possuem previsão de séries.
        /// </summary>
        /// <param name="pfi_id">Id do processo fechamento/início.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>      
        /// <param name="ent_id">Entidade do usuário logado</param>  
        /// <returns>Tabela com os turnos.</returns>
        public static DataTable SelecionaTurnosComPrevisao
        (
            int pfi_id
            , int cur_id
            , int crr_id
            , Guid ent_id
        )
        {
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.SelecionaTurnosComPrevisao(pfi_id, cur_id, crr_id, 0, 0, 0, 0, 0, ent_id);
        }

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
        public static DataTable SelecionaTurnosComPrevisao
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
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.SelecionaTurnosComPrevisao(pfi_id, cur_id, crr_id, crp_controleTempo, crp_qtdeDiasSemana, crp_qtdeTempoSemana, crp_qtdeHorasDia, crp_qtdeMinutosDia, ent_id);
        }
        
        /// <summary>
        /// Retorna todos os turnos não excluídos logicamente
        /// </summary>                        
        /// <param name="ttn_id">ID do tipo de turno</param>
        /// <param name="trn_descricao">Descrição do turno</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTurno
        (
            int ttn_id
            , string trn_descricao
            , Guid ent_id
        )
        {
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.SelectBy_Pesquisa(ttn_id, trn_descricao, ent_id, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os turnos não excluídos logicamente        
        /// Sem paginação
        /// </summary>     
        /// <param name="ent_id">Entidade do usuário logado</param>   
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTurno
        (
            Guid ent_id
        )
        {
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.SelectBy_Pesquisa(0, string.Empty, ent_id, out totalRecords);
        }

        /// <summary>
        /// Retorna os turnos ativos ou de um turno específico     
        /// Sem paginação
        /// </summary>        
        /// <param name="trn_id">Id do turno</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="mostrarportipoturno"></param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTurnoPorTurnoAtivo
        (
            int trn_id
            , Guid ent_id
            , bool mostrarportipoturno
        )
        {
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.SelectBy_TurnoAtivo(trn_id, ent_id, mostrarportipoturno);
        }

        /// <summary>
        /// Retorna os turnos ativos ou de um turno específico  
        /// E que possui horário cadastrado para o turno
        /// Sem paginação
        /// </summary>        
        /// <param name="trn_id">Id do turno</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="mostrarportipoturno"></param>
        public static DataTable SelecionaTurnoPorTurnoAtivoHorarioCadastrado
        (
            int trn_id
            , Guid ent_id
            , bool mostrarportipoturno
        )
        {
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.SelectBy_TurnoAtivoHorarioCadastrado(trn_id, ent_id, mostrarportipoturno);
        }

        /// <summary>
        /// Retorna todos os turnos não excluídos logicamente
        /// de acordo com o controle de tempo do período do curso
        /// de um turno específico ou que estejam ativos
        /// Sem paginação
        /// </summary>        
        /// <param name="trn_id">Id do turno</param>
        /// <param name="crp_controleTempo">Tipo de controle de tempo do período do curso</param>
        /// <param name="crp_qtdeDiasSemana">Quantidade de dias da semana que tem aula</param>
        /// <param name="crp_qtdeTempoSemana">Quantidade de tempos de aula por semana</param>        
        /// <param name="crp_qtdeHorasDia">Quantidade de horas por dia</param>        
        /// <param name="crp_qtdeMinutosDia">Quantidade de minutos por dia</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="mostrarportipoturno"></param>   
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTurnoPorTurnoPeriodoControleTempoAtivo
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
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.SelectBy_TurnoPeriodoControleTempoAtivo(trn_id, crp_controleTempo, crp_qtdeDiasSemana, crp_qtdeTempoSemana, crp_qtdeHorasDia, crp_qtdeMinutosDia, ent_id, mostrarportipoturno);
        }
        
        /// <summary>
        /// BD:GestaoEscolar / TB:ACA_Turno
        /// Retorna os turnos de acordo com a escola e o docente.
        /// ***Metodo do Quadro de Preferencia
        /// </summary>
        /// <param name="esc_id">ID da escola</param>   
        /// <param name="uni_id">ID do tipo de nível de ensino</param>        
        /// <param name="doc_id">Entidade do usuário logdao</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Escola_Docente
        (
            int uni_id
            , int esc_id
            , int doc_id
        )
        {
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.SelectBy_Escola_Docente(uni_id, esc_id, doc_id);
        }

        /// <summary>
        /// Retorna a quantidade de aulas por semana
        /// </summary>
        /// <param name="trn_id">id da tabela turno</param>
        /// <returns>Quantidade de aulas por semana</returns>
        public static int QuantidadeTemposAulaTurno
        (
            int trn_id
        )
        {
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.QuantidadeTemposAulaTurno(trn_id);
        }

        /// <summary>
        /// Retorna a horas de aula por semana
        /// </summary>
        /// <param name="trn_id">id da tabela turno</param>
        /// <returns>Quantidade de horas de aula por semana</returns>
        public static int QuantidadeHorasTurno
        (
            int trn_id
        )
        {
            ACA_TurnoDAO dao = new ACA_TurnoDAO();
            return dao.QuantidadeHorasTurno(trn_id);
        }
    }
}
