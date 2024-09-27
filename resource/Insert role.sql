INSERT INTO [dbo].[Role]
VALUES
    ('Guest'),
    ('User'),
    ('Admin');

INSERT INTO [dbo].[RoleEvent]
VALUES 
	('Guest'),
	('Sponsor'),
	('EventOperator'),
	('CheckingStaff'),
	('Visitor');

INSERT INTO [dbo].[Tag]
VALUES 
	('SGT'),
	('CSG');

INSERT INTO [dbo].[Price]
     ([PriceType]
     ,[amount]
     ,[note]
     ,[status]
     ,[CreatedAt]
     ,[UpdatedAt]
     ,[CreatedBy])
VALUES
     ('advertisement'
     , 33000.00 -- Replace with desired amount
     , 'day'
     , 'active' -- Assuming status is stored as text; adjust if it's a boolean or integer
     , GETDATE() -- Automatically set CreatedAt to current date and time
     , GETDATE() -- Automatically set UpdatedAt to current date and time
     , '556CFC28-FAF1-4B43-86C2-8F702960031F') -- Replace with actual CreatedBy value

