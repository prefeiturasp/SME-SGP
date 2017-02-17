<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboAtividadeAvaliativa.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.UCComboAtividadeAvaliativa" %>
<asp:Label ID="lblAtividade" runat="server" Text="Atividade avaliativa" AssociatedControlID="ddlAtividade"></asp:Label>
<asp:DropDownList ID="ddlAtividade" runat="server" AppendDataBoundItems="True" DataTextField="nome"
    DataValueField="tnt_id" SkinID="text60C" OnSelectedIndexChanged="ddlAtividade_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Atividade avaliativa é obrigatório."
    ControlToValidate="ddlAtividade" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>