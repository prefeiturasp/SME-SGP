<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboNivelEnsino" Codebehind="UCComboTipoNivelEnsino.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Nível de ensino" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tne_nome" DataValueField="tne_id" SkinID="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Nível de ensino é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoNivelEnsino"
    SelectMethod="SelecionaTipoNivelEnsino" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoNivelEnsinoBO"
    OnSelected="odsDados_Selected"></asp:ObjectDataSource>
