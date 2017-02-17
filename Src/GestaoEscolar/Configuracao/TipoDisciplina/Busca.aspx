<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoDisciplina_Busca" CodeBehind="Busca.aspx.cs" %>

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
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Configuracao, TipoDisciplina.Busca.lblLegend.Text %>"></asp:Label></legend>
        <asp:GridView ID="_dgvTipoDisciplina" runat="server" AutoGenerateColumns="False"
            DataKeyNames="tds_id,tds_ordem" DataSourceID="odsTipoDisciplina" EmptyDataText="<%$ Resources:Configuracao, TipoDisciplina.Busca._dgvTipoDisciplina.EmptyDataText %>"
            OnDataBound="_dgvTipoDisciplina_DataBound" OnRowDataBound="_dgvTipoDisciplina_RowDataBound"
            OnRowCommand="_dgvTipoDisciplina_RowCommand" >
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, TipoDisciplina.Busca._dgvTipoDisciplina.Coluna1 %>">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tds_nome") %>'
                            PostBackUrl="~/Configuracao/TipoDisciplina/Cadastro.aspx"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ordem">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tds_ordem") %>'></asp:TextBox>
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
                <asp:BoundField DataField="tne_nome" HeaderText="Tipo de nível de ensino" />
                <asp:BoundField DataField="tds_base_nome" HeaderText="Base" />
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc4:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvTipoDisciplina" />
        <asp:ObjectDataSource ID="odsTipoDisciplina" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoDisciplina"
            SelectMethod="SelecionaTipoDisciplinaNaoPaginado" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoDisciplinaBO"
            OnSelecting="odsTipoDisciplina_Selecting" SelectCountMethod="GetTotalRecords">
        </asp:ObjectDataSource>
    </fieldset>
</asp:Content>
