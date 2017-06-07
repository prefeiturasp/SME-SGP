<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCListaSugestoes.ascx.cs" Inherits="GestaoEscolar.WebControls.Curriculo.UCListaSugestoes" %>

<ul>
<asp:Repeater ID="rptSugestao" runat="server" OnItemDataBound="rptSugestao_ItemDataBound">
    <ItemTemplate>
        <li>
            <asp:Label ID="lblSugestao" runat="server" Text='<%# Bind("crs_sugestao") %>'></asp:Label>
            <br /><asp:Label ID="lblDetalhes" runat="server"></asp:Label>
        </li>
    </ItemTemplate>
</asp:Repeater>
</ul>
