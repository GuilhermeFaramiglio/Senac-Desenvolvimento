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

?>

<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <title>Agendamentos</title>
    <link rel="stylesheet" href="css/global.css">
    <link rel="stylesheet" href="css/backoffice.css">
</head>
<body>
    <div>
        <header>
            <h1>Bem vindo, <?php echo$nomeusuario?></h1>
            <nav>
            <div class="logout" method='post'>
                <form action='logout.php'>
                    <input type="submit" value='SAIR'>
                </form>
            </div>
            </nav>
        </header>
        
        <main>
            <div class="card"> 
                <a href="#"><img src="icons/add9.png" alt=""></a>
                <p>Cadastro de Usuário</p>
            </div>

            <div class="card">
                <a href="#"><img src="icons/th2.png" alt=""></a>
                <p>Lista de Usuários</p>
            </div>

            <div class="card">
                <a href="#"><img src="icons/business.png" alt=""></a>
                <p>Cadastro de Funcionário</p>
            </div>

            <div class="card">
                <a href="#"><img src="icons/group1.png" alt=""></a>
                <p>Lista de Funcionários</p>
            </div>
        </main>
    </div>
</body>
</html>