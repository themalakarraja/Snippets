<?php
	require_once('dbconncetion.php');
	
	if ($_SERVER['REQUEST_METHOD'] === 'GET') {
		
		// ------- SERVER URLS -------
		
		$STATIC_CONTENT_SERVER_URL = $_SERVER['DOCUMENT_ROOT']."/unitydemo/";
		
		// Server folder urls
		$MEDIA_SRC_URL = $STATIC_CONTENT_SERVER_URL."media/";
        $PLAYER_FOLDER_URL = $MEDIA_SRC_URL."players/";

        $SOOPERSIM_FOLDER_NAME = "soopersims";

        $NEW_FOLDER_NAME = "5";

        $SOOPERSIM_OBJECT_ANDROID_FOLDER_NAME = "android";
        $SOOPERSIM_OBJECT_IOS_FOLDER_NAME = "ios";
        $SOOPERSIM_OBJECT_WEBGL_FOLDER_NAME = "webgl";
        $SOOPERSIM_OBJECT_UNITY_PACKAGE_FOLDER_NAME = "unity_package";

        $SOOPERSIM_OBJECT_ANDROID_PREFIX = "_android.unity3d";
        $SOOPERSIM_OBJECT_IOS_PREFIX = "_ios.unity3d";
        $SOOPERSIM_OBJECT_WEBGL_PREFIX = "_webgl.unity3d";
        $SOOPERSIM_OBJECT_UNITY_PACKAGE_PREFIX = ".unitypackage";
		
		$SOOPERSIM_OBJECT_FOLDER_URL = $PLAYER_FOLDER_URL."%s/".$SOOPERSIM_FOLDER_NAME."/%s/object/";

		$SOOPERSIM_ANDROID_ASSET_FOLDER_URL = $SOOPERSIM_OBJECT_FOLDER_URL.$SOOPERSIM_OBJECT_ANDROID_FOLDER_NAME."/";
		$SOOPERSIM_IOS_ASSET_FOLDER_URL = $SOOPERSIM_OBJECT_FOLDER_URL.$SOOPERSIM_OBJECT_IOS_FOLDER_NAME."/";
		$SOOPERSIM_WEBGL_ASSET_FOLDER_URL = $SOOPERSIM_OBJECT_FOLDER_URL.$SOOPERSIM_OBJECT_WEBGL_FOLDER_NAME."/";
		$SOOPERSIM_UNITY_PACKAGE_ASSET_FOLDER_URL = $SOOPERSIM_OBJECT_FOLDER_URL.$SOOPERSIM_OBJECT_UNITY_PACKAGE_FOLDER_NAME."/";

		$SOOPERSIM_NEW_ANDROID_ASSET_FOLDER_URL = $SOOPERSIM_OBJECT_FOLDER_URL.$NEW_FOLDER_NAME."/".
					$SOOPERSIM_OBJECT_ANDROID_FOLDER_NAME."/";
		$SOOPERSIM_NEW_IOS_ASSET_FOLDER_URL = $SOOPERSIM_OBJECT_FOLDER_URL.$NEW_FOLDER_NAME."/".
					$SOOPERSIM_OBJECT_IOS_FOLDER_NAME."/";
		$SOOPERSIM_NEW_WEBGL_ASSET_FOLDER_URL = $SOOPERSIM_OBJECT_FOLDER_URL.$NEW_FOLDER_NAME."/".
					$SOOPERSIM_OBJECT_WEBGL_FOLDER_NAME."/";
		$SOOPERSIM_NEW_UNITY_PACKAGE_ASSET_FOLDER_URL = $SOOPERSIM_OBJECT_FOLDER_URL.$NEW_FOLDER_NAME."/".
					$SOOPERSIM_OBJECT_UNITY_PACKAGE_FOLDER_NAME."/";

		//QUERY
		$query = "SELECT id, user_id, ss_id FROM soopersims ORDER BY id ASC";
		
		$conn = GetConnection();

		$androidFileNotFoundArr = array();
		$iosFileNotFoundArr = array();
		$webglFileNotFoundArr = array();
		$unitypackageFileNotFoundArr = array();
		
		if ($conn) {
			$result = mysqli_query($conn, $query);
			$response = array();

			if (mysqli_num_rows($result) > 0) {
			    while($row = mysqli_fetch_assoc($result)) {
					$androidFileUrl = sprintf($SOOPERSIM_ANDROID_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]).
								$row["ss_id"].$SOOPERSIM_OBJECT_ANDROID_PREFIX;
					$iosFileUrl = sprintf($SOOPERSIM_IOS_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]).
								$row["ss_id"].$SOOPERSIM_OBJECT_IOS_PREFIX;
					$webglFileUrl = sprintf($SOOPERSIM_WEBGL_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]).
								$row["ss_id"].$SOOPERSIM_OBJECT_WEBGL_PREFIX;
					$unitypackageFileUrl = sprintf($SOOPERSIM_UNITY_PACKAGE_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]).
								$row["ss_id"].$SOOPERSIM_OBJECT_UNITY_PACKAGE_PREFIX;

			    	if (!file_exists($androidFileUrl)) {
			    		array_push($androidFileNotFoundArr, $androidFileUrl);
			    	}
			    	else {
			    		mkdir(sprintf($SOOPERSIM_NEW_ANDROID_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]), 0777, true);
			    		copy($androidFileUrl, sprintf($SOOPERSIM_NEW_ANDROID_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]).
								$row["ss_id"].$SOOPERSIM_OBJECT_ANDROID_PREFIX);
			    	}

			    	if (!file_exists($iosFileUrl)) {
			    		array_push($iosFileNotFoundArr, $iosFileUrl);
			    	}
			    	else {
			    		mkdir(sprintf($SOOPERSIM_NEW_IOS_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]), 0777, true);
			    		copy($iosFileUrl, sprintf($SOOPERSIM_NEW_IOS_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]).
								$row["ss_id"].$SOOPERSIM_OBJECT_IOS_PREFIX);
			    	}

			    	if (!file_exists($webglFileUrl)) {
			    		array_push($webglFileNotFoundArr, $webglFileUrl);
			    	}
			    	else {
			    		mkdir(sprintf($SOOPERSIM_NEW_WEBGL_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]), 0777, true);
			    		copy($webglFileUrl, sprintf($SOOPERSIM_NEW_WEBGL_ASSET_FOLDER_URL,$row["user_id"],$row["ss_id"]).
								$row["ss_id"].$SOOPERSIM_OBJECT_WEBGL_PREFIX);
			    	}

			    	if (!file_exists($unitypackageFileUrl)) {
			    		array_push($unitypackageFileNotFoundArr, $unitypackageFileUrl);
			    	}
			    	else {
			    		mkdir(sprintf($SOOPERSIM_NEW_UNITY_PACKAGE_ASSET_FOLDER_URL,$row["user_id"],
			    			$row["ss_id"]), 0777, true);
			    		copy($unitypackageFileUrl, sprintf($SOOPERSIM_NEW_UNITY_PACKAGE_ASSET_FOLDER_URL,$row["user_id"],
			    			$row["ss_id"]).$row["ss_id"].$SOOPERSIM_OBJECT_UNITY_PACKAGE_PREFIX);
			    	}
			    }

			    if (!empty($androidFileNotFoundArr) || isset($androidFileNotFoundArr)) {
			    	array_push($response, $androidFileNotFoundArr);
				}

				if (!empty($iosFileNotFoundArr) || isset($iosFileNotFoundArr)) {
				   	array_push($response, $iosFileNotFoundArr);
				}

				if (!empty($webglFileNotFoundArr) || isset($webglFileNotFoundArr)) {
				   	array_push($response, $webglFileNotFoundArr);
				}

				if (!empty($unitypackageFileNotFoundArr) || isset($unitypackageFileNotFoundArr)) {
				   	array_push($response, $unitypackageFileNotFoundArr);
				}
			}
			else {
				$response = array("status" => 3, "message" => "No Data Found");
			}
		}
		else {
			$response = array("status" => 2, "message" => "Error connecting database");
		}
	}
	else {
		$response = array("status" => 0, "message" => "Invalid request method");
	}

	header('Content-Type: application/json');
	echo json_encode($response);
?>