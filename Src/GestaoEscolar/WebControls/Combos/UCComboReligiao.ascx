<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboReligiao" Codebehind="UCComboReligiao.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Religião" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="rlg_nome" DataValueField="rlg_id" SkinID="text30C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Religião é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Religiao"
    SelectMethod="SelecionaReligiao" TypeName="MSTech.GestaoEscolar.BLL.ACA_ReligiaoBO"
    OnSelected="odsDados_Selected"></asp:ObjectDataSource>
