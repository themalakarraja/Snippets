<?php
	function CreateResizedImage($dir, $srcFileName, $targetFileName, $newImgWidth, $newImgHeight, $imageQuality) {
		$src_file = $dir.$srcFileName;
  		$dest_file = $dir.$targetFileName;

  		$resizeImage = new ResizeImage($src_file);
  		$resizeImage -> saveNewImage($newImgWidth, $newImgHeight, $dest_file, $imageQuality);
	}

class ResizeImage
{
	private $image;
	private $originalWidth;
	private $originalHeight;
	private $newImage;
	private $resizeWidth;
	private $resizeHeight;
	private $newX;
	private $newY;
	
	public function __construct($filename)
	{
		if(file_exists($filename))
		{
			$this->setImage($filename);
		} else {
			throw new Exception('Image ' . $filename . ' can not be found, try another image.');
		}
	}
	
	private function setImage($filename)
	{
		$size = getimagesize($filename);
		switch($size['mime'])
	    {
	    	// Image is a JPG
	        case 'image/jpg':
	        // create image from jpeg
	        case 'image/jpeg':
	            $this->image = imagecreatefromjpeg($filename);
	            break;
	        // create image from gif
	        case 'image/gif':
	            $this->image = imagecreatefromgif($filename);
	            break;
	        // create image from gif
	        case 'image/png':
	            $this->image = @imagecreatefrompng($filename);
	            break;
	        // Mime type not found
	        default:
	            throw new Exception("File is not an image, please use another file type.", 1);
	    }

	    $this->originalWidth = imagesx($this->image);
	    $this->originalHeight = imagesy($this->image);
	}

	public function saveNewImage($newWidth, $newHeight, $savePath, $imageQuality)
	{
		$this->newImage = imagecreatetruecolor($newWidth, $newHeight);
		$defaultBGColor = imagecolorallocate($this->newImage, 0, 0, 0);
		imagefill($this->newImage, 0, 0, $defaultBGColor);

		if (($this->originalWidth / $this->originalHeight) >= ($newWidth / $newHeight)) {
		    $this->resizeWidth = $newWidth;
		    $this->resizeHeight = $this->originalHeight * ($newWidth / $this->originalWidth);
		    $this->newX = 0;
		    $this->newY = round(abs($newHeight - $this->resizeHeight) / 2);
		} else {
		    $this->resizeWidth = $this->originalWidth * ($newHeight / $this->originalHeight);
		    $this->resizeHeight = $newHeight;
		    $this->newX = round(abs($newWidth - $this->resizeWidth) / 2);
		    $this->newY = 0;
		}

		imagecopyresampled($this->newImage, $this->image, $this->newX, $this->newY, 0, 0, 
			$this->resizeWidth, $this->resizeHeight, $this->originalWidth, $this->originalHeight);
		//imageantialias($this->newImage, true);
		//imagesetinterpolation($this->newImage, IMG_BICUBIC_FIXED);
		//imagefilter($this->newImage, IMG_FILTER_SMOOTH, -1);
		
		imagejpeg($this->newImage, $savePath, $imageQuality);
		imagedestroy($this->newImage);
		imagedestroy($this->image);
	}
}
?>