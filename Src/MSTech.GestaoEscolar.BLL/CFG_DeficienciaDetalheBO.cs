/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using System.Collections.Generic;
    using Data.Common;
    using System;
    using System.Linq;

    /// <summary>
    /// Description: CFG_DeficienciaDetalhe Business Object. 
    /// </summary>
    public class CFG_DeficienciaDetalheBO : BusinessBase<CFG_DeficienciaDetalheDAO, CFG_DeficienciaDetalhe>
    {

        /// <summary>
        /// Retorna todas as áreas de conhecimento não excluídas logicamente
        /// Sem paginação
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAtivos()
        {
            CFG_DeficienciaDetalheDAO dao = new CFG_DeficienciaDetalheDAO();
            return dao.SelecionaAtivos();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstDetelhe"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool Salvar(Guid tde_id,List<CFG_DeficienciaDetalhe> lstDetalhe, TalkDBTransaction banco = null)
        {
            CFG_DeficienciaDetalheDAO dao = new CFG_DeficienciaDetalheDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                //Carrega os detalhes ligados a deficiencia
                List<CFG_DeficienciaDetalhe> lstDetalheBanco =  CFG_DeficienciaDetalheBO.SelectDetalheBy_Deficiencia(tde_id, dao._Banco);

                //Salva questões
                foreach (CFG_DeficienciaDetalhe dfd in lstDetalhe)
                {
                    if (dfd.IsNew)
                    {
                        dfd.dfd_id = -1;
                        dfd.dfd_dataCriacao = DateTime.Now;
                        dfd.tde_id = tde_id;
                    }
                    dfd.dfd_dataAlteracao = DateTime.Now;
                    if (!CFG_DeficienciaDetalheBO.Save(dfd, dao._Banco))
                        return false;
                }

                //Remove logicamente no banco detalhes que foram removidas da deficiencia
                foreach (CFG_DeficienciaDetalhe dfdB in lstDetalheBanco)
                    if (!lstDetalhe.Any(q => q.dfd_id == dfdB.dfd_id && q.dfd_situacao != 3))
                    {
                        CFG_DeficienciaDetalheBO.Delete(dfdB, dao._Banco);
                    }

                return true;

            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Seleciona os detalhes relacionados a uma deficiencia
        /// </summary>
        /// <param name="tde_id"> Id do tipo de deficiencia do Core</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static List<CFG_DeficienciaDetalhe> SelectDetalheBy_Deficiencia(Guid tde_id, TalkDBTransaction banco = null)
        {
            CFG_DeficienciaDetalheDAO dao = new CFG_DeficienciaDetalheDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectDetelheBy_Deficiencia(tde_id);
        }

        
        public static CFG_DeficienciaDetalhe GetDetalhamento(CFG_DeficienciaDetalhe entity, TalkDBTransaction banco = null)
        {
            CFG_DeficienciaDetalheDAO dao = new CFG_DeficienciaDetalheDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectDetelheBy_dfd_id(entity.dfd_id);
        }

    }
}