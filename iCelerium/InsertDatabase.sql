USE [SMSServers]
GO
INSERT [AspNetRoles] ([Id], [Name]) VALUES (N'36368c53-d7cb-4b4c-a8f5-00e31afe586e', N'Super Admin')
INSERT [AspNetRoles] ([Id], [Name]) VALUES (N'4cc0e448-cc7b-49e5-8bc5-7a65e186eea1', N'Admin')
INSERT [AspNetRoles] ([Id], [Name]) VALUES (N'e06f027c-fdeb-4921-99f6-33402e2ab060', N'Utilisateur')


INSERT [AspNetUsers] ([Id], [UserName], [PasswordHash], [SecurityStamp], [Discriminator], [Email], [FullName]) VALUES (N'c6831078-3d0e-419c-9b71-6225043be7e9', N'Admin', N'ACGw4OUIVX/C1iCJOEyCIloiP7Vd1UiYeGYhR/r8KEM6/3/7onIp3av78oqVH4N+Gw==', N'8d12e743-587a-482c-b87b-6f73ec2b7b9a', N'ApplicationUser', N'admin@admin.com', N'Administrateur')

INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c6831078-3d0e-419c-9b71-6225043be7e9', N'36368c53-d7cb-4b4c-a8f5-00e31afe586e')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c6831078-3d0e-419c-9b71-6225043be7e9', N'4cc0e448-cc7b-49e5-8bc5-7a65e186eea1')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c6831078-3d0e-419c-9b71-6225043be7e9', N'e06f027c-fdeb-4921-99f6-33402e2ab060')


SET IDENTITY_INSERT [Echeances] ON 

INSERT [Echeances] ([EcheanceID], [EcheanceName], [DPart]) VALUES (1, N'MENSUELLE', N'month')
INSERT [Echeances] ([EcheanceID], [EcheanceName], [DPart]) VALUES (2, N'HEBDOMADAIRE', N'week')
INSERT [Echeances] ([EcheanceID], [EcheanceName], [DPart]) VALUES (3, N'JOURNALIERE', N'day')
INSERT [Echeances] ([EcheanceID], [EcheanceName], [DPart]) VALUES (4, N'ANNUELLE', N'year')
SET IDENTITY_INSERT [Echeances] OFF
INSERT [Settings] ([HasManualEntry]) VALUES (0)
