<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboPais.ascx.cs"
    Inherits="MSTech.CoreSSO.UserControlLibrary.Combos.UCComboPais" %>
<asp:Label ID="_lblRotulo" runat="server" AssociatedControlID="_ddlCombo" Text="País"></asp:Label>
<asp:DropDownList ID="_ddlCombo" runat="server" DataSourceID="_odsCombo" DataTextField="pai_nome"
    DataValueField="pai_id" AppendDataBoundItems="True" SkinID="text30C">
</asp:DropDownList>
<asp:CompareValidator ID="_cpvCombo" runat="server" ControlToValidate="_ddlCombo"
    Display="Dynamic" ErrorMessage="{0} é obrigatório" Operator="NotEqual" ValueToCompare="-1">*</asp:CompareValidator>
<asp:ObjectDataSource ID="_odsCombo" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.END_Pais"
    OldValuesParameterFormatString="original_{0}" SelectMethod="GetSelect" TypeName="MSTech.CoreSSO.BLL.END_PaisBO"
    OnSelecting="_odsCombo_Selecting" EnablePaging="True" MaximumRowsParameterName="pageSize"
    SelectCountMethod="GetTotalRecords" StartRowIndexParameterName="currentPage">
</asp:ObjectDataSource>
