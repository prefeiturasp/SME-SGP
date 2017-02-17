/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using MSTech.Data.Common;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estrutura

    /// <summary>
    /// Estrutura necessária para cadastrar as orientações curriculares.
    /// </summary>
    public struct ORC_Objetivo_Cadastro
    {
        public ORC_Objetivo entObjetivo;
        public List<ORC_Conteudo_Cadastro> listConteudos;
    }

    #endregion

    #region Enumerador

    /// <summary>
    /// Enumerador da situação do objetivo das orientações curriculares.
    /// </summary>
    public enum ORC_ObjetivoSituacao : byte
    {
        Ativo = 1
        , Excluído = 3
    }

    #endregion

    /// <summary>
	/// ORC_Objetivo Business Object 
	/// </summary>
	public class ORC_ObjetivoBO : BusinessBase<ORC_ObjetivoDAO,ORC_Objetivo>
	{
        /// <summary>
        /// Retorna os objetivos cadastrados para o CurriculoPeriodo e Tipo de disciplina.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do curriculoPeriodo</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPor_Curriculo_Disciplina
        (
             int cur_id
            , int crr_id
            , int crp_id
            , int tds_id
            , int cal_id
        )
        {
            ORC_ObjetivoDAO dao = new ORC_ObjetivoDAO();
            DataTable dt = dao.SelectBy_Curriculo_Disciplina(cur_id, crr_id, crp_id, tds_id, cal_id);

            //// Adiciona nova linha vazia.
            //DataRow dr = dt.NewRow();
            //dr["obj_id"] = -1;
            //dt.Rows.Add(dr);

            return dt;
        }

        /// <summary>
        /// Retorna os objetivos por curso, período e tipo de disciplina
        /// </summary>                
        /// <param name="cur_id">ID da curso</param>
        /// <param name="crr_id">ID da curriculo do curso</param>
        /// <param name="crp_id">ID da periodo do curriculo</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaObjetivoPorCursoPeriodoTipoDisciplina
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int tds_id
        )
        {
            ORC_ObjetivoDAO dao = new ORC_ObjetivoDAO();
            return dao.SelectBy_cur_id_crr_id_crp_id_tds_id(cur_id, crr_id, crp_id, tds_id);
        }

        /// <summary>
        /// Retorna os objetivos por turma, tipo de disciplina e tipo de periodo do calendário
        /// </summary>                
        /// <param name="tur_id">ID da turma</param>        
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="cur_idAtual">ID do curso</param>
        /// <param name="crr_idAtual">ID do currículo do curso</param>
        /// <param name="crp_idAtual">ID do período do currículo</param>
        /// <param name="crp_idAnterior">ID do período do currículo</param>
        /// <param name="naoAlcancadasAnoAnterior">Indica se vai trazer os dados do ano atual ou do anterior</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaObjetivoPorTurmaTipoDisciplina
        (
            long tur_id
            , long tud_id
            , int tds_id
            , int cur_idAtual
            , int crr_idAtual
            , int crp_idAtual
            , int crp_idAnterior
            , bool naoAlcancadasAnoAnterior
        )
        {
            ORC_ObjetivoDAO dao = new ORC_ObjetivoDAO();
            return dao.SelectBy_tur_id_tds_id(tur_id, tud_id, tds_id, cur_idAtual, crr_idAtual, crp_idAtual, crp_idAnterior, naoAlcancadasAnoAnterior);
        }

        /// <summary>
        /// Retorna os objetivos por turma disciplina
        /// </summary>                        
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="cur_idAtual">ID do curso</param>
        /// <param name="crr_idAtual">ID do currículo do curso</param>
        /// <param name="crp_idAtual">ID do período do currículo</param>
        /// <param name="crp_idAnterior">ID do período do currículo</param>
        /// <param name="tdt_posicao">Posição do docente responsável</param>
        /// <param name="cal_idAtual">ID do calendário</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaObjetivoPorTurmaDisciplina
        (
            long tud_id
            , int cur_idAtual
            , int crr_idAtual
            , int crp_idAtual
            , int crp_idAnterior
            , byte tdt_posicao
            , int cal_idAtual
        )
        {
            ORC_ObjetivoDAO dao = new ORC_ObjetivoDAO();
            return dao.SelectBy_tur_id_CurriculoPeriodo(tud_id, cur_idAtual, crr_idAtual, crp_idAtual, crp_idAnterior, tdt_posicao, cal_idAtual);
        }

        /// <summary>
        /// Verifica se existe um objetivo com a descrição passada no
        /// curso, currículo, período e disciplina passados com o id
        /// diferente do informado.
        /// </summary>    
        /// <param name="entity">Entidade ORC_Objetivo com o id, 
        /// descrição, crr_id, crp_id, cur_id e tds_id preenchidos.</param>
        public static bool VerificaNomeExistente(ORC_Objetivo entity)
        {
            ORC_ObjetivoDAO dao = new ORC_ObjetivoDAO();
            return dao.VerificaNomeExistente(entity.obj_id, entity.cur_id, entity.crp_id, entity.crr_id, entity.tds_id, entity.obj_descricao, entity.cal_id);
        }

        #region Salvar

        /// <summary>
        /// Salva todas as entidades dentro da estrutura, para salvar um objetivo dos componentes curriculares.
        /// </summary>
        /// <param name="cadastro">Estrutura a ser salva.</param>
        /// <returns></returns>
        public static bool Save(ORC_Objetivo_Cadastro cadastro)
        {
            TalkDBTransaction banco = new ORC_ObjetivoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                ORC_Objetivo entity = cadastro.entObjetivo;

                if (!entity.Validate())
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

                Save(entity, banco);

                // Salvar conteúdos.
                ORC_ConteudoBO.SalvarConteudos(cadastro.listConteudos, entity, banco);

                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        /// <summary>
        /// Deleta o objetivo da orientação curricular
        /// </summary>
        /// <param name="entity">Entidade ORC_Objetivo</param>        
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ORC_Objetivo entity
        )
        {
            ORC_ObjetivoDAO dao = new ORC_ObjetivoDAO();

            // Verifica se o objetivo da orientação curricular pode ser deletado
            if (GestaoEscolarUtilBO.VerificarIntegridade
            (
                "obj_id"
                , entity.obj_id.ToString()
                , "ORC_Objetivo,ORC_Conteudo,ORC_ConteudoItem,ORC_Habilidades,ORC_ConteudoTipoPeriodoCalendario"
                , dao._Banco
            ))
            {
                throw new ValidationException("Não é possível excluir o objetivo da orientação curricular, pois possui outros registros ligados a ele.");
            }

            // Deleta logicamente o objetivo da orientação curricular
            dao.Delete(entity);

            return true;
        }

        #endregion
    }
}