/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

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
    public enum ORC_ConteudoItemSituacao : byte
    {
        Ativo = 1
        , Excluído = 3
    }

    #endregion

	/// <summary>
	/// ORC_ConteudoItem Business Object 
	/// </summary>
	public class ORC_ConteudoItemBO : BusinessBase<ORC_ConteudoItemDAO,ORC_ConteudoItem>
	{
        /// <summary>
        /// Retorna os itens de conteúdo cadastrados para o conteúdo.
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
            ORC_ConteudoItemDAO dao = new ORC_ConteudoItemDAO();
            DataTable dt = dao.SelectBy_Conteudo(obj_id, ctd_id);

            if (obj_id <= 0)
            {
                // Adiciona uma linha nova, para retornar uma linha para edição na tela.
                DataRow dr = dt.NewRow();
                dr["obj_id"] = -1;
                dr["ctd_id"] = -1;
                dr["cti_id"] = -1;
                dt.Rows.Add(dr);
            }

            return dt;
        }	

        /// <summary>
        /// Retorna os itens do conteúdo do objetivo
        /// </summary>                
        /// <param name="obj_id">ID do objetivo</param>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="naoAlcancadasAnoAnterior">Indica se vai trazer os dados do ano atual ou do anterior</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaConteudoItemPorObjetivo
        (            
            int obj_id
            , long tud_id
            , bool naoAlcancadasAnoAnterior
        )
        {
            ORC_ConteudoItemDAO dao = new ORC_ConteudoItemDAO();
            return dao.SelectBy_obj_id(obj_id, tud_id, naoAlcancadasAnoAnterior);
        }

        /// <summary>
        /// Verifica se já existe um conteúdo cadastrado com o mesmo nome
        /// no mesmo objetivo e com cti_id diferente do passado.
        /// </summary>
        /// <param name="obj_id">Id do objetivo.</param>
        /// <param name="cti_id">Id do contéudo item.</param>
        /// <param name="cti_descricao">Descrição do conteúdo.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaConteudoItemPorNome_Objetivo
        (
            int obj_id
            , int cti_id
            , string cti_descricao
        )
        {
            ORC_ConteudoItemDAO dao = new ORC_ConteudoItemDAO();
            return dao.SelectBy_Nome(obj_id, cti_id, cti_descricao);
        }

        /// <summary>
        /// Retorna os itens do conteúdos dos objetivos da turma disciplina e currículo período
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="cur_idAtual">ID do curso</param>
        /// <param name="crr_idAtual">ID do curriculo</param>
        /// <param name="crp_idAtual">ID do currículo período atual</param>
        /// <param name="crp_idAnterior">ID do currículo período anterior</param>
        /// <param name="tdt_posicao">Posição do docente responsável</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTurmaDisciplinaCurriculoPeriodo
        (
            long tud_id
            , int cur_idAtual
            , int crr_idAtual
            , int crp_idAtual
            , int crp_idAnterior
            , byte tdt_posicao
        )
        {
            ORC_ConteudoItemDAO dao = new ORC_ConteudoItemDAO();
            return dao.SelecionaPorTurmaDisciplinaCurriculoPeriodo(tud_id, cur_idAtual, crr_idAtual, crp_idAtual, crp_idAnterior, tdt_posicao);
        }

        #region Salvar

        /// <summary>
        /// Salva os registros de itens para o conteúdo.
        /// </summary>
        /// <param name="entConteudo">Entidade conteúdo</param>
        /// <param name="list">Lista de itens para salvar</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        internal static void SalvarItensConteudo(ORC_Conteudo entConteudo, List<ORC_ConteudoItem> list, TalkDBTransaction banco)
        {
            foreach (ORC_ConteudoItem entity in list)
            {
                entity.obj_id = entConteudo.obj_id;
                entity.ctd_id = entConteudo.ctd_id;
                entity.IsNew = entity.cti_id <= 0;

                if (!string.IsNullOrEmpty(entity.cti_descricao))
                {
                    if (!entity.Validate())
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

                    // Verifica se já existe um conteúdo item com a mesma descrição no mesmo objetivo.
                    if (VerificaConteudoItemPorNome_Objetivo(entity.obj_id, entity.cti_id, entity.cti_descricao))
                        throw new ValidationException("Já existe um conteúdo cadastrado com a descrição " + entity.cti_descricao + " no objetivo.");

                    Save(entity, banco);
                }
                else
                {
                    if (entity.cti_id > 0)
                    {
                        Delete(entity, banco);
                    }
                }
            }
        }
        
        #endregion
    }
}