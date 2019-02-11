

-- Pricing.Queue je fronta k ceneni v cenici sluzbe. Abych nasel novejsi zorderuju si to descending
SELECT TOP 100 *
FROM pricing.Queue q ORDER BY q.Created DESC

-- Vyberu si z resultu nejakou polozku k ceneni a zmenim ji QueueStatus na 1 -new. MediaType message musi byt  Press (1) nebo Radio 
UPDATE pricing.Queue
SET 
    Pricing.Queue.QueueStatusId = 1 
WHERE pricing.Queue.Id = 72178703 