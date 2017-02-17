<%@ Page Title="" Language="C#" MasterPageFile="~/Academico/HistoricoEscolar/MasterPageHistorico.master" AutoEventWireup="true" CodeBehind="InformacoesComplementares.aspx.cs" Inherits="GestaoEscolar.Academico.HistoricoEscolar.InformacoesComplementares" %>

<%@ Register Src="~/WebControls/HistoricoEscolar/UCInformacoesComplementares.ascx" TagPrefix="uc1" TagName="UCInformacoesComplementares" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentTab" runat="server">
    <div id="divInformacoesComplementares" class="ui-tabs-panel ui-widget-content ui-corner-bottom">
        <div runat="server" id="divUC">
            <uc1:UCInformacoesComplementares runat="server" ID="UCInformacoesComplementares" />
        </div>
    </div>
</asp:Content>
