<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoUnidadeAdministrativa.ascx.cs" Inherits="MSTech.CoreSSO.UserControlLibrary.Combos.UCComboTipoUnidadeAdministrativa" %>
<asp:Label ID="_lblRotulo" runat="server" AssociatedControlID="_ddlCombo" 
    Text="Tipo de unidade administrativa"></asp:Label>
<asp:DropDownList ID="_ddlCombo" runat="server" DataSourceID="_odsCombo" DataTextField="tua_nome"
    DataValueField="tua_id" AppendDataBoundItems="True" SkinID="text30C">
</asp:DropDownList>
<asp:CompareValidator ID="_cpvCombo" runat="server" ControlToValidate="_ddlCombo"
    Display="Dynamic" ErrorMessage="{0} é obrigatório" Operator="NotEqual" ValueToCompare="-1">*</asp:CompareValidator>
<asp:ObjectDataSource ID="_odsCombo" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.SYS_TipoUnidadeAdministrativa"
    SelectMethod="GetSelect" TypeName="MSTech.CoreSSO.BLL.SYS_TipoUnidadeAdministrativaBO"
    OnSelecting="_odsCombo_Selecting" EnablePaging="True" MaximumRowsParameterName="pageSize"
    SelectCountMethod="GetTotalRecords" 
    StartRowIndexParameterName="currentPage">
</asp:ObjectDataSource>
