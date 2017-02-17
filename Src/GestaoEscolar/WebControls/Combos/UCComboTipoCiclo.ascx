<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoCiclo.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboTipoCiclo" %>

<asp:Label ID="lblTipoCiclo" Text="Tipo de ciclo" runat="server" AssociatedControlID="ddlTipoCiclo"></asp:Label>
<asp:DropDownList ID="ddlTipoCiclo" runat="server" AppendDataBoundItems="True"
    SkinID="text60C" DataTextField="tci_nome" DataValueField="tci_id"
    OnSelectedIndexChanged="ddlTipoCiclo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvTipoCiclo" runat="server" ControlToValidate="ddlTipoCiclo"
    Visible="false" ValueToCompare="-1" Operator="NotEqual" Display="Dynamic">*</asp:CompareValidator>