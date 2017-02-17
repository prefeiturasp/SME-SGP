using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.BoletimCompletoAluno
{
    public partial class UCBoletimAngular : MotherUserControl
    {
        public void Carregar(int tpc_id, long alu_id, int mtu_id = 0, bool mostrarPeriodos = true)
        {
            Carregar(tpc_id, new long[] { alu_id }, new int[] { mtu_id }, mostrarPeriodos);
        }
        public void Carregar(int tpc_id, long[] alu_ids, int[] mtu_ids = null, bool mostrarPeriodos = true)
        {
            Session["tpc_id"] = tpc_id;
            Session["alu_ids"] = string.Join(",", alu_ids);
            Session["mtu_ids"] = mtu_ids == null || mtu_ids.Length == 0 ? string.Empty : string.Join(",", mtu_ids);
            Session["mostrarPeriodos"] = mostrarPeriodos;
        }
    }
}