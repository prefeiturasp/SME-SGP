<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCDevReportView.ascx.cs" Inherits="WebControls_Relatorio_UCDevReportView" %>
<%@ Register Assembly="DevExpress.XtraReports.v16.1.Web, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Label ID="_lblMessageLayout" runat="server" EnableViewState="False"></asp:Label>
<fieldset style="height: 650px;" class="rel-fieldset">
    <legend>
        <asp:Label ID="_lblTitulo" runat="server" EnableViewState="False"></asp:Label></legend>
    <asp:Label ID="_lblMensagem" runat="server" EnableViewState="False"></asp:Label>

    <div align="right" class="area-botoes-bottom">
        <asp:Button ID="_btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" EnableViewState="False" />
    </div>
    <div align="center" style="height: 100%;">
        <dx:ReportToolbar ID="DevReportTools" runat="server" ReportViewerID="DevReportView" ShowDefaultButtons="False"
            Style="width: 902px; min-width: 519px; float: none; height: 30px;" CssClass="rel-responsive">
            <Items>
                <dx:ReportToolbarButton ItemKind="Search" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="PrintReport" />
                <dx:ReportToolbarButton ItemKind="PrintPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                <dx:ReportToolbarLabel ItemKind="PageLabel" />
                <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                </dx:ReportToolbarComboBox>
                <dx:ReportToolbarLabel ItemKind="OfLabel" />
                <dx:ReportToolbarTextBox ItemKind="PageCount" />
                <dx:ReportToolbarButton ItemKind="NextPage" />
                <dx:ReportToolbarButton ItemKind="LastPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                    <Elements>
                        <dx:ListElement Value="pdf" />
                        <dx:ListElement Value="csv" />
                        <dx:ListElement Value="html" />
                        <dx:ListElement Value="xls" />
                        <dx:ListElement Value="png" />
                        <dx:ListElement Value="rtf" />
                    </Elements>
                </dx:ReportToolbarComboBox>
            </Items>
            <Styles>
                <LabelStyle>
                    <Margins MarginLeft="3px" MarginRight="3px" />
                </LabelStyle>
            </Styles>
        </dx:ReportToolbar>
        <div class="rel-responsive overflowx-auto">
            <dx:ReportViewer ID="DevReportView" runat="server" PageByPage="true" ShowLoadingPanelImage="true" Height="550px" Width="900px" OnUnload="DevReportView_Unload" AutoSize="false"/>
        </div>
    </div>
</fieldset>
