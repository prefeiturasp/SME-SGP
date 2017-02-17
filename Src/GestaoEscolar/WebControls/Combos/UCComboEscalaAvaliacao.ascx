<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboEscalaAvaliacao"
    CodeBehind="UCComboEscalaAvaliacao.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Escala de avaliação" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="esa_nome"
    DataValueField="esa_id_tipo" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged"
    SkinID="text30C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Escala de avaliação é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1;-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="Label1" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
