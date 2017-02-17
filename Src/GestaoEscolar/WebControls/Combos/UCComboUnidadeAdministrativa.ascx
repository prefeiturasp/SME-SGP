<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboUnidadeAdministrativa" Codebehind="UCComboUnidadeAdministrativa.ascx.cs" %>
<asp:Label ID="LabelUA" runat="server" Text="Unidade administrativa" AssociatedControlID="_ddlUA"></asp:Label>
<asp:DropDownList ID="_ddlUA" runat="server" AppendDataBoundItems="True" DataTextField="uad_nome"
    DataValueField="uad_id" OnSelectedIndexChanged="_ddlUA_SelectedIndexChanged"
    CssClass="text60C">
</asp:DropDownList>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false" CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsUA" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.SYS_UnidadeAdministrativa"
    SelectMethod="GetSelectBy_Pesquisa_PermissaoUsuario" TypeName="MSTech.CoreSSO.BLL.SYS_UnidadeAdministrativaBO"
    EnablePaging="false" DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}"
    OnSelecting="odsUA_Selecting"
    UpdateMethod="Save" onselected="odsUA_Selected"></asp:ObjectDataSource>


