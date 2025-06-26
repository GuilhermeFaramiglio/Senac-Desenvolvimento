<?php
include('utils/conectadb.php');

session_start();

if (isset($_SESSION['idfuncionario'])) {
    $idfuncionario = $_SESSION['idfuncionario'];
    $sql = "SELECT FUNC_NOME FROM funcionarios WHERE FUNC_ID = '$idfuncionario'";
    $enviaquery = mysqli_query($link, $sql);
    $nomeusuario = mysqli_fetch_array($enviaquery)[0];
} else {
    echo "<script>alert('Usuário não logado!');</script>";
    echo "<script>window.location.href = 'login.php';</script>";
}

//traz os clientes do banco
$sql = "SELECT * FROM clientes ORDER BY CLI_ID ASC";
$queryfun = mysqli_query($link, $sql);

?>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Lista de Clientes</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <link rel="stylesheet" href="css/lista_cliente.css">
    <link rel="stylesheet" href="css/global.css"> 
</head>
<body>
    <div>
        <header>
            <h1><?php echo $nomeusuario; ?></h1>
            <nav>
            <div class="logout">
            
            <a href="backoffice.php" class=""><button>Voltar</button></a>
            </div>
            </nav>
        </header>
    </div>
    <div class="container">
        <h1>Lista de Clientes</h1>
        <table class="table table-bordered table-striped">
            <thead class="table-dark">
                <tr>
                    <th><i class="fas fa-id-card"></i> ID</th>
                    <th><i class="fas fa-user"></i> Nome</th>
                    <th><i class="fas fa-id-badge"></i> CPF</th>
                    <th><i class="fas fa-phone"></i> Telefone</th>
                    <th><i class="fas fa-calendar-alt"></i> Data de Nascimento</th>
                    <th><i class="fas fa-check-circle"></i> Status Cliente</th>
                    <th><i>Ações</i></th>
                </tr>

                <!-- PHP -->
                <?php while ($tbl = mysqli_fetch_array($queryfun)) { ?> 
                    <tr>
                        <td><?=$tbl[0]?></td> <!-- ID do Cliente -->
                        <td><?=$tbl[1]?></td> <!-- Nome do Cliente -->
                        <td><?=$tbl[2]?></td> <!-- CPF do Cliente -->
                        <td><?=$tbl[3]?></td> <!-- Função do Cliente -->
                        <td><?=$tbl[4]?></td> <!-- Telefone do Cliente -->
                        <td><?=$tbl[5] == 1? 'Ativo':'Inativo'?></td> <!-- Ativo/Inativo do Cliente --> 
                        <td>
                            <a href='edita_cliente.php?id=<?= $tbl[0]?>'><button>Editar</button></a>
                        </td>
                    
                    </tr>
                <?php } ?>                
            </thead>
        </table>
    </div>
    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>