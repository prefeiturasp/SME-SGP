<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs"
    Inherits="GestaoEscolar.Configuracao.MotivoInfrequencia.Cadastro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .right
        {
            text-align: right;
            margin: -15px -10px 10px -10px !important;
            padding: 10px 10px 5px 10px;
            clear: both;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="uppPrincipal" runat="server">

        <ContentTemplate>

            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>

            <asp:ValidationSummary ID="vlsArea" runat="server" ValidationGroup="Area" EnableViewState="False" />

            <fieldset id="fdsMotivosInfrequencia" runat="server">
                <legend>Motivos de infrequência</legend>
                <fieldset id="fdsNovaArea" runat="server">
                    <legend>Incluir nova área</legend>
                    <table class="table-padding">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblNome" runat="server" Text="Nome da área *" AssociatedControlID="txtNome"></asp:Label>
                                <asp:TextBox ID="txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvArea" runat="server" ControlToValidate="txtNome"
                                    ValidationGroup="Area" ErrorMessage="Nome da área é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>

                                <asp:Button ID="btnAdicionaNovaArea" runat="server"
                                    OnClick="btnAdicionaNovaArea_Click" Text="Adicionar nova área"
                                    ValidationGroup="Area" />
                            </td>
                        </tr>
                    </table>
                </fieldset>

                <div id="divAreas" runat="server">
                    <%--Monta o accordion--%>
                    <asp:Repeater ID="rptAreasInfrequencia" runat="server" OnItemDataBound="rptAreasInfrequencia_ItemDataBound"
                        OnItemCommand="rptAreasInfrequencia_ItemCommand">
                        <HeaderTemplate>
                            <div id="accordion" >
                        </HeaderTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                        <ItemTemplate>
                            <h3 id="H1">
                                <%--título do acoordion, onde será expandido--%>
                                <asp:HyperLink ID="hplLink" runat="server" ToolTip='<%#Bind("mbf_descricao") %>' 
                                    Text='<%#Bind("mbf_descricao") %>'>
                                </asp:HyperLink>
                                <asp:HiddenField ID="hdnMbf_id" runat="server" Value='<%#Bind("mbf_id") %>'/>
                            </h3>

                            <fieldset id="fdsItemArea" runat="server" style="font-size: 0.9em;">
                                <div id="divBtnExcluir" class="right">
                                     <asp:Button ID="btnExcluirArea" runat="server" Text="Excluir" CommandName="Deletar"
                                                     CausesValidation="False" />  
                                </div>
                                
                                <asp:GridView ID="gvItensArea" runat="server" AutoGenerateColumns="false"                                                                                                           DataKeyNames="mbf_id,mbf_idPai"
                                  OnRowDataBound="gvItensArea_RowDataBound" OnRowCommand="gvItensArea_RowCommand"
                                  EmptyDataText="Nenhum item foi incluído.">
                                  <Columns>
                                       <asp:BoundField DataField="mbf_sigla" HeaderText="Código do item" />
                                       <asp:BoundField DataField="mbf_descricao" HeaderText="Descrição do item" />

                                       <asp:TemplateField HeaderText="Excluir">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server"                                                                                                                 CommandName="DeletarItem" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="center"></ItemStyle>
                                       </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>

                                <fieldset id="fdsCadItemArea" runat="server" style="font-size: 1em;">
                                    <legend>Incluir novo item</legend>
                                    <table class="table-padding">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSiglaItem" runat="server" Text="Código do item *"                                                                                                                             AssociatedControlID="txtSiglaItem"></asp:Label>
                                                <asp:TextBox ID="txtSiglaItem" runat="server" MaxLength="5" SkinID="text10C">
                                                </asp:TextBox>
                                            </td>
                                         </tr>
                                         <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblDescricaoItem" runat="server" Text="Descrição do item *"                                                                                                                      AssociatedControlID="txtDescricaoItem"></asp:Label>
                                                <asp:TextBox ID="txtDescricaoItem" runat="server" MaxLength="200"                                                                                                                               SkinID="text60C"></asp:TextBox>

                                                <asp:Button ID="btnAdicionaNovoItem" runat="server"
                                                    OnClick="btnAdicionaNovoItem_Click" Text="Adicionar novo item" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>

                            </fieldset>

                        </ItemTemplate>

                    </asp:Repeater>

                    <asp:HiddenField ID="hdnAreaSelecionada" runat="server" Value="-1" />

                </div>

            </fieldset>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

