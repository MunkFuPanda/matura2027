
-- 1
GO
DROP TRIGGER IF EXISTS dbo.trg_commission;
GO

CREATE TRIGGER dbo.trg_commission
ON employees
AFTER INSERT, UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (
    SELECT 1 FROM inserted
    WHERE job_id IN ('SA_MAN', 'SA_REP')
      AND commission_pct IS NULL OR commission_pct <= 0
  )
  BEGIN
    RAISERROR('Verkäufer und Manager benötigen zwingend eine gültige Provision.', 16, 1);
    ROLLBACK TRANSACTION;
    RETURN;
  END

  IF EXISTS (
    SELECT 1 FROM inserted
    WHERE job_id NOT IN ('SA_MAN', 'SA_REP')
      AND commission_pct IS NOT NULL OR commission_pct > 0
  )
  BEGIN
    RAISERROR('Nur Verkäufer und Manager sind berechtigt, eine Provision zu erhalten.', 16, 2);
    ROLLBACK TRANSACTION;
    RETURN;
  END
END;
GO

-- 2
GO
DROP TRIGGER IF EXISTS dbo.trg_president;
GO

CREATE TRIGGER dbo.trg_president
ON employees
AFTER INSERT, UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (SELECT 1 FROM inserted WHERE job_id = 'AD_PRES')
  BEGIN
    IF (SELECT COUNT(*) FROM employees WHERE job_id = 'AD_PRES') > 1
    BEGIN
      RAISERROR('Das Unternehmen darf nur einen einzigen Präsidenten haben.', 16, 3);
      ROLLBACK TRANSACTION;
      RETURN;
    END
  END
END;
GO

-- 3
GO
DROP TRIGGER IF EXISTS dbo.trg_max_employees;
GO

CREATE TRIGGER dbo.trg_max_employees
ON employees
AFTER INSERT, UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (
    SELECT manager_id
    FROM employees
    WHERE manager_id IN (SELECT manager_id FROM inserted WHERE manager_id IS NOT NULL)
    GROUP BY manager_id
    HAVING COUNT(*) > 15
  )
  BEGIN
    RAISERROR('Einem Manager können maximal 15 Mitarbeiter zugeordnet sein.', 16, 4);
    ROLLBACK TRANSACTION;
    RETURN;
  END
END;
GO

-- 4
GO
DROP TRIGGER IF EXISTS dbo.trg_salary_no_decrease;
GO

CREATE TRIGGER dbo.trg_salary_no_decrease
ON employees
AFTER UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (
    SELECT 1
    FROM inserted i
    JOIN deleted d ON i.employee_id = d.employee_id
    WHERE i.salary < d.salary
  )
  BEGIN
    RAISERROR('Eine Gehaltsreduktion ist nicht zulässig – nur Erhöhungen sind erlaubt.', 16, 5);
    ROLLBACK TRANSACTION;
    RETURN;
  END
END;
GO

-- 5
GO
DROP TRIGGER IF EXISTS dbo.trg_location_raise;
GO

CREATE TRIGGER dbo.trg_location_raise
ON departments
AFTER UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  IF UPDATE(location_id)
  BEGIN
    UPDATE e
    SET e.salary = e.salary * 1.02
    FROM employees e
    JOIN inserted i ON e.department_id = i.department_id
    JOIN deleted  d ON i.department_id = d.department_id
    WHERE i.location_id <> d.location_id;
  END
END;
GO

-- 6
GO
DROP TRIGGER IF EXISTS dbo.trg_working_hours;
GO

CREATE TRIGGER dbo.trg_working_hours
ON employees
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @now  DATETIME = GETDATE();
  DECLARE @day  INT      = DATEPART(weekday, @now);
  DECLARE @time TIME     = CAST(@now AS TIME);

  IF @day IN (1, 7) OR @time < '08:45' OR @time > '17:30'
  BEGIN
    RAISERROR('Datenbankänderungen sind außerhalb der Geschäftszeiten nicht gestattet.', 16, 6);
    ROLLBACK TRANSACTION;
    RETURN;
  END
END;
GO

-- 7
GO
DROP TRIGGER IF EXISTS dbo.trg_min_salary_update;
GO

CREATE TRIGGER dbo.trg_min_salary_update
ON jobs
AFTER UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  IF UPDATE(min_salary)
  BEGIN
    UPDATE e
    SET e.salary = i.min_salary
    FROM employees e
    JOIN inserted i ON e.job_id = i.job_id
    JOIN deleted  d ON i.job_id = d.job_id
    WHERE i.min_salary > d.min_salary
      AND e.salary = d.min_salary;
  END
END;
GO

-- 8
-- krieg ich ned hin :/

-- 9
GO
DROP TRIGGER IF EXISTS dbo.trg_emp_audit;
GO

CREATE TRIGGER dbo.trg_emp_audit
ON employees
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @action VARCHAR(100);

  IF      EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted) SET @action = 'UPDATE';
  ELSE IF EXISTS (SELECT 1 FROM inserted)                                     SET @action = 'INSERT';
  ELSE                                                                         SET @action = 'DELETE';

  INSERT INTO emp_audit (employee_id, action_type)
  SELECT employee_id, @action FROM inserted
  UNION ALL
  SELECT employee_id, @action FROM deleted
  WHERE NOT EXISTS (SELECT 1 FROM inserted);
END;
GO

-- 10
DROP TRIGGER IF EXISTS dbo.trg_job_change;
GO

CREATE TRIGGER dbo.trg_job_change
ON employees
AFTER UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  IF UPDATE(job_id)
  BEGIN
    INSERT INTO job_history (employee_id, start_date, end_date, job_id, department_id)
    SELECT
      d.employee_id,
      d.hire_date,
      GETDATE(),
      d.job_id,
      d.department_id
    FROM deleted  d
    JOIN inserted i ON d.employee_id = i.employee_id
    WHERE d.job_id <> i.job_id;
  END
END;
GO

-- 11
GO
DROP TRIGGER IF EXISTS dbo.trg_salary_range;
GO

CREATE TRIGGER dbo.trg_salary_range
ON employees
AFTER INSERT, UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (
    SELECT 1
    FROM inserted i
    JOIN jobs j ON i.job_id = j.job_id
    WHERE i.salary < j.min_salary
       OR i.salary > j.max_salary
  )
  BEGIN
    RAISERROR('Das angegebene Gehalt überschreitet die zulässige Gehaltsspanne für diese Stelle.', 16, 7);
    ROLLBACK TRANSACTION;
    RETURN;
  END
END;
GO
