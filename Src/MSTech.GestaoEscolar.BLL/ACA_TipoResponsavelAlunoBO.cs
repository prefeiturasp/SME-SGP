using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Classe que contém propriedades que retornam o id do tipo de responsável de aluno
    /// que estão cadastrados nos parâmetros.
    /// </summary>
    public static class TipoResponsavelAlunoParametro
    {
        /// <summary>
        /// Retorna o tra_id correspondente à mãe, cadastrado nos parâmetros acadêmicos.
        /// Caso o parâmetro não esteja configurado, retorna -1.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static Int32 tra_idMae(Guid ent_id = new Guid())
        {            
            int tra_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_RESPONSAVEL_MAE, ent_id);
            return tra_id > 0 ? tra_id : -1;         
        }

        /// <summary>
        /// Retorna o tra_id correspondente ao pai, cadastrado nos parâmetros acadêmicos.
        /// Caso o parâmetro não esteja configurado, retorna -1.
        /// </summary>
        public static Int32 tra_idPai(Guid ent_id = new Guid())
        {
            int tra_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_RESPONSAVEL_PAI, ent_id);
            return tra_id > 0 ? tra_id : -1;
        }

        /// <summary>
        /// Retorna o tra_id correspondente ao "O próprio", cadastrado nos parâmetros acadêmicos.
        /// Caso o parâmetro não esteja configurado, retorna -1.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static Int32 tra_idProprio(Guid ent_id = new Guid())
        {
            int tra_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_RESPONSAVEL_O_PROPRIO, ent_id);
            return tra_id > 0 ? tra_id : -1;
        }

        /// <summary>
        /// Retorna o tra_id correspondente ao responsável "Outro", cadastrado nos parâmetros acadêmicos.
        /// Caso o parâmetro não esteja configurado, retorna -1.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static Int32 tra_idOutro(Guid ent_id)
        {            
            int tra_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_RESPONSAVEL_OUTRO, ent_id);
            return tra_id > 0 ? tra_id : -1;
        }

        /// <summary>
        /// Retorna o tra_id correspondente ao responsável "Não existe", cadastrado nos parâmetros acadêmicos.
        /// Caso o parâmetro não esteja configurado, retorna -1.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static Int32 tra_idNaoExiste(Guid ent_id)
        {
            int tra_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_RESPONSAVEL_NAO_EXISTE, ent_id);
            return tra_id > 0 ? tra_id : -1;
        }
    }

    public enum eTipoResponsavelAlunoPadrao : byte
    {
        Mae = 1, 
        Pai = 2, 
        Familiar = 3, 
        Tutor = 4, 
        Instituicao = 5, 
        Proprio = 6, 
        Outro = 7,
        NaoExiste = 8
    }

    public class ACA_TipoResponsavelAlunoBO : BusinessBase<ACA_TipoResponsavelAlunoDAO, ACA_TipoResponsavelAluno>
    {
        /// <summary>
        /// Consulta se já existe um tipo de responsável do aluno cadastrado com o nome passado 
        /// e preenche a entidade com nome e id.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoResponsavelAluno</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelecionaTipoResponsavelPorNome
        (
            ACA_TipoResponsavelAluno entity
        )
        {
            ACA_TipoResponsavelAlunoDAO dao = new ACA_TipoResponsavelAlunoDAO();
            return dao.SelectBy_NomeTipo(entity);
        }
        
        /// <summary>
        /// Retorna todos os tipos de responsável do aluno não excluídos logicamente
        /// Sem paginação
        /// </summary>                                
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoResponsavelAluno()
        {
            ACA_TipoResponsavelAlunoDAO dao = new ACA_TipoResponsavelAlunoDAO();
            return dao.SelectBy_Pesquisa(false, 1, 1, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de responsável do aluno não excluídos logicamente
        /// Exceto o tipo "Não existe", caso trazerNaoExiste verdadeiro.
        /// </summary>    
        /// <param name="trazerNaoExiste">Traz ou não o tipo "Não existe"</param>
        public static DataTable SelecionaTipoResponsavelAluno(bool trazerNaoExiste)
        {
            return new ACA_TipoResponsavelAlunoDAO().SelecionaTipoResponsavelAluno(trazerNaoExiste, true);
        }

        /// <summary>
        /// Retorna todos os tipos de responsável do aluno não excluídos logicamente
        /// Exceto o tipo "Não existe", caso trazerNaoExiste verdadeiro.
        /// Exceto o tipo "O Próprio", caso trazerOProprio verdadeiro.
        /// </summary>    
        /// <param name="trazerNaoExiste">Traz ou não o tipo "Não existe"</param>
        /// <param name="trazerOProprio">Traz ou não o tipo "O Próprio".</param>
        public static DataTable SelecionaTipoResponsavelAluno(bool trazerNaoExiste, bool trazerOProprio)
        {
            return new ACA_TipoResponsavelAlunoDAO().SelecionaTipoResponsavelAluno(trazerNaoExiste, trazerOProprio);
        }
    }
}
