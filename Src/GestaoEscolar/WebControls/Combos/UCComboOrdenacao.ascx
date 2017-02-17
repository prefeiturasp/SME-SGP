<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboOrdenacao" Codebehind="UCComboOrdenacao.ascx.cs" %>
<div style="float: left; margin-bottom: 4px !important;">
    <asp:Label Style="display: inline" ID="_lblOrdenacao" runat="server" Text="Ordenar por:"
        AssociatedControlID="_ddlOrdenacao"></asp:Label>
    <asp:DropDownList ID="_ddlOrdenacao" runat="server" OnSelectedIndexChanged="_ddlOrdenacao_SelectedIndexChanged"
        SkinID="text20C">
        <asp:ListItem Selected="True" Value="0">Número de chamada</asp:ListItem>
        <asp:ListItem Value="1">Nome do aluno</asp:ListItem>
    </asp:DropDownList>
</div>
<div class="clear">
</div>
