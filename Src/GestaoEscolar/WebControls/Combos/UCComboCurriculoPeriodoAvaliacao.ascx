<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboCurriculoPeriodoAvaliacao.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.UCComboCurriculoPeriodoAvaliacao1" %>
<asp:Label ID="lblTitulo" runat="server" Text="Avaliação" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" DataTextField="crp_nomeAvaliacao"
    DataValueField="numeroAvaliacao" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged"
    SkinID="text60C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ControlToValidate="ddlCombo" Display="Dynamic"
    ErrorMessage="Avaliação é obrigatório." Operator="NotEqual" ValueToCompare="-1"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>