<?php

function Version($gServer, $gClient)
{
    $server = explode(".", $gServer);
    $client = explode(".", $gClient);
	
	if($client[1] == 9){return false;}
	
	for($i=0; $i<4; $i++){
		if($client[$i] > $server[$i]){ return true; }
		elseif($client[$i] < $server[$i]){ break; }
	}
	
    return false;
}

$xml = simplexml_load_file("pro.xml");

if (Version($xml->tanks, $_REQUEST['data'])) {
    $xml->tanks = $_REQUEST['data'];
	
    unlink("pro.xml");

    $handle = fopen("pro.xml", 'a');

    fwrite($handle, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
    fwrite($handle, "<pro>\n");
    fwrite($handle, "\t<version>" . $xml->version . "</version>\n");
    fwrite($handle, "\t<tanks>" . $xml->tanks . "</tanks>\n\n");


    fwrite($handle, "\t<full>\n");
    fwrite($handle, "\t\t<message>" . $xml->full->message . "</message>\n");
    fwrite($handle, "\t\t<download>" . $xml->full->download . "</download>\n");
    fwrite($handle, "\t</full>\n\n");

    fwrite($handle, "\t<base>\n");
    fwrite($handle, "\t\t<message>" . $xml->base->message . "</message>\n");
    fwrite($handle, "\t\t<download>" . $xml->base->download . "</download>\n");
    fwrite($handle, "\t</base>\n");

    fwrite($handle, "</pro>");

    fclose($handle);
}

echo $xml->tanks;