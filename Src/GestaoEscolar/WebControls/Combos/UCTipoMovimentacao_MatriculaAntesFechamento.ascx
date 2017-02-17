<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCTipoMovimentacao_MatriculaAntesFechamento.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCTipoMovimentacao_MatriculaAntesFechamento" %>

<asp:Label ID="lblTitulo" runat="server" Text="Tipo de movimentação *"></asp:Label>
<asp:RadioButtonList ID="rdbTipoMovimentacao" runat="server" 
    onselectedindexchanged="rdbTipoMovimentacao_SelectedIndexChanged">
</asp:RadioButtonList>
<asp:RequiredFieldValidator ID="rfvTipoMovimentacao" runat="server" 
    Display="Dynamic" ControlToValidate="rdbTipoMovimentacao"
    ErrorMessage="Tipo de movimentação é obrigatório.">*</asp:RequiredFieldValidator>