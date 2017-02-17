<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboOrdenacaoPosNome.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboOrdenacaoPosNome" %>
<div>
    <div>
        <asp:Label Style="display: inline" ID="_lblOrdenacao" runat="server" Text="Ordenar por:"
            AssociatedControlID="_ddlOrdenacao"></asp:Label>
    </div>
    <div>
        <asp:DropDownList ID="_ddlOrdenacao" runat="server" AutoPostBack="True" OnSelectedIndexChanged="_ddlOrdenacao_SelectedIndexChanged"
            SkinID="text20C">
            <asp:ListItem Selected="True" Value="0">Posição</asp:ListItem>
            <asp:ListItem Value="1">Nome do aluno</asp:ListItem>
        </asp:DropDownList>
    </div>
</div>
