CREATE PROCEDURE IF NOT EXISTS spUser_GetAll()
BEGIN
    SELECT Id, FirstName, LastName FROM users;
END;