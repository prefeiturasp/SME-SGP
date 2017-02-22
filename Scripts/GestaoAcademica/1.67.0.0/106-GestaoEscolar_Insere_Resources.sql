USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	/***************
		Copiar do exemplo abaixo.
	****************

        -- Insere resources. 
        EXEC MS_InsereResource 
            @rcr_chave = 'Relatorios.UCRelatorios.lblMessageLayout.MsgAviso' 
            , @rcr_NomeResource = 'WebControls'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'A visualização do texto na tela abaixo não corresponde, necessariamente, ao formato no qual ele será impresso. Este formato segue as normas estabelecidas pela Secretaria Municipal de Educação.'

	*/

	EXEC MS_InsereResource 
            @rcr_chave = 'UCAlunoEfetivacaoObservacaoGeral.lblFaltasExternas.Text' 
            , @rcr_NomeResource = 'UserControl'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Falta outra rede'


	EXEC MS_InsereResource 
            @rcr_chave = 'EfetivacaoNotas.UCEfetivacaoNotas.btnFaltasExternas.ToolTip'
            , @rcr_NomeResource = 'UserControl'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Exibir ausências de outras redes'

	EXEC MS_InsereResource 
            @rcr_chave = 'EfetivacaoNotas.UCEfetivacaoNotas.divFrequenciaExterna.title'
            , @rcr_NomeResource = 'UserControl'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Ausências de outras redes'

			

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



