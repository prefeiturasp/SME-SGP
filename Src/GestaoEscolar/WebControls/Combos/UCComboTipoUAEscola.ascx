<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboTipoUAEscola" Codebehind="UCComboTipoUAEscola.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de escola" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tua_nome" DataValueField="tua_id" SkinID="text30C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo de escola é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="00000000-0000-0000-0000-000000000000"
    Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_ParametroAcademico"
    SelectMethod="SelecionaTipoUAEscola" TypeName="MSTech.GestaoEscolar.BLL.ACA_ParametroAcademicoBO"
    OnSelected="odsDados_Selected"></asp:ObjectDataSource>
