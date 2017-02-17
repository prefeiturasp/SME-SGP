/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;

namespace MSTech.GestaoEscolar.BLL
{
	
	/// <summary>
	/// ACA_TipoEquipamentoDeficiente Business Object 
	/// </summary>
	public class ACA_TipoEquipamentoDeficienteBO : BusinessBase<ACA_TipoEquipamentoDeficienteDAO,ACA_TipoEquipamentoDeficiente>
	{
        /// <summary>
        /// Retorna todos os tipos de equipamentos para deficientes.
        /// </summary> 
        /// <returns>DataTable contendo os tipos de equipamentos de deficientes </returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoEquipamentoDeficiente()
        {
            ACA_TipoEquipamentoDeficienteDAO dao = new ACA_TipoEquipamentoDeficienteDAO();
            return dao.SelectBy_Pesquisa(out totalRecords);
        }

        /// <summary>
        /// Verifica se já existe um tipo de equipamento para deficienete cadastrado com o mesmo nome.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoEquipamentoDeficiente</param> 
        public static bool VerificaNomeExistente(ACA_TipoEquipamentoDeficiente entity)
        {
            ACA_TipoEquipamentoDeficienteDAO dao = new ACA_TipoEquipamentoDeficienteDAO();
            return dao.SelectBy_Nome(entity.ted_id, entity.ted_nome);
        }

        /// <summary>
        /// Inclui ou altera o tipo de equipamento de deficienete.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoEquipamentoDeficiente</param>
        public new static bool Save(ACA_TipoEquipamentoDeficiente entity)
        {
            if (entity.Validate())
            {
                if (VerificaNomeExistente(entity))
                    throw new DuplicateNameException(
                        CustomResource.GetGlobalResourceObject("BLL", "TipoEquipamentoDeficiente.ValidaDuplicidade")
                        );

                ACA_TipoEquipamentoDeficienteDAO dao = new ACA_TipoEquipamentoDeficienteDAO();
                return dao.Salvar(entity);
            }

            throw new ValidationException(UtilBO.ErrosValidacao(entity));
        }
        
        /// <summary>
        /// Deleta logicamente um tipo de equipamento para deficiente
        /// </summary>
        /// <param name="entity">ACA_TipoEquipamentoDeficiente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete(ACA_TipoEquipamentoDeficiente entity)
        {
            ACA_TipoEquipamentoDeficienteDAO dao = new ACA_TipoEquipamentoDeficienteDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {

                if (GestaoEscolarUtilBO.VerificarIntegridade("ted_id"
                                       , entity.ted_id.ToString()
                                       , "ACA_TipoEquipamentoDeficiente"
                                       , dao._Banco))
                {
                    throw new ValidationException(
                        CustomResource.GetGlobalResourceObject("BLL", "TipoEquipamentoDeficiente.ErroAoExcluir")
                        );
                }
                return dao.Delete(entity);
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
    }
}