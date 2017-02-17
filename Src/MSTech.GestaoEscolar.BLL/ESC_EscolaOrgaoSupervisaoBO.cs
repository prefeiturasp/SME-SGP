using System;
using System.Data;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.BLL
{
    public class ESC_EscolaOrgaoSupervisaoBO : BusinessBase<ESC_EscolaOrgaoSupervisaoDAO, ESC_EscolaOrgaoSupervisao>
    {
        #region Estruturas

        //[Serializable]
        //public struct EscolaOrgaoSupervisao
        //{
        //    public ESC_EscolaOrgaoSupervisao entityEscolaOrgaoSupervisao { get; set; }
        //    public string uad_nome { get; set; }
        //}

        #endregion
        
        /// <summary>
        /// Retorna um datatable contendo todos os "Órgãos de Supervisão" da Escola
        /// que não foram excluídos logicamente, filtrados por 
        /// id da escola
        /// </summary>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com os "Órgãos de Supervisão" da Escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int esc_id            
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ESC_EscolaOrgaoSupervisaoDAO dao = new ESC_EscolaOrgaoSupervisaoDAO();
            return dao.SelectBy_esc_id(esc_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Seleciona o id do último orgão de supervisao cadastrado por escola + 1
        /// se não houver orgão de supervisao cadastrado para a escola retorna 1
        /// filtrados por esc_id
        /// </summary>
        /// <param name="esc_id">Campo esc_id da tabela ESC_Escola do bd</param>        
        /// <returns>psc_id + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 VerificaUltimoOrgaoCadastrado
        (
            int esc_id
        )
        {
            ESC_EscolaOrgaoSupervisaoDAO dao = new ESC_EscolaOrgaoSupervisaoDAO();
            return dao.SelectBy_esc_id_top_one(esc_id);
        }

        /// <summary>
        /// Inclui um novo orgão de supervisão para a escola
        /// </summary>
        /// <param name="entity">Entidade ESC_EscolaOrgaoSupervisao</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ESC_EscolaOrgaoSupervisao entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ESC_EscolaOrgaoSupervisaoDAO dao = new ESC_EscolaOrgaoSupervisaoDAO {_Banco = banco};

                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));            
        }

        /// <summary>
        /// Retorna um list com os orgão de supervisão para a escola.
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ESC_EscolaOrgaoSupervisao> GetSelectBy_OrgaoSupervisor(int esc_id, TalkDBTransaction banco)
        {
            List<ESC_EscolaOrgaoSupervisao> retorno = new List<ESC_EscolaOrgaoSupervisao>();
            ESC_EscolaOrgaoSupervisaoDAO dao = new ESC_EscolaOrgaoSupervisaoDAO { _Banco = banco };
            DataTable dt = dao.SelectBy_esc_id(esc_id, false, 1, 1, out totalRecords);

            foreach (DataRow dr in dt.Rows)
            {
                ESC_EscolaOrgaoSupervisao ent = new ESC_EscolaOrgaoSupervisao();
                ent = dao.DataRowToEntity(dr, ent);

                retorno.Add(ent);
            }

            return retorno;
        }



        /// <summary>
        /// Retorna um list com os orgão de supervisão para a escola.
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ESC_EscolaOrgaoSupervisao> GetSelectBy_OrgaoSupervisor(int esc_id)
        {
            List<ESC_EscolaOrgaoSupervisao> retorno = new List<ESC_EscolaOrgaoSupervisao>();
            ESC_EscolaOrgaoSupervisaoDAO dao = new ESC_EscolaOrgaoSupervisaoDAO {};
            DataTable dt = dao.SelectBy_esc_id(esc_id, false, 1, 1, out totalRecords);

            foreach (DataRow dr in dt.Rows)
            {
                ESC_EscolaOrgaoSupervisao ent = new ESC_EscolaOrgaoSupervisao();
                ent = dao.DataRowToEntity(dr, ent);

                retorno.Add(ent);
            }

            return retorno;
        }



        /// <summary>
        /// Salva os pareceres do orgão de supervisão para escola.
        /// </summary>
        /// <param name="entityEscola">Entidade da escola</param>
        /// <param name="listOrgaoSupervisao">Lista de pareceres pra salvar</param>
        /// <param name="banco">Transação com banco de dados</param>
        internal static void SalvarOrgaoSupervisao(ESC_Escola entityEscola, List<ESC_EscolaOrgaoSupervisao> listOrgaoSupervisao, TalkDBTransaction banco)
        {
            List<ESC_EscolaOrgaoSupervisao> listaOrgaoSupervisaoBanco = GetSelectBy_OrgaoSupervisor(entityEscola.esc_id, banco);

            foreach (ESC_EscolaOrgaoSupervisao row in listOrgaoSupervisao)
            {
                ESC_EscolaOrgaoSupervisao entityOrgaoSupervisao = new ESC_EscolaOrgaoSupervisao();
                entityOrgaoSupervisao.esc_id = entityEscola.esc_id;
                entityOrgaoSupervisao.eos_id = row.eos_id;
                entityOrgaoSupervisao = ESC_EscolaOrgaoSupervisaoBO.GetEntity(entityOrgaoSupervisao, banco);
                entityOrgaoSupervisao.ent_id = row.ent_id;
                entityOrgaoSupervisao.uad_id = row.uad_id;
                entityOrgaoSupervisao.eos_nome = row.eos_nome;
                entityOrgaoSupervisao.uad_nome = row.uad_nome;
                entityOrgaoSupervisao.eos_dataAlteracao = row.eos_dataAlteracao;
                entityOrgaoSupervisao.eos_dataCriacao = row.eos_dataCriacao;
                entityOrgaoSupervisao.eos_situacao = row.eos_situacao;

                entityOrgaoSupervisao.IsNew = row.eos_id == 0 ? true : false;
                ESC_EscolaOrgaoSupervisaoBO.Save(entityOrgaoSupervisao, banco);
            }

            foreach (ESC_EscolaOrgaoSupervisao ent in listaOrgaoSupervisaoBanco)
            {
                if (!listOrgaoSupervisao.Exists
                    (
                        p =>
                            p.esc_id == ent.esc_id &&
                            p.eos_id == ent.eos_id
                    ))
                {
                    // O registro foi excluído.
                    Delete(ent, banco);
                }
            }
        }
    }
}
