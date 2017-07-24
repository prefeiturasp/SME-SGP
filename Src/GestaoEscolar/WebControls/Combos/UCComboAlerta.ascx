<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboAlerta.ascx.cs" Inherits="WebControls_Combos_UCComboAlerta" %>
<asp:Label ID="lblTitulo" runat="server" Text="Alerta" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="cfa_nome" DataValueField="cfa_id" SkinID="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Alerta é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.CFG_Alerta"
    SelectMethod="SelecionarAlertas" TypeName="MSTech.GestaoEscolar.BLL.CFG_AlertaBO"
    OnSelected="odsDados_Selected">
</asp:ObjectDataSource>