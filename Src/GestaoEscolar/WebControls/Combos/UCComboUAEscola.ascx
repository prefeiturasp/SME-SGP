<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboUAEscola.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.UCComboUAEscola" %>

<!-- Unidade administrativa superior -->
<asp:Label ID="lblUA" runat="server" Text="Unidade administrativa" AssociatedControlID="ddlUA"></asp:Label>
<asp:DropDownList ID="ddlUA" runat="server" AppendDataBoundItems="True" DataTextField="uad_nome"
    DataValueField="uad_id" OnSelectedIndexChanged="ddlUA_SelectedIndexChanged" SkinID="text60C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvUA" runat="server" ErrorMessage="Unidade administrativa é obrigatório."
    ControlToValidate="ddlUA" Operator="NotEqual" ValueToCompare="00000000-0000-0000-0000-000000000000" 
    Visible="false"
    Display="Dynamic">*</asp:CompareValidator>
<asp:Label ID="lblMessageUA" runat="server" Visible="false" EnableViewState="false" CssClass="msgErroCombo"></asp:Label>

<!-- Escola -->
<asp:Label ID="lblEscola" runat="server" Text="Escola"  AssociatedControlID="ddlUnidadeEscola"></asp:Label>
<asp:DropDownList ID="ddlUnidadeEscola" runat="server" AppendDataBoundItems="True" DataTextField="uni_escolaNome" 
    DataValueField="esc_uni_id" OnSelectedIndexChanged="ddlUnidadeEscola_SelectedIndexChanged" SkinID="text60C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvEscola" runat="server" ErrorMessage="Escola é obrigatório."
    ControlToValidate="ddlUnidadeEscola" Operator="NotEqual" ValueToCompare="-1;-1" 
    Visible="false"
    Display="Dynamic">*</asp:CompareValidator>
<asp:Label ID="lblMessageEscola" runat="server" Visible="false" EnableViewState="false" SkinID="msgErroCombo"></asp:Label>