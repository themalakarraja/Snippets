<?PHP  
	
	if ($_SERVER['REQUEST_METHOD'] === 'POST')
	{
		$name = trim($_POST["name"]);
			
			if (!empty($name))
			{
				$projectDirectory = $_SERVER["DOCUMENT_ROOT"]."/unitydemo";

				$srcFile = $projectDirectory.'/images/';
				$dstFile = $projectDirectory.'/name_img/'.$name;

				$firstCharOfName = "";

				for( $i = 0; $i < strlen($name); $i++ )
				{
					if (!is_numeric($name[$i]))
					{
						$firstCharOfName = $name[$i];
						break;
					}
	         	}

	         	if (!empty($firstCharOfName))
	         	{
	         		if (is_dir($dstFile))
	         		{
	         			copy($srcFile.strtoupper($firstCharOfName).".png", $dstFile."/".$name.".png");
	         		}
	         		else
	         		{
	         			if(mkdir($dstFile))
	         			{
	         				copy($srcFile.strtoupper($firstCharOfName).".png", $dstFile."/".$name.".png");
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