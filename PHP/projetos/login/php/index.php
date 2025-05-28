<?php
session_start();

if (isset($_SESSION['usuario'])) {
    $usuario = $_SESSION['usuario'];
}
else {
    echo "<script>alert('Usuário não logado!');</script>";
    echo "<script>window.location.href = '../login.html';</script>";
}
?>

<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home</title>
</head>
<body>
    <h1>Bem-vindo!</h1>
    <p>Olá, <?php echo($_SESSION['usuario']) ?>. Você está logado.</p>

    <form action="logout.php" >

        <input type="submit" value="SAIR">

    </form>

</body>
</html>