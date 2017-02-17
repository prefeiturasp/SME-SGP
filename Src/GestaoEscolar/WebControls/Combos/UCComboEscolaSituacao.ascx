<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboEscolaSituacao" Codebehind="UCComboEscolaSituacao.ascx.cs" %>
<asp:Label ID="LabelSituacao" runat="server" Text="Situação" AssociatedControlID="_ddlEscolaSituacao"></asp:Label>
<asp:DropDownList ID="_ddlEscolaSituacao" runat="server" AppendDataBoundItems="True"
    SkinID="text30C">
    <asp:ListItem Value="-1">-- Selecione uma situação --</asp:ListItem>
    <asp:ListItem Value="1">Ativo</asp:ListItem>
    <asp:ListItem Value="4">Desativado</asp:ListItem>
    <asp:ListItem Value="5">Em ativação</asp:ListItem>
</asp:DropDownList>
