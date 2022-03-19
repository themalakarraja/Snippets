<?php

if($_SERVER['REQUEST_METHOD'] == "POST") {
	$username = isset($_POST["username"]) ? trim($_POST["username"]) : "";
	$chunkByteArray = isset($_FILES["chunk_byte_array"]) ? $_FILES["chunk_byte_array"] : "";
	$currentChunkNumber = isset($_POST["current_chunk_number"]) ? trim($_POST["current_chunk_number"]) : "";
	$totalChunksNumber = isset($_POST["total_chunks_number"]) ? trim($_POST["total_chunks_number"]) : "";
	$startMerge = isset($_POST["start_merge"]) ? trim($_POST["start_merge"]) : "";
	$package = isset($_POST["package"]) ? trim($_POST["package"]) : "";
	$totalFileSize = isset($_POST["total_file_size"]) ? trim($_POST["total_file_size"]) : "";
	$startRollback = isset($_POST["start_rollback"]) ? trim($_POST["start_rollback"]) : "";
	$tempDir = $_SERVER['DOCUMENT_ROOT']."/unitydemo/temp_unity_files/";

	//Check if directory is exist return true, else create new directory and returns bool
	function createDirectory($directoryPath) {
		if (!is_dir($directoryPath))
			return mkdir($directoryPath, 0777, true);
		else
			return true;
	}

	function targetFileDirectoryPath($packageName, $username) {
		$targetFileDirectory = "";
		switch ($packageName) {
		    case "android":
		    	$targetFileDirectory = $_SERVER['DOCUMENT_ROOT']."/unitydemo/media/players/".$username."/soopersim/".$username."123/objects/android_package/";
		        break;
		    case "ios":
		        $targetFileDirectory = $_SERVER['DOCUMENT_ROOT']."/unitydemo/media/players/".$username."/soopersim/".$username."123/objects/ios_package/";
		        break;
		    case "webgl":
		        $targetFileDirectory = $_SERVER['DOCUMENT_ROOT']."/unitydemo/media/players/".$username."/soopersim/".$username."123/objects/webgl_package/";
		        break;
	        case "unitypackage":
	        	$targetFileDirectory = $_SERVER['DOCUMENT_ROOT']."/unitydemo/media/players/".$username."/soopersim/".$username."123/objects/unitypackage_package/";
		        break;
		    default:
		        return "";
		        break;
		}
		createDirectory($targetFileDirectory);
        return $targetFileDirectory;
	}

	function mergeChunkFiles($targetFileName, $chunkFileDir, $targetFileTempPath) {
		$files = array_diff(scandir($chunkFileDir), array('.','..',$targetFileName));
		sort($files);
		
		$final = fopen($targetFileTempPath, 'w');
		foreach ($files as $file) {
			if(($chunkFileDir.$file != $targetFileTempPath) && (filesize($chunkFileDir.$file) > 0)) {
				$myfile = fopen($chunkFileDir.$file, "r");
				$buff = fread($myfile,filesize($chunkFileDir.$file));
			    $write = fwrite($final, $buff);
			    fclose($myfile);
			}
		}
		fclose($final);
	}

	function moveTargetFile($targetFileTempPath, $targetFilePath) {
		return rename($targetFileTempPath, $targetFilePath);
	}

	//Remove folder and its inner folder and files
	function removeFolder($folder) {
		rtrim($folder,'/');
		if(is_dir($folder) === true) {
			$folderContents = scandir($folder);
			unset($folderContents[0], $folderContents[1]);
			foreach($folderContents as $content => $contentName) {
				$currentPath = $folder."/".$contentName;
				$filetype = filetype($currentPath);
				if($filetype == 'dir') {
					removeFolder($currentPath);
				}
				else {
					unlink($currentPath);
				}
				unset($folderContents[$content]);
			}
			rmdir($folder);
		}
	}

    if (!empty($username) && !empty($currentChunkNumber) && !empty($totalChunksNumber) && !empty($chunkByteArray)) {
    	$chunkFileDir = $tempDir.$username."/";
    	$chunkFilePath = $chunkFileDir.$currentChunkNumber;
		$tempPath = $chunkByteArray['tmp_name'];

		if (createDirectory($chunkFileDir)) {
			if(move_uploaded_file($tempPath, $chunkFilePath)) {
				$response = array("status" => 1, "message" => $currentChunkNumber." uploaded successfully");
			}
			else {
				$response = array("status" => 3, "message" => $currentChunkNumber." not uploaded");
			}
		}
		else {
			$response = array("status" => 4, "message" => "Chunk file user directory not created");
		}
    }
    elseif (!empty($startMerge) && !empty($totalFileSize) && !empty($username) && !empty($package)) {
		$targetFileName = $username.".unitypackage";
	 	$chunkFileDir = $tempDir.$username."/";
	 	$targetFileTempPath = $tempDir.$username."/".$targetFileName;
	 	$targetFileDir = targetFileDirectoryPath($package, $username);

	 	mergeChunkFiles($targetFileName, $chunkFileDir, $targetFileTempPath);

	 	if (filesize($targetFileTempPath) == $totalFileSize) {
			if (moveTargetFile($targetFileTempPath, $targetFileDir.$targetFileName)) {
				removeFolder($chunkFileDir);
				$response = array("status" => 1, "message" => "Target file saved successfully");
			}
			else {
				$response = array("status" => 4, "message" => "Target file not save. Try Again");
			}
		}
		else {
			removeFolder($chunkFileDir);
			$response = array("status" => 4, "message" => "Target file size doesn't match with actual file size. Try Again");
		}
	}
	elseif (!empty($startRollback) && !empty($username)) {
		$chunkFileDir = $tempDir.$username."/";
		removeFolder($chunkFileDir);
		removeFolder($_SERVER['DOCUMENT_ROOT']."/unitydemo/media/players/".$username."/soopersim/".$username."123/");

		$response = array("status" => 6, "message" => "Some error occurred. Try Again");
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