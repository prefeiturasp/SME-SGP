using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Web.UI;

namespace GestaoEscolar.Academico.Curriculo
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Page life cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            if (!IsPostBack)
            {
                UCCurriculo1.permiteIncluir = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                UCCurriculo1.permiteEditar = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                UCCurriculo1.permiteExcluir = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
            }
        }

        #endregion Page life cycle
    }
}