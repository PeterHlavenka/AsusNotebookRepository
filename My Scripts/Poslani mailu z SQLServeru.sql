-- posilam z Metatronu - MediaData3

declare @sended bit;

exec Common.proc_SendEmail 

@recipients='petr.hlavenka@mediaresearch.cz',
@copy_recipients = '', 
@subject = 'Chyba v proc_TvMediaMessage_SynchronizeBlockVariables', 
@htmlbody = 'nejaka zprava', 
@Sended = @sended output;   -- output je to co storovka vraci (1 odeslano, 0 neodeslano)

