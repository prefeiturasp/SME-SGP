<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.NivelAprendizado.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ValidationSummary ID="vsOrientacao" runat="server" ValidationGroup="NivelAprendizado" />
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="fdsConsulta" runat="server">
        <legend>Cadastro de nível de aprendizado</legend>
        <asp:UpdatePanel ID="updNivelAprendizado" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:Button ID="btnAdicionar" runat="server" Text="Incluir novo nível de aprendizado" OnClick="btnAdicionar_Click"
                CausesValidation="false" />
                <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                <asp:GridView ID="grvNivelAprendizado" runat="server" AutoGenerateColumns="False" 
                    EmptyDataText="Não existem níveis de aprendizado cadastrados." DataKeyNames="nap_id" AllowPaging="True"
                    OnRowCommand="grvNivelAprendizado_RowCommand" OnRowDataBound="grvNivelAprendizado_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Descrição" HeaderStyle-Width="40%">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" PostBackUrl="~/Academico/NivelAprendizado/Cadastro.aspx"
                                    Text='<%# Bind("nap_descricao") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sigla">
                            <ItemTemplate>
                                <asp:Label ID="lblSigla" runat="server" Text='<%# Bind("nap_sigla") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Curso" DataField="tur_curso"/>
                        <%--Excluir--%>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                                    ToolTip="Excluir nível de aprendizado" CausesValidation="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <asp:ObjectDataSource ID="odsNivelAprendizado" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ORC_NivelAprendizado"
        SelectMethod="GetSelectNiveisAprendizadoAtivos" TypeName="MSTech.GestaoEscolar.BLL.ORC_NivelAprendizadoBO"
        OldValuesParameterFormatString="original_{0}" OnSelected="odsNivelAprendizado_Selected">
        <SelectParameters>
            <asp:Parameter Name="cur_id" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="crr_id" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="crp_id" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="appMinutosCacheLongo" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
