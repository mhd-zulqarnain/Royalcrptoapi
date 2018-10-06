<?php
$data = json_decode(file_get_contents('php://input'), true);;
$photo = $data['image'];
$price = $data['Saving'];

$name = rand(). ".jpeg";

define('UPLOAD_DIR',  $price);
    $image_parts = explode(";base64,", $photo);
     // $image_type_aux = explode("image/", $image_parts[0]);
	$image_base64 = base64_decode($image_parts[0]);

	$name = rand(). ".jpeg";

if(file_put_contents(UPLOAD_DIR .$name, $image_base64)){
  	echo json_encode(["message" => $name,
						"status" => "OK"]);
    
}else{
	echo json_encode(["message" => "Sorry, Not Successful",
						"status" => 'false']);	
						
}




?>