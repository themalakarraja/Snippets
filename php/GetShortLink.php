<?PHP

if($_SERVER['REQUEST_METHOD'] == "POST") {
    $longDynamicLink  = isset($_POST["longDynamicLink"]) ? trim($_POST["longDynamicLink"]) : "";
    $url = "";
    
    if (!empty($longDynamicLink)) {
        $longDynamicLinkArr = array("longDynamicLink" => $longDynamicLink);
        $data_string = json_encode($longDynamicLinkArr);

        $ch = curl_init($url);
        curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");
        curl_setopt($ch, CURLOPT_POSTFIELDS, $data_string);
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_HTTPHEADER, array(
            'Content-Type: application/json',
            'Content-Length: ' . strlen($data_string))
        );

        //execute post
        $result = curl_exec($ch);

        //close connection
        curl_close($ch);

        $result = json_decode($result, true); 

        $response = array("shortLink" => $result['shortLink'], "previewLink" => $result['previewLink']);
    }
    else {
        $response = array("status" => 2, "message" => "Invalid request parameters!!");
    }
}
else {
    $response = array("status" => 0, "message" => "Invalid request method!!");
}

/* Output header */
header('Content-type: application/json');
echo json_encode($response);

?>