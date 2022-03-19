<?php
	require_once('dbConnection.php');
	$downloadLinkStatus = isset($_POST["download_link_status"]) ? trim($_POST["download_link_status"]) : "";
	$offset = 0;
	$limit = 5;

	if (!empty($downloadLinkStatus)) {
		$conn = GetConnection();
		
		if ($conn) {
			// $query = "SELECT ss_id, user_id, unitypackage_filename FROM soopersims WHERE unitypackage_filename <> '' AND unitypackage_filesize <> '' ORDER BY id ASC";// LIMIT ".$offset.", ".$limit;
			// $query = "SELECT ss_id, user_id, unitypackage_filename FROM soopersims WHERE unitypackage_filesize = '16406'";
			$query = "SELECT ss_id, user_id, unitypackage_filename FROM soopersims WHERE ss_id =  '151020187IC' OR ss_id = '15102018SQ5'";
			
			$result = mysqli_query($conn, $query);
			$response = array();

			if (mysqli_num_rows($result) > 0) {
			    while($row = mysqli_fetch_assoc($result)) {
					array_push($response, $row);
			    }
			}
			else {
				$response = array("status" => 3, "message" => "No Data Found");
			}
		}
		else {
			$response = array("status" => 2, "message" => "Error connecting database");
		}

		closeConnection($conn);
	}
	else {
		$response = array("status" => 0, "message" => "Invalid request method");
	}

	header('Content-Type: application/json');
	echo json_encode($response);
?>