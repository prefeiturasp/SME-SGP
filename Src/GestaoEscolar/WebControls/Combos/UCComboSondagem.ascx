<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboSondagem.ascx.cs" Inherits="WebControls_Combos_UCComboSondagem" %>

<asp:Label ID="lblSondagem" runat="server" AssociatedControlID="ddlCombo"
    Text="Sondagem"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" DataTextField="snd_titulo" DataValueField="snd_id"
    AppendDataBoundItems="True" Enabled="true" SkinID="text60C" AutoPostBack="true"
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Sondagem é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false" CssClass="msgErroCombo"></asp:Label>