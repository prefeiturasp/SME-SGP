/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;

namespace MSTech.GestaoEscolar.BLL
{
	
	/// <summary>
	/// ACA_FichaMedicaContato Business Object 
	/// </summary>
	public class ACA_FichaMedicaContatoBO : BusinessBase<ACA_FichaMedicaContatoDAO,ACA_FichaMedicaContato>
	{

        /// <summary>
        /// Faz a busca de todas fichas médicas contatos do aluno.
        /// filtrado por alu_id
        /// </summary>
        /// <returns>Um datatable dos contatos da ficha médica do aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect_By_Aluno
        (
             long alu_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {

            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_FichaMedicaContatoDAO dao = new ACA_FichaMedicaContatoDAO();
            try
            {
                return dao.SelectBy_Aluno(alu_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todos as ficha médica de contatos responsaveis
        /// filtrados por alu_id.   
        /// </summary>
        /// <returns>Um datatable dos contatos da ficha médica do aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectContatosResponsaveis_By_Aluno(long alu_id)
        {

            ACA_FichaMedicaContatoDAO dao = new ACA_FichaMedicaContatoDAO();
            try
            {
                return dao.SelectContatosResponsaveisBy_Aluno(alu_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Salva os contatos da fica médica do aluno.
        /// </summary>
        /// <param name="bancoGestao">Transação - obrigatório</param>
        /// <param name="entityAluno">Aluno</param>
        /// <param name="Salvar_Sempre_Maiusculo">Flag que indica se deve ser salvo em maiúsculo os dados.</param>
        /// <param name="dtFichaMedicaContato">Tabela de contatos a salvar.</param>
        public static void SalvarFichaMedicaContatosAluno(TalkDBTransaction bancoGestao, ACA_Aluno entityAluno, bool Salvar_Sempre_Maiusculo, DataTable dtFichaMedicaContato)
        {
            ACA_FichaMedicaContato entityFichaMedicaContato = new ACA_FichaMedicaContato
            {
                alu_id = entityAluno.alu_id
            };

            int ordem = 1;

            // Salvar ficha médica contato.
            for (int j = 0; j < dtFichaMedicaContato.Rows.Count; j++)
            {
                if (dtFichaMedicaContato.Rows[j].RowState != DataRowState.Deleted)
                {
                    if (dtFichaMedicaContato.Rows[j].RowState == DataRowState.Added)
                    {
                        entityFichaMedicaContato.fmc_id = Convert.ToInt32(dtFichaMedicaContato.Rows[j]["fmc_id"].ToString());
                        entityFichaMedicaContato.fmc_nome = Salvar_Sempre_Maiusculo ? dtFichaMedicaContato.Rows[j]["fmc_nome"].ToString().ToUpper() : dtFichaMedicaContato.Rows[j]["fmc_nome"].ToString();
                        entityFichaMedicaContato.fmc_telefone = dtFichaMedicaContato.Rows[j]["fmc_telefone"].ToString();
                        if (!string.IsNullOrEmpty(dtFichaMedicaContato.Rows[j]["tra_id"].ToString()))
                            entityFichaMedicaContato.tra_id = Convert.ToInt32(dtFichaMedicaContato.Rows[j]["tra_id"].ToString());
                        entityFichaMedicaContato.fmc_ordem = ordem;

                        ordem = ordem + 1;

                        Save(entityFichaMedicaContato, bancoGestao);
                    }
                    else if (dtFichaMedicaContato.Rows[j].RowState == DataRowState.Modified)
                    {
                        entityFichaMedicaContato.fmc_id = Convert.ToInt32(dtFichaMedicaContato.Rows[j]["fmc_id"].ToString());
                        entityFichaMedicaContato.fmc_nome = Salvar_Sempre_Maiusculo ? dtFichaMedicaContato.Rows[j]["fmc_nome"].ToString().ToUpper() : dtFichaMedicaContato.Rows[j]["fmc_nome"].ToString();
                        entityFichaMedicaContato.fmc_telefone = dtFichaMedicaContato.Rows[j]["fmc_telefone"].ToString();
                        if (!string.IsNullOrEmpty(dtFichaMedicaContato.Rows[j]["tra_id"].ToString()))
                            entityFichaMedicaContato.tra_id = Convert.ToInt32(dtFichaMedicaContato.Rows[j]["tra_id"].ToString());
                        entityFichaMedicaContato.fmc_ordem = ordem;

                        ordem = ordem + 1;

                        Delete(entityFichaMedicaContato, bancoGestao);

                        Save(entityFichaMedicaContato, bancoGestao);
                    }
                }
                else
                {

                    entityFichaMedicaContato.fmc_id = Convert.ToInt32(dtFichaMedicaContato.Rows[j]["fmc_id", DataRowVersion.Original].ToString());
                    entityFichaMedicaContato.fmc_nome = dtFichaMedicaContato.Rows[j]["fmc_nome", DataRowVersion.Original].ToString();
                    entityFichaMedicaContato.fmc_telefone = dtFichaMedicaContato.Rows[j]["fmc_telefone", DataRowVersion.Original].ToString();
                    if (!string.IsNullOrEmpty(dtFichaMedicaContato.Rows[j]["tra_id", DataRowVersion.Original].ToString()))
                        entityFichaMedicaContato.tra_id = Convert.ToInt32(dtFichaMedicaContato.Rows[j]["tra_id", DataRowVersion.Original].ToString());

                    Delete(entityFichaMedicaContato, bancoGestao);
                }
            }
        }
	}
}