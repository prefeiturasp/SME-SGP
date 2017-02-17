/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumerador

    /// <summary>
    /// Enumerador da situação do registro.
    /// </summary>
    public enum ORC_HabilidadesSituacao : byte
    {
        Ativo = 1
        , Excluído = 3
    }

    #endregion

	/// <summary>
	/// ORC_Habilidades Business Object 
	/// </summary>
	public class ORC_HabilidadesBO : BusinessBase<ORC_HabilidadesDAO,ORC_Habilidades>
	{
        /// <summary>
        /// Retorna as habilidades cadastradas para o conteúdo.
        /// </summary>
        /// <param name="obj_id">ID do objetivo</param>
        /// <param name="ctd_id">ID do conteúdo</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPor_Conteudo
        (
            int obj_id
            , int ctd_id
        )
        {
            ORC_HabilidadesDAO dao = new ORC_HabilidadesDAO();
            DataTable dt = dao.SelectBy_Conteudo(obj_id, ctd_id);

            if (obj_id <= 0)
            {
                // Adiciona uma linha nova, para retornar uma linha para edição na tela.
                DataRow dr = dt.NewRow();
                dr["obj_id"] = -1;
                dr["ctd_id"] = -1;
                dr["hbl_id"] = -1;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Verifica se já existe uma habilidade cadastrada com o mesmo nome
        /// no objetivo e conteúdo passado com o hbl_id diferente do passado.
        /// </summary>
        /// <param name="obj_id">Id do objetivo.</param>
        /// <param name="ctd_id">Id do conteúdo.</param>
        /// <param name="hbl_id">Id da habilidade.</param>
        /// <param name="hbl_descricao">Descrição da habilidade.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaHabilidadePorNome_Objetivo_Conteudo
        (
            int obj_id
            , int ctd_id
            , int hbl_id
            , string hbl_descricao
        )
        {
            ORC_HabilidadesDAO dao = new ORC_HabilidadesDAO();
            return dao.SelectBy_Nome_Objetivo_Conteudo(obj_id, ctd_id, hbl_id, hbl_descricao);
        }

        #region Salvar

        /// <summary>
        /// Salva os registros de habilidades para o conteúdo.
        /// </summary>
        /// <param name="entConteudo">Entidade conteúdo</param>
        /// <param name="list">Lista de habilidades para salvar</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        internal static void SalvarHabilidadesConteudo(ORC_Conteudo entConteudo, List<ORC_Habilidades> list, TalkDBTransaction banco)
        {
            foreach (ORC_Habilidades entity in list)
            {
                entity.obj_id = entConteudo.obj_id;
                entity.ctd_id = entConteudo.ctd_id;
                entity.IsNew = entity.hbl_id <= 0;

                if (!string.IsNullOrEmpty(entity.hbl_descricao))
                {
                    if (!entity.Validate())
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

                    // Verifica se já existe uma habilidade cadastrada com a mesma descrição, no mesmo objetivo e conteúdo.
                    if (VerificaHabilidadePorNome_Objetivo_Conteudo(entity.obj_id, entity.ctd_id, entity.hbl_id, entity.hbl_descricao))
                        throw new ValidationException("Já existe uma habilidade cadastrada com a descrição " + entity.hbl_descricao + " no mesmo conteúdo.");

                    Save(entity, banco);
                }
                else
                {
                    if (entity.hbl_id > 0)
                    {
                        Delete(entity, banco);
                    }
                }
            }
        }

        /// <summary>
        /// Deleta o conteúdo do objetivo da orientação curricular
        /// </summary>
        /// <param name="entity">Entidade ORC_Habilidades</param>    
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>    
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ORC_Habilidades entity
            , TalkDBTransaction banco
        )
        {
            ORC_HabilidadesDAO dao = new ORC_HabilidadesDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {

                // Verifica se o conteúdo do objetivo da orientação curricular pode ser deletado
                if (GestaoEscolarUtilBO.VerificaIntegridadaChaveTripla
                (
                    "obj_id"
                    , "ctd_id"
                    , "hbl_id"
                    , entity.obj_id.ToString()
                    , entity.ctd_id.ToString()
                    , entity.hbl_id.ToString()
                    , "ORC_Objetivo,ORC_Conteudo,ORC_ConteudoItem,ORC_Habilidades,ORC_ConteudoTipoPeriodoCalendario"
                    , dao._Banco
                ))
                {
                    throw new ValidationException("Não é possível excluir a habilidade do objetivo da orientação curricular, pois possui outros registros ligados a ela.");
                }

                // Deleta logicamente o conteúdo do objetivo da orientação curricular
                dao.Delete(entity);

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

        #endregion
	}
}