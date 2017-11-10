-- Products table
CREATE TABLE IF NOT EXISTS products
(
	product_id   INTEGER PRIMARY KEY,
	product_name TEXT NOT NULL,
	color        TEXT NOT NULL,
	weight       NUMERIC DEFAULT 0 CHECK (weight IS NOT NULL AND weight > 0),
	city         TEXT NOT NULL,
	CONSTRAINT   uq_product_color UNIQUE (product_name, color)
);
