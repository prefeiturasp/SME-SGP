<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCBuscaLancamentoClasse.ascx.cs"
    Inherits="GestaoEscolar.WebControls.BuscaLancamentoClasse.UCBuscaLancamentoClasse" %>
<%@ Register Src="../Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc1" %>
<%@ Register Src="../Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="../Combos/UCComboCalendario.ascx" TagName="UCComboCalendario" TagPrefix="uc3" %>
<%@ Register Src="../Combos/UCComboTurno.ascx" TagName="UCComboTurno" TagPrefix="uc4" %>
<asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
<asp:Panel ID="pnlPesquisa" runat="server">
    <div id="divPesquisa" runat="server">
        <asp:UpdatePanel ID="upnBusca" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCComboUAEscola ID="UCComboUAEscola1" runat="server" CarregarEscolaAutomatico="true"
                    ObrigatorioEscola="false" ObrigatorioUA="false" MostrarMessageSelecioneEscola="true"
                    MostrarMessageSelecioneUA="true" />
                <uc2:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" runat="server" />
                <uc3:UCComboCalendario ID="UCComboCalendario1" runat="server" />
                <uc4:UCComboTurno ID="UCComboTurno1" runat="server" />
                <asp:Label ID="lblCodigoTurma" runat="server" Text="Código da turma" AssociatedControlID="txtCodigoTurma"></asp:Label>
                <asp:TextBox ID="txtCodigoTurma" runat="server" MaxLength="30" SkinID="text30C"></asp:TextBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="right">
        <asp:Button ID="btnPesquisarTurmas" runat="server" Text="Pesquisar" OnClick="btnPesquisarTurmas_Click" />
        <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click" />
    </div>
</asp:Panel>