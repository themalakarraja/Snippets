<?php
	$rootDir = $_SERVER['DOCUMENT_ROOT']."/unitydemo/";
	$oldMediaDirPath = $rootDir."media/";
	$newMediaDirPath = $rootDir."new_media/";

	function createDirectory($directory_path) {
        if (!is_dir($directory_path))
            return mkdir($directory_path, 0777, true);
        else {
            return true;
        }
    }

    function getFileNames($floder_url) {
		$files = array_diff(scandir($floder_url), array('.','..'));
		sort($files);
		return $files;
    }

    function copyFile($source_file_path, $dest_file_path) {
    	if(file_exists($source_file_path)) {
    		createDirectory(dirname($dest_file_path));
    		copy($source_file_path, $dest_file_path);
    	}
    	else {
    		echo "$source_file_path This file is not exists";
    	}
    }

    function copyCpsImages() {
    	$distributors_dir_path = $GLOBALS['oldMediaDirPath']."distributors/";
		$balloon_img_old_dir_path = $distributors_dir_path."%s/balloon_image/"; //distributor_id
		$qr_code_old_dir_path = $distributors_dir_path."%s/qr_code/"; //distributor_id
		$location_img_old_dir_path = $distributors_dir_path."%s/"; //distributor_id

		$collection_point_dir_path = $GLOBALS['newMediaDirPath']."cps/";
		$balloon_img_new_dir_path = $collection_point_dir_path."%s/bi/"; //distributor_id
		$qr_code_new_dir_path = $collection_point_dir_path."%s/qr/"; //distributor_id
		$location_img_new_dir_path = $collection_point_dir_path."%s/li/"; //distributor_id

		$new_img_name = "default.png";

		copyCpsImagesInner($balloon_img_old_dir_path, $balloon_img_new_dir_path, $distributors_dir_path);
		copyCpsImagesInner($qr_code_old_dir_path, $qr_code_new_dir_path, $distributors_dir_path);
		copyCpsImagesInner($location_img_old_dir_path, $location_img_new_dir_path, $distributors_dir_path);

		function copyCpsImagesInner($old_img_path, $new_img_path, $distributors_dir_path) {
			$distributors_ids = getFileNames($distributors_dir_path);
			if (count($distributors_ids)) {
				foreach ($distributors_ids as $distributors_id) {
					$old_folder_path = sprintf($old_img_path, $distributors_id);
					$new_folder_path = sprintf($new_img_path, $distributors_id);
					$image_name = getFileNames($old_folder_path)[0];
					if (count($image_name) > 0) {
						copyFile($old_folder_path.$image_name, $new_folder_path.$new_file_name);
					}
					else {
						echo "Can't found image. distributors_id ".$distributors_id;
					}
				}
			}
			else {
				echo "Can't found any distributors ids";
			}
		}
    }

    function copyVideosAndThumbnails() {
    	$videos_dir_old_path = $GLOBALS['oldMediaDirPath']."%s/videos/"; //user_id
    	$video_id_dir_old_path = $videos_dir_old_path."%s/"; //user_id, video_id
    	
    	$videos_new_dir_path = $GLOBALS['newMediaDirPath']."videos/";
		$thumbnail_new_dir_path = $videos_new_dir_path."%s/t/"; //video_id
		$video_new_dir_path = $videos_new_dir_path."%s/t/"; //video_id

		$thumbnail_name = "default.png";
		$video_name = "default.mp4";

		$user_ids = getFileNames($GLOBALS['oldMediaDirPath']);
		if (count($user_ids) > 0) {
			foreach ($user_ids as $user_id) {
				$video_ids = getFileNames(sprintf($videos_dir_old_path, $user_id));
				if (count($user_ids) > 0) {
					foreach ($video_ids as $video_id) {
						$files = getFileNames(sprintf($video_id_dir_old_path, $user_id, $video_id));
						if(count($files) == 2) {
							if (pathinfo($files[0]['extension'] == "png") || pathinfo($files[0]['extension'] == "PNG")) {
								copyFile(sprintf($video_id_dir_old_path, $user_id, $video_id).$files[0], 
									sprintf($thumbnail_new_dir_path, $video_id).$thumbnail_name);
							}
							elseif (pathinfo($files[1]['extension'] == "png") || pathinfo($files[1]['extension'] == "PNG")) {
								copyFile(sprintf($video_id_dir_old_path, $user_id, $video_id).$files[1], 
									sprintf($thumbnail_new_dir_path, $video_id).$thumbnail_name);
							}
							else {
								echo "png not found. user_id ".$user_id.", video_id ".$video_id;
							}

							if (pathinfo($files[0]['extension'] == "mp4") || pathinfo($files[0]['extension'] == "MP4")) {
								copyFile(sprintf($video_id_dir_old_path, $user_id, $video_id).$files[0], 
									sprintf($video_new_dir_path, $video_id).$video_name);
							}
							elseif (pathinfo($files[1]['extension'] == "mp4") || pathinfo($files[1]['extension'] == "MP4")) {
								copyFile(sprintf($video_id_dir_old_path, $user_id, $video_id).$files[1], 
									sprintf($video_new_dir_path, $video_id).$video_name);
							}
							else {
								echo "mp4 not found. user_id ".$user_id.", video_id ".$video_id;
							}
						}
						else {
							echo "File count doesn't match. user_id ".$user_id.", video_id ".$video_id;
						}
					}
				}
				else {
					echo "Can't found any video ids";
				}
			}
		}
		else {
			echo "Can't found any user ids";
		}
    }

?>