<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.ParametroDocumentoAluno.Cadastro"
    ValidateRequest="false" %>

<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="_updMessage" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="<%=validationGroup %>" />
    <fieldset>
        <legend>Parâmetros de documentos do aluno</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Button ID="_btnNovo" runat="server" CausesValidation="False" Text="Incluir novo parâmetro de documentos do aluno"
            OnClick="_btnNovo_Click" />
        <asp:UpdatePanel ID="_updParametro" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:UCLoader ID="UCLoader1" runat="server" />
                <asp:GridView ID="_grvParametros" runat="server" AutoGenerateColumns="False" DataKeyNames="pda_id,rlt_id,ent_id,pda_situacao,IsNew"
                    EmptyDataText="Não existem parâmetros de documentos do aluno cadastrados." OnDataBinding="_grvParametros_DataBinding"
                    OnRowDataBound="_grvParametros_RowDataBound" OnRowEditing="_grvParametros_RowEditing"
                    OnRowUpdating="_grvParametros_RowUpdating" OnRowDeleting="_grvParametros_RowDeleting"
                    OnRowCancelingEdit="_grvParametros_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Relatório *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                &nbsp;
                                <asp:Label ID="_lblRelatorio" runat="server" Text='<%#Bind("NomeRelatorio")%>' CssClass="wrap150px"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                &nbsp;
                                <asp:DropDownList ID="_ddlRelatorio" runat="server" SelectedValue='<%#Bind("rlt_id")%>'
                                    CssClass="wrap150px">
                                    <asp:ListItem Text="Boletim Escolar" Value="40">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Declaração de matrícula" Value="41">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Declaração de matrícula para ex-aluno" Value="42">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Declaração de pedido de transferência" Value="43">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Declaração de conclusão de curso" Value="44">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Ficha individual de alunos" Value="47">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Ficha cadastral de alunos" Value="48">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Autorização para passeio gratuito" Value="49">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Controle de recebimento da APM" Value="50">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Termo de compromisso" Value="51">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Declaração de escolaridade" Value="55">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Histórico escolar" Value="61">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Certificado de conclusão de etapa de ensino" Value="70">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:CompareValidator runat="server" ID="_cpvRelatorio" ControlToValidate="_ddlRelatorio"
                                    Operator="GreaterThan" ValueToCompare="0" ErrorMessage="Relatório é obrigatório."
                                    ValidationGroup='<%=validationGroup %>'>*
                                </asp:CompareValidator>
                            </EditItemTemplate>
                            <HeaderStyle Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Chave *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="_ddlChave" runat="server" CssClass="wrap150px">
                                </asp:DropDownList>
                                <asp:CompareValidator runat="server" ID="_cpvChave" ControlToValidate="_ddlChave"
                                    Operator="GreaterThan" ValueToCompare="0" ErrorMessage="Chave é obrigatório."
                                    ValidationGroup='<%=validationGroup %>'>*
                                </asp:CompareValidator>
                                </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                &nbsp;
                                <asp:Label ID="_lblDescricao" runat="server" Text='<%# Bind("pda_descricao") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                &nbsp; &nbsp;
                                <asp:TextBox ID="_txtDescricao" runat="server" Text='<%# Bind("pda_descricao") %>'
                                    MaxLength="200" SkinID="text15C" CssClass="wrap200px"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor *">
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Valor *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                &nbsp;
                                <div style="overflow: auto; width: 150px;">
                                    <asp:Label ID="_lblValor" runat="server" Text='<%# Bind("pda_valor") %>' CssClass="wrap150px"></asp:Label>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                &nbsp;
                                <asp:TextBox ID="_txtValor" runat="server" Text='<%# Bind("pda_valor") %>' MaxLength="1000"
                                    TextMode="MultiLine" Rows="5" Columns="10" SkinID="text30C" 
                                    CssClass="wrap150px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvValor" runat="server" ErrorMessage="Valor é obrigatório."
                                    ControlToValidate="_txtValor" ValidationGroup='<%=validationGroup %>'>*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <HeaderStyle Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                    ToolTip="Editar parâmetro de documentos do aluno" CausesValidation="false" />
                                <asp:ImageButton ID="_imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                    ToolTip="Cancelar edição" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar parâmetro de documentos do aluno" ValidationGroup='<%=validationGroup %>'
                                    Visible="false" />
                                <asp:ImageButton ID="_imgCancelarParametro" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                    ToolTip="Cancelar novo parâmetro de documentos do aluno" CausesValidation="false"
                                    Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                    ToolTip="Excluir parâmetro de documentos do aluno" CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="_btnNovo" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
