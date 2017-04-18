<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Classe.LancamentoSondagem.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Classe/LancamentoSondagem/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCComboCalendario" TagPrefix="uc4"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updLancamento" />
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsSondagem" runat="server">
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Classe, LancamentoSondagem.Cadastro.lblLegend.Text %>" /></legend>
        <asp:Label ID="lblDadosSondagem" runat="server" EnableViewState="False"></asp:Label>
        <br /><br />
        <asp:UpdatePanel runat="server" ID="updLancamento" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend><asp:Literal runat="server" ID="litFiltroTurma" Text="<%$ Resources:Classe, LancamentoSondagem.Cadastro.litFiltroTurma.Text %>"></asp:Literal></legend>
                    <uc2:UCComboUAEscola runat="server" ID="UCComboUAEscola" AsteriscoObg="true" ObrigatorioEscola="true" ObrigatorioUA="true"
                        CarregarEscolaAutomatico="true" MostraApenasAtivas="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" />
                    <uc4:UCComboCalendario ID="UCComboCalendario" runat="server" MostrarMensagemSelecione="true" Obrigatorio="true" PermiteEditar="false" />
                    <uc3:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" Obrigatorio="true" MostrarMensagemSelecione="true" PermiteEditar="false" />
                    <asp:Label ID="lblTurma" Text="<%$ Resources:Classe, LancamentoSondagem.Cadastro.lblTurma.Text %>" runat="server" AssociatedControlID="ddlTurma"></asp:Label>
                    <asp:DropDownList ID="ddlTurma" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="true" DataTextField="tur_esc_nome" DataValueField="tur_id"
                        SkinID="text60C" OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged">
                    </asp:DropDownList>
                </fieldset>
                <div class="right">
                    <asp:Button ID="bntSalvar" runat="server" Text="<%$ Resources:Classe, LancamentoSondagem.Cadastro.bntSalvar.Text %>" OnClick="bntSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Classe, LancamentoSondagem.Cadastro.btnCancelar.Text %>" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
