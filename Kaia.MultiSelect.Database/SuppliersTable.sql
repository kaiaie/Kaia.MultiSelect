-- Suppliers table
CREATE TABLE IF NOT EXISTS suppliers
(
	supplier_id   INTEGER PRIMARY KEY,
	supplier_name TEXT NOT NULL UNIQUE,
	status        INTEGER DEFAULT 10 NOT NULL,
	city          TEXT NOT NULL
);

