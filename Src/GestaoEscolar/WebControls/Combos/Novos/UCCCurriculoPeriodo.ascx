<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCCurriculoPeriodo.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCCCurriculoPeriodo" %>
    
<asp:Label ID="lblTitulo" runat="server" Text="Período" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    DataTextField="crp_descricao" DataValueField="cur_id_crr_id_crp_id" SkinID="text60C" 
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:Label ID="lblFormatoPeriodo" runat="server" Text="(Ano/Série)" Visible="false"></asp:Label>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Período é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual"  Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>