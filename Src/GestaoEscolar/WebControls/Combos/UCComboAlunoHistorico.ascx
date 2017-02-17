<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboAlunoHistorico"
    CodeBehind="UCComboAlunoHistorico.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Observação padrão" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="hop_nome" DataValueField="hop_id" SkinID="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Observação é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_HistoricoObservacaoPadrao"
    SelectMethod="SelecionaTodos" TypeName="MSTech.GestaoEscolar.BLL.ACA_HistoricoObservacaoPadraoBO"
    OnSelected="odsDados_Selected" DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}"
    UpdateMethod="Save"></asp:ObjectDataSource>
