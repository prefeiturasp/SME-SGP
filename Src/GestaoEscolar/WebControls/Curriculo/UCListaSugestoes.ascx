<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCListaSugestoes.ascx.cs" Inherits="GestaoEscolar.WebControls.Curriculo.UCListaSugestoes" %>

<ul class="ul-sugestoes">
<asp:Repeater ID="rptSugestao" runat="server" OnItemDataBound="rptSugestao_ItemDataBound">
    <ItemTemplate>
        <li>
            <asp:Label ID="lblSugestao" runat="server" Text='<%# Bind("crs_sugestao") %>'></asp:Label>
            <small><asp:Label ID="lblDetalhes" runat="server"></asp:Label></small>
        </li>
    </ItemTemplate>
</asp:Repeater>
</ul>
