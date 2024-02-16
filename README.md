# apiBackend - .NET 5.0 e PostgreSQL

## 1. Passos para começar

### Clonando o Repositório

`git clone https://github.com/antony-93/apiBackend`

ATENÇÃO: Certifique-se de ter instalado em sua maquina o .NET na versão 5.0 e o banco de dados PostgreSQL!

### Gerando tabela PostgreSQL

Execute esses comandos sql para gerar a tabela no postgreSQL!

```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE cliente (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    nome VARCHAR(255) NOT NULL,
    cnpj BIGINT UNIQUE NOT NULL,
    dataCadastro DATE DEFAULT CURRENT_DATE,
    endereco VARCHAR(255),
    telefone BIGINT
);
```

### Configurando a aplicação

Em appsettings.Development.json e appsettings.json, atualize DefaultConnection para as configurações de seu banco de dados PostgreSQL!

### Iniciando a aplicação

Use o comando `dotnet run`  para iniciar a aplicação!

ATENÇÃO: Certifique-se de ter instalado em sua maquina o .NET na versão 5.0!

### Rotas da aplicação 

GET http://localhost:5000/api/Cliente

GET http://localhost:5000/api/Cliente/idAqui

DELETE http://localhost:5000/api/Cliente/idAqui

POST http://localhost:5000/api/Cliente

```JSON
{
    "nome": "nomeAqui",
    "cnpj": 00000000000000,
    "endereco": "enderecoAqui",
    "telefone": 48996686585
}
```

PUT http://localhost:5000/api/Cliente/idAqui

```JSON
{
    "nome": "nomeAqui",
    "cnpj": 00000000000000,
    "endereco": "enderecoAqui",
    "telefone": 48996686585
}
```



