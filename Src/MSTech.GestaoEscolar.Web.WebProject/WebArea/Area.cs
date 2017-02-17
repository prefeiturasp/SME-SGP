using System;
using System.Web;
using System.Web.UI;

namespace MSTech.GestaoEscolar.Web.WebProject.WebArea
{
    [Serializable]
    public class Area : MSTech.Web.WebProject.Area
    {
        public override MSTech.Web.WebProject.SubArea _SubArea
        {
            get
            {
                if (base._SubArea == null)
                    return base._SubArea = new WebSubArea.SubAreaHome();
                return base._SubArea;
            }
            set
            {
                base._SubArea = value;
            }
        }
        
        public override string _Diretorio
        {
            get { return _DiretorioVirtual; }
        }

        /// <summary>
        /// Retorna o diretório da pasta de imagens do tema atual.
        /// </summary>
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

        public override string _DiretorioIncludes
        {
            get { return _DiretorioVirtual + "Includes/"; }
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
