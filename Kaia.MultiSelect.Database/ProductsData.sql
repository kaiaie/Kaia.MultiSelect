BEGIN TRANSACTION;

INSERT OR REPLACE INTO products
(
	product_id,
	product_name,
	color,
	weight,      
	city
)
      SELECT 1 AS product_id, 'Nut' AS product_name, 'Red' AS color, 12 AS weight, 'London' AS city
UNION SELECT 2,               'Bolt',                'Green',        17,           'Paris'
UNION SELECT 3,               'Screw',               'Blue',         17,           'Rome'
UNION SELECT 4,               'Screw',               'Red',          14,           'London'
UNION SELECT 5,               'Cam',                 'Blue',         12,           'Paris'
UNION SELECT 6,               'Cog',                 'Red',          19,           'London';

COMMIT;
