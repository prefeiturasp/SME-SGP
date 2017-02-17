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

namespace GestaoEscolar.Configuracao.Declaracoes
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Eventos Page Life Cycle

        /// <summary>
        /// Load da pagina
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Quando for edicao
            if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
            {
                UCAvisosTextosGerais1.VS_rda_id = PreviousPage.Edit_rda_id;
                UCAvisosTextosGerais1.VS_rlt_id = PreviousPage.Edit_rlt_id;
                UCAvisosTextosGerais1.VS_pda_id = PreviousPage.Edit_pda_id;
                UCAvisosTextosGerais1.TipoAvisotextoGerais = (int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.Declaracao;
            }
        }

        #endregion
    }
}