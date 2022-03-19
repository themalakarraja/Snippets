<?php

	if ($_SERVER['REQUEST_METHOD'] === 'POST')
	{
		$text = trim($_POST["name"]);
		
		if (!empty($text))
		{
			$lines = explode('|', wordwrap($text, 25, '|'));

			$save_image_source = "sooperpark-img/".$text;
			$image_width = 720;
			$image_height = 405;
			$text_size = 30;
			$image_rotation = 0;
			$image_font = "font/Roboto-Light.ttf";
			$prev_origin_y = 0;

			$image = imagecreate($image_width, $image_height);
	
			$background_color = imagecolorallocate($image, 0, 0, 0); //black
			$text_color = imagecolorallocate($image, 255, 255, 255); //white

			for($i=0; $i<sizeof($lines); $i++)
			{
			    $text_box = imagettfbbox($text_size, 0, $image_font, $lines[$i]);
			
				$text_width = $text_box[2] - $text_box[0];
				$text_height = $text_box[1] - $text_box[7];

				// Calculate coordinates of the text
				$origin_x = ($image_width - $text_width)/2;
				if($prev_origin_y == 0)
				{
					$origin_y = (($image_height - ($text_height * sizeof($lines)))/2) + $text_height;
				}
				else
				{
					$origin_y = $prev_origin_y + $text_height;
				}

				$prev_origin_y = $origin_y;

				imagettftext($image, $text_size, $image_rotation, $origin_x, $origin_y, $text_color, $image_font, $lines[$i]);
			}

			if (is_dir($save_image_source))
			{
				if(imagepng($image, $save_image_source."/".$text.".png"))
				{
					imagedestroy($image);
					echo "Image is saved<br>";
					echo '<img src="'.$save_image_source.'/'.$text.'.png">';
				}
				else
				{
					echo "Some error occurred. Image not saved";
				}
			}
			else
			{
				if(mkdir($save_image_source))
				{
					if(imagepng($image, $save_image_source."/".$text.".png"))
					{
						imagedestroy($image);
						echo "Image is saved<br>";
						echo '<img src="'.$save_image_source.'/'.$text.'.png">';
					}
					else
					{
						echo "Some error occurred. Image not saved";
					}
				}
			}
		}
		else
		{
			echo "Invalid request parameter";
		}
	}
	else
	{
		echo "Invalid request method";
	}
?>
