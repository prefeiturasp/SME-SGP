<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboCargaHoraria.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.UCComboCargaHoraria" %>
<asp:Label ID="lblTitulo" runat="server" Text="Carga horária" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="chr_descricaoCompleta"
    DataValueField="chr_id" CssClass="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Carga horária é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
