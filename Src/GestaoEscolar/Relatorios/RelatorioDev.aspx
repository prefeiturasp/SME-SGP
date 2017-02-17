<%@ Page MasterPageFile="~/MasterPage.master" Language="C#" AutoEventWireup="true" CodeBehind="RelatorioDev.aspx.cs" Inherits="Relatorios_RelatorioDev" %>

<%@ Register Src="../WebControls/Relatorio/UCDevReportView.ascx" TagName="UCDevReportView"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:UCDevReportView ID="UCDevReportView1" runat="server" />
</asp:Content>

