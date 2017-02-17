<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Busca_UCPessoasAluno"
    CodeBehind="UCPessoasAluno.ascx.cs" %>
<asp:UpdatePanel ID="_uppMessage" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>
<fieldset>
    <asp:Panel ID="pnlPesquisar" runat="server">
        <asp:Label ID="Label1" runat="server" Text="Nome" AssociatedControlID="_txtNome"></asp:Label>
        <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
        <asp:Label ID="_lblCPF" runat="server" Text="CPF" AssociatedControlID="_txtCPF"></asp:Label>
        <asp:TextBox ID="_txtCPF" runat="server" MaxLength="50" SkinID="text15C"></asp:TextBox>
        <asp:Label ID="_lblRG" runat="server" Text="RG" AssociatedControlID="_txtRG"></asp:Label>
        <asp:TextBox ID="_txtRG" runat="server" MaxLength="50" SkinID="text15C"></asp:TextBox>
        <asp:Label ID="_lblNIS" runat="server" Text="NIS" AssociatedControlID="_txtNIS"></asp:Label>
        <asp:TextBox ID="_txtNIS" runat="server" MaxLength="11" SkinID="Numerico" Width="120px"></asp:TextBox>
        <asp:CheckBox ID="chkBuscaAlunos" runat="server" Text="Incluir alunos na busca" class="left" />
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click"
                CausesValidation="False" />
            <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClientClick="$('#divBuscaResponsavel').dialog('close');" />
        </div>
    </asp:Panel>
</fieldset>
<fieldset id="fdsResultados" runat="server" visible="false">
    <legend>Resultados</legend>
    <asp:GridView ID="_dgvPessoas" runat="server" EmptyDataText="A pesquisa não encontrou resultados."
        AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="pes_id,pes_nome"
        EnableViewState="false" DataSourceID="odsPessoas" OnSelectedIndexChanging="_dgvPessoas_SelectedIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="Nome" SortExpression="pes_nome">
                <ItemTemplate>
                    <asp:LinkButton ID="_lkbSelect" runat="server" Text='<%# Bind("pes_nome") %>' CausesValidation="False"
                        CommandName="Select" CssClass="wrap100px"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="pes_dataNascimento" DataFormatString="{0:dd/MM/yyyy}"
                HeaderText="Data nasc." />
            <asp:TemplateField HeaderText="CPF">
                <ItemTemplate>
                    <asp:Label ID="_lblCPF" runat="server" Text='<%# Bind("TIPO_DOCUMENTACAO_CPF") %>'
                        CssClass="wrap100px" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="RG">
                <ItemTemplate>
                    <asp:Label ID="_lblRG" runat="server" Text='<%# Bind("TIPO_DOCUMENTACAO_RG") %>'
                        CssClass="wrap100px" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="NIS">
                <ItemTemplate>
                    <asp:Label ID="_lblNIS" runat="server" Text='<%# Bind("TIPO_DOCUMENTACAO_NIS") %>'
                        CssClass="wrap100px" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsPessoas" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Aluno"
        EnablePaging="True" MaximumRowsParameterName="pageSize" OnSelecting="odsPessoas_Selecting"
        SelectCountMethod="GetTotalRecords" SelectMethod="BuscaPessoas" StartRowIndexParameterName="currentPage"
        TypeName="MSTech.GestaoEscolar.BLL.ACA_AlunoBO"></asp:ObjectDataSource>
</fieldset>
