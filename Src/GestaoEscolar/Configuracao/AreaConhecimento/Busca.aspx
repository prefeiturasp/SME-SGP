<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.AreaConhecimento.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <legend><asp:Label runat="server" ID="lblLegend" Text="Listagem de área de conhecimento"></asp:Label></legend>
        <asp:Button ID="_btnNovoAreaConhecimento" runat="server" Text="Nova área de conhecimento"
            OnClick="_btnNovoAreaConhecimento_Click" />
        <asp:GridView ID="_dgvAreaConhecimento" runat="server" AutoGenerateColumns="False"
            DataKeyNames="aco_id,aco_ordem" DataSourceID="odsAreaConhecimento" EmptyDataText="Nenhuma área de conhecimento encontrada."
            OnDataBound="_dgvAreaConhecimento_DataBound" OnRowDataBound="_dgvAreaConhecimento_RowDataBound"
            OnRowCommand="_dgvAreaConhecimento_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Área de conhecimento">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("aco_nome") %>'
                            PostBackUrl="~/Configuracao/AreaConhecimento/Cadastro.aspx"></asp:LinkButton>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("aco_nome") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ordem">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("aco_ordem") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                            Height="16" Width="16" />
                        <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                            Height="16" Width="16" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-Width="70px">
                    <ItemTemplate>
                        <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc4:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvAreaConhecimento" />
        <asp:ObjectDataSource ID="odsAreaConhecimento" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_AreaConhecimento"
            SelectMethod="SelecionaAtivas" TypeName="MSTech.GestaoEscolar.BLL.ACA_AreaConhecimentoBO"
            OnSelecting="odsAreaConhecimento_Selecting" SelectCountMethod="GetTotalRecords">
        </asp:ObjectDataSource>
    </fieldset>
</asp:Content>
