using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.OrientacaoCurricular
{
    public partial class UCNivelOrientacao : MotherUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void CarregarOrientacaoCurricular(List<ORC_OrientacaoCurricularBO.OrientacaoCurricular> orientacao)
        {
            grvOrientacaoCurricular.DataSource = orientacao;
            grvOrientacaoCurricular.DataBind();
        }

        protected void grvOrientacaoCurricular_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<ORC_OrientacaoCurricularBO.OrientacaoCurricular> ltOrientacaoCurricularFilho = (List<ORC_OrientacaoCurricularBO.OrientacaoCurricular>)DataBinder.Eval(e.Row.DataItem, "ltOrientacaoCurricularFilho");
                UCNivelOrientacao ucNivelOrientacao = (UCNivelOrientacao)e.Row.FindControl("UCNivelOrientacaoFilho");
                ucNivelOrientacao.CarregarOrientacaoCurricular(ltOrientacaoCurricularFilho);
            }
        }
    }
}