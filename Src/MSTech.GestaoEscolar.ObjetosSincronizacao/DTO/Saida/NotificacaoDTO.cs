using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida
{
    public class NotificacaoDTO
    {
        public string SenderName { get; set; }
        public DestinatarioNotificacao Recipient { get; set; }
        public byte MessageType { get; set; }
        public string DateStartNotification { get; set; }
        public string DateEndNotification { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class DestinatarioNotificacao
    {
        public List<string> UserRecipient { get; set; }
    }
}
