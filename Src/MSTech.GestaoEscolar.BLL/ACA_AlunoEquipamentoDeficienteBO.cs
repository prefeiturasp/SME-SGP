/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.ComponentModel;
using MSTech.Data.Common;
using MSTech.Validation.Exceptions;
using System.Data;
using System.Collections.Generic;
using System;

namespace MSTech.GestaoEscolar.BLL
{
	
	/// <summary>
	/// ACA_AlunoEquipamentoDeficiente Business Object 
	/// </summary>
	public class ACA_AlunoEquipamentoDeficienteBO : BusinessBase<ACA_AlunoEquipamentoDeficienteDAO,ACA_AlunoEquipamentoDeficiente>
    {
        #region Consultas

        /// <summary>
        /// Retorna todos os tipos de equipamento para deficientes filtrado por aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns></returns>
        public static DataTable SelecionaPorAluno(long alu_id)
        {
            ACA_AlunoEquipamentoDeficienteDAO dao = new ACA_AlunoEquipamentoDeficienteDAO();
            return dao.SelecionaPorAluno(alu_id);
        }

        #endregion

        #region Incluir ou alterar

        /// <summary>
        /// Inclui um novo tipo de equipamento para deficientes para o aluno
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoEquipamentoDeficiente</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save(ACA_AlunoEquipamentoDeficiente entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                ACA_AlunoEquipamentoDeficienteDAO dao = new ACA_AlunoEquipamentoDeficienteDAO { _Banco = banco };
                return dao.Salvar(entity);
            }
            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Salva uma lista de tipos de equipamento para deficiente para o aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="ltEquipamentoDeficiente">Lista de  tipos de equipamento para deficiente</param>
        /// <param name="banco"></param>
        public static void SalvarEquipamentoDeficienteAluno(long alu_id, List<int> ltEquipamentoDeficiente, TalkDBTransaction banco)
        {
            DataTable dt = SelecionaPorAluno(alu_id);

            foreach (int ted_id in ltEquipamentoDeficiente)
            {
                ACA_AlunoEquipamentoDeficiente entity = new ACA_AlunoEquipamentoDeficiente
                {
                    alu_id = alu_id
                    ,
                    ted_id = ted_id
                };

                GetEntity(entity, banco);

                if (entity.IsNew)
                {
                    Save(entity, banco);
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                if (!ltEquipamentoDeficiente.Exists(p => p == Convert.ToInt32(dr["ted_id"])))
                {
                    ACA_AlunoEquipamentoDeficiente entity = new ACA_AlunoEquipamentoDeficiente
                    {
                        alu_id = alu_id
                        ,
                        ted_id = Convert.ToInt32(dr["ted_id"])
                    };

                    Delete(entity, banco);
                }
            }
        }

        #endregion
    }
}