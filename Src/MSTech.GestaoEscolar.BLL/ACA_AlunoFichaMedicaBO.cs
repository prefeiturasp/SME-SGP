using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_AlunoFichaMedicaBO : BusinessBase<ACA_AlunoFichaMedicaDAO, ACA_AlunoFichaMedica> 
    {
        /// <summary>
        /// Verifica se já existe ficha médica para o aluno                
        /// </summary>
        /// <param name="alu_id">ID da tabela ACA_Aluno</param>                                      
        /// <returns>true ou false</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaFichaMedicaExistente
        (
            long alu_id            
        )
        {
            ACA_AlunoFichaMedicaDAO dao = new ACA_AlunoFichaMedicaDAO();
            return dao.SelectBy_alu_id(alu_id);
        }
    }
}
