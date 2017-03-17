/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Linq;
    using System.Data;
    using System.Collections.Generic;
    using Validation.Exceptions;
    using CoreSSO.BLL;
    using System.Linq;

    public enum ObjetoAprendizagemSituacao
    {
        Ativo = 1,
        Bloqueado = 2,
        Excluido = 3
    }

    /// <summary>
    /// Estrutura com períodos do calendário
    /// </summary>
    [Serializable]
    public struct Struct_ObjetosAprendizagem
    {
        public int oap_id { get; set; }
        public string oap_descricao { get; set; }
        public byte oap_situacao { get; set; }
        public int tds_id { get; set; }
        public int tud_id { get; set; }
    }

    /// <summary>
    /// Description: ACA_ObjetoAprendizagem Business Object. 
    /// </summary>
    public class ACA_ObjetoAprendizagemBO : BusinessBase<ACA_ObjetoAprendizagemDAO, ACA_ObjetoAprendizagem>
	{
        public static DataTable SelectBy_TipoDisciplina(int tds_id)
        {
            totalRecords = 0;

            ACA_ObjetoAprendizagemDAO dao = new ACA_ObjetoAprendizagemDAO();
            return dao.SelectBy_TipoDisciplina(tds_id, out totalRecords);
        }

        public static void Save(ACA_ObjetoAprendizagem entity, IEnumerable<int> listTci_ids)
        {
            if (entity.Validate())
            {
                var dao = new ACA_ObjetoAprendizagemDAO();
                dao.Salvar(entity);

                var list = listTci_ids.Select(x => new ACA_ObjetoAprendizagemTipoCiclo
                {
                    oap_id = entity.oap_id,
                    tci_id = x
                }).ToList();

                var daoTipoCiclo = new ACA_ObjetoAprendizagemTipoCicloDAO();
                daoTipoCiclo.DeleteNew(entity.oap_id);

                foreach (var item in list)
                {
                    daoTipoCiclo.Salvar(item);
                }
            }
            else
                throw new ValidationException(UtilBO.ErrosValidacao(entity));
        }

        public static List<Struct_ObjetosAprendizagem> SelectListaBy_TipoDisciplina(int tds_id, long tud_id)
        {
            totalRecords = 0;
            List<Struct_ObjetosAprendizagem> dados = null;

            dados = (from DataRow dr in new ACA_ObjetoAprendizagemDAO().SelectListaBy_TipoDisciplina(tds_id, tud_id, out totalRecords).Rows
                     select (Struct_ObjetosAprendizagem)GestaoEscolarUtilBO.DataRowToEntity(dr, new Struct_ObjetosAprendizagem())).ToList();

            return dados;
        }
    }
}