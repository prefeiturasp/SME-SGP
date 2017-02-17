<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboEstadoCivil" Codebehind="UCComboEstadoCivil.ascx.cs" %>
<asp:Label ID="LabelEstadoCivil" runat="server" Text="Estado civil" AssociatedControlID="_ddlEstadoCivil"></asp:Label>
<asp:DropDownList ID="_ddlEstadoCivil" runat="server" AppendDataBoundItems="True" SkinID="text30C">
    <asp:ListItem Value="-1">-- Selecione um estado civil --</asp:ListItem>
    <asp:ListItem Value="1">Solteiro (a)</asp:ListItem>
    <asp:ListItem Value="2">Casado (a)</asp:ListItem>
    <asp:ListItem Value="3">Separado (a)</asp:ListItem>
    <asp:ListItem Value="4">Divorciado (a)</asp:ListItem>
    <asp:ListItem Value="5">Viúvo (a)</asp:ListItem>
</asp:DropDownList>
