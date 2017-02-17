<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboAvisoTextoGeral.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.UCComboAvisoTextoGeral" %>
<asp:Label ID="lblTitulo" runat="server" Text="Aviso e texto" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" DataTextField="atg_titulo" DataValueField="atg_id"
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged" SkinID="text60C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ControlToValidate="ddlCombo" Display="Dynamic"
    ErrorMessage="Aviso e texto é obrigatório." Operator="NotEqual" ValueToCompare="-1"
    Visible="false">*</asp:CompareValidator>