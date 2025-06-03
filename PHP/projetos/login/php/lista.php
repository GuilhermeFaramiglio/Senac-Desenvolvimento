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
    <link rel="stylesheet" href="../css/lista.css">
    <title>Lista</title>
</head>
<body>

    <table class="Lista">
        <tr>
            <th>ID</th>
            <th>Nome</th>
        </tr>

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

    </table>
    
</body>
</html>