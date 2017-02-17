/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using System.Data;
    using System;
    using System.Linq;
	
	/// <summary>
	/// Description: ORC_OrientacaoCurricularRelacionamento Business Object. 
	/// </summary>
	public class ORC_OrientacaoCurricularRelacionamentoBO : BusinessBase<ORC_OrientacaoCurricularRelacionamentoDAO, ORC_OrientacaoCurricularRelacionamento>
	{
        /// <summary>
        /// O método salva uma lista de habilidades relacionadas.
        /// </summary>
        /// <param name="ltEntities">Lista de habilidades relacionadas.</param>
        /// <returns></returns>
        public static bool Salvar(List<ORC_OrientacaoCurricularRelacionamento> ltEntities)
        {
            TalkDBTransaction banco = new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return ltEntities.Aggregate(true, (salvou, entity) => salvou & Save(entity, banco));
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// O método inclui/altera/exclui uma registro na tabela ORC_OrientacaoCurricularRelacionamento.
        /// </summary>
        /// <param name="entity">Entidade ORC_OrientacaoCurricularRelacionamento</param>
        /// <param name="banco">Transação.</param>
        /// <returns></returns>
        private static new bool Save(ORC_OrientacaoCurricularRelacionamento entity, TalkDBTransaction banco)
        {
            return entity.isChecked ? (new ORC_OrientacaoCurricularRelacionamentoDAO { _Banco = banco }).Salvar(entity)
                    : (new ORC_OrientacaoCurricularRelacionamentoDAO { _Banco = banco }).Delete(entity);
        }

        /// <summary>
        /// Seleciona os relacionamentos pelo id da orientação curricular.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular</param>
        /// <returns></returns>
        public static List<ORC_OrientacaoCurricularRelacionamento> SelecionaPorOrientacaoCurricular(Int64 ocr_id, Int64 ocr_idSuperiorRelacionada)
        {
            return new ORC_OrientacaoCurricularRelacionamentoDAO().SelecionaPorOrientacaoCurricular(ocr_id, ocr_idSuperiorRelacionada);
        }
	}
}