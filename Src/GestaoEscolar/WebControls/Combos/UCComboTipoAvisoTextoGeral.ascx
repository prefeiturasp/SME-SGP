<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoAvisoTextoGeral.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboTipoAvisoTextoGeral" %>

<asp:Label ID="lblTipoAviso" runat="server" AssociatedControlID="ddlAviso" Text="Tipo aviso"></asp:Label>
<asp:DropDownList ID="ddlAviso" runat="server" SkinID="text30C" OnSelectedIndexChanged="ddlAviso_SelectedIndexChanged" AutoPostBack="true">
    <asp:ListItem Value="-1" Text="-- Selecione um tipo de aviso --"></asp:ListItem>
    <asp:ListItem Value="1" Text="Por Aluno"></asp:ListItem>
    <asp:ListItem Value="3" Text="Informativo"></asp:ListItem>
</asp:DropDownList>
<asp:CompareValidator ID="cpvTipo" runat="server" ControlToValidate="ddlAviso"
    Display="Dynamic" ErrorMessage="Tipo de aviso é obrigatório" Operator="NotEqual" ValueToCompare="-1">*</asp:CompareValidator>
<asp:Label ID="lblMessageTipo" runat="server" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>