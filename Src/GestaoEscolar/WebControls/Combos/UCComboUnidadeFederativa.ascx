<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboUnidadeFederativa.ascx.cs" Inherits="MSTech.CoreSSO.UserControlLibrary.Combos.UCComboUnidadeFederativa" %>
<asp:Label ID="_lblRotulo" runat="server" AssociatedControlID="_ddlCombo" 
    Text="Estado"></asp:Label>
<asp:DropDownList ID="_ddlCombo" runat="server" DataSourceID="_odsCombo" DataTextField="unf_nome"
    DataValueField="unf_id" AppendDataBoundItems="True" SkinID="text30C">
</asp:DropDownList>
<asp:CompareValidator ID="_cpvCombo" runat="server" ControlToValidate="_ddlCombo"
    Display="Dynamic" ErrorMessage="{0} é obrigatório" Operator="NotEqual" ValueToCompare="-1">*</asp:CompareValidator>
<asp:ObjectDataSource ID="_odsCombo" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.END_UnidadeFederativa"
    SelectMethod="GetSelect" TypeName="MSTech.CoreSSO.BLL.END_UnidadeFederativaBO"
    OnSelecting="_odsCombo_Selecting" EnablePaging="True" MaximumRowsParameterName="pageSize"
    SelectCountMethod="GetTotalRecords" 
    StartRowIndexParameterName="currentPage">
</asp:ObjectDataSource>
