<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboTipoMovimentacao"
    CodeBehind="UCComboTipoMovimentacao.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de movimentação" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tmv_nome" DataValueField="tmv_id" SkinID="text60C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo de movimentação é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoMovimentacao"
    SelectMethod="SelecionaTipoMovimentacao" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoMovimentacaoBO"
    OnSelected="odsDados_Selected">
    <SelectParameters>
        <asp:Parameter Name="tmv_motivo" DbType="Byte"/>
    </SelectParameters>
</asp:ObjectDataSource>
