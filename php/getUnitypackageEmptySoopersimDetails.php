<?php
	require_once('dbconncetion.php');
	$rootDir = $_SERVER['DOCUMENT_ROOT']."unitydemo/";

	if ($_SERVER['REQUEST_METHOD'] === 'GET') {
		$conn = GetConnection();
		
		if ($conn) {
			//userid, ssid
			$unityPackageDirUrl = $rootDir."\\media\\players\\%s\\soopersims\\%s\\object\\unity_package\\";
			$query = "SELECT id, user_id, ss_id, sso_title,unitypackage_filename, unitypackage_filesize FROM soopersims ORDER BY id ASC";
			
			$result = mysqli_query($conn, $query);
			$response = array();
			
			if (mysqli_num_rows($result) > 0) {
			    while($row = mysqli_fetch_assoc($result)) {
					$fileUrl = sprintf($unityPackageDirUrl,$row["user_id"],$row["ss_id"]).$row["unitypackage_filename"];
					if(!file_exists($fileUrl)) {
						$row["unityPackageDirUrl"] = $fileUrl;
						array_push($response, $row);
					}
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