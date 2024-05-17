CREATE PROCEDURE IF NOT EXISTS spUser_Get(IN p_Id INT)
BEGIN
    SELECT Id, FirstName, LastName FROM users WHERE Id = p_Id;
END;