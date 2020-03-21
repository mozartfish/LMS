/*DEPARTMENTS*/
Create Table Departments(
DeptName varchar(100) NOT NULL,
DeptAbbreviation varchar(4) NOT NULL,
Primary Key(DeptAbbreviation)
);
/*COURSES*/
Create Table Courses(
courseID int unsigned NOT NULL AUTO_INCREMENT,
CourseName varchar(100) NOT NULL,
CourseNumber int unsigned NOT NULL,
DeptAbbreviation varchar(4) NOT NULL,
UNIQUE(CourseNumber, DeptAbbreviation),
Primary Key(courseID),
Foreign Key(DeptAbbreviation) References Departments(DeptAbbreviation)
);
/*CLASSES*/
Create Table Classes(
classID int unsigned NOT NULL AUTO_INCREMENT,
courseID int unsigned NOT NULL,
Year int unsigned NOT NULL,
Season Varchar(6) NOT NULL,
Location VARCHAR(100) NOT NULL,
StartTime DATETIME NOT NULL,
EndTime DATETIME NOT NULL,
UNIQUE(courseID, Year, Season),
Primary Key(classID),
Foreign Key(courseID) references Courses(courseID)
);
/*STUDENTS*/
Create Table Students(
uID char(8) NOT NULL,
FirstName varchar(100) NOT NULL,
LastName varchar(100) NOT NULL,
DOB DATE NOT NULL,
Primary Key(uID)cor
);

/*Professors*/
Create Table Professors(
uID char(8) NOT NULL,
FirstName varchar(100) NOT NULL,
LastName varchar(100) NOT NULL,
DOB DATE NOT NULL,
Primary Key(uID)
);

/*Administrators*/
Create Table Administrators(
uID char(8) NOT NULL,
FirstName varchar(100) NOT NULL,
LastName varchar(100) NOT NULL,
DOB DATE NOT NULL,
Primary Key(uID)
);

/*ASSIGNMENT CATEGORIES*/
Create Table AssignmentCategories(
categoryID int unsigned NOT NULL AUTO_INCREMENT,
classID int unsigned NOT NULL,
Weight int unsigned NOT NULL,
CategoryName VARCHAR(100) NOT NULL,
UNIQUE(classID, CategoryNAME),
Primary Key(categoryID),
Foreign Key(classID) References Classes(classID)
);

/*ASSIGNMENTS*/
CREATE TABLE Assignments(
assignmentID int unsigned NOT NULL AUTO_INCREMENT,
categoryID int unsigned NOT NULL,
AsgmtName VARCHAR(100) NOT NULL,
MaxPointValue INT UNSIGNED NOT NULL,
Contents VARCHAR(8192) NOT NULL,
DueDate DATETIME NOT NULL,
UNIQUE(categoryID, AsgmtName),
Primary Key(assignmentID),
Foreign Key(categoryID) References AssignmentCategories(categoryID)
);

/*SUBMISSIONS*/
Create Table Submission(
uID char(8) NOT NULL,
assignmentID int unsigned NOT NULL,
SubTime DateTime NOT NULL,
Score int unsigned NOT NULL,
Content varchar(8192) NOT NULL,
Primary Key(uID, assignmentID),
Foreign Key(uID) References Students(uID),
Foreign Key(assignmentID) References Assignments(assignmentID)
);

/*ENROLLMENT GRADE*/
Create Table EnrollmentGrade(
uID char(8) NOT NULL,
classID int unsigned NOT NULL,
Grade varchar(2) NOT NULL,
Primary Key(uID, classID),
Foreign Key(uID) References Students(uId),
Foreign Key(classID) References Classes(classID) 
);


