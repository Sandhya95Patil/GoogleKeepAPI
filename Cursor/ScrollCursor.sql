declare @Id int
declare @FirstName nvarchar(max)
declare @LastName nvarchar(max)

declare User_Cursor cursor
scroll 
for select Id, FirstName, LastName from Registrations
open User_Cursor
fetch next from User_Cursor into @Id, @FirstName, @LastName

--while @@FETCH_STATUS=0
--begin
--fetch next from User_Cursor into @Id, @FirstName, @LastName
--print 'Id: ' +convert(nvarchar(max), @Id)+ 'First name: ' +@FirstName+ 'Last name: ' +@LastName
--end

--fetch first from User_Cursor into @Id, @FirstName, @LastName
--print 'Id: ' +convert(nvarchar(max), @Id)+ ' First name: ' +@FirstName+ ' Last name: ' +@LastName

--fetch relative 1 from User_Cursor into @Id, @FirstName, @LastName
--print 'Id: ' +convert(nvarchar(max), @Id)+ ' First name: ' +@FirstName+ ' Last name: ' +@LastName

fetch absolute 1 from User_Cursor into @Id, @FirstName, @LastName
print 'Id : ' +convert(nvarchar(max), @Id)+' first name: ' +@FirstName+ ' last name: ' +@LastName

--fetch prior from User_Cursor into @Id, @FirstName, @LastName
--print 'Id: ' +convert(nvarchar(max), @Id)+ ' First name: ' +@FirstName+ ' Last name: ' +@LastName
close User_Cursor
deallocate User_Cursor
