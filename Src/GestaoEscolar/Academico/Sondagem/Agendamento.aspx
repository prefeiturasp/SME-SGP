<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Agendamento.aspx.cs" Inherits="GestaoEscolar.Academico.Sondagem.Agendamento" %>

<%@ PreviousPageType VirtualPath="~/Academico/Sondagem/Busca.aspx" %>

<%@ Register src="~/WebControls/Sondagem/UCAgendamento.ascx" tagname="UCAgendamento" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UCAgendamento ID="UCAgendamento1" runat="server" />
</asp:Content>
