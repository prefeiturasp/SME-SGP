<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Cidade_UCCadastroCidade" Codebehind="UCCadastroCidade.ascx.cs" %>
<%@ Register Src="../Combos/UCComboPais.ascx" TagName="UCComboPais" TagPrefix="uc1" %>
<%@ Register Src="../Combos/UCComboUnidadeFederativa.ascx" TagName="UCComboUnidadeFederativa"
    TagPrefix="uc2" %>
<fieldset class="fieldset">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vlgPais" />
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <uc1:UCComboPais ID="UCComboPais1" runat="server" _ValidationGroup="vlgPais" />
    <uc2:UCComboUnidadeFederativa ID="UCComboUnidadeFederativa1" runat="server" _ValidationGroup="vlgPais" />
    <asp:Label ID="LabelCidade" runat="server" Text="Cidade *" AssociatedControlID="_txtCidade"></asp:Label>
    <asp:TextBox ID="_txtCidade" runat="server" MaxLength="200" SkinID="text30C"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvCidade" runat="server" ControlToValidate="_txtCidade"
        ErrorMessage="Cidade é obrigatório." ValidationGroup="vlgPais">*</asp:RequiredFieldValidator>
    <asp:Label ID="LabelDDD" runat="server" Text="DDD" AssociatedControlID="_txtDDD"></asp:Label>
    <asp:TextBox ID="_txtDDD" runat="server" MaxLength="3" CssClass="numeric" SkinID="Numeros"></asp:TextBox>
    <asp:RegularExpressionValidator ID="_revDDD" runat="server" ControlToValidate="_txtDDD"
        ValidationGroup="vlgPais" Display="Dynamic" ErrorMessage="DDD inválido." ValidationExpression="^([0-9]){1,10}$">*</asp:RegularExpressionValidator>
    <div class="right">
        <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" ValidationGroup="vlgPais"
            OnClick="_btnSalvar_Click" />
        <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="False" />
    </div>
</fieldset>
