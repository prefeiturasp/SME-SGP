<%@ Page Title="" Language="C#" MasterPageFile="~/Academico/HistoricoEscolar/MasterPageHistorico.master" AutoEventWireup="true" CodeBehind="EnsinoFundamental.aspx.cs" Inherits="GestaoEscolar.Academico.HistoricoEscolar.EnsinoFundamental" %>

<%@ Register Src="~/WebControls/HistoricoEscolar/UCEnsinoFundamental.ascx" TagPrefix="uc1" TagName="UCEnsinoFundamental" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentTab" runat="server">
    <div id="divEnsinoFundamental" class="ui-tabs-panel ui-widget-content ui-corner-bottom">
        <div runat="server" id="divUC">
            <uc1:UCEnsinoFundamental runat="server" ID="UCEnsinoFundamental" />
        </div>
    </div>
</asp:Content>
