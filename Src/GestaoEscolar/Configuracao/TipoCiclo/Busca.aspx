<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.TipoCiclo.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Qualidade" />
        </contenttemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updResultado" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend>Listagem de tipos de ciclo</legend>
                <div id="divResultado" runat="server">
                    <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                    <asp:GridView ID="grvTpCiclo" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        BorderStyle="None" DataKeyNames="tci_id, tci_ordem" DataSourceID="odsTpCiclo"
                        EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="grvTpCiclo_RowCommand"
                        OnRowDataBound="grvTpCiclo_RowDataBound" OnDataBound="grvTpCiclo_DataBound" AllowSorting="False" EnableModelValidation="True">
                        <Columns>
                            <asp:TemplateField HeaderText="Descrição" SortExpression="tci_nome">
                                <ItemTemplate>
                                    <asp:Label ID="btnAlterar" runat="server" Text='<%# Bind("tci_nome") %>' CssClass="wrap400px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="tci_exibirBoletim" HeaderText="Exibir compromisso do aluno no boletim"/>  
                            <asp:TemplateField HeaderText="Ordem" SortExpression="tpc_ordem">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tci_ordem") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                        Height="16" Width="16" ToolTip="Subir uma linha" />
                                    <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                        Height="16" Width="16" ToolTip="Descer uma linha" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <uc3:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvTpCiclo" />
                    <asp:ObjectDataSource ID="odsTpCiclo" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="SelecionarAtivos" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoCicloBO"
                        OnSelecting="odsTipoCiclo_Selecting"></asp:ObjectDataSource>
                </div>
            </fieldset>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
