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
    using Data.Common;

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
        public long tud_id { get; set; }
        public int oap_id { get; set; }
        public string oap_descricao { get; set; }
        public byte oap_situacao { get; set; }
        public int tpc_id { get; set; }
        public string tpc_nome { get; set; }
        public int tpc_ordem { get; set; }
        public bool selecionado { get; set; }
        public int oae_id { get; set; }
        public string oae_descricao { get; set; }
        public int oae_ordem { get; set; }
        public int oae_idSub { get; set; }
        public string oae_descricaoSub { get; set; }
        public int oae_ordemSub { get; set; }
    }

    /// <summary>
    /// Description: ACA_ObjetoAprendizagem Business Object. 
    /// </summary>
    public class ACA_ObjetoAprendizagemBO : BusinessBase<ACA_ObjetoAprendizagemDAO, ACA_ObjetoAprendizagem>
	{
        public static DataTable SelectBy_TipoDisciplina(int tds_id, int cal_ano)
        {
            totalRecords = 0;

            ACA_ObjetoAprendizagemDAO dao = new ACA_ObjetoAprendizagemDAO();
            return dao.SelectBy_TipoDisciplina(tds_id, cal_ano, out totalRecords);
        }

        /// <summary>
        /// Busca os objetos cadastrados para a disciplina no ano e eixo informados
        /// </summary>
        /// <param name="tds_id">ID da disciplina</param>
        /// <param name="cal_ano">Ano letivo</param>
        /// <param name="oae_id">ID do eixo (ou sub eixo)</param>
        /// <returns></returns>
        public static List<ACA_ObjetoAprendizagem> SelectBy_TipoDisciplinaEixo(int tds_id, int cal_ano, int oae_id)
        {
            ACA_ObjetoAprendizagemDAO dao = new ACA_ObjetoAprendizagemDAO();
            return dao.SelectBy_TipoDisciplinaEixo(tds_id, cal_ano, oae_id);
        }

        /// <summary>
        /// Salva o objeto de aprendizagem
        /// </summary>
        /// <param name="entity">Entidade do objeto de aprendizagem</param>
        /// <param name="listTci_ids">Lista de ciclos</param>
        public static void Save(ACA_ObjetoAprendizagem entity, List<int> listTci_ids)
        {
            ACA_ObjetoAprendizagemDAO dao = new ACA_ObjetoAprendizagemDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (entity.Validate())
                {
                    bool isNew = entity.oap_id <= 0;

                    if (VerificaObjetoMesmoNome(entity.oap_id, entity.tds_id, entity.cal_ano, entity.oae_id, entity.oap_descricao, dao._Banco))
                        throw new ValidationException("Já existe um objeto de conhecimento cadastrado com a mesma descrição no eixo.");

                    Save(entity, dao._Banco);

                    List<ACA_ObjetoAprendizagemTipoCiclo> list = listTci_ids.Select(x => new ACA_ObjetoAprendizagemTipoCiclo
                    {
                        oap_id = entity.oap_id,
                        tci_id = x
                    }).ToList();

                    if (isNew)
                    {
                        Dictionary<int, string> lstCiclosEmUso = ACA_ObjetoAprendizagemTipoCicloBO.CiclosEmUso(entity.oap_id, dao._Banco);
                        if (lstCiclosEmUso.Any(c => !list.Any(p => p.tci_id == c.Key)))
                        {
                            if (lstCiclosEmUso.Where(c => !list.Any(p => p.tci_id == c.Key)).Count() > 1)
                                throw new ValidationException("Ciclos " + lstCiclosEmUso.Where(c => !list.Any(p => p.tci_id == c.Key)).Select(p => p.Value).Aggregate((a, b) => a + ", " + b) +
                                                              " estão em uso e não podem ser removidos.");
                            else
                                throw new ValidationException("Ciclo " + lstCiclosEmUso.Where(c => !list.Any(p => p.tci_id == c.Key)).First().Value +
                                                              " está em uso e não pode ser removido.");
                        }
                    }

                    ACA_ObjetoAprendizagemTipoCicloBO.DeleteNew(entity.oap_id, dao._Banco);

                    foreach (ACA_ObjetoAprendizagemTipoCiclo item in list)
                    {
                        ACA_ObjetoAprendizagemTipoCicloBO.Save(item, dao._Banco);
                    }
                }
                else
                    throw new ValidationException(UtilBO.ErrosValidacao(entity));

                GestaoEscolarUtilBO.LimpaCache("Cache_SelecionaTipoDisciplinaObjetosAprendizagem");
                GestaoEscolarUtilBO.LimpaCache("Cache_SelecionaTipoCicloAtivosEscola");
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Verifica se existe um objeto de aprendizagem cadastrado com o mesmo nome (verifica apenas no eixo)
        /// </summary>
        /// <param name="oap_id">ID do objeto que está sendo salvo</param>
        /// <param name="tds_id">ID da disciplina</param>
        /// <param name="cal_ano">Ano letivo</param>
        /// <param name="oae_id">ID do eixo</param>
        /// <param name="oap_descricao">Descrição do eixo</param>
        /// <param name="banco">Transação do banco</param>
        /// <returns></returns>
        private static bool VerificaObjetoMesmoNome(int oap_id, int tds_id, int cal_ano, int oae_id, string oap_descricao, TalkDBTransaction banco)
        {
            ACA_ObjetoAprendizagemDAO dao = new ACA_ObjetoAprendizagemDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.VerificaEixoMesmoNome(oap_id, tds_id, cal_ano, oae_id, oap_descricao);
        }

        /// <summary>
        /// Seleciona a lista de objetos por turma disciplina
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="cal_id">Ano letivo</param>
        /// <param name="banco">Transação de banco</param>
        /// <returns></returns>
        public static List<Struct_ObjetosAprendizagem> SelectListaBy_TurmaDisciplina(long tud_id, int cal_id, TalkDBTransaction banco = null)
        {
            ACA_ObjetoAprendizagemDAO dao = new ACA_ObjetoAprendizagemDAO();
            if (banco != null)
                dao._Banco = banco;

            totalRecords = 0;
            List<Struct_ObjetosAprendizagem> dados = null;

            dados = (from DataRow dr in dao.SelectListaBy_TurmaDisciplina(tud_id, cal_id, out totalRecords).Rows
                     select (Struct_ObjetosAprendizagem)GestaoEscolarUtilBO.DataRowToEntity(dr, new Struct_ObjetosAprendizagem())).ToList();

            return dados;
        }

        /// <summary>
        /// Exclui o objeto de aprendizagem, verifica se não está sendo usado.
        /// </summary>
        /// <param name="entity">Entidade do objeto de aprendizagem que será excluído</param>
        /// <returns></returns>
        public static bool Excluir(ACA_ObjetoAprendizagem entity)
        {
            ACA_ObjetoAprendizagemDAO dao = new ACA_ObjetoAprendizagemDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (ObjetoEmUso(entity.oap_id, dao._Banco))
                    throw new ValidationException("Não foi possível excluir o objeto de conhecimento pois existem outros registros ligados a ele.");

                return Delete(entity);
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Verifica se o objeto de aprendizagem está em uso
        /// </summary>
        /// <param name="oap_id">ID do objeto de aprendizagem</param>
        /// <returns>true = em uso</returns>
        private static bool ObjetoEmUso(int oap_id, TalkDBTransaction banco = null)
        {
            ACA_ObjetoAprendizagemDAO dao = new ACA_ObjetoAprendizagemDAO();
            if (banco != null)
                dao._Banco = banco;

            return dao.ObjetoEmUso(oap_id);
        }
    }
}