-- get all user accounts
SELECT * FROM User_account;

-- add new user account
INSERT INTO User_account (username, password, userId)
VALUES("barbi", "musta99", 2)

SELECT userId FROM User_account
WHERE username = "barbi"
AND password = "musta99" 