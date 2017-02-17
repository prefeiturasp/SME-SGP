<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCCadastroTipoRecursoFisicoDidatico" Codebehind="UCComboTipoRecursoFisicoDidatico.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Recurso físico didático" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="trc_nome" DataValueField="trc_id" SkinID="text60C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Recurso físico didático é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ESC_TipoRecursoFisicoDidatico"
    SelectMethod="SelecionaRecursoFisicoDidatico" TypeName="MSTech.GestaoEscolar.BLL.ESC_TipoRecursoFisicoDidaticoBO"
    OnSelected="odsDados_Selected"></asp:ObjectDataSource>
