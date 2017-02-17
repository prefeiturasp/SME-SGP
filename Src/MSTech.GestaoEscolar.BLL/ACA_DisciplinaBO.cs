using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using MSTech.Data.Common;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Web;
using MSTech.CoreSSO.BLL;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_DisciplinaBO : BusinessBase<ACA_DisciplinaDAO, ACA_Disciplina>
    {
        /// <summary>
        /// Situações da disciplina
        /// </summary>
        public enum ACA_DisciplinaSituacao : byte
        {
            Ativo = 1
            ,
            Excluido = 3
            ,
            Inativo = 4
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_Disciplina entity)
        {
            return string.Format("ACA_Disciplina_GetEntity_{0}", entity.dis_id);
        }

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_Disciplina GetEntity(ACA_Disciplina entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    new ACA_DisciplinaDAO().Carregar(entity);
                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    GestaoEscolarUtilBO.CopiarEntity(cache, entity);
                }

                return entity;
            }

            new ACA_DisciplinaDAO().Carregar(entity);

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_Disciplina entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_GetEntity(entity));
            }
        }

        /// <summary>
        /// BD:GestaoEscolar / TB:ACA_Disciplina
        /// -Seleção de todos os registros relacionados com
        /// unidade, escola, docente e turno.
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        //***Metodo do Quadro de Preferencia
        public static DataTable GetSelectBy_Escola_Docente_Turno
        (
            int uni_id,
            int esc_id,
            int doc_id,
            int trn_id
        )
        {
            try
            {
                ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO();
                return dao.SelectBy_Escola_Docente_Turno
                (
                    uni_id,
                    esc_id,
                    doc_id,
                    trn_id
                );
            }
            catch
            {
                throw;
            }
        }//fim GetSelectBy_Escola_Docente_Turno

        /// <summary>
        /// Seleciona as disciplias com o dis_id na lista passada por parametro
        /// </summary>
        /// <param name="ids">lista de dis_ids</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_Disciplina> GetSelectBy_DisIds(string ids)
        {
            ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO();
            return dao.GetSelectBy_DisIds(ids);
        }

        ///<sumary>
        /// Retorna as disciplinas pelo tipo
        /// </sumary>
        /// <param name="tds_id"> Id da tabela ACA_TipoDisciplina</param>              
        /// <returns>Datatable com as disciplinas</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable RetornaDisciplinasPorTipo
        (
            int tds_id
        )
        {
            ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO();
            return dao.SelectBy_tds_id(tds_id);
        }

        ///<sumary>
        /// Verifica se o TipoDisciplina está na tabela Disciplina e CurriculoDisciplina.
        /// </sumary>
        /// <param name="tds_id">ID do tipo de disciplina</param>              
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable VerificaTipoDisciplina
        (
            int tds_id
        )
        {
            ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO();
            return dao.Verifica_TipoDisciplina(tds_id);
        }

        /// <summary>
        /// Retorna as disciplinas do tipo informado, cadastradas para o curso.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="dis_situacao">Situação da disciplina</param>
        /// <returns></returns>
        public static DataTable SelecionaPor_Tipo_Curso
        (
             int cur_id
            , int crr_id
            , int tds_id
            , ACA_DisciplinaSituacao dis_situacao
        )
        {
            ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO();
            return dao.SelectBy_Tipo_Curso(cur_id, crr_id, tds_id, (byte)dis_situacao);
        }

        /// <summary>
        /// Retorna as disciplinas do tipo informado, cadastradas para o curso e que tenha
        /// períodos compatíveis com a escola.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="dis_situacao">Situação da disciplina</param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <returns></returns>
        public static DataTable SelecionaPor_Tipo_CursoPeriodo
        (
             int cur_id
            , int crr_id
            , int tds_id
            , ACA_DisciplinaSituacao dis_situacao
            , int esc_id
            , int uni_id
        )
        {
            ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO();
            return dao.SelectBy_Tipo_CursoPeriodo(cur_id, crr_id, tds_id, (byte)dis_situacao, esc_id, uni_id);
        }

        /// <summary>
        /// Retorna as disciplinas do tipo informado, cadastradas para o curso e que tenha
        /// períodos compatíveis com a escola.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crd_tipo">Tipo de disciplina vindo do enum (ACA_CurriculoDisciplinaBO)</param>
        /// <param name="dis_situacao">Situação da disciplina</param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <returns></returns>
        public static DataTable SelecionaPor_TipoDisciplinaEnum_CursoPeriodo
        (
             int cur_id
            , int crr_id
            , int crp_id
            , byte crd_tipo
            , ACA_DisciplinaSituacao dis_situacao
            , int esc_id
            , int uni_id
        )
        {
            ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO();
            return dao.SelectBy_TipoDisciplinaEnum_CursoPeriodo(cur_id, crr_id, crp_id, crd_tipo, (byte)dis_situacao, esc_id, uni_id);
        }

        /// <summary>
        /// Retorna uma lista com as disciplinas eletiva do aluno
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>        
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        public static List<ACA_Disciplina> SelecionaListaDisciplinasEletivasAlunos
        (
            int cur_id
            , int crr_id
            , TalkDBTransaction banco
        )
        {
            List<ACA_Disciplina> list = new List<ACA_Disciplina>();

            ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO { _Banco = banco };
            DataTable dt = dao.SelectBy_EletivasAlunos(cur_id, crr_id);

            foreach (DataRow dr in dt.Rows)
            {
                ACA_Disciplina ent = new ACA_Disciplina();
                ent = dao.DataRowToEntity(dr, ent);

                list.Add(ent);
            }

            // Retorna a lista de disciplinas eletiva do aluno
            return list;
        }

        /// <summary>
        /// Retorna as disciplinas eletivas dos alunos matriculados na turma selecionada 
        /// (ulitizado na tela de lançamento de frequência mensal)
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_ids"></param>
        /// <param name="tpc_id"></param>
        /// <returns>DataTable de disciplinas eletivas</returns>
        public static DataTable SelecionaEletivasAlunosPorTurma(long tur_id, string alu_ids, int tpc_id)
        {
            ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO();
            return dao.SelectEletivasAlunosByTurma(tur_id, alu_ids, tpc_id);
        }

        /// <summary>
        /// Seleciona as disciplinas por tipo da disciplina na grade curricular.
        /// </summary>
        /// <param name="crd_tipo">Tipo de disciplina.</param>
        /// <returns></returns>
        public static List<ACA_Disciplina> SelecionaPorTipoGradeCurricular(byte crd_tipo)
        {
            return new ACA_DisciplinaDAO().SelecionaPorTipoGradeCurricular(crd_tipo);
        }

        /// <summary>
        /// Inclui uma nova disciplina
        /// </summary>
        /// <param name="entity">Entidade ACA_Disciplina</param> 
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        public new static bool Save(ACA_Disciplina entity, TalkDBTransaction banco)
        {
            LimpaCache(entity);

            if (entity.Validate())
            {
                ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Override do método Save.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="cur_id">cur_id</param>
        /// <param name="crr_id">crr_id</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static bool Save(ACA_Disciplina entity, int cur_id, int crr_id, TalkDBTransaction banco, Guid ent_id)
        {
            int tds_idEletiva = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_DISCIPLINA_ELETIVA_ALUNO, ent_id);

            if (VerificaExistentePorCodigoTipo(entity, tds_idEletiva, cur_id, crr_id, banco))
                throw new DuplicateNameException("Já existe um(a) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " eletiva cadastrada com este código.");

            return Save(entity, banco);
        }
        
        /// <summary>
        /// Deleta logicamente uma disciplina
        /// </summary>
        /// <param name="entity">Entidade ACA_Disciplina</param>        
        /// <param name="tabelasAdicionaisNaoVerificarIntegridade">Nome das tabelas adicionais (separadas por vírgula), 
        ///                                                        em que não será verificada a integridade dos dados.
        ///                                                        Tabelas padrão que sempre verifica: ACA_Disciplina 
        ///                                                        e ACA_CurriculoDisciplina</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Delete
        (
            ACA_Disciplina entity
            , string tabelasAdicionaisNaoVerificarIntegridade
            , TalkDBTransaction banco
        )
        {
            ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            LimpaCache(entity);

            string tabelasNaoVerificarIntegridade = "ACA_Disciplina,ACA_CurriculoDisciplina,ACA_DisciplinaMacroCampoEletivaAluno";
            if (!string.IsNullOrEmpty(tabelasAdicionaisNaoVerificarIntegridade))
                tabelasNaoVerificarIntegridade = tabelasNaoVerificarIntegridade + "," + tabelasAdicionaisNaoVerificarIntegridade;

            try
            {
                //Verifica se a disciplina pode ser deletada
                if (GestaoEscolarUtilBO.VerificarIntegridade("dis_id", entity.dis_id.ToString(), tabelasNaoVerificarIntegridade, dao._Banco))
                    throw new ValidationException("Não é possível excluir o(a) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " " + entity.dis_nome + ", pois possui outros registros ligados a ela.");

                //Deleta logicamente a disciplina
                return dao.Delete(entity);
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Verifica se já foi cadastrada algum disciplina com o mesmo codigo e tipo
        /// </summary>
        /// <param name="entity">Entidade ACA_Disciplina</param>
        /// <param name="tds_id">Id do tipo de disciplina</param>
        /// <param name="cur_id">Id co curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>True | False</returns>
        public static bool VerificaExistentePorCodigoTipo(ACA_Disciplina entity, int tds_id, int cur_id, int crr_id, TalkDBTransaction banco)
        {
            // Só verifica se já existe código da disciplina, se ele foi informado pelo usuário.
            // Agora o campo é obrigatório, porém no início era opcional.
            if (!string.IsNullOrEmpty(entity.dis_codigo))
            {
                ACA_DisciplinaDAO dao = new ACA_DisciplinaDAO { _Banco = banco };
                return dao.VerificaExistentePorCodigoTipo(entity.dis_id, entity.dis_codigo, tds_id, cur_id, crr_id);
            }

            return false;
        }

        /// <summary>
        /// Classe de excessão referente à entidade ACA_Disciplina.
        /// Utilizada nas telas de cadastro, para identificar se houve erro de validação
        /// na entidade da disciplina.
        /// </summary>
        public class ACA_Disciplina_ValidationException : ValidationException
        {
            public ACA_Disciplina_ValidationException(string message)
                : base(message)
            {
            }
        }
    }
}