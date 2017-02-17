<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboEscalaAvaliacaoParecer" Codebehind="UCComboEscalaAvaliacaoParecer.ascx.cs" %>
<div>
    <asp:Label ID="_lblEscalaAvaliacao" runat="server" Text="Parecer mínimo para aprovação *" AssociatedControlID="_ddlEscalaParecer"></asp:Label>
    <asp:DropDownList ID="_ddlEscalaParecer" runat="server" AppendDataBoundItems="True"
        DataSourceID="odsEscalaParecer" DataTextField="descricao" DataValueField="eap_valor"
        SkinID="text30C">
    </asp:DropDownList>
    <asp:Label ID="lblMessage" runat="server" Text="" visible="false" EnableViewState="false" SkinID="SkinMsgErroCombo" ></asp:Label>
    
    <asp:CompareValidator ID="_cpvParecer" runat="server" 
                                ControlToValidate="_ddlEscalaParecer" Display="Dynamic" 
                                ErrorMessage="Escolha um parecer." 
        Operator="NotEqual" ValidationGroup="_ValidationParecer" 
                                ValueToCompare="-1" Visible ="false">*</asp:CompareValidator>
    
    <asp:ObjectDataSource ID="odsEscalaParecer" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_EscalaAvaliacao"
        SelectMethod="Seleciona_esa_id" TypeName="MSTech.GestaoEscolar.BLL.ACA_EscalaAvaliacaoParecerBO"
        EnablePaging="false" OnSelecting="odsEscalaParecer_Selecting" DeleteMethod="Delete"
        OldValuesParameterFormatString="original_{0}" UpdateMethod="Save" 
        onselected="odsEscalaParecer_Selected">
        <SelectParameters>
            <asp:Parameter Name="esa_id" Type="Int32" />
            <asp:Parameter Name="paginado" Type="Boolean" DefaultValue="false" />
            <asp:Parameter Name="currentPage" Type="Int32" DefaultValue="1" />
            <asp:Parameter Name="pageSize" Type="Int32" DefaultValue="1" />
        </SelectParameters>
    </asp:ObjectDataSource>
</div>
