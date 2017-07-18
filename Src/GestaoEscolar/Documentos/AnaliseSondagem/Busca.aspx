<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Documentos.AnaliseSondagem.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc2" TagName="UCCamposObrigatorios" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagPrefix="uc1" TagName="UCComboUAEscola" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagPrefix="uc1" TagName="UCCCalendario" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagPrefix="uc1" TagName="UCCCursoCurriculo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagPrefix="uc1" TagName="UCComboTurma" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagPrefix="uc1" TagName="UCComboCurriculoPeriodo" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoCiclo.ascx" TagPrefix="uc1" TagName="UCComboTipoCiclo" %>
<%@ Register Src="~/WebControls/Combos/UCComboSondagem.ascx" TagPrefix="uc1" TagName="UCComboSondagem" %>

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

                <%--<asp:Label ID="lblTituloSondagem" runat="server" Text="Título da sondagem *" AssociatedControlID="txtTituloSondagem"></asp:Label>--%>
                <%--<asp:TextBox ID="txtTituloSondagem" runat="server" SkinID="text60C"></asp:TextBox>--%>
                <uc1:UCComboSondagem runat="server" id="UCComboSondagem" />
                <%--<asp:RequiredFieldValidator ID="rfvTituloSondagem" runat="server" ControlToValidate="txtTituloSondagem"
                    Display="Dynamic" ValidationGroup="Relatorio" ErrorMessage="Título da sondagem é obrigatório.">*</asp:RequiredFieldValidator>--%>

                <div runat="server" id="divData">
                    <div style="display: inline-block">
                        <asp:Label ID="lblDataInicio" runat="server" Text="Data inicial da análise *" AssociatedControlID="txtDataInicio"></asp:Label>
                        <asp:TextBox runat="server" ID="txtDataInicio" SkinID="Data"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDataInicio" runat="server" ErrorMessage="Data inicial da análise é obrigatória."
                            Display="Dynamic" ControlToValidate="txtDataInicio" ValidationGroup="Relatorio">*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                            Display="Dynamic" ErrorMessage="Data início deve estar no formato DD/MM/AAAA." OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                    </div>
                    &nbsp;
                    <div style="display: inline-block">
                        <asp:Label ID="lblDataFim" runat="server" Text="Data final da análise *" AssociatedControlID="txtDataFim"></asp:Label>
                        <asp:TextBox runat="server" ID="txtDataFim" SkinID="Data"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDataFim" runat="server" ErrorMessage="Data final da análise é obrigatória."
                            Display="Dynamic" ControlToValidate="txtDataFim" ValidationGroup="Relatorio">*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvDataFim" runat="server" ControlToValidate="txtDataFim"
                            ValidationGroup="Relatorio" Display="Dynamic" ErrorMessage="Data fim deve estar no formato DD/MM/AAAA." OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                        <asp:CompareValidator ID="cpvDataFim" runat="server" ControlToValidate="txtDataFim"
                            ValidationGroup="Relatorio" Display="Dynamic" Type="Date" Operator="GreaterThanEqual"
                            ControlToCompare="txtDataInicio" ErrorMessage="Data fim da análise deve ser maior ou igual à data início.">*</asp:CompareValidator>
                    </div>
                </div>

                <uc1:UCComboUAEscola runat="server" ID="UCComboUAEscola" ValidationGroup="Relatorio" CarregarEscolaAutomatico="true" MostraApenasAtivas="true" 
                    MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" AsteriscoObg="true" ObrigatorioEscola="true" ObrigatorioUA="true" />

                <uc1:UCCCalendario runat="server" ID="UCCCalendario" Obrigatorio="false" MostrarMensagemSelecione="true" PermiteEditar="false" ValidationGroup="Relatorio" />

                <uc1:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" Obrigatorio="false" MostrarMensagemSelecione="true" PermiteEditar="false" ValidationGroup="Relatorio" />
                
                <uc1:UCComboTipoCiclo runat="server" ID="UCComboTipoCiclo" Obrigatorio="false" Titulo="Ciclo" Enabled="false" Visible="false" ValidationGroup="Relatorio" />
                
                <uc1:UCComboCurriculoPeriodo runat="server" ID="UCComboCurriculoPeriodo" Obrigatorio="true" _MostrarMessageSelecione="true" PermiteEditar="false" ValidationGroup="Relatorio" />
                
                <uc1:UCComboTurma runat="server" ID="UCComboTurma" Obrigatorio="true" _MostrarMessageSelecione="true" PermiteEditar="false" ValidationGroup="Relatorio" />

                <asp:CheckBox runat="server" ID="chkSuprimirPercentual" Text="Suprimir percentual das respostas" />

                <div class="right area-botoes-bottom">
                    <asp:Button ID="btnGerar" runat="server" Text="Gerar documento" OnClick="btnGerar_Click" ValidationGroup="Relatorio" />
                    <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
