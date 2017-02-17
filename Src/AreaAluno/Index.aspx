<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePaginaInicial.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AreaAluno.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upnBusca" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="bem-vindo">
                <fieldset id="Fieldset2" runat="server" visible="false">
                    <asp:Label runat="server" ID="lblBoletimNaoDisponivel"></asp:Label>
                </fieldset>
                <fieldset id="Fieldset1" runat="server">
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                    <div id="divInformacao" runat="server" class="caixaBemVindo centraliza">
                        <asp:Label ID="lblBemVindo" runat="server">
                            <strong>Bem-vindo(a),</strong> 
                        </asp:Label>
                        <br /><br />
                        <div runat="server" id="divResponsavel">
                            <span style="font-weight:bold">Responsável por</span>
                            <br /><br />
                        </div>
                        <asp:Image ID="imgFotoAluno" runat="server" SkinID="imgFotoAluno" ToolTip="Foto Aluno" />
                        <br />
                        <asp:Label ID="lblInformacaoAluno" runat="server" Visible="true"></asp:Label><br />
                    </div>
                    <div class="clear"></div>
                    <asp:Literal id="_lblSiteMap" runat="server"></asp:Literal>
                    <div id="SiteMap2">
                        <ul runat="server" id="ulItemAcessoExterno" class="listaMenu" visible="false">
                            <li class="txtMenu">
                                <h2 id="h2TituloAcessoExterno" runat="server"></h2>
                            </li>
                            <li class="txtSubMenu">
                                <a class="link externo" id="lnkAcessoExterno" runat="server">
                                    <asp:Label ID="lblAcessoExterno" runat="server"></asp:Label>
                                    <span class="linkHover"></span>
                                </a>
                            </li>
                        </ul>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnVoltar" runat="server" CausesValidation="False" Visible="false" Text="Voltar" />
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
