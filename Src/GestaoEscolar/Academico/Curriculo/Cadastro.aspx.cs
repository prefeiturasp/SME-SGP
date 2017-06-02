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
                UCCurriculo1.VS_permiteIncluir = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                UCCurriculo1.VS_permiteEditar = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                UCCurriculo1.VS_permiteExcluir = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                UCCurriculo1.VS_permiteConsultarSugestao = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                UCCurriculo1.VS_permiteIncluirSugestao = false;
            }
        }

        #endregion Page life cycle
    }
}