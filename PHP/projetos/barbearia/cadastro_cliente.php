<?php

include('utils/conectadb.php');

session_start();

if (isset($_SESSION['idfuncionario'])) {

    $idfuncionario = $_SESSION['idfuncionario'];

    $sql = "SELECT FUNC_NOME FROM funcionarios 
        WHERE FUNC_ID = '$idfuncionario'";

    $enviaquery = mysqli_query($link, $sql);
    $nomeusuario = mysqli_fetch_array($enviaquery) [0];
} 
else {
    echo "<script>alert('Usuário não logado!');</script>";
    echo "<script>window.location.href = 'login.php';</script>";
}

if ($_SERVER['REQUEST_METHOD'] == 'POST') {

    $nomecliente = $_POST ['txtNome'];
    $cpfcliente = $_POST ['txtCPF'];
    $telefonecliente = $_POST ['txtTelefone'];
    $datanascimento = $_POST ['txtDatanascimento'];
    $ativocliente = $_POST['rbAtivo'];

    //verifica se cliente existe
    $sql = "SELECT COUNT(CLI_CPF) FROM clientes 
    WHERE CLI_CPF = '$cpfcliente'";
    
    $enviaquery = mysqli_query($link, $sql);
    $retorno = mysqli_fetch_array($enviaquery) [0];

    //validar o retorno se cpf existe
    if ($retorno == 1)
    {
        echo "<script>alert('O CPF informado já possui cadastro!');</script>";
        echo "<script>window.location.href = 'cadastro_cliente.php';</script>";
    }
    else 
    {
        // CASO FUNCIONÁRIO NÃO ESTEJA CADASTRADO
        $sql = "INSERT INTO clientes (CLI_NOME, CLI_CPF, CLI_TEL, CLI_DATANASCIMENTO, CLI_ATIVO)
        VALUES ('$nomecliente', '$cpfcliente', '$telefonecliente', '$datanascimento', $ativocliente)";
        
        $enviaquery = mysqli_query($link, $sql);
        
        echo ("<script>window.alert('Cadastro realizado com sucesso!');</script>");
        echo "<script>window.location.href = 'backoffice.php';</script>";
        exit();
    }
}

?>

<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="css/cadastro_cliente.css">
    <link rel="stylesheet" href="css/global.css">
    <title>Cadastro de cliente</title>
</head>

    <body>
        <form id="login" class="form1" action="cadastro_cliente.php" method="post">
           
            <h2> Cadastro de cliente </h2>
            <br>
            <input type="text" name="txtNome" placeholder="Nome do cliente" required>
            <input type="text" name="txtCPF" placeholder="CPF" required>
            <input type="number" name="txtTelefone" placeholder="Telefone" required>
            <input type="date" name="txtDatanascimento" required>
            <br>
            <label>
                <input type="radio" name="rbAtivo" value="1" checked required> Ativo
                <input type="radio" name="rbAtivo" value="0" required> Inativo
            </label>
            <br>
            <button type="submit">Cadastrar</button>
        </form>    
    </body>

</html>