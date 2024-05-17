CREATE PROCEDURE IF NOT EXISTS spUser_Insert(IN p_FirstName NVARCHAR(50), IN p_LastName NVARCHAR(50))
BEGIN
    INSERT INTO users (FirstName, LastName) VALUES (p_FirstName, p_LastName);
END;