<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.Curriculo.Cadastro" %>

<%@ Register Src="~/WebControls/Curriculo/UCCurriculo.ascx" TagName="UCCurriculo" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UCCurriculo ID="UCCurriculo1" runat="server" Titulo="<%$ Resources:Academico, Curriculo.Cadastro.UCCurriculo1.Titulo %>" />
</asp:Content>
