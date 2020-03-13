declare @Id int 
declare @FirstName nvarchar(max)
declare @LastName nvarchar(max)
declare @Profile nvarchar(max)

declare User_Cursor cursor
keyset scroll
for 
select Id, FirstName, LastName, Profile from Registrations order by FirstName
open User_Cursor
if @@CURSOR_ROWS > 0
begin
fetch next from User_Cursor into @Id, @FirstName, @LastName, @Profile
while @@FETCH_STATUS=0
begin
if @Id%2=0
update Registrations set LastName='Patil' where current of User_Cursor
fetch next from User_Cursor into @Id, @FirstName, @LastName, @Profile
end
end
close User_Cursor
deallocate User_Cursor
select * from Registrations

