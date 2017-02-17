<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboTurno"
    CodeBehind="UCComboTurno.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Turno" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="trn_descricao" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged" DataValueField="trn_id" SkinID="text30C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Turno é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Turno"
    SelectMethod="SelecionaTurno" TypeName="MSTech.GestaoEscolar.BLL.ACA_TurnoBO"
    OnSelecting="odsDados_Selecting" OnSelected="odsDados_Selected">
    <SelectParameters>
        <asp:Parameter Name="ent_id" DbType="Guid" />
    </SelectParameters>
</asp:ObjectDataSource>
