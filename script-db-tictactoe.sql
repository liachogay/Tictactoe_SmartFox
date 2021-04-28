use master

create database TicTacToe

go
use tictactoe
	
go
create table UserInfo(
	ID int IDENTITY(1,1) primary key ,
	UserName varchar(50) unique,
	PassAcc varchar(50),
)

create table UserMoney(
	IDUser int primary key,
	MoneyHave bigint,
	CONSTRAINT FK_ID FOREIGN KEY (IDUSER) REFERENCES USERINFO(ID)
)

go
create proc CheckPass @user varchar(50), @pass varchar(50)
as
begin
	select * from UserInfo where USERNAME = @user and PassAcc = @pass
end;

go
create proc CheckAcc @user varchar(50)
as
begin
	select * from UserInfo where USERNAME = @user 
end;

go
create proc SignUp @user varchar(50),@pass varchar(50)
as
begin
	INSERT INTO UserInfo VALUES(@user,@pass);

end;

go
create proc GetMoneyByName @name varchar(50)
as
begin
	SELECT ID,MoneyHave FROM UserMoney INNER JOIN UserInfo ON ID = IDUser AND UserName = @name
end;

go
CREATE proc SetMoneyByID  @ID int,@money bigint
as
begin
	UPDATE UserMoney
	SET MoneyHave = @money
	where IDUser = @ID
end;


go
create trigger trg_signup on userinfo after insert
as
begin
	DECLARE @ID bigint;
	SET @ID = (select ID from inserted);
	insert into UserMoney VALUES (@ID ,10000);
end

INSERT INTO UserInfo VALUES ('AAA1','BBB');
INSERT INTO UserInfo VALUES ('AAA2','BBB');
INSERT INTO UserInfo VALUES ('AAA3','BBB');
INSERT INTO UserInfo VALUES ('AAA4','BBB');
INSERT INTO UserInfo VALUES ('AAA5','BBB');
INSERT INTO UserInfo VALUES ('AAA6','BBB');
INSERT INTO UserInfo VALUES ('AAA7','BBB');
INSERT INTO UserInfo VALUES ('AAA8','BBB');


select * from UserInfo as UI inner join UserMoney as UM on UI.ID = UM.IDUser
go
select * from UserInfo

go
exec CheckPass 'AAA1','BBB'
go
exec CheckAcc 'Kiet1'

go
exec SignUp 'Kiet234','CCCC'

go
EXEC GetMoneyByName'AAA2'

EXEC CHECKACC 'AAA12342'

GO
EXEC SetMoneyByID 2, 200000

select * from UserMoney