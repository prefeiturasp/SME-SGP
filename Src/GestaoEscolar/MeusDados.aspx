<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" Inherits="MeusDados" Codebehind="MeusDados.aspx.cs" %>

<%@ Register src="WebControls/MeusDados/UCMeusDados.ascx" tagname="UCMeusDados" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:UCMeusDados ID="UCMeusDados1" runat="server"/>
</asp:Content>
    