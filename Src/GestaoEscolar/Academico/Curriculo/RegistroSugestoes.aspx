<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="RegistroSugestoes.aspx.cs" Inherits="GestaoEscolar.Academico.Curriculo.RegistroSugestoes" %>

<%@ Register Src="~/WebControls/Curriculo/UCCurriculo.ascx" TagName="UCCurriculo" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UCCurriculo ID="UCCurriculo1" runat="server" Titulo="<%$ Resources:Academico, Curriculo.RegistroSugestoes.UCCurriculo1.Titulo %>" />
</asp:Content>
