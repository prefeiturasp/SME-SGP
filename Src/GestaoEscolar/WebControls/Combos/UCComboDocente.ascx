<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboDocente"
    CodeBehind="UCComboDocente.ascx.cs" %>
<asp:Label ID="_lblDocente" runat="server" Text="Docente" AssociatedControlID="_ddlDocente"></asp:Label>
<asp:DropDownList ID="_ddlDocente" runat="server" AppendDataBoundItems="True" DataTextField="doc_nome"
    DataValueField="doc_id" SkinID="text60C" OnSelectedIndexChanged="_ddlDocente_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Docente é obrigatório."
    ControlToValidate="_ddlDocente" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
