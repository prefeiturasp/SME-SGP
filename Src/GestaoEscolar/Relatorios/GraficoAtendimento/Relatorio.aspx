<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Relatorio.aspx.cs" Inherits="GestaoEscolar.Relatorios.GraficoAtendimento.Relatorio" %>

<%@ PreviousPageType VirtualPath="~/Relatorios/GraficoAtendimento/Busca.aspx" %>

<%@ Register Src="~/WebControls/GraficoAtendimento/UCGraficoAtendimento.ascx" TagName="UCGraficoAtendimento" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:UCGraficoAtendimento ID="UCGraficoAtendimento" runat="server"></uc:UCGraficoAtendimento>
</asp:Content>
