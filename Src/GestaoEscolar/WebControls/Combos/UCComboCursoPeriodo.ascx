<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboCursoPeriodo.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.UCComboCursoPeriodo" %>
<asp:Label ID="lblCursoPeriodo" runat="server" AssociatedControlID="ddlCursoPeriodo"
    Visible="false"></asp:Label>
<asp:DropDownList ID="ddlCursoPeriodo" runat="server" AppendDataBoundItems="True"
    Width="350px" Visible="false" DataTextField="Descricao" DataValueField="cur_crr_crp_id"
    OnSelectedIndexChanged="ddlCursoPeriodo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCursoPeriodo" runat="server" ControlToValidate="ddlCursoPeriodo"
    Visible="false" ValueToCompare="-1" Operator="NotEqual" Display="Dynamic">*</asp:CompareValidator>