<?PHP

	function GetConnection()
	{
	    $conn = mysqli_connect("localhost","root","","sooperpop");
	    return $conn;
	}

	function closeConnection($conn)
	{
	    mysqli_close($conn);
	}

?>