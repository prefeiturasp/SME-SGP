using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class CFG_ConfiguracaoInscricaoCrecheBO : BusinessBase<CFG_ConfiguracaoInscricaoCrecheDAO, CFG_ConfiguracaoInscricaoCreche>
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
        /// <returns>List de entidades CFG_ConfiguracaoInscricaoCreche</returns>
        public static List<CFG_ConfiguracaoInscricaoCreche> Consultar()
        {
            CFG_ConfiguracaoInscricaoCrecheDAO dao = new CFG_ConfiguracaoInscricaoCrecheDAO();
            return dao.Select(false, 0, 0, out totalRecords);
        }

        /// <summary>
        /// Retorna as configurações ativas.
        /// </summary>
        /// <param name="paginado">Indica se será paginado</param>
        /// <param name="currentPage">Página atual</param>
        /// <param name="pageSize">Quantidade de registros por página</param>
        /// <returns>List de entidades CFG_ConfiguracaoInscricaoCreche</returns>
        public static List<CFG_ConfiguracaoInscricaoCreche> Consultar
            (
                 bool paginado
                , int currentPage
                , int pageSize
            )
        {
            CFG_ConfiguracaoInscricaoCrecheDAO dao = new CFG_ConfiguracaoInscricaoCrecheDAO();
            return dao.Select(paginado, currentPage, pageSize, out totalRecords);
        }

        /// <summary>
        /// Verifica se existe uma configuração que possua a mesma chave.
        /// </summary>
        /// <param name="entity">Entidade CFG_Configuracao</param>
        /// <returns></returns>
        public static bool VerificarChaveExistente(CFG_ConfiguracaoInscricaoCreche entity)
        {
            CFG_ConfiguracaoInscricaoCrecheDAO dao = new CFG_ConfiguracaoInscricaoCrecheDAO();
            return dao.SelectBy_cfg_chave(entity);
        }

        /// <summary>
        ///  Salva (inclusão ou alteração) uma configuração.
        /// </summary>
        /// <param name="entity">Entidade CFG_Configuracao</param>
        /// <returns></returns>
        public static bool Salvar(CFG_ConfiguracaoInscricaoCreche entity)
        {
            CFG_ConfiguracaoInscricaoCrecheDAO dao = new CFG_ConfiguracaoInscricaoCrecheDAO();

            // Verifica chave da configuração
            if (VerificarChaveExistente(entity))
                throw new DuplicateNameException("Já existe uma configuração cadastrada com esta chave.");

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
        public static bool Deletar(CFG_ConfiguracaoInscricaoCreche entity)
        {
            CFG_ConfiguracaoInscricaoCrecheDAO dao = new CFG_ConfiguracaoInscricaoCrecheDAO();

            // Verifica situação da configuração
            if (entity.cfg_situacao == Convert.ToByte(eSituacao.Interno))
                throw new ValidationException("A configuração possui situação obrigatória, não pode ser excluída.");

            return dao.Delete(entity);
        }
    }
}
