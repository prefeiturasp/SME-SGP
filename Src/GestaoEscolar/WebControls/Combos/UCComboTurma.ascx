<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboTurma" Codebehind="UCComboTurma.ascx.cs" %>

<asp:Label ID="lblTurmas" runat="server" Text="Turma" AssociatedControlID="_ddlTurma"></asp:Label>
<asp:DropDownList ID="_ddlTurma" runat="server" DataSourceID="odsTurma" DataTextField="tur_cod_desc_nome"
    DataValueField="tur_crp_ttn_id" OnSelectedIndexChanged="_ddlTurma_SelectedIndexChanged"
    SkinID="text60C"
    OnDataBound="_ddlTurma_DataBound">
</asp:DropDownList>
<asp:CompareValidator ID="_cpvCombo" runat="server" ControlToValidate="_ddlTurma"
    Display="Dynamic" ErrorMessage="Turma é obrigatório." Operator="NotEqual" Visible="false"
    ValueToCompare="-1;-1;-1">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<div id="divDadosTurma" runat="server" visible="false" style="display:inline;position: absolute;margin-left: 10px;margin-top: -5px;">
    <asp:Label ID="lblCapacidade" runat="server" ></asp:Label>
    <asp:Label ID="lblMatriculados" runat="server" ></asp:Label>
    <asp:HiddenField ID="hdnQuantidadesDoCombo" runat="server" />
</div>
<asp:ObjectDataSource ID="odsTurma" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.TUR_Turma"
    DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" SelectMethod="GetSelectBy_Escola_Periodo_Situacao"
    TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaBO" UpdateMethod="Save" EnablePaging="false"
    OnSelecting="odsTurma_Selecting" OnSelected="odsTurma_Selected">
    <SelectParameters>
        <asp:Parameter Name="usu_id" DbType="Guid" />
        <asp:Parameter Name="gru_id" DbType="Guid" />
        <asp:Parameter Name="adm" Type="Boolean" />
        <asp:Parameter Name="esc_id" Type="Int32" />
        <asp:Parameter Name="uni_id" Type="Int32" />
        <asp:Parameter Name="cal_id" Type="Int32" />
        <asp:Parameter Name="cur_id" Type="Int32" />
        <asp:Parameter Name="crr_id" Type="Int32" />
        <asp:Parameter Name="crp_id" Type="Int32" />
        <asp:Parameter Name="ent_id" DbType="Guid" />
        <asp:Parameter Name="tur_tipo" Type="Byte" />
        <asp:Parameter Name="tur_situacao" Type="Byte" />
    </SelectParameters>
</asp:ObjectDataSource>
