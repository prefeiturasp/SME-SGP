<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.MatrizHabilidades.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc1" TagName="UCCamposObrigatorios" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional" EnableViewState="false">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsOrientacao" runat="server" ValidationGroup="MatrizHabilidades" />
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlPesquisa" runat="server" GroupingText="<%$ Resources:Academico, MatrizHabilidades.Busca.pnlPesquisa.GroupingText %>">        
        <asp:Label ID="lblNome" runat="server" Text="<%$ Resources:Academico, MatrizHabilidades.Busca.lblNome.Text %>" AssociatedControlID="txtNome"></asp:Label>
        <asp:TextBox ID="txtNome" runat="server" SkinID="text60C"></asp:TextBox>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="<%$ Resources:Academico, MatrizHabilidades.Busca.btnPesquisar.Text %>" ValidationGroup="MatrizHabilidades" OnClick="btnPesquisar_Click" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Academico, MatrizHabilidades.Busca.btnLimparPesquisa.Text %>" CausesValidation="false" OnClick="btnLimparPesquisa_Click" />
            <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Academico, MatrizHabilidades.Busca.btnNovo.Text %>" OnClick="btnNovo_Click" />
        </div>
    </asp:Panel>
   <asp:Panel runat="server" ID="pnlResultado" GroupingText="<%$ Resources:Academico, MatrizHabilidades.Busca.pnlResultado.GroupingText %>" Visible="False">
        <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="gvMatriz" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" EmptyDataText="<%$ Resources:Academico, MatrizHabilidades.Busca.gvMatriz.EmptyDataText %>"
            DataKeyNames="mat_id" OnDataBound="gvMatriz_DataBound" OnRowCommand="gvMatriz_RowCommand" OnRowDataBound="gvMatriz_RowDataBound"
            DataSourceID="odsMatriz">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:Academico, MatrizHabilidades.Busca.gvMatriz.HeaderTextNome %>" SortExpression="mat_nome">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lkbNome" Text='<%# Bind("mat_nome") %>'
                            PostBackUrl="~/Academico/MatrizHabilidades/Cadastro.aspx"
                            CommandName="Edit"></asp:LinkButton> 
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField HeaderText="<%$ Resources:Academico, MatrizHabilidades.Busca.gvMatriz.HeaderTextPadrao %>"
                    DataField="mat_padrao" ItemStyle-HorizontalAlign="Center" />  --%>
                <asp:TemplateField HeaderText="<%$ Resources:Academico, MatrizHabilidades.Busca.gvMatriz.HeaderTextPadrao %>">
                    <ItemTemplate>
                        <asp:Label ID="lblPadrao" runat="server" Text='<%# Bind("mat_padrao") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<%$ Resources:Academico, MatrizHabilidades.Busca.gvMatriz.HeaderTextExcluir %>" 
                    HeaderStyle-CssClass="center">
                    <ItemTemplate>
                        <asp:ImageButton ID="_imgExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                            ToolTip="<%$ Resources:Academico, MatrizHabilidades.Busca.gvMatriz._imgExcluir.ToolTip %>"
                            CausesValidation="false" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" Width="25px" />
                    <ItemStyle HorizontalAlign="Center" />
                 </asp:TemplateField>                                            
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsMatriz" runat="server" 
            SelectMethod="BuscaMatrizHabilidades" TypeName="MSTech.GestaoEscolar.BLL.ORC_MatrizHabilidadesBO"
            DataObjectTypeName="MSTech.GestaoEscolar.Entities.ORC_MatrizHabilidades"
            OnSelecting="odsMatriz_Selecting"></asp:ObjectDataSource>
        <uc2:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="gvMatriz" />
    </asp:Panel>
</asp:Content>
