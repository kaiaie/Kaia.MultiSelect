BEGIN TRANSACTION;

INSERT OR REPLACE INTO suppliers
(
	supplier_id,
	supplier_name,
	status,
	city
)
      SELECT 1 AS supplier_id, 'Smith' AS supplier_name, 20 AS status, 'London' AS city
UNION SELECT 2,                'Jones',                  10,           'Paris'
UNION SELECT 3,                'Blake',                  30,           'Paris'
UNION SELECT 4,                'Clark',                  20,           'London'
UNION SELECT 5,                'Adams',                  30,           'Athens';

COMMIT;
