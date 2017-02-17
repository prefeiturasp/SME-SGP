<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboFuncao"
    CodeBehind="UCComboFuncao.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Função" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="fun_nome" DataValueField="fun_id" CssClass="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Função é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.RHU_Funcao"
    SelectMethod="SelecionaFuncao" TypeName="MSTech.GestaoEscolar.BLL.RHU_FuncaoBO"
    OnSelected="odsDados_Selected">
    <SelectParameters>
        <asp:Parameter Name="ent_id" DbType="Guid" />
    </SelectParameters>
</asp:ObjectDataSource>
