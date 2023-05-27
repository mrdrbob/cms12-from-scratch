CREATE TABLE Blend_Index(
	Id					int					NOT NULL IDENTITY(1, 1), -- Database ID
	Identifier			varchar(64)			NOT NULL, -- Content Identifier (content ID)
	Language            varchar(8)          NOT NULL, -- Language Identifier
	StoreName           varchar(64)         NOT NULL, -- Allows storing content from multiple sources (not just the CMS)
	Name	            varchar(64)         NOT NULL, -- Name of the property being indexed

	Value				nvarchar(512)		NULL, -- Value of the property

	CONSTRAINT PK_Blend_Index PRIMARY KEY (Id)

);

GO

CREATE INDEX IDX_Blend_Index_Value ON Blend_Index(Value) INCLUDE (Identifier, Language, Name, StoreName);

GO

CREATE PROCEDURE Blend_GetIndexVersion AS 
BEGIN 
	SELECT 1
END
