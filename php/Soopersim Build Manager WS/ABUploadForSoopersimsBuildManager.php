<?php

if($_SERVER['REQUEST_METHOD'] == "POST") {
	require_once('dbConnection.php');

    $userId = isset($_POST["user_id"]) ? trim($_POST["user_id"]) : "";

    $chunkByteArray = isset($_FILES["chunk_byte_array"]) ? $_FILES["chunk_byte_array"] : "";
    $currentChunkNumber = isset($_POST["current_chunk_number"]) ? trim($_POST["current_chunk_number"]) : "";
    $totalChunksNumber = isset($_POST["total_chunks_number"]) ? trim($_POST["total_chunks_number"]) : "";
    
    $startMerge = isset($_POST["start_merge"]) ? trim($_POST["start_merge"]) : "";
    $package = isset($_POST["package"]) ? trim($_POST["package"]) : "";
    $totalFileSize = isset($_POST["total_file_size"]) ? trim($_POST["total_file_size"]) : "";
    $ssId = isset($_POST["ss_id"]) ? trim($_POST["ss_id"]) : "";
    $unityVersion = isset($_POST["unity_version"]) ? trim($_POST["unity_version"]) : "";
    
    $startRollback = isset($_POST["start_rollback"]) ? trim($_POST["start_rollback"]) : "";
    
    $rootDir = $_SERVER['DOCUMENT_ROOT'];
    $tempDir = $rootDir."/media/temp/ws_chunk_data/";
    $uploadDir = $rootDir."/media/players/".$userId."/soopersims/".$ssId."/object/".$unityVersion."/";
    $chunkFileDir = !empty($userId) ? $tempDir.$userId."/" : "";

    //Check if directory is exist return true, else create new directory and returns bool
    function createDirectory($directoryPath) {
        if (!is_dir($directoryPath))
            return mkdir($directoryPath, 0777, true);
        else {
            return true;
        }
    }

    function targetFileDirectoryPath($packageName, $uploadDir) {
        $targetFileDirectory = "";
        switch ($packageName) {
            case "android":
                $targetFileDirectory = $uploadDir."android/";
                break;
            case "ios":
                $targetFileDirectory = $uploadDir."ios/";
                break;
            case "webgl":
                $targetFileDirectory = $uploadDir."webgl/";
                break;
            case "unitypackage":
                $targetFileDirectory = $uploadDir."unity_package/";
                break;
            default:
                return "";
                break;
        }

        if (!empty($targetFileDirectory)) {
            createDirectory($targetFileDirectory);
        }
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

    function getTergetFileName($packageName, $ss_id) {
    	$conn = GetConnection();
    	$query = "SELECT android_filename, ios_filename, webgl_filename, unitypackage_filename FROM soopersims WHERE ss_id = '".$ss_id."'";
    	$result = mysqli_query($conn, $query);
    	$fileName;
    	$row;
    	if (mysqli_num_rows($result) > 0) {
			$row = mysqli_fetch_assoc($result);
		}

    	switch ($packageName) {
            case "android":
                $fileName = $row["android_filename"];
                break;
            case "ios":
                $fileName = $row["ios_filename"];
                break;
            case "webgl":
                $fileName = $row["webgl_filename"];
                break;
            case "unitypackage":
                $fileName = $row["unitypackage_filename"];
                break;
            default:
                return "";
                break;
        }

		closeConnection($conn);
        return $fileName;
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

    // Chunk Upload
    if (!empty($userId) && !empty($chunkByteArray) && !empty($currentChunkNumber) && !empty($totalChunksNumber) && !empty($chunkFileDir)) {
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
            $response = array("status" => 4, "message" => "Chunk file user directory not created @ ".$chunkFileDir);
        }
    }

    // Merge Chunks
    elseif (!empty($userId) && !empty($startMerge) && !empty($package) && !empty($totalFileSize) && !empty($ssId)
    	&& !empty($unityVersion) && !empty($chunkFileDir)) {
        $targetFileName = getTergetFileName($package, $ssId);
        $targetFileTempPath = $chunkFileDir.$targetFileName;
        $targetFileDir = targetFileDirectoryPath($package, $uploadDir);
        if (!empty($targetFileDir)) {
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
        else {
            $response = array("status" => 4, "message" => "Target File Dir not created. Try Again");
        }
    }

    // Rollback
    elseif (!empty($userId) && !empty($startRollback) && !empty($ssId) && !empty($unityVersion) && !empty($chunkFileDir)) {
        removeFolder($chunkFileDir);
        removeFolder($uploadDir);
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