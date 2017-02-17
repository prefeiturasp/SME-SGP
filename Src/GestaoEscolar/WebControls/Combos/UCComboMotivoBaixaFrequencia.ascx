<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboMotivoBaixaFrequencia.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboMotivoBaixaFrequencia" %>
<div runat="server" id="divCombo">
    <asp:Button ID="btnExpandir" runat="server" Text="   -- Selecione um motivo de baixa frequência --   " OnClick="btnExpandir_Click"
        CssClass="comboHierarquicoButton" />
    <asp:TreeView ID="tvMotivosBaixaFreq" runat="server" OnSelectedNodeChanged="tvMotivosBaixaFreq_SelectedNodeChanged" 
        Visible="false" CssClass="comboHierarquicoItens">
        <NodeStyle CssClass="comboHierarquicoNodes" />
    </asp:TreeView>
    <asp:HiddenField ID="hdnValor" runat="server" />
</div>