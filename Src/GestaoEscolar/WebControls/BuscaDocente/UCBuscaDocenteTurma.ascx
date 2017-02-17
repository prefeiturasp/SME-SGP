<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCBuscaDocenteTurma.ascx.cs" Inherits="GestaoEscolar.WebControls.BuscaDocente.UCBuscaDocenteTurma" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagPrefix="uc1" TagName="UCComboUAEscola" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagPrefix="uc1" TagName="UCCCalendario" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagPrefix="uc1" TagName="UCCCursoCurriculo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagPrefix="uc1" TagName="UCComboTurma" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagPrefix="uc1" TagName="UCComboCurriculoPeriodo" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoCiclo.ascx" TagPrefix="uc1" TagName="UCComboTipoCiclo" %>


<asp:UpdatePanel ID="updMensagem" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="BuscaDocente" />
    </ContentTemplate>
</asp:UpdatePanel>
<div id="divPesquisa" runat="server">
    <uc1:UCComboUAEscola runat="server" ID="UCComboUAEscola" AsteriscoObg="true" ObrigatorioEscola="true" ObrigatorioUA="true"
        CarregarEscolaAutomatico="true" MostraApenasAtivas="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" />

    <uc1:UCCCalendario runat="server" ID="UCCCalendario" Obrigatorio="true" MostrarMensagemSelecione="true" PermiteEditar="false" />
   
     <div runat="server" id="divComboCursoTurma">
        
         <uc1:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" Obrigatorio="true" MostrarMensagemSelecione="true" PermiteEditar="false" />
         <uc1:UCComboTipoCiclo runat="server" ID="UCComboTipoCiclo" Obrigatorio="false" Titulo="Ciclo" Enabled="false" Visible="false" />
        <uc1:UCComboCurriculoPeriodo runat="server" ID="UCComboCurriculoPeriodo" Obrigatorio="true" _MostrarMessageSelecione="true" PermiteEditar="false" />
         <uc1:UCComboTurma runat="server" ID="UCComboTurma" Obrigatorio="true" _MostrarMessageSelecione="true" PermiteEditar="false" />
    </div>
</div>
