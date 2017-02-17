<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeBehind="Cadastro.aspx.cs" Inherits="Configuracao_FaixaRelatorio_Cadastro" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<%@ Register src="~/WebControls/Combos/UCComboEscalaAvaliacao.ascx" tagname="UCComboEscalaAvaliacao" tagprefix="uc3" %>
<%@ Register src="~/WebControls/Combos/Novos/UCCEscalaAvaliacaoParecer.ascx" tagname="UCCEscalaAvaliacaoParecer" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="<%=validationGroup %>" />
    <fieldset>        
        <legend><asp:Label ID="lblRelatorio" runat="server" Text="Cadastro de faixa por relatório: "></asp:Label></legend>        
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Button ID="btnNovaFaixaNota" runat="server" CausesValidation="False" Text="Incluir nova faixa de " 
            OnClick="btnNovaFaixaNota_Click" ToolTip="Incluir nova faixa"/>
        <br /><br />
        <fieldset>
            <legend><asp:Label runat="server" ID="lblLegendaNota"></asp:Label></legend>
                <asp:GridView ID="grvFaixaNota" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="far_id, rlt_id, far_cor, far_inicio, far_fim, far_situacao, IsNew"
                    EmptyDataText="Não existem faixas por relatório cadastradas."                    
                    OnRowDataBound="grvFaixaNota_RowDataBound" 
                    OnRowEditing="grvFaixaNota_RowEditing"
                    OnRowUpdating="grvFaixaNota_RowUpdating" 
                    OnRowDeleting="grvFaixaNota_RowDeleting"
                    OnRowCancelingEdit="grvFaixaNota_RowCancelingEdit">
                    <Columns>                         
                        <%-- Descrição --%>
                        <asp:TemplateField HeaderText="Descrição *">
                             <HeaderTemplate>
                                <asp:Label ID="lblTituloDescricao" runat="server" Text="Descrição *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("far_descricao") %>' CssClass="wrap150px"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("far_descricao") %>'
                                    MaxLength="200" SkinID="text30C" CssClass="wrap200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDescricao" runat="server" ErrorMessage="Campo descrição é obrigatório."
                                    ControlToValidate="txtDescricao" ValidationGroup='<%=validationGroup %>'>*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="Left" Width="200px" />
                        </asp:TemplateField>
                        <%-- Início --%>
                        <asp:TemplateField HeaderText="Início">
                            <HeaderTemplate>
                                <asp:Label ID="lblTituloInicio" runat="server" Text="Início" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="overflow: auto; width: 150px;">
                                    <asp:Label ID="lblInicio" runat="server" Text='<%# Bind("far_inicio") %>' CssClass="wrap150px"></asp:Label>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtInicio" runat="server" Text='<%# Bind("far_inicio") %>' MaxLength="5"
                                    Rows="5" Columns="10" SkinID="Decimal"
                                    CssClass="wrap150px"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%-- Fim--%>
                        <asp:TemplateField HeaderText="Fim">
                            <HeaderTemplate>
                                <asp:Label ID="lblTituloFim" runat="server" Text="Fim" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="overflow: auto; width: 150px;">
                                    <asp:Label ID="lblFim" runat="server" Text='<%# Bind("far_fim") %>' CssClass="wrap150px"></asp:Label>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFim" runat="server" Text='<%# Bind("far_fim") %>' MaxLength="5"
                                    Rows="5" Columns="10" SkinID="Decimal"
                                    CssClass="wrap150px"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%-- Cor da faixa --%>
                        <asp:TemplateField HeaderText="Cor">
                            <HeaderTemplate>
                                <asp:Label ID="lblCorPaleta" runat="server" Text="Cor" />
                            </HeaderTemplate> 
                            <ItemTemplate>        
                                <asp:TextBox ID="txtCorPaleta" runat="server" Text='<%# Bind("far_cor") %>' MaxLength="200" 
                                    class="colorInput color {hash:true}" ReadOnly="true" Enabled="false">
                                </asp:TextBox>                
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCorPaleta" runat="server" Text='<%# Bind("far_cor") %>' MaxLength="200" 
                                    class="colorInput color {hash:true}">
                                </asp:TextBox>
                                <asp:CheckBox runat="server" ID="chkSemCor" AutoPostBack="true" Text="Sem cor específica"
                                    ToolTip="Sem cor específica o relatório irá utilizar as cores padrões definidas."
                                    TextAlign="Right" OnCheckedChanged="chkSemCor_CheckedChanged" />                                                              
                            </EditItemTemplate>
                            <HeaderStyle CssClass="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                    ToolTip="Editar" CausesValidation="false" />
                                <asp:ImageButton ID="_imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                    ToolTip="Cancelar" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar" ValidationGroup='<%=validationGroup %>'
                                    Visible="false" />
                                <asp:ImageButton ID="_imgCancelarParametro" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                    ToolTip="Cancelar" CausesValidation="false"
                                    Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                    ToolTip="Excluir" CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
        </fieldset>
        <asp:Button ID="btnNovaFaixaConceito" runat="server" CausesValidation="False" Text="Incluir nova faixa de " 
            OnClick="btnNovaFaixaConceito_Click" ToolTip="Incluir nova faixa"/>
        <br /><br />
        <fieldset>
            <legend><asp:Label runat="server" ID="lblLegendaConceito"></asp:Label></legend>
            <asp:GridView ID="grvFaixaConceito" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="far_id, rlt_id, esa_id, esa_tipo, eap_id, eap_ordem, eap_valor, far_cor, far_inicio, far_fim, far_situacao, IsNew"
                EmptyDataText="Não existem faixas por relatório cadastradas."                    
                OnRowDataBound="grvFaixaConceito_RowDataBound" 
                OnRowEditing="grvFaixaConceito_RowEditing"
                OnRowUpdating="grvFaixaConceito_RowUpdating" 
                OnRowDeleting="grvFaixaConceito_RowDeleting"
                OnRowCancelingEdit="grvFaixaConceito_RowCancelingEdit">
                <Columns>                         
                    <%-- Descrição --%>
                    <asp:TemplateField HeaderText="Descrição *">
                            <HeaderTemplate>
                            <asp:Label ID="lblTituloDescricao" runat="server" Text="Descrição *" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("far_descricao") %>' CssClass="wrap150px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <br />
                        </EditItemTemplate>
                        <HeaderStyle CssClass="Left" Width="200px" />
                    </asp:TemplateField>
                    <%-- Escala de avaliação --%>
                    <asp:TemplateField HeaderText="Escala de avaliação">
                        <HeaderTemplate>
                            <asp:Label ID="lblTituloEscala" runat="server" Text="Escala de avaliação" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="overflow: auto; width: 150px;">
                                <asp:Label ID="lblEscala" runat="server" Text='<%# Bind("esa_nome") %>' CssClass="wrap150px"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEscalaAvaliacao" runat="server" AppendDataBoundItems="True" DataTextField="esa_nome"
                                DataValueField="esa_id" OnSelectedIndexChanged="ddlEscalaAvaliacao_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="cpvEscalaAvaliacao" runat="server" ErrorMessage="Escala de avaliação é obrigatório."
                                ControlToValidate="ddlEscalaAvaliacao" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
                                ValidationGroup="<%=validationGroup %>">*</asp:CompareValidator>
                        </EditItemTemplate>
                        <HeaderStyle CssClass="center" Width="25px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <%-- Faixa--%>
                    <asp:TemplateField HeaderText="Faixa">
                        <HeaderTemplate>
                            <asp:Label ID="lblTituloEscalaParecer" runat="server" Text="Faixa" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="overflow: auto; width: 150px;">
                                <asp:Label ID="lblEscalaParecer" runat="server" Text='<%# Bind("eap_abreviatura") %>' CssClass="wrap150px"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <uc4:UCCEscalaAvaliacaoParecer ID="UCCEscalaAvaliacaoParecer1" runat="server" MostrarMessageSelecione="true" 
                                MostraTitulo="false" Obrigatorio="true" ValidationGroup='<%=validationGroup %>' />
                        </EditItemTemplate>
                        <HeaderStyle CssClass="center" Width="25px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <%-- Cor da faixa --%>
                    <asp:TemplateField HeaderText="Cor">
                        <HeaderTemplate>
                            <asp:Label ID="lblCorPaleta" runat="server" Text="Cor" />
                        </HeaderTemplate> 
                        <ItemTemplate>        
                            <asp:TextBox ID="txtCorPaleta" runat="server" Text='<%# Bind("far_cor") %>' MaxLength="200" 
                                class="colorInput color {hash:true}" ReadOnly="true" Enabled="false">
                            </asp:TextBox>                
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCorPaleta" runat="server" Text='<%# Bind("far_cor") %>' MaxLength="200"
                                 class="colorInput color {hash:true}">
                            </asp:TextBox>                        
                            <asp:CheckBox runat="server" ID="chkSemCor" AutoPostBack="true" Text="Sem cor específica"
                                ToolTip="Sem cor específica o relatório irá utilizar as cores padrões definidas."
                                TextAlign="Right" OnCheckedChanged="chkSemCor_CheckedChanged" />                                                                                                    
                        </EditItemTemplate>
                        <HeaderStyle CssClass="Left" Width="200px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:ImageButton ID="_imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                ToolTip="Editar" CausesValidation="false" />
                            <asp:ImageButton ID="_imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                ToolTip="Cancelar" CausesValidation="false" Visible="false" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" Width="25px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:ImageButton ID="_imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                ToolTip="Salvar" ValidationGroup='<%=validationGroup %>'
                                Visible="false" />
                            <asp:ImageButton ID="_imgCancelarParametro" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                ToolTip="Cancelar" CausesValidation="false"
                                Visible="false" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" Width="25px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:ImageButton ID="_imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                ToolTip="Excluir" CausesValidation="false" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" Width="25px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
        <div style="text-align: right;">
            <asp:Button ID="btnVoltar" runat="server" CausesValidation="False" Text="Voltar" OnClick="btnVoltar_Click" 
                ToolTip="Voltar para página anterior" />  
        </div>
    </fieldset>
</asp:Content>
