/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Validation.Exceptions;
    using System;
    using Data.Common;
    using System.Data;

    /// <summary>
    /// Description: ESC_TipoClassificacaoEscolaCargos Business Object. 
    /// </summary>
    public class ESC_TipoClassificacaoEscolaCargosBO : BusinessBase<ESC_TipoClassificacaoEscolaCargosDAO, ESC_TipoClassificacaoEscolaCargos>
    {
        /// <summary>
        /// Retorna os cargos vinculados ao tipo de classificação da escola.
        /// </summary>
        /// <param name="tce_id">Tipo de classificação da escola</param>
        public static List<ESC_TipoClassificacaoEscolaCargos> SelectTipoClassificacaoEscolaCargosByTipoClassificacaoEscola(int tce_id)
        {
            ESC_TipoClassificacaoEscolaCargosDAO dao = new ESC_TipoClassificacaoEscolaCargosDAO();
            return dao.SelectTipoClassificacaoEscolaCargosByTipoClassificacaoEscola(tce_id);
        }
    }
}