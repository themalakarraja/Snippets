<?php

if($_SERVER['REQUEST_METHOD'] == "POST"){
    // Connect to DB
    $sooperpop_id = isset($_POST['sooperpop_id'])  ? trim($_POST['sooperpop_id']) : "";
    $password = isset($_POST['password'])  ? trim($_POST['password']) : "";

    if(!empty($sooperpop_id)) {
        include("/../config/open_db_connection.php");
        $sql = "SELECT `sooperpop_id` FROM `players` WHERE `sooperpop_id` = '$sooperpop_id' AND `password` = '$password'";
        $result = mysqli_query($con, $sql);
        if($row = mysqli_fetch_assoc($result)) {
            $json = array("status" => 1, "message" => "Valid credentials!");
        }
        else {
            $json = array("status" => 2, "message" => "Invalid credentials!");
        }
        include("/../config/close_db_connection.php");
    }
    else {
        $json = array("status" => 0, "message" => "Invalid request parameters!!");
    }
}
else {
    $json = array("status" => 0, "message" => "Invalid request method!!");
}

/* Output header */
header('Content-type: application/json');
echo json_encode($json);

?