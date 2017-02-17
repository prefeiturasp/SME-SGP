/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Tipo de etapa
    /// </summary>
    public enum MTR_ProcessoFechamentoInicioEtapaTipoEtapa : byte
    {
        FormacaoTurmas = 5
        ,
        SequenciamentoChamada = 6
    }


    /// <summary>
    /// Tipo de alcance
    /// </summary>
    public enum MTR_ProcessoFechamentoInicioEtapaAlcance : byte
    {
        TodaRede = 1
        ,
        Escola = 2
        ,
        Curso = 3
        ,
        CalendarioEscolar = 4
    }

    /// <summary>
    /// Classificação do tipo da etapa
    /// </summary>
    public enum MTR_ProcessoFechamentoInicioEtapaInicioFechamento : byte
    {
        Fechamento = 1
        ,
        Inicio = 2
    }
    
    #endregion Enumeradores

	/// <summary>
	/// MTR_ProcessoFechamentoInicioEtapa Business Object 
	/// </summary>
	public class MTR_ProcessoFechamentoInicioEtapaBO : BusinessBase<MTR_ProcessoFechamentoInicioEtapaDAO,MTR_ProcessoFechamentoInicioEtapa>
    {
        #region Métodos de verificação / validação

        /// <summary>
        /// Verifica se uma etapa pode ser realizada
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início do ano letivo</param>        
        /// <param name="etapa">Tipo da etapa do processo de fechamento/início do ano letivo</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo do curso</param>
        /// <param name="crp_id">Id do período do curso</param>        
        /// <returns>True: A etapa está vigente / False: A etapa não está vigente</returns>        
        public static bool VerificaEtapaVigente
        (
            MTR_ProcessoFechamentoInicioEtapaTipoEtapa etapa
            , int pfi_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id            
        )
		{
            MTR_ProcessoFechamentoInicioEtapaDAO dao = new MTR_ProcessoFechamentoInicioEtapaDAO();
            return dao.VerificaEtapaVigente((byte)etapa, pfi_id, esc_id, uni_id, cur_id, crr_id, crp_id);
        }

        #endregion
    }
}
