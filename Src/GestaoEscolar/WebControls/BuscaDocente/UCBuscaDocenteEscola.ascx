<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCBuscaDocenteEscola.ascx.cs" Inherits="GestaoEscolar.WebControls.BuscaDocente.UCBuscaDocenteEscola" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc2" %>

<asp:Panel ID="pnlBuscaDocente" runat="server" DefaultButton="btnPesquisa">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>

    <asp:Label ID="lblNome" runat="server" Text="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.lblNome.Text %>" AssociatedControlID="txtNome"></asp:Label>
    <asp:TextBox ID="txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>

    <asp:Label ID="lblMatricula" runat="server" Text="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.lblMatricula.Text %>" AssociatedControlID="txtMatricula"></asp:Label>
    <asp:TextBox ID="txtMatricula" runat="server" MaxLength="30" SkinID="text20C"></asp:TextBox>

    <asp:Label ID="lblCPF" runat="server" Text="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.lblCPF.Text %>" AssociatedControlID="txtCPF"></asp:Label>
    <asp:TextBox ID="txtCPF" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>

    <div id="divDisciplina" runat="server" visible="false">
        <asp:Label ID="lblDisciplina" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlDisciplina"></asp:Label>
        <asp:DropDownList ID="ddlDisciplina" runat="server" AppendDataBoundItems="True" SkinID="text30C">
        </asp:DropDownList>
    </div>

    <div id="divRG" runat="server" visible="false">
        <asp:Label ID="lblRG" runat="server" Text="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.lblRG.Text %>" AssociatedControlID="txtRG"></asp:Label>
        <asp:TextBox ID="txtRG" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
    </div>

    <div class="right">
        <asp:Button ID="btnPesquisa" runat="server" Text="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.btnPesquisa.Text %>" OnClick="btnPesquisa_Click"
            CausesValidation="false" />
        <asp:Button ID="btnVoltar" runat="server" OnClientClick="$('#divBuscaDocente').dialog('close'); return false;"
            Text="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.btnVoltar.Text %>" CausesValidation="False" />
    </div>
</asp:Panel>
<br />
<fieldset id="fdsResultado" runat="server" visible="false">
    <legend>
        <asp:Literal ID="litResultado" runat="server" Text="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.litResultado.Text %>"></asp:Literal></legend>
    <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
    <asp:GridView ID="grvDocente" runat="server" AutoGenerateColumns="False" BorderStyle="None" 
        EmptyDataText="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.grvDocente.EmptyDataText %>"
        DataKeyNames="doc_id,pes_nome,col_id,crg_id,coc_id" AllowPaging="True" AllowSorting="false"
        OnSelectedIndexChanging="grvDocente_SelectedIndexChanging" OnDataBound="grvDocente_DataBound"
        OnPageIndexChanging="grvDocente_PageIndexChanged">
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.grvDocente.ColunaNome %>">
                <ItemTemplate>
                    <asp:LinkButton ID="btnAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CommandName="Select"
                        CausesValidation="false" CssClass="wrap100px"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.grvDocente.ColunaCPF %>" DataField="tipo_documentacao_cpf"  />
            <asp:BoundField HeaderText="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.grvDocente.ColunaEscola %>" DataField="esc_nome"  />
            <asp:BoundField HeaderText="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.grvDocente.ColunaMatricula %>" DataField="coc_matricula" />
            <asp:BoundField HeaderText="<%$ Resources:WebControls, BuscaDocente.UCBuscaDocenteEscola.grvDocente.ColunaCargo %>" DataField="cargofuncao" />
        </Columns>
    </asp:GridView>
    <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvDocente" />
    <asp:HiddenField ID="hdnEscola" runat="server" Value="-1" />
</fieldset>
