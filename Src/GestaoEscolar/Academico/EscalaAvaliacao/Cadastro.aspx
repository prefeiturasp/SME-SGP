<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_EscalaAvaliacao_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/EscalaAvaliacao/Busca.aspx" %>
<%@ Register Src="~/WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="_UCFiltroEscolas"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc3" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--Para chamar o Summary dentro do UpdatePanel e na GridView--%>

    <script type="text/javascript" language="javascript">
        function PerformValidation() {
            ValidationSummaryOnSubmit(ValidationGroup = "ValidarGrid");
        }
    </script>

    <asp:UpdatePanel ID="_uppCadastro" runat="server">
        <ContentTemplate>
            <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="_uppCadastro" />
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <fieldset>
                <legend>Cadastro de escala de avaliação</legend>
                <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
                <asp:Label ID="_lblTipo" runat="server" Text="Tipo da escala *" AssociatedControlID="_ddlTipo"></asp:Label>
                <asp:DropDownList ID="_ddlTipo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="_ddlTipo_SelectedIndexChanged">
                    <asp:ListItem Value="-1" Text="-- Selecione o Tipo --"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Numérico"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Pareceres"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Relatórios"></asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ID="_cvTipo" runat="server" ErrorMessage="Tipo da escala é obrigatório."
                    ControlToValidate="_ddlTipo" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic">*</asp:CompareValidator>
                <asp:Label ID="_lblNome" runat="server" Text="Nome da escala de avaliação *" AssociatedControlID="_txtNome"></asp:Label>
                <asp:TextBox ID="_txtNome" runat="server" SkinID="text60C" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="_rfvNome" runat="server" ErrorMessage="Nome da escala de avaliação é obrigatório."
                    ControlToValidate="_txtNome">*</asp:RequiredFieldValidator>
                <asp:CheckBox ID="_ckbBloqueado" Text="Bloqueado" runat="server" />
                <asp:Label ID="_lblMenorValor" runat="server" Text="Menor valor *" Visible="false"
                    AssociatedControlID="_txtMenorValor"></asp:Label>
                <asp:TextBox ID="_txtMenorValor" runat="server" Visible="false" MaxLength="5" SkinID="Decimal"></asp:TextBox>
                <asp:RequiredFieldValidator ID="_rfvMenorValor" runat="server" ErrorMessage="Menor valor é obrigatório."
                    ControlToValidate="_txtMenorValor" Visible="false">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvMenorValor" runat="server" ErrorMessage="Menor valor inválido."
                    ControlToValidate="_txtMenorValor" Operator="GreaterThanEqual" ValueToCompare="0"
                    Display="Dynamic">*</asp:CompareValidator>
                <asp:Label ID="_lblMaiorValor" runat="server" Text="Maior valor *" Visible="false"
                    AssociatedControlID="_txtMaiorValor"></asp:Label>
                <asp:TextBox ID="_txtMaiorValor" runat="server" Visible="false" MaxLength="5" SkinID="Decimal"></asp:TextBox>
                <asp:RequiredFieldValidator ID="_rfvMaiorValor" runat="server" ErrorMessage="Maior valor é obrigatório."
                    ControlToValidate="_txtMaiorValor" Visible="false">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvMaiorValor" runat="server" ErrorMessage="Maior valor inválido."
                    ControlToValidate="_txtMaiorValor" Operator="GreaterThan" ValueToCompare="0"
                    Display="Dynamic">*</asp:CompareValidator>
                <asp:Label ID="_lblVariacao" runat="server" Text="Variação *" Visible="false" AssociatedControlID="_txtVariacao"></asp:Label>
                <asp:TextBox ID="_txtVariacao" runat="server" Visible="false" SkinID="Decimal" MaxLength="5"></asp:TextBox>
                <asp:RequiredFieldValidator ID="_rfvVariacao" runat="server" ErrorMessage="Variação é obrigatório."
                    ControlToValidate="_txtVariacao" Visible="false">*</asp:RequiredFieldValidator>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="_uppGridParecer" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:UCLoader ID="UCLoader2" runat="server" AssociatedUpdatePanelID="_uppGridParecer" />
            <asp:Label ID="_lblMessage2" runat="server" EnableViewState="False"></asp:Label>
            <fieldset id="_fieldParecer" runat="server" visible="false">
                <legend>
                    <asp:Label ID="lbl" runat="server" Text="Cadastro de parecer *" />
                </legend>
                <asp:Label ID="lblMsgParecer" runat="server" Text="Realize o cadastro no sentido do parecer de menor valor para o parecer de maior valor."></asp:Label>
                <asp:GridView ID="_dgvParecer" runat="server" AutoGenerateColumns="False" OnRowCommand="_dgvParecer_RowCommand"
                    OnRowDataBound="_dgvParecer_RowDataBound" EmptyDataText="Não existem pareceres cadastrados.">
                    <Columns>
                        <asp:TemplateField HeaderText="eap_id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEap_id" runat="server" Text='<%#Bind("eap_id") %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl" runat="server" Text="Valor *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <div style="float: left">
                                        <asp:TextBox ID="txtValor" runat="server" Text='<%# Bind("eap_Valor") %>' CssClass="wrap150px"
                                            ValidationGroup="ValidarGrid" />
                                    </div>
                                    <div style="float: left">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvValor" Text="&nbsp*" ErrorMessage="Valor é obrigatório."
                                            Display="Dynamic" ControlToValidate="txtDescricao" ValidationGroup="ValidarGrid" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl" runat="server" Text="Descrição *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <div style="float: left">
                                        <asp:TextBox ID="txtDescricao" runat="server" CausesValidation="False" Text='<%# Bind("eap_descricao") %>'
                                            CommandName="Alterar" CssClass="wrap150px" ValidationGroup="ValidarGrid" />
                                    </div>
                                    <div style="float: left">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDescricao" Text="&nbsp*" ErrorMessage="Descrição é obrigatório."
                                            Display="Dynamic" ControlToValidate="txtDescricao" ValidationGroup="ValidarGrid" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Abreviatura">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAbreviatura" runat="server" CausesValidation="False" Text='<%# Bind("eap_abreviatura") %>'
                                    CommandName="Alterar" CssClass="wrap150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Equivalência inicial">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="eap_equivalenteInicio" SkinID="Numerico" Text='<%#Bind("eap_equivalenteInicio")%>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Equivalência final">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="eap_equivalenteFim" SkinID="Numerico" Text='<%#Bind("eap_equivalenteFim")%>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ordem">
                            <ItemTemplate>
                                <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                    Width="16" Height="16" />
                                <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                    Width="16" Height="16" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <div>
                                    <div style="float: left">
                                        <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir"
                                            CausesValidation="False" Visible="false" />
                                    </div>
                                    <div style="float: left">
                                        <asp:ImageButton ID="ibtnAdd" SkinID="btNovo" runat="server" CommandName="Adicionar"
                                            CausesValidation="False" Visible="true" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <fieldset>
                <div class="right">
                    <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                        OnClick="_btnCancelar_Click" /></div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
