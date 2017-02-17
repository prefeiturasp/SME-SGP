<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboAnoLetivo.ascx.cs" Inherits="WebControls_Combos_UCComboAnoLetivo" %>

<asp:Label ID="lblAnoLetivo" runat="server" AssociatedControlID="ddlCombo"
    Text="Ano letivo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" DataTextField="cal_ano" DataValueField="cal_ano"
    AppendDataBoundItems="True" Enabled="true" SkinID="text30C" AutoPostBack="true"
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Ano letivo é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false" CssClass="msgErroCombo"></asp:Label>
