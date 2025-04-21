CREATE DATABASE PAYROLL
USE PAYROLL


CREATE TABLE Employee(
EmployeeID int PRIMARY KEY,
FirstName varchar(30) not null,
LastName varchar(30)not null,
DateOfBirth date not null,
Gender varchar(10) not null,
Email varchar(255) not null,
PhoneNumber varchar(30) not null,
Address varchar(255) not null,
Position varchar(50) not null,
JoiningDate date not null,
TermininationDate date null)
GO

CREATE TABLE Payroll(
PayrollID int PRIMARY KEY,
EmployeeID int FOREIGN KEY REFERENCES Employee(EmployeeID),
PayPeriodStartDate date,
PayPeriodEndDate date,
BasicSalary money,
OvertimePay money,
Deductions money,
NetSalary money
)
GO

CREATE TABLE Tax(
TaxID int PRIMARY KEY,
EmployeeID int FOREIGN KEY REFERENCES Employee(EmployeeID),
TaxYear int,
TaxableIncome money,
TaxAmount money
)
GO

CREATE TABLE FinancialRecord(
RecordID int PRIMARY KEY,
EmployeeID int FOREIGN KEY REFERENCES Employee(EmployeeID),
RecordDate date,
Description varchar(255),
Amount money,
RecordType varchar(100)
)
GO

