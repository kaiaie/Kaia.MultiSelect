-- Supplier/ Products table
CREATE TABLE IF NOT EXISTS supplier_products
(
	supplier_id INT NOT NULL,
	product_id  INT NOT NULL,
	quantity    INT DEFAULT 1 CHECK (quantity IS NOT NULL AND quantity > 0),
	CONSTRAINT pk_supplier_products PRIMARY KEY (supplier_id, product_id),
	CONSTRAINT fk_supp_prod_supplier FOREIGN KEY (supplier_id)
		REFERENCES suppliers (supplier_id) ON DELETE CASCADE,
	CONSTRAINT fk_supp_prod_product FOREIGN KEY (product_id)
		REFERENCES products (product_id) ON DELETE CASCADE
);