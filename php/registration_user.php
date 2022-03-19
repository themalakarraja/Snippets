<?PHP

	require_once('dbconncetion.php');
	if ($_SERVER['REQUEST_METHOD'] === 'POST')
	{
		$user_name = $_POST["name_post"];
		$user_gender = $_POST["gender_post"];
		$user_email = $_POST["email_post"];
		$user_password = $_POST["password_post"];

		$conn = GetConnection();

		if ($conn) 
		{
			$query = "INSERT INTO users (name, gender, email, password) VALUES ('" . $user_name . "', '" . $user_gender . "', '" . $user_email . "', '" . $user_password. "')";

			if (mysqli_query($conn, $query)) 
			{
			    $response = array("status" => 1, "message" => "New record inserted successfully");
			}
			else 
			{
				$response = array("status" => 3, "message" => "Some Error occurred. Try Again");
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
		$response = array("status" => 0, "message" => "Invalid request method");
	}

	header('Content-Type: application/json');
	echo json_encode($response);

?>