/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Situações da movimentação dos tipos de movimentação
    /// </summary>
    public enum ACA_TipoMovimentacaoSituacao : byte
    {
        Ativo = 1
        ,
        Bloqueado = 2
        ,
        Excluido = 3
    }

    /// <summary>
    /// Motivos do tipo de movimentação
    /// </summary>
    public enum ACA_TipoMovimentacaoMotivo : byte
    {
        Entrada = 1
        ,
        Saida = 2
    }

	/// <summary>
	/// ACA_TipoMovimentacao Business Object 
	/// </summary>
	public class ACA_TipoMovimentacaoBO : BusinessBase<ACA_TipoMovimentacaoDAO,ACA_TipoMovimentacao>
	{
    }			
}