CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE cliente (
	id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    nome VARCHAR(255) NOT NULL,
    cnpj BIGINT UNIQUE NOT NULL,
	dataCadastro DATE DEFAULT CURRENT_DATE,
    endereco VARCHAR(255),
    telefone BIGINT
);
