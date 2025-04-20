IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CollegeTaskV2')
BEGIN
    CREATE DATABASE CollegeTaskV2;
END
GO

USE CollegeTaskV2;
GO

create table BookingStatus
(
    id   int identity
        constraint PK__BookingS__3213E83FDAD5A503
            primary key,
    name nvarchar(50) not null
)
go

create table Client
(
    id          int identity
        constraint PK__Client__3213E83FAFB5A7C0
            primary key,
    first_name  nvarchar(50) not null,
    middle_name nvarchar(50),
    last_name   nvarchar(50) not null,
    created_at  datetime default getdate(),
    updated_at  datetime default getdate()
)
go

create table Account
(
    id                  int identity
        constraint PK__Account__3213E83FAAA4239E
            primary key,
    id_client           int          not null
        constraint FK__Account__id_clie__3C69FB99
            references Client,
    balance             money,
    login               nvarchar(50) not null,
    password            binary(64)   not null,
    last_login          datetime,
    password_changed_at datetime,
    created_at          datetime default getdate(),
    updated_at          datetime default getdate(),
    email               nvarchar(50),
    is_alive            bit      default 1
)
go

create index IX_Account_id_client
    on Account (id_client)
go

create table BalanceHistory
(
    id               int identity
        constraint PK__BalanceH__3213E83FB261A664
            primary key,
    action           nvarchar(50),
    price            money,
    previous_balance money,
    created_at       datetime default getdate(),
    account_id       int not null
        constraint FK__BalanceHi__accou__0F624AF8
            references Account
)
go

create index IX_BalanceHistory_account_id
    on BalanceHistory (account_id)
go

create table Club
(
    id            int identity
        constraint PK__Club__3213E83FBE0BCDC2
            primary key,
    address       nvarchar(max) not null,
    name          nvarchar(50)  not null,
    phone         varchar(15)   not null,
    working_hours nvarchar(50) default N'10:00-21:00',
    created_at    datetime     default getdate(),
    updated_at    datetime     default getdate(),
    is_alive      bit          default 1
)
go

create table CostRevenue
(
    id          int identity
        constraint PK__CostReve__3213E83F693444FD
            primary key,
    id_club     int      not null
        constraint FK__CostReven__id_cl__55009F39
            references Club,
    amount      money    not null,
    revenue     bit,
    description nvarchar(max),
    created_at  datetime not null
)
go

create index IX_CostRevenue_id_club
    on CostRevenue (id_club)
go

create table EquipmentStatus
(
    id   int identity
        constraint PK__Equipmen__3213E83F47EA80CB
            primary key,
    name nvarchar(50) not null
)
go

create table Equipment
(
    id             int identity
        constraint PK__Equipmen__3213E83F7552EB31
            primary key,
    type           nvarchar(50) not null,
    name           nvarchar(80) not null,
    specification  nvarchar(120),
    purchase_date  date,
    purchase_price money,
    id_club        int          not null
        constraint FK__Equipment__id_cl__5CD6CB2B
            references Club,
    status         int          not null
        constraint FK__Equipment__statu__5DCAEF64
            references EquipmentStatus,
    quantity       int      default 1,
    created_at     datetime default getdate(),
    updated_at     datetime default getdate()
)
go

create index IX_Equipment_id_club
    on Equipment (id_club)
go

create index IX_Equipment_status
    on Equipment (status)
go

create table EquipmentMaintenance
(
    id               int identity
        constraint PK__Equipmen__3213E83FBFB6EB88
            primary key,
    equipment_id     int not null
        constraint FK__Equipment__equip__6A30C649
            references Equipment,
    maintenance_date datetime default getdate(),
    description      nvarchar(max),
    cost             money,
    created_at       datetime default getdate()
)
go

create index IX_EquipmentMaintenance_equipment_id
    on EquipmentMaintenance (equipment_id)
go

create table Feedback
(
    id         int identity
        constraint PK__Feedback__3213E83FE11FE75C
            primary key,
    id_club    int not null
        constraint FK__Feedback__id_clu__0B91BA14
            references Club,
    rating     int not null,
    comment    nvarchar(max),
    created_at datetime default getdate(),
    account_id int not null
        constraint FK__Feedback__accoun__17036CC0
            references Account
)
go

create index IX_Feedback_account_id
    on Feedback (account_id)
go

create index IX_Feedback_id_club
    on Feedback (id_club)
go

create table Payment
(
    id                    int identity
        constraint PK__Payment__3213E83FCB0EC01C
            primary key,
    encrypted_card_number varbinary(32),
    encrypted_CVV         varbinary(32),
    account_id            int not null
        constraint FK__Payment__account__1332DBDC
            references Account,
    created_at            datetime default getdate()
)
go

create index IX_Payment_account_id
    on Payment (account_id)
go

create table PaymentMethod
(
    id   int identity
        constraint PK__PaymentM__3213E83F3C8802FA
            primary key,
    name nvarchar(50) not null
)
go

create table Role
(
    id         int identity
        constraint PK__Role__3213E83FAD08622B
            primary key,
    name       nvarchar(50) not null,
    created_at datetime default getdate()
)
go

create table Employee
(
    id                  int identity
        constraint PK__Employee__3213E83F27CF2024
            primary key,
    login               nvarchar(50) not null,
    password            binary(64)   not null,
    passport_data       nvarchar(50) not null,
    hire_date           date         not null,
    id_role             int          not null
        constraint FK__Employee__id_rol__7C4F7684
            references Role,
    salary              money        not null,
    id_club             int          not null
        constraint FK__Employee__id_clu__7D439ABD
            references Club,
    last_login          datetime,
    password_changed_at datetime,
    created_at          datetime default getdate(),
    updated_at          datetime default getdate()
)
go

create index IX_Employee_id_club
    on Employee (id_club)
go

create index IX_Employee_id_role
    on Employee (id_role)
go

create table EmployeeActionLog
(
    id          int identity
        constraint PK__Employee__3213E83F8B98D81C
            primary key,
    id_employee int          not null
        constraint FK__EmployeeA__id_em__02FC7413
            references Employee,
    action_type nvarchar(50) not null,
    description nvarchar(max),
    created_at  datetime default getdate()
)
go

create index IX_EmployeeActionLog_id_employee
    on EmployeeActionLog (id_employee)
go

create table Shift
(
    id          int identity
        constraint PK__Shift__3213E83F37EEADBA
            primary key,
    id_employee int      not null
        constraint FK__Shift__id_employ__06CD04F7
            references Employee,
    start_time  datetime not null,
    end_time    datetime not null,
    created_at  datetime default getdate(),
    updated_at  datetime default getdate()
)
go

create index IX_Shift_id_employee
    on Shift (id_employee)
go

create table Statistic
(
    id            int identity
        constraint PK__Statisti__3213E83F70667EBB
            primary key,
    finances      money not null,
    client_number int,
    id_club       int   not null
        constraint FK_Statistic_Club_id_club
            references Club
            on delete cascade
)
go

create index IX_Statistic_id_club
    on Statistic (id_club)
go

create table Tariff
(
    id             int identity
        constraint PK__Tariff__3213E83FFCBC004C
            primary key,
    name           nvarchar(50) not null,
    pricePerMinute money        not null
)
go

create table WorkingSpace
(
    id         int identity
        constraint PK__WorkingS__3213E83F500C02FE
            primary key,
    id_club    int          not null
        constraint FK__WorkingSp__id_cl__5535A963
            references Club,
    name       nvarchar(50),
    status     nvarchar(50) not null,
    created_at datetime default getdate(),
    updated_at datetime default getdate(),
    tariff_id  int
        constraint FK__WorkingSp__tarif__3A4CA8FD
            references Tariff
)
go

create table Booking
(
    id                int identity
        constraint PK__Booking__3213E83F0889DDB9
            primary key,
    id_working_space  int      not null
        constraint FK__Booking__id_work__71D1E811
            references WorkingSpace,
    start_time        datetime not null,
    end_time          datetime,
    id_status         int      not null
        constraint FK__Booking__id_stat__72C60C4A
            references BookingStatus,
    total_cost        money,
    id_payment_method int
        constraint FK__Booking__id_paym__73BA3083
            references PaymentMethod,
    created_at        datetime default getdate(),
    updated_at        datetime default getdate(),
    account_id        int      not null
        constraint FK__Booking__account__160F4887
            references Account
)
go

create index IX_Booking_account_id
    on Booking (account_id)
go

create index IX_Booking_id_payment_method
    on Booking (id_payment_method)
go

create index IX_Booking_id_status
    on Booking (id_status)
go

create index IX_Booking_id_working_space
    on Booking (id_working_space)
go

create index IX_WorkingSpace_id_club
    on WorkingSpace (id_club)
go

create index IX_WorkingSpace_tariff_id
    on WorkingSpace (tariff_id)
go

create table WorkingSpaceEquipment
(
    id               int identity
        constraint PK__WorkingS__3213E83F2597700A
            primary key,
    id_working_space int not null
        constraint FK__WorkingSp__id_wo__6477ECF3
            references WorkingSpace,
    id_equipment     int not null
        constraint FK__WorkingSp__id_eq__656C112C
            references Equipment,
    created_at       datetime default getdate(),
    updated_at       datetime default getdate()
)
go

create index IX_WorkingSpaceEquipment_id_equipment
    on WorkingSpaceEquipment (id_equipment)
go

create index IX_WorkingSpaceEquipment_id_working_space
    on WorkingSpaceEquipment (id_working_space)
go






INSERT INTO CollegeTaskV2.dbo.Club (address, name, phone, working_hours, created_at, updated_at, is_alive) VALUES (N'Пример адреса. Изменить для первого клуба.', N'площадка 1', N'?????? ??????', N'10:00-21:00', N'2025-02-17 09:28:30.403', N'2025-02-17 09:28:30.403', 1);
INSERT INTO CollegeTaskV2.dbo.Employee (login, password, passport_data, hire_date, id_role, salary, id_club, last_login, password_changed_at, created_at, updated_at) VALUES (N'Owner', 0xEF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F0000000000000000000000000000000000000000000000000000000000000000, N'test', N'2025-02-17', 1, 1000000000.0000, 1, N'2025-04-15 21:53:23.153', null, N'2025-02-17 12:49:22.503', N'2025-02-17 09:49:22.600');

INSERT INTO CollegeTaskV2.dbo.EquipmentStatus (name) VALUES ( N'working');
INSERT INTO CollegeTaskV2.dbo.EquipmentStatus (name) VALUES ( N'not_working');

SET IDENTITY_INSERT CollegeTaskV2.dbo.Role ON;
INSERT INTO CollegeTaskV2.dbo.Role (id, name, created_at)VALUES (1, N'Owner', N'2025-02-17 09:32:59.557');
SET IDENTITY_INSERT CollegeTaskV2.dbo.Role OFF;
SET IDENTITY_INSERT CollegeTaskV2.dbo.Role ON;
INSERT INTO CollegeTaskV2.dbo.Role (id, name, created_at)VALUES (2, N'Admin', N'2025-02-17 09:32:59.573');
SET IDENTITY_INSERT CollegeTaskV2.dbo.Role OFF;
