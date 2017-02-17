<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_ParametroAcademico_Cadastro" Title="Untitled Page" CodeBehind="Cadastro.aspx.cs" %>

<%@ Register Src="../../WebControls/Combos/UCComboTipoDocumentacao.ascx" TagName="UCComboTipoDocumentacao"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- GRID PRINCIPAL PARAMETRO--%>
    <asp:UpdatePanel ID="_updMessage" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <legend>Listagem de parâmetros acadêmicos</legend>
        <asp:UpdatePanel ID="_updGridParametrosAcademicos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="_updGridParametrosAcademicos" />
                <div>
                    <asp:GridView ID="gvParametrosAcademicos" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="pac_id,pac_chave,pac_obrigatorio,pac_descricao,tipo" OnRowEditing="gvParametrosAcademicos_RowEditing"
                        OnRowDataBound="gvParametrosAcademicos_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="pac_descricao" HeaderText="Parâmetro" SortExpression="pac_descricao" />
                            <asp:BoundField DataField="pac_valor_nome" HeaderText="Valor" SortExpression="pac_valor_nome"  HeaderStyle-Width="250px"/>
                            <asp:BoundField DataField="pac_vigencia" HeaderText="Vigência" SortExpression="pac_vigencia"
                                ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Alterar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="_btnAlterar" CausesValidation="false" CommandName="Edit"
                                        SkinID="btEditar" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <fieldset>
        <legend>Cadastro de parâmetros de busca de aluno</legend>
        <asp:UpdatePanel ID="_updGridParametroBuscaAluno" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader2" runat="server" AssociatedUpdatePanelID="_updGridParametroBuscaAluno" />
                <div>
                    <asp:Button ID="_btnNovoParametroBuscaAluno" runat="server" Text="Incluir novo parâmetro de busca de aluno"
                        OnClick="_btnNovoParametroBuscaAluno_Click" />
                </div>
                <asp:GridView ID="_grvParametroBuscaAluno" runat="server" AutoGenerateColumns="False"
                    DataSourceID="odsParametroBuscaAluno" AllowPaging="True" DataKeyNames="pba_id"
                    EmptyDataText="Não existem parâmetros de busca de aluno cadastrados." OnRowDataBound="_grvParametroBuscaAluno_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="pba_tipoDescricao" HeaderText="Tipo" />
                        <asp:BoundField DataField="pba_detalhe" HeaderText="Detalhe" />
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="_btnExcluir" runat="server" CommandName="Delete" SkinID="btExcluir" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ObjectDataSource ID="odsParametroBuscaAluno" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_ParametroBuscaAluno"
            SelectMethod="GetSelect" TypeName="MSTech.GestaoEscolar.BLL.ACA_ParametroBuscaAlunoBO"
            EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
            DeleteMethod="Delete" StartRowIndexParameterName="currentPage" OnSelecting="odsParametroBuscaAluno_Selecting"
            OnDeleted="odsParametroBuscaAluno_Deleted"></asp:ObjectDataSource>
    </fieldset>
    <%--DIV FILTRO PARAMETROS--%>
    <div id="divParametroAcademico" title="Cadastro de parâmetros acadêmicos" class="hide">
        <asp:UpdatePanel ID="_updCadastroParametroAcademico" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader3" runat="server" AssociatedUpdatePanelID="_updCadastroParametroAcademico" />
                <asp:Label ID="_lblMessageInsert" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="_ValidationParametroAcademico" />
                <br />
                <fieldset>
                    <legend>Incluir parâmetro</legend>
                    <asp:Label ID="_lblNome_Par" runat="server" EnableViewState="True" Text="" AssociatedControlID="_ddlParametroAcademicoValor"></asp:Label>
                    <div id="parametroTextBox" runat="server">
                        <asp:TextBox ID="_txtValor" runat="server" SkinID="text60C"></asp:TextBox>
                    </div>
                    <div id="parametroData" runat="server" visible="false">
                        <asp:TextBox ID="txtData" runat="server" SkinID="Data"></asp:TextBox>
                    </div>
                    <div id="parametroCombo" runat="server" visible="false">
                        <asp:DropDownList ID="_ddlParametroAcademicoValor" runat="server" AppendDataBoundItems="False"
                            ValidationGroup="_ValidationParametroAcademico" CssClass="text60C">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="_cvParametroAcademicoValor" runat="server" ErrorMessage=""
                            ValidationGroup="_ValidationParametroAcademico" ControlToValidate="_ddlParametroAcademicoValor"
                            Operator="GreaterThan" ValueToCompare="0" Display="Dynamic">*</asp:CompareValidator>
                        <div id="divVigencia" runat="server" style="display: block;">
                            <div id="divVigenciaInicio" runat="server" style="display: inline;">
                                <asp:Label ID="_lblVigencia" runat="server" Text="Vigência" AssociatedControlID="_txtVigenciaIni"></asp:Label>
                                <asp:TextBox ID="_txtVigenciaIni" runat="server" MaxLength="10" Width="100px" SkinID="Data"
                                    CssClass="maskData"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvVigenciaIni" runat="server" ControlToValidate="_txtVigenciaIni"
                                    Display="Dynamic" ErrorMessage="Data de vigência inicial é obrigatório." Visible="false"
                                    ValidationGroup="_ValidationParametroAcademico">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="_revVigenciaIni" runat="server" ControlToValidate="_txtVigenciaIni"
                                    ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                                    ErrorMessage="" ValidationGroup="_ValidationParametroAcademico">*</asp:RegularExpressionValidator>
                            </div>
                            <div id="divVigenciaFim" runat="server" style="display: inline;">
                                <asp:Label ID="_lbla" runat="server" Text="à"></asp:Label>
                                <asp:TextBox ID="_txtVigenciaFim" runat="server" MaxLength="10" Width="100px" CssClass="maskData"
                                    SkinID="Data"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="_revVigenciaFim" runat="server" ControlToValidate="_txtVigenciaFim"
                                    ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                                    ErrorMessage="" ValidationGroup="_ValidationParametroAcademico">*</asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                    <div class="right">
                        <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" CausesValidation="true"
                            OnClick="_btnSalvar_Click" ValidationGroup="_ValidationParametroAcademico" />
                        <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                            OnClick="_btnCancelar_Click" />
                    </div>
                </fieldset>
                <%--DIV PESQUISA PARAMETROS--%>
                <fieldset id="fsValoresParametros" runat="server">
                    <legend>Parâmetros cadastrados</legend>
                    <asp:GridView ID="gvValoresParametrosAcademicos" runat="server" 
                        AutoGenerateColumns="False" DataKeyNames="pac_id" AllowPaging="True" OnRowEditing="gvValoresParametrosAcademicos_RowEditing"
                        OnRowCommand="gvValoresParametrosAcademicos_RowCommand" OnRowDataBound="gvValoresParametrosAcademicos_RowDataBound"
                        OnPageIndexChanging="gvValoresParametrosAcademicos_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="pac_descricao" HeaderText="Parâmetro" SortExpression="pac_descricao" />
                            <asp:BoundField DataField="pac_valor_nome" HeaderText="Valor" SortExpression="pac_valor_nome" />
                            <asp:BoundField DataField="pac_vigencia" HeaderText="Vigência" SortExpression="pac_vigencia"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Detalhar/Alterar" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="_btnAlterar" CausesValidation="false" CommandArgument='<%#Bind("pac_chave") %>'
                                        CommandName="Edit" SkinID="btEditar" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="_btnExcluir" CausesValidation="false" SkinID="btExcluir" CommandArgument='<%#Bind("pac_id") %>'
                                        runat="server" CommandName="Deletar" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--DIV CADASTRO DE PARAMETRO DE BUSCA DE ALUNO--%>
    <div id="divParametroBuscaAluno" title="Cadastro de parâmetro de busca de aluno"
        class="hide">
        <asp:UpdatePanel ID="_updCadastroParametroBuscaAluno" runat="server" UpdateMode="Conditional">
            <ContentTemplate>                
                    <uc1:UCLoader ID="UCLoader4" runat="server" AssociatedUpdatePanelID="_updCadastroParametroBuscaAluno" />
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="ParametroBuscaAluno" />
                    <asp:Label ID="_lblMessageParametroBuscaAluno" runat="server" EnableViewState="False"></asp:Label>
                    <fieldset>
                    <asp:Label ID="LabelTipoParametroBuscaAluno" runat="server" Text="Tipo *" AssociatedControlID="_ddlTipoParametroBuscaAluno"></asp:Label>
                    <asp:DropDownList ID="_ddlTipoParametroBuscaAluno" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="True" OnSelectedIndexChanged="_ddlTipoParametroBuscaAluno_SelectedIndexChanged"
                        ValidationGroup="ParametroBuscaAluno">
                        <asp:ListItem Value="-1" Text="-- Selecione um tipo --"/>
                        <asp:ListItem Value="1" Text="Tipo de documentação"/>
                        <asp:ListItem Value="2" Text="Nome"/>
                        <asp:ListItem Value="3" Text="Nome da mãe"/>
                        <asp:ListItem Value="4" Text="Nome do pai"/>
                        <asp:ListItem Value="5" Text="Data de nascimento"/>
                        <asp:ListItem Value="6" Text="<%$ Resources:Mensagens, MSG_NUMEROMATRICULA%>"></asp:ListItem>
                        <asp:ListItem Value="7" Text="Certidão de nascimento"/>
                    </asp:DropDownList>
                    <asp:CompareValidator ID="_cpvTipoParamtroBuscaAluno" runat="server" ErrorMessage="Tipo é obrigatório."
                        ControlToValidate="_ddlTipoParametroBuscaAluno" Operator="GreaterThan" ValueToCompare="0"
                        Display="Dynamic" ValidationGroup="ParametroBuscaAluno">*</asp:CompareValidator>
                    <uc2:UCComboTipoDocumentacao ID="UCComboTipoDocumentacao1" runat="server" />
                    <asp:CompareValidator ID="_cpvTipoDocumentacao" runat="server" ErrorMessage="Tipo de documentação é obrigatório."
                        ControlToValidate="UCComboTipoDocumentacao1:_ddlTipoDocumentacao" Operator="NotEqual"
                        ValueToCompare="00000000-0000-0000-0000-000000000000" Display="Dynamic" ValidationGroup="ParametroBuscaAluno">*</asp:CompareValidator>
                    <div class="right">
                        <asp:Button ID="_btnSalvarParametroBuscaAluno" runat="server" Text="Salvar" OnClick="_btnSalvarParametroBuscaAluno_Click"
                            ValidationGroup="ParametroBuscaAluno" />
                        <asp:Button ID="_btnCancelarParametroBuscaAluno" runat="server" Text="Cancelar" CausesValidation="false"
                            OnClientClick="$('#divParametroBuscaAluno').dialog('close');body.scrollTop; return false;" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
