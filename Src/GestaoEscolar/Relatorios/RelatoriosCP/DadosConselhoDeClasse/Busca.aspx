<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.RelatoriosCP.DadosConselhoDeClasse.Busca" %>

<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagName="UCCPeriodoCalendario"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCComboTurma"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/BuscaAluno/UCCamposBuscaAluno.ascx" TagName="UCCamposBuscaAluno"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Busca/UCAluno.ascx" TagName="UCAluno"
    TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="uppPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <fieldset id="fdsPesquisa" runat="server" style="margin-left: 10px;">
                    <legend id="lgdTitulo" runat="server"></legend>
                    <uc10:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" Visible="false" />
                    <div id="_divPesquisa" class="divPesquisa" runat="server">
                        <asp:Label ID="lblAvisoMensagem" runat="server"></asp:Label>
                        <!-- FiltrosPadrao -->
                        <uc3:UCComboUAEscola ID="UCComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                            MostrarMessageSelecioneEscola="true" 
                            MostrarMessageSelecioneUA="true" OnIndexChangedUA="UCComboUAEscola_IndexChangedUA"
                            OnIndexChangedUnidadeEscola="UCComboUAEscola_IndexChangedUnidadeEscola" />
                        <div id="msgCertConcCurso" class="msgInformacao" runat="server" style="margin-top: -30px; width: 180px;"
                            visible="false">
                            Verifique se os alunos possuem data de nascimento, naturalidade e nacionalidade
                            cadastrados.
                        </div>
                        <uc2:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" MostrarMensagemSelecione="true"
                            runat="server" />
                        <uc7:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" MostrarMensagemSelecione="true"
                            runat="server" />
                        <uc5:UCComboCalendario ID="UCComboCalendario1" runat="server" MostrarMensagemSelecione="true" />

                        <div id="divPeriodoCalendario" runat="server" visible="false">
                            <uc1:UCCPeriodoCalendario runat="server" ID="UCCPeriodoCalendario" 
                                MostrarMensagemSelecione="true"  Obrigatorio="false" PermiteEditar="false" />
                        </div>

                        <uc4:UCComboTurma ID="UCComboTurma1" runat="server" MostrarMessageSelecione="true" Obrigatorio ="true" />

                        <br /><br />
                        <asp:CheckBox ID="chkBuscaAvancada" runat="server" 
                            Text="Busca avançada"  OnCheckedChanged="chkBuscaAvancada_CheckedChanged" 
                            AutoPostBack="True"/>

                        <div id="divBuscaAvancada" runat="server" visible="false">
                            <uc6:UCCamposBuscaAluno ID="UCCamposBuscaAluno1" runat="server" />
                        </div>

                        <div class="clear">
                        </div>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar relatório" OnClick="btnGerarRelatorio_Click"  />
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
