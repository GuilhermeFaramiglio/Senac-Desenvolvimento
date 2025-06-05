<?php

include('utils/conectadb.php');
session_start();


if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $usuario = $_POST['txtUsuario'];
    $senha = $_POST['txtSenha'];

    //coleta do nome do funcionario
    $sqlfun = "SELECT USU_FK_FUNC_ID FROM usuarios 
    WHERE USU_LOGIN = '$usuario' AND USU_SENHA = '$senha'";
    
    $enviaquery2 = mysqli_query($link, $sqlfun);
    $idfuncionario = mysqli_fetch_array($enviaquery2) [0];
    
    //-------------------------------------------------------------------

    //verifica usuario e senha se existe
    $sql = "SELECT COUNT(USU_ID) FROM usuarios 
    WHERE USU_LOGIN = '$usuario' AND USU_SENHA = '$senha'";
    
    $enviaquery1 = mysqli_query($link, $sql);
    $retorno = mysqli_fetch_array($enviaquery1) [0];

    //verifica se ativo ou não
    $sqlativo = "SELECT USU_ATIVO FROM usuarios 
    WHERE USU_LOGIN = '$usuario' AND USU_SENHA = '$senha'";

    $enviaquery3 = mysqli_query($link, $sqlativo);
    $ativo = mysqli_fetch_array($enviaquery3) [0];

    //validar o retorno se existe login e se ativo
    if ($retorno == 1 && $ativo == 1)
    {
        $_SESSION['idfuncionario'] = $idfuncionario;
        Header ("Location: backoffice.php");
    }
    else if ($retorno == 1 && $ativo == 0) {
        echo "<script>alert('Usuário inativo!');</script>";
        echo "<script>window.location.href = 'login.php';</script>";
    } 
    else 
    {
        echo "<script>alert('Usuário ou senha incorretos!');</script>";
        echo "<script>window.location.href = 'login.php';</script>";
    }
}

?>

<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="css/login.css">
    <title>Login</title>
</head>

    <body>
        <form id="login" class="form1" action="login.php" method="post">
           
            <h2> LOGIN </h2>
            <br>
            <input type="text" name="txtUsuario" placeholder="Usuário" required>
            <input type="password" name="txtSenha" placeholder="Senha">
            <br>
            <br>
            <button type="submit">Entrar</button>
        </form>    
    </body>

</html>