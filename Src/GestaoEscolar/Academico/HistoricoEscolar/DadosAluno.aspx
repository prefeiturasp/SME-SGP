<%@ Page Title="" Language="C#" MasterPageFile="~/Academico/HistoricoEscolar/MasterPageHistorico.master" AutoEventWireup="true" CodeBehind="DadosAluno.aspx.cs" Inherits="GestaoEscolar.Academico.HistoricoEscolar.DadosAluno" %>

<%@ Register Src="~/WebControls/HistoricoEscolar/UCDadosAluno.ascx" TagPrefix="uc1" TagName="UCDadosAluno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentTab" runat="server">
    <div id="divDadosAlunos" class="ui-tabs-panel ui-widget-content ui-corner-bottom">
        <div runat="server" id="divUC">
            <uc1:UCDadosAluno runat="server" ID="UCDadosAluno" />
        </div>
    </div>
</asp:Content>