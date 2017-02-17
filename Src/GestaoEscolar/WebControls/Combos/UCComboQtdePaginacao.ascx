<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboQtdePaginacao" Codebehind="UCComboQtdePaginacao.ascx.cs" %>
<div align="right">
    <asp:Label ID="lblPaginado" runat="server" Text="Itens por página" EnableViewState="false" CssClass="responsive-float-left"></asp:Label>
    <asp:DropDownList ID="ddlQtPaginado" runat="server" OnSelectedIndexChanged="ddlQtPaginado_SelectedIndexChanged">
        <asp:ListItem>10</asp:ListItem>
        <asp:ListItem>20</asp:ListItem>
        <asp:ListItem>50</asp:ListItem>
        <asp:ListItem>100</asp:ListItem>
    </asp:DropDownList>
</div>
