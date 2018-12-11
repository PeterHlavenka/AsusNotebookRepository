declare @EmailBody nvarchar(max), @EmailBody2 nvarchar(max);
DECLARE @singleQuote CHAR 
SET @singleQuote =  CHAR(39)

            select @EmailBody = 'For these MediaMessageId'+ @singleQuote +'s , footage is not filled:<BR><BR>';					
            select @EmailBody = @EmailBody + '<TABLE BORDER="1" cellspacing="0" cellpadding="4"><TR><TH>MessageId</TH></TR>';


			declare @isFootageNull bit
			DECLARE @advertisedFrom datetime, @advertisedTo datetime
			SELECT @advertisedFrom = '20180712 06:00'
			SELECT @advertisedTo = '20180712 08:00'


			select top 1 1
				from Media.MediaMessage mm
					join Media.TvMediaMessage tmm on mm.Id = tmm.Id
				where mm.AdvertisedFrom >= @advertisedFrom and mm.AdvertisedFrom < @advertisedTo and mm.CodingPlausibilityId not in (0, 4, 16)
					and mm.MediaTypeId = 2 -- optimalizace
					and [tmm].[Footage] is null
				OPTION (OPTIMIZE FOR (@AdvertisedFrom = '22220101'))
			select @isFootageNull = @@rowcount;

			if (@isFootageNull = 1)
			begin															
					;WITH MessageCTE AS 
					(
						SELECT mm.Id
						FROM Media.MediaMessage mm 
						join Media.TvMediaMessage tmm on mm.Id = tmm.Id
						where mm.AdvertisedFrom >= @advertisedFrom and mm.AdvertisedFrom < @advertisedTo and mm.CodingPlausibilityId not in (0, 4, 16)
						and mm.MediaTypeId = 2 -- optimalizace
					    and [tmm].[Footage] is null				        
					)
					SELECT @EmailBody2 = Coalesce(@EmailBody2 + '</TR><TR><TD>', '<TR><TD>')					
					+ cast(mc.Id as varchar(max)) + '</TD>' 
					FROM MessageCTE mc
                    					

                    select @EmailBody = @EmailBody + @EmailBody2 + '</TR></TABLE>'
		               
                    print @EmailBody
					
                    declare @sended bit;
                    exec Common.proc_SendEmail @recipients='petr.hlavenka@mediaresearch.cz', @copy_recipients = '', @subject = 'Footage not filled out', @htmlbody = @emailbody, @Sended = @sended output;
			end;