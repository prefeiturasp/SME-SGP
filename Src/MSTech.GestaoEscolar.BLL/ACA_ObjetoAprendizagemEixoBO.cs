/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Collections.Generic;
    using Data.Common;
    using System.Data;
    using Validation.Exceptions;
    /// <summary>
    /// Description: ACA_ObjetoAprendizagemEixo Business Object. 
    /// </summary>
    public class ACA_ObjetoAprendizagemEixoBO : BusinessBase<ACA_ObjetoAprendizagemEixoDAO, ACA_ObjetoAprendizagemEixo>
    {
        public static bool Excluir(ACA_ObjetoAprendizagemEixo entity)
        {
            ACA_ObjetoAprendizagemEixoDAO dao = new ACA_ObjetoAprendizagemEixoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (ObjetoEmUso(entity.oae_id, dao._Banco))
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
        /// Retorna os eixos por disciplina e ano letivo.
        /// </summary>
        /// <param name="tds_id">ID da disciplina</param>
        /// <param name="cal_ano">Ano letivo</param>
        /// <param name="oae_idPai">ID do eixo pai</param>
        /// <returns></returns>
        public static List<ACA_ObjetoAprendizagemEixo> SelectByDiscAno(int tds_id, int cal_ano, int oae_idPai)
        {
            ACA_ObjetoAprendizagemEixoDAO dao = new ACA_ObjetoAprendizagemEixoDAO();
            return dao.SelectBy_TipoDisciplina(tds_id, cal_ano, oae_idPai);
        }

        /// <summary>
        /// Altera a ordem dos eixos de objetos de conhecimento
        /// </summary>
        /// <param name="entitySubir">Entidade que vai subir (já com a ordem preenchida)</param>
        /// <param name="entityDescer">Entidade que vai descer (já com a ordem preenchida)</param>
        /// <returns></returns>
        public static bool SalvarOrdem(ACA_ObjetoAprendizagemEixo entitySubir, ACA_ObjetoAprendizagemEixo entityDescer, TalkDBTransaction banco = null)
        {
            ACA_ObjetoAprendizagemEixoDAO dao = new ACA_ObjetoAprendizagemEixoDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;
            try
            {
                return dao.Salvar(entityDescer) && dao.Salvar(entitySubir);
            }
            catch (Exception ex)
            {
                if (banco == null)
                    dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Verifica se o eixo de objeto de aprendizagem está em uso
        /// </summary>
        /// <param name="oae_id">ID do eixo de objeto de aprendizagem</param>
        /// <returns>true = em uso</returns>
        private static bool ObjetoEmUso(int oae_id, TalkDBTransaction banco = null)
        {
            ACA_ObjetoAprendizagemEixoDAO dao = new ACA_ObjetoAprendizagemEixoDAO();
            if (banco != null)
                dao._Banco = banco;

            return dao.ObjetoEmUso(oae_id);
        }

        /// <summary>
        /// Verifica se já existe um eixo cadastrado com o mesmo nome e salva
        /// </summary>
        /// <param name="oae">Objeto de eixo que está sendo salvo</param>
        /// <returns></returns>
        public static bool Salvar(ACA_ObjetoAprendizagemEixo oae, TalkDBTransaction banco = null)
        {
            ACA_ObjetoAprendizagemEixoDAO dao = new ACA_ObjetoAprendizagemEixoDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;
            try
            {
                if (VerificaEixoMesmoNome(oae.oae_id, oae.tds_id, oae.cal_ano, oae.oae_idPai, oae.oae_descricao, dao._Banco))
                    throw new ValidationException("Já existe um " + (oae.oae_idPai > 0 ? "sub " : "") +
                                                  "eixo de objeto de conhecimento cadastrado com a mesma descrição.");

                return dao.Salvar(oae);
            }
            catch (Exception ex)
            {
                if (banco == null)
                    dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Verifica se existe um eixo cadastrado com o mesmo nome (se for sub eixo verifica apenas os sub eixos do eixo pai)
        /// </summary>
        /// <param name="oae_id">ID do eixo que está sendo salvo</param>
        /// <param name="tds_id">ID da disciplina</param>
        /// <param name="cal_ano">Ano letivo</param>
        /// <param name="oae_idPai">ID do eixo pai</param>
        /// <param name="oae_descricao">Descrição do eixo</param>
        /// <param name="banco">Transação do banco</param>
        /// <returns></returns>
        private static bool VerificaEixoMesmoNome(int oae_id, int tds_id, int cal_ano, int oae_idPai, string oae_descricao, TalkDBTransaction banco = null)
        {
            ACA_ObjetoAprendizagemEixoDAO dao = new ACA_ObjetoAprendizagemEixoDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.VerificaEixoMesmoNome(oae_id, tds_id, cal_ano, oae_idPai, oae_descricao);
        }
    }
}