using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace GestaoAcademica.WebApi.Controllers
{

    public class usuariosController : ApiController
    {
        /// <summary>
        /// Seleciona os dados do usuário por id.
        /// -- Utilização: URL_API/usuario/00000000-0000-0000-0000-000000000000
        /// -- Parâmetros: id do usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public SYS_UsuarioDTO Get(Guid id)
        {
            try
            {
                List<SYS_UsuarioDTO> usuarioAlunoDocente = ApiBO.SelecionaUsuario(id, 0, Guid.Empty, new DateTime());
                if (usuarioAlunoDocente != null && usuarioAlunoDocente.Count > 0) return usuarioAlunoDocente.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Seleciona os dados do usuário por escola.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<SYS_UsuarioDTO> GetPorEscola(Int64 esc_id)
        {
            return GetAllEntidade(esc_id, Guid.Empty, new DateTime());
        }

        /// <summary>
        /// Seleciona os dados do usuário por entidade.
        /// </summary>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<SYS_UsuarioDTO> GetPorEntidade(Guid ent_id)
        {
            return GetAllEntidade(0, ent_id, new DateTime());
        }
        
        /// <summary>
        /// Seleciona os dados do usuário por entidade e data base.
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpGet]
        public List<SYS_UsuarioDTO> GetPorEntidade(Guid ent_id, DateTime dataBase)
        {
            return GetAllEntidade(0, ent_id, dataBase);
        }
        
        /// <summary>
        /// Seleciona os dados do usuário por escola e data base.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpGet]
        public List<SYS_UsuarioDTO> GetAll(Int64 esc_id, DateTime dataBase)
        {
            return GetAllEntidade(esc_id, Guid.Empty, dataBase);
        }

        /// <summary>
        /// Seleciona os dados do usuário por escola, entidade e data base.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="ent_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpGet]
        public List<SYS_UsuarioDTO> GetAllEntidade(Int64 esc_id, Guid ent_id, DateTime dataBase)
        {
            try
            {
                List<SYS_UsuarioDTO> usuarioAlunoDocente = ApiBO.SelecionaUsuario(Guid.Empty, esc_id, ent_id, dataBase);
                return usuarioAlunoDocente;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Seleciona os dados do usuário por login e entidade.
        /// -- Utilização: URL_API/usuario/?usu_login=login&ent_id=00000000-0000-0000-0000-000000000000
        /// -- Parâmetros: usu_login: login do usuario
        /// --             ent_id: id da entidade do usuario
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpGet]
        public SYS_UsuarioDTO SelecionaAlunoDocentePorUsuario([FromUri] UsuarioEntradaDTO filtro)
        {
            try
            {
                List<SYS_UsuarioDTO> usuarioAlunoDocente = ApiBO.SelecionaUsuario(filtro.usu_login, filtro.ent_id);
                if (usuarioAlunoDocente != null && usuarioAlunoDocente.Count > 0) return usuarioAlunoDocente.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Seleciona os IDs dos usuários docentes por escola.
        /// </summary>
        /// <param name="uad_idEscola"></param>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Guid> GetDocentesPorEscola(Guid uad_idEscola, Guid ent_id)
        {
            try
            {
                return ApiBO.SelecionaUsuarioDocentePorEscola(uad_idEscola, ent_id);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Seleciona os IDs dos usuários docentes por diretoria.
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Guid> GetDocentesPorDiretoria(Guid uad_idSuperior, Guid ent_id)
        {
            try
            {
                return ApiBO.SelecionaUsuarioDocentePorDiretoria(uad_idSuperior, ent_id);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}