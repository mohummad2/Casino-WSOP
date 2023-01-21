<?php
    if(!isset($_POST["action"])) 
    {
        echo ("NO DATA RECIEVED");
        return;
    }

    $action = $_POST["action"];
   
    $data = $action;
    $myFile = $_POST["fname"];
     echo $myFile;

    if(file_put_contents("players/" . $myFile, $data))
        echo (" success ");
    else
        echo ("failed to write file");
        
 ?>