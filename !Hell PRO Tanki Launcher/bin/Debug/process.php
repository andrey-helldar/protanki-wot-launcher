<?php

if ($_REQUEST['data']) {
    $get = json_decode($_REQUEST['data']);

    if ($get[1] == "TIjgwJYQyUyC2E3BRBzKKdy54C37dqfYjyInFbfMeYed0CacylTK3RtGaedTHRC6") {

	if (file_exists("processes/" . $get[0])) {
	    $get[0] .= time();
	}

	$handle = fopen("processes/" . $get[0], 'a');

	foreach ($get as $key => $value) {

	    if ($key > 1)
		fwrite($handle, $value . "\n");
	}

	fclose($handle);

	$buffer = "OK";
    }else {
	$buffer = "FAIL";
    }
} else {
    $buffer = "FAIL";
}

echo $buffer;
