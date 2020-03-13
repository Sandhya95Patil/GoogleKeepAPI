declare @Id int
declare @FirstName nvarchar(max)
declare @LastName nvarchar(max)

declare User_Cursor cursor
static for
select Id, FirstName, LastName from Registrations
open User_Cursor 
fetch next from User_Cursor into @Id, @FirstName, @LastName
while @@FETCH_STATUS=0
begin
if @Id%2=0
begin
print 'Id: ' +Convert(nvarchar(max), @Id)+ ' first name: ' +@FirstName+ ' last name: ' +@LastName
update Registrations set LastName='Sonawane' where current of User_Cursor
end
fetch from User_Cursor into @Id, @FirstName, @LastName
end
close User_Cursor
deallocate User_Cursor