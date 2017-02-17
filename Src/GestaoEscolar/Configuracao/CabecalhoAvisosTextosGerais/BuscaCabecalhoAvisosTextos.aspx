<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="BuscaCabecalhoAvisosTextos.aspx.cs" Inherits="GestaoEscolar.Configuracao.CabecalhoAvisosTextosGerais.BuscaCabecalhoAvisosTextos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Cabecalhos" />
        </ContentTemplate>
    </asp:UpdatePanel>
            <fieldset id="fdsCabecalhos" runat="server">
                <legend>Cabeçalhos</legend>
                <asp:GridView ID="grvCabecalhos" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
                    BorderStyle="None" EmptyDataText="A pesquisa não encontrou resultados." OnRowEditing="grvCabecalhos_RowEditing"  >
                    <Columns>
                        <asp:TemplateField HeaderText="Tipo de cabeçalho" SortExpression="Cabecalhos">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" PostBackUrl="CadastroCabecalhoAvisosTextos.aspx"
                                    CausesValidation="False" Text='<%# Bind("Cabecalhos") %>' CssClass="wrap400px">
                                </asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle CssClass="thLeft" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
</asp:Content>
