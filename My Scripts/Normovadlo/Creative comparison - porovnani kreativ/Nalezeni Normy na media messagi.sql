SELECT TOP 100 *
FROM Media.MediaMessage mm 

WHERE mm.MediaTypeId = 4 --  1 press, 2 tv,  4 audio


ORDER BY mm.AdvertisedFrom DESC