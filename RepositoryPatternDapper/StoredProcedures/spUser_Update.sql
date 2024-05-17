CREATE PROCEDURE IF NOT EXISTS spUser_Update(IN p_Id INT, IN p_FirstName NVARCHAR(50), IN p_LastName NVARCHAR(50))
BEGIN
    UPDATE users SET FirstName = p_FirstName, LastName = p_LastName WHERE Id = p_Id;
END;
