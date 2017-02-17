<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAddResultadoFinal.ascx.cs" Inherits="GestaoEscolar.WebControls.HistoricoEscolar.UCAddResultadoFinal" %>

<asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
<fieldset>
    <legend>
        <asp:Label ID="lblLegendAddResFinal" Text="<%$ Resources:UserControl, UCAddResultadoFinal.lblLegendAddResFinal.Text %>" runat="server" EnableViewState="False"></asp:Label>
    </legend>
    <%--EmptyDataText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalCompCurricular.EmptyDataText %>"--%>
    <asp:GridView ID="grvResFinalCompCurricular" runat="server" AllowPaging="false" AutoGenerateColumns="false"
        BorderStyle="None" DataKeyNames="tds_id,tds_nome,grade,ahd_id"
        OnRowDataBound="grvResFinalCompCurricular_RowDataBound" OnRowCommand="grvResFinalCompCurricular_RowCommand" AllowSorting="false">
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalCompCurricular.Header.Disciplina %>">
                <ItemTemplate>
                    <asp:TextBox ID="txtNome" runat="server" MaxLength="40"></asp:TextBox>
                    <asp:Label ID="lblNome" runat="server" Text='<%# Bind("tds_nome") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalCompCurricular.Header.Nota %>">
                <ItemTemplate>
                    <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                    <asp:DropDownList ID="ddlPareceres" runat="server"></asp:DropDownList>
                </ItemTemplate>
                <HeaderStyle CssClass="center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalCompCurricular.Header.Frequencia %>">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnAhdId" runat="server" Value='<%# Bind("ahd_id") %>' />
                    <asp:TextBox ID="txtFrequencia" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                    <asp:DropDownList ID="ddlFrequencia" runat="server"></asp:DropDownList>
                </ItemTemplate>
                <HeaderStyle CssClass="center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="btnExcluir" SkinId="btExcluir" runat="server" CausesValidation="false" CommandName="Excluir"
                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="Excluir" />
                    <asp:ImageButton ID="btnAdicionar" SkinId="btNovo" runat="server" CausesValidation="false" CssClass="margin: 15px" CommandName="Adicionar"
                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="Adicionar" />                 
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <%--<asp:Button ID="btnAddDisciplina" runat="server" Text="Adicionar componente curricular" OnClick="grvResFinalCompCurricular_RowCommand" />--%>
    <%--EmptyDataText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalEnrCurricular.EmptyDataText %>"--%>
    <asp:GridView ID="grvResFinalEnrCurricular" runat="server" AllowPaging="false" AutoGenerateColumns="false"
        BorderStyle="None" DataKeyNames="tds_id,tds_nome"
        OnRowDataBound="grvResFinalEnrCurricular_RowDataBound" AllowSorting="false">
        <Columns>
            <asp:BoundField DataField="tds_nome" HeaderText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalEnrCurricular.Header.Disciplina %>" />
            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalEnrCurricular.Header.Frequencia %>">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnAhdId" runat="server" />
                    <asp:TextBox ID="txtFrequencia" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                    <asp:DropDownList ID="ddlFrequencia" runat="server"></asp:DropDownList>
                </ItemTemplate>
                <HeaderStyle CssClass="center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <%--EmptyDataText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalProjAtivCompl.EmptyDataText %>"--%>
    <asp:GridView ID="grvResFinalProjAtivCompl" runat="server" AllowPaging="false" AutoGenerateColumns="false"
        BorderStyle="None" DataKeyNames="ahp_id,ahp_nome"
        OnRowDataBound="grvResFinalProjAtivCompl_RowDataBound" AllowSorting="false">
        <Columns>
            <asp:BoundField DataField="ahp_nome" HeaderText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalProjAtivCompl.Header.Disciplina %>" />
            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalProjAtivCompl.Header.Frequencia %>">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnAhdId" runat="server" />
                    <asp:TextBox ID="txtFrequencia" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                    <asp:DropDownList ID="ddlFrequencia" runat="server"></asp:DropDownList>
                </ItemTemplate>
                <HeaderStyle CssClass="center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <%--EmptyDataText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalConclusivo.EmptyDataText %>"--%>
    <asp:GridView ID="grvResFinalConclusivo" runat="server" AllowPaging="false" AutoGenerateColumns="false"
        BorderStyle="None" DataKeyNames=""
        OnRowDataBound="grvResFinalConclusivo_RowDataBound" AllowSorting="false">
        <Columns>
            <asp:BoundField DataField="tds_nome" HeaderText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalConclusivo.Header.Disciplina %>" />
            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAddResultadoFinal.grvResFinalConclusivo.Header.Conclusivo %>">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnAhdId" runat="server" />
                    <asp:DropDownList ID="ddlParecerConclusivo" runat="server"></asp:DropDownList>
                </ItemTemplate>
                <HeaderStyle CssClass="center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>
