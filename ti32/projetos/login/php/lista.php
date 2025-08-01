<?php 

include('conectadb.php');

//pesquisa no banco
$sql = "SELECT * FROM usuarios";
$enviaquery = mysqli_query($link, $sql);

?>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="../css/lista.css">
    <title>Lista</title>
</head>
<body class="container py-5">

    <h1 class="text-center text-primary mb-4">Lista de Usuários</h1>

    <table class="table table-dark table-striped">
        <thead>
            <tr>
                <th scope="col">ID</th>
                <th scope="col">Nome</th>
            </tr>
        </thead>
        <tbody>
            <?php
                while($tbl = mysqli_fetch_array($enviaquery)) {
            ?>
            <tr>
                <!-- coleta id na posição 0 -->
                <td><?=$tbl[0] ?></td>
                <!-- coleta nome na posição 1 -->
                <td><?=$tbl[1] ?></td>
            </tr>
            <?php
                }
            ?>
        </tbody>
    </table>
    <div class="text-center mt-4">
        <a href="index.php" class="btn btn-primary">Voltar para a Página Inicial</a>
    </div>
</body>
</html>