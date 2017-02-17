<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCNivelOrientacao.ascx.cs" Inherits="GestaoEscolar.WebControls.OrientacaoCurricular.UCNivelOrientacao" %>
<%@ Register Src="~/WebControls/OrientacaoCurricular/UCNivelOrientacao.ascx" TagPrefix="uc1" TagName="UCNivelOrientacao" %>

<asp:GridView ID="grvOrientacaoCurricular" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvOrientacaoCurricular_RowDataBound">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Label runat="server" Text="lblOriencaoCurricular"></asp:Label>
                <uc1:UCNivelOrientacao runat="server" ID="UCNivelOrientacaoFilho" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
            <ItemTemplate>
                <asp:ImageButton ID="imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                    ToolTip="Editar orientação curricular" CausesValidation="false" />
                <asp:ImageButton ID="imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                    ToolTip="Cancelar edição" CausesValidation="false" Visible="false" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
            <ItemTemplate>
                <asp:ImageButton ID="imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                    ToolTip="Salvar orientação curricular"  Visible="false" />
                <asp:ImageButton ID="imgCancelarOrientacao" runat="server" CommandName="Cancel"
                    SkinID="btCancelar" ToolTip="Cancelar nova orientação curricular" CausesValidation="false"
                    Visible="false" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
            <ItemTemplate>
                <asp:ImageButton ID="imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                    ToolTip="Excluir orientação curricular" CausesValidation="false" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>
