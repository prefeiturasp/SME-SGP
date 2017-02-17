<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.AtaSinteseFinal.Busca" %>

<%@ Register Src="~/WebControls/BuscaDocente/UCBuscaDocenteTurma.ascx" TagPrefix="uc1" TagName="UCBuscaDocenteTurma" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc2" TagName="UCCamposObrigatorios" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagPrefix="uc3" TagName="UCCPeriodoCalendario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="uppPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsDocumentosEscola" runat="server" style="margin-left: 10px;">
                <legend id="lgdTitulo" runat="server"></legend>
                <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                <div id="divPesquisa" class="divPesquisa" runat="server">
                    <uc1:UCBuscaDocenteTurma runat="server" ID="UCBuscaDocenteTurma" Visible="false" />
                    <div runat="server" id="divPeriodoCalendario" visible="false">
                        <uc3:UCCPeriodoCalendario runat="server" id="UCCPeriodoCalendario" mostrarmensagemselecione="true" obrigatorio="false" permiteeditar="false" />
                    </div>
                </div>
                <div class="right">
                    <asp:Button ID="btnGerar" runat="server" Text="Gerar relatório" OnClick="btnGerar_Click" CausesValidation="true" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
