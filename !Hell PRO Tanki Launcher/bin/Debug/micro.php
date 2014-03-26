<?php

$get = json_decode($_REQUEST['data']);
$verClient = str_replace(".", "", $get) + 0;

$xml = simplexml_load_file("pro.xml");
$verServer = str_replace(".", "", $xml->tanks) + 0;

$buffer = $get ."  :: ".$verClient . " :: ".$verServer;
/*
if ($verClient > $verServer) {
    // Если у клиента версия больше, чем на сервере, то записываем новое значение
    $xml->tanks = $get;    
    $buffer = $get;

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
} else {
    $buffer = $xml->tanks;
}*/

echo $buffer;
