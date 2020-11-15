CREATE DATABASE [InventoryManagement]

USE [InventoryManagement]

CREATE TABLE [dbo].[Login]([user_id] [int] NOT NULL PRIMARY KEY,[user_name] [varchar](50), [pwd] [varchar](50))

INSERT INTO [dbo].[Login]([user_id],[user_name],[pwd]) VALUES ('" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "')

SELECT * from [dbo].[Login] where user_id = " + textBox3.Text + " 

select * from [dbo].[Login] where user_id = " + textBox1.Text + " and pwd ='" + textBox2.Text + "'


CREATE VIEW [dbo].[Loginview] AS SELECT [user_id],[user_name] FROM [dbo].[Login]

SELECT * from [LoginView]




CREATE TABLE [dbo].[Products]([p_code] [int] NOT NULL PRIMARY KEY,[p_name] [varchar](80), [p_brand] [varchar](80))

INSERT INTO [dbo].[Products] ([p_code],[p_name],[p_brand]) VALUES(" + textBox1.Text + ", '" + textBox2.Text + "', '" + textBox3.Text + "')

UPDATE[dbo].[Products] SET [p_name] = '" + textBox2.Text + "', [p_brand] = '" + textBox3.Text + "' WHERE [p_code] = " + textBox1.Text + " 

DELETE FROM [dbo].[Products] WHERE [p_code] = " + textBox1.Text + " 

select * from [dbo].[Products]

SELECT * from [dbo].[Products] where p_code = '"+ productCode +"' 

SELECT p_name FROM [dbo].[Products] where p_code = '" + textBox1.Text + "'


SELECT p.p_code, p.p_name, p.p_brand, s.p_qty, s.date, s.p_status from Products p INNER JOIN Stock s on p.p_code = s.p_code and s.p_status = 1 

SELECT p.p_code, p.p_name, p.p_brand, s.p_qty, s.date, s.p_status from Products p INNER JOIN Stock s on p.p_code = s.p_code and s.p_status = 0 


CREATE TABLE [dbo].[Stock]([p_code] [int] NOT NULL PRIMARY KEY,[p_name] [varchar](100), [date] [date], [p_qty] [float], [p_status] [bit]) 

INSERT INTO [dbo].[Stock] ([p_code],[p_name],[date],[p_qty],[p_status]) VALUES(" + textBox1.Text + ", '" + textBox2.Text + "', '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "', " + textBox3.Text + ", '" + status + "')

UPDATE[dbo].[Stock] SET [p_name] = '" + textBox2.Text + "', [p_qty] = " + textBox3.Text + ", [p_status] = '" + status + "', [date] = '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "' WHERE [p_code] = " + textBox1.Text + " 

DELETE FROM [dbo].[Stock] WHERE [p_code] = " + textBox1.Text + " 

SELECT * from [dbo].[Stock]

SELECT COUNT(p_code), SUM(p_qty) FROM [dbo].[Stock]

SELECT * from [dbo].[Stock] where p_code = '" + productCode + "' 

SELECT p_name FROM [dbo].[Products] where p_code = '" + textBox1.Text + "'




CREATE PROCEDURE [dbo].[Report]	@status int
AS
	SELECT COUNT(p.p_code), SUM(s.p_qty) FROM Products p INNER JOIN Stock s on p.p_code = s.p_code and s.p_status = @status
	
EXEC Report @status = 0

EXEC Report @status = 1




