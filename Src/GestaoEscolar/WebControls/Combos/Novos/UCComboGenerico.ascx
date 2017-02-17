<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboGenerico.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCComboGenerico" %>

<asp:Label ID="lblTitulo" runat="server" Text="" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    SkinID="text60C" 
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage='<%#RetornaTituloCombo() + "é obrigatório." %>'
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>