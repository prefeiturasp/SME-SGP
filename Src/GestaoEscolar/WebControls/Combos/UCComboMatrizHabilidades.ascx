<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboMatrizHabilidades.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboMatrizHabilidades" %>
<asp:Label ID="lblTitulo" runat="server" Text="Matriz de habilidades" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="mat_nome" DataValueField="mat_id" SkinID="text60C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Matriz de habilidades é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ORC_MatrizHabilidades"
    SelectMethod="SelectComboMatrizHabilidades" TypeName="MSTech.GestaoEscolar.BLL.ORC_MatrizHabilidadesBO"
    OnSelected="odsDados_Selected">    
</asp:ObjectDataSource>