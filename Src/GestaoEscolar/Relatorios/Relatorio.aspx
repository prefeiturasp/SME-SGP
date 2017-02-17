<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Relatorios_Relatorio" CodeBehind="Relatorio.aspx.cs" %>

<%@ Register Src="../WebControls/Relatorio/UCReportView.ascx" TagName="UCReportView"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessageLayout" runat="server" EnableViewState="False"></asp:Label>
    <fieldset style="height: 650px;" class="rel-fieldset">
        <legend>
            <asp:Label ID="_lblTitulo" runat="server" EnableViewState="False"></asp:Label>
        </legend>
        <div align="right" class="area-botoes-bottom">
            <asp:Button ID="_btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" EnableViewState="False" />
        </div>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>

        <asp:PlaceHolder runat="server" ID="Placeholder1"></asp:PlaceHolder>

    </fieldset>
</asp:Content>
