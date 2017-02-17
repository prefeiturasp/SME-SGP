<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="VisualizaCache.aspx.cs" Inherits="GestaoEscolar.Configuracao.Conteudo.VisualizaCache" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false"></asp:Label>
    <asp:Panel ID="pnlConteudo" runat="server" GroupingText="Visualização">
        <div>
            <asp:Button ID="btnVerCache" Text="Ver cache" runat="server" OnClick="btnVerCache_Click" />
            <asp:Button ID="btnLimpaCache" Text="Atualizar cache" runat="server" OnClick="btnLimpaCache_Click" />
            <asp:Button ID="btnListarCache" Text="Listar por tipo" runat="server" OnClick="btnListarCache_Click" />
        </div>
        <asp:Label ID="lblInformacao" runat="server" Text="" EnableViewState="false"></asp:Label>
        <asp:Repeater ID="rptTipos" runat="server" OnItemDataBound="rptTipos_ItemDataBound" OnItemCommand="rptTipos_ItemCommand" Visible="false">
            <HeaderTemplate>
                <table style="border-collapse: collapse; border-left: 1px; border-right: 1px;">
                    <thead style="background-color: #aaaaaa;">
                        <tr>
                            <th style="padding: 5px;">Tipo de valor da chave</th>
                            <th style="padding: 5px;">Quantidade</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="padding: 5px;">
                        <asp:LinkButton ID="btnChave" runat="server" CommandName="ListarChaves"><%# Eval("Key") %></asp:LinkButton>
                    </td>
                    <td style="padding: 5px;">
                        <%# Eval("Value") %>
                    </td>
                    <asp:Repeater ID="rptChaves" runat="server" Visible="false">
                        <HeaderTemplate>
                            <td>
                                <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li><%# Container.DataItem %></li>
                        </ItemTemplate>
                        <FooterTemplate>
                                </ul>
                            </td>
                        </FooterTemplate>
                    </asp:Repeater>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
</asp:Content>
