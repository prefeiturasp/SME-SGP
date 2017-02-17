using System;
using System.Collections.Generic;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class CFG_ConfiguracaoAcademicoBO : BusinessBase<CFG_ConfiguracaoAcademicoDAO, CFG_ConfiguracaoAcademico>
    {
        #region Enumerador

        public enum eSituacao
        {
            Ativo = 1
                ,
            Interno = 4
        }

        #endregion

        /// <summary>
        /// Retorna as configurações ativas.
        /// </summary>
        /// <returns>List de entidades CFG_ConfiguracaoAcademico</returns>
        public static List<CFG_ConfiguracaoAcademico> Consultar()
        {
            CFG_ConfiguracaoAcademicoDAO dao = new CFG_ConfiguracaoAcademicoDAO();
            return dao.Select(false, 0, 0, out totalRecords);
        }

        /// <summary>
        /// Retorna as configurações ativas.
        /// </summary>
        /// <param name="paginado">Indica se será paginado</param>
        /// <param name="currentPage">Página atual</param>
        /// <param name="pageSize">Quantidade de registros por página</param>
        /// <returns>List de entidades CFG_ConfiguracaoAcademico</returns>
        public static List<CFG_ConfiguracaoAcademico> Consultar
            (
                 bool paginado
                , int currentPage
                , int pageSize
            )
        {
            CFG_ConfiguracaoAcademicoDAO dao = new CFG_ConfiguracaoAcademicoDAO();
            return dao.Select(paginado, currentPage, pageSize, out totalRecords);
        }

        /// <summary>
        /// Verifica se existe uma configuração que possua a mesma chave.
        /// </summary>
        /// <param name="entity">Entidade CFG_Configuracao</param>
        /// <returns></returns>
        public static bool VerificarChaveExistente(CFG_ConfiguracaoAcademico entity)
        {
            CFG_ConfiguracaoAcademicoDAO dao = new CFG_ConfiguracaoAcademicoDAO();
            return dao.SelectBy_cfg_chave(entity);
        }

        /// <summary>
        ///  Salva (inclusão ou alteração) uma configuração.
        /// </summary>
        /// <param name="entity">Entidade CFG_Configuracao</param>
        /// <returns></returns>
        public static bool Salvar(CFG_ConfiguracaoAcademico entity)
        {
            CFG_ConfiguracaoAcademicoDAO dao = new CFG_ConfiguracaoAcademicoDAO();

            // Verifica chave da configuração
            if (VerificarChaveExistente(entity))
                throw new DuplicateNameException("Já existe uma configuração cadastrada com esta chave.");
            if (entity.cfg_chave == "appPaginacao")
            {
                if(entity.cfg_valor=="0")
                    throw new ValidationException("appPaginacao não pode ser igual a 0.");
            }
            if (entity.Validate())
                return dao.Salvar(entity);
            else
                throw new ValidationException(MSTech.CoreSSO.BLL.UtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Deleta uma configuração.
        /// </summary>
        /// <param name="entityConfiguracao">Entidade CFG_Configuracao</param>
        /// <returns></returns>
        public static bool Deletar(CFG_ConfiguracaoAcademico entity)
        {
            CFG_ConfiguracaoAcademicoDAO dao = new CFG_ConfiguracaoAcademicoDAO();

            // Verifica situação da configuração
            if (entity.cfg_situacao == Convert.ToByte(eSituacao.Interno))
                throw new ValidationException("A configuração possui situação obrigatória, não pode ser excluída.");

            return dao.Delete(entity);
        }
    }
}
