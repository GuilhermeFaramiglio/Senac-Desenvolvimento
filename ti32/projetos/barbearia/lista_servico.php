<?php
// CONEXÃO COM O BANCO
include("utils/conectadb.php");

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

$sqlcat = "SELECT * FROM servicos WHERE SERV_ATIVO = 1";
$enviaquery = mysqli_query($link, $sqlcat);

$ativo = 1;

if($_SERVER['REQUEST_METHOD'] == 'POST'){
    $ativo = $_POST['filtro'];
   
    
    if($ativo == 1){
        $sql = "SELECT * FROM servicos WHERE SERV_ATIVO = 1";
        $enviaquery = mysqli_query($link, $sql);
    }
    else if($ativo == 0){
        $sql = "SELECT * FROM servicos WHERE SERV_ATIVO = 0";
        $enviaquery = mysqli_query($link, $sql);
    }
    else{
        $sql = "SELECT * FROM servicos";
        $enviaquery = mysqli_query($link, $sql);
    }
}
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
        <h1>Lista de Serviços</h1>
        <table class="table table-bordered table-striped">
            <thead class="table-dark">
                <tr>
                    <th><i class="fas fa-id-card"></i> ID</th>
                    <th><i class="fas fa-user"></i> NOME</th>
                    <th><i class="fas fa-id-badge"></i> PREÇO</th>
                    <th><i class="fas fa-clock"></i> DURAÇÃO</th>
                    <th><i class="fas fa-calendar-alt"></i> STATUS</th>
                    <th><i class="fas fa-check-circle"></i> IMAGEM</th>
                    <th><i>Ações</i></th>
                </tr>

                <!-- PHP -->
                <?php while ($tbl = mysqli_fetch_array($enviaquery)){ ?>
                
                <tr class='linha'>
                    <td><?=$tbl[0]?></td> <!--COLETA ID [0] -->
                    <td><?=$tbl[1]?></td> <!--COLETA NOME [1]-->
                    <td>R$ <?=$tbl[2]?></td> <!--COLETA PREÇO [2]-->
                    <td><?= $tbl[3] <= 59? $tbl[3]." Minutos": ($tbl[3] / 60)." Hora(s)"?> </td> <!--COLETA TEMPO [3]-->
                    <td><?=$tbl[4] == 1? 'ATIVO':'INATIVO'?></td> <!--COLETA ATIVO DO CAT [4]-->
                    <td><img id='cat_imagem' src='data:img/jpeg;base64,<?=$tbl[5]?>' width=100 height=100></td>
                 
                    <td>
                        <a href='edita_servico.php?id=<?= $tbl[0]?>'><button>Editar</button></a>
                    </td>
                </tr>

                <?php } ?>              
            </thead>
        </table>
    </div>
    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>