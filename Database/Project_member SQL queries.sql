SELECT * FROM Project_member

-- get projects where user participates by his id
SELECT projectId FROM Project_member
WHERE userId = 2
