<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboCursoCurriculo"
    CodeBehind="UCComboCursoCurriculo.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Curso" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="cur_crr_nome" DataValueField="cur_crr_id" SkinID="text60C" 
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Curso é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1;-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Curso"
    SelectMethod="SelecionaCursoCurriculo" TypeName="MSTech.GestaoEscolar.BLL.ACA_CursoBO"
    OnSelected="odsDados_Selected" OnSelecting="odsDados_Selecting">
    <SelectParameters>
        <asp:Parameter Name="esc_id" DbType="Int32" />
        <asp:Parameter Name="uni_id" DbType="Int32" />
        <asp:Parameter Name="cur_situacao" DbType="Byte" />
        <asp:Parameter Name="ent_id" DbType="Guid" />
        <asp:Parameter Name="mostraEJAModalidades" DbType="Boolean"/>
        <asp:Parameter Name="appMinutosCacheLongo" DbType="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
