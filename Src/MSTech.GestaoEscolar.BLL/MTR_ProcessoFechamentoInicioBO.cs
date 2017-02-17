/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// MTR_ProcessoFechamentoInicio Business Object
    /// </summary>
    public class MTR_ProcessoFechamentoInicioBO : BusinessBase<MTR_ProcessoFechamentoInicioDAO, MTR_ProcessoFechamentoInicio>
    {
        
        /// <summary>
        /// Seleciona os processos de fechamento/início do ano letivo por ano letivo corrente.
        /// </summary>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>
        /// DataTable contendo todos os processos de fechamento/início
        /// pelo ano letivo corrente
        /// </returns>
        public static MTR_ProcessoFechamentoInicio CarregaPorAnoLetivoCorrente(Guid ent_id)
        {
            MTR_ProcessoFechamentoInicioDAO dao = new MTR_ProcessoFechamentoInicioDAO();
            return dao.CarregaPorAnoLetivoCorrente(ent_id);
        }
    }
}