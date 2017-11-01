# RenomearBaseIdentity

Solution Folder
0 - Presentation
1 - Services (????)
(2) 1 - Application
(3) 2 - Domain
(4) 3 - Infra
3.1 - Data
3.2 - CrossCutting


Criar Pastas para organizar o Domain
2 - Domain 
.Domain (Clica com Direito)
Add - New Folder
Entities
Add - New Folder
Interfaces
Add - New Folder
Services
Criar Pastas para organizar as Interfaces dentro de Domain / .Domain
Interfaces (Clica com Direito)
Add - New Folder
Repositories
Add - New Folder
Services

Criar Pastas para organizar o Data
3 - Infra
3.1 - Data
.Infra.Data (Clica com Direito)
Add - New Folder
Context
Add - New Folder
Repositories
Add - New Folder
EntitiesConfig(EntityConfig )
Add - New Folder
EntityFramework


----------------------------------------------

-- ****************************************************************************************
-- fsp_sp_alterarcobrancafaturamentovencimento
-- ----------------------------------------------------------------------------------------
-- Parâmetros de Entrada:
--	@vcob_id                  INT,       --ID DA COBRANÇA
--  @vcob_dtvencimento        DATETIME,  --NOVA DATA DE VENCIMENTO
--  @vcob_observacao          TEXT,      --OBSERVACAO DO HISTORICO DA COBRANÇA
--  @vcob_fk_operador         INT,       --COLSUPORTE.DBO.PRO_USUARIO.USU_ID
--
-- Parâmetros de Saída:
--	@vcob_id_out 	       INT           OUTPUT,
--	@vcob_id_out_msg	   VARCHAR(2000) OUTPUT
--
--	Valores:
--	@vcob_id_out  <= 0 : Erro conforme descrito em @vcob_id_out_msg
--	@vcob_id_out   > 0 : Sucesso e @vcob_id_out_msg NULL
-- ----------------------------------------------------------------------------------------
-- Utilização:
-- Altera a data de vencimento da cobrança inclui o histórico da cobrança.
-- E refaz o controle das fases, apagando se necessário as anteriores.
-- Altera a data de vencimento do faturamento.
-- --------------------------------------------------------------------------------------
-- Histórico:
-- 14008. Título de Cobrança alterado por/em
-- 14010. Fases da Cobrança excluídas por/em
-- 14011. Histórico de Alteração da Cobrança incluída por/em
-- 14212. Data de Vencimento do Faturamento Gerado alterada por/em
-- 14213. Data de Vencimento do Faturamento Gerado Pagamento alterada por/em
-- ----------------------------------------------------------------------------------------
-- Autor    : 
-- Criação  : 18/04/2011
-- Alteração: 
-- DATA		    DESENVOLVEDOR	DESCRIÇÃO
-- ----------	--------------	-----------------------------------------------------------
-- ****************************************************************************************
USE COLSIP

GO
IF EXISTS (SELECT name FROM sysobjects WHERE name = 'fsp_sp_alterarcobrancafaturamentovencimento' AND type = 'P')
	DROP PROCEDURE dbo.fsp_sp_alterarcobrancafaturamentovencimento

GO
CREATE PROCEDURE dbo.fsp_sp_alterarcobrancafaturamentovencimento

	@vcob_id                  INT,
    @vcob_dtvencimento        DATETIME,
    @vcob_observacao          TEXT,
    @vcob_fk_operador         INT,
	@vcob_id_out 	          INT OUTPUT,
	@vcob_id_out_msg		  VARCHAR(2000) OUTPUT
AS

	--VARIÁVEIS DA TRANSAÇÃO
	DECLARE @transacao   INT
	DECLARE @tafspcobsip VARCHAR(100)

	--INICIALIZAÇÃO
	SET @transacao       = @@TRANCOUNT 
	SET @tafspcobsip     = NULL

	--CONTROLE DE TANSAÇÃO DO REGISTRO CONCATENA COM A DATA ATUAL H:M:S  
	IF (@transacao = 0)
	BEGIN
		SET NOCOUNT ON
		SET @tafspcobsip = 'tafspcobsip' + ISNULL(REPLACE(REPLACE(CAST(GETDATE() AS varchar(20)),' ',''),':',''),'')
		BEGIN TRANSACTION @tafspcobsip
	END

	--VARIÁVEIS DA PROCEDURE
	DECLARE @RET	           INT
	DECLARE @RETMSG            VARCHAR(2000)
	DECLARE @agora             DATETIME
	DECLARE @vcob_fk_fase      INT
	DECLARE @vcob_status       CHAR(1)
	DECLARE @aqtdcobrancas     INT
	DECLARE @apendencia        INT
	DECLARE @ptrval            BINARY(16)
	DECLARE @vffg_id           INT
	DECLARE @vfgp_id           INT

	--INICIALIZAÇÃO
	SET @vcob_id_out       = 0
	SET @vcob_id_out_msg   = 'Nada foi executado.'
	SET @RET               = 0
	SET @agora             = GETDATE()
	SET @vcob_fk_fase      = 0
	SET @aqtdcobrancas     = NULL
	SET @apendencia        = 0
	SET @vffg_id           = 0
	SET @vfgp_id           = 0

	--VARIÁVEIS DO HISTÓRICO
	DECLARE @vhst_fk_cadacaodono INT
	DECLARE @acob_fk_fase        INT
	DECLARE @acob_status         CHAR(1)
	DECLARE @acob_dtvencimento   DATETIME

	--INICIALIZAÇÃO DAS VARIÁVEIS DO HISTÓRICO
	SET @vhst_fk_cadacaodono = NULL
	SET @acob_fk_fase        = 0
	SET @acob_dtvencimento   = NULL

 	--CADASTRO DONO DA AÇÃO É O CADASTRO AMARRADO AO OPERADOR QUE EFETUOU A AÇÃO
	SELECT @vhst_fk_cadacaodono = cad_id 
	FROM colsuporte.dbo.pro_usuario
	INNER JOIN colsuporte.dbo.pro_contaacesso ON cta_id = usu_fk_cta_id
	INNER JOIN colsip.dbo.fsp_cadastro ON cad_codigo COLLATE Latin1_General_CI_AI = cta_codigo COLLATE Latin1_General_CI_AI
	WHERE usu_id = @vcob_fk_operador

	--VERIFICA SE A DATA ATUAL DA COBRANÇA É MAIOR QUE A DATA ALTERADA
	IF (@vcob_dtvencimento < @acob_dtvencimento)
	BEGIN
		SET @vcob_id_out     = -1
		SET @vcob_id_out_msg = 'A data de vencimento alterada não pode ser menor que a data de vencimento atual.'
	END

	--APAGA TODAS AS FASES > 10
	IF (@vcob_id > 0)
	BEGIN
		DELETE FROM fsp_cobranca_fase WHERE cfs_fk_cobranca = @vcob_id AND cfs_fk_fase > 10

		IF(@@ERROR <> 0)
		BEGIN
			SET @vcob_id_out     = -2
			SET @vcob_id_out_msg = 'Problemas ao excluir as fases da cobrança.'
		END
		ELSE
		BEGIN

			SET @vcob_id_out = @vcob_id

			--14010. Fases da Cobrança excluídas por/em
			EXEC fsp_sp_historico 14010, @vcob_fk_operador, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vcob_id_out, NULL, 'N', 'Fases da Cobrança excluídas por/em', @RET OUTPUT 
			-- --------------------------------------------------------------------------------------------------------------------
			--  Parâmetros de Entrada:
			--	@vhst_fk_tipohistorico	INT,
			--	@vhst_fk_operador	    INT,
			--  @vhst_fk_cadacaodono    INT,
			--  @vhst_fk_cadacaode      INT,
			--  @vhst_fk_cadacaopara    INT,
			--	@vhst_fk_dinamico	    INT,
			--	@vhst_fk_contexto	    INT,
			--	@vhst_mensagem		    CHAR(1),
			--	@vhst_motivo		    TEXT,
			--
			--  Parâmetros de Saída:
			--	@RET        		    INT OUTPUT
			
			IF(@RET <= 0)
			BEGIN
				SET @vcob_id_out     = -3
				SET @vcob_id_out_msg = 'Problemas ao incluir o histórico.'
			END
		END
	END

	--CONTROLE PARA NÃO MOSTRAR OS REGISTROS VINCULADOS NA COBRANÇA E NÃO UTILIZA-LOS

	--ATUALIZA A NOTIFICAÇÃO
	IF (@vcob_id_out > 0)
	BEGIN
		UPDATE fsp_cobranca_notificacao
			SET not_flagmostracobranca = 'N'
		WHERE
			not_fk_cobranca = @vcob_id
			AND not_flagmostracobranca = 'S'

		IF (@@ERROR <> 0)
		BEGIN
			SET @vcob_id_out     = -4
			SET @vcob_id_out_msg = 'Problemas ao alterar as notificações da cobrança.'
		END
		ELSE
		BEGIN
			SET @vcob_id_out = @vcob_id
		END
	END

	--ATUALIZA A CARTA
	IF (@vcob_id_out > 0)
	BEGIN
		UPDATE fsp_cobranca_carta
			SET car_flagmostracobranca = 'N'
		WHERE
			car_fk_cobranca = @vcob_id
			AND car_flagmostracobranca = 'S'

		IF (@@ERROR <> 0)
		BEGIN
			SET @vcob_id_out     = -5
			SET @vcob_id_out_msg = 'Problemas ao alterar as cartas de cobrança.'
		END
		ELSE
		BEGIN
			SET @vcob_id_out = @vcob_id
		END
	END

	--ATUALIZA A AUTORIZAÇÃO DE NEGATIVAÇÃO
	IF (@vcob_id_out > 0)
	BEGIN
		UPDATE fsp_cobranca_autorizacaonegativacao
			SET ang_flagmostracobranca = 'N'
		WHERE
			ang_fk_cobranca = @vcob_id
			AND ang_flagmostracobranca = 'S'

		IF (@@ERROR <> 0)
		BEGIN
			SET @vcob_id_out     = -6
			SET @vcob_id_out_msg = 'Problemas ao alterar as autorizações de negativação da cobrança.'
		END
		ELSE
		BEGIN
			SET @vcob_id_out = @vcob_id
		END
	END

	--ATUALIZA A NEGATIVAÇÃO DE CLIENTE
	IF (@vcob_id_out > 0)
	BEGIN
		UPDATE fsp_negativacaocliente
			SET ngc_flagmostracobranca = 'N'
		FROM fsp_negativacaocliente AS a
		INNER JOIN fsp_cobranca_autorizacaonegativacao AS b ON a.ngc_fk_autorizacao = b.ang_id 
			AND b.ang_flagmostracobranca = 'N'
		WHERE
			a.ngc_flagmostracobranca = 'S'

		IF (@@ERROR <> 0)
		BEGIN
			SET @vcob_id_out     = -7
			SET @vcob_id_out_msg = 'Problemas ao alterar as negativações do cliente desta cobrança.'
		END
		ELSE
		BEGIN
			SET @vcob_id_out = @vcob_id
		END
	END

	--DESBLOQUEIA
	IF (@vcob_id_out > 0)
	BEGIN
		--VERIFICA SE EXISTE APENAS UMA COBRANÇA EM ABERTO E A PENDENCIA É DO TIPO 7 E ESTA EM ABERTO
		SET @aqtdcobrancas = (SELECT COUNT(b.cob_id)
							  FROM fsp_cobranca AS a
							  INNER JOIN fsp_cobranca  AS b ON a.cob_fk_empresa = b.cob_fk_empresa
							  AND b.cob_status <> 'P'
							  INNER JOIN fsp_pendencia AS c ON c.pdn_id = b.cob_fk_pendencia
							  AND c.pdn_fk_pendenciatipo = 7 AND c.pdn_status = 'C'
							  WHERE
							  a.cob_id = @vcob_id)
	
		--VERIFICA SE EXISTE SOMENTE UMA COBRANÇA REFERENTE A PENDÊNCIA
		IF (@aqtdcobrancas = 1)
		BEGIN

			--PEGA O ID DA PENDENCIA
			SET @apendencia = (SELECT cob_fk_pendencia FROM fsp_cobranca WHERE cob_id = @vcob_id)

			IF(ISNULL(@apendencia,0) > 0)
			BEGIN

				--EXECUTAR PROCEDURE QUE DA BAIXA NA PENDÊNCIA (ENCERRA)
				EXEC fsp_sp_alterarpendencia @apendencia, 7, 'Favor transferir a ligação para a Administração.', 
				'Cobranças finalizadas.', @agora, 'P', '', @vcob_fk_operador, @RET OUTPUT
				-- ----------------------------------------------------------------------------------------
				-- Parâmetros de Entrada:
				--	@vpdn_id		        int,		   -- ID da Pendência (fsp_pendencia.pdn_id)
				--	@vpdn_fk_pendenciatipo	int,		   -- ID do Tipo de Pendência (fsp_pendencia_tipo.pet_id)
				--	@vpdn_mensagem		    varchar(2000),
				--  @vpdn_detalhe		    text,
				--	@vpdn_dtinicio		    datetime,	   -- Data de abertura da Pendência
				--	@vpdn_status		    char(1),	   -- C.aberta, P.fechada, I.cancelada
				--  @vpdn_observacao	    text,
				--  @vphs_fk_operador	    int,		   -- Operador que altera/fecha a Pendência (colsuporte.pro_usuario.usu_id)
				-- Parâmetros de Saída:
				--	@vpdn_id_out 		    int		OUTPUT

				IF (@RET <= 0)
				BEGIN
					SET @vcob_id_out     = -8
					SET @vcob_id_out_msg = 'Problemas ao encerrar a pendência da cobrança.'
				END
				ELSE
				BEGIN
					SET @vcob_id_out = @vcob_id
				END
			END
		END
	END

	--ATUALIZA O HEADER DA COBRANÇA
	IF(@vcob_id_out > 0)
	BEGIN 

		--DENTRO DO PRAZO DE PAGAMENTO FASE 10
		SET @vcob_fk_fase = 10
		SET @vcob_status  = 'C'

		--BUSCA OS DADOS ATUAIS DA COBRANÇA
		SELECT
			@acob_fk_fase      = cob_fk_fase,
			@acob_status       = cob_status,
			@acob_dtvencimento = cob_dtvencimento
		FROM fsp_cobranca 
		WHERE cob_id = @vcob_id

		--ALTERA A COBRANÇA
		UPDATE fsp_cobranca SET
			cob_fk_fase	     = @vcob_fk_fase,
			cob_status       = @vcob_status,
			cob_dtvencimento = @vcob_dtvencimento
		WHERE cob_id = @vcob_id

		IF(@@ERROR <> 0)
		BEGIN
			SET @vcob_id_out     = -9
			SET @vcob_id_out_msg = 'Problemas ao alterar o título de cobrança.'
		END
		ELSE
		BEGIN
			SET @vcob_id_out = @vcob_id

			--14008. Título de Cobrança alterado por/em
			EXEC fsp_sp_historico 14008, @vcob_fk_operador, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vcob_id_out, NULL, 'N', 'Título de Cobrança alterado por/em', @RET OUTPUT 
			-- --------------------------------------------------------------------------------------------------------------------
			--  Parâmetros de Entrada:
			--	@vhst_fk_tipohistorico	INT,
			--	@vhst_fk_operador	    INT,
			--  @vhst_fk_cadacaodono    INT,
			--  @vhst_fk_cadacaode      INT,
			--  @vhst_fk_cadacaopara    INT,
			--	@vhst_fk_dinamico	    INT,
			--	@vhst_fk_contexto	    INT,
			--	@vhst_mensagem		    CHAR(1),
			--	@vhst_motivo		    TEXT,
			--
			--  Parâmetros de Saída:
			--	@RET        		    INT OUTPUT
			
			IF(@RET <= 0)
			BEGIN
				SET @vcob_id_out     = -10
				SET @vcob_id_out_msg = 'Problemas ao incluir o histórico.'
			END
			ELSE
			BEGIN

				DECLARE @vcdh_id	INT
				DECLARE @vcha_de	VARCHAR(8000)
				DECLARE @vcha_para	VARCHAR(8000)

				SET @vcdh_id = @RET

				--FASE
				IF (ISNULL(CAST(@acob_fk_fase AS varchar(8000)),'') <> ISNULL(CAST(@vcob_fk_fase AS varchar(8000)),'') AND @vcob_id_out > 0)
				BEGIN
					SET @vcha_de   = CAST(@acob_fk_fase AS varchar(8000))
					SET @vcha_para = CAST(@vcob_fk_fase AS varchar(8000))

					EXEC fsp_sp_cadhistoricoatualizacao @vcdh_id, 'cob_fk_fase', 'Fase', @vcha_de, @vcha_para, @RET OUTPUT
					-- --------------------------------------------------------------------------------------------------------------------
					-- Parâmetros de Entrada:
					--	@vcha_fk_cadhistorico	int,		    -- fsp_cadastro_historico.cdh_id
					--	@vcha_coluna		    varchar(50),	-- nome da coluna real nas tabelas para LOG
					--	@vcha_descricao		    varchar(100),	-- nome ou descrição sobre o que é a coluna
					--	@vcha_de		        text,		    -- conteúdo antigo
					--	@vcha_para		        text,		    -- conteúdo novo
					-- Parâmetros de Saída:
					--	@vcha_id_out 		    int		OUTPUT

					IF (@RET <= 0)
					BEGIN
						SET @vcob_id_out = -11
						SET @vcob_id_out_msg    = 'Problemas ao inserir o histórico de atualização.'
					END
				END

				--STATUS
				IF (ISNULL(@acob_status,'') <> ISNULL(@vcob_status,'') AND @vcob_id_out > 0)
				BEGIN
					SET @vcha_de   = CAST(@acob_status AS varchar(8000))
					SET @vcha_para = CAST(@vcob_status AS varchar(8000))

					EXEC fsp_sp_cadhistoricoatualizacao @vcdh_id, 'cob_status', 'Status', @vcha_de, @vcha_para, @RET OUTPUT

					IF (@RET <= 0)
					BEGIN
						SET @vcob_id_out = -12
						SET @vcob_id_out_msg    = 'Problemas ao inserir o histórico de atualização.'
					END
				END

				--DATA VENCIMENTO
				IF (ISNULL(CAST(@acob_dtvencimento AS varchar(8000)),'') <> ISNULL(CAST(@vcob_dtvencimento AS varchar(8000)),'') AND @vcob_id_out > 0)
				BEGIN
					SET @vcha_de   = CAST(@acob_dtvencimento AS varchar(8000))
					SET @vcha_para = CAST(@vcob_dtvencimento AS varchar(8000))

					EXEC fsp_sp_cadhistoricoatualizacao @vcdh_id, 'cob_dtvencimento', 'Data Vencimento', @vcha_de, @vcha_para, @RET OUTPUT

					IF (@RET <= 0)
					BEGIN
						SET @vcob_id_out = -13
						SET @vcob_id_out_msg    = 'Problemas ao inserir o histórico de atualização.'
					END
				END

			END
		END
	END

	--GRAVA O REGISTRO DE HISTÓRICO DE ATUALIZAÇÃO
	IF(@vcob_id_out > 0)
	BEGIN 

		INSERT INTO fsp_cobranca_historicoalteracao
		(hal_fk_cobranca, hal_fk_fasede, hal_fk_fasepara, hal_dtvencimentode, hal_dtvencimentopara, hal_observacao, hal_opeinclusao, hal_dtinclusao)
		VALUES
		(@vcob_id, @acob_fk_fase, @vcob_fk_fase, @acob_dtvencimento, @vcob_dtvencimento, @vcob_observacao, @vcob_fk_operador, @agora)

		--14011. Histórico de Alteração da Cobrança incluída por/em
		EXEC fsp_sp_historico 14011, @vcob_fk_operador, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vcob_id_out, NULL, 'N', 'Histórico de Alteração da Cobrança incluída por/em', @RET OUTPUT 
		-- --------------------------------------------------------------------------------------------------------------------
		--  Parâmetros de Entrada:
		--	@vhst_fk_tipohistorico	INT,
		--	@vhst_fk_operador	    INT,
		--  @vhst_fk_cadacaodono    INT,
		--  @vhst_fk_cadacaode      INT,
		--  @vhst_fk_cadacaopara    INT,
		--	@vhst_fk_dinamico	    INT,
		--	@vhst_fk_contexto	    INT,
		--	@vhst_mensagem		    CHAR(1),
		--	@vhst_motivo		    TEXT,
		--
		--  Parâmetros de Saída:
		--	@RET        		    INT OUTPUT
		
		IF(@RET <= 0)
		BEGIN
			SET @vcob_id_out     = -14
			SET @vcob_id_out_msg = 'Problemas ao incluir o histórico.'
		END
		ELSE
		BEGIN
			SET @vcob_id_out = SCOPE_IDENTITY()
		END
	END

	--PONTEIRO PARA CAMPO TEXT (OBSERVAÇÃO)
	IF (@vcob_observacao IS NOT NULL AND @vcob_id_out > 0)
	BEGIN
		SELECT @ptrval = TEXTPTR(hal_observacao) 
		FROM fsp_cobranca_historicoalteracao WITH (NOLOCK)
		WHERE hal_id = @vcob_id_out

		WRITETEXT fsp_cobranca_historicoalteracao.hal_observacao @ptrval @vcob_observacao
	
		IF (@@ERROR <> 0)
		BEGIN
			SET @vcob_id_out     = -15
			SET @vcob_id_out_msg = 'Problemas ao alterar campo do tipo text (observação).'
		END
	END

	--CONTROLES FASE
	IF(@vcob_id_out > 0)
	BEGIN
		EXEC fsp_sp_gravarcontrolescobranca @vcob_id, @vcob_fk_operador, @RET OUTPUT, @RETMSG OUTPUT
		-- --------------------------------------------------------------------------------------------------------------------
		--  Parâmetros de Entrada:
		--  @vcob_fk_cobranca       INT
		--  @vcob_fk_operador       INT
		--  
		--  Parâmetros de Saída:
		--	@RET        		    INT           OUTPUT
		--	@RETMSG        		    VARCHAR(2000) OUTPUT
		
		IF(@RET <= 0)
		BEGIN
			SET @vcob_id_out     = -16
			SET @vcob_id_out_msg = 'Problemas ao executar o controle das fases da cobrança.'
		END
	END
	
	--ALTERA A DATA DE VENCIMENTO DO FATURAMENTO VINCULADO
	IF(@vcob_id_out > 0)
	BEGIN

		--BUSCA O ID DO FATURAMENTO VINCULADO
		SELECT @vffg_id = ISNULL(ffg_id,0), @vfgp_id = ISNULL(fgp_id,0) FROM fsp_cobranca AS a
		INNER JOIN fsp_fatgerado AS b ON a.cob_fk_fatgerado = b.ffg_id
		INNER JOIN fsp_fatgerado_pagamento AS c ON b.ffg_id = c.fgp_fk_fatgerado
		WHERE
		cob_id = @vcob_id
		
		IF (@vffg_id > 0)
		BEGIN
			--ALTERA A DATA DE VENCIMENTO DO FATURAMENTO GERADO (fsp_fatgerado)
			UPDATE fsp_fatgerado SET
				ffg_dtvencimento = @vcob_dtvencimento
			WHERE
				ffg_id = @vffg_id
			
			IF (@@ERROR <> 0)
			BEGIN
				SET @vcob_id_out     = -17
				SET @vcob_id_out_msg = 'Problemas ao alterar a data de vencimento do faturamento gerado.'
			END
			ELSE
			BEGIN

				--14212. Data de Vencimento do Faturamento Gerado alterada por/em
				EXEC fsp_sp_historico 14212, @vcob_fk_operador, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vffg_id, NULL, 'N', 'Data de Vencimento do Faturamento Gerado alterada por/em', @RET OUTPUT 
				-- --------------------------------------------------------------------------------------------------------------------
				--  Parâmetros de Entrada:
				--	@vhst_fk_tipohistorico	INT,
				--	@vhst_fk_operador	    INT,
				--  @vhst_fk_cadacaodono    INT,
				--  @vhst_fk_cadacaode      INT,
				--  @vhst_fk_cadacaopara    INT,
				--	@vhst_fk_dinamico	    INT,
				--	@vhst_fk_contexto	    INT,
				--	@vhst_mensagem		    CHAR(1),
				--	@vhst_motivo		    TEXT,
				--
				--  Parâmetros de Saída:
				--	@RET        		    INT OUTPUT
				
				IF(@RET <= 0)
				BEGIN
					SET @vcob_id_out     = -18
					SET @vcob_id_out_msg = 'Problemas ao incluir o histórico.'
				END
				ELSE
				BEGIN

					IF (@vfgp_id > 0)
					BEGIN			
						--ALTERA A DATA DE VENCIMENTO DO FATURAMENTO GERADO PAGAMENTO (fsp_fatgerado_pagamento)
						UPDATE fsp_fatgerado_pagamento SET
							fgp_dtvencimento = @vcob_dtvencimento
						WHERE
							fgp_id = @vfgp_id
						
						IF (@@ERROR <> 0)
						BEGIN
							SET @vcob_id_out     = -19
							SET @vcob_id_out_msg = 'Problemas ao alterar a data de vencimento do faturamento gerado pagamento.'
						END
						ELSE
						BEGIN

							--14213. Data de Vencimento do Faturamento Gerado Pagamento alterada por/em
							EXEC fsp_sp_historico 14213, @vcob_fk_operador, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vhst_fk_cadacaodono, @vfgp_id, NULL, 'N', 'Data de Vencimento do Faturamento Gerado Pagamento alterada por/em', @RET OUTPUT 
							-- --------------------------------------------------------------------------------------------------------------------
							--  Parâmetros de Entrada:
							--	@vhst_fk_tipohistorico	INT,
							--	@vhst_fk_operador	    INT,
							--  @vhst_fk_cadacaodono    INT,
							--  @vhst_fk_cadacaode      INT,
							--  @vhst_fk_cadacaopara    INT,
							--	@vhst_fk_dinamico	    INT,
							--	@vhst_fk_contexto	    INT,
							--	@vhst_mensagem		    CHAR(1),
							--	@vhst_motivo		    TEXT,
							--
							--  Parâmetros de Saída:
							--	@RET        		    INT OUTPUT
							
							IF(@RET <= 0)
							BEGIN
								SET @vcob_id_out     = -20
								SET @vcob_id_out_msg = 'Problemas ao incluir o histórico.'
							END

						END
					END
				END
			END
		END
	END

	--MENSAGEM DE ERRO
	IF(@vcob_id_out > 0)
	BEGIN
		SET @vcob_id_out_msg = NULL
	END
	BEGIN
		SET @vcob_id_out_msg = 'Mensagem nº'+CAST(@vcob_id_out AS VARCHAR(5))+'. '+@vcob_id_out_msg
	END

--RETORNA O PARÂMETRO DE SAÍDA
IF (@tafspcobsip IS NOT NULL)
BEGIN
	IF (@vcob_id_out <= 0)
	BEGIN
		ROLLBACK TRANSACTION @tafspcobsip
	END
	ELSE
	BEGIN
		COMMIT TRANSACTION @tafspcobsip
	END
	SET NOCOUNT OFF
END

SELECT @vcob_id_out
GO
