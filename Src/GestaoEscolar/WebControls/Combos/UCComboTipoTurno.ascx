<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboTipoTurno"
    CodeBehind="UCComboTipoTurno.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de turno" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="ttn_nome"
    DataValueField="ttn_id" SkinID="text30C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo de turno é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<%--<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoTurno"
    SelectMethod="SelecionaTipoTurno" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoTurnoBO"
    OnSelected="odsDados_Selected"></asp:ObjectDataSource>--%>
