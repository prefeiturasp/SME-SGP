using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using MSTech.Data.Common;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Situações da movimentação dos tipos de movimentação
    /// </summary>
    public enum ACA_TurnoHorarioTipo : byte
    {
        AulaNormal = 1
        ,
        AulaForaPeriodo = 2
        ,
        IntervaloEntreAulas = 3
        ,
        IntervaloEntrePeriodos = 4
    }    

    public class ACA_TurnoHorarioBO : BusinessBase<ACA_TurnoHorarioDAO, ACA_TurnoHorario>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trn_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_TurnoHorario> ListaHorariosPorTurnoId
        (
            int trn_id
        )
        {
            try
            {
                ACA_TurnoHorarioDAO dao = new ACA_TurnoHorarioDAO();
                return dao.ListaHorariosPorTurnoId(trn_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todos os Horarios
        /// que não foram excluídas logicamente, filtradas por 
        /// ID Turno, paginado.
        /// </summary>
        /// <param name="trn_id">ID de Turno</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com o turno horario</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_trn_id
        (
            int trn_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            try
            {
                ACA_TurnoHorarioDAO dao = new ACA_TurnoHorarioDAO();
                return dao.SelectBy_trn_id(trn_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todos os dias da semana cadastrados para o turno        
        /// </summary>
        /// <param name="trn_id">ID do Turno</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectDiasSemana
        (
            int trn_id
        )
        {
            try
            {
                ACA_TurnoHorarioDAO dao = new ACA_TurnoHorarioDAO();
                return dao.Select_thr_diaSemana(trn_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica se existe conflito de horarios.
        /// Faz verificação para entidade de entrada existe conflito com a entidade.
        /// </summary>
        /// <param name="EntidadeEntrada"></param>
        /// <param name="Entidade"></param>
        /// <returns>True - existe conflito / False - não existe conflito</returns>
        public static bool VerificaConflitoHorario(ACA_TurnoHorario EntidadeEntrada, ACA_TurnoHorario Entidade)
        {
            if (EntidadeEntrada.trh_diaSemana == Entidade.trh_diaSemana)
            {
                //verifica se horario esta ente horario existente - inicio >= Entrada <= fim
                if (EntidadeEntrada.trh_horaInicio >= Entidade.trh_horaInicio && EntidadeEntrada.trh_horaInicio < Entidade.trh_horaFim && EntidadeEntrada.trh_horaFim > Entidade.trh_horaInicio && EntidadeEntrada.trh_horaFim <= Entidade.trh_horaFim)
                    return true;
                //ExistenteINI >= EntradaINI <= ExistenteFim e EntradaFIM >= ExistenteFIM
                if (EntidadeEntrada.trh_horaInicio >= Entidade.trh_horaInicio && EntidadeEntrada.trh_horaInicio < Entidade.trh_horaFim && EntidadeEntrada.trh_horaFim > Entidade.trh_horaInicio && EntidadeEntrada.trh_horaFim >= Entidade.trh_horaFim)
                    return true;
                    //EntradaINI <= ExistenteINI e  ExistenteINI >= EntradaFIM <= ExistenteFIM
                if (EntidadeEntrada.trh_horaInicio <= Entidade.trh_horaInicio && EntidadeEntrada.trh_horaInicio < Entidade.trh_horaFim && EntidadeEntrada.trh_horaFim > Entidade.trh_horaInicio && EntidadeEntrada.trh_horaFim <= Entidade.trh_horaFim)
                    return true;
                    //EntradaINI <= ExistenteINI e  EntradaFIM >= ExistenteFIM
                if (EntidadeEntrada.trh_horaInicio <= Entidade.trh_horaInicio && EntidadeEntrada.trh_horaInicio < Entidade.trh_horaFim && EntidadeEntrada.trh_horaFim > Entidade.trh_horaInicio && EntidadeEntrada.trh_horaFim >= Entidade.trh_horaFim)
                    return true;

                return false;
            }

            return false;
        }

        /// <summary>
        /// Verifica se hora inicial é menor ou igual que hora final na entidade Turno Horario.
        /// </summary>
        /// <param name="Entidade">Entidade Turno Horario</param>
        /// <returns>True - Menor ou igual / False - Maior e diferente</returns>
        public static bool VerificaHoraInicio_MenorIgual_Fim(ACA_TurnoHorario Entidade)
        {
            return Entidade.trh_horaFim <= Entidade.trh_horaInicio;
        }
    }
}
