<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.FrequenciaMensal.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc2" TagName="UCCamposObrigatorios" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagPrefix="uc1" TagName="UCComboUAEscola" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagPrefix="uc1" TagName="UCCCalendario" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagPrefix="uc1" TagName="UCCCursoCurriculo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagPrefix="uc1" TagName="UCComboTurma" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagPrefix="uc1" TagName="UCComboCurriculoPeriodo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurmaDisciplina.ascx" TagPrefix="uc1" TagName="UCComboTurmaDisciplina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="updMessage" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Relatorio" />
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updFiltros" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsRelatorioSondagem" runat="server" style="margin-left: 10px;">
                <legend id="relTitulo" runat="server"></legend>
                <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                <uc1:UCComboUAEscola runat="server" ID="UCComboUAEscola" AsteriscoObg="true" ObrigatorioEscola="true" ObrigatorioUA="true" ValidationGroup="Relatorio"
                    CarregarEscolaAutomatico="true" MostraApenasAtivas="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" />
                <uc1:UCCCalendario runat="server" ID="UCCCalendario" Obrigatorio="true" MostrarMensagemSelecione="true" PermiteEditar="false" ValidationGroup="Relatorio" />
                <uc1:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" Obrigatorio="true" MostrarMensagemSelecione="true" PermiteEditar="false" ValidationGroup="Relatorio" />
                <uc1:UCComboCurriculoPeriodo runat="server" ID="UCComboCurriculoPeriodo" Obrigatorio="true" _MostrarMessageSelecione="true" PermiteEditar="false" ValidationGroup="Relatorio" />
                <uc1:UCComboTurma runat="server" ID="UCComboTurma" Obrigatorio="true" _MostrarMessageSelecione="true" PermiteEditar="false" ValidationGroup="Relatorio" />
                <uc1:UCComboTurmaDisciplina runat="server" ID="UCComboTurmaDisciplina" Obrigatorio="false" PermiteEditar="false" MostrarMensagemSelecione="true" ValidationGroup="Relatorio" VS_MostraFilhosRegencia="false" />
                <div runat="server" id="divData">
                    <div style="display: inline-block">
                        <asp:Label ID="lblDataInicio" runat="server" Text="Data inicial *" AssociatedControlID="txtDataInicio"></asp:Label>
                        <asp:TextBox runat="server" ID="txtDataInicio" SkinID="Data"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDataInicio" runat="server" ErrorMessage="Data inicial é obrigatória."
                            Display="Dynamic" ControlToValidate="txtDataInicio" ValidationGroup="Relatorio">*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                            Display="Dynamic" ErrorMessage="Data início deve estar no formato DD/MM/AAAA." OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                    </div>
                    &nbsp;
                    <div style="display: inline-block">
                        <asp:Label ID="lblDataFim" runat="server" Text="Data final *" AssociatedControlID="txtDataFim"></asp:Label>
                        <asp:TextBox runat="server" ID="txtDataFim" SkinID="Data"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDataFim" runat="server" ErrorMessage="Data final é obrigatória."
                            Display="Dynamic" ControlToValidate="txtDataFim" ValidationGroup="Relatorio">*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvDataFim" runat="server" ControlToValidate="txtDataFim"
                            ValidationGroup="Relatorio" Display="Dynamic" ErrorMessage="Data fim deve estar no formato DD/MM/AAAA." OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                        <asp:CompareValidator ID="cpvDataFim" runat="server" ControlToValidate="txtDataFim"
                            ValidationGroup="Relatorio" Display="Dynamic" Type="Date" Operator="GreaterThanEqual"
                            ControlToCompare="txtDataInicio" ErrorMessage="Data fim da análise deve ser maior ou igual à data início.">*</asp:CompareValidator>
                    </div>
                </div>
                <div class="right area-botoes-bottom">
                    <asp:Button ID="btnGerar" runat="server" Text="Gerar documento" OnClick="btnGerar_Click" ValidationGroup="Relatorio" />
                    <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

