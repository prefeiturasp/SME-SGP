<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboEntidade.ascx.cs" Inherits="MSTech.CoreSSO.UserControlLibrary.Combos.UCComboEntidade" %>
<asp:Label ID="_lblRotulo" runat="server" AssociatedControlID="_ddlCombo" 
    Text="Entidade"></asp:Label>
<asp:DropDownList ID="_ddlCombo" runat="server" DataSourceID="_odsCombo" 
    DataTextField="ent_razaoSocial" DataValueField="ent_id"  SkinID="text30C"
    AppendDataBoundItems="True">
</asp:DropDownList>
<asp:CompareValidator ID="_cpvCombo" runat="server" 
    ControlToValidate="_ddlCombo" Display="Dynamic" 
    ErrorMessage="{0} é obrigatório" Operator="NotEqual" ValueToCompare="00000000-0000-0000-0000-000000000000">*</asp:CompareValidator>
<asp:ObjectDataSource ID="_odsCombo" runat="server" 
    DataObjectTypeName="MSTech.CoreSSO.Entities.SYS_Entidade" 
    OldValuesParameterFormatString="original_{0}" SelectMethod="GetSelect" 
    TypeName="MSTech.CoreSSO.BLL.SYS_EntidadeBO" 
    onselecting="_odsCombo_Selecting" EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords" StartRowIndexParameterName="currentPage">
</asp:ObjectDataSource>
