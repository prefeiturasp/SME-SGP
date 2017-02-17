<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAlunoEducacaoEspecial.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Busca.UCAlunoEducacaoEspecial" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>
<asp:Panel ID="pnlBuscaAluno" runat="server" DefaultButton="btnPesquisa">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="Validation" runat="server" ValidationGroup="BuscaAluno" />
    <uc1:UCComboUAEscola ID="UCComboUAEscola1" runat="server" MostrarMessageSelecioneUA="true"
        MostrarMessageSelecioneEscola="true" ObrigatorioUA="false" OnIndexChangedUA="UCComboUAEscola1_IndexChangedUA"
        ObrigatorioEscola="false" />
    <div id="divNomeAluno" runat="server">
        <div id="divEscolhaBusca" runat="server">
            <asp:Label ID="lblEscolhaBusca" runat="server" Text="Tipo de busca por nome do aluno"
                AssociatedControlID="rblEscolhaBusca"></asp:Label>
            <asp:RadioButtonList ID="rblEscolhaBusca" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Começa por" Value="2" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Contém" Value="1"></asp:ListItem>
                <asp:ListItem Text="Fonética" Value="3"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div style="float: left;">
            <asp:Label ID="lblNome" runat="server" Text="1" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
        </div>
    </div>
    <asp:Label ID="lblDataNascimento" runat="server" Text="2" AssociatedControlID="txtDataNascimento"></asp:Label>
    <asp:TextBox ID="txtDataNascimento" runat="server" MaxLength="10" SkinID="DataSemCalendario"
        ValidationGroup="BuscaAluno"></asp:TextBox>
    <asp:CustomValidator ID="cvDataNascimento" runat="server" ControlToValidate="txtDataNascimento"
        ValidationGroup="BuscaAluno" Display="Dynamic" OnServerValidate="ValidarData_ServerValidate"
        ErrorMessage="Data de nascimento não está no formato dd/mm/aaaa ou é inexistente.">* </asp:CustomValidator>
    <asp:Label ID="lblMae" runat="server" Text="6" AssociatedControlID="txtMae"></asp:Label>
    <asp:TextBox ID="txtMae" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
    <asp:Label ID="lblMatricula" runat="server" AssociatedControlID="txtMatricula"></asp:Label>
    <asp:TextBox ID="txtMatricula" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
    <div class="right">
        <asp:Button ID="btnPesquisa" runat="server" Text="Pesquisar" OnClick="btnPesquisa_Click"
            CausesValidation="true" ValidationGroup="BuscaAluno" />
        <asp:Button ID="btnVoltar" runat="server" OnClientClick="$('#divBuscaAluno').dialog('close');"
            Text="Voltar" CausesValidation="False" />
    </div>
</asp:Panel>
<br />
<fieldset id="fdsResultado" runat="server" visible="false">
    <legend>Resultados</legend>
    <asp:GridView ID="grvAluno" runat="server" AutoGenerateColumns="False" BorderStyle="None"
        DataSourceID="odsAluno" EmptyDataText="A pesquisa não encontrou resultados."
        DataKeyNames="alu_id,pes_nome,msr_id,mtu_id,alc_matricula,esc_codigo,esc_nome,cur_nome,crp_descricao,tur_codigo"
        AllowPaging="True" OnSelectedIndexChanging="grvAluno_SelectedIndexChanging" OnDataBound="grvAluno_DataBound"
        AllowSorting="true">
        <Columns>
            <asp:TemplateField HeaderText="Nome" SortExpression="pes_nome">
                <ItemTemplate>
                    <asp:LinkButton ID="lkbNomeAluno" runat="server" Text='<%# Bind("pes_nome") %>' CommandName="Select"
                        CausesValidation="false" CssClass="wrap200px"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="esc_nome" HeaderText="Escola" SortExpression="esc_nome" />
            <asp:BoundField DataField="alc_matricula" SortExpression="alc_matricula" HeaderText="<%$ Resources:Mensagens, MSG_NUMEROMATRICULA %>"/>
            <asp:BoundField DataField="cur_nome" SortExpression="cur_nome" />
            <asp:BoundField DataField="crp_descricao" SortExpression="crp_descricao" />
            <asp:BoundField DataField="tur_codigo" HeaderText="Turma" SortExpression="tur_codigo" />
            <asp:BoundField DataField="tsr_esc_nome" HeaderText="Escola"
                SortExpression="tsr_esc_nome" Visible="false" />
            <asp:BoundField DataField="tsr_turma" HeaderText="Turma"
                SortExpression="tsr_turma" Visible="false" />
        </Columns>
    </asp:GridView>
    <uc3:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvAluno" />
    <asp:ObjectDataSource ID="odsAluno" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Aluno"
        TypeName="MSTech.GestaoEscolar.BLL.ACA_AlunoBO"></asp:ObjectDataSource>
</fieldset>
