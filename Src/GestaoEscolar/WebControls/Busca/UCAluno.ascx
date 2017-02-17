<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAluno.ascx.cs" Inherits="GestaoEscolar.WebControls.Busca.UCAluno" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>
<asp:Panel ID="pnlBuscaAluno" runat="server" DefaultButton="btnPesquisa">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="vsSummary" runat="server" ValidationGroup="aluno" />
    <uc1:UCComboUAEscola ID="UCComboUAEscola1" runat="server" MostrarMessageSelecioneUA="true"
        MostrarMessageSelecioneEscola="true" ObrigatorioUA="false" OnIndexChangedUA="UCComboUAEscola1_IndexChangedUA"
        ObrigatorioEscola="false" />
    <div id="divNomeAluno" runat="server">
        <div id="_divEscolhaBusca" runat="server">
            <asp:Label ID="_lblEscolhaBusca" runat="server" Text="Tipo de busca por nome do aluno"
                AssociatedControlID="_rblEscolhaBusca"></asp:Label>
            <asp:RadioButtonList ID="_rblEscolhaBusca" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Começa por" Value="2" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Contém" Value="1"></asp:ListItem>
                <asp:ListItem Text="Fonética" Value="3"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div style="float: left;">
            <asp:Label ID="_lblNome" runat="server" Text="Nome do aluno" AssociatedControlID="_txtNome"></asp:Label>
            <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
        </div>
    </div>
    <asp:Label ID="_lblDataNascimento" runat="server" Text="Data de nascimento" AssociatedControlID="_txtDataNascimento"></asp:Label>
    <asp:TextBox ID="_txtDataNascimento" runat="server" MaxLength="10" SkinID="DataSemCalendario"></asp:TextBox>
    <asp:CustomValidator ID="cvDataNascimento" runat="server" ControlToValidate="_txtDataNascimento"
        ValidationGroup="aluno" Display="Dynamic" ErrorMessage="Data de nascimento não está no formato dd/mm/aaaa ou é inexistente." OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
    <asp:Label ID="_lblMae" runat="server" Text="Nome da mãe" AssociatedControlID="_txtMae"></asp:Label>
    <asp:TextBox ID="_txtMae" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
    <asp:Label ID="_lblMatricula" runat="server" AssociatedControlID="_txtMatricula"></asp:Label>
    <asp:TextBox ID="_txtMatricula" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
    <asp:Label ID="_lblMatrEst" runat="server" AssociatedControlID="_txtMatriculaEstadual"
        Text="Matrícula estadual" Visible="False"></asp:Label>
    <asp:TextBox ID="_txtMatriculaEstadual" runat="server" MaxLength="50" SkinID="text30C"
        Visible="False"></asp:TextBox>
    <asp:Label ID="_lblSituacao" runat="server" Text="Situação" AssociatedControlID="_ddlSituacao"></asp:Label>
    <asp:DropDownList ID="_ddlSituacao" runat="server" AppendDataBoundItems="True" SkinID="text30C">
    </asp:DropDownList>
    <div class="right">
        <asp:Button ID="btnPesquisa" runat="server" Text="Pesquisar" OnClick="btnPesquisa_Click"
            CausesValidation="true" ValidationGroup="aluno" />
        <asp:Button ID="_btnVoltar" runat="server" OnClientClick="$('#divBuscaAluno').dialog('close'); return false;"
            Text="Voltar" CausesValidation="False" />
    </div>
</asp:Panel>
<br />
<fieldset id="fdsResultado" runat="server" visible="false">
    <legend>Resultados</legend>
    <div id="divLegenda" runat="server" style="width: 230px;" visible="false">
        <b>Legenda:</b>
        <div style="border-style: solid; border-width: thin;">
            <table id="tbLegenda" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                <tr runat="server" id="lnInativos">
                    <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;">
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litInativo" Text="<%$ Resources:Mensagens, MSG_ALUNO_INATIVO %>"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <br />
    <asp:GridView ID="_grvAluno" runat="server" AutoGenerateColumns="False" BorderStyle="None"
        EmptyDataText="A pesquisa não encontrou resultados." DataKeyNames="alu_id,pes_nome,uad_id,pes_nomeMae,permissao,alu_situacaoID,pes_id"
        AllowPaging="True" OnSelectedIndexChanging="grvAluno_SelectedIndexChanging" OnDataBound="grvAluno_DataBound"
        OnRowDataBound="grvAluno_RowDataBound" AllowSorting="true" AllowCustomPaging="true" OnSorting="_grvAluno_Sorting" OnPageIndexChanging="_grvAluno_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaNome %>" SortExpression="pes_nome">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap200px"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="_btnAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CommandName="Select"
                        CausesValidation="false" CssClass="wrap100px"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="pes_dataNascimento" HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaDataNascimento %>" DataFormatString="{0:dd/MM/yyy}"
                SortExpression="pes_dataNascimento">
                <HeaderStyle CssClass="center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaNomeMae %>" SortExpression="pes_nomeMae">
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("pes_nomeMae") %>' CssClass="wrap150px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_NUMEROMATRICULA %>" SortExpression="alc_matricula" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblMatricula" runat="server" Text='<%# Bind("alc_matricula") %>' CssClass="wrap100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaMatriculaEstadual %>" SortExpression="alc_matriculaEstadual" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblMatriculaEstadual" runat="server" Text='<%# Bind("alc_matriculaEstadual") %>' CssClass="wrap100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="alu_dataCriacao" HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaDataCriacao %>" DataFormatString="{0:dd/MM/yyy}"
                SortExpression="alu_dataCriacao">
                <HeaderStyle CssClass="center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="alu_dataAlteracao" HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaDataAlteracao %>"
                DataFormatString="{0:dd/MM/yyy}" SortExpression="alu_dataAlteracao">
                <HeaderStyle CssClass="center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaEscola %>" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("esc_nome") %>' CssClass="wrap100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaCurso %>" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("cur_nome") %>' CssClass="wrap150px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaPeriodoCurso %>" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblPeríodo" runat="server" Text='<%# Bind("crp_descricao") %>' CssClass="wrap100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:WebControls, Busca.UCAluno._grvAluno.ColunaTurma %>" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblTurma" runat="server" Text='<%# Bind("tur_codigo") %>' CssClass="wrap100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <uc3:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvAluno" />
</fieldset>
