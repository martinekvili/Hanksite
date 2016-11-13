PRAGMA foreign_keys = ON;

CREATE TABLE User (
    ID INTEGER PRIMARY KEY,
    UserName TEXT NOT NULL,
    PasswordSalt CHAR(24),
    Password CHAR(44)
);
CREATE UNIQUE INDEX User_UserName ON User(UserName);

CREATE TABLE Game (
    ID INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL
);

CREATE TABLE GameUser (
    UserID INTEGER NOT NULL,
    GameID INTEGER NOT NULL,
    Position INTEGER NOT NULL,
    PRIMARY KEY (UserID, GameID),
    FOREIGN KEY (UserID) REFERENCES User(ID),
    FOREIGN KEY (GameID) REFERENCES Game(ID)
) WITHOUT ROWID;

INSERT INTO User (UserName)
VALUES 
     ('AI1'), 
     ('AI2'),
     ('AI3'),
     ('AI4'),
     ('AI5'),
     ('AI6'),
     ('AI7');