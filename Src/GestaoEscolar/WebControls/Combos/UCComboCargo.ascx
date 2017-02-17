<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboCargo"
    CodeBehind="UCComboCargo.ascx.cs" %>

<asp:Label ID="lblTitulo" runat="server" Text="Cargo" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    DataTextField="crg_nome" DataValueField="crg_id" SkinID="text60C"  OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Cargo é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
