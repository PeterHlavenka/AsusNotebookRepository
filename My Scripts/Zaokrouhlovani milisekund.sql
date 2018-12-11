
declare @date datetime;
select @date = '2018-06-27 16:18:30.498'


select (
case 
	when datepart(ms, @date) >= 500 

		then dateadd(ms, 1000-datepart(ms, @date), @date) 

		else dateadd(ms, -datepart(ms, @date), @date)
	end
)