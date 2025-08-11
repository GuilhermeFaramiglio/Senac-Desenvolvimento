<?php
include '../utils/conectadb.php';
session_start();

// fazer a validação de cliente logado
if (isset($_SESSION['idcliente'])) {
    $idcliente = $_SESSION['idcliente'];
    $query = "SELECT * FROM clientes WHERE CLI_ID = $idcliente";
    $result = mysqli_query($link, $query);
    if ($result) {
    $cliente = mysqli_fetch_assoc($result);
    $nomecliente = htmlspecialchars($cliente['CLI_NOME']);
    } else {
        $bemvindo = "Bem Vindo";
    }
}


//coleta serviços
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
            <?php if (isset($nomecliente)) { ?>
                <h1 class="mb-3">Bem vindo <?php echo $nomecliente; ?></h1>
                <nav>
                    <form action='../logout.php' method='post' class="d-inline">
                        <button type="submit" class="btn btn-danger">Logoff</button>
                    </form>
                </nav>
            <?php } else { ?>
                <div>
                    <h1 class="mb-3"><?php echo isset($bemvindo) ? $bemvindo : 'Bem Vindo'; ?></h1>
                    <div>
                        <a href="cli_login.php" class="btn btn-primary me-2">Login</a>
                        <a href="cli_cadastro.php" class="btn btn-success">Cadastrar</a>
                    </div>
                </div>
            <?php } ?>
        </header>
        
        <main class="catalogo">

            <h3>Catálogo de Serviços</h3>
            <br>

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