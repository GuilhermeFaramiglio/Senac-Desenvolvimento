<?php

include('conectadb.php');

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $usuario = $_POST['txtUsuario'];
    $senha = $_POST['txtSenha'];

    //comunica com o banco
    $sql = "SELECT COUNT(USUARIO_LOGIN) FROM usuarios 
    WHERE USUARIO_LOGIN = '$usuario'";
    
    $enviaquery = mysqli_query($link, $sql);
    $retorno = mysqli_fetch_array($enviaquery) [0];

    //validar o retorno
    if ($retorno == 1)
    {
        echo "<script>alert('Usuário já cadastrado');</script>";
        echo "<script>window.location.href = '../login.html';</script>"; // Redireciona para a página de login
    }
    else 
    {
        $sql = "INSERT INTO usuarios (USUARIO_LOGIN, USUARIO_SENHA) 
            VALUES ('$usuario', '$senha')";

        $enviaquery = mysqli_query($link, $sql);        
        echo "<script>alert('Cadastro realizado com sucesso!');</script>";
        echo "<script>window.location.href = '../login.html';</script>"; // Redireciona para a página de login
    }
}

?>