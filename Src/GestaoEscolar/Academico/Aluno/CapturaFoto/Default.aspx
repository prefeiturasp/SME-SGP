<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GestaoEscolar.Academico.Aluno.CapturaFoto.Default" %>

<%@ PreviousPageType VirtualPath="~/Academico/Aluno/Busca.aspx" %>

<%@ Register Src="~/WebControls/InfoComplementarAluno/InfoComplementarAluno.ascx" TagName="InfoComplementarAluno" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language="javascript">
        function webcamCapturada(caminhoImagem) {
            $("#imgCapturada").attr('src', 'imagem.ashx?file=' + caminhoImagem);
            $("#imgCapturada").css('display', 'block');
            $("#<%=hdnArqExcluir.ClientID %>").val($("#<%=hdnArqExcluir.ClientID %>").val() + $("#<%=hdnArq.ClientID %>").val() + ';');
            $("#<%=hdnArq.ClientID %>").val(caminhoImagem)
            $("#<%=imgAntiga.ClientID %>").attr('Src', '');
            $("#<%=imgAntiga.ClientID %>").css('display', 'none');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false"></asp:Label>
    <fieldset>
        <uc1:InfoComplementarAluno ID="InfoComplementarAluno1" runat="server" EnableViewState="false" />
        <div class="right area-botoes-top">
                                                                                    
            <asp:Button ID="btnConfirmar2" runat="server" Text="Confirmar" OnClick="btnConfirmar_Click" EnableViewState="false" />
            <asp:Button ID="btnCancelar2" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" EnableViewState="false" />
        </div>
    </fieldset>
    <fieldset class="area-form form-captura">
        <legend>Captura de foto do aluno</legend>
        <div></div>
        <asp:Label ID="lblMessageFlash" runat="server" Text=""></asp:Label>
        <br />
        <div class="area-form">
            <table>
                <tr>
                    <td style="padding-right: 5px;">
                        <div style="position: relative; z-index: 1; background: #fff;" class="area-camera">
                            <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" width="400" height="300" id="flashcam">
                                <param name="movie" value="jscam.swf" />
                                <param name="wmode" value="transparent" />
                                <!--[if !IE]>-->
                                <object type="application/x-shockwave-flash" data="jscam.swf" width="400" height="300">
                                    <param name="movie" value="jscam.swf" />
                                    <param name="wmode" value="transparent" />
                                    <param name="scale" value="noscale" />
                                    <!--<![endif]-->
                                    <a href="http://www.adobe.com/go/getflash">
                                        <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash player" />
                                    </a>
                                    <!--[if !IE]>-->
                                </object>
                                <!--<![endif]-->
                            </object>
                        </div>
                    </td>
                    <td valign="top" style="padding-left: 5px;">
                        <div id="img">
                            <asp:Label ID="lblImagem" runat="server" Text="Imagem atual *" EnableViewState="false"></asp:Label>
                            <img id="imgCapturada" src="" alt="Imagem capturada" style="width: 200px; height: 265px; display: none;" /><br />
                            <img id="imgAntiga" runat="server" src="" alt="Imagem cadastrada" style="width: 200px; height: 265px; display: block;" />
                            <asp:Label ID="lblDataFoto" runat="server" Visible="false"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="upnFoto" runat="server" UpdateMode="Conditional">
                            <%--<Triggers>
                                <asp:PostBackTrigger ControlID="btnCapturaFoto" />
                            </Triggers>--%>
                            <ContentTemplate>
                                <asp:FileUpload ID="fupAnexo" runat="server" ToolTip="Procurar documento" Style="width: 350px;" />
                                <asp:HyperLink ID="hplAnexo" runat="server"></asp:HyperLink>
                                </ItemTemplate>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="right area-botoes-bottom">
            <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" OnClick="btnConfirmar_Click" EnableViewState="false" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" EnableViewState="false" />
        </div>
        <asp:HiddenField ID="hdnArq" runat="server" />
        <asp:HiddenField ID="hdnArqExcluir" runat="server" />
    </fieldset>
</asp:Content>
