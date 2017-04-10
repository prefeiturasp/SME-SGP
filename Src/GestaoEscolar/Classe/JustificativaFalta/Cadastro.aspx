<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="GestaoEscolar.Classe.JustificativaFalta.Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Busca/UCAluno.ascx" TagName="UCAluno" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:Label ID="lblMessageInfo" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="JustificativaFalta" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Busca de aluno -->
    <asp:Panel ID="pnlAluno" runat="server" GroupingText="Cadastro de justificativas de faltas">
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:UpdatePanel ID="updAluno" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblInformacao" runat="server" Visible="false"></asp:Label>
                <div id="divPesquisaAluno" runat="server">
                    <asp:Label ID="lblNomeAluno" runat="server" Text="Aluno *" AssociatedControlID="txtNomeAluno"
                        EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtNomeAluno" runat="server" Enabled="False" SkinID="text60C" MaxLength="200"></asp:TextBox>
                    <asp:ImageButton ID="btnBuscaAluno" runat="server" OnClick="btnBuscaAluno_Click"
                        SkinID="btPesquisar" CausesValidation="false" />
                </div>
                <br />
                <div id="divLimparPesquisa" runat="server" class="right" visible="false">
                    <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CausesValidation="false"
                        OnClick="btnVoltar_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <!-- Justificativas de faltas -->
    <asp:UpdatePanel ID="updJustificativaFalta" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlJustificativaFalta" runat="server" GroupingText="Justificativas de faltas do aluno"
                Visible="false">
                <asp:Button ID="btnNovaJustificativaFalta" runat="server" Text="Incluir nova justificativa de falta"
                    CausesValidation="false" OnClick="btnNovaJustificativaFalta_Click" />
                <asp:GridView ID="grvJustificativaFalta" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="Não existem justificativas de faltas para este aluno." DataKeyNames="alu_id,afj_id,afj_dataInicio,afj_dataFim,IsNew,afj_observacao"
                    OnDataBinding="grvJustificativaFalta_DataBinding" OnRowCancelingEdit="grvJustificativaFalta_RowCancelingEdit"
                    OnRowDataBound="grvJustificativaFalta_RowDataBound" OnRowDeleting="grvJustificativaFalta_RowDeleting"
                    OnRowEditing="grvJustificativaFalta_RowEditing" OnRowUpdating="grvJustificativaFalta_RowUpdating">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl" runat="server" Text="Justificativa da falta *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTipoJustificativaFalta" runat="server" Text='<%#Bind("tjf_nome") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlTipoJustificativaFalta" runat="server" DataValueField="tjf_id"
                                    DataTextField="tjf_nome" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoJustificativaFalta_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="-1">-- Selecione uma justificativa --</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CompareValidator ID="cpvTipoJustificativaFalta" runat="server" ErrorMessage="Justificativa da falta é obrigatório."
                                    ControlToValidate="ddlTipoJustificativaFalta" Operator="NotEqual" ValueToCompare="-1"
                                    Display="Dynamic" ValidationGroup="JustificativaFalta">*</asp:CompareValidator>
                                <asp:Label ID="lblAbonaFalta" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl" runat="server" Text="Data de início *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDataInicio" runat="server" Text='<%#Bind("afj_dataInicio", "{0:dd/MM/yyyy}") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDataInicio" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                                    ErrorMessage="Data de início é obrigatório." Display="Dynamic" ValidationGroup="JustificativaFalta">*</asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="ctvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                                    ValidationGroup="JustificativaFalta" Display="Dynamic" ErrorMessage="Data de início não está no formato dd/mm/aaaa ou é inexistente."
                                    OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data de fim">
                            <ItemTemplate>
                                <asp:Label ID="lblDataFim" runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDataFim" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                <asp:CustomValidator ID="ctvDataFim" runat="server" ControlToValidate="txtDataFim"
                                    ValidationGroup="JustificativaFalta" Display="Dynamic" ErrorMessage="Data de fim não está no formato dd/mm/aaaa ou é inexistente."
                                    OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                                <asp:CustomValidator ID="cpvData" runat="server" Display="Dynamic" ValidationGroup="JustificativaFalta"
                                    ErrorMessage="Data de fim deve ser maior ou igual a data de início." OnServerValidate="ValidarDatas_ServerValidate">*</asp:CustomValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observação">
                            <ItemTemplate>
                                <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" MaxLength="4000"></asp:TextBox>
                                <asp:Label ID="lblObservacao" runat="server" Text='<%#Bind("afj_observacao") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editar/Salvar">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                    ToolTip="Editar" CausesValidation="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="btnSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar" ValidationGroup="JustificativaFalta" />
                                <asp:ImageButton ID="btnCancelar" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                    ToolTip="Cancelar" CausesValidation="false" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                    ToolTip="Excluir" CausesValidation="False" />
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Busca de alunos -->
    <div id="divBuscaAluno" title="Busca de alunos" class="hide">
        <asp:UpdatePanel ID="updBuscaAluno" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:UCAluno ID="UCAluno1" runat="server" OnReturnValues="UCAluno1_ReturnValues" VS_DocumentoOficial="false" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscaAluno" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
