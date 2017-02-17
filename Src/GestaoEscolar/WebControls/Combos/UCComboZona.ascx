<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboZona" Codebehind="UCComboZona.ascx.cs" %>
<asp:Label ID="LabelZona" runat="server" Text="Zona" AssociatedControlID="_ddlZona"></asp:Label>
<asp:DropDownList ID="_ddlZona" runat="server" 
    AppendDataBoundItems="True" SkinID="ComboZonaEndereco">
    <asp:ListItem Value="-1">-- Selecione uma zona --</asp:ListItem>
    <asp:ListItem Value="1">Urbana</asp:ListItem>
    <asp:ListItem Value="2">Rural</asp:ListItem>
</asp:DropDownList>
