<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCControleVigenciaDocentes.ascx.cs"
    Inherits="GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes" %>
<%@ Register Src="../Combos/UCComboDocente.ascx" TagName="UCComboDocente" TagPrefix="uc1" %>
<asp:UpdatePanel ID="updBotao" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="display:inline-block; float:left">
            <asp:Label ID="lblNomeDocente" runat="server" Visible="false"></asp:Label>
            <asp:TextBox ID="txtNomeDocente" SkinID="text60C" runat="server" Enabled="false" Visible="false"></asp:TextBox>
            <asp:Label ID="lblTud_id" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblposicao" runat="server" Visible="false"></asp:Label>
            <asp:ImageButton runat="server" ID="btnControleVigencia" Style="vertical-align: middle;"
                SkinID="btDetalhar" CausesValidation="false" Visible="true" ToolTip="Alterar/alocar docente" />
            <asp:ImageButton ID="btnRemover" runat="server" SkinID="btExcluir" Style="display: inline; vertical-align: middle;" OnClick="btnAdicionarRemoverDocente_Click"/>
            <asp:ImageButton ID="btnAdicionar" runat="server" SkinID="btNovo" OnClick="btnAdicionarRemoverDocente_Click" Style="display: inline; vertical-align: middle; float: right"/>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div id="divVigenciaDocentesDis" runat="server" title="Vigências dos docentes" class="divVigenciaDocentesDis hide">
    <asp:UpdatePanel ID="updVigenciaDocente" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divVigencia" runat="server" style="display: block;">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup='<%#ConfiguraValidationGroup %>' />
                <fieldset id="fdsDadosDocentes" runat="server">
                    <legend>Dados do docente</legend>
                    <uc1:UCComboDocente ID="uccDocente" runat="server" Obrigatorio="true" ValidationGroup='<%#ConfiguraValidationGroup %>' />
                    <asp:Label runat="server" ID="lblIndice" Visible="false"></asp:Label>
                    <asp:Label ID="lbltdt_id" runat="server" Visible="false"></asp:Label>
                    <div runat="server" style="display: inline;">
                        <asp:Label ID="lblVigencia" runat="server" Text="Vigência *" AssociatedControlID="txtVigenciaIni"></asp:Label>
                        <asp:TextBox ID="txtVigenciaIni" runat="server" SkinID="Data"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDataInicio" runat="server" ControlToValidate="txtVigenciaIni"
                            Display="Dynamic" ErrorMessage="Data de início de vigência é obrigatório." ValidationGroup='<%#ConfiguraValidationGroup %>'>*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="ctvDataInicioFormato" runat="server" ControlToValidate="txtVigenciaIni"
                            Display="Dynamic" ErrorMessage="Data de início de vigência não está no formato dd/mm/aaaa ou é inexistente."
                            OnServerValidate="ValidarData_ServerValidate" ValidationGroup='<%#ConfiguraValidationGroup %>'>*</asp:CustomValidator>
                        <asp:CompareValidator ID="cpvDataInicio" runat="server" ControlToValidate="txtVigenciaFim"
                            ErrorMessage="Data de fim de vigência deve ser maior ou igual à data de início de vigência."
                            ValidationGroup='<%#ConfiguraValidationGroup %>' ControlToCompare="txtVigenciaIni"
                            Operator="GreaterThanEqual" Display="Dynamic" Type="Date">*</asp:CompareValidator>
                        <asp:Label ID="lbla" runat="server" Text="à "></asp:Label>
                        <asp:TextBox ID="txtVigenciaFim" runat="server" SkinID="Data"></asp:TextBox>
                        <asp:CustomValidator ID="ctvDataFimFormato" runat="server" ControlToValidate="txtVigenciaFim"
                            Display="Dynamic" ErrorMessage="Data de fim de vigência não está no formato dd/mm/aaaa ou é inexistente."
                            OnServerValidate="ValidarData_ServerValidate" ValidationGroup='<%#ConfiguraValidationGroup %>'>*</asp:CustomValidator>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CausesValidation="true" OnClick="btnSalvar_Click"
                            ValidationGroup='<%#ConfiguraValidationGroup %>' />
                        <asp:Button ID="btnLimpar" runat="server" Text="Limpar" CausesValidation="False"
                            OnClick="btnLimpar_Click" />
                        <asp:Button ID="btnFechar" runat="server" Text="Fechar" CausesValidation="False"
                            OnClientClick="$('#divVigenciaDocentesDis').dialog('close');" />
                    </div>
                </fieldset>
            </div>
            <fieldset>
                <legend>Vigências dos docentes</legend>
                <asp:GridView ID="grvControleVigencia" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="tdt_id,coc_id,col_id,crg_id,tdt_vigenciaInicio,tdt_vigenciaFim,doc_nome,doc_id"
                    OnRowDataBound="grvControleVigencia_RowDataBound" OnRowCommand="grvControleVigencia_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="doc_nome" HeaderText="Nome docente" SortExpression="doc_nome" />
                        <asp:TemplateField HeaderText="Vigência inicial" SortExpression="vigenciaInicio">
                            <ItemTemplate>
                                <asp:Label ID="lblVigenciaInicio" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vigência final" SortExpression="vigenciaFim">
                            <ItemTemplate>
                                <asp:Label ID="lblVigenciaFim" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalhar/Alterar" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnAlterar" CausesValidation="false" CommandName="Editar"
                                    SkinID="btEditar" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" CausesValidation="false" SkinID="btExcluir" runat="server"
                                    CommandName="Deletar" />
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
