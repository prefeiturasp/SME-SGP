<%@ Page Title="" Language="C#" MasterPageFile="~/Academico/HistoricoEscolar/MasterPageHistorico.master" AutoEventWireup="true" CodeBehind="Transferencia.aspx.cs" Inherits="GestaoEscolar.Academico.HistoricoEscolar.Transferencia" %>

<%@ Register Src="~/WebControls/HistoricoEscolar/UCTransferencia.ascx" TagPrefix="uc1" TagName="UCTransferencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentTab" runat="server">
    <div id="divTransferencia" class="ui-tabs-panel ui-widget-content ui-corner-bottom">
        <div runat="server" id="divUC">
            <uc1:UCTransferencia runat="server" ID="UCTransferencia" />
        </div>
    </div>
</asp:Content>
