CREATE TABLE Usuario (
  user_id INTEGER   NOT NULL ,
  user_nome VARCHAR(50)    ,
  user_cpf VARCHAR(11)    ,
  user_endereco VARCHAR(100)    ,
  user_endereco_num INTEGER    ,
  user_cidade VARCHAR (50)	,
  user_estado_uf VARCHAR (2),
  user_datanascimento DATE    ,
  user_sexo VARCHAR(20)    ,
  user_telefone VARCHAR(11)      ,
PRIMARY KEY(user_id));



CREATE TABLE Livro (
  livro_id INTEGER   NOT NULL ,
  livro_nome VARCHAR    ,
  livro_capa TEXT    ,
  livro_descricao VARCHAR    ,
  livro_autor_fk INTEGER    ,
  livro_editora_fk INTEGER    ,
  livro_publicacao DATETIME    ,
  livro_npaginas INTEGER    ,
  livro_valor INTEGER      ,
PRIMARY KEY(livro_id));



CREATE TABLE Autor (
  autor_id INTEGER   NOT NULL ,
  Livro_livro_id INTEGER   NOT NULL ,
  autor_nome VARCHAR    ,
  autor_sobrenome VARCHAR    ,
  autor_desc VARCHAR      ,
PRIMARY KEY(autor_id)  ,
  FOREIGN KEY(Livro_livro_id)
    REFERENCES Livro(livro_id));


CREATE INDEX Autor_FKIndex1 ON Autor (Livro_livro_id);


CREATE INDEX IFK_Rel_01 ON Autor (Livro_livro_id);


CREATE TABLE Editora (
  editora_id INTEGER   NOT NULL ,
  Livro_livro_id INTEGER   NOT NULL ,
  editora_nome VARCHAR    ,
  editora_cnpj INTEGER      ,
PRIMARY KEY(editora_id)  ,
  FOREIGN KEY(Livro_livro_id)
    REFERENCES Livro(livro_id));


CREATE INDEX Editora_FKIndex1 ON Editora (Livro_livro_id);


CREATE INDEX IFK_Rel_02 ON Editora (Livro_livro_id);



