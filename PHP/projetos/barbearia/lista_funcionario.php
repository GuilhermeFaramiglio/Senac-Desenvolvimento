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

//traz os funcionarios do banco
$sql = "SELECT * FROM funcionarios INNER JOIN usuarios ON USU_FK_FUNC_ID = FUNC_ID";
$queryfun = mysqli_query($link, $sql);

// AQUI FILTRA AS MINHAS ESCOLHAS
$ativo = 1;
// echo($ativo);
// AGORA FUNÇÕES DE CADA CLICK
if($_SERVER['REQUEST_METHOD'] == 'POST'){
    $ativo = $_POST['filtro'];
    echo($ativo);
    if($ativo == 1){
        $sql = "SELECT * FROM usuarios 
        INNER JOIN funcionarios ON USU_FK_FUNC_ID = FUNC_ID 
        WHERE USU_ATIVO = 1;";
        $enviaquery = mysqli_query($link, $sql);
    }
        $sql = "SELECT * FROM usuarios 
        INNER JOIN funcionarios ON USU_FK_FUNC_ID = FUNC_ID 
        WHERE USU_ATIVO = 0;";
        $enviaquery = mysqli_query($link, $sql);
    }
    else{
        $sql = "SELECT * FROM usuarios INNER JOIN funcionarios ON USU_FK_FUNC_ID = FUNC_ID;";
        $enviaquery = mysqli_query($link, $sql);
    }


?>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Lista de Funcionários</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <link rel="stylesheet" href="css/lista_funcionario.css">
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
        <h1>Lista de Funcionários</h1>
        <table class="table table-bordered table-striped">
            <!-- CRIAÇÃO DE FILTRO DE TABLE -->
            <form action='lista_funcionario.php' method='post'>
                <div class='filtro'>
                    <input type='radio' name='filtro' value='1' required onclick='submit()' <?= $ativo == '1'?'checked':''?>>Ativos 
                    <input type='radio' name='filtro' value='0' required onclick='submit()' <?= $ativo == '0'?'checked':''?>>Inativos 
                    <input type='radio' name='filtro' value='2' required onclick='submit()' <?= $ativo == '2'?'checked':''?>>Todos 

                </div>
            </form>

            <thead class="table-dark">
                <tr>
                    <th><i class="fas fa-id-card"></i> ID</th>
                    <th><i class="fas fa-user"></i> Nome</th>
                    <th><i class="fas fa-id-badge"></i> CPF</th>
                    <th><i class="fas fa-briefcase"></i> Função</th>
                    <th><i class="fas fa-phone"></i> Telefone</th>
                    <th><i class="fas fa-toggle-on"></i> Status Func.</th>
                    <th><i class="fas fa-user-circle"></i> Usuário do Sistema</th>
                    <th><i class="fas fa-check-circle"></i> Status Usuário</th>
                    <th><i>Ações</i></th>
                </tr>

                <!-- PHP -->
                <?php while ($tbl = mysqli_fetch_array($queryfun)) { ?> 
                    <tr>
                        <td><?=$tbl[0]?></td> <!-- ID do Funcionário -->
                        <td><?=$tbl[1]?></td> <!-- Nome do Funcionário -->
                        <td><?=$tbl[2]?></td> <!-- CPF do Funcionário -->
                        <td><?=$tbl[3]?></td> <!-- Função do Funcionário -->
                        <td><?=$tbl[4]?></td> <!-- Telefone do Funcionário -->
                        <td><?=$tbl[5] == 1? 'Ativo':'Inativo'?></td> <!-- Ativo/Inativo do Funcionário -->
                        <td><?=$tbl[7]?></td> <!-- Nome do Usuário -->    
                        <td><?=$tbl[10] == 1? 'Ativo':'Inativo'?></td> <!-- Senha do Usuário  -->   
                        <td>
                            <a href='edita_funcionario.php?id=<?= $tbl[0]?>'><button>Editar</button></a>
                        </td>
                    
                    </tr>
                <?php } ?>                
            </thead>
        </table>
    </div>
    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>