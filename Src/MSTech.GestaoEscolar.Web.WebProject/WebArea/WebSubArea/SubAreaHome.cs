using System;
using System.Web;
using System.Web.UI;

namespace MSTech.GestaoEscolar.Web.WebProject.WebArea.WebSubArea
{
    public class SubAreaHome : MSTech.Web.WebProject.SubArea
    {
        public override string _Diretorio
        {
            get { return _DiretorioVirtual; }
        }

        public override string _DiretorioImagens
        {
            get
            {
                // Pega o tema da página que chamou.
                string temaPadrao = HttpContext.Current.Handler is Page ?
                                ((Page)HttpContext.Current.Handler).Theme ??
                                "Default" :
                                "Default";

                return "~/App_Themes/" + temaPadrao + "/images/";
            }
        }

        public override string _Nome
        {
            get { throw new NotImplementedException(); }
        }

        public override string _PaginaInicial
        {
            get { return _Diretorio + "Index.aspx"; }
        }
    }
}
