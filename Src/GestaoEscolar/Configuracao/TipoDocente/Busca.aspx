<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Configuracao_TipoDocente_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Listagem de tipos de docente</legend>
        <div>
            <div>
                <asp:Button ID="btnNovo" runat="server" Text="Incluir novo tipo de docente"
                    OnClick="btnNovo_Click" />
            </div>
            <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="gdvTipoDocente" runat="server" AutoGenerateColumns="False" DataKeyNames="tdc_id"
                DataSourceID="odsTipoDocente" AllowPaging="True" EmptyDataText="Não existem tipos de docente cadastrados."
                OnRowCommand="gdvTipoDocente_RowCommand" OnRowDataBound="gdvTipoDocente_RowDataBound">
                <Columns>

                    <asp:TemplateField HeaderText="Descrição" SortExpression="tdc_descricao">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tdc_descricao") %>'
                                PostBackUrl="~/Configuracao/TipoDocente/Cadastro.aspx"></asp:LinkButton>
                            <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("tdc_descricao") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField DataField="tdc_nome" HeaderText="Nome" />

                    <asp:BoundField DataField="tdc_posicao" HeaderText="Posição" />

                    <asp:BoundField DataField="tdc_corDestaque" HeaderText="Cor" />

                    <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
        </div>
        <div>
            <asp:ObjectDataSource ID="odsTipoDocente" runat="server"
                SelectMethod="SelecionaAtivos"
                TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoDocenteBO" OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:Parameter Name="appMinutosCacheLongo" Type="Int32" DefaultValue="0" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </fieldset>
</asp:Content>

