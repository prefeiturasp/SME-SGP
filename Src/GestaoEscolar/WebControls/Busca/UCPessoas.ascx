<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPessoas.ascx.cs" Inherits="MSTech.CoreSSO.UserControlLibrary.Buscas.UCPessoas" %>
<fieldset>
    <asp:Label ID="Label1" runat="server" Text="Nome" AssociatedControlID="_txtNome"></asp:Label>
    <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
    <asp:Label ID="_lblCPF" runat="server" Text="Label" AssociatedControlID="_txtCPF"></asp:Label>
    <asp:TextBox ID="_txtCPF" runat="server" MaxLength="50" SkinID="text15C"></asp:TextBox>
    <asp:Label ID="_lblRG" runat="server" Text="Label" AssociatedControlID="_txtRG"></asp:Label>
    <asp:TextBox ID="_txtRG" runat="server" MaxLength="50" SkinID="text15C"></asp:TextBox>
    <div class="right">
        <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click"
            CausesValidation="False" />
        <asp:Button ID="_btnVoltar" runat="server" OnClientClick="$('#divBuscaPessoa').dialog('close');"
            Text="Voltar" CausesValidation="False" />
    </div>
</fieldset>
<fieldset id="fdsResultados" runat="server">
    <legend>Resultados</legend>
    <asp:GridView ID="_dgvPessoas" runat="server" EmptyDataText="A pesquisa não encontrou resultados."
        AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="pes_id,pes_nome"
        DataSourceID="odsPessoas" OnRowEditing="_dgvPessoas_RowEditing">
        <Columns>
            <asp:BoundField DataField="pes_id" HeaderText="pes_id">
                <HeaderStyle CssClass="hide" />
                <ItemStyle CssClass="hide" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Nome">
                <ItemTemplate>
                    <asp:LinkButton ID="_lkbSelect" runat="server" Text='<%# Bind("pes_nome") %>' CausesValidation="False"
                        CommandName="Edit" CssClass="wrap150px" ></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="pes_dataNascimento" DataFormatString="{0:dd/MM/yyyy}"
                HeaderText="Data nasc." />
            <asp:BoundField DataField="TIPO_DOCUMENTACAO_CPF" HeaderText="CPF" />
            <asp:BoundField DataField="TIPO_DOCUMENTACAO_RG" HeaderText="RG" />
        </Columns>
    </asp:GridView>
</fieldset>
<asp:ObjectDataSource ID="odsPessoas" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.PES_Pessoa"
    EnablePaging="True" MaximumRowsParameterName="pageSize" OnSelecting="odsPessoas_Selecting"
    SelectCountMethod="GetTotalRecords" SelectMethod="GetSelect" StartRowIndexParameterName="currentPage"
    TypeName="MSTech.CoreSSO.BLL.PES_PessoaBO"></asp:ObjectDataSource>
