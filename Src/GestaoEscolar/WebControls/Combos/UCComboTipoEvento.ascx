<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboTipoEvento"
    CodeBehind="UCComboTipoEvento.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de evento" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tev_nome" DataValueField="tev_id" SkinID="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo de evento é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoEvento"
    SelectMethod="SelecionaTipoEvento" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoEventoBO"
    OnSelected="odsDados_Selected">
    <SelectParameters>
        <asp:Parameter Name="tev_situacao" DbType="Byte" />
    </SelectParameters>
</asp:ObjectDataSource>
