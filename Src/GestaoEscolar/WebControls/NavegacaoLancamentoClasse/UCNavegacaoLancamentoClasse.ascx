<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCNavegacaoLancamentoClasse.ascx.cs"
    Inherits="GestaoEscolar.WebControls.NavegacaoLancamentoClasse.UCNavegacaoLancamentoClasse" %>
<div runat="server" id="divBotoesNavegacao" class="planEscopo" style="text-align:right;">
    <div runat="server" id="divInfoLancamentoFrequenciaMensal" style="text-align:left;">
        <asp:Label ID="lblInfoLancamentoFrequenciaMensal" runat="server"></asp:Label>
    </div>
    <asp:Button ID="btnEfetivacao" runat="server" Text="Efetivação de notas" OnClick="btnEfetivacao_Click"
        ToolTip="Efetivar notas da turma" CausesValidation="false" />
    <asp:Button ID="btnLancamentoAvaliacao" runat="server" Text="Lançamento de notas"
        OnClick="btnLancamentoAvaliacao_Click" ToolTip="Lançar notas da turma" CausesValidation="false" />
    <asp:Button ID="btnLancamentoFrequencia" runat="server" Text="Lançamento de frequências"
        OnClick="btnLancamentoFrequencia_Click" ToolTip="Lançar frequências da turma" CausesValidation="false" />
    <asp:Button ID="btnLancamentoFrequenciaMensal" runat="server" Text="Lançamento de frequência mensal"
        OnClick="btnLancamentoFrequenciaMensal_Click" ToolTip="Lançar frequência mensal da turma" CausesValidation="false" />
</div>
<div></div>