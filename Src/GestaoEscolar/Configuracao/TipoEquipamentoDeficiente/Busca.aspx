<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Busca.aspx.cs" Inherits="Configuracao_TipoEquipamentoDeficiente_Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
    <fieldset>
        <legend>
           <asp:Label runat="server" ID="lblLegend" 
                      Text="<%$ Resources:Configuracao, TipoEquipamentoDeficiente.Busca.lblLegend.Text %>"></asp:Label>
        </legend>
        <div></div>
        <asp:Button ID="btnTipoEquipamentoDeficiente" runat="server" 
            Text="<%$ Resources:Configuracao, TipoEquipamentoDeficiente.Busca.btnTipoEquipamentoDeficiente.Text %>"
            OnClick="btnTipoEquipamentoDeficiente_Click" />
        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" ComboDefaultValue="true"
            OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
          <asp:GridView ID="grvTipoEquipamentoDeficiente" runat="server" DataKeyNames="ted_id"
            DataSourceID="OdsTipoEquipamentoDeficiente" OnRowDataBound="grvTipoEquipamentoDeficiente_RowDataBound"
            AllowPaging="True" AutoGenerateColumns="False" OnDataBound="grvTipoEquipamentoDeficiente_DataBound"
            EmptyDataText="<%$ Resources:Configuracao, TipoEquipamentoDeficiente.Busca.grvTipoEquipamentoDeficiente.EmptyDataText %>" 
            OnRowCommand="grvTipoEquipamentoDeficiente_RowCommand">
            <Columns>
                <asp:BoundField DataField="ted_nome" 
                    HeaderText="<%$ Resources:Configuracao, TipoEquipamentoDeficiente.Busca.grvTipoEquipamentoDeficiente.Coluna1%>"
                    SortExpression="ted_nome" Visible="false" />
                <asp:TemplateField 
                    HeaderText="<%$ Resources:Configuracao,TipoEquipamentoDeficiente.Busca.grvTipoEquipamentoDeficiente.Coluna1%>">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("ted_nome") %>'
                            PostBackUrl="~/Configuracao/TipoEquipamentoDeficiente/Cadastro.aspx" CssClass="wrap600px"></asp:LinkButton>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("ted_nome") %>' CssClass="wrap600px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Excluir">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="OdsTipoEquipamentoDeficiente" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoEquipamentoDeficiente"
            SelectMethod="SelecionaTipoEquipamentoDeficiente" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoEquipamentoDeficienteBO">
        </asp:ObjectDataSource>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvTipoEquipamentoDeficiente" />
    </fieldset>
</asp:Content>
