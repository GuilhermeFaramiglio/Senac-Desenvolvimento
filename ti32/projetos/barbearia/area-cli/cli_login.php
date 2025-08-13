<?php
session_start();
include('../utils/conectadb.php');


if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $usuario = $_POST['txtUsuario'];
    $senha = $_POST['txtSenha'];
    // Use prepared statements to prevent SQL injection
    $stmt = $link->prepare("SELECT CLI_ID, CLI_ATIVO FROM CLIENTES WHERE CLI_CPF = ? AND CLI_SENHA = ?");
    $stmt->bind_param("ss", $usuario, $senha);
    $stmt->execute();
    $result = $stmt->get_result();
    $row = $result->fetch_assoc();

    if ($row) {
        $idcliente = $row['CLI_ID'];
        $ativo = $row['CLI_ATIVO'];
        $retorno = 1;
    } else {
        $idcliente = null;
        $ativo = 0;
        $retorno = 0;
    }

    //validar o retorno se existe login e se ativo
    if ($retorno == 1 && $ativo == 1)
    {
        $_SESSION['idcliente'] = $idcliente;
        Header ("Location: catalogo.php");
    }
    else if ($retorno == 1 && $ativo == 0) {
        echo "<script>alert('Usuário inativo!');</script>";
        echo "<script>window.location.href = 'cli_login.php';</script>";
    } 
    else 
    {
        echo "<script>alert('Usuário ou senha incorretos!');</script>";
        echo "<script>window.location.href = 'cli_login.php';</script>";
    }
}

?>

<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="../css/login.css">
    <title>Login</title>
</head>

    <body>
        <form id="login" class="form1" action="cli_login.php" method="post">

            <h2> LOGIN CLIENTE</h2>
            <br>
            <input type="text" name="txtUsuario" placeholder="CPF" required>
            <input type="password" name="txtSenha" placeholder="Senha">
            <br>
            <button type="submit">Entrar</button>
            <br>
            <br>
            <div style="text-align:center; margin-top:10px;">
                <span>É cliente e não tem cadastro? <a href="cli_cadastro.php" class="cad" style="display:inline; color:#007bff; text-decoration:underline;">Cadastre-se Aqui</a></span>
                <br>
                <span>É funcionário? <a href="../login.php" class="cad" style="display:inline; color:#007bff; text-decoration:underline;">Login Aqui</a></span>
            </div>
            <br>
        </form>    
    </body>
</html>