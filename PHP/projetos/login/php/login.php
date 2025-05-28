<?php

include('conectadb.php');
session_start();


if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $usuario = $_POST['txtUsuario'];
    $senha = $_POST['txtSenha'];

    //comunica com o banco
    $sql = "SELECT COUNT(USUARIOS_ID) FROM usuarios 
    WHERE USUARIO_LOGIN = '$usuario' AND USUARIO_SENHA = '$senha'";
    
    $enviaquery = mysqli_query($link, $sql);
    $retorno = mysqli_fetch_array($enviaquery) [0];

    //validar o retorno
    if ($retorno == 1)
    {
        $_SESSION['usuario'] = $usuario;
        Header ("Location: index.php");
    }
    else 
    {
        echo "<script>alert('Login ou senha incorretos!');</script>";
        echo "<script>window.location.href = '../login.html';</script>"; // Redireciona para a página de login
    }
}

?>