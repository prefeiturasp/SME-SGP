<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboTipoFormacaoDocente" Codebehind="UCComboTipoFormacaoDocente.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Formação *" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tfd_nome" DataValueField="tfd_id" SkinID="text30C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Formação é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoFormacaoDocente"
    SelectMethod="SelecionaTipoFormacaoDocente" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoFormacaoDocenteBO"
    OnSelected="odsDados_Selected"></asp:ObjectDataSource>
