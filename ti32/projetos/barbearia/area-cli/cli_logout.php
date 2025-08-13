<?php

session_start();
session_destroy();

echo "<script>alert('Você foi desconectado!');</script>";
echo "<script>window.location.href = 'catalogo.php';</script>";

?>