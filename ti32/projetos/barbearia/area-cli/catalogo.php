<?php
include '../utils/conectadb.php';
session_start();

// fazer a validação de cliente logado
$nomecliente = null;
if (isset($_SESSION['idcliente'])) {
    $idcliente = intval($_SESSION['idcliente']);
    $query = "SELECT * FROM clientes WHERE CLI_ID = $idcliente";
    $result = mysqli_query($link, $query);
    if ($result && mysqli_num_rows($result) > 0) {
        $cliente = mysqli_fetch_assoc($result);
        $nomecliente = htmlspecialchars($cliente['CLI_NOME']);
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
<style>

</style>
<body>
    <div>
        <header style="display: flex; justify-content: space-between; align-items: center; padding: 24px 40px; background: rgba(25, 34, 74, 0.95); border-radius: 32px; margin: 24px auto 32px auto; max-width: 80%; box-sizing: border-box;">
            <div>
            <?php if (isset($nomecliente)) { ?>
            <h1 class="mb-3" style="margin: 0; font-size: 2rem; font-weight: bold; color: #fff;">Bem vindo <?php echo $nomecliente; ?></h1>
            <?php } else { ?>
            <h1 class="mb-3" style="margin: 0; font-size: 2rem; font-weight: bold; color: #fff;"><?php echo isset($bemvindo) ? $bemvindo : 'Bem Vindo'; ?></h1>
            <?php } ?>
            </div>
            <nav style="display: flex; gap: 16px;">
            <?php if (isset($nomecliente)) { ?>
            <form action='cli_logout.php' method='post' style="display: inline;">
            <button type="submit" class="btn btn-danger">Logoff</button>
            </form>
            <?php } else { ?>
            <a href="cli_login.php" class="btn btn-primary me-2">Login</a>
            <a href="cli_cadastro.php" class="btn btn-success">Cadastrar</a>
            <?php } ?>
            </nav>
        </header>
        <style>
            
        </style>
        
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