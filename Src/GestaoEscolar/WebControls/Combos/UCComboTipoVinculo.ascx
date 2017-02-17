<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboTipoVinculo"
    CodeBehind="UCComboTipoVinculo.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de vínculo" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tvi_nome" DataValueField="tvi_id" SkinID="text30C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo de vínculo é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.RHU_TipoVinculo"
    SelectMethod="SelecionaTipoVinculo" TypeName="MSTech.GestaoEscolar.BLL.RHU_TipoVinculoBO"
    OnSelected="odsDados_Selected">
    <SelectParameters>
        <asp:Parameter Name="ent_id" DbType="Guid" />
    </SelectParameters>
</asp:ObjectDataSource>
