<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.Declaracoes.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/Declaracoes/Busca.aspx" %>

<%@ Register Src="~/WebControls/AvisosTextosGerais/UCAvisosTextosGerais.ascx" TagName="UCAvisosTextosGerais"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UCAvisosTextosGerais ID="UCAvisosTextosGerais1" runat="server" />
</asp:Content>
