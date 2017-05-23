namespace GestaoEscolar.WebControls.RelatorioPedagogico
{
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;

    public partial class UCRelatorioPedagogico : MotherUserControl
    {
        #region Métodos

        public void Carregar(long alu_id, string urlRetorno)
        {
            Session["alu_id"] = alu_id;
            Session["URLRetorno"] = urlRetorno;
        }

        #endregion Métodos
    }
}