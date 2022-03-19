<?php

	$chunkFileName = isset($_POST["currentChunkNumber"]) ? trim($_POST["currentChunkNumber"]) : "";
	$currentChunkNumber = isset($_POST["currentChunkNumber"]) ? trim($_POST["currentChunkNumber"]) : "";
	$totalNumberOfChunks = isset($_POST["totalNumberOfChunks"]) ? trim($_POST["totalNumberOfChunks"]) : "";
	$byteArray = isset($_FILES["byteArray"]) ? $_FILES["byteArray"] : "";
	$totalFileSize = isset($_POST["fileSize"]) ? trim($_POST["fileSize"]) : "";
	$username = isset($_POST["username"]) ? trim($_POST["username"]) : "";
	$mergeChunkStatus = isset($_POST["mergeChunkStatus"]) ? trim($_POST["mergeChunkStatus"]) : "";
	$rollbackStatus = isset($_POST["rollbackStatus"]) ? trim($_POST["rollbackStatus"]) : "";
	$parent_dir = "temp_unity_files/";

	//Move chunk php temp folder to users folder
	function moveUploadedChunkFile($temp_path, $move_file_dir, $file_name) {
		if (is_dir($move_file_dir)) {
			return move_uploaded_file($temp_path, $move_file_dir.$file_name);
		}
		else {
			if (mkdir($move_file_dir)) {
				return move_uploaded_file($temp_path, $move_file_dir.$file_name);
			}
			else {
				return false;
			}
		}
	}

	// After marge all chunks, move target unity package to target folder
	function moveTargetFileToTargetFolder($target_file_temp_path, $target_file_dir, $target_file_name) {
		if (is_dir($target_file_dir)) {
			return rename($target_file_temp_path, $target_file_dir.$target_file_name);
		}
		else {
			if (mkdir($target_file_dir)) {
				return rename($target_file_temp_path, $target_file_dir.$target_file_name);
			}
			else {
				return false;
			}
		}
	}

	// Delete all chunks after marge or if any error found
	function deleteChunkFiles($files_dir) {
		$files = array_diff(scandir($files_dir), array('.','..'));
	    foreach ($files as $file) { 
	    	if(unlink($files_dir.$file)){
	    		unset($file);
	    	}
	    }
	    
	    $files = array_diff(scandir($files_dir), array('.','..'));

	    if (empty($files)) {
	    	return rmdir($files_dir);
	    }
	    else {
	    	return false;
	    }
	}

	// Merge all chunk files
	function mergeChunkFiles($target_file_name, $chunk_file_dir, $target_file_temp_path) {
		$files = array_diff(scandir($chunk_file_dir), array('.','..',$target_file_name));
		sort($files);

		$final = fopen($target_file_temp_path, 'w');
		foreach ($files as $file) {
			if(($chunk_file_dir.$file != $target_file_temp_path) && (filesize($chunk_file_dir.$file) > 0)) {
				$myfile = fopen($chunk_file_dir.$file, "r");
				$buff = fread($myfile,filesize($chunk_file_dir.$file));
			    $write = fwrite($final, $buff);
			    fclose($myfile);
			}
		}
		fclose($final);
	}

	// Response Status:
	// 0-Invalid request method, 1-Chunk file uploaded successfully, 2-Error in created parent directory,
	// 3-Chunk file not uploaded, 4-Error in merge target file, 5-Target file saved successfully
	// 6-Rollback successfully
	if (!empty($chunkFileName) && !empty($currentChunkNumber) && !empty($totalNumberOfChunks)
	 && !empty($byteArray) && !empty($username)) {
		$parent_dir = "temp_unity_files/";
		$chunk_file_dir = $parent_dir.$username."/";
		$temp_path = $byteArray['tmp_name'];

		if (is_dir($parent_dir)) {
			if(moveUploadedChunkFile($temp_path, $chunk_file_dir, $chunkFileName)) {
				$response = array("status" => 1, "message" => $chunkFileName." file uploaded successfully");
			}
			else {
				$response = array("status" => 3, "message" => $chunkFileName." file not uploaded");
			}
		}
		else {
			if(mkdir($parent_dir)) {
				if(moveUploadedChunkFile($temp_path, $chunk_file_dir, $chunkFileName)) {
					$response = array("status" => 1, "message" => $chunkFileName." file uploaded successfully");
				}
				else {
					$response = array("status" => 3, "message" => $chunkFileName." file not uploaded");
				}
			}
			else {
				$response = array("status" => 2, "message" => "Parent directory not created");
			}
		}
	}
	elseif (!empty($mergeChunkStatus) && !empty($totalFileSize) && !empty($username)) {
		$target_file_name ="target_file.unitypackage";
	 	$chunk_file_dir = $parent_dir.$username."/";
	 	$target_file_temp_path = $parent_dir.$username."/".$target_file_name;
	 	$target_file_dir = $username."/";

	 	mergeChunkFiles($target_file_name, $chunk_file_dir, $target_file_temp_path);

		if (filesize($target_file_temp_path) == $totalFileSize) {
			if (moveTargetFileToTargetFolder($target_file_temp_path, $target_file_dir, $target_file_name)) {
				if(deleteChunkFiles($chunk_file_dir)) {
					$response = array("status" => 5, "message" => "Target file saved successfully");
				}
				else {
					$response = array("status" => 4, "message" => "Target file not saved");
				}
			}
			else {
				$response = array("status" => 4, "message" => "Target file not save. Try Again");
			}
		}
		else {

			deleteChunkFiles($chunk_file_dir);
			$response = array("status" => 4, "message" => "Target file size doesn't match with actual file size. Try Again");
		}

	}
	elseif (!empty($rollbackStatus) && !empty($username)) {
		$chunk_file_dir = $parent_dir.$username."/";
		deleteChunkFiles($chunk_file_dir);
		$response = array("status" => 6, "message" => "Some error occurred. Try Again");
	}
	else {
		$response = array("status" => 0, "message" => "Invalid request method");
	}

	header('Content-Type: application/json');
	echo json_encode($response);
?>