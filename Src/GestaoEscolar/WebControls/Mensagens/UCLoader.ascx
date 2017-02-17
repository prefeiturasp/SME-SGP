<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Mensagens_UCLoader" Codebehind="UCLoader.ascx.cs" %>

<asp:UpdateProgress ID="upgProgress" runat="server" DisplayAfter="0" >
    <ProgressTemplate>
        <div class="loader">
            <asp:Image SkinID="imgLoader" runat="server" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
