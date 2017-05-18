<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="RelatorioPedagogico.aspx.cs" Inherits="GestaoEscolar.Documentos.RelatorioPedagogico.RelatorioPedagogico" Theme="IntranetSMEBootStrap" %>

<%@ Register Src="~/WebControls/RelatorioPedagogico/UCRelatorioPedagogico.ascx" TagName="UCRelatorioPedagogico" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="area-form">
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
        <uc:UCRelatorioPedagogico ID="UCRelatorioPedagogico" runat="server" />
    </div>
    <div style="text-align: right; margin: 5px;" class="area-botoes-bottom">
        <asp:Button ID="btnVoltar" runat="server" Text="Voltar" />
    </div>
</asp:Content>
