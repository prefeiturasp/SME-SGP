using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region ENUMERADOR

    /// <summary>
    /// Relatório que será utilizado o parâmetro.
    /// </summary>
    public enum ParametroCorRelatorio : byte
    {
        DocDctGraficoAtividadeAvaliativa = 244
    }

    #endregion ENUMERADOR

    #region Consultas

    /// <summary>
	/// Description: CFG_CorRelatorio Business Object. 
	/// </summary>
	public class CFG_CorRelatorioBO : BusinessBase<CFG_CorRelatorioDAO, CFG_CorRelatorio>
	{
      
        /// <summary>
        /// Selecionas the cores relatorio.
        /// </summary>
        /// <param name="rlt_id">The rlt_id.</param>
        /// <returns></returns>
        /// <author>juliano.real</author>
        /// <datetime>22/08/2014-16:25</datetime>
        public static List<CFG_CorRelatorio> SelecionaCoresRelatorio(int rlt_id)
        {
           return new CFG_CorRelatorioDAO().SelecionaCoresRelatorio(rlt_id);
        }

        /// <summary>
        /// Retorna todos as cores não excluídos logicamente
        /// Sem paginação
        /// <param name="rlt_id">The rlt_id.</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// </summary>            
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCoresNaoPaginado(int rlt_id, Guid ent_id)
        {
            bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

            CFG_CorRelatorioDAO dao = new CFG_CorRelatorioDAO();
            return dao.SelectBy_Pesquisa(rlt_id, out totalRecords);
        }

        #endregion 

        #region Inserir

        /// <summary>
        /// Altera a ordem da cor do relatorio
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo corRelatorio</param>
        /// <param name="entityDescer">Entidade do tipo corRelatorio</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            CFG_CorRelatorio entityDescer
            , CFG_CorRelatorio entitySubir
        )
        {
            CFG_CorRelatorioDAO dao = new CFG_CorRelatorioDAO();

            if (entityDescer.Validate())
                dao.Salvar(entityDescer);
            else
                throw new ValidationException(entityDescer.PropertiesErrorList[0].Message);

            if (entitySubir.Validate())
                dao.Salvar(entitySubir);
            else
                throw new ValidationException(entitySubir.PropertiesErrorList[0].Message);

            return true;
        }

        public static bool SalvarCor(CFG_CorRelatorio entity)
        {           
            CFG_CorRelatorioDAO dao = new CFG_CorRelatorioDAO();      
            
            //if (dao.ValidaCor(entity))
            //    throw new DuplicateNameException("Essa cor já está cadastrada.");
            if (!dao.ValidaCor(entity))
                dao.Salvar(entity);
            else
                throw new ValidationException(entity.PropertiesErrorList[0].Message);

            return true;
        }

        #endregion

    }
}