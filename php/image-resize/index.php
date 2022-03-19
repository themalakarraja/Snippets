<html>
    <body>
        <form enctype="multipart/form-data" method="post">
            <label for="file">Filename:</label>
            <input type="file" name="file1" id="file1" /> 
            <br />
            <input type="submit" name="submit" value="Submit" />
        </form>
    </body>
</html>

<?php
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

if(isset($_POST['submit'])) {
	ini_set('memory_limit', '500M');
    /*if ($_FILES["file1"]["error"] > 0) {
        echo "Error: " . $_FILES["file1"]["error"] . "<br />";
    }*/
    //else {
    	include("ImageCompress.php");
		$root_dir = $_SERVER["DOCUMENT_ROOT"]."/sooperpop/image-resize/media/";
		$images_dir = "C:/Users/A/Desktop/new folder/";

		$images = array_diff(scandir($images_dir), array('.','..'));

		foreach ($images as $image) {
			//$path_parts = pathinfo($_FILES["file1"]["name"]);
			//$src_dir_name = $root_dir.$path_parts['filename']."/";
			//$src_file_name = $_FILES["file1"]["name"];

			$ext = pathinfo($image, PATHINFO_EXTENSION);
			$filename = basename($image, ".".$ext);
			$src_dir_name = $root_dir.$filename."/";
			$src_file_name = $image;

			if (is_dir($src_dir_name)) {
				removeFolder($src_dir_name);
			}
			mkdir($src_dir_name, "0755");
			//move_uploaded_file($_FILES['file1']['tmp_name'], $src_dir_name . $src_file_name);
			copy($images_dir.$image, $src_dir_name.$image);

			CreateResizedImage($src_dir_name, $src_file_name, "hqdefault.jpg", 480, 270, 90);
			CreateResizedImage($src_dir_name, $src_file_name, "mqdefault.jpg", 320, 180, 90);
			CreateResizedImage($src_dir_name, $src_file_name, "default.jpg", 160, 90, 90);
			CreateResizedImage($src_dir_name, $src_file_name, "sddefault.jpg", 640, 360, 90);
			if (filesize($src_dir_name.$image) > (100*1000)) {
				CreateResizedImage($src_dir_name, $src_file_name, "maxresdefault.jpg", 1920, 1080, 50);
			}
			else {
				CreateResizedImage($src_dir_name, $src_file_name, "maxresdefault.jpg", 1920, 1080, 90);
			}

	        // echo "Upload: " . $_FILES["file1"]["name"] . "<br />";
	        // echo "Type: " . $_FILES["file1"]["type"] . "<br />";
	        // echo "Size: " . ($_FILES["file1"]["size"] / 1024) . " Kb<br />";
	        // echo "Stored in: " . $_FILES["file1"]["tmp_name"];
		}
		echo "saved";
    //}
}

?>