CREATE TABLE [User]
(
  ID BIGINT NOT NULL,
  Version INT NOT NULL CONSTRAINT DF_User_Version DEFAULT(0),
  LastModifiedBy DATETIME NOT NULL,
  LastModifiedOn BIGINT NOT NULL,
  CreatedBy DATETIME NOT NULL,
  CreatedOn BIGINT NOT NULL,
  DeletedBy DATETIME NULL,
  DeletedOn BIGINT NULL,
  FirstName NVARCHAR (200) NOT NULL,
  LastName NVARCHAR (200) NOT NULL,
  Email NVARCHAR (200) NOT NULL,
  Password VARBINARY (100) NULL,
  Salt VARBINARY (50) NULL,
  LastChangedPasswordOn DATETIME NULL,
  CONSTRAINT PK_User PRIMARY KEY (ID)
)