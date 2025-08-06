<?php
include '../utils/conectadb.php';

// fazer a validação de cliente logado futuramente

$sql = "SELECT * FROM servicos WHERE SERV_ATIVO = 1";
$enviaquery = mysqli_query($link, $sql);


?>

<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <title>Catalogo de serviços</title>
    <link rel="stylesheet" href="../css/catalogo.css">
    <link rel="stylesheet" href="../css/global.css">
</head>
<body>
    <div>
        <header>
            <!-- <h1>Bem vindo, <?php // echo$nomeusuario?></h1> -->
            <h1>Catálogo de Serviços</h1>
            <nav>
            <div class="logout" method='post'>
            <form action='../logout.php' method='post' class="me-2">
            <input type="submit" value='Logoff'>
            </form>
            </div>
            </nav>
        </header>
        
        <main class="catalogo">

            <?php while($retorno = mysqli_fetch_assoc($enviaquery)){ ?>
            
            <div class="card">
                <h3><?php echo $retorno['SERV_NOME']; ?></h3>
                <img src="data:img/jpeg;base64,<?php echo $retorno['SERV_IMAGEM']; ?>">
                <p><?php echo 'R$ ' . number_format($retorno['SERV_PRECO'], 2, ',', '.'); ?></p>
                <p><?php echo $retorno['SERV_TEMPO'] . ' min'; ?></p>

                <a href="agendar.php?id=<?php echo $retorno['SERV_ID']; ?>" class="btn-agendar">Agendar</a>
            </div>
            
            <?php } ?>
        </main>

    </div>
</body>
</html>