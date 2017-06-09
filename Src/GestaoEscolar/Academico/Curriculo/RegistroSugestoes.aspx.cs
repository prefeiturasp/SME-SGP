using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Web.UI;

namespace GestaoEscolar.Academico.Curriculo
{
    public partial class RegistroSugestoes : MotherPageLogado
    {
        #region Page life cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UCCurriculo1.VS_permiteIncluir = false;
                UCCurriculo1.VS_permiteEditar = false;
                UCCurriculo1.VS_permiteExcluir = false;
                UCCurriculo1.VS_permiteConsultarSugestao = false;
                UCCurriculo1.VS_permiteIncluirSugestao = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        #endregion Page life cycle
    }
}