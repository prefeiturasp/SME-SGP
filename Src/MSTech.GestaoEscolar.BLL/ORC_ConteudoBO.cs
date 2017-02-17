/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Data.Common;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estrutura

    /// <summary>
    /// Estrutura necessária para salvar o conteúdo das orientações curriculares.
    /// </summary>
    public struct ORC_Conteudo_Cadastro
    {
        public ORC_Conteudo entConteudo;
        public List<ORC_ConteudoItem> listItensConteudo;
        public List<ORC_Habilidades> listHabilidades;
        public List<int> listPeriodos;
    }

    #endregion

    /// <summary>
    /// ORC_Conteudo Business Object 
    /// </summary>
    public class ORC_ConteudoBO : BusinessBase<ORC_ConteudoDAO, ORC_Conteudo>
    {
        /// <summary>
        /// Retorna os conteúdos cadastrados para o objetivo.
        /// </summary>
        /// <param name="obj_id">ID do objetivo</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPor_Objetivo
        (
            int obj_id
        )
        {
            ORC_ConteudoDAO dao = new ORC_ConteudoDAO();
            DataTable dt = dao.SelectBy_Objetivo(obj_id);

            if (obj_id <= 0)
            {
                // Adiciona uma linha nova, para retornar uma linha para edição na tela.
                DataRow dr = dt.NewRow();
                dr["obj_id"] = -1;
                dr["ctd_id"] = -1;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #region Salvar

        /// <summary>
        /// Salva as entiades dentro da lista de conteúdos relacionados ao objetivo.
        /// </summary>
        /// <param name="listConteudos">Lista de conteúdos a serem salvos.</param>
        /// <param name="entObjetivo">Objetivo</param>
        /// <param name="banco">Transação com banco - obrigatório.</param>
        internal static void SalvarConteudos(List<ORC_Conteudo_Cadastro> listConteudos, ORC_Objetivo entObjetivo, TalkDBTransaction banco)
        {

            if (listConteudos.Count == 0)
                throw new ValidationException("Necessário cadastrar no mínimo um conteúdo para o objetivo.");  

            foreach (ORC_Conteudo_Cadastro conteudo in listConteudos)
            {
                ORC_Conteudo entity = conteudo.entConteudo;

                // Verifica se o conteúdo já existe no banco
                ORC_Conteudo aux = new ORC_Conteudo {obj_id = entObjetivo.obj_id, ctd_id = entity.ctd_id};
                GetEntity(aux, banco);

                entity.obj_id = entObjetivo.obj_id;
                entity.IsNew = aux.IsNew;

                if (!entity.Validate())
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

                if (conteudo.listItensConteudo.Count == 0)
                    throw new ValidationException("Necessário cadastrar no mínimo um item para o conteúdo.");
               
                if (conteudo.listHabilidades.Count == 0)
                    throw new ValidationException("Necessário cadastrar no mínimo uma habilidade para o conteúdo.");
                
                // Salva somente se tiver sub-item cadastrado.
                if ((conteudo.listItensConteudo.Count > 0) ||
                    (conteudo.listHabilidades.Count > 0) ||
                    (conteudo.listPeriodos.Count > 0))
                {
                    if (entity.IsNew)
                        Save(entity, banco);
                }

                // Salvar sub-cadastros.
                ORC_ConteudoItemBO.SalvarItensConteudo(entity, conteudo.listItensConteudo, banco);
                ORC_HabilidadesBO.SalvarHabilidadesConteudo(entity, conteudo.listHabilidades, banco);
                ORC_ConteudoTipoPeriodoCalendarioBO.SalvarPeriodos(entity, conteudo.listPeriodos, banco);
            }
        }

        /// <summary>
        /// Deleta o conteúdo do objetivo da orientação curricular
        /// </summary>
        /// <param name="entity">Entidade ORC_Conteudo</param>        
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ORC_Conteudo entity
        )
        {
            ORC_ConteudoDAO dao = new ORC_ConteudoDAO();

            // Verifica se o conteúdo do objetivo da orientação curricular pode ser deletado
            if (GestaoEscolarUtilBO.VerificaIntegridadaChaveDupla
            (
                "obj_id"
                , "ctd_id"
                , entity.obj_id.ToString()
                , entity.ctd_id.ToString()
                , "ORC_Objetivo,ORC_Conteudo,ORC_ConteudoItem,ORC_Habilidades,ORC_ConteudoTipoPeriodoCalendario"
                , dao._Banco
            ))
            {
                throw new ValidationException("Não é possível excluir o conteúdo do objetivo da orientação curricular, pois possui outros registros ligados a ele.");
            }

            if (dao.VerificaConteudosBy_obj_id(entity.obj_id, entity.ctd_id) > 0)
            {
                // Deleta logicamente o conteúdo do objetivo da orientação curricular
                dao.Delete(entity);
            }
            else
            {
                throw new ValidationException("Não é possível excluir o conteúdo do objetivo da orientação curricular, pois é necessário no mínimo um conteúdo para o objetivo.");
            }

            return true;
        }

        #endregion
    }
}