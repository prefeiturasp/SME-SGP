<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCTurma.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCCTurma" %>

<asp:Label ID="lblTitulo" runat="server" Text="Turma" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    DataTextField="tur_cod_desc_nome" DataValueField="tur_crp_ttn_id" SkinID="text60C" 
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Turma é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
<div id="divDadosTurma" runat="server" visible="false" style="display:inline;position: absolute;margin-left: 10px;margin-top: -5px;">
    <asp:Label ID="lblCapacidade" runat="server" ></asp:Label>
    <asp:Label ID="lblMatriculados" runat="server" ></asp:Label>
</div>