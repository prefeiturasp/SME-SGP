using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.CabecalhoAvisosTextosGerais
{
    public partial class CadastroCabecalhoAvisosTextos : MotherPageLogado
    {
        private static int TipoAvisoTextoGerais;
        #region Page Life Cycle
        protected void Page_Load(object sender, EventArgs e)
        {         
            if (PreviousPage != null)
            {
                UCAvisosTextosGerais1.TipoAvisotextoGerais = Convert.ToInt32(PreviousPage.Edit_Cabecalho);
                TipoAvisoTextoGerais = Convert.ToInt32(PreviousPage.Edit_Cabecalho);
            }
            else
            {
                UCAvisosTextosGerais1.TipoAvisotextoGerais = TipoAvisoTextoGerais;
            }
        }
        #endregion
    }
}