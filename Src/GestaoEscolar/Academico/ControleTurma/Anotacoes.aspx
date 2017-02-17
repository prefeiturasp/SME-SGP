<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Anotacoes.aspx.cs" Inherits="GestaoEscolar.Academico.ControleTurma.Anotacoes" %>

<%@ Register src="~/WebControls/AlunoAnotacoes/UCAlunoAnotacoes.ascx" tagname="UCAlunoAnotacoes" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:UCAlunoAnotacoes ID="UCAlunoAnotacoes1" runat="server" />
</asp:Content>