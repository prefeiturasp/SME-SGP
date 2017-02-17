<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboModalidadeEnsino" Codebehind="UCComboTipoModalidadeEnsino.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Modalidade de ensino" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    DataTextField="tme_nome" DataValueField="tme_id" SkinID="text60C"
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<%--DataSourceID="odsDados"--%>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Modalidade de ensino é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<%--<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoModalidadeEnsino"
    SelectMethod="SelecionaTipoModalidadeEnsino" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoModalidadeEnsinoBO"
    OnSelected="odsDados_Selected"></asp:ObjectDataSource>--%>
