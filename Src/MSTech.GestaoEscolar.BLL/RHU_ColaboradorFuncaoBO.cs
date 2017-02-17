using System;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações da função do colaborador
    /// </summary>
    public enum RHU_ColaboradorFuncaoSituacao : byte
    {
        Ativo = 1
        ,
        Excluido = 3
        ,
        Designado = 4
        ,
        Afastado = 5
        ,
        Desativado = 6
    }

    #endregion

    public class RHU_ColaboradorFuncaoBO : BusinessBase<RHU_ColaboradorFuncaoDAO, RHU_ColaboradorFuncao>    
    {
        /// <summary>
        /// Seleciona o id da última função cadastrada por colaborador/função + 1
        /// se não houver função cadastrada para o colaborador/função retorna 1
        /// filtrados por col_id, fun_id
        /// </summary>
        /// <param name="col_id">Campo col_id da tabela RHU_ColaboradorFuncao do bd</param>        
        /// <param name="fun_id">Campo fun_id da tabela RHU_ColaboradorFuncao do bd</param>        
        /// <returns>cof_id + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 VerificaUltimaFuncaoCadastrada
        (
            long col_id
            , int fun_id
        )
        {
            RHU_ColaboradorFuncaoDAO dao = new RHU_ColaboradorFuncaoDAO();
            return dao.SelectBy_col_id_fun_id_top_one(col_id, fun_id);
        }

        /// <summary>
        /// Retorno booleano na qual verifica se já existe um resposável
        /// pela UA dentro de um determinado período           
        /// </summary>
        /// <returns>True/False</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaVigenciaResponsavelUA
        (
            long col_id
            , int fun_id
            , int cof_id
            , Guid ent_id
            , Guid uad_id
            , DateTime cof_vigenciaInicio
            , DateTime cof_vigenciaFim
        )
        {
            RHU_ColaboradorFuncaoDAO dao = new RHU_ColaboradorFuncaoDAO();
            return dao.SelectBy_VigenciaResponsavelUA(col_id, fun_id, cof_id, ent_id, uad_id, cof_vigenciaInicio, cof_vigenciaFim);
        }

        /// <summary>
        /// Inclui uma nova função para o coloborador
        /// </summary>
        /// <param name="entity">Entidade RHU_ColaboradorCargo</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            RHU_ColaboradorFuncao entity
            , TalkDBTransaction banco
        )
        {            
            //Verifica se já existe algum outro colaborador responsável pela unidade administrativa
            if (entity.cof_responsavelUa)
            {
                int cof_id = entity.IsNew ? -1 : entity.cof_id;
                if (VerificaVigenciaResponsavelUA(entity.col_id, entity.fun_id, cof_id, entity.ent_id, entity.uad_id, entity.cof_vigenciaInicio, entity.cof_vigenciaFim))
                    throw new ArgumentException("Não pode existir mais de um colaborador responsável por uma mesma unidade administrativa ao mesmo tempo.");
            }

            if (entity.Validate())
            {
                RHU_ColaboradorFuncaoDAO dao = new RHU_ColaboradorFuncaoDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }
    }
}
