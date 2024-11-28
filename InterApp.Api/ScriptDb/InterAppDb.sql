CREATE DATABASE InterAppDB
GO
USE InterAppDB
GO

if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TypeDocuments')
begin
CREATE TABLE TypeDocuments(
	Id int IDENTITY(1,1) NOT NULL,
	Code nvarchar(10) NULL,
	Name nvarchar(50) NULL,
	CreateDate smalldatetime NULL,
	Status bit NULL
	PRIMARY KEY (Id)
	)
end
go

--TABLA PARA EL REGISTRO DE USUARIOS
if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
begin
CREATE TABLE Users(
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(100),
	Email nvarchar(150),
	Document nvarchar(20),
	TypeUser int,
	Password nvarchar(100),
	CreateDate smalldatetime NULL,
	Status bit NULL
	PRIMARY KEY (Id)
	)
end
go


--TABLA PARA EL REGISTRO DE ESTUDIANTES
if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Students')
begin
CREATE TABLE Students(
	Id INT IDENTITY(1,1) NOT NULL,
	TypeDocument int FOREIGN KEY REFERENCES TypeDocuments(Id),
	Document nvarchar(20) NULL,
	Names nvarchar(100) NULL,
	LastName nvarchar(100) NULL,
	Email nvarchar(150) NULL,
	BirtDate smalldatetime NULL,
	CreateDate smalldatetime NULL,
	Gender char(2),
	Status bit NULL
	PRIMARY KEY (Id)
	)
end
go

--TABLA PARA EL REGISTRO DE PROFESORES
if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Professor')
begin
CREATE TABLE Professor(
	Id INT IDENTITY(1,1) NOT NULL,
	TypeDocument int FOREIGN KEY REFERENCES TypeDocuments(Id),
	Document nvarchar(20) NULL,
	Names nvarchar(100) NULL,
	LastName nvarchar(100) NULL,
	Email nvarchar(150) NULL,
	BirtDate smalldatetime NULL,
	CreateDate smalldatetime NULL,
	Gender char(2),
	Status bit NULL
	PRIMARY KEY (Id)
	)
end
go

--TABLA PARA EL REGISTRO DE MATERIAS
if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Subjects')
begin
CREATE TABLE Subjects(
	Id INT IDENTITY(1,1) NOT NULL,
	Name nvarchar(100) NULL,
	Credits int NULL,
	CreateDate smalldatetime NULL,
	CreatedBy INT FOREIGN KEY REFERENCES Users(Id),
	Status bit NULL
	PRIMARY KEY (Id)
	)
end
go


--TABLA PARA ASOCIAR UN PROFESOR A UNA MATERIA
if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProfessorSubject')
begin
CREATE TABLE ProfessorSubject(
	Id INT IDENTITY(1,1) NOT NULL,
	SubjectId INT FOREIGN KEY REFERENCES Subjects(Id),
	ProfessorId INT FOREIGN KEY REFERENCES Professor(Id),
	CreatedBy INT FOREIGN KEY REFERENCES Users(Id),
	CreateDate smalldatetime NULL,
	Status bit NULL
	PRIMARY KEY (Id)
	)
end
go


--TABLA PARA EL REGISTRO DE INCRIPCIONES DE MATERIAS A UN ESTUDIANTE
if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Registrations')
begin
CREATE TABLE Registrations(
	Id INT IDENTITY(1,1) NOT NULL,
	ProfessorId INT FOREIGN KEY REFERENCES Professor(Id),
	SubjectId INT FOREIGN KEY REFERENCES Subjects(Id),
	StudentId INT FOREIGN KEY REFERENCES Students(Id),
	CreateDate smalldatetime NULL,
	Status bit NULL
	PRIMARY KEY (Id)
	)
end
go

--SELECT 
CREATE OR ALTER PROC dbo.SP_GET_TYPESDOCUMENTS
	@Id int = 0,
	@Code nvarchar(10) = NULL ,
	@Name nvarchar(50) = NULL 
AS
BEGIN
	SELECT tp.*
	FROM TypeDocuments tp
	WHERE (tp.Id = CASE WHEN @Id = 0 THEN tp.Id else @Id end) 
	AND (tp.Name  LIKE '%' + @Name + '%' OR COALESCE(@Name, '') = '') 
	AND (tp.Code = @Code OR COALESCE(@Code, '') = '')
	AND tp.Status = 1
END
go

CREATE OR ALTER PROC dbo.SP_GET_STUDENTS
	@Id int,
	@Document nvarchar(20) = NULL ,
	@Names nvarchar(20) = NULL,
	@Status bit = NULL 
AS
BEGIN
	SELECT s.*,tp.Name  TypeDocumentName
	FROM Students s
	INNER JOIN TypeDocuments tp on s.TypeDocument =tp.Id
	WHERE (s.Id = @Id OR COALESCE(@Id, 0) = 0) 
	AND (s.Names  LIKE '%' + @Names + '%' OR COALESCE(@Names, '') = '') 
	AND (s.Document = @Document OR COALESCE(@Document, '') = '')
	AND coalesce(s.Status,0) = case when coalesce(@Status,0) =0 then  coalesce(s.Status,0)  else @Status end
END
go

CREATE OR ALTER PROC dbo.SP_GET_PROFESSOR
	@Id INT =0,
	@Document nvarchar(20) = NULL ,
	@Names nvarchar(20) = NULL ,
	@Status bit = NULL 
AS
BEGIN
	SELECT p.*,tp.Name TypeDocumentName
	FROM Professor p
	INNER JOIN TypeDocuments tp on p.TypeDocument =tp.Id
	WHERE (p.Id = @Id OR COALESCE(@Id, 0) = 0) 
	AND (p.Names  LIKE '%' + @Names + '%' OR COALESCE(@Names, '') = '') 
	AND (p.Document = @Document OR COALESCE(@Document, '') = '')
		AND coalesce(p.Status,0) = case when coalesce(@Status,0) =0 then  coalesce(p.Status,0)  else @Status end
END
go

CREATE OR ALTER PROC dbo.SP_GET_SUBJECTS
	 @Id INT =0,
	 @Name nvarchar(20) = NULL,
	 @Status bit = NULL 
AS
BEGIN
	SELECT s.*
	FROM Subjects s
	WHERE (s.Id = @Id OR COALESCE(@Id, 0) =0) 
	AND (s.Name  LIKE '%' + @Name + '%' OR COALESCE(@Name, '') = '') 
	AND coalesce(s.Status,0) = case when coalesce(@Status,0) =0 then  coalesce(s.Status,0)  else @Status end
END
go

CREATE OR ALTER PROC dbo.SP_GET_USERS_INT
	@Id INT =0,
	@Document nvarchar(100)=null,
	@Password nvarchar(100)=null,
	@Email nvarchar(150)=null
AS
BEGIN
	SELECT u.*,coalesce(p.Id,0) ProfessorId,coalesce(s.Id,0) StudentId
	FROM Users u
	LEFT JOIN Professor p on u.Document = p.Document
	LEFT JOIN Students s on u.Document = s.Document
	WHERE (u.Id = @Id OR COALESCE(@Id, 0) = 0) 
	AND (u.Document = @Document OR COALESCE(@Document, '') = '')
	AND (u.Password = @Password OR COALESCE(@Password, '') = '')
	AND (u.Email = @Email OR COALESCE(@Email, '') = '')
	and u.Status =1
END
go

CREATE OR ALTER PROC dbo.SP_GET_PROFESSORSUBJECTS
	 @Id INT =0,
	@SubjectId INT =0,
	@ProfessorId INT =0,
	@StudentId INT =0
AS
BEGIN
	SELECT ps.*,s.Name SubjectName,p.Names +' '+  p.LastName Professor,u.Name UserCreated,R.ProfessorId,R.StudentId	
	FROM ProfessorSubject ps
	INNER JOIN Subjects s on ps.SubjectId = s.Id
	INNER JOIN Professor p on ps.ProfessorId = p.Id
	LEFT JOIN Registrations R on s.id = R.SubjectId and R.ProfessorId =p.id
	LEFT JOIN Users u on ps.CreatedBy = u.Id
	WHERE (ps.Id = @Id OR COALESCE(@Id, 0) = 0) 
	AND (ps.SubjectId = @SubjectId OR COALESCE(@SubjectId, 0) = 0) 
	AND (ps.ProfessorId = @ProfessorId OR COALESCE(@ProfessorId, 0) = 0) 
	AND ps.Status = 1 
	AND (coalesce(R.StudentId, 0) not in(@StudentId) OR COALESCE(@StudentId, 0) = 0) 

	
END
go

CREATE OR ALTER PROC dbo.SP_GET_REGISTRATIONS 
	@Id INT =0,
	@SubjectId INT =0,
	@ProfessorId INT =0,
	@StudentId INT = 0
AS
BEGIN
	SELECT rg.*,S.Name SubjectName,p.Names +' '+  p.LastName Professor,st.Names +' '+  st.LastName Student	
	FROM Registrations rg
	INNER JOIN Subjects s on rg.SubjectId = s.Id
	INNER JOIN Professor p on rg.ProfessorId = p.Id
	INNER JOIN Students st on rg.StudentId = st.Id
	WHERE (rg.Id = @Id OR COALESCE(@Id, '') = 0) 
	AND (rg.SubjectId = @SubjectId OR COALESCE(@SubjectId, '') = 0) 
	AND (rg.ProfessorId = @ProfessorId OR COALESCE(@ProfessorId, '') = 0) 
	AND (rg.StudentId = @StudentId OR COALESCE(@StudentId, '') = 0) 
	AND rg.Status = 1
END
go

--CREATE AND UPDATES
CREATE OR ALTER PROC SP_INSERT_UPDATE_STUDENTS
	@Id INT,
	@TypeDocument int ,
	@Document nvarchar(20) ,
	@Names nvarchar(100) ,
	@LastName nvarchar(100) ,
	@Email nvarchar(150) ,
	@BirtDate smalldatetime ,
	@Gender char(2),
	@Status bit NULL
as
begin
	IF not exists( SELECT Id FROM Students WHERE Id=@Id)
	begin
		INSERT INTO Students (TypeDocument,Document,Names,LastName,Email,BirtDate,CreateDate,Gender,Status)
		VALUES (@TypeDocument,@Document,@Names,@LastName,@Email,@BirtDate,GETDATE(),@Gender,@Status)
	end
	ELSE
	begin
		Update Students set Names =@Names,LastName = @LastName,Email = @Email,BirtDate=@BirtDate,Gender = @Gender,Status = @Status
		WHERE Id =@Id
	end
end
go

CREATE OR ALTER PROC SP_INSERT_UPDATE_PROFESSORS
	@Id INT,
	@TypeDocument int ,
	@Document nvarchar(20),
	@Names nvarchar(100),
	@LastName nvarchar(100),
	@Email nvarchar(150),
	@BirtDate smalldatetime,
	@Gender char(2),
	@Status bit 
as
begin
	IF not exists( SELECT Id FROM Professor WHERE Id=@Id)
	begin
		INSERT INTO Professor (TypeDocument,Document,Names,LastName,Email,BirtDate,CreateDate,Gender,Status)
		VALUES (@TypeDocument,@Document,@Names,@LastName,@Email,@BirtDate,GETDATE(),@Gender,@Status)
	end
	ELSE
	begin
		Update Professor set Names =@Names,LastName = @LastName,Email = @Email,BirtDate=@BirtDate,Gender = @Gender,Status = @Status
		WHERE Id =@Id
	end
end
go
 
CREATE OR ALTER PROC SP_INSERT_UPDATE_SUBJECTS
	@Id INT  ,
	@CreatedBy INT  ,
	@Name nvarchar(100) ,
	@Credits int ,
	@Status bit,
	@CreateDate smalldatetime = null
as
begin
	IF not exists( SELECT Id FROM Subjects WHERE Id=@Id)
	begin
		INSERT INTO Subjects (Name,Credits,CreateDate,CreatedBy,Status)
		VALUES (@Name,@Credits,GETDATE(),@CreatedBy,@Status)
	end
	ELSE
	begin
		Update Subjects set Name =@Name ,Credits = @Credits,Status = @Status
		WHERE Id =@Id
	end
end
go

CREATE OR ALTER PROC SP_INSERT_UPDATE_USERS
	@Id INT  ,
	@Name nvarchar(100),
	@Document nvarchar(20),
	@Password nvarchar(100) ,
	@TypeUser int ,
	@Email nvarchar(150),
	@Status bit ,
	@CreateDate smalldatetime = null,
	@ProfessorId int ,
	@StudentId int 
as
begin
	IF not exists( SELECT Id FROM Users WHERE Id=@Id)
	begin
		INSERT INTO Users (Name,Document,Password,Email,CreateDate,Status,TypeUser)
		VALUES (@Name,@Document,@Password,@Email,GETDATE(),@Status,@TypeUser)
	end
	ELSE
	begin
		Update Users set Name =@Name, @Document =@Document,Email = @Email,Status = @Status
		WHERE Id =@Id
	end
end
go

CREATE OR ALTER PROC SP_INSERT_UPDATE_PROFESSORSUBJECTS
	@Id INT  ,
	@SubjectId INT,
	@ProfessorId INT,
	@CreatedBy INT ,
	@Status bit,
	@CreateDate smalldatetime,
	@SubjectName nvarchar(50),
	@Professor nvarchar(50),
	@UserCreated nvarchar(50)
	
as
begin
	IF not exists( SELECT Id FROM ProfessorSubject WHERE Id=@Id)
	begin
		INSERT INTO ProfessorSubject (SubjectId,ProfessorId,CreatedBy,CreateDate,Status)
		VALUES (@SubjectId,@ProfessorId,@CreatedBy,GETDATE(),@Status)
	end
	ELSE
	begin
		Update ProfessorSubject set ProfessorId =@ProfessorId,Status = @Status
		WHERE Id =@Id
	end
end
go

CREATE OR ALTER PROC SP_INSERT_UPDATE_REGISTRATIONS
	@Id int  ,
	@ProfessorId int,
	@SubjectId int,
	@StudentId int,
	@Status bit,
	@CreateDate smalldatetime =null,
	@SubjectName nvarchar(50) = null,
	@Professor nvarchar(50) = null,
	@Student nvarchar(50) = null
as
begin
	IF not exists( SELECT Id FROM Registrations WHERE Id=@Id)
	begin
		INSERT INTO Registrations (ProfessorId,SubjectId,StudentId,CreateDate,Status)
		VALUES (@ProfessorId,@SubjectId,@StudentId,GETDATE(),@Status)
	end
	ELSE
	begin
		Update Registrations set Status = @Status
		WHERE Id =@Id
	end
end
go

IF NOT EXISTS(SELECT * FROM Users WHERE Document = '1023970895')
BEGIN
	INSERT INTO Users(Name,Email,Document, Password, CreateDate, Status,TypeUser) VALUES('ROINER GOMEZ', 'rstiven_98@hotmail.com', '1023970895', 'uEtsAkLivnjNJurAvCqqfw==', '2024-11-25 16:47:39.793', 1,1); 
END

GO
IF NOT EXISTS(SELECT * FROM TypeDocuments WHERE Code = 'CC')
BEGIN
	INSERT INTO TypeDocuments(Code, Name,CreateDate, Status) VALUES( 'CC', 'CEDULA DE CIUDADANIA', '2024-11-25 16:47:39.793', 1); 
END

GO

IF NOT EXISTS(SELECT * FROM TypeDocuments WHERE Code = 'TI')
BEGIN
	INSERT INTO TypeDocuments(Code, Name,CreateDate, Status) VALUES( 'TI', 'TARJETA DE IDENTIDAD', '2024-11-25 16:47:39.793', 1); 
END

GO

IF NOT EXISTS(SELECT * FROM TypeDocuments WHERE Code = 'PE')
BEGIN
	INSERT INTO TypeDocuments(Code, Name,CreateDate, Status) VALUES( 'PE', 'PASAPORTE', '2024-11-25 16:47:39.793', 1); 
END

GO

IF NOT EXISTS(SELECT * FROM TypeDocuments WHERE Code = 'CE')
BEGIN
	INSERT INTO TypeDocuments(Code, Name,CreateDate, Status) VALUES( 'CE', 'CEDULA DE EXTRANJERIA', '2024-11-25 16:47:39.793', 1); 
END

GO

SET IDENTITY_INSERT Subjects ON  
GO   
IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 1) 
BEGIN   
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status) VALUES( 1, 'Ingenieria de software', 3, '2024-11-25 19:35:00', 1, 1);   
END  
GO  

IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 2)
BEGIN  
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status)  VALUES( 2, 'Ingenieria civil', 3, '2024-11-27 20:49:00', 1, 1); 
END 
GO  

IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 3) 
BEGIN 
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status)  VALUES( 3, 'Ingenieria Ambiental', 3, '2024-11-27 21:44:00', 1, 1);
END 
GO    

IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 4)  
BEGIN 
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status)  VALUES( 4, 'Ingenieria Industrial', 3, '2024-11-27 21:44:00', 1, 1);  
END   
GO    

IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 5) 
BEGIN 
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status)  VALUES( 5, 'Mecatronica', 3, '2024-11-27 21:45:00', 1, 1);
END
GO    

IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 6) 
BEGIN
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status)  VALUES( 6, 'Fisica', 3, '2024-11-27 21:45:00', 1, 1);  
END 
GO   

IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 7)  BEGIN 
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status)  VALUES( 7, 'Economia', 3, '2024-11-27 21:46:00', 1, 1);  
END
GO    

IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 8) 
BEGIN   
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status)  VALUES( 8, 'Calculo 1 y 2', 3, '2024-11-27 21:46:00', 1, 1); 
END
GO    

IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 9) 
BEGIN   
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status)  VALUES( 9, 'Metodología de la investigación', 3, '2024-11-27 21:48:00', 1, 1); 
END 
GO  

IF NOT EXISTS(SELECT * FROM Subjects WHERE Id = 10) 
BEGIN
INSERT INTO Subjects(Id ,Name,Credits, CreateDate, CreatedBy, Status)  VALUES( 10, 'Medio ambiente', 3, '2024-11-27 21:48:00', 1, 1);  
END  
GO  

SET IDENTITY_INSERT Subjects OFF  
GO    