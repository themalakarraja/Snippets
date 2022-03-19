<?PHP

	require_once('dbconncetion.php');
	
	if ($_SERVER['REQUEST_METHOD'] === 'POST')
	{
		$temp = $_POST["temp"];

		if ($temp === "temp")
		{
			$response = array();
			$conn = GetConnection();

			if ($conn) 
			{
				$query = "SELECT name, gender, email FROM users";
				$result = mysqli_query($conn, $query);

				$response = array();
				if (mysqli_num_rows($result) > 0)
				{
					$index = 0;
				    while($row = mysqli_fetch_assoc($result)) {
						$response[$index] = array("name" => $row["name"], "gender" => $row["gender"], "email" => $row["email"]);
						$index++;
				    }
				}
				else
				{
					$response = array("status" => 3, "message" => "No Data Found");
				}
			}
			else
			{
				$response = array("status" => 2, "message" => "Error connecting database");
			}

			closeConnection($conn);
		}
		else
		{
			$response = array("status" => 1, "message" => "Parameter doesn't match");
		}
		
	}
	else
	{
		$response = array("status" => 0, "message" => "Invalid request method");
	}

	header('Content-Type: application/json');
	echo json_encode($response);
?>