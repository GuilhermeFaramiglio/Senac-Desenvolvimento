<?php
session_start();
include 'conectadb.php';

// Coleta aleatória os nomes registrados no banco
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_POST['letra'])) {
        $letra = $_POST['letra'] ?? '';

        if (!empty($letra)) {
            $letra = mysqli_real_escape_string($link, $letra);
            $sql = "SELECT DADOS_NOMES FROM dados WHERE DADOS_NOMES LIKE '$letra%' ORDER BY RAND() LIMIT 1";
            $result = mysqli_query($link, $sql);

            if ($result && mysqli_num_rows($result) > 0) {
                $row = mysqli_fetch_assoc($result);
                $nomeAleatorio = $row['DADOS_NOMES'];
                $_SESSION['nomeAleatorio'] = $nomeAleatorio;
            } else {
                echo "<script>
                        alert('Nenhum nome encontrado para a letra selecionada.');
                      </script>";
            }
        }
    }

    // Cadastra as informações do formulário no banco
    if (!empty($_POST['nome']) && isset($_POST['cadastrar'])) {
        $nome = mysqli_real_escape_string($link, $_POST['nome']);
        mysqli_query($link, "INSERT INTO formulario_teste (FORM_NOME) VALUES ('$nome')");
        echo "<script>
                alert('Cadastro realizado com sucesso!');
              </script>";
    }
}
?>

<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Formulario tester</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet">
</head>
<body>
    <div class="d-flex justify-content-center align-items-start vh-100">
        <div class="container mt-4" style="max-width: 400px;">
            <h2 class="text-center mb-4">Formulario tester</h2>
            <form id="formulario" action="index.php" method="post" class="needs-validation" novalidate>
                <div class="mb-3">
                    <label for="nome" class="form-label">Escolha seu nome</label>
                    <div class="input-group">
                        <label for="letra" class="form-label me-2">1ª letra:</label>
                        <select class="form-select" id="letra" name="letra" required style="max-width: 80px;">
                            <option value="" disabled selected></option>
                            <?php
                            $letras = range('A', 'Z');
                            foreach ($letras as $letra) {
                                $selected = (isset($_POST['letra']) && $_POST['letra'] === $letra) ? 'selected' : '';
                                echo "<option value='$letra' $selected>$letra</option>";
                            }
                            ?>
                        </select>
                        <input type="text" name="nome" id="nome" class="form-control ms-2" value="<?php echo isset($_SESSION['nomeAleatorio']) ? $_SESSION['nomeAleatorio'] : ''; ?>" readonly>
                        <button type="button" class="btn btn-outline-secondary ms-2 shuffle-btn" title="Gerar nome aleatório">
                            <i class="bi bi-shuffle"></i>
                        </button>
                        <script>
                            document.querySelector('.shuffle-btn').addEventListener('click', function () {
                                const letra = document.getElementById('letra').value;
                                if (letra) {
                                    fetch('index.php', {
                                        method: 'POST',
                                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                                        body: `letra=${encodeURIComponent(letra)}`
                                    })
                                    .then(response => response.text())
                                    .then(data => {
                                        const parser = new DOMParser();
                                        const doc = parser.parseFromString(data, 'text/html');
                                        const nomeAleatorio = doc.querySelector('#nome').value;
                                        document.getElementById('nome').value = nomeAleatorio || '';
                                    })
                                    .catch(error => console.error('Erro:', error));
                                } else {
                                    alert('Selecione uma letra antes de gerar um nome.');
                                }
                            });
                        </script>
                    </div>
                </div>
                <script>
                    document.querySelector('.shuffle-btn').addEventListener('click', function () {
                        const letra = document.getElementById('letra').value;
                        if (letra) {
                            fetch(`get_nome.php?letra=${letra}&random=true`)
                                .then(response => {
                                    if (!response.ok) {
                                        throw new Error('Erro na resposta do servidor');
                                    }
                                    return response.json();
                                })
                                .then(data => {
                                    const nomeInput = document.getElementById('nome');
                                    nomeInput.value = data.nome || '';
                                })
                                .catch(error => console.error('Erro:', error));
                        } else {
                            alert('Selecione uma letra antes de gerar um nome.');
                        }
                    });
                </script>
                <br>
                <button type="submit" name="cadastrar" class="btn btn-primary">Cadastrar</button>
            </form>
        </div>
    </div>

    <!-- Bootstrap JS (optional, for interactivity) -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
