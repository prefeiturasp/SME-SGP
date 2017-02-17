<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAluno.Master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="AreaAluno.Consulta.Documentos.Busca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divTabs-Documentos">
        <asp:UpdatePanel ID="updDocumentos" runat="server">
            <ContentTemplate>
                <fieldset>
                    <asp:Label ID="lblSemAreas" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.lblSemAreas.Text %>" Visible="false"></asp:Label>
                    <asp:Repeater ID="rptAreas" runat="server" OnItemDataBound="rptAreas_ItemDataBound">
                        <ItemTemplate>
                            <fieldset>
                                <legend><asp:Label ID="lblArea" runat="server" Text='<%# Eval("tad_nome") %>'></asp:Label></legend>
                                <asp:Label ID="lblSemDocumentos" runat="server" Visible="false"></asp:Label>
                                <table>
                                    <asp:Repeater ID="rptDocumentos" runat="server" OnItemDataBound="rptDocumentos_ItemDataBound" Visible="false">
                                        <ItemTemplate>
                                            <tr><td>
                                                <asp:Label ID="lblEspaco" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- "></asp:Label> 
                                                <asp:HyperLink ID="hplDocumento" runat="server" Text='<%# Eval("aar_descricao") %>'></asp:HyperLink>
                                            </td></tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </fieldset>
                        </ItemTemplate>
                    </asp:Repeater>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>


