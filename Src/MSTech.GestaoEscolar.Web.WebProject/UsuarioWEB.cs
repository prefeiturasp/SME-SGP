using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;

namespace MSTech.GestaoEscolar.Web.WebProject
{
    using MSTech.GestaoEscolar.Entities;

    [Serializable]
    public class UsuarioWEB : MSTech.Web.WebProject.UsuarioWEB
    {
        #region ATRIBUTOS

        // Grupo logado
        private SYS_Grupo grupo;

        // Usuário logado
        private SYS_Usuario usuario;

        // UA ou entidades do usuário logado
        private IList<SYS_UsuarioGrupoUA> uasGrupo;

        // Permissao do módulo do usuário
        private SYS_GrupoPermissao grupoPermissao;

        // Entidade docente, de acordo com o usuário logado.
        private ACA_Docente docente;

        #endregion ATRIBUTOS

        #region PROPRIEDADES

        public Guid Uad_idSuperiorPermissao { get; set; }

        public int Esc_idPermissao { get; set; }

        public long alu_id { get; set; }

        public int esc_id { get; set; }

        public int uni_id { get; set; }

        public int mtu_id { get; set; }

        public int tpc_id { get; set; }

        public bool responsavel { get; set; }

        public Guid pes_idAluno { get; set; }

        public string language { get; set; }
        
        /// <summary>
        /// Retorna os dados do usuário logado
        /// </summary>
        public SYS_Usuario Usuario
        {
            get
            {
                return usuario;
            }
            set
            {
                usuario = value;
            }
        }

        /// <summary>
        /// Retorna o grupo na qual o usuário pertense
        /// naquele sistema.
        /// </summary>
        public SYS_Grupo Grupo
        {
            get
            {
                return grupo;
            }
            set
            {
                if (usuario != null)
                {
                    grupo = value;
                    uasGrupo = SYS_UsuarioBO.GetSelectByUsuarioGrupoUA(usuario.usu_id, grupo.gru_id);

                    // Se está setando o grupo Individual, verificar se o usuário é docente cadastrado no sistema.
                    if (grupo.vis_id == SysVisaoID.Individual)
                    {
                        // Carrega a entidade docente de acordo com a pessoa do usuário logado.
                        ACA_Docente entityDocente;
                        ACA_DocenteBO.GetSelectBy_Pessoa(usuario.ent_id,usuario.pes_id, out entityDocente);
                        Docente = entityDocente;
                    }
                    else
                    {
                        Docente = null;
                    }
                }
                else
                    throw new ArgumentNullException("objeto __SessionWEB.__UsuarioWEB.Usuario nulo");
            }
        }

        /// <summary>
        /// Retorna as unidades administrativa e entidades de um grupo do usuário.
        /// </summary>
        public IList<SYS_UsuarioGrupoUA> GrupoUA
        {
            get { return this.uasGrupo; }
        }

        /// <summary>
        /// Recebe a permissão do módulo do usuário logado.
        /// </summary>
        public SYS_GrupoPermissao GrupoPermissao
        {
            get
            {
                return this.grupoPermissao;
            }
            set
            {
                if (grupo != null)
                    this.grupoPermissao = value;
                else
                    throw new ArgumentNullException("objeto __SessionWEB.__UsuarioWEB.Grupo nulo");
            }
        }

        /// <summary>
        /// Retorna a entidade do docente de acordo com o usuário logado (usuário com visão Individual).
        /// </summary>
        public ACA_Docente Docente
        {
            get
            {
                if (docente == null)
                {
                    return new ACA_Docente();
                }

                return this.docente;
            }

            set
            {
                if (this.usuario != null)
                {
                    this.docente = value;
                }
                else
                {
                    throw new ArgumentNullException("objeto __SessionWEB.__UsuarioWEB.Usuario nulo");
                }
            }
        }

        #endregion PROPRIEDADES
    }
}