
--m_intervalRemarkDao.SelectByParams

exec sp_executesql N'SELECT
	[x].[MediumId],
	[x].[IntervalRemarkTypeId],
	[x].[From] as [From1],
	[x].[To] as [To1],
	[x].[Created],
	[x].[Modified],
	[x].[CreatedBy],
	[x].[ModifiedBy],
	[x].[Id]
FROM
	[SimLog].[IntervalRemark] [x]
WHERE
	[x].[MediumId] = @p1 AND [x].[From] >= @dateTimeFrom AND
	[x].[To] <= @dateTimeTo
',N'@p1 int,@dateTimeFrom datetime,@dateTimeTo datetime',@p1=491,@dateTimeFrom='2018-07-20 08:59:55.600',@dateTimeTo='2018-07-20 10:15:25.600'