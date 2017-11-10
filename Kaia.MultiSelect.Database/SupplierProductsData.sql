BEGIN TRANSACTION;

INSERT OR REPLACE INTO supplier_products
(
	supplier_id,
	product_id,
	quantity
)
      SELECT 1 AS supplier_id, 1 AS product_id, 300 AS quantity
UNION SELECT 1,                2,               200
UNION SELECT 1,                3,               400
UNION SELECT 1,                4,               200
UNION SELECT 1,                5,               100
UNION SELECT 1,                6,               100
UNION SELECT 2,                1,               300
UNION SELECT 2,                2,               400
UNION SELECT 3,                2,               200
UNION SELECT 4,                2,               200
UNION SELECT 4,                4,               300
UNION SELECT 4,                5,               400;

COMMIT;

